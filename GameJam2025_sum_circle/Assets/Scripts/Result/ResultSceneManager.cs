using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ResultSceneManager : MonoBehaviour
{
    public Text resultText;
    public Text highScoreText;
    public Text rankingText;
    public Text treasureCountText; // 宝の数表示用

    public List<Item> itemDatabase;

    private void Start()
    {
        int totalValue = PlayerPrefs.GetInt("TotalValue", 0);
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        int myTreasureCount = PlayerPrefs.GetInt("MyTreasureCount", 0);  // ← 追加

        resultText.text = $"合計金額: {totalValue} G";
        highScoreText.text = $"ハイスコア: {highScore} G";

        // ランキングは金額のみ表示
        RankingManager.RankData[] ranking = RankingManager.GetRanking();

        string rankingTextStr = "ランキング:\n";
        for (int i = 0; i < ranking.Length; i++)
        {
            rankingTextStr += $"{i + 1}位: {ranking[i].score} G\n";
        }
        rankingText.text = rankingTextStr;

        // 自分が獲得した宝数を表示
        treasureCountText.text = $"獲得した宝の数: {myTreasureCount} 個";
    }
}
