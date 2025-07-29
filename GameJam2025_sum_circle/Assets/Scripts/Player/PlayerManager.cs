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
using Global;

/// <summary>
/// セリフデータ.
/// </summary>
public class SerifData
{
    public string msg; //メッセージ内容.
}

public class PlayerManager : MyObject
{
    [Header("- script-")]
    [SerializeField] GameManager scptGameMng;

    [Header("- object -")]
    [SerializeField] GameObject objPlyImg; //プレイヤー画像オブジェクト.

    [Header("- panel -")]
    [SerializeField] GameObject pnlSerif1; //セリフ記録.
    [SerializeField] GameObject pnlSerif2; //セリフ記録.

    [Header("- text -")]
    [SerializeField] Text txtSerif;

    [Header("- value -")]
    [SerializeField]              float moveSpeed;    //移動速度.
    [SerializeField, Range(0, 1)] float reduceInputY; //Y入力をどれだけ減らすか.

    Animator anmSerif1;
    Animator anmSerif2;
    bool     isShowSerif = false; //表示しているか.

    RoomType nowRoom; //今いる部屋のタイプ.

    List<SerifData> serif = new List<SerifData>(); //セリフデータ配列.

    void Start()
    {
        InitMyObj();     //MyObject初期化.
        ClearAllSerif(); //セリフリセット.

        //component取得.
        anmSerif1 = pnlSerif1.GetComponent<Animator>();
        anmSerif2 = pnlSerif2.GetComponent<Animator>();

        //test.
        SaveSerif("A: 東に行くと何かがあるよ");
        SaveSerif("B: そこには何もないよ");
        SaveSerif("C: 嘘だったよ");
        SaveSerif("D: ふざけんなよ");
        SaveSerif("A: はああああああああ");
        SaveSerif("B: ああああああああ");
        SaveSerif("C: いいいいいいいいい");
        SaveSerif("D: うああああああああああああああああ");
        nowRoom = RoomType.FullScreen;
    }
    void Update()
    {
        PlayerMove(); //プレイヤー移動.
        CameraMove(); //カメラ移動.
        ShowSerif();  //セリフメモを表示.
    }

    /// <summary>
    /// リセット処理.
    /// </summary>
    public void ResetPlayer()
    {
        ResetMyObj();    //MyObjectのリセット.
        ClearAllSerif(); //セリフ削除.
    }

    /// <summary>
    /// アニメーションのリセット処理.
    /// </summary>
    public void ResetAnim()
    {
        ResetAnimMyObj("Front");
        ResetAnimMyObj("Side");
        ResetAnimMyObj("Back");
    }

    /// <summary>
    /// プレイヤー移動.
    /// </summary>
    private void PlayerMove()
    {
        Vector2 input = IN_Func.GetMove4dir();       //入力を取得.
        Vector2 vec   = CL_Func.CalcInputVec(input); //入力した方向の角度を求める.
        vec.y *= reduceInputY;                       //上下の入力は少し抑える.

        //少しでも移動していれば.
        if (vec != Vector2.zero)
        {
            //横に移動してるなら.
            if (Mathf.Abs(vec.x) >= Mathf.Abs(vec.y))
            {
                ResetAnim();
                SetAnimMyObj("Side");
                objPlyImg.GetComponent<SpriteRenderer>().flipX = (vec.x < 0); //画像反転.
            }
            //縦に移動してるなら.
            else
            {
                //前か後ろか.
                if (vec.y >= 0)
                {
                    ResetAnim();
                    SetAnimMyObj("Back");
                }
                else
                {
                    ResetAnim();
                    SetAnimMyObj("Front");
                }
            }

            MoveMyObj(vec, moveSpeed); //移動処理.
        }
        //歩いているか.
        SetAnimMyObj("isWalk", vec != Vector2.zero);
    }

    /// <summary>
    /// カメラ移動.
    /// </summary>
    private void CameraMove()
    {
        //カメラ動く.
        if (nowRoom == RoomType.MoveScreen)
        {
            //カメラ座標.
            Vector3 cameraPos = Camera.main.transform.position;
            //プレイヤーの座標にカメラを追尾.
            Camera.main.transform.position = new Vector3(Pos.x, Pos.y, cameraPos.z);
        }
        //カメラ固定.
        else
        {
            //TODO: 部屋の座標を求める.
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
    /// 全てのセリフ記録を削除.
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
                anmSerif1.SetBool("Show", false); //隠す.
                anmSerif2.SetBool("Show", true);  //出す.
            }
            else
            {
                anmSerif1.SetBool("Show", true);  //出す.
                anmSerif2.SetBool("Show", false); //隠す.
            }
            isShowSerif = !isShowSerif; //切り替え.
        }
    }
}
