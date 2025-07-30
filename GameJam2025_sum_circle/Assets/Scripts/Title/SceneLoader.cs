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

        //  リザルトシーンに移動する場合はスコアを保存
        if (sceneName == "ResultScene")
        {
            if (InventoryManager.Instance != null)
            {
                // 合計金額を計算
                int totalValue = InventoryManager.Instance.CalculateTotalValue(itemDatabase);

                // PlayerPrefs に保存
                PlayerPrefs.SetInt("TotalValue", totalValue);

                // ランキングに保存
                RankingManager.SaveScore(totalValue);

                Debug.Log($"リザルトシーンに移動前に合計金額を保存: {totalValue}");
            }
            else
            {
                Debug.LogWarning("InventoryManagerが見つかりませんでした");
            }
        }

        // シーン遷移
        SceneManager.LoadScene(sceneName);
    }
}
