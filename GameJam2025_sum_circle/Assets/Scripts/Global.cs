
/// <summary>
/// グローバルのテンプレ(空)
/// 汎用定数や汎用関数などをまとめる用.
/// </summary>
namespace Global
{
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

        public const int   GAME_DAY_COUNT = 7;  //ゲームで行う日数.
        public const int   SERIF_SAVE_MAX = 21; //セリフを保存する最大数.
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