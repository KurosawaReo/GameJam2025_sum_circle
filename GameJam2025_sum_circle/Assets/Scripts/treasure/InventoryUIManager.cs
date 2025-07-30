using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventoryPanel; // ← 表示／非表示を切り替えるパネル
    public Transform contentParent;   // ScrollViewのContent
    public GameObject itemSlotPrefab; // プレハブ（中に "Icon", "Name", "Count", "Price" の子要素あり）

    private Dictionary<string, GameObject> displayedItems = new Dictionary<string, GameObject>();

    public void OpenInventoryPanel()
    {
        inventoryPanel.SetActive(true); // パネルを表示
        RefreshInventory();             // 中身更新
    }

    public void CloseInventoryPanel()
    {
        inventoryPanel.SetActive(false); // パネルを非表示
    }

    public void RefreshInventory()
    {
        // 既存の表示をすべて削除
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
        displayedItems.Clear();

        var allItems = InventoryManager.Instance.GetAllItems();

        foreach (var kvp in allItems)
        {
            string itemId = kvp.Key;
            int count = kvp.Value;

            Item item = GetItemById(itemId);
            if (item == null) continue;

            GameObject slot = Instantiate(itemSlotPrefab, contentParent);

            // 子オブジェクトの取得と代入（nullチェック付き）
            Transform iconObj = slot.transform.Find("Icon");
            if (iconObj != null && item.icon != null)
                iconObj.GetComponent<Image>().sprite = item.icon;

            Transform nameObj = slot.transform.Find("Name");
            if (nameObj != null)
                nameObj.GetComponent<Text>().text = item.itemName;

            Transform countObj = slot.transform.Find("Count");
            if (countObj != null)
                countObj.GetComponent<Text>().text = $"×{count}";

            Transform priceObj = slot.transform.Find("Price");
            if (priceObj != null)
                priceObj.GetComponent<Text>().text = $"{item.price} G";

            displayedItems[itemId] = slot;
        }
    }

    private Item GetItemById(string id)
    {
        // Resources/Items フォルダにある ScriptableObject（Item）を全取得
        Item[] items = Resources.LoadAll<Item>("Items");
        foreach (Item item in items)
        {
            if (item.itemId == id)
                return item;
        }
        return null;
    }
}
