using UnityEngine;

public class NPCInteractor : MonoBehaviour
{
    public string npcId = "npc1";      // 一意なID
    public string npcName = "ナナ";     // 表示名
    [TextArea(2, 5)]
    public string message = "こんにちは、最近調子どう？";

    private void OnMouseDown()
    {
        NPCConversationManager.Instance.StartConversation(npcId, npcName, message);
    }
}
