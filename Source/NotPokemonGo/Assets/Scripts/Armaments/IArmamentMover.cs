using System;

namespace Armaments
{
	public interface IArmamentMover //TODO Не используется? 
	{
		void Move(Armament armament, bool isReturn = false);
		event Action<Armament, ArmamentMover>  Reached;
	}
}