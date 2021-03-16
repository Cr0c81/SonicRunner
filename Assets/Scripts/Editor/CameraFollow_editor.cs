using System;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(CameraFollow))]
public class CameraFollow_editor : Editor
{
	private CameraFollow _holder;
	private void         OnEnable() => _holder = (CameraFollow)target;
	public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Reset"))
			_holder.ResetDirection();
		base.OnInspectorGUI();
	}
}
