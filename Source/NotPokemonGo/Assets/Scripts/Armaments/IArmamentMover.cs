using System;

namespace Armaments
{
	public interface IArmamentMover
	{
		void Move(Armament armament);
		event Action<Armament> Reached;
	}
}