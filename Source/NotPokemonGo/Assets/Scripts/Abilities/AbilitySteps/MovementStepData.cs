using Units;

namespace Abilities.AbilitySteps
{
	public class MovementStepData : AbilityStepData
	{
		public enum MovementMode
		{
			Run,
			Jump,
			Teleport
		}
		
		public MovementMode Mode;
		public float StopDistance = 1.5f;
		public float LiftDelay = 0f;
		public int JumpPower = 2;
		public bool ReturnToStart = false;
	}
}