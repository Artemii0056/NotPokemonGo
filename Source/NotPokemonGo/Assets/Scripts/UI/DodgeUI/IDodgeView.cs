using System;
using Infrastructure.MVP.Implementation;
using Units;

namespace UI.DodgeUI
{
	public interface IDodgeView : IView
	{
		event Action<Unit> Dodged;
		void Destroy();
	}
}