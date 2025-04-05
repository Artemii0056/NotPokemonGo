using System.Collections;
using UnityEngine;

namespace Infrastructure.Scripts.Services
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}