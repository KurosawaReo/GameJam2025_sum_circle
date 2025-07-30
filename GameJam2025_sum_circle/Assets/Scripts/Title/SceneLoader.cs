using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneLoader : MonoBehaviour
{
    [Header("アイテムデータベース")]
    public List<Item> itemDatabase;

    /// <summary>
    /// 指定したシーン名に移動する
    /// </summary>
    public void LoadSceneByName(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("シーン名が指定されていません");
            return;
        }

        if (sceneName == "ResultScene")
        {
            if (InventoryManager.Instance != null)
            {
                int totalValue = InventoryManager.Instance.CalculateTotalValue(itemDatabase);

                // 宝の総数を計算
                int treasureCount = 0;
                foreach (var pair in InventoryManager.Instance.GetAllItems())
                {
                    treasureCount += pair.Value;
                }

                // PlayerPrefsに保存
                PlayerPrefs.SetInt("TotalValue", totalValue);

                // 自分用の宝数キーに保存（ここを追加）
                PlayerPrefs.SetInt("MyTreasureCount", treasureCount);

                // ランキングに保存
                RankingManager.SaveScore(totalValue, treasureCount);

                PlayerPrefs.Save();

                Debug.Log($"リザルト移動前: 金額={totalValue}, 宝数={treasureCount}");
            }
            else
            {
                Debug.LogWarning("InventoryManagerが見つかりませんでした");
            }
        }

        SceneManager.LoadScene(sceneName);
    }
}
