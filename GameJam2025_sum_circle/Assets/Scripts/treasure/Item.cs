using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemId;    // ユニークID（自動設定）
    public string itemName;
    public Sprite icon;
    public string description;
    public int price;       // 追加：アイテムの価格（例：売値や換算値）

    public Rarity rarity;

    public enum Rarity
    {
        Normal,
        Rare,
        Epic,
        Legendary
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // itemId が未設定、または空の場合にのみ自動生成
        if (string.IsNullOrEmpty(itemId))
        {
            itemId = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this); // 変更を保存対象にする
        }
    }
#endif
}
