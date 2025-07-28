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
        // UI更新など通知処理もここに入れる
    }

    public int GetItemCount(string itemId)
    {
        return itemCounts.TryGetValue(itemId, out int count) ? count : 0;
    }

    // 他、アイテム削除や所持チェックも追加可能
}
