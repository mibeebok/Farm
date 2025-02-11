using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player Instance {get; private set;}

    private float movingSpeed = 5f;
    private Rigidbody2D rb;

    private float minMovingSpeed=0.1f;

    private bool isWolk = false;
    private bool isWolkLeft = false;
    private bool isWolkStraight = false;
    private bool isWolkBack = false;
    
    private bool isRunning = false;

    private void Awake () { //запускается до функции Start (инициализация)
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate () { // запускается через равные промежутки времени для всех пользователей
        Vector2 InputVector = new Vector2(0, 0);
        
        if(Input.GetKey(KeyCode.W)){
            InputVector.y = 1f; //положительное значение при нажатии на W по оси y
            isWolkBack = true;
        }
        else{
            isWolkBack = false;
        }

        if(Input.GetKey(KeyCode.S)){
            InputVector.y = -1f;
            isWolkStraight = true;
        }
        else{
            isWolkStraight = false;
        }

        if(Input.GetKey(KeyCode.A)){
            InputVector.x = -1f;
            isWolkLeft = true;
        }
        else{
            isWolkLeft = false;
        }

        if(Input.GetKey(KeyCode.D)){
            InputVector.x = 1f;
            isWolk = true;
        }
        else{
            isWolk = false;
        }

        InputVector = InputVector.normalized;

        if (Input.GetKey(KeyCode.LeftShift)){
        rb.MovePosition(rb.position + InputVector * (movingSpeed * Time.fixedDeltaTime) * 2); //ускорение
        isRunning = true;
        }else{
        rb.MovePosition(rb.position + InputVector * (movingSpeed * Time.fixedDeltaTime)); //начальная позиция + inputVector * зафиксированное ускорение * умноженное количество времени
        isRunning = false;
        }

        //проверка для анимации на ходьбу
       /* if (Mathf.Abs(InputVector.x)> minMovingSpeed || Mathf.Abs(InputVector.y)>minMovingSpeed){
            isWolk = true;
        }else{
            isWolk = false;
        }*/

    }
    public bool IsWolk() {
        return isWolk;
    }
    public bool IsWolkLeft() {
        return isWolkLeft;
    }
    public bool IsWolkStraight() {
        return isWolkStraight;
    }
    public bool IsWolkBack() {
        return isWolkBack;
    }
    public bool IsRunning() {
        return isRunning;
    }
}
