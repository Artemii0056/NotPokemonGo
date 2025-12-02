using System;
using Infrastructure.MVP.Implementation;
using Units;

namespace UI.DodgeUI
{
	public interface IDodgePresenter : IPresenter
	{
		event Action<Unit> Dodged;
	}
}