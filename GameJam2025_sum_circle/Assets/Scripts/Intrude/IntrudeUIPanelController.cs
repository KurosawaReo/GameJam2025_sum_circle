using UnityEngine;

public class IntrudeUIPanelController : MonoBehaviour
{
    [Header("UIパネル設定")]
    public GameObject intrudePanel;      // 確認パネル
    public GameObject nightTurnPanel;    // 「はい」を押したら表示するパネル

    public GameProgressManager gameProgressManager;

    [Header("NPC好感度テスト用")]
    public int npcFavorability = 0;

    /// <summary>
    /// 侵入確認パネルを表示
    /// </summary>
    public void ShowIntrudePanel()
    {
        if (intrudePanel != null)
            intrudePanel.SetActive(true);

        Time.timeScale = 0; // ゲームを一時停止
    }

    /// <summary>
    /// はいを押した場合
    /// </summary>
    public void OnClickYes()
    {
        Time.timeScale = 1;

        if (intrudePanel != null)
            intrudePanel.SetActive(false);

        // 夜ターンパネルを表示
        if (nightTurnPanel != null)
        {
            nightTurnPanel.SetActive(true);
            Debug.Log("▶ 夜ターンパネルを表示しました");
        }
        else
        {
            Debug.LogWarning("nightTurnPanel が設定されていません");
        }
    }

    /// <summary>
    /// いいえを押した場合
    /// </summary>
    public void OnClickNo()
    {
        Time.timeScale = 1;

        if (intrudePanel != null)
            intrudePanel.SetActive(false);

        // 好感度を10上げる
        npcFavorability += 10;
        Debug.Log($"NPC好感度が10上がった！ 現在値：{npcFavorability}");

        // 日付を進める
        if (gameProgressManager != null)
        {
            gameProgressManager.AdvanceDay();
            Debug.Log($"日付を進めました。現在の日付: {gameProgressManager.currentDay}");
        }
        else
        {
            Debug.LogWarning("GameProgressManagerが未設定です");
        }
    }
}
