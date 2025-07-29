/*
   - MyLib.Object -
   ver.2025/07/29
*/
using UnityEngine;

using MyLib.Position;
using MyLib.Variable;
using MyLib.Inspector;

/// <summary>
/// オブジェクト管理用の追加機能.
/// </summary>
namespace MyLib.Object
{
    /// <summary>
    /// コンポーネント集.
    /// </summary>
    public class ObjComponents
    {
        public SpriteRenderer sr;
        public Rigidbody2D    rb2d;
        public Animator       animr;
    }

    /// <summary>
    /// オブジェクトデータ(2D用)
    /// このMyObjectを継承することでMonoBehaviourの機能も使える.
    /// </summary>
    public class MyObject : MonoBehaviour
    {
    //▼private変数.
        private ObjComponents cmp = new ObjComponents();

        private Vector2 facing;   //向いてる方向.
        private bool    isActive; //有効かどうか.
        private bool    isFlip;   //反転するかどうか.

    //▼private変数.[入力可]
        [Header("- MyObject -")] 
        [Space(4)]
        [SerializeField] 
            private bool    isAutoInit = true;                      //自動で値を初期化するか.
        [InspectorDisable("isAutoInit"), SerializeField] 
            private Vector2 initPos;                                //初期座標.
        [InspectorDisable("isAutoInit"), SerializeField] 
            private Vector2 initFacing;                             //初期向き.
        [InspectorDisable("isAutoInit"), SerializeField] 
            private bool    initActive = true;                      //有効かどうか.
        [InspectorDisable("isAutoInit"), SerializeField] 
            private bool    initFlip   = false;                     //反転するかどうか.

        [SerializeField] private Vector2 size = new Vector2(1, 1);  //当たり判定サイズ.
        [SerializeField] private IntR    hp;                        //体力.

    //▼public.
        //set, get.
        public Vector2 Pos { 
            get => transform.position;
            protected set => transform.position = value;
        }
        public float PosX {
            get => transform.position.x;
            protected set => transform.position = new Vector2(value, PosY);
        }
        public float PosY {
            get => transform.position.y;
            protected set => transform.position = new Vector2(PosX, value);
        }
        public Vector2 Size {
            get => size;
        }
        public Vector2 Facing {
            get => facing;
        }
        public Vector2 Vel {
            get => cmp.rb2d.velocity;
            protected set => cmp.rb2d.velocity = value;
        }
        public float VelX {
            get => cmp.rb2d.velocity.x;
            protected set => cmp.rb2d.velocity = new Vector2(value, VelY);
        }
        public float VelY {
            get => cmp.rb2d.velocity.y;
            protected set => cmp.rb2d.velocity = new Vector2(VelX, value);
        }
        public float Gravity {
            get => cmp.rb2d.gravityScale;
            protected set => cmp.rb2d.gravityScale = value;
        }
        public int Hp {
            get => hp.Now;
            protected set => hp.Now = value;
        }
        public Sprite Sprite
        {
            get => cmp.sr.sprite;
            set => cmp.sr.sprite = value;
        }
        public Color Color {
            get => cmp.sr.color;
            protected set => cmp.sr.color = value;
        }
        public bool IsActive {
            get => isActive; 
            protected set {
                isActive = value;
                gameObject.SetActive(value); //設定.
            }
        }
        public bool IsFlip {
            get => isFlip;
            protected set {
                isFlip = value;
                cmp.sr.flipX = value; //設定.
            }
        }

        /// <summary>
        /// 初期化処理.
        /// </summary>
        public void InitMyObj()
        {
            //コンポーネント取得.
            cmp.sr    = GetComponent<SpriteRenderer>();
            cmp.rb2d  = GetComponent<Rigidbody2D>();
            cmp.animr = GetComponent<Animator>();
            //サイズ取得.
            size = new Vector2(cmp.sr.bounds.size.x * size.x, cmp.sr.bounds.size.x * size.y);
            //自動初期化モードなら.
            if (isAutoInit)
            {
                initPos    = transform.position;     //初期座標登録.
                initFacing = new Vector2(1, 0);      //右.
                initActive = gameObject.activeSelf;  //アクティブ状態取得.
                initFlip   = cmp.sr.flipX;           //反転状態取得.
            }
            else
            {
                ResetMyObj(); //リセット処理.
            }
        }

        /// <summary>
        /// 変数のリセット処理.
        /// </summary>
        public void ResetMyObj()
        {
            Pos      = initPos;
            facing   = initFacing;
            IsActive = initActive;
            IsFlip   = initFlip;
            hp.Reset();
        }

        /// <summary>
        /// 移動処理.
        /// </summary>
        public void MoveMyObj(Vector2 vec, float speed)
        {
            //移動.
            Pos += vec * speed * Time.deltaTime;
            //方向の保存.
            facing = vec;
        }
        /// <summary>
        /// 移動処理.
        /// </summary>
        /// <param name="lim">限界座標</param>
        public void MoveMyObj(Vector2 vec, float speed, LBRT lim)
        {
            //移動.
            Pos += vec * speed * Time.deltaTime;
            Pos = PS_Func.FixPosInArea(Pos, size, lim); //移動限界.
            //方向の保存.
            facing = vec;
        }

        /// <summary>
        /// 移動速度を与える.
        /// </summary>
        public void AddForceMyObj(Vector2 vec, float speed)
        {
            cmp.rb2d.AddForce(vec * speed);
        }
        /// <summary>
        /// 移動速度を与える.
        /// </summary>
        /// <param name="mode">力を与えるモード</param>
        public void AddForceMyObj(Vector2 vec, float speed, ForceMode2D mode)
        {
            cmp.rb2d.AddForce(vec * speed, mode);
        }

        /// <summary>
        /// Animatorのパラメーターをセット(Trigger)
        /// </summary>
        public void SetAnimMyObj(string name)
        {
            cmp.animr.SetTrigger(name);
        }
        /// <summary>
        /// Animatorのパラメーターをセット(Bool)
        /// </summary>
        public void SetAnimMyObj(string name, bool value)
        {
            cmp.animr.SetBool(name, value);
        }
        /// <summary>
        /// Animatorのパラメーターをセット(Int)
        /// </summary>
        public void SetAnimMyObj(string name, int value)
        {
            cmp.animr.SetInteger(name, value);
        }
        /// <summary>
        /// Animatorのパラメーターをセット(Float)
        /// </summary>
        public void SetAnimMyObj(string name, float value)
        {
            cmp.animr.SetFloat(name, value);
        }
    }
}