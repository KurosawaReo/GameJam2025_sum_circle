using UnityEngine;

public static class ExternalDataReceiver
{
    public static void ReceiveData(string key, string message)
    {
        Debug.Log($"[受信] Key:{key} / Message:{message}");
        // ここでイベント処理やUI表示などを行う
    }
}
