using System;
using Internal;
using Player;
using TMPro;
using UnityEngine;
namespace UI
{
	public class MainMenuWidget : WidgetBase, MainMenuWidget.IMainMenu
	{
		public interface IMainMenu : IWidgetBase
		{
		}

		[Space]
		[SerializeField] private LevelConstructor _levelConstructor;
		[SerializeField] private TextMeshProUGUI _scoreValue;
		private                  void            Awake()     => Register();
		private                  void            OnDestroy() => Unregister();
		private void Start()
		{
			_levelConstructor = Locator.GetObject<LevelConstructor>();
		}

		public override void Register()   => Locator.Register(typeof(IMainMenu), this);
		public override void Unregister() => Locator.Unregister(typeof(IMainMenu));

		protected override void Show(Action afterShow = null)
		{
			_scoreValue.text = $"{Locator.GetObject<IPlayer>().Score:000}";
			base.Show(afterShow);
		}
		public void ButtonStartGame()
		{
			Hide(() => _levelConstructor.CreateLevel(0, 0));
		}

	}
}
