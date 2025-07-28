/*
   - PlayerManager.cs -
   担当:黒澤
*/
using System.Collections.Generic;
using UnityEngine;

using MyLib.Object;
using MyLib.Calc;
using MyLib.InputST;

/// <summary>
/// セリフデータ.
/// </summary>
public class SerifData
{
    public int    dayCnt; //日数.
    public string msg;    //メッセージ内容.
}

public class PlayerManager : MyObject
{
    [Header("- script-")]
    [SerializeField] GameManager scptGameMng;

    [Header("- value -")]
    [SerializeField] float moveSpeed; //移動速度.

    List<SerifData> serif; //セリフデータ配列.

    void Start()
    {
        InitMyObj(); //MyObject初期化.
    }
    void Update()
    {
        Move(); //プレイヤー移動.
    }

    /// <summary>
    /// プレイヤー移動.
    /// </summary>
    private void Move()
    {
        //入力を取得.
        Vector2 input = IN_Func.GetMove4dir();
        //入力した方向の角度を求める.
        Vector2 vec   = CL_Func.CalcInputVec(input);
        //移動処理.
        MoveMyObj(vec, moveSpeed);
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
