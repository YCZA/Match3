using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wooga.Coroutines
{
	// Token: 0x020003C8 RID: 968
	public class CoroutineRunner : MonoBehaviour
	{
		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06001D28 RID: 7464 RVA: 0x0007DE3C File Offset: 0x0007C23C
		public static CoroutineRunner Instance
		{
			get
			{
				if (CoroutineRunner._coroutineRunner == null)
				{
					CoroutineRunner._coroutineRunner = new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();
					try
					{
						global::UnityEngine.Object.DontDestroyOnLoad(CoroutineRunner._coroutineRunner);
					}
					catch
					{
					}
				}
				return CoroutineRunner._coroutineRunner;
			}
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x0007DE98 File Offset: 0x0007C298
		public static Future<object> StartAfterDelay(TimeSpan delay, Action action)
		{
			return CoroutineRunner.Instance.StartAfterDelay(delay, action);
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x0007DEA6 File Offset: 0x0007C2A6
		public static Future<T> FromResult<T>(Func<T> factory)
		{
			return CoroutineRunner.Instance.FromResult(factory);
		}

		// Token: 0x06001D2B RID: 7467 RVA: 0x0007DEB3 File Offset: 0x0007C2B3
		public static Future<T> FromResult<T>(T result)
		{
			return CoroutineRunner.Instance.FromResult(result);
		}

		// Token: 0x06001D2C RID: 7468 RVA: 0x0007DEC0 File Offset: 0x0007C2C0
		public static Future<object> StartTask(IEnumerator enumerator)
		{
			return CoroutineRunner.Instance.StartTask(enumerator);
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x0007DECD File Offset: 0x0007C2CD
		public static Future<object> StartTask(IEnumerator enumerator, ICancellationToken cancellationToken)
		{
			return CoroutineRunner.Instance.StartTask(enumerator, cancellationToken);
		}

		// Token: 0x06001D2E RID: 7470 RVA: 0x0007DEDB File Offset: 0x0007C2DB
		public static Future<object> StartTask(IEnumerator enumerator, Action<Exception> errorHandler, ICancellationToken cancellationToken)
		{
			return CoroutineRunner.Instance.StartTask(enumerator, errorHandler, cancellationToken);
		}

		// Token: 0x06001D2F RID: 7471 RVA: 0x0007DEEA File Offset: 0x0007C2EA
		public static Future<T> StartTask<T>(IEnumerator<T> enumerator, Action<Exception> errorHandler, ICancellationToken cancellationToken)
		{
			return CoroutineRunner.Instance.StartTask(enumerator, errorHandler, cancellationToken);
		}

		// Token: 0x06001D30 RID: 7472 RVA: 0x0007DEF9 File Offset: 0x0007C2F9
		public static Future<T> StartTask<T>(IEnumerator<T> enumerator)
		{
			return CoroutineRunner.Instance.StartTask(enumerator);
		}

		// Token: 0x06001D31 RID: 7473 RVA: 0x0007DF06 File Offset: 0x0007C306
		public static Coroutine StartRoutine(IEnumerator routine)
		{
			return CoroutineRunner.Instance.StartCoroutine(routine);
		}

		// Token: 0x06001D32 RID: 7474 RVA: 0x0007DF13 File Offset: 0x0007C313
		public static void StopRoutine(Coroutine coroutine)
		{
			CoroutineRunner.Instance.StopCoroutine(coroutine);
		}

		// Token: 0x06001D33 RID: 7475 RVA: 0x0007DF20 File Offset: 0x0007C320
		public static void Shutdown()
		{
			CoroutineRunner.Instance.StopAllCoroutines();
			global::UnityEngine.Object.DestroyImmediate(CoroutineRunner._coroutineRunner.gameObject);
		}

		// Token: 0x06001D34 RID: 7476 RVA: 0x0007DF3B File Offset: 0x0007C33B
		private void OnDestroy()
		{
			CoroutineRunner._coroutineRunner = null;
		}

		// Token: 0x040049C4 RID: 18884
		private static CoroutineRunner _coroutineRunner;
	}
}
