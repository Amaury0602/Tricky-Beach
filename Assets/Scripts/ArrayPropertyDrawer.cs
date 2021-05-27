using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ArrayLayout))]
public class ArrayPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PrefixLabel(position, label);
        Rect newposition = position;
        newposition.y += 25f;
        SerializedProperty data = property.FindPropertyRelative("rows");
        for (int j = 0; j < 7 ; j++)
        {
            SerializedProperty row = data.GetArrayElementAtIndex(j).FindPropertyRelative("row");
            newposition.height = 25f;
            if (row.arraySize != 7)
            {
                row.arraySize = 7;
            }
            newposition.width = position.width / 7;
            for (int i = 0; i < 7 ; i++)
            {
                EditorGUI.PropertyField(newposition, row.GetArrayElementAtIndex(i), GUIContent.none);
                newposition.x += newposition.width;
            }

            newposition.x = position.x;
            newposition.y += 25f;

        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 25f * 8;
    }
}
#endif
