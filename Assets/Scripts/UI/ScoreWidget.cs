using System;
using Internal;
using TMPro;
using UnityEngine;
namespace UI
{
	public class ScoreWidget : WidgetBase, IScoreWidget
	{
		[SerializeField] private TextMeshProUGUI _scoreValue;

		public override void Register()   => Locator.Register(typeof(IScoreWidget), (IScoreWidget)this);
		public override void Unregister() => Locator.Unregister(typeof(IScoreWidget));
		public          void SetScore(int value) => _scoreValue.text = $"{value:000}";

		private void Awake()     => Register();
		private void OnDestroy() => Unregister();
	}
}
