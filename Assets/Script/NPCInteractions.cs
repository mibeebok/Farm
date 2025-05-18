using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public List<DialogueLine> dialogueLines;

    public GameObject TextE; // ��������� "����� E"

    public string npcName;       // ��� NPC
    public Sprite npcFace;       // ���� NPC

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TextE.SetActive(false);

            // ������� ��� ������ ������ � ���������� ��������
            dialogueManager.StartDialogue(dialogueLines, npcName, npcFace);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() != null)
        {
            TextE.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() != null)
        {
            TextE.SetActive(false);
            dialogueManager.EndDialogue(); // ������ ������, ���� ����� �� ����
            playerInRange = false;
        }
    }

}
