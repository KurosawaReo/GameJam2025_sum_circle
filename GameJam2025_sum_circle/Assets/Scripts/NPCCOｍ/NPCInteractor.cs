using UnityEngine;

public class NPCInteractor : MonoBehaviour
{
    public string npcId = "npc1";      // 一意なID
    public string npcName = "ナナ";     // 表示名
    [TextArea(2, 5)]
    public string message = "こんにちは、最近調子どう？";

    private bool isPlayerInRange = false;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            NPCConversationManager.Instance.StartConversation(npcId, npcName, message);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log($"{npcName} に近づいた（Eキーで会話）");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log($"{npcName} から離れた");
        }
    }
}
