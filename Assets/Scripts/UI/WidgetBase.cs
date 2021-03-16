using System;
using UnityEngine;
namespace UI
{
	public abstract class WidgetBase : MonoBehaviour, IWidgetBase
	{
		[Header("Show\\Hide")]
		[SerializeField] private float _showTime = 1f;
		[SerializeField] private float _hideTime = 1f;
		public abstract          void  Register();
		public abstract          void  Unregister();

		protected virtual void Show(Action afterShow)
		{
			var cg = GetComponent<CanvasGroup>();
			if (cg)
				this.InvokeDelegateUnscaled((value) => { cg.alpha = value; }, _showTime, () => { cg.interactable = true; afterShow?.Invoke();});
			else
				this.gameObject.SetActive(true);
		}
		protected virtual void Hide(Action afterHide)
		{
			var cg = GetComponent<CanvasGroup>();
			if (cg)
			{
				cg.interactable = false;
				this.InvokeDelegateUnscaled((value) => { cg.alpha = 1f - value; }, _hideTime, afterHide);
			}
			else
				this.gameObject.SetActive(false);
		}
		
		void IWidgetBase.Show(Action afterShow = null) => Show(afterShow);
		void IWidgetBase.Hide(Action afterHide = null) => Hide(afterHide);
	}
}
