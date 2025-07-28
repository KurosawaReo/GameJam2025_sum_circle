/*
   - MyLib.Calc -
   ver.2025/07/29
*/
using UnityEngine;
using static UnityEditor.PlayerSettings;

/// <summary>
/// 計算を扱う用の追加機能.
/// </summary>
namespace MyLib.Calc
{
    /// <summary>
    /// Calc関数.
    /// </summary>
    public static class CL_Func
    {
        /// <summary>
        /// プラスかマイナスかを取得.
        /// </summary>
        /// <param name="num">元の値</param>
        /// <returns>符号(0/+1/-1)</returns>
        public static int GetNumSign(float num)
        {
            //0なら0、それ以外は符号を返す.
            return (num == 0) ? 0 : (num > 0) ? +1 : -1;
        }

        /// <summary>
        /// 0.0～99.9の乱数値を取得(確率計算用)
        /// </summary>
        /// <returns></returns>
        public static float Random100()
        {
            return Random.Range(0.0f, 99.9f);
        }

        /// <summary>
        /// 入力情報から移動方向を求める.
        /// </summary>
        public static Vector2 CalcInputVec(Vector2 input)
        {
            //操作してるなら.
            if (input != Vector2.zero)
            {
                //角度(theta:シータ)をラジアンで求める.
                var theta = Mathf.Atan2(input.y, input.x); //tan(タンジェント)
                //xとyの移動量を求める.
                var y = Mathf.Sin(theta); //sin = y成分.
                var x = Mathf.Cos(theta); //cos = x成分.
                //ほぼ0の値なら、0とみなす(計算上誤差があるため)
                if (Mathf.Abs(y) < 0.0001f) { y = 0; }
                if (Mathf.Abs(x) < 0.0001f) { x = 0; }

                return new Vector2(x, y);
            }
            else
            {
                return Vector2.zero; //(0, 0)を返す.
            }
        }
    }
}