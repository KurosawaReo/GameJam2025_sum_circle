using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int totalValue; //  インベントリの合計金額を格納

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // シーンをまたいでも残す
    }
}
