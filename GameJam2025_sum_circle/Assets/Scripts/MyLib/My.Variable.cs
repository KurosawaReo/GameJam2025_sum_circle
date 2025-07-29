/*
   - MyLib.Variable -
   ver.2025/06/28
*/
using UnityEngine;
using System;

/// <summary>
/// 変数用の追加機能.
/// </summary>
namespace MyLib.Variable
{
    /// <summary>
    /// 範囲ありint型変数.
    /// </summary>
    [Serializable] public struct IntR //int range.
    {
        [SerializeField] private int max;  //最大値.
        [SerializeField] private int min;  //最小値.
        [SerializeField] private int init; //初期化値.
        private int now; //現在値.

        //set, get.
        public int Now //現在値.
        {
            get => now;
            set {
                now = value;
                if (now > max) { now = max; } //最大に設定.
                if (now < min) { now = min; } //最小に設定.
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IntR(int _max, int _min, int _init)
        {
            max  = _max;
            min  = _min;
            init = _init;
            now  = _init; //初期値に設定.
        }
        /// <summary>
        /// リセット.
        /// </summary>
        public void Reset()
        {
            now = init;
        }
    }

    /// <summary>
    /// 範囲ありfloat型変数.
    /// </summary>
    [Serializable] public struct FloatR //float range.
    {
        [SerializeField] private float max;  //最大値.
        [SerializeField] private float min;  //最小値.
        [SerializeField] private float init; //初期化値.
        private float now; //現在値.

        //set, get.
        public float Now //現在値.
        {
            get => now;
            set {
                now = value;
                if (now > max) { now = max; } //最大に設定.
                if (now < min) { now = min; } //最小に設定.
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FloatR(float _max, float _min, float _init)
        {
            max  = _max;
            min  = _min;
            init = _init;
            now  = _init; //初期値に設定.
        }
        /// <summary>
        /// リセット.
        /// </summary>
        public void Reset()
        {
            now = init;
        }
    }
}