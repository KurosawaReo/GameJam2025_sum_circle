using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    public int currentDay = 1;

    // 宝箱のレアリティをランダムに返す
    public Utility.ChestRarity GetRandomChestRarity()
    {
        float random = Random.value; // 0.0〜1.0の乱数

        // 日数によってレア・スーパーレアの出現確率を上げる
        float superRareChance = 0f;
        float rareChance = 0f;

        if (currentDay >= 6)
        {
            superRareChance = 0.2f;
            rareChance = 0.4f;
        }
        else if (currentDay >= 4)
        {
            superRareChance = 0.1f;
            rareChance = 0.3f;
        }
        else if (currentDay >= 2)
        {
            superRareChance = 0.05f;
            rareChance = 0.2f;
        }
        else
        {
            superRareChance = 0.01f;
            rareChance = 0.1f;
        }

        // 判定
        if (random < superRareChance)
        {
            return Utility.ChestRarity.SuperRare;
        }
        else if (random < superRareChance + rareChance)
        {
            return Utility.ChestRarity.Rare;
        }
        else
        {
            return Utility.ChestRarity.Normal;
        }
    }

    // 現在の日数に基づく「最高レアリティ」
    public Utility.ChestRarity GetCurrentChestRarity()
    {
        if (currentDay >= 6) return Utility.ChestRarity.SuperRare;
        if (currentDay >= 3) return Utility.ChestRarity.Rare;
        return Utility.ChestRarity.Normal;
    }

    // 日を進める（最大7日）
    public void AdvanceDay()
    {
        currentDay = Mathf.Min(currentDay + 1, 7);
    }
}
