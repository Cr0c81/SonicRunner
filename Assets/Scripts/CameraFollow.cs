using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour
{
	public enum CameraDirection
	{
		Forward = 0,
		LookAt = 1
	}
	public                   Transform       target;
	[SerializeField] private Vector3         _offset;
	[SerializeField] private CameraDirection _camDirection    = CameraDirection.LookAt;
	[SerializeField] private float           _directionSmooth = 1f;
	private void LateUpdate()
	{
		if (target)
		{
			switch (_camDirection)
			{

				case CameraDirection.Forward:
					transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, _directionSmooth);
					break;
				case CameraDirection.LookAt:
					transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), _directionSmooth);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			transform.position = target.position + transform.rotation * _offset;
		}
	}

	public void ResetDirection()
	{
		switch (_camDirection)
		{

			case CameraDirection.Forward:
				transform.rotation = target.rotation;
				break;
			case CameraDirection.LookAt:
				transform.rotation = Quaternion.LookRotation(target.position - transform.position);
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
		transform.position = target.position + transform.rotation * _offset;
	}
}
