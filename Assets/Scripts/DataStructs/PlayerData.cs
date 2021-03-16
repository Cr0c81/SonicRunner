using System;
using Internal;
using UnityEngine;
namespace DataStructs
{
	[Serializable]
	public class PlayerData
	{
		public string                 name;
		public Sprite                 logo;
		public LazyObject<GameObject> prefab;
	}
}
