using System;
using Internal;
namespace UI
{
	public interface IWidgetBase : IRegistrable
	{
		void Show(Action afterShow = null);
		void Hide(Action afterHide = null);
	}
}
