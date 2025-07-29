
/// <summary>
/// グローバルのテンプレ(空)
/// 汎用定数や汎用関数などをまとめる用.
/// </summary>
namespace Global
{
    /// <summary>
    /// 部屋番号.
    /// </summary>
    public enum RoomNum
    {
        //C = 廊下, R = 部屋.
        C_01,
        C_02,
        R_01,
        R_02,
        R_03,
        R_04,
        R_05,
        R_06,
        R_07,
        R_08,
        R_09,
        R_10,
        R_Treasure, //最奥の宝箱部屋.

        RoomCount, //部屋総数.
    }
    /// <summary>
    /// フェーズ.
    /// </summary>
    public enum Phase
    {
        Day,   //昼.
        Night, //夜.
    }

    /// <summary>
    /// Global定数.
    /// </summary>
    public static class GL_Const
    {
        public const float PLAYER_SIZE = 1;
        public const float GRID_SIZE   = 1;

        public const int   GAME_DAY_COUNT = 7;   //ゲームで行う日数.
        public const int   SERIF_SAVE_MAX = 21;  //セリフを保存する最大数.
    }

    /// <summary>
    /// Global関数.
    /// </summary>
    public static class GL_Func
    {
#if false
        public static void Test()
        {

        }
#endif
    }
}