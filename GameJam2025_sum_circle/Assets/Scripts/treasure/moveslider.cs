using UnityEngine;
using UnityEngine.UI;

public class MoveSlider : MonoBehaviour
{
    public Slider timingSlider;
    public float speed = 1.0f;
    public float speedIncrease = 0.1f;

    [Header("ゾーン表示")]
    public Image successZoneImage;  // 成功ゾーン（緑の帯）
    public Image justZoneImage;     // ジャストゾーン（別画像）

    [Header("ハートUI")]
    public Image[] hearts;          // 3つのハートImageをInspectorでセット
    public Sprite brokenHeartSprite; // 割れたハート画像

    [Header("ゲームパネル")]
    public GameObject timingGamePanel; // タイミングゲーム全体のパネル

    private bool isIncreasing = true;
    private float successMin;
    private float successMax;
    private float justMin;
    private float justMax;

    private float justZoneWidthRatio = 0.2f;

    private int failCount = 0;
    public int maxFailCount = 3;

    void Start()
    {
        timingSlider.direction = Slider.Direction.BottomToTop;
        ResetHearts();
        GenerateSuccessZone();
    }

    void Update()
    {
        if (isIncreasing)
            timingSlider.value += Time.deltaTime * speed;
        else
            timingSlider.value -= Time.deltaTime * speed;

        if (timingSlider.value >= 1f) isIncreasing = false;
        if (timingSlider.value <= 0f) isIncreasing = true;
    }

    public void OnPress()
    {
        float value = timingSlider.value;

        if (value >= successMin && value <= successMax)
        {
            if (value >= justMin && value <= justMax)
            {
                Debug.Log("ジャスト成功！");
                speed += speedIncrease * 2f;
            }
            else
            {
                Debug.Log("成功！");
                speed += speedIncrease;
            }
            failCount = 0;
            ResetHearts();
            GenerateSuccessZone();
        }
        else
        {
            Debug.Log("失敗！");
            failCount++;
            UpdateHearts();

            if (failCount >= maxFailCount)
            {
                Debug.Log("宝箱が壊れた！");
                // パネルを非表示にする
                if (timingGamePanel != null)
                {
                    timingGamePanel.SetActive(false);
                }
                // 必要ならここに追加の壊れた演出やゲームオーバー処理を入れる
            }
        }
    }

    private void GenerateSuccessZone()
    {
        float fixedZoneWidth = Random.Range(0.3f, 0.5f);

        successMin = Random.Range(0f, 1f - fixedZoneWidth);
        successMax = successMin + fixedZoneWidth;

        float justZoneWidth = fixedZoneWidth * justZoneWidthRatio;
        justMin = successMin + (fixedZoneWidth - justZoneWidth) / 2f;
        justMax = justMin + justZoneWidth;

        RectTransform fillRect = timingSlider.fillRect;
        float sliderHeight = ((RectTransform)fillRect.parent).rect.height;

        if (successZoneImage != null)
        {
            RectTransform rt = successZoneImage.rectTransform;
            float zoneHeight = (successMax - successMin) * sliderHeight;
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, successMin * sliderHeight, zoneHeight);
        }

        if (justZoneImage != null)
        {
            RectTransform rt = justZoneImage.rectTransform;
            float justHeight = (justMax - justMin) * sliderHeight;
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, justMin * sliderHeight, justHeight);
        }
    }

    // 失敗回数に応じてハートを割れたハートに差し替える
    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < failCount)
            {
                hearts[i].sprite = brokenHeartSprite;
            }
            else
            {
                hearts[i].sprite = hearts[i].sprite; // 元のハート画像に戻す（必要なら別途元画像を保持）
                
            }
        }
    }

    // 成功時などにハートを全部元に戻す（割れたハート画像を元に戻す）
    private void ResetHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            // ここはInspectorで設定した元画像のままにする想定
            // 必要なら元画像を別に保持し、ここで差し替える
            hearts[i].sprite = hearts[i].sprite;
        }
    }
}
