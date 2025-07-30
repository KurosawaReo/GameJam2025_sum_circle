using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ResultSceneManager : MonoBehaviour
{
    public Text resultText;
    public Text highScoreText;
    public Text rankingText;
    public Text treasureCountText; // ← 追加

    public List<Item> itemDatabase; // アイテムデータベースをインスペクタで設定

    private void Start()
    {
        int totalValue = PlayerPrefs.GetInt("TotalValue", 0);
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        resultText.text = $"合計金額: {totalValue} G";
        highScoreText.text = $"ハイスコア: {highScore} G";

        // ランキング表示
        int[] ranking = RankingManager.GetRanking();
        string text = "ランキング:\n";
        for (int i = 0; i < ranking.Length; i++)
        {
            text += $"{i + 1}位: {ranking[i]} G\n";
        }
        rankingText.text = text;

        // 宝の内訳表示
        if (InventoryManager.Instance != null)
        {
            Dictionary<Item.Rarity, int> counts = InventoryManager.Instance.GetItemCountByRarity(itemDatabase);

            string treasureText = "獲得した宝:\n";
            foreach (var pair in counts)
            {
                treasureText += $"{pair.Key}: {pair.Value}個\n";
            }

            treasureCountText.text = treasureText;
        }
    }
}
