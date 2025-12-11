using System;

namespace Armaments
{
	public interface IArmamentMover 
	{
		void Move(Armament armament, ArmamentFlyingType armamentFlyingType);
		event Action<Armament, ArmamentMover>  Reached;
	}
}