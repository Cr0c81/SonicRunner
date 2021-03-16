using Internal;
using UnityEngine;
namespace Player
{
	public interface IPlayer : IRegistrable
	{
		void OnPlayerTrigger(GameObject target);
		int  Score { get; set; }
	}
}
