using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wooga.Coroutines
{
	// Token: 0x02000B7A RID: 2938
	public static class WooroutineRunnerExtensions
	{
		// Token: 0x060044C8 RID: 17608 RVA: 0x0015D6AF File Offset: 0x0015BAAF
		public static Wooroutine<T> StartWooroutine<T>(this MonoBehaviour behaviour, IEnumerator routineWithReturnValue)
		{
			return new Wooroutine<T>(routineWithReturnValue, behaviour);
		}

		// Token: 0x060044C9 RID: 17609 RVA: 0x0015D6B8 File Offset: 0x0015BAB8
		public static Coroutine StartCoroutine(this MonoBehaviour behaviour, IEnumerator routine, Action onComplete = null)
		{
			return behaviour.StartCoroutine(routine);
		}

		// Token: 0x060044CA RID: 17610 RVA: 0x0015D6C1 File Offset: 0x0015BAC1
		public static Wooroutine<T> StartWooroutine<T>(this IEnumerator<T> enumerator)
		{
			return WooroutineRunner.StartWooroutine<T>(enumerator);
		}
	}
}
