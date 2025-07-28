/*
   - MyLib.InputST -
   ver.2025/07/29
*/
using UnityEngine;

/// <summary>
/// 座標管理をする用の追加機能.
/// </summary>
namespace MyLib.InputST
{
    /// <summary>
    /// マウスボタン判定用.
    /// </summary>
    public enum MouseCode
    { 
        Left,       //左クリック.
        Right,      //右クリック.
        Middle,     //ホイール.
        SideFront,  //横の手前.
        SideBack,   //横の奥.
    }

    /// <summary>
    /// Input関数.
    /// </summary>
    public static partial class IN_Func
    {
        /// <summary>
        /// マウスクリック判定.
        /// </summary>
        public static bool IsPushMouse(MouseCode id)
        {
            return Input.GetMouseButton((int)id);
        }
        /// <summary>
        /// マウスクリック判定.
        /// </summary>
        public static bool IsPushMouseDown(MouseCode id)
        {
            return Input.GetMouseButtonDown((int)id);
        }
        /// <summary>
        /// マウスクリック判定.
        /// </summary>
        public static bool IsPushMouseUp(MouseCode id)
        {
            return Input.GetMouseButtonUp((int)id);
        }

        /// <summary>
        /// マウス座標取得.
        /// </summary>
        public static Vector2 GetMousePos()
        {
            Vector2 mPos = Input.mousePosition;
            Vector2 wPos = Camera.main.ScreenToWorldPoint(mPos);

            return wPos;
        }
        /// <summary>
        /// 上下左右の操作取得.
        /// </summary>
        public static Vector2 GetMove4dir()
        {
            Vector2 input = new Vector2(
                Input.GetAxisRaw("Horizontal"), //水平方向入力.
                Input.GetAxisRaw("Vertical")    //垂直方向入力.
            );
            return input;
        }
    }
}