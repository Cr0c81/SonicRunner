using System;
using UnityEngine;
namespace Bonus
{
	public class BonusSpeed : BonusBase
	{
		[Header("Duration")]
		public float value;
		public bool consumable = true;
		private void OnTriggerEnter(Collider other)
		{
			if (consumable) Destroy(gameObject);
		}
	}
}
