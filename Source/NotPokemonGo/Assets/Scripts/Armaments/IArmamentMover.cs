using System;

namespace Armaments
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