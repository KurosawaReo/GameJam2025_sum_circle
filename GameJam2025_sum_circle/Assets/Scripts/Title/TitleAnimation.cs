using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleAnimation : MonoBehaviour
{
    public RectTransform titleTransform;
    public float minScale = 0.8f;  // 最小サイズ
    public float maxScale = 1.2f;  // 最大サイズ
    public float scaleSpeed = 1.5f; // 拡大縮小のスピード

    private bool scalingUp = true;

    private void Update()
    {
        if (titleTransform == null) return;

        // 現在のスケールを取得
        Vector3 currentScale = titleTransform.localScale;

        // 拡大・縮小処理
        if (scalingUp)
        {
            currentScale += Vector3.one * scaleSpeed * Time.deltaTime;
            if (currentScale.x >= maxScale)
            {
                currentScale = Vector3.one * maxScale;
                scalingUp = false;
            }
        }
        else
        {
            currentScale -= Vector3.one * scaleSpeed * Time.deltaTime;
            if (currentScale.x <= minScale)
            {
                currentScale = Vector3.one * minScale;
                scalingUp = true;
            }
        }

        // スケールを適用
        titleTransform.localScale = currentScale;
    }
}
