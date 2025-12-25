using System;

namespace Armaments
{
	public interface IArmamentMover 
	{
		void Move(Armament armament);
		event Action<IArmamentMover>  Reached;
		event Action<IArmamentMover> Launched;
		Armament Armament { get; }
		float Duration { get;}
	}
}