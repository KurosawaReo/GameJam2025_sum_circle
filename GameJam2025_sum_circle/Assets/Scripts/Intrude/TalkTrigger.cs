using UnityEngine;

public class TalkTrigger : MonoBehaviour
{
    [Header("侵入確認パネルのコントローラー")]
    public IntrudeUIPanelController intrudeUIPanel;

    [Header("会話開始に使うキー")]
    public KeyCode talkKey = KeyCode.E;

    private bool isPlayerInRange = false;

    // プレイヤーが範囲に入った
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("プレイヤーが会話範囲に入りました");
        }
    }

    // プレイヤーが範囲から出た
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("プレイヤーが会話範囲から出ました");
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(talkKey))
        {
            if (intrudeUIPanel != null)
            {
                intrudeUIPanel.ShowIntrudePanel();
            }
            else
            {
                Debug.LogWarning("IntrudeUIPanelControllerが設定されていません");
            }
        }
    }
}
