using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Utility;

public class TimingGameController : MonoBehaviour
{
    public Slider timingSlider;
    public float speed = 1.0f;
    public float speedIncrease = 0.1f;

    [Header("ゾーン表示")]
    public Image successZoneImage;  // 成功ゾーン（緑の帯）
    public Image justZoneImage;     // ジャストゾーン（別画像）

    [Header("ハートUI")]
    public Transform heartsParent;      // ハートの親オブジェクト（UIのPanelなど）
    public GameObject heartPrefab;      // ハートのPrefab（通常の♡画像）
    public Sprite brokenHeartSprite;    // 割れたハート画像

    [Header("ゲームパネル")]
    public GameObject timingGamePanel;

    [Header("レアリティ別報酬リスト")]
    public List<RarityReward> rarityRewards;

    [Header("現在の宝箱レアリティ")]
    public ChestRarity currentChestRarity = ChestRarity.Normal;

    private List<Image> hearts = new List<Image>();
    private List<Sprite> originalHeartSprites = new List<Sprite>();
    private GameProgressManager progressManager;

    private bool isIncreasing = true;
    private float successMin;
    private float successMax;
    private float justMin;
    private float justMax;

    private float justZoneWidthRatio = 0.2f;

    private int failCount = 0;
    private int justSuccessCount = 0;
    private int normalSuccessCount = 0;
    private int successCount = 0; // 成功カウント（ジャスト含む）

    public int maxFailCount = 3; // 難易度に応じて設定
    public int successToReward = 3; // 何回成功したら報酬？



    void Start()
    {
        progressManager = FindObjectOfType<GameProgressManager>();
        if (progressManager != null)
        {
            currentChestRarity = progressManager.GetCurrentChestRarity();
            Debug.Log($"宝箱レアリティ設定：{currentChestRarity}");
        }

        timingSlider.direction = Slider.Direction.BottomToTop;
        SetupHeartsForDifficulty(GetFailCountByRarity(currentChestRarity)); // 難易度調整
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
                justSuccessCount++;  // ← ここでジャスト成功回数をカウント
                speed += speedIncrease * 2f;
            }
            else
            {
                Debug.Log("成功！");
                normalSuccessCount++;  // ← ここで通常成功回数をカウント
                speed += speedIncrease;
            }

            failCount = 0;
            successCount++;
            ResetHearts();
            GenerateSuccessZone();

            if (successCount >= successToReward)
            {
                GiveReward();
                successCount = 0;
            }
        }
        else
        {
            Debug.Log("失敗！");
            failCount++;
            UpdateHearts();

            if (failCount >= maxFailCount)
            {
                Debug.Log("宝箱が壊れた！");
                if (timingGamePanel != null)
                    timingGamePanel.SetActive(false);
                successCount = 0;
            }
        }
    }

    private void GenerateSuccessZone()
    {
        // 難易度やレアリティに応じて成功ゾーンの幅を変えたい場合はここで調整可能
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

    // 指定された数でハートを動的生成・セットアップ
    public void SetupHeartsForDifficulty(int heartCount)
    {
        failCount = 0;

        // 既存のハートを削除
        foreach (Transform child in heartsParent)
        {
            Destroy(child.gameObject);
        }
        hearts.Clear();
        originalHeartSprites.Clear();

        // 新規生成
        for (int i = 0; i < heartCount; i++)
        {
            GameObject heartObj = Instantiate(heartPrefab, heartsParent);
            Image heartImage = heartObj.GetComponent<Image>();
            hearts.Add(heartImage);
            originalHeartSprites.Add(heartImage.sprite); // 元画像を保存
        }
    }

    // 失敗数に応じてハートを割れたものに差し替え
    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < failCount)
            {
                hearts[i].sprite = brokenHeartSprite;
            }
            else
            {
                hearts[i].sprite = originalHeartSprites[i];
            }
        }
    }

    // ハートを元画像に戻す
    private void ResetHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].sprite = originalHeartSprites[i];
        }
    }
    private int GetFailCountByRarity(ChestRarity rarity)
    {
        switch (rarity)
        {
            case ChestRarity.SuperRare: return 1;
            case ChestRarity.Rare: return 2;
            case ChestRarity.Normal:
            default: return 3;
        }
    }

    private void GiveReward()
    {
        // レアリティに対応した報酬リスト取得
        var rarityReward = rarityRewards.Find(r => r.rarity == currentChestRarity);
        if (rarityReward == null || rarityReward.rewardItems.Count == 0)
        {
            Debug.LogWarning("報酬リストが空か設定されていません");
            return;
        }

        int index = Random.Range(0, rarityReward.rewardItems.Count);
        Item reward = rarityReward.rewardItems[index];

        // InventoryManagerなどにアイテムを追加（あなたのゲームの実装に合わせてください）
        InventoryManager.Instance.AddItem(reward, 1);

        if (timingGamePanel != null)
            timingGamePanel.SetActive(false);

        Debug.Log($"報酬アイテム「{reward.itemName}」を獲得！（レアリティ：{currentChestRarity}）");
    }
}
