using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Bonus;
using Dreamteck.Splines;
using Internal;
using UI;
using UnityEngine;
namespace Player
{
	public class GameplayController : MonoBehaviour, IPlayer
	{
		public bool _movePlayer;

		[Header("Move speeds")]
		[SerializeField] private float _speedBase = 10f;
		[SerializeField] private float _speedMult       = 1f;
		[SerializeField] private float _speedMultNeeded = 1f;
		[SerializeField] private float _speedMultSpeed  = 1f;
		[SerializeField] private float _speedMultTimer  = 0f;
		[SerializeField] private float _jumpHeight      = 4f;

		private                  Transform        _playerTransform;
		private                  Vector3          _playerOffset = new Vector3(0f, 0f, 0f);
		private                  Animator         _playerAnimator;
		private                  SplineComputer[] _tracks;
		private                  float            _track            = 1;
		private                  int              _trackNeeded      = 1;
		[SerializeField] private float            _trackChangeSpeed = 1f;

		private IScoreWidget _scoreWidget;
		private int          _score;
		private double[]     _projects;
		private bool         _isReady = false;
		private int          _startTrack;

		private                 float _animLegJumpB;
		private                 float _animLegRollkickA;
		private static readonly int   Jump      = Animator.StringToHash("jump");
		private static readonly int   Roll      = Animator.StringToHash("roll");
		private static readonly int   MoveSpeed = Animator.StringToHash("moveSpeed");
		private static readonly int   JumpSpeed = Animator.StringToHash("jumpSpeed");
		private static readonly int   RollSpeed = Animator.StringToHash("rollSpeed");

		private void Awake()     => Register();
		private void OnDestroy() => Unregister();
		private void Update()
		{
			if (!_isReady) return;
			_track = Mathf.MoveTowards(_track, _trackNeeded, _trackChangeSpeed * Time.deltaTime);
			for (var i = 0; i < _tracks.Length; i++)
				_projects[i] = _tracks[i].Project(_playerTransform.position - _playerOffset);
			var track1 = Mathf.FloorToInt(_track);
			var track2 = track1 + 1;
			if (track2 >= _tracks.Length) track2--;
			var t1  = _tracks[track1].Evaluate(_projects[track1]);
			var t2  = _tracks[track2].Evaluate(_projects[track2]);
			var pos = Vector3.Lerp(t1.position, t2.position, _track     % 1);
			var rot = Quaternion.Slerp(t1.rotation, t2.rotation, _track % 1);
			_playerTransform.position = pos + _playerOffset;
			_playerTransform.rotation = rot;

			if (_movePlayer)
			{
				_playerTransform.position += _playerTransform.forward * (_speedBase * _speedMult * Time.deltaTime);
				_speedMult                =  Mathf.MoveTowards(_speedMult, _speedMultNeeded, Time.deltaTime * _speedMultSpeed);
				if (_speedMultTimer > 0f)
				{
					_speedMultTimer -= Time.deltaTime;
					if (_speedMultTimer <= 0f) _speedMultNeeded = 1f;
				}
			}
			_playerAnimator.SetFloat(MoveSpeed, _speedBase * _speedMult / 10f);
		}

#region Interfaces
		public void Register()   => Locator.Register(typeof(IPlayer), this);
		public void Unregister() => Locator.Unregister(typeof(IPlayer));

		public void OnPlayerTrigger(GameObject target)
		{
			var bonuses = target.GetComponentsInParent<BonusBase>();
			foreach (var bonus in bonuses)
			{
				switch (bonus.bonusType)
				{
					case BonusBase.BonusType.Slow:
						Debug.Log($"[GameplayController] Trigger with ({target.gameObject.name}) :: Slow");
						_speedMultNeeded = bonus.valueFloat;
						_speedMultTimer  = bonus.duration;
						break;
					case BonusBase.BonusType.Fast:
						Debug.Log($"[GameplayController] Trigger with ({target.gameObject.name}) :: Speed");
						_speedMultNeeded = bonus.valueFloat;
						_speedMultTimer  = bonus.duration;
						break;
					case BonusBase.BonusType.Coin:
						Debug.Log($"[GameplayController] Trigger with ({target.gameObject.name}) :: Coin");
						_score += bonus.valueInt;
						_scoreWidget.SetScore(_score);
						break;
					case BonusBase.BonusType.Finish:
						Debug.Log($"[GameplayController] Trigger with ({target.gameObject.name}) :: Finish");
						FinishGame();
						break;
				}
			}
		}

		public int Score
		{
			get => _score;
			set => _score = value;
		}
  #endregion

		public void StartGame(SplineComputer[] tracks, Transform player, int track)
		{
			var input = Locator.GetObject<InputController>();
			input.onInput    += OnInput;
			_tracks          =  tracks;
			_startTrack      =  track;
			_trackNeeded     =  _startTrack;
			_track           =  _startTrack;
			_projects        =  new double[_tracks.Length];
			_playerTransform =  player;
			_playerAnimator  =  _playerTransform.GetComponentInChildren<Animator>();
			GetAnimatorClips();
			_playerTransform.position = _tracks[(int)_track].Evaluate(0).position;
			_playerTransform.rotation = _tracks[(int)_track].Evaluate(0).rotation;

			player.GetComponentInChildren<ColliderProxy>().onTrigger += OnPlayerTrigger;

			_speedMult       = 1f;
			_speedMultNeeded = 1f;
			_speedMultTimer  = 0f;
			_score           = 0;
			_scoreWidget     = Locator.GetObject<IScoreWidget>();
			_scoreWidget.SetScore(_score);
			_scoreWidget.Show();
			_isReady    = true;
			_movePlayer = true;
		}

		private void GetAnimatorClips()
		{
			var aoc   = (AnimatorOverrideController)_playerAnimator.runtimeAnimatorController;
			var clips = new List<KeyValuePair<AnimationClip, AnimationClip>>();
			aoc.GetOverrides(clips);
			_animLegJumpB     = clips.Select(t => t.Key).FirstOrDefault(t => t.name.Equals("Anim_Leg_JumpB")).length     / _playerAnimator.GetFloat(JumpSpeed);
			_animLegRollkickA = clips.Select(t => t.Key).FirstOrDefault(t => t.name.Equals("Anim_Leg_RollkickA")).length / _playerAnimator.GetFloat(RollSpeed);
		}
		private void FinishGame()
		{
			var input = Locator.GetObject<InputController>();
			input.onInput -= OnInput;
			_isReady      =  false;
			Locator.GetObject<IScoreWidget>().Hide(() => {
				Locator.GetObject<MainMenuWidget.IMainMenu>().Show(() => {
					Locator.GetObject<LevelConstructor>().DestroyLevel();
				});
			});
		}
		private void OnInput(InputController.InputType inputType)
		{
			Debug.Log($"[GameplayController] Input: ({inputType})");
			switch (inputType)
			{
				case InputController.InputType.None:
					break;
				case InputController.InputType.Left:
					_trackNeeded = Mathf.Clamp(--_trackNeeded, 0, _tracks.Length - 1);
					break;
				case InputController.InputType.Right:
					_trackNeeded = Mathf.Clamp(++_trackNeeded, 0, _tracks.Length - 1);
					break;
				case InputController.InputType.Jump:
					_playerAnimator.SetTrigger(Jump);
					this.InvokeDelegate((value) => { _playerOffset = new Vector3(0f, Mathf.PingPong(value * 2f, 1f) * _jumpHeight, 0f); }, _animLegJumpB);
					break;
				case InputController.InputType.Roll:
					_playerAnimator.SetTrigger(Roll);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(inputType), inputType, null);
			}
		}
	}
}
