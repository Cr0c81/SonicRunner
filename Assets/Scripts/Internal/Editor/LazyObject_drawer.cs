using Internal;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(LazyObject<GameObject>))]
public class LazyObject_drawer_go : LazyObject_drawerbase
{
	protected override (Object oldValue, Object newValue) DrawObject(string path, Rect position) => base.DrawObjectGeneric<GameObject>(path, position);
}
[CustomPropertyDrawer(typeof(LazyObject<AudioClip>))]
public class LazyObject_drawer_audioclip : LazyObject_drawerbase
{
	protected override (Object oldValue, Object newValue) DrawObject(string path, Rect position) => base.DrawObjectGeneric<AudioClip>(path, position);
}

public class LazyObject_drawerbase : PropertyDrawer
{
	protected virtual (Object oldValue, Object newValue) DrawObject(string path, Rect position) => (null, null);
	protected (Object oldValue, Object newValue) DrawObjectGeneric<T>(string path, Rect position) where T : Object
	{
		var oldV = ResourceLoader.LoadObject<T>(path);
		var newV = EditorGUI.ObjectField(position, "", oldV, typeof(T));
		return (oldV, newV);
	}
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		position.height *= 0.5f;
		GUI.enabled     =  false;
		EditorGUI.PropertyField(position, property.FindPropertyRelative("path"), label);
		GUI.enabled =  true;
		position.y  += position.height;
		var    path = property.FindPropertyRelative("path").stringValue;
		string aPath;
		var (oldValue, newValue) = DrawObject(path, position);
		if (newValue == null)
			aPath = "";
		else
		{
			aPath = AssetDatabase.GetAssetPath(newValue).Replace("Assets/Resources/", "");
			var extPos = aPath.LastIndexOf('.');
			if (extPos > 0)
				aPath = aPath.Substring(0, extPos);
		}
		if (newValue != oldValue && aPath != path)
			property.FindPropertyRelative("path").stringValue = aPath;
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUIUtility.singleLineHeight * 2f;
}
