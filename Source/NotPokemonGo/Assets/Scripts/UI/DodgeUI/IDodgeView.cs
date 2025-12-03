using System;
using Infrastructure.MVP.Implementation;

namespace UI.DodgeUI
{
	public interface IDodgeView : IView
	{
		event Action Dodged;
		void Destroy();
	}
}