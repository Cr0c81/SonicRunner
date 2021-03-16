using System.Collections;
using System.Collections.Generic;
using DataStructs;
using Dreamteck.Splines;
using UnityEngine;

public class LevelConstructor : MonoBehaviour
{
	[SerializeField] private LevelsSO         _levels;
	[SerializeField] private PlayersViewSO    _players;
	[SerializeField] private PlayerController _playerController;
	[SerializeField] private CameraFollow     _cameraFollow;
	
	private LevelData            _level;
	private LevelData.PropData[] props;

	private GameObject _levelGo;
	private GameObject _playerGo;
	private GameObject _propsLeftGo;
	private GameObject _propsRightGo;

#if UNITY_EDITOR
	[ContextMenu("Create level")]
	public void CreateLevel() => CreateLevel(0, 0);
	[ContextMenu("Destroy level")]
	public void DestroyLevel()
	{
		if (_levelGo) DestroyImmediate(_levelGo);
		if (_playerGo) DestroyImmediate(_playerGo);
		if (_propsLeftGo) DestroyImmediate(_propsLeftGo);
		if (_propsRightGo) DestroyImmediate(_propsRightGo);
	}

#endif
	public void CreateLevel(int levelIndex, int playerIndex)
	{
		//StartCoroutine(CreateLevelCor(levelIndex, playerIndex));
		CreateLevelCor(levelIndex, playerIndex);
	}

	private void CreateLevelCor(int levelIndex, int playerIndex)
	{
		_level   = _levels.levels[levelIndex];
		_levelGo = Instantiate(_level.trackPrefab.@object, Vector3.zero, Quaternion.identity);
		var levelConfig = _levelGo.GetComponent<LevelConfig>();
		props = _level.props;

		var sc = levelConfig.tracks[1];
		CreateProps(sc, levelConfig, true);
		CreateProps(sc, levelConfig, false);

		_playerGo            = Instantiate(_players.playerPrefabs[playerIndex].prefab.@object);
		_cameraFollow.target = _playerGo.transform;
		_playerController.StartGame(levelConfig.tracks, _playerGo.transform);
	}

	private void CreateProps(SplineComputer spline, LevelConfig levelConfig, bool leftSide)
	{
		var parent = new GameObject().transform;
		parent.name = $"Props left:({leftSide})";
		if (leftSide)
			_propsLeftGo = parent.gameObject;
		else
			_propsRightGo = parent.gameObject;
		var pos    = 0d;
		var length = spline.CalculateLength();
		while (pos <= length)
		{
			var splineResult = spline.Evaluate(pos / length);
			var prop         = _level.GetRandomProp();
			var offset       = splineResult.right * Random.Range(levelConfig.minWidth, levelConfig.maxWidth);
			var rotation     = prop.XZRotation    * Quaternion.LookRotation(splineResult.direction, splineResult.normal);
			var go           = Instantiate(prop.prefab.@object, splineResult.position + (offset * (leftSide ? -1f : 1f)), rotation, parent);
			go.transform.localScale =  prop.Scale;
			pos                     += prop.Space;
		}

	}
}
