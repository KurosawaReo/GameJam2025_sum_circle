using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackgroundSlideshow : MonoBehaviour
{
    public Image backgroundImage;    // UIのImage
    public Sprite[] images;          // 2枚以上対応可能
    public float switchTime = 3f;    // 画像切り替え間隔
    public float fadeDuration = 1f;  // フェード時間

    private int currentIndex = 0;

    private void Start()
    {
        if (images.Length > 0)
        {
            backgroundImage.sprite = images[0];
            StartCoroutine(SwitchImages());
        }
    }

    private IEnumerator SwitchImages()
    {
        while (true)
        {
            yield return new WaitForSeconds(switchTime);
            currentIndex = (currentIndex + 1) % images.Length;

            // フェードアウト
            yield return StartCoroutine(FadeImage(0f));

            // 画像切り替え
            backgroundImage.sprite = images[currentIndex];

            // フェードイン
            yield return StartCoroutine(FadeImage(1f));
        }
    }

    private IEnumerator FadeImage(float targetAlpha)
    {
        Color c = backgroundImage.color;
        float startAlpha = c.a;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            backgroundImage.color = c;
            yield return null;
        }

        c.a = targetAlpha;
        backgroundImage.color = c;
    }
}
