using UnityEngine;
using UnityEngine.UI; // ��� UI

public class NPCInteraction : MonoBehaviour
{
    public GameObject TextE; // ���� �������� UI ����� "����� E"
    public GameObject dialogueBox; // �������� UI Image � PNG

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
            Debug.Log("����� ����� � ���� NPC");
            TextE.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() != null)
        {
            TextE.SetActive(false);
            dialogueBox.SetActive(false); // ������� ������ ��� �����
            playerInRange = false;
        }
    }
}
