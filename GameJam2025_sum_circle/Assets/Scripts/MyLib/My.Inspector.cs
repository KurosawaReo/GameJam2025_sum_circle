/*
   - MyLib.Inspector -
   ver.2025/06/28

   ↓参考サイト.
   https://mu-777.hatenablog.com/entry/2022/09/04/113850

   コメントは推測の元書いたもの.
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// インスペクター用の追加機能.
/// </summary>
namespace MyLib.Inspector
{
    //不明.
    using GetCondFunc = Func<SerializedProperty, InspectorDisableAttribute, bool>;

    /// <summary>
    /// Attributeの方で保存した値を読み取るclass.
    /// ここでInspector表示の設定をする.
    /// </summary>
    [CustomPropertyDrawer(typeof(InspectorDisableAttribute))]
    internal sealed class ConditionalDisableDrawer : PropertyDrawer
    {
    //▼メンバ.
        //???
        private Dictionary<Type, GetCondFunc> DisableCondFuncMap = new Dictionary<Type, GetCondFunc>() {
            { typeof(bool),   (prop, attr) => { return attr.isDisable ? !prop.boolValue : prop.boolValue;} },
            { typeof(string), (prop, attr) => { return attr.isDisable ? prop.stringValue == attr.compStr   : prop.stringValue != attr.compStr;  } },
            { typeof(int),    (prop, attr) => { return attr.isDisable ? prop.intValue    == attr.compInt   : prop.intValue    != attr.compInt;  } },
            { typeof(float),  (prop, attr) => { return attr.isDisable ? prop.floatValue  <= attr.compFloat : prop.floatValue  >  attr.compFloat;} }
        };

    //▼public関数.

        /// <summary>
        /// ???
        /// </summary>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //???
            var attr = attribute as InspectorDisableAttribute;
            //???
            SerializedProperty prop;

            //パスを辿る, これがないとclassのメンバにアクセスできない.
            {
                string parentPath = property.propertyPath; //パス.
                int lastDot = parentPath.LastIndexOf('.'); //最後のドットを探す.
            
                //最後のドットがあれば.
                if (lastDot >= 0)
                {
                    string parent = parentPath.Substring(0, lastDot); //例:player
                    prop = property.serializedObject.FindProperty($"{parent}.{attr.varName}"); //親を含むパス.
                }
                else
                {
                    prop = property.serializedObject.FindProperty(attr.varName); //そのままのパス.
                }
                //値なしエラー.
                if (prop == null)
                {
                    Debug.LogError($"Not found '{attr.varName}' property");
                    EditorGUI.PropertyField(position, property, label, true); //???
                }
            }

            //???
            var isDisable = IsDisable(attr, prop);
            if (attr.isInvisible && isDisable)
            {
                return; //この先の処理をしない.
            }

            EditorGUI.BeginDisabledGroup(isDisable);                  //begin
            EditorGUI.PropertyField(position, property, label, true); //???
            EditorGUI.EndDisabledGroup();                             //end
        }

        /// <summary>
        /// Inspectorで入力欄を表示する高さを取得.
        /// </summary>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            //???
            var attr = attribute as InspectorDisableAttribute;
            //???
            SerializedProperty prop;

            //パスを辿る, これがないとclassのメンバにアクセスできない.
            {
                string parentPath = property.propertyPath; //パス.
                int lastDot = parentPath.LastIndexOf('.'); //最後のドットを探す.
                //最後のドットがあれば.
                if (lastDot >= 0)
                {
                    string parent = parentPath.Substring(0, lastDot); //例:player
                    prop = property.serializedObject.FindProperty($"{parent}.{attr.varName}"); //親を含むパス.
                }
                else
                {
                    prop = property.serializedObject.FindProperty(attr.varName); //そのままのパス.
                }
            }
            //適切な高さを返す.
            if (attr.isInvisible && IsDisable(attr, prop))
            {
                return -EditorGUIUtility.standardVerticalSpacing;   //非表示時の表示位置.
            }
            else
            {
                return EditorGUI.GetPropertyHeight(property, true); //通常の表示位置.
            }
        }

        /// <summary>
        /// 無効かどうか取得.
        /// </summary>
        private bool IsDisable(InspectorDisableAttribute attr, SerializedProperty prop)
        {
            //何かを入れる用の変数.
            GetCondFunc condFunc;
            
            //???の取得を試みる.
            bool ret = DisableCondFuncMap.TryGetValue(attr.varType, out condFunc);
            if (!ret) //失敗エラー.
            {
                Debug.LogError($"{attr.varType} type is not supported");
                return false;
            }
            //結果を返す.
            return condFunc(prop, attr);
        }
    }

    /// <summary>
    /// [InspectorDisable()]で実行されるclass.
    /// このクラスに値を保存しておく.
    /// </summary>
    public partial class InspectorDisableAttribute : PropertyAttribute
    {
    //▼メンバ.
        public readonly string varName;      //変数名.
        public readonly Type   varType;      //変数型.
        public readonly bool   isDisable;    //無効かどうか.
        public readonly bool   isInvisible;  //OFFの時に非表示にするか.

        public readonly string compStr;      //string型の比較値.
        public readonly int    compInt;      //int   型の比較値.
        public readonly float  compFloat;    //float 型の比較値.

    //▼コンストラクタ.
        /// <summary>
        /// メインのコンストラクタ(全ての型対応)
        /// </summary>
        private InspectorDisableAttribute(
            string _varName, Type _varType, bool _isDisable = false, bool _isInvisible = false
        ){
            varName     = _varName;
            varType     = _varType;
            isDisable   = _isDisable;
            isInvisible = _isInvisible;
        }
        /// <summary>
        /// bool型.
        /// </summary>
        public InspectorDisableAttribute(
            string _varName, bool _isNot = false, bool _isInvisible = false
        )
        : this(_varName, typeof(bool), _isNot, _isInvisible) //自身のコンストラクタへ.
        {}
        /// <summary>
        /// string型.
        /// </summary>
        public InspectorDisableAttribute(
            string _varName, string _str, bool _isNot = false, bool _isInvisible = false
        )
        : this(_varName, _str.GetType(), _isNot, _isInvisible) //自身のコンストラクタへ.
        {
            compStr = _str;
        }
        /// <summary>
        /// int型.
        /// </summary>
        public InspectorDisableAttribute(
            string _varName, int _int, bool _isNot = false, bool _isInvisible = false
        )
        : this(_varName, _int.GetType(), _isNot, _isInvisible) //自身のコンストラクタへ.
        {
            compInt = _int;
        }
        /// <summary>
        /// float型.
        /// </summary>
        public InspectorDisableAttribute(
            string _varName, float _float, bool _isGreater = true, bool _isInvisible = false
        )
        : this(_varName, _float.GetType(), _isGreater, _isInvisible) //自身のコンストラクタへ.
        {
            compFloat = _float;
        }
    }
}