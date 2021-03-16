using System;
using System.Linq;
using Internal;
using UnityEngine;
using Random = UnityEngine.Random;
namespace DataStructs
{
	[Serializable]
	public class LevelData
	{
		[Serializable]
		public class PropData
		{
			public                   LazyObject<GameObject> prefab;
			[SerializeField] private MinMaxFloat            space;
			[SerializeField] private MinMaxFloat            sсale;
			[SerializeField] private bool                   allowRotation = true;
			[SerializeField] private MinMaxFloat            xzRotation    = new MinMaxFloat(0f, 360f, 0f, 360f);
			public                   float                  weight        = 1f;

			public float   Space => Random.Range(space.Min, space.Max);
			public Vector3 Scale => Vector3.one * Random.Range(sсale.Min, sсale.Max);

			public Quaternion XZRotation => allowRotation ? Quaternion.Euler(0f, Random.Range(xzRotation.Min, xzRotation.Max), 0f) : Quaternion.identity;
		}
		public  string                 name;
		public  Sprite                 logo;
		public  LazyObject<GameObject> trackPrefab;
		public  PropData[]             props;
		public  LazyObject<AudioClip>  bgMusic;
		private float                  weights = -1;
		public PropData GetRandomProp()
		{
			if (weights < 0f)
				weights = props.Select(t => t.weight).Sum();
			var weight = Random.Range(0f, weights);
			var w      = weight;
			foreach (var prop in props)
			{
				if (prop.weight >= weight)
					return prop;
				else
					weight -= prop.weight;
			}
			throw new Exception($"Error random prop! ({w} of {weights})");
		}
	}
}
