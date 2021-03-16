using System;
using UnityEngine;
namespace Internal
{
	[Serializable]
	public class LazyObject<T> where T : UnityEngine.Object
	{
		[SerializeField] private string path;
		public                   T      @object => ResourceLoader.LoadObject<T>(path);
	}
}
