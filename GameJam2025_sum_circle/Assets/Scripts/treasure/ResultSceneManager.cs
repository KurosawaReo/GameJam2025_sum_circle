using UnityEngine;
using UnityEngine.UI;

public class ResultSceneManager : MonoBehaviour
{
    public Text resultText;
    public PlayerData playerData;

    private void Start()
    {
        if (playerData != null)
        {
            resultText.text = $"合計金額: {playerData.totalValue} G";
        }
    }
}
