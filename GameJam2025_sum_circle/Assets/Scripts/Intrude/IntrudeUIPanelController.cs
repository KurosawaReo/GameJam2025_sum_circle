using UnityEngine;

public class IntrudeUIPanelController : MonoBehaviour
{
    public GameObject intrudePanel;

    public void ShowIntrudePanel()
    {
        intrudePanel.SetActive(true);
        Time.timeScale = 0; // ゲームを一時停止
    }

    public void OnClickYes()
    {
        Time.timeScale = 1;
        intrudePanel.SetActive(false);
        // 侵入処理へ進む
    }

    public void OnClickNo()
    {
        Time.timeScale = 1;
        intrudePanel.SetActive(false);
    }
}
