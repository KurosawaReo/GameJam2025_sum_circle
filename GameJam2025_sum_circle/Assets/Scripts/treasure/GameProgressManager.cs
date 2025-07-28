using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    public int currentDay = 1;

    public Utility.ChestRarity GetCurrentChestRarity()
    {
        if (currentDay >= 6) return Utility.ChestRarity.SuperRare;
        if (currentDay >= 3) return Utility.ChestRarity.Rare;
        return Utility.ChestRarity.Normal;
    }

    public void AdvanceDay()
    {
        currentDay = Mathf.Min(currentDay + 1, 7);
    }
}
