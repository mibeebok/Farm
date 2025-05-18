using UnityEngine;
using UnityEngine.UI; 

public class NPCInteraction : MonoBehaviour
{
    public GameObject TextE; // В это поле в Unity вставляем наш гейм-объект с всплывающей подсказкой (текст)
    public GameObject dialogueBox; // А сюда вставляем наше диалоговое окно, чтобы оно всплывало
    public Sprite npcFace; // Здесь у нас будет храниться иконка с лицом наших NPC.
    public Image NPCFaceImageUI; // Сюда подставляется наша png картинка лица NPC.
    public string npcName; // Здесь будет храниться текстовое поле с именем NPC.
    public Text npcNameTextUI; // Сюда подставляется имя NPC.

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            dialogueBox.SetActive(true);
            TextE.SetActive(false);

            if (NPCFaceImageUI != null && npcFace != null)
            {
                NPCFaceImageUI.sprite = npcFace;
            }

            if (npcNameTextUI != null)
            {
                npcNameTextUI.text = npcName;
            }
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
            TextE.SetActive(false); // Всплывающая подсказка пропадает
            dialogueBox.SetActive(false); // Диалог закрывается при уходе.
            playerInRange = false;
        }
    }
}
