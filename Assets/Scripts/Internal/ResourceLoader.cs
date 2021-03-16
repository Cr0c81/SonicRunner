using System.Collections.Generic;
using UnityEngine;
namespace Internal
{
	public static class ResourceLoader
	{
		private static readonly Dictionary<string, Object> Cache = new Dictionary<string, Object>();
		
		public static T LoadObject<T>(string path) where T : Object
		{
			if (string.IsNullOrEmpty(path)) return null;
			if (Cache.ContainsKey(path) && (Cache[path] is T)) return (T)Cache[path];
			var obj = Resources.Load<T>(path);
			Cache[path] = obj;
			return obj;
		}

		public static void Clean()
		{
			Cache.Clear();
			Resources.UnloadUnusedAssets();
		}
	}
}
