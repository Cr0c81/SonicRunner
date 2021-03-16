using System;
using UnityEngine;
namespace Bonus
{
	public class BonusCoin : BonusBase
	{
		[Header("Coins")]
		public int value;
		private void OnTriggerEnter(Collider other) => Destroy(gameObject);
	}
}
