/*
   - MyLib.InputST -
   ver.2025/07/29
*/
using UnityEngine;

/// <summary>
/// ���W�Ǘ�������p�̒ǉ��@�\.
/// </summary>
namespace MyLib.InputST
{
    /// <summary>
    /// �}�E�X�{�^������p.
    /// </summary>
    public enum MouseCode
    { 
        Left,       //���N���b�N.
        Right,      //�E�N���b�N.
        Middle,     //�z�C�[��.
        SideFront,  //���̎�O.
        SideBack,   //���̉�.
    }

    /// <summary>
    /// Input�֐�.
    /// </summary>
    public static partial class IN_Func
    {
        /// <summary>
        /// �}�E�X�N���b�N����.
        /// </summary>
        public static bool IsPushMouse(MouseCode id)
        {
            return Input.GetMouseButton((int)id);
        }
        /// <summary>
        /// �}�E�X�N���b�N����.
        /// </summary>
        public static bool IsPushMouseDown(MouseCode id)
        {
            return Input.GetMouseButtonDown((int)id);
        }
        /// <summary>
        /// �}�E�X�N���b�N����.
        /// </summary>
        public static bool IsPushMouseUp(MouseCode id)
        {
            return Input.GetMouseButtonUp((int)id);
        }

        /// <summary>
        /// �}�E�X���W�擾.
        /// </summary>
        public static Vector2 GetMousePos()
        {
            Vector2 mPos = Input.mousePosition;
            Vector2 wPos = Camera.main.ScreenToWorldPoint(mPos);

            return wPos;
        }
        /// <summary>
        /// �㉺���E�̑���擾.
        /// </summary>
        public static Vector2 GetMove4dir()
        {
            Vector2 input = new Vector2(
                Input.GetAxisRaw("Horizontal"), //������������.
                Input.GetAxisRaw("Vertical")    //������������.
            );
            return input;
        }
    }
}