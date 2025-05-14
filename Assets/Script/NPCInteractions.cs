using UnityEngine;
using UnityEngine.UI; // Для UI

public class NPCInteraction : MonoBehaviour
{
    public GameObject TextE; // Сюда перетащи UI текст "Нажми E"
    public GameObject dialogueBox; // Перетащи UI Image с PNG

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            dialogueBox.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() != null)
        {
            Debug.Log("Игрок вошёл в зону NPC");
            TextE.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() != null)
        {
            TextE.SetActive(false);
            dialogueBox.SetActive(false); // Закрыть диалог при уходе
            playerInRange = false;
        }
    }
}
