using UnityEngine;
using System.Collections.Generic;

public static class RankingManager
{
    private const int MaxRank = 5; // 上位5位まで保存

    // スコア＋宝数をまとめた構造体
    public struct RankData
    {
        public int score;
        public int treasureCount;
    }

    /// <summary>
    /// スコアと宝数を保存
    /// </summary>
    public static void SaveScore(int score, int treasureCount)
    {
        List<RankData> rankList = new List<RankData>();

        // 既存データを読み込み
        for (int i = 0; i < MaxRank; i++)
        {
            int s = PlayerPrefs.GetInt("Rank_" + i, 0);
            int t = PlayerPrefs.GetInt("RankTreasure_" + i, 0);
            rankList.Add(new RankData { score = s, treasureCount = t });
        }

        // 新しいデータを追加
        rankList.Add(new RankData { score = score, treasureCount = treasureCount });

        // 降順ソート（スコア優先）
        rankList.Sort((a, b) => b.score.CompareTo(a.score));

        // 上位5位だけ保存
        for (int i = 0; i < MaxRank; i++)
        {
            if (i < rankList.Count)
            {
                PlayerPrefs.SetInt("Rank_" + i, rankList[i].score);
                PlayerPrefs.SetInt("RankTreasure_" + i, rankList[i].treasureCount);
            }
            else
            {
                PlayerPrefs.SetInt("Rank_" + i, 0);
                PlayerPrefs.SetInt("RankTreasure_" + i, 0);
            }
        }

        PlayerPrefs.Save();
    }

    /// <summary>
    /// ランキングを取得
    /// </summary>
    public static RankData[] GetRanking()
    {
        RankData[] scores = new RankData[MaxRank];
        for (int i = 0; i < MaxRank; i++)
        {
            scores[i].score = PlayerPrefs.GetInt("Rank_" + i, 0);
            scores[i].treasureCount = PlayerPrefs.GetInt("RankTreasure_" + i, 0);
        }
        return scores;
    }
}
