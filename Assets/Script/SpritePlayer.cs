using System.Runtime.CompilerServices;
using UnityEngine;

public class SpritePlayer : MonoBehaviour
{

    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;


    private void Start () { //запускается с началом работы программы
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate () { // запускается через равные промежутки времени для всех пользователей

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        if (moveX > 0) {
            spriteRenderer.sprite = sprites[2];
        }
        else if (moveX < 0) {
            spriteRenderer.sprite = sprites[3];
        }
        else if (moveY > 0) {
            spriteRenderer.sprite = sprites[0];
        }
        else if (moveY < 0) {
            spriteRenderer.sprite = sprites[1];
        }


    }
}
