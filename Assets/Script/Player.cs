using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{

    private float movingSpeed = 5f;
    private Rigidbody2D rb;

    private void Awake () { //запускается до функции Start (инициализация)
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate () { // запускается через равные промежутки времени для всех пользователей
        Vector2 InputVector = new Vector2(0, 0);
        
        if(Input.GetKey(KeyCode.W)){
            InputVector.y = 1f; //положительное значение при нажатии на W по оси y
        }

        if(Input.GetKey(KeyCode.S)){
            InputVector.y = -1f;
        }

        if(Input.GetKey(KeyCode.A)){
            InputVector.x = -1f;
        }

        if(Input.GetKey(KeyCode.D)){
            InputVector.x = 1f;
        }

        InputVector = InputVector.normalized;

        if (Input.GetKey(KeyCode.LeftShift)){
        rb.MovePosition(rb.position + InputVector * (movingSpeed * Time.fixedDeltaTime) * 2); //ускорение
        }else{
        rb.MovePosition(rb.position + InputVector * (movingSpeed * Time.fixedDeltaTime)); //начальная позиция + inputVector * зафиксированное ускорение * умноженное количество времени
        }

    }
}
