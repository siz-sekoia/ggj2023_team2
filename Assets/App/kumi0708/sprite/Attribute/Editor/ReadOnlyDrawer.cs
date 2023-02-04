using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
//https://kazupon.org/unity-no-edit-param-view-inspector/
namespace PressSDK.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(_position, _property, _label);
            EditorGUI.EndDisabledGroup();
        }
    }
}
#endif