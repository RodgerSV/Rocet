using UnityEngine;
using UnityEngine.SceneManagement;//подключение библиотеки для переключения сцен
using UnityEngine.UI;//подкючение библиотеки для работы с текстом UI

public class Roket1 : MonoBehaviour
{  
    [SerializeField] Text energyText;//переменная для работы отображения текста UI

    [SerializeField] int energyTotal=1000;//переменная для смотрит Энергии в баке всего можно поменять на flot тогда будут выводится дробные с запятой UI

    [SerializeField] int energyApply=20;//переменная для использования энергии UI

    [SerializeField] float rotSpeed=100f;//переменная для уточнения скорости поворота

    [SerializeField] float flySpeed=100f;//переменная для уточнения скорости полета

    [SerializeField] AudioClip flySound;//переменная для проигрывания звука полета

    [SerializeField] AudioClip boomSound;//переменная для проигрывать звук взрыва

    [SerializeField] AudioClip finSound;//переменная дляпроигрывть звук финиша

    [SerializeField] ParticleSystem flyPar;//переменная для подключение спец эффектов для полета

    [SerializeField] ParticleSystem boomPar;//переменная для подключение спец эффектов для взрыва
    
    [SerializeField] ParticleSystem finPar;//переменная для подключение спец эффектов для финиша

     bool collisionOff = false;//переменная для проверки вкл/выкл метода коллизии

     Rigidbody Fly;//переменная для смотрит в ресурс Тела

     AudioSource sound;//переменная для смотрит в ресурс audiosource
     enum State {Playing,Dead,NextLevel};//переменная для проверки состояния Игры

     State state = State.Playing;//статус уровень равен Играю
    
    void Start()// Метод старт (запускается всего один раз и не выклчается)
    { 
      energyText.text = energyTotal.ToString();//необходимо при запуске
      state = State.Playing;//статус уровень равен Играю
      Fly = GetComponent<Rigidbody>();// функция смотрит в компонент тела
      sound = GetComponent<AudioSource>();// функция смотрит в компонент звука
    }
    
    //если мы играем управление не отключается
    void Update() // Метод обнавляет экран каждый кадр (не очень хорошо так как у всех железо разное нужно вводить параметр Time.deltaTime или void FixedUpdate)
    {
       if (state == State.Playing)
       {
          Launch();// функция запуск
          Rotation();//функция врашения
       }
       if (Debug.isDebugBuild)//если эта строика указана и это не билд для конечной сборки галочка не выставлена то читы работать не будут
       {
          DebugKeys();
       }      
    }
    void DebugKeys()
    {  if(Input.GetKeyDown(KeyCode.L))// использования пролистывания уровня с помощью кнопки L
         
        LoadNextLevel();//пролистывания уровня
      
        else if(Input.GetKeyDown(KeyCode.C))//использования пролистывания уровня с помощью кнопки С
      
        collisionOff = !collisionOff;//переключатель состаяния и возвращающий метод вкл/выкл коллизии(всего метода)
     }      
    void OnCollisionEnter(Collision collision)//Метод если что то случается далее переключение по ситуации
    { 
        if (state == State.Dead || state == State.NextLevel || collisionOff)// если мы не живы дальше скрипт не обрабатывается ||-знак или
        {
          return;//останавливает скрипт если дошел до этого метода 
        }
        
        switch(collision.gameObject.tag)
        {
            case "Frendly":
            break;      
            case "Finish":
            Finish();//обращение к метаду финиш
            break;
            case "Battary":
            PlusEnergy(100,collision.gameObject);//обращение к метаду батарейка
            break;
            default:
            Lose();//обращение к метаду проигрыш 
            break;
        }
    }
    public void PlusEnergy(int energyToAdd, GameObject battaryObject)
    {
      {
        battaryObject.GetComponent<BoxCollider>().enabled = false;//если прикаснутся только один раз до калайдера любым местом другого колайдера
        energyTotal += energyToAdd;//переменная для для добавления энергии если датронулись до предмета
        energyText.text = energyTotal.ToString();//переменная благодаря которой можно текст перевести в цифры(целое число)
        Destroy(battaryObject);//компонент уничтожаюзий обект при соединении с ним также можно добавить 2F -это задержка в секундах   
      }
    }
    void Lose()
    {       state = State.Dead;//состояние Умер
            sound.Stop();//для того чтобы менялись звуки проигрывания нужно остнаовить
            sound.PlayOneShot(boomSound);//проигрывания звука взрыва один раз
            boomPar.Play();//проигрывания спец эффектов взрыва
            Invoke("LoadNextLevel",2f);// Задержка при переключения уровня
    }
    void Finish()
    {       state = State.NextLevel;//состояние перехожу на новый уровень
            sound.Stop();//для того чтобы менялись звуки проигрывания нужно остнаовить
            sound.PlayOneShot(finSound);//проигрывания звука финиша один раз
            finPar.Play();// проигрывания спец эффектов финиша
            Invoke("LoadNextLevel",2f);// Задержка при переключения уровня          
    }
    void LoadNextLevel() //старт уровня загрузка другого уровня
    {     int currentLevelindex = SceneManager.GetActiveScene().buildIndex;//вычисления индекса сцены за счет целого числа(int)
          int nextLevelindex = currentLevelindex + 1;//переключения по индексу сцены + 1
       if (nextLevelindex == SceneManager.sceneCountInBuildSettings)
       {
          nextLevelindex = 1;//обнуления индекса уровня когда они заканчиваются и зпускается с первого(так как меню стало 0 сцена стала 1)
       }

           SceneManager.LoadScene(nextLevelindex); //менеджер сцены загрузка 1сцены SceneManager.LoadScene(1); 
    }
    void LoadFirstLevel() // проигрыш загрузка 1 сцены
    {
      SceneManager.LoadScene(1);//менеджер сцены загрузка 0сцены  
    }
      
