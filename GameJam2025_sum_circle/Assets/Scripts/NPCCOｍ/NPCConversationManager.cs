using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueOption
{
    public string optionText;
    [TextArea(2, 5)]
    public string responseText;
}

public class NPCConversationManager : MonoBehaviour
{
    public static NPCConversationManager Instance { get; private set; }

    [Header("会話UI")]
    public GameObject conversationPanel;
    public Text npcNameText;
    public Text messageText;

    [Header("選択肢UI")]
    public Transform optionParent;
    public GameObject optionButtonPrefab;


    public float typingSpeed = 0.05f;
    public int charsPerPage = 110;

    private bool isTalking = false;
    private bool isTyping = false;
    private bool canSkip = false;

    private string fullMessage;
    private List<string> pages = new List<string>();
    private int currentPage = 0;

    private Coroutine typingCoroutine;

    private HashSet<string> talkedToNPCs = new HashSet<string>();

    // 好感度データ
    private Dictionary<string, int> npcAffection = new Dictionary<string, int>();
    private HashSet<string> triedAffectionOption = new HashSet<string>();

    // 現在会話中のNPC
    private string currentNpcId = "";
    private NPCDialogueData currentNpcData;

    // 会話状態
    private enum ConversationPhase
    {
        InitialChoice,
        AffectionChoice,
        ShowResponse
    }
    private ConversationPhase conversationPhase = ConversationPhase.InitialChoice;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        conversationPanel.SetActive(false);

