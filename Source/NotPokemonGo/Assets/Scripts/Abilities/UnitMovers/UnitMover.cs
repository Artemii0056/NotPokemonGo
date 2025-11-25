using System.Collections;
using DG.Tweening;
using Services;
using Units;
using UnityEngine;

namespace Abilities.UnitMovers
{
	public class UnitMover : IUnitMover
	{
		private readonly ICoroutineRunner _coroutineRunner;

		public UnitMover(ICoroutineRunner coroutineRunner)
		{
			_coroutineRunner = coroutineRunner;
		}
		
		public void MoveByType(UnitMoveType unitMoveType, Unit unit, Vector3 target, float animationLength)
		{
			switch (unitMoveType)
			{
				case UnitMoveType.JumpByDotween:
					_coroutineRunner.StartCoroutine(JumpByDotween(unit, target, animationLength));
					break;

				case UnitMoveType.MoveByMoveTowards:
					_coroutineRunner.StartCoroutine(MoveUnit(unit, target, animationLength));
					break;
				
				default:
					throw new System.NotImplementedException();
			}
		}

		private IEnumerator JumpByDotween(Unit unit, Vector3 target, float animationLength)
		{
			float liftDelay = 0.6f;
			int jumpPower = 2;
			var duration = animationLength;

			yield return new WaitForSeconds(liftDelay);

			float moveDuration = duration - liftDelay;
			unit.transform.DOKill();

			Tween jumpTween = unit.transform
				.DOJump(target, jumpPower, 1, moveDuration)
				.SetEase(Ease.InQuad);

			yield return jumpTween.WaitForCompletion();
		}
		
		private IEnumerator MoveUnit(Unit unit, Vector3 targetPosition, float offset = 0)
		{
			const float Speed = 4f;

			while (Vector3.Distance(unit.transform.position, targetPosition) > offset)
			{
				unit.transform.position = Vector3.MoveTowards(
					unit.transform.position,
					targetPosition,
					Speed * Time.deltaTime);

				yield return null;
			}
		}
	}

	public enum UnitMoveType
	{
		JumpByDotween,
		MoveByMoveTowards
	}
}