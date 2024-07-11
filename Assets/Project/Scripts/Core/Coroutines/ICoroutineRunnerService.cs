using System.Collections;
using Fingers.Core.Services;
using UnityEngine;

namespace Fingers.Core.Coroutines
{
    public interface ICoroutineRunnerService : IService
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}