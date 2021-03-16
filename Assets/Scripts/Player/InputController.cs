using System;
using System.Collections;
using System.Collections.Generic;
using Internal;
using UnityEngine;

public class InputController : MonoBehaviour
{
	public enum InputType
	{
		None  = 0,
		Left  = 1,
		Right = 2,
		Jump  = 3,
		Roll  = 4
	}

	public Action<InputType> onInput;

	private                  Vector2? _touchStart;
	private                  Vector2  _screenSize;
	private                  bool     _process = false;
	[SerializeField] private float    _sens    = 0.01f;
	private void Update()
	{
#if UNITY_ANDROID
		var pointer = GetPointerDroid();
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
		var pointer = GetPointerPC();
#endif
		if (!_process && pointer != null)
		{
			_touchStart = pointer.Value;
			_process    = true;
		}
		if (pointer == null)
		{
			_process = false;
		}

		if (_process && _touchStart != null)
		{
			var delta = (pointer.Value - _touchStart.Value) / _screenSize; // normalize to screen size
			if (delta.sqrMagnitude >= _sens)
			{
				if (Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))
				{
					// horizontal
					onInput?.Invoke(delta.x < 0 ? InputType.Left : InputType.Right);
					_touchStart = null;
				}
				else
				{
					//vertical
					onInput?.Invoke(delta.y > 0 ? InputType.Jump : InputType.Roll);
					_touchStart = null;
				}
			}
		}
	}

	private Vector2? GetPointerDroid()
	{
		var touches = Input.touches;
		if (touches.Length == 0) return null;
		var touch = touches[0];
		return touch.position;
	}
	private Vector2? GetPointerPC()
	{
		if (Input.GetMouseButton(0)) return Input.mousePosition;
		return null;
	}
	private void Awake()
	{
		_screenSize = new Vector2(Mathf.Min(Screen.width, Screen.height), Mathf.Min(Screen.width, Screen.height));
		Locator.Register(typeof(InputController), this);
	}
	private void OnDestroy() => Locator.Unregister(typeof(InputController));
}
