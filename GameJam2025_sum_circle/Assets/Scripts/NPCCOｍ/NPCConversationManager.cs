using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCConversationManager : MonoBehaviour
{
    public static NPCConversationManager Instance { get; private set; }

    public GameObject conversationPanel;
    public Text npcNameText;
    public Text messageText;

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

    // ゲーム日数などが取れる管理クラス（適宜セット）
    public GameProgressManager scptGameMng;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        conversationPanel.SetActive(false);
    }

    private void Update()
    {
        if (!isTalking) return;

        if (Input.GetKeyDown(KeyCode.E))
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

    public void StartConversation(string npcId, string npcName, string message)
    {
        if (isTalking) return;

        isTalking = true;
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
        conversationPanel.SetActive(false);
        isTalking = false;
        messageText.text = "";
    }

    private void SendNightTrigger()
    {
        Debug.Log("[夜イベントへ切り替え] SendNightTrigger() が呼び出されました");
    }


}

