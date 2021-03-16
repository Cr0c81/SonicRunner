using UnityEngine;
namespace DataStructs
{
	[CreateAssetMenu(fileName = "Players", menuName = "Players view", order = 0)]
	public class PlayersViewSO : ScriptableObject
	{
		public PlayerData[] playerPrefabs;
	}

}