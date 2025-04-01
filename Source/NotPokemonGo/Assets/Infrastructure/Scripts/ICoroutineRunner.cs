using System.Collections;
using UnityEngine;

namespace Infrastructure.Scripts
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}