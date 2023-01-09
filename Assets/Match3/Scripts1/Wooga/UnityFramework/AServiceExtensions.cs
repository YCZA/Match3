using System;
using System.Collections;
using Wooga.Coroutines;
using UnityEngine;

namespace Wooga.UnityFramework
{
	// Token: 0x02000B68 RID: 2920
	public static class AServiceExtensions
	{
		// Token: 0x0600444A RID: 17482 RVA: 0x0015C229 File Offset: 0x0015A629
		public static YieldInstruction Await<T>(this T service, Action<T> setter) where T : AService
		{
			return WooroutineRunner.StartCoroutine(AServiceExtensions.AwaitRoutine<T>(setter), null);
		}

		// Token: 0x0600444B RID: 17483 RVA: 0x0015C238 File Offset: 0x0015A638
		private static IEnumerator AwaitRoutine<T>(Action<T> setter)
		{
			Wooroutine<T> s = ServiceLocator.Instance.Await<T>(true);
			yield return s;
			setter(s.ReturnValue);
			yield break;
		}
	}
}
