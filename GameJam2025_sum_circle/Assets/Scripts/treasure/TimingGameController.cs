using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Utility;
using System.Collections;

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

    [Header("レジェンダリー専用報酬リスト")]
    public List<Item> legendaryRewards;  // ここにレジェンダリーだけ入れる（未使用だが保持可能）

    [Header("報酬アイコン表示")]
    [SerializeField] private Transform rewardIconParent;   // アイコンを表示するUIの親
    [SerializeField] private GameObject rewardIconPrefab;  // アイコンのPrefab

    [Header("結果エフェクト")]
    [SerializeField] private GameObject successEffectPrefab;
    [SerializeField] private GameObject failEffectPrefab;
    [SerializeField] private GameObject AllfailEffectPrefab;
    [SerializeField] private GameObject justSuccessEffectPrefab;
    [SerializeField] public GameObject SpecialRewardEffectPrefab;
    [SerializeField] public GameObject JustBonusEffectPrefab;
    [SerializeField] public Transform resultEffectParent;

    [Header("宝箱画像関連")]
    [SerializeField] private Image chestImage;
    [SerializeField] private Sprite chestClosedSprite;      // 閉じている
    [SerializeField] private Sprite chestOpeningSprite;     // 開きかけ
    [SerializeField] private Sprite chestOpenedSprite;      // 開いた

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
    private float initialSpeed;  // ← スピードリセット用

    private bool isChestOpenedCompletely = false; // 追加

    void Start()
    {
        GameStart();
    }

    void GameStart()
    {
        progressManager = FindObjectOfType<GameProgressManager>();
        if (progressManager != null)
        {
            currentChestRarity = progressManager.GetCurrentChestRarity();
            Debug.Log($"宝箱レアリティ設定：{currentChestRarity}");
        }

        initialSpeed = speed;  // 初期値保存
        timingSlider.direction = Slider.Direction.BottomToTop;
        SetupHeartsForDifficulty(GetFailCountByRarity(currentChestRarity)); // 難易度調整
        GenerateSuccessZone();
    }

    void Update()
    {
        // 宝箱完全オープン時はスライダーを動かさない
        if (isChestOpenedCompletely) return;

        if (isIncreasing)
            timingSlider.value += Time.deltaTime * speed;
        else
            timingSlider.value -= Time.deltaTime * speed;

        if (timingSlider.value >= 1f) isIncreasing = false;
        if (timingSlider.value <= 0f) isIncreasing = true;


        // デバッグ用にFキーで強制ジャスト成功を呼ぶ例
        if (Input.GetKeyDown(KeyCode.F))
        {
            ForceJustSuccess();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            ForceNormalSuccess();
        }
    }

    public void OnPress()
    {
        float value = timingSlider.value;

        if (value >= successMin && value <= successMax)
        {
            if (value >= justMin && value <= justMax)
            {
                Debug.Log("ジャスト成功！");
                justSuccessCount++;
                speed += speedIncrease * 2f;
                PlayResultEffect(justSuccessEffectPrefab);
            }
            else
            {
                Debug.Log("成功！");
                normalSuccessCount++;
                speed += speedIncrease;
                PlayResultEffect(successEffectPrefab);
            }

            failCount = 0;
            successCount++;
            ResetHearts();
            GenerateSuccessZone();

            // 宝箱画像更新
            UpdateChestImage();

            if (successCount >= successToReward)
            {
                GiveReward();
                
                // ここで宝箱を完全オープン状態に固定
                isChestOpenedCompletely = true;
                chestImage.sprite = chestOpenedSprite;

                // スライダーを非表示または無効化
                timingSlider.interactable = false;

                successCount = 0;

            }
        }
        else
        {
            Debug.Log("失敗！");
            failCount++;
            UpdateHearts();
            PlayResultEffect(failEffectPrefab);

            if (failCount >= maxFailCount)
            {
                Debug.Log("宝箱が壊れた！");
                PlayResultEffect(AllfailEffectPrefab);

                if (timingGamePanel != null)
                    timingGamePanel.SetActive(false);

                successCount = 0;
                UpdateChestImage();
            }
        }

    }

    private void UpdateChestImage()
    {
        if (chestImage == null) return;

        // successToReward = 3 回の成功で完全オープン
        if (successCount == 0)
        {
            chestImage.sprite = chestClosedSprite;
        }
        else if (successCount < successToReward)
        {
            chestImage.sprite = chestOpeningSprite;
        }
        else if (successCount >= successToReward)
        {
            chestImage.sprite = chestOpenedSprite;
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

    public void SetupHeartsForDifficulty(int heartCount)
    {
        failCount = 0;

        foreach (Transform child in heartsParent)
        {
            Destroy(child.gameObject);
        }
        hearts.Clear();
        originalHeartSprites.Clear();

        for (int i = 0; i < heartCount; i++)
        {
            GameObject heartObj = Instantiate(heartPrefab, heartsParent);
            Image heartImage = heartObj.GetComponent<Image>();
            hearts.Add(heartImage);
            originalHeartSprites.Add(heartImage.sprite);
        }
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < failCount)
                hearts[i].sprite = brokenHeartSprite;
            else
                hearts[i].sprite = originalHeartSprites[i];
        }
    }

    private void ResetHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
            hearts[i].sprite = originalHeartSprites[i];
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
        // ジャスト成功3回の特別報酬優先
        if (justSuccessCount >= 3)
        {
            Item specialItem = GetSpecialJustReward();
            if (specialItem != null)
            {
                InventoryManager.Instance.AddItem(specialItem, 1);
                Debug.Log($"ジャスト3回達成！特別アイテム「{specialItem.itemName}」を獲得！");
                ShowRewardIcon(specialItem);
                PlaySpecialRewardEffect();
            }
            else
            {
                Debug.LogWarning("特別アイテムが取得できませんでした");
            }
        }
        else
        {
            // 通常報酬はレジェンダリーを除外してランダム選択
            var rarityReward = rarityRewards.Find(r => r.rarity == currentChestRarity);
            if (rarityReward == null || rarityReward.rewardItems.Count == 0)
            {
                Debug.LogWarning("報酬リストが空か設定されていません");
                return;
            }
            var nonLegendaryItems = rarityReward.rewardItems.FindAll(item => item.rarity != Item.Rarity.Legendary);
            if (nonLegendaryItems.Count == 0)
            {
                Debug.LogWarning("レジェンダリー以外の報酬アイテムがありません");
                return;
            }

            int index = Random.Range(0, nonLegendaryItems.Count);
            Item reward = nonLegendaryItems[index];
            InventoryManager.Instance.AddItem(reward, 1);
            ShowRewardIcon(reward);

            Debug.Log($"報酬アイテム「{reward.itemName}」を獲得！（レアリティ：{currentChestRarity}）");

            // ジャスト2回以上3未満なら30%の確率で追加報酬と演出
            if (justSuccessCount >= 2 && justSuccessCount < 3)
            {
                float bonusChance = 0.3f;
                if (Random.value < bonusChance)
                {
                    int bonusIndex = Random.Range(0, nonLegendaryItems.Count);
                    Item bonusReward = nonLegendaryItems[bonusIndex];
                    InventoryManager.Instance.AddItem(bonusReward, 1);
                    ShowRewardIcon(bonusReward);
                    Debug.Log($"ジャストボーナス！追加報酬「{bonusReward.itemName}」を獲得！");
                    PlayJustBonusEffect();

                }
            }
        }
        
        speed = initialSpeed;
        justSuccessCount = 0;
        normalSuccessCount = 0;
    }

    private void ShowRewardIcon(Item item)
    {
        if (item == null || rewardIconPrefab == null || rewardIconParent == null) return;

        // 既存のアイコンを削除
        foreach (Transform child in rewardIconParent)
        {
            Destroy(child.gameObject);
        }

        // 新しいアイコンを生成
        GameObject iconObj = Instantiate(rewardIconPrefab, rewardIconParent);
        Image iconImage = iconObj.GetComponent<Image>();

        if (iconImage != null)
        {
            iconImage.sprite = item.icon;
        }

        // CanvasGroupがなければ追加
        CanvasGroup cg = iconObj.GetComponent<CanvasGroup>();
        if (cg == null) cg = iconObj.AddComponent<CanvasGroup>();
        cg.alpha = 0f;

        // フェードアニメーション開始
        StartCoroutine(AnimateRewardIcon(iconObj, cg));
    }

    private IEnumerator AnimateRewardIcon(GameObject iconObj, CanvasGroup cg)
    {
        float fadeInTime = 0.5f;
        float waitTime = 1.5f;
        float fadeOutTime = 0.5f;

        // フェードイン
        float t = 0f;
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, t / fadeInTime);
            yield return null;
        }

        // 少し待機
        yield return new WaitForSeconds(waitTime);

        // フェードアウト
        t = 0f;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, t / fadeOutTime);
            yield return null;
        }

        Destroy(iconObj);

        // パネルをオフにする
        if (timingGamePanel != null)
            timingGamePanel.SetActive(false);

    }


    private void PlayResultEffect(GameObject effectPrefab)
    {
        if (effectPrefab == null || resultEffectParent == null) return;

        // 既存エフェクトを削除してから新しいエフェクトを表示
        foreach (Transform child in resultEffectParent)
        {
            Destroy(child.gameObject);
        }

        GameObject effect = Instantiate(effectPrefab, resultEffectParent);
        Destroy(effect, 1.5f);  // 1.5秒後に自動削除
    }



    private void PlayJustBonusEffect()
    {
        Debug.Log("ジャストボーナス演出再生！");
        PlayResultEffect(JustBonusEffectPrefab);

        //SE
    }

    private void PlaySpecialRewardEffect()
    {
        Debug.Log("特別報酬演出再生！");
        PlayResultEffect(SpecialRewardEffectPrefab);

        //SE
    }

    private Item GetSpecialJustReward()
    {
        var superRareReward = rarityRewards.Find(r => r.rarity == ChestRarity.SuperRare);

        if (superRareReward != null && superRareReward.rewardItems.Count > 0)
        {
            var legendaryItems = superRareReward.rewardItems.FindAll(item => item.rarity == Item.Rarity.Legendary);
            if (legendaryItems.Count > 0)
            {
                int index = Random.Range(0, legendaryItems.Count);
                return legendaryItems[index];
            }
            else
            {
                Debug.LogWarning("スーパーレア報酬リストにレジェンダリーアイテムがありません");
                return null;
            }
        }
        else
        {
            Debug.LogWarning("スーパーレア報酬リストが設定されていません");
            return null;
        }
    }

#if UNITY_EDITOR
    public void ForceJustSuccess()
    {
        Debug.Log("【デバッグ】強制ジャスト成功！");

        justSuccessCount++;
        speed += speedIncrease * 2f;

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
    public void ForceNormalSuccess()
    {
        Debug.Log("【デバッグ】強制通常成功！");

        normalSuccessCount++;
        speed += speedIncrease;

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
#endif
}