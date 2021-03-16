using System;
using UnityEngine;
namespace Internal
{
	public class ColliderProxy : MonoBehaviour
	{
		public  Action<GameObject> onTrigger;
		private void               OnTriggerEnter(Collider other) => onTrigger?.Invoke(other.gameObject);
	}
}
