using System;
using System.Collections;
using System.Collections.Generic;
using Bonus;
using Dreamteck.Splines;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public                   bool      movePlayer;
	[SerializeField] private float     speed     = 5f;
	[SerializeField] private float     slowSpeed = 5f;
	[SerializeField] private float     walkSpeed = 10f;
	[SerializeField] private float     runSpeed  = 20f;
	private                  Transform _playerTransform;

	[SerializeField]              private SplineComputer[] _tracks;
	[SerializeField]              private float            _track            = 1;
	[Range(0, 2)][SerializeField] private int              _trackNeeded      = 1;
	[SerializeField]              private float            _trackChangeSpeed = 1f;
	public                                int              score;

	private double[] _projects;
	private bool     _isReady = false;

	public void StartGame(SplineComputer[] tracks, Transform player)
	{
		_tracks                   = tracks;
		_projects                 = new double[_tracks.Length];
		_playerTransform          = player;
		_playerTransform.position = _tracks[(int)_track].Evaluate(0).position;
		_playerTransform.rotation = _tracks[(int)_track].Evaluate(0).rotation;

		player.GetComponentInChildren<ColliderProxy>().onTrigger += OnPlayerTrigger;

		speed      = walkSpeed;
		score      = 0;
		_isReady   = true;
		movePlayer = true;
	}

	private void Update()
	{
		if (!_isReady) return;
		_track = Mathf.MoveTowards(_track, _trackNeeded, _trackChangeSpeed * Time.deltaTime);
		for (var i = 0; i < _tracks.Length; i++)
			_projects[i] = _tracks[i].Project(_playerTransform.position);
		var track1 = Mathf.FloorToInt(_track);
		var track2 = track1 + 1;
		if (track2 >= _tracks.Length) track2--;
		var t1  = _tracks[track1].Evaluate(_projects[track1]);
		var t2  = _tracks[track2].Evaluate(_projects[track2]);
		var pos = Vector3.Lerp(t1.position, t2.position, _track     % 1);
		var rot = Quaternion.Slerp(t1.rotation, t2.rotation, _track % 1);
		_playerTransform.position = pos;
		_playerTransform.rotation = rot;

		if (movePlayer)
		{
			_playerTransform.position += _playerTransform.forward * (speed * Time.deltaTime);
		}
	}

	private void OnPlayerTrigger(GameObject target)
	{
		if (!target.TryGetComponent<BonusBase>(out var bonus)) return;
		switch (bonus)
		{
			case BonusSpeed bonusSpeed:
				speed = runSpeed;
				this.InvokeDelegate(() => { speed = walkSpeed; }, bonusSpeed.value);
				break;
			case BonusSlow bonusSlow:
				speed = slowSpeed;
				this.InvokeDelegate(() => { speed = walkSpeed; }, bonusSlow.value);
				break;
			case BonusCoin bonusCoin:
				score += bonusCoin.value;
				break;
			case BonusFinish bonusFinish:
				FinishGame();
				break;
		}
	}

	private void FinishGame()
	{
		_isReady = false;
		StartGame(_tracks, _playerTransform);
	}
}
