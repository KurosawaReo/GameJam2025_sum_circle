using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public enum ChestRarity
    {
        Normal,
        Rare,
        SuperRare,
    }

    [System.Serializable]
    public class RarityReward
    {
        public ChestRarity rarity;
        public List<Item> rewardItems;
    }


    public class ChestData
    {
        public ChestRarity rarity;
        public int maxFailCount;
        public float speedIncrease;
        public Vector2 successZoneWidthRange;

        public ChestData(ChestRarity rarity, int failCount, float speedInc, Vector2 widthRange)
        {
            this.rarity = rarity;
            maxFailCount = failCount;
            speedIncrease = speedInc;
            successZoneWidthRange = widthRange;
        }
    }

    public static class Common
    {
        private static ChestData normalChest = new ChestData(ChestRarity.Normal, 3, 0.1f, new Vector2(0.3f, 0.5f));
        private static ChestData rareChest = new ChestData(ChestRarity.Rare, 4, 0.15f, new Vector2(0.2f, 0.4f));
        private static ChestData superRareChest = new ChestData(ChestRarity.SuperRare, 5, 0.2f, new Vector2(0.1f, 0.3f));

        public static ChestData GetChestDataByRarity(ChestRarity rarity)
        {
            return rarity switch
            {
                ChestRarity.Normal => normalChest,
                ChestRarity.Rare => rareChest,
                ChestRarity.SuperRare => superRareChest,
                _ => normalChest,
            };
        }
    }
}
