/*
   - PlayerManager.cs -
   担当:黒澤
*/
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// セリフデータ.
/// </summary>
public class SerifData
{
    public int    dayCnt; //日数.
    public string msg;    //メッセージ内容.
}

public class PlayerManager : MonoBehaviour
{
    [Header("- script-")]
    [SerializeField] GameManager scptGameMng;

    [Header("- value -")]
    [SerializeField] float moveSpeed; //移動速度.

    List<SerifData> serif; //セリフデータ配列.

    void Update()
    {
        Move(); //プレイヤー移動.
    }

    /// <summary>
    /// プレイヤー移動.
    /// </summary>
    private void Move()
    {
        //左.
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
        //右.
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        //上.
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        }
        //下.
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        }
    }

    /// <summary>
    /// プレイヤー発見.
    /// </summary>
    public void Found()
    {
        //TODO: ゲームオーバー処理.

        scptGameMng.ResultGame(); //リザルトへ.
    }

    /// <summary>
    /// セリフを履歴として保存する.
    /// </summary>
    /// <param name="message">メッセージ内容</param>
    public void SaveSerif(string message)
    {
        SerifData tmp = new SerifData(); //データ作成.
        tmp.msg    = message;            //メッセージ保存.
        tmp.dayCnt = scptGameMng.DayCnt; //GameManagerから日数取得.

        serif.Add(tmp); //履歴に追加.
    }

    /// <summary>
    /// セリフ履歴を表示.
    /// </summary>
    public void ShowSerif()
    {
        //TODO: どう表示するか.
    }

    /// <summary>
    /// 全てのセリフ履歴を削除.
    /// </summary>
    public void ClearAllSerif()
    {
        serif.Clear();
    }
}
