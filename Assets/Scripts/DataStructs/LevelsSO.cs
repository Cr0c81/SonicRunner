using UnityEngine;
namespace DataStructs
{
	[CreateAssetMenu(fileName = "Levels", menuName = "Levels list", order = 0)]
	public class LevelsSO : ScriptableObject
	{
		public LevelData[] levels;
	}

}
