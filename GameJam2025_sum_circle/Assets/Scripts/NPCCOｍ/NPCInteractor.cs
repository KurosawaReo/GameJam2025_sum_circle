using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AffectionOption
{
    public string optionText;     // ボタンに表示されるテキスト
    [TextArea(2, 5)]
    public string responseText;   // NPCの返答
    public int affectionChange;   // 好感度変化量（+や-）
}

[System.Serializable]
public class NPCDialogueData
{
    [Header("情報を聞くの返答（好感度低い場合）")]
    [TextArea(2, 5)]
    public string normalHint;

    [Header("情報を聞くの返答（好感度高い場合）")]
    [TextArea(2, 5)]
    public string highAffectionHint;

    [Header("好感度を上げる時の質問候補")]
    public List<string> affectionQuestions;

    [Header("好感度を上げる選択肢（このNPC専用）")]
    public List<AffectionOption> affectionOptions = new List<AffectionOption>();

    // ✅ 追加データ（他のスクリプトへ送る用の情報）
    [Header("他スクリプトに送る追加データ")]
    public string externalDataKey;    // 例: 送信IDやクエストキー
    [TextArea(2, 5)]
    public string externalDataText;   // 例: 送信用メッセージや説明文
}

public class NPCInteractor : MonoBehaviour
{
    [Header("NPC情報")]
    public string npcId = "npc1";
    public string npcName = "ナナ";

    [Header("会話メッセージ")]
    [TextArea(2, 5)]
    public string message = "こんにちは、最近調子どう？";

    [Header("このNPC専用の会話データ")]
    public NPCDialogueData dialogueData;

    private bool isPlayerInRange = false;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (NPCConversationManager.Instance != null)
            {
                bool alreadyTried = NPCConversationManager.Instance.HasTriedAffection(npcId);

                string greetMessage = alreadyTried
                    ? "何を聞きたい？"
                    : message;

                // NPC専用データを渡す
                NPCConversationManager.Instance.StartConversationWithData(
                    npcId,
                    npcName,
                    greetMessage,
                    dialogueData
                );

                // ✅ 外部スクリプトに送る処理例
                if (!string.IsNullOrEmpty(dialogueData.externalDataKey))
                {
                    Debug.Log($"外部データ送信: {dialogueData.externalDataKey} | {dialogueData.externalDataText}");
                    // 他スクリプトの関数呼び出し例
                    ExternalDataReceiver.ReceiveData(dialogueData.externalDataKey, dialogueData.externalDataText);
                }
            }
            else
            {
                Debug.LogWarning("NPCConversationManagerがシーンに存在しません");
            }
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
