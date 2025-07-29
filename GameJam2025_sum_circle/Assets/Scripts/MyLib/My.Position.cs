/*
   - MyLib.Position -
   ver.2025/06/27
*/
using UnityEngine;

/// <summary>
/// 座標管理をする用の追加機能.
/// </summary>
namespace MyLib.Position
{
    /// <summary>
    /// 上下左右.
    /// </summary>
    public struct LBRT
    {
        public float left;
        public float bottom;
        public float right;
        public float top;

        //コンストラクタ.
        public LBRT(float _l, float _b, float _r, float _t)
        {
            left   = _l;
            bottom = _b;
            right  = _r;
            top    = _t;
        }
    }

    /// <summary>
    /// Position関数.
    /// </summary>
    public static class PS_Func
    {
        /// <summary>
        /// 画面の上下左右の座標を取得.
        /// </summary>
        public static LBRT GetWindowLBRT()
        {
            //左下と右上の座標取得.
            Vector3 lb = Camera.main.ScreenToWorldPoint(Vector3.zero);
            Vector3 rt = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

            return new LBRT(lb.x, lb.y, rt.x, rt.y);
        }

        /// <summary>
        /// 移動可能範囲内に補正する.
        /// </summary>
        /// <param name="pos">オブジェクト座標</param>
        /// <param name="size">オブジェクトサイズ</param>
        /// <param name="lim">限界座標(上下左右)</param>
        /// <returns>補正済座標</returns>
        public static Vector3 FixPosInArea(Vector3 pos, Vector2 size, LBRT lim)
        {
            if (pos.x < lim.left   + size.x/2) { pos.x = lim.left   + size.x/2; }
            if (pos.y < lim.bottom + size.y/2) { pos.y = lim.bottom + size.y/2; }
            if (pos.x > lim.right  - size.x/2) { pos.x = lim.right  - size.x/2; }
            if (pos.y > lim.top    - size.y/2) { pos.y = lim.top    - size.y/2; }

            return pos;
        }

        /// <summary>
        /// ローカル座標をワールド座標に変換.
        /// </summary>
        /// <param name="obj">親オブジェクト</param>
        /// <param name="lPos">ローカル座標</param>
        /// <returns>ワールド座標</returns>
        public static Vector2 LPosToWPos(GameObject obj, Vector2 lPos)
        {
            var wPos = obj.transform.TransformPoint(lPos);
            return wPos;
        }

        /// <summary>
        /// ワールド座標をローカル座標に変換.
        /// </summary>
        /// <param name="obj">親オブジェクト</param>
        /// <param name="wPos">ワールド座標</param>
        /// <returns>ローカル座標</returns>
        public static Vector2 WPosToLPos(GameObject obj, Vector2 wPos)
        {
            var lPos = obj.transform.InverseTransformPoint(wPos);
            return lPos;
        }
    }
}