using System;
using UnityEngine;
namespace Bonus
{
	[ExecuteInEditMode]
	public class BonusBase : MonoBehaviour
	{
		public enum BonusType
		{
			Coin = 0,
			Slow = 1,
			Fast = 2,
			Finish = 3
		}
		[Header("Bonus type")]
		public BonusType bonusType = BonusType.Coin;
		[Header("Duration")]
		public float duration;
		[Header("Value float")]
		public float valueFloat;
		[Header("Value int")]
		public int valueInt;
		public bool consumable = true;
		internal void OnTriggerEnter(Collider other)
		{
			if (consumable) Destroy(gameObject);
		}


#if UNITY_EDITOR
		public enum Direction
		{
			X = 0, Y = 1, Z = 2
		}
		public  bool      linkToTracks = true;
		public  Direction direction;
		private void      Update() => OnValidate();
		private void OnValidate()
		{
			var levelConfig = GetComponentInParent<LevelConfig>();
			if (levelConfig == null || !linkToTracks) return;
			var pos      = new double[levelConfig.tracks.Length];
			var trackNum = -1;
			for (var i = 0; i < pos.Length; i++)
			{
				pos[i] = levelConfig.tracks[i].Project(transform.position);
			}
			var dist = float.MaxValue;
			for (var i = 0; i < pos.Length; i++)
			{
				var res = levelConfig.tracks[i].EvaluatePosition(pos[i]);
				var d   = Vector3.Distance(res, transform.position);
				if (d < dist)
				{
					dist     = d;
					trackNum = i;
				}
			}
			transform.position = levelConfig.tracks[trackNum].EvaluatePosition(pos[trackNum]);
			switch (direction)
			{

				case Direction.X:
					CheckRotationX();
					break;
				case Direction.Y:
					CheckRotationY();
					break;
				case Direction.Z:
					CheckRotationZ();
					break;
			}
		}
		[ContextMenu("Check rotation X")]
		public void CheckRotationX()
		{
			var levelConfig = GetComponentInParent<LevelConfig>();
			if (levelConfig == null || !linkToTracks) return;
			var pos      = new double[levelConfig.tracks.Length];
			var trackNum = -1;
			for (var i = 0; i < pos.Length; i++)
			{
				pos[i] = levelConfig.tracks[i].Project(transform.position);
			}
			var dist = float.MaxValue;
			for (var i = 0; i < pos.Length; i++)
			{
				var res = levelConfig.tracks[i].EvaluatePosition(pos[i]);
				var d   = Vector3.Distance(res, transform.position);
				if (d < dist)
				{
					dist     = d;
					trackNum = i;
				}
			}
			var r = levelConfig.tracks[trackNum].Evaluate(pos[trackNum]);
			transform.rotation = Quaternion.LookRotation(r.right, r.normal);
		}
		[ContextMenu("Check rotation Y")]
		public void CheckRotationY()
		{
			var levelConfig = GetComponentInParent<LevelConfig>();
			if (levelConfig == null || !linkToTracks) return;
			var pos      = new double[levelConfig.tracks.Length];
			var trackNum = -1;
			for (var i = 0; i < pos.Length; i++)
			{
				pos[i] = levelConfig.tracks[i].Project(transform.position);
			}
			var dist = float.MaxValue;
			for (var i = 0; i < pos.Length; i++)
			{
				var res = levelConfig.tracks[i].EvaluatePosition(pos[i]);
				var d   = Vector3.Distance(res, transform.position);
				if (d < dist)
				{
					dist     = d;
					trackNum = i;
				}
			}
			var r = levelConfig.tracks[trackNum].Evaluate(pos[trackNum]);
			transform.rotation = Quaternion.LookRotation(r.normal, r.direction);
		}
		[ContextMenu("Check rotation Z")]
		public void CheckRotationZ()
		{
			var levelConfig = GetComponentInParent<LevelConfig>();
			if (levelConfig == null || !linkToTracks) return;
			var pos      = new double[levelConfig.tracks.Length];
			var trackNum = -1;
			for (var i = 0; i < pos.Length; i++)
			{
				pos[i] = levelConfig.tracks[i].Project(transform.position);
			}
			var dist = float.MaxValue;
			for (var i = 0; i < pos.Length; i++)
			{
				var res = levelConfig.tracks[i].EvaluatePosition(pos[i]);
				var d   = Vector3.Distance(res, transform.position);
				if (d < dist)
				{
					dist     = d;
					trackNum = i;
				}
			}
			var r = levelConfig.tracks[trackNum].Evaluate(pos[trackNum]);
			transform.rotation = Quaternion.LookRotation(r.direction, r.normal);
		}
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(transform.position, 1f);
		}
#endif
	}
}
