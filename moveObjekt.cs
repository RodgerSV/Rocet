using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]//запрет использования боее одного скрипта на один и тот же обект(одного и того же)

public class moveObjekt : MonoBehaviour

{   [SerializeField] Vector3 movePos;//переменная за двежение обекта по одной из осей
    [SerializeField] [Range(0,1)]float moveProg;//переменая за прогресс обекта если 0 не двигался если 1 прогресс выполнен
    [SerializeField] float moveSpeed;//переменная за скорость движения
     Vector3 startPos;//стартовая позиция (или с какой позиции стартуем относительно координат)

    // Start is called before the first frame update
    void Start()
    { startPos = transform.position;//указание позиции на начало игры
        
    }

    // Update is called once per frame
    void Update()
    {   moveProg = Mathf.PingPong(Time.time*moveSpeed,1);//математическая функуия который работает заготовки для быстрых вычислений
        
        Vector3 offset = movePos * moveProg;//сдвиг начальнюю позицию * на прогресс
        transform.position = startPos + offset;//передвижене со стартовой позиции в сторну сдвига
        
    }
}
