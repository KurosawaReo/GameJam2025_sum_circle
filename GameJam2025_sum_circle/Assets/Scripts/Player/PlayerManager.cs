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
    [Space]
    [SerializeField] Vector2[] exitPos = new Vector2[(int)RoomExit.Count];
    [SerializeField] Vector2[] roomPos = new Vector2[(int)RoomNum.Count];

    Animator anmSerif1;
    Animator anmSerif2;
    bool     isShowSerif = false; //表示しているか.

    RoomNum  nowRoom; //今いる部屋番号.

    List<SerifData> serif = new List<SerifData>(); //セリフデータ配列.

    void Start()
    {
        InitMyObj();     //MyObject初期化.
        ClearAllSerif(); //セリフリセット.

        //component取得.
        anmSerif1 = pnlSerif1.GetComponent<Animator>();
        anmSerif2 = pnlSerif2.GetComponent<Animator>();

        //プレイヤー初期状態.
        SetAnimMyObj("Back");   //後ろ向きから開始.
        nowRoom = RoomNum.C_01; //最初は廊下01.
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
                SetAnimMyObj((scptGameMng.Phase == Phase.Day) ? "Side" : "Side_Night"); //昼夜で切り替え.
                objPlyImg.GetComponent<SpriteRenderer>().flipX = (vec.x < 0);           //画像反転.
            }
            //縦に移動してるなら.
            else
            {
                //前か後ろか.
                if (vec.y >= 0)
                {
                    ResetAnim();
                    SetAnimMyObj("Back"); //背面は共通.
                }
                else
                {
                    ResetAnim();
                    SetAnimMyObj((scptGameMng.Phase == Phase.Day) ? "Front" : "Front_Night"); //昼夜で切り替え.
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
        //カメラ座標.
        float cameraZ = Camera.main.transform.position.z;

        //部屋別に制御.
        switch (nowRoom)
        {
        //▼範囲内を動く.
            case RoomNum.C_01:
            case RoomNum.C_02:
            case RoomNum.R_Treasure:
            {
                //プレイヤーの座標にカメラを追尾.
                Camera.main.transform.position = new Vector3(Pos.x, Pos.y, cameraZ);
            }
            break;

        //▼1画面固定.
            case RoomNum.R_01:
            case RoomNum.R_02:
            case RoomNum.R_03:
            case RoomNum.R_04:
            case RoomNum.R_05:
            case RoomNum.R_06:
            case RoomNum.R_07:
            case RoomNum.R_08:
            case RoomNum.R_09:
            case RoomNum.R_10:
            {
                //部屋の位置.
                Camera.main.transform.position = new Vector3(roomPos[(int)nowRoom].x, roomPos[(int)nowRoom].y, cameraZ);
            }
            break;

            default: Debug.LogError("[Error] 不正な値です"); break;
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

    /// <summary>
    /// 部屋移動.
    /// </summary>

    private void MoveRoom()
    {
        //TODO: 音やアニメーション.
    }

    /// <summary>
    /// 何かに当たり始めた瞬間の処理.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D hit)
    {
        //name別で処理.
        switch (hit.gameObject.name) 
        {
        //▼各部屋から出る.
            case "Exit-R01":
                Pos = exitPos[(int)RoomExit.C01_1]; //移動.
                nowRoom = RoomNum.C_01;
                MoveRoom();
                break;
            case "Exit-R02":
                Pos = exitPos[(int)RoomExit.C01_2]; //移動.
                nowRoom = RoomNum.C_01;
                MoveRoom();
                break;
            case "Exit-R03":
                Pos = exitPos[(int)RoomExit.C01_3]; //移動.
                nowRoom = RoomNum.C_01;
                MoveRoom();
                break;
            case "Exit-R04":
                Pos = exitPos[(int)RoomExit.C01_4]; //移動.
                nowRoom = RoomNum.C_01;
                MoveRoom();
                break;
            case "Exit-R05":
                Pos = exitPos[(int)RoomExit.C01_5]; //移動.
                nowRoom = RoomNum.C_01;
                MoveRoom();
                break;

            case "Exit-R06":
                Pos = exitPos[(int)RoomExit.C02_1]; //移動.
                nowRoom = RoomNum.C_02;
                MoveRoom();
                break;
            case "Exit-R07":
                Pos = exitPos[(int)RoomExit.C02_2]; //移動.
                nowRoom = RoomNum.C_02;
                MoveRoom();
                break;
            case "Exit-R08":
                Pos = exitPos[(int)RoomExit.C02_3]; //移動.
                nowRoom = RoomNum.C_02;
                MoveRoom();
                break;
            case "Exit-R09":
                Pos = exitPos[(int)RoomExit.C02_4]; //移動.
                nowRoom = RoomNum.C_02;
                MoveRoom();
                break;
            case "Exit-R10":
                Pos = exitPos[(int)RoomExit.C02_5]; //移動.
                nowRoom = RoomNum.C_02;
                MoveRoom();
                break;

        //▼廊下1から出る.
            case "Exit-C01-1":
                Pos = exitPos[(int)RoomExit.R_01]; //移動.
                nowRoom = RoomNum.R_01;
                MoveRoom();
                break;
            case "Exit-C01-2":
                Pos = exitPos[(int)RoomExit.R_02]; //移動.
                nowRoom = RoomNum.R_02;
                MoveRoom();
                break;
            case "Exit-C01-3":
                Pos = exitPos[(int)RoomExit.R_03]; //移動.
                nowRoom = RoomNum.R_03;
                MoveRoom();
                break;
            case "Exit-C01-4":
                Pos = exitPos[(int)RoomExit.R_04]; //移動.
                nowRoom = RoomNum.R_04;
                MoveRoom();
                break;
            case "Exit-C01-5":
                Pos = exitPos[(int)RoomExit.R_05]; //移動.
                nowRoom = RoomNum.R_05;
                MoveRoom();
                break;
            case "Exit-C01-6":
                Pos = exitPos[(int)RoomExit.C02_7]; //移動.
                nowRoom = RoomNum.C_02;
                MoveRoom();
                break;

        //▼廊下2から出る.
            case "Exit-C02-1":
                Pos = exitPos[(int)RoomExit.R_06]; //移動.
                nowRoom = RoomNum.R_06;
                MoveRoom();
                break;
            case "Exit-C02-2":
                Pos = exitPos[(int)RoomExit.R_07]; //移動.
                nowRoom = RoomNum.R_07;
                MoveRoom();
                break;
            case "Exit-C02-3":
                Pos = exitPos[(int)RoomExit.R_08]; //移動.
                nowRoom = RoomNum.R_08;
                MoveRoom();
                break;
            case "Exit-C02-4":
                Pos = exitPos[(int)RoomExit.R_09]; //移動.
                nowRoom = RoomNum.R_09;
                MoveRoom();
                break;
            case "Exit-C02-5":
                Pos = exitPos[(int)RoomExit.R_10]; //移動.
                nowRoom = RoomNum.R_10;
                MoveRoom();
                break;
            case "Exit-C02-6":
                Pos = exitPos[(int)RoomExit.R_Treasure]; //移動.
                nowRoom = RoomNum.R_Treasure;
                MoveRoom();
                break;
            case "Exit-C02-7":
                Pos = exitPos[(int)RoomExit.C01_6]; //移動.
                nowRoom = RoomNum.C_01;
                MoveRoom();
                break;

        //▼宝箱部屋から出る.
            case "Exit-Treasure":
                Pos = exitPos[(int)RoomExit.C02_6]; //移動.
                nowRoom = RoomNum.C_02;
                MoveRoom();
                break;
        }

        Debug.Log("行き先: " + nowRoom);
    }
}
