using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleAnimation : MonoBehaviour
{
    public RectTransform titleTransform;
    public float fadeDuration = 1.5f;
    public float scaleDuration = 1.5f;
    public float waitTime = 1f; // アニメーション後の待機時間

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = titleTransform.gameObject.AddComponent<CanvasGroup>();
        ResetTitleState();
    }

    private void Start()
    {
        StartCoroutine(LoopTitleAnimation());
    }

    private IEnumerator LoopTitleAnimation()
    {
        while (true)
        {
            yield return StartCoroutine(PlayTitleAnimation()); // フェードイン・拡大
            yield return new WaitForSeconds(waitTime);

            yield return StartCoroutine(FadeOutAnimation()); // フェードアウト
            yield return new WaitForSeconds(waitTime);

            ResetTitleState(); // 初期状態に戻す
        }
    }

    private IEnumerator PlayTitleAnimation()
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            titleTransform.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one, time / scaleDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        titleTransform.localScale = Vector3.one;
    }

    private IEnumerator FadeOutAnimation()
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }

    private void ResetTitleState()
    {
        canvasGroup.alpha = 0f;
        titleTransform.localScale = Vector3.one * 0.5f;
    }
}
