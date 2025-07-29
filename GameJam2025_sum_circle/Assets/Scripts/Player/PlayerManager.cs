/*
   - PlayerManager.cs -
   担当:黒澤
*/
using System.Collections.Generic;
using UnityEngine;

using MyLib.Object;
using MyLib.Calc;
using MyLib.InputST;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// セリフデータ.
/// </summary>
public class SerifData
{
    public string msg = ""; //メッセージ内容.
}

public class PlayerManager : MyObject
{
    [Header("- script-")]
    [SerializeField] GameManager scptGameMng;

    [Header("- text -")]
    [SerializeField] Text txtSerif;

    [Header("- panel -")]
    [SerializeField] GameObject pnlSerif; //セリフメモ.

    [Header("- value -")]
    [SerializeField]              float moveSpeed;    //移動速度.
    [SerializeField, Range(0, 1)] float reduceInputY; //Y入力をどれだけ減らすか.

    Animator anmSerif;
    bool     isShowSerif = false; //表示しているか.

    List<SerifData> serif = new List<SerifData>(); //セリフデータ配列.

    void Start()
    {
        InitMyObj();     //MyObject初期化.
        ClearAllSerif(); //セリフリセット.

        //component取得.
        anmSerif = pnlSerif.GetComponent<Animator>();

        SaveSerif("東に行くと何かがあるよ");
        SaveSerif("そこには何もないよ");
        SaveSerif("嘘だったよ");
        SaveSerif("はああああああああ");
        SaveSerif("ああああああああ");
        SaveSerif("いいいいいいいいい");
        SaveSerif("うああああああああ");
    }
    void Update()
    {
        PlayerMove(); //プレイヤー移動.
        CameraMove(); //カメラ移動.
        ShowSerif();  //セリフメモを表示.
    }

    /// <summary>
    /// プレイヤー移動.
    /// </summary>
    private void PlayerMove()
    {
        Vector2 input = IN_Func.GetMove4dir();       //入力を取得.
        Vector2 vec   = CL_Func.CalcInputVec(input); //入力した方向の角度を求める.
        vec.y *= reduceInputY;                       //上下の入力は少し抑える.

        MoveMyObj(vec, moveSpeed); //移動処理.
    }

    /// <summary>
    /// カメラ移動.
    /// </summary>
    private void CameraMove()
    {
        //カメラ座標.
        Vector3 cameraPos = Camera.main.transform.position;
        //プレイヤーの座標にカメラを追尾.
        Camera.main.transform.position = new Vector3(Pos.x, Pos.y, cameraPos.z);
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
    /// セリフをメモとして保存する.
    /// </summary>
    /// <param name="message">メッセージ内容</param>
    public void SaveSerif(string message)
    {
        SerifData tmp = new SerifData(); //データ作成.
        tmp.msg = message;               //メッセージ保存.
        serif.Add(tmp);                  //メモに追加.

        UpdateSerif(); //セリフ更新.
    }

    /// <summary>
    /// 全てのセリフメモを削除.
    /// </summary>
    public void ClearAllSerif()
    {
        serif.Clear(); //全削除.
        UpdateSerif(); //セリフ更新.
    }

    private void UpdateSerif()
    {
        txtSerif.text = ""; //一旦空にする.

        //セリフ全取得.
        foreach (var i in serif)
        {
            txtSerif.text += i.msg; //メッセージを取り出す.
            txtSerif.text += "\n";  //改行.
        }
    }

    /// <summary>
    /// セリフメモを表示.
    /// </summary>
    private void ShowSerif()
    {
        //ボタンを押したら.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //表示済なら.
            if (isShowSerif)
            {
                anmSerif.SetBool("Show", false); //隠す.
            }
            else
            {
                anmSerif.SetBool("Show", true);  //出す.
            }
            isShowSerif = !isShowSerif; //切り替え.
        }
    }
}