    void Launch()
   {        
      {         
           if (Input.GetKey(KeyCode.Space) && energyTotal >5)//кнопка  если мы нажали то выполняется алгоритм или если энергия кончилась корабль упадет
            {                
              energyTotal-= Mathf.RoundToInt(energyApply * Time.deltaTime);//можно написать energyTotal = energyTotal -10 для удобства так не пишут это отнять какоето колечиство при нажатии
              energyText.text = energyTotal.ToString();//функция ToString позволяет выводить значение цифры на экран даже если они целые или дробные
              Fly.AddRelativeForce(Vector3.up * flySpeed * Time.deltaTime);//функция отвечающий за добовление силы по вектору вверх (тяга)
              if (sound.isPlaying == false)// функция если мы играем то играет
              sound.PlayOneShot(flySound);//проигрывания звука полета(в моменте)
              flyPar.Play();// функция остановка спец эффектов  
            }     
              else
            {  
             sound.Pause();// функция остановка звука
             flyPar.Stop();// функция остановка спец эффектов
            }      
    }
}
    private void Rotation()
    {
        float rotationSpeed = rotSpeed * Time.deltaTime;

        Fly.freezeRotation = true;//функция отвечающий на заморозку если мы впилились в обьект (по умолчанию можем поворачиватся)
        if (Input.GetKey(KeyCode.A))//кнопка если мы нажали то выполняется алгоритм
        {
           // Fly.AddRelativeForce(Vector3.left);//добовление силы в влево
            transform.Rotate(Vector3.forward * rotationSpeed);// функция отвечающий за направления и скорость поворота по вектору в влево 
        }
        else if (Input.GetKey(KeyCode.D))//кнопка если мы нажали то выполняется алгоритм
        {
           // Fly.AddRelativeForce(Vector3.right);//добовление силы в вправо
            transform.Rotate(-Vector3.forward * rotationSpeed);// функция отвечающая за направления скорости поворота по вектору в вправо
        }
        Fly.freezeRotation = false;//параметр отвечающий на заморозку если мы впилились в обьект (по умолчанию не можем поворачивать)
    }
}
