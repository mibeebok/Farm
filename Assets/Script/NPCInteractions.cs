using UnityEngine;
using UnityEngine.UI; 

public class NPCInteraction : MonoBehaviour
{
    public GameObject TextE; // � ��� ���� � Unity ��������� ��� ����-������ � ����������� ���������� (�����)
    public GameObject dialogueBox; // � ���� ��������� ���� ���������� ����, ����� ��� ���������
    public Sprite npcFace; // ����� � ��� ����� ��������� ������ � ����� ����� NPC.
    public Image NPCFaceImageUI; // ���� ������������� ���� png �������� ���� NPC.
    public string npcName; // ����� ����� ��������� ��������� ���� � ������ NPC.
    public Text npcNameTextUI; // ���� ������������� ��� NPC.

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
            Debug.Log("����� ����� � ���� NPC");
            TextE.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() != null)
        {
            TextE.SetActive(false); // ����������� ��������� ���������
            dialogueBox.SetActive(false); // ������ ����������� ��� �����.
            playerInRange = false;
        }
    }
}
