using System;
using System.Dynamic;
using UnityEngine;
[Serializable]
public class MinMaxFloat
{
	public static int GetId() => Guid.NewGuid().GetHashCode();
	public float Min
	{
		get => _Min;
		set => _Min = Mathf.Clamp(value, AbsoluteMin, AbsoluteMax);
	}

	[SerializeField]
	private float _Min;

	public float Max
	{
		get => _Max;
		set => _Max = Mathf.Clamp(value, AbsoluteMin, AbsoluteMax);
	}

	[SerializeField]
	private float _Max;

	public float AbsoluteMin
	{
		get => _AbsoluteMin;
		set {
			_AbsoluteMin = value;
			Min          = Min;
		}
	}

	public float AbsoluteMax
	{
		get => _AbsoluteMax;
		set {
			_AbsoluteMax = value;
			Max          = Max;
		}
	}

	[SerializeField] private float _AbsoluteMin;
	[SerializeField] private float _AbsoluteMax;

	[SerializeField] private int id;

	public MinMaxFloat(float min = 0f, float max = 1f, float absoluteMin = 0f, float absoluteMax = 1f)
	{
		id          = Guid.NewGuid().GetHashCode();
		_Min        = min;
		_Max        = max;
		AbsoluteMin = absoluteMin;
		AbsoluteMax = absoluteMax;
	}

	public MinMaxFloat()
	{
		id          = Guid.NewGuid().GetHashCode();
		_Min        = 0f;
		_Max        = 1f;
		AbsoluteMin = 0f;
		AbsoluteMax = 1f;
	}

	public Vector2 vector2 => new Vector2(_Min, _Max);

	public float Size => _Max - _Min;
}
