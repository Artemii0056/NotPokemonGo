using System;
using System.Collections;
using Services;
using UnityEngine;

namespace Armaments
{
	public class ArmamentMover : IArmamentMover
	{
		private const float Epsilon = 0.5f;

		private readonly ICoroutineRunner _coroutineRunner;

		public event Action<Armament> Reached;

		public ArmamentMover(ICoroutineRunner coroutineRunner) => 
			_coroutineRunner = coroutineRunner;

		public void Move(Armament armament) => 
			_coroutineRunner.StartCoroutine(MoveCoroutine(armament));

		private IEnumerator MoveCoroutine(Armament armament)
		{
			while (Vector3.Distance(armament.transform.position, armament.Target.transform.position) >= Epsilon)
			{
				armament.Move();
				yield return null;
			}

			Reached?.Invoke(armament);
		}
	}
}