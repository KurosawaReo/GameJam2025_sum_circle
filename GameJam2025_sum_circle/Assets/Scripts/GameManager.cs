/*
   - GameManager.cs -
   担当:黒澤
*/
using UnityEngine;
using Global;

public class GameManager : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] PlayerManager scptPlyMng;

    [Header("- value -")]
    [SerializeField] int   dayCnt;      //日数.
    [SerializeField] int   treasureCnt; //宝の数.
    [SerializeField] Phase phase;       //フェーズ.

    //get, set.
    public int   DayCnt { get => dayCnt; }
    public Phase Phase  { get => phase; }

    public void Start()
    {
        phase = Phase.Night; //テスト.のため夜に
    }

    /// <summary>
    /// ゲームリセット処理.
    /// </summary>
    public void ResetGame()
    {
        dayCnt = 1;
        treasureCnt = 0;
        phase = Phase.Day; //昼から開始.

        //他class処理.
        scptPlyMng.ResetPlayer();
    }

    /// <summary>
    /// リザルトへ.
    /// </summary>
    public void ResultGame()
    {
        //TODO
    }

    /// <summary>
    /// 宝の数加算.
    /// </summary>
    public void AddTreasureCount(int n)
    {
        treasureCnt += n;
    }

    /// <summary>
    /// 日数経過.
    /// </summary>
    public void PassDay()
    {
        dayCnt++;
        //最終日終了したら.
        if (dayCnt > GL_Const.GAME_DAY_COUNT)
        {
            ResultGame();
        }
    }
}
