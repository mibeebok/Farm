using UnityEngine;

public class SavePosition : MonoBehaviour
{
    public GameObject player;

    public void Save(){
        PlayerPrefs.SetFloat("playerX", player.transform.position.x);
        PlayerPrefs.SetFloat("playerY", player.transform.position.y);
        //PlayerPrefs.SetFloat("playerVelocityX", player.GetComponent<Rigidbody2D>().velocity.x);
        //PlayerPrefs.SetFloat("playerVelocitY", player.GetComponent<Rigidbody2D>().velocity.y);
    }

    
}
