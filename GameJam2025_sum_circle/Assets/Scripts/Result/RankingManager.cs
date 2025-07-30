using UnityEngine;

public static class RankingManager
{
    private const int MaxRank = 5; // 上位5位まで保存

    public static void SaveScore(int score)
    {
        // 既存スコアを読み込み
        int[] scores = new int[MaxRank];
        for (int i = 0; i < MaxRank; i++)
        {
            scores[i] = PlayerPrefs.GetInt("Rank_" + i, 0);
        }

        // スコア追加
        System.Collections.Generic.List<int> scoreList = new System.Collections.Generic.List<int>(scores);
        scoreList.Add(score);

        // 降順にソート
        scoreList.Sort((a, b) => b.CompareTo(a));

        // 上位のみ保持
        for (int i = 0; i < MaxRank; i++)
        {
            PlayerPrefs.SetInt("Rank_" + i, i < scoreList.Count ? scoreList[i] : 0);
        }

        PlayerPrefs.Save();
    }

    public static int[] GetRanking()
    {
        int[] scores = new int[MaxRank];
        for (int i = 0; i < MaxRank; i++)
        {
            scores[i] = PlayerPrefs.GetInt("Rank_" + i, 0);
        }
        return scores;
    }
}
