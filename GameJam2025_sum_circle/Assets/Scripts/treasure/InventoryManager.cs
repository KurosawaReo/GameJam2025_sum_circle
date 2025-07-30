using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private Dictionary<string, int> itemCounts = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(Item item, int count = 1)
    {
        if (item == null) return;

        if (itemCounts.ContainsKey(item.itemId))
            itemCounts[item.itemId] += count;
        else
            itemCounts[item.itemId] = count;

        Debug.Log($"アイテム「{item.itemName}」を{count}個追加しました。合計：{itemCounts[item.itemId]}");
    }

    public int GetItemCount(string itemId)
    {
        return itemCounts.TryGetValue(itemId, out int count) ? count : 0;
    }

    public Dictionary<string, int> GetAllItems()
    {
        return new Dictionary<string, int>(itemCounts);
    }

    // 合計金額を計算
    public int CalculateTotalValue(List<Item> itemDatabase)
    {
        int total = 0;
        foreach (var pair in itemCounts)
        {
            Item item = itemDatabase.Find(i => i.itemId == pair.Key);
            if (item != null)
                total += item.price * pair.Value;
        }
        return total;
    }
    public void SaveTotalValue(List<Item> itemDatabase)
    {
        int total = CalculateTotalValue(itemDatabase);

        // 合計金額を保存
        PlayerPrefs.SetInt("TotalValue", total);

        // ハイスコアを更新
        int currentHigh = PlayerPrefs.GetInt("HighScore", 0);
        if (total > currentHigh)
        {
            PlayerPrefs.SetInt("HighScore", total);
        }

        // セーブ反映
        PlayerPrefs.Save();

        Debug.Log($"[SAVE] PlayerPrefsに合計金額を保存: {total}");
    }
    public Dictionary<Item.Rarity, int> GetItemCountByRarity(List<Item> itemDatabase)
    {
        Dictionary<Item.Rarity, int> rarityCounts = new Dictionary<Item.Rarity, int>();

        foreach (var pair in itemCounts)
        {
            Item item = itemDatabase.Find(i => i.itemId == pair.Key);
            if (item != null)
            {
                if (!rarityCounts.ContainsKey(item.rarity))
                    rarityCounts[item.rarity] = 0;
                rarityCounts[item.rarity] += pair.Value;
            }
        }
        return rarityCounts;
    }

}
