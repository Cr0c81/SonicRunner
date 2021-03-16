using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Internal
{
	[CustomPropertyDrawer(typeof(MinMaxFloat))]
	public class MinMaxFloat_drawer : PropertyDrawer
	{
		private static Dictionary<int, bool> isFold = new Dictionary<int, bool>();

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			int id = property.FindPropertyRelative("id").intValue;
			if (id == 0)
			{
				property.FindPropertyRelative("id").intValue = Guid.NewGuid().GetHashCode();
				id                                           = property.FindPropertyRelative("id").intValue;
			}
			if (!isFold.ContainsKey(id))
				isFold[id] = false;
			var height = EditorGUIUtility.singleLineHeight;
			position.height = height;
			var pMin  = property.FindPropertyRelative("_Min");
			var pMax  = property.FindPropertyRelative("_Max");
			var min   = pMin.floatValue;
			var max   = pMax.floatValue;
			var pAMin = property.FindPropertyRelative("_AbsoluteMin");
			var pAMax = property.FindPropertyRelative("_AbsoluteMax");
			EditorGUI.MinMaxSlider(position, label.text, ref min, ref max, pAMin.floatValue, pAMax.floatValue);

			position.y += position.height;
			var w = position.width;
			position.width = isFold[id] ? 45f : w;
			var foldText = isFold[id] ? "Data" : $"Data [{min:F3} : {max:F3}]";
			isFold[id]     = EditorGUI.Foldout(position, isFold[id], foldText, true);
			position.width = w;
			if (isFold[id])
			{
				position.width -= 50f;
				position.x     += 50f;
				position.width *= 0.5f;

				var lw = EditorGUIUtility.labelWidth;
				EditorGUIUtility.labelWidth = 60f;

				min        =  EditorGUI.FloatField(position, "Min value", min);
				position.x += position.width;
				max        =  EditorGUI.FloatField(position, "Max value", max);

				position.y       += position.height;
				position.x       -= position.width;
				pAMin.floatValue =  EditorGUI.FloatField(position, "Minimum", pAMin.floatValue);
				position.x       += position.width;
				pAMax.floatValue =  EditorGUI.FloatField(position, "Maximum", pAMax.floatValue);

				EditorGUIUtility.labelWidth = lw;
			}

			pMin.floatValue = Mathf.Clamp(min, pAMin.floatValue, pAMax.floatValue);
			pMax.floatValue = Mathf.Clamp(max, pAMin.floatValue, pAMax.floatValue);
		}


		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			int id = property.FindPropertyRelative("id").intValue;
			if (!isFold.ContainsKey(id))
				isFold[id] = false;
			return EditorGUIUtility.singleLineHeight * (isFold[id] ? 3f : 2f);
		}
	}
}