        // ゲーム開始時に好感度をロード
        LoadAffection();
    }

    private void OnApplicationQuit()
    {
        SaveAffection(); // 終了時に保存
    }

    private void Update()
    {
        if (!isTalking) return;

        // 選択肢が出ている時はEキー入力を無視
        if (Input.GetKeyDown(KeyCode.E) && optionParent.childCount == 0)
        {
            if (isTyping && canSkip)
            {
                if (typingCoroutine != null)
                    StopCoroutine(typingCoroutine);

                messageText.text = pages[currentPage];
                isTyping = false;
            }
            else if (!isTyping)
            {
                currentPage++;
                if (currentPage >= pages.Count)
                {
                    EndConversation();
                }
                else
                {
                    typingCoroutine = StartCoroutine(TypeMessage(pages[currentPage]));
                }
            }
        }
    }

    // 会話開始
    public void StartConversationWithData(string npcId, string npcName, string message, NPCDialogueData npcData)
    {
        currentNpcData = npcData;
        StartConversation(npcId, npcName, message);
    }

    public void StartConversation(string npcId, string npcName, string message)
    {
        if (isTalking) return;

        isTalking = true;
        currentNpcId = npcId;
        conversationPhase = ConversationPhase.InitialChoice;

        conversationPanel.SetActive(true);
        npcNameText.text = npcName;
        fullMessage = message;
        messageText.text = "";
        currentPage = 0;

        pages = SplitMessageIntoPages(fullMessage, charsPerPage);
        typingCoroutine = StartCoroutine(TypeMessage(pages[currentPage]));

        if (!talkedToNPCs.Contains(npcId))
        {
            talkedToNPCs.Add(npcId);
            if (talkedToNPCs.Count >= 3)
            {
                SendNightTrigger();
            }
        }
    }

    private IEnumerator TypeMessage(string message)
    {
        isTyping = true;
        canSkip = false;
        messageText.text = "";

        yield return new WaitForSeconds(0.3f);
        canSkip = true;

        foreach (char c in message)
        {
            messageText.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;
    }

    private List<string> SplitMessageIntoPages(string message, int maxCharsPerPage)
    {
        List<string> pages = new List<string>();
        int start = 0;
        while (start < message.Length)
        {
            int length = Mathf.Min(maxCharsPerPage, message.Length - start);

            int lastPeriodIndex = -1;
            int searchEnd = start + length;
            if (searchEnd > message.Length) searchEnd = message.Length;

            for (int i = start; i < searchEnd; i++)
            {
                if (message[i] == '。')
                {
                    lastPeriodIndex = i;
                }
            }

            if (lastPeriodIndex >= start)
            {
                int pageLength = lastPeriodIndex - start + 1;
                pages.Add(message.Substring(start, pageLength));
                start += pageLength;
            }
            else
            {
                pages.Add(message.Substring(start, length));
                start += length;
            }
        }
        return pages;
    }

    private void EndConversation()
    {
        if (conversationPhase == ConversationPhase.InitialChoice || conversationPhase == ConversationPhase.AffectionChoice)
        {
            ShowOptions();
        }
        else
        {
            conversationPanel.SetActive(false);
            isTalking = false;
            messageText.text = "";

            SaveAffection(); // 会話終了ごとに保存
        }
    }

    private void ShowOptions()
    {
        foreach (Transform child in optionParent)
        {
            Destroy(child.gameObject);
        }

        if (conversationPhase == ConversationPhase.InitialChoice)
        {
            GameObject infoBtn = Instantiate(optionButtonPrefab, optionParent);
            infoBtn.transform.localScale = Vector3.one;
            infoBtn.GetComponentInChildren<Text>().text = "情報を聞く";
            infoBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                string hint = (GetAffection(currentNpcId) >= 50)
                    ? currentNpcData.highAffectionHint
                    : currentNpcData.normalHint;

                OnOptionSelected(hint, ConversationPhase.ShowResponse);
            });

            if (!triedAffectionOption.Contains(currentNpcId))
            {
                GameObject affectionBtn = Instantiate(optionButtonPrefab, optionParent);
                affectionBtn.transform.localScale = Vector3.one;
                affectionBtn.GetComponentInChildren<Text>().text = "好感度を上げる";
                affectionBtn.GetComponent<Button>().onClick.AddListener(() =>
                {
                    string question = "どうやって仲良くする？";
                    if (currentNpcData != null && currentNpcData.affectionQuestions.Count > 0)
                    {
                        question = currentNpcData.affectionQuestions[Random.Range(0, currentNpcData.affectionQuestions.Count)];
                    }

                    OnOptionSelected(question, ConversationPhase.AffectionChoice);
                });
            }
        }
        else if (conversationPhase == ConversationPhase.AffectionChoice)
        {
            if (currentNpcData != null && currentNpcData.affectionOptions.Count > 0)
            {
                foreach (var option in currentNpcData.affectionOptions)
                {
                    GameObject btn = Instantiate(optionButtonPrefab, optionParent);
                    btn.transform.localScale = Vector3.one;
                    btn.GetComponentInChildren<Text>().text = option.optionText;
                    btn.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        ChangeAffection(currentNpcId, option.affectionChange);
                        triedAffectionOption.Add(currentNpcId);
                        OnOptionSelected(option.responseText, ConversationPhase.ShowResponse);
                    });
                }
            }
        }
    }

    private void OnOptionSelected(string response, ConversationPhase nextPhase)
    {
        foreach (Transform child in optionParent)
        {
            Destroy(child.gameObject);
        }

        conversationPhase = nextPhase;
        isTalking = true;
        fullMessage = response;
        pages = SplitMessageIntoPages(fullMessage, charsPerPage);
        currentPage = 0;
        messageText.text = "";

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeMessage(pages[currentPage]));

        // ✅ 好感度選択肢を選んだ後は再度選択肢を表示できるように設定
        if (nextPhase == ConversationPhase.ShowResponse && triedAffectionOption.Contains(currentNpcId))
        {
            StartCoroutine(ShowOptionsAfterDelay(0.5f));
        }
    }

    private IEnumerator ShowOptionsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!isTalking) yield break; // すでに会話終了なら作らない

        foreach (Transform child in optionParent)
            Destroy(child.gameObject);

        // 情報を聞く
        GameObject infoBtn = Instantiate(optionButtonPrefab, optionParent);
        infoBtn.transform.localScale = Vector3.one;
        infoBtn.GetComponentInChildren<Text>().text = "情報を聞く";
        infoBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            string hint = (GetAffection(currentNpcId) >= 50)
                ? currentNpcData.highAffectionHint
                : currentNpcData.normalHint;

            OnOptionSelected(hint, ConversationPhase.ShowResponse);
        });

        // 会話を終える
        GameObject endBtn = Instantiate(optionButtonPrefab, optionParent);
        endBtn.transform.localScale = Vector3.one;
        endBtn.GetComponentInChildren<Text>().text = "会話を終える";
        endBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            conversationPanel.SetActive(false);
            isTalking = false;
            foreach (Transform child in optionParent) Destroy(child.gameObject);
        });
    }


    private void ChangeAffection(string npcId, int amount)
    {
        if (!npcAffection.ContainsKey(npcId))
            npcAffection[npcId] = 0;

        npcAffection[npcId] = Mathf.Clamp(npcAffection[npcId] + amount, 0, 100);
        Debug.Log($"[{npcId}] 好感度: {npcAffection[npcId]}");
    }

    public int GetAffection(string npcId)
    {
        return npcAffection.ContainsKey(npcId) ? npcAffection[npcId] : 0;
    }

    public bool HasTriedAffection(string npcId)
    {
        return triedAffectionOption.Contains(npcId);
    }

    private void SendNightTrigger()
    {
        Debug.Log("[夜イベントへ切り替え] SendNightTrigger() が呼び出されました");
    }

    // ✅ 好感度データの保存
    public void SaveAffection()
    {
        foreach (var kvp in npcAffection)
        {
            PlayerPrefs.SetInt("Affection_" + kvp.Key, kvp.Value);
        }
        PlayerPrefs.Save();
        Debug.Log("好感度データを保存しました");
    }

    public void LoadAffection()
    {
        // NPCごとにデータが存在するかチェック
        // 必要に応じて事前に登録されているNPCリストを持たせてもOK
        string[] npcIds = { "npc1", "npc2", "npc3" };
        foreach (string id in npcIds)
        {
            int value = PlayerPrefs.GetInt("Affection_" + id, 0);
            npcAffection[id] = value;
        }
        Debug.Log("好感度データを読み込みました");
    }

    public void ResetAllAffection()
    {
        npcAffection.Clear();
        PlayerPrefs.DeleteAll();
        Debug.Log("全ての好感度をリセットしました");
    }
}
