using System;

namespace Armaments.Movers
{
	public interface IArmamentMover 
	{
		void Move();
		event Action<IArmamentMover>  Reached;
		event Action<IArmamentMover> Launched;
		Armament Armament { get; }
		float Duration { get; }
	}
}