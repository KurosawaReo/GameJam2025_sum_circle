using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCConversationManager : MonoBehaviour
{
    public static NPCConversationManager Instance;

    public GameObject conversationPanel;
    public Text npcNameText;
    public Text conversationText;

    private HashSet<string> talkedNPCs = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (conversationPanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            CloseConversation();
        }
    }

    public void StartConversation(string npcId, string npcName, string message)
    {
        if (!talkedNPCs.Contains(npcId))
        {
            talkedNPCs.Add(npcId);
            Debug.Log($"[会話記録] {npcName}（ID: {npcId}）と会話しました。");
        }

        npcNameText.text = npcName;
        conversationText.text = message;
        conversationPanel.SetActive(true);

        if (talkedNPCs.Count >= 3)
        {
            Debug.Log("全てのNPCと話しました → 夜に切り替え");
        }
    }

    public void CloseConversation()
    {
       // conversationPanel.SetActive(false);
    }
}
