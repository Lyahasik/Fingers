using System.Collections;
using UnityEngine;

using EmpireCafe.Core.Services;

namespace EmpireCafe.Core.Coroutines
{
    public interface ICoroutineRunnerService : IService
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}