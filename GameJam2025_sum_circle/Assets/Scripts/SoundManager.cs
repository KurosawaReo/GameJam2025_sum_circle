/*
   - SoundManager.cs -
   陳尾先輩提供のソースを元に制作.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Utility;

/// <summary>
/// サウンド管理クラス（BGM, SE対応 + フェード + スライダー連動 + SEプール）
/// </summary>
public class SoundManager : MonoBehaviour
{
//▼Inspector入力.
    [Header("BGM用 AudioSource")]
    [SerializeField] AudioSource bgmSource;

    [Header("BGM クリップ一覧")]
    [SerializeField] List<AudioClip> bgmClips; //BGM用.

    [Header("SE クリップ一覧")]
    [SerializeField] List<AudioClip> seClips; //SE用.

    [Header("SE AudioSource プール数")]
    [SerializeField] int sePoolSize = 5;

//▼static変数.
    static SoundManager Instance; //このクラスの実体.

//▼private変数.
    private Dictionary<string, AudioClip> bgmDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> seDict = new Dictionary<string, AudioClip>();

    private List<AudioSource> seSources = new List<AudioSource>();
    private int seIndex = 0;

    private Coroutine fadeCoroutine;

//▼関数.
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeClips();
            InitSEPool();

            // 初期音量設定
            SetBGMVolume(0.5f);
            SetSEVolume(0.5f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeClips()
    {
        foreach (var clip in bgmClips)
        {
            //入ってなければ.
            if (!bgmDict.ContainsKey(clip.name))
            {
                bgmDict.Add(clip.name, clip); //追加.
            }
        }
        foreach (var clip in seClips)
        {
            //入ってなければ.
            if (!seDict.ContainsKey(clip.name))
            {
                seDict.Add(clip.name, clip); //追加.
            }
        }
    }

    void InitSEPool()
    {
        for (int i = 0; i < sePoolSize; i++)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            seSources.Add(src);
        }
    }

    // ==== BGM ====

    public void PlayBGM(string name)
    {
        if (bgmDict.TryGetValue(name, out var clip))
        {
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"BGM '{name}' が見つかりません。");
        }
    }

    public void PlayBGMWithFade(string name, float fadeDuration = 1f)
    {
        if (bgmDict.TryGetValue(name, out var newClip))
        {
            if (bgmSource.clip == newClip) return;
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeInNewBGM(newClip, fadeDuration));
        }
        else
        {
            Debug.LogWarning($"BGM '{name}' が見つかりません。");
        }
    }

    private IEnumerator FadeInNewBGM(AudioClip newClip, float duration)
    {
        float originalVolume = bgmSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(originalVolume, 0, t / duration);
            yield return null;
        }


        bgmSource.Stop();
        bgmSource.clip = newClip;
        bgmSource.loop = true; 
        bgmSource.Play();

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(0, originalVolume, t / duration);
            yield return null;
        }

        bgmSource.volume = originalVolume;
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    public float GetBGMVolume() => bgmSource.volume;

    // ==== SE ====

    public void PlaySE(string name)
    {
        if (seDict.TryGetValue(name, out var clip))
        {
            seSources[seIndex].PlayOneShot(clip);
            seIndex = (seIndex + 1) % seSources.Count;
        }
        else
        {
            Debug.LogWarning($"SE '{name}' が見つかりません。");
        }
    }

    public void SetSEVolume(float volume)
    {
        foreach (var src in seSources)
        {
            src.volume = volume;
        }
    }

    public float GetSEVolume() => seSources[0].volume;

    // ==== スライダー連動 ====
    //スライダーの値をここに代入する.

    public void OnBGMVolumeChanged(float value)
    {
        SetBGMVolume(value);
    }

    public void OnSEVolumeChanged(float value)
    {
        SetSEVolume(value);
    }
}
