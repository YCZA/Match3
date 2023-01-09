using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Wooga.Coroutines
{
	// Token: 0x020003C7 RID: 967
	public static class MonoBehaviourTaskRunnerExtensions
	{
		// Token: 0x06001D1D RID: 7453 RVA: 0x0007DD52 File Offset: 0x0007C152
		public static Future<object> StartAfterDelay(this MonoBehaviour monoBehaviour, TimeSpan delay, Action action)
		{
			return monoBehaviour.StartTask(new WaitForSeconds((float)delay.TotalSeconds).Yield<WaitForSeconds>().OnComplete(action), null, null);
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x0007DD74 File Offset: 0x0007C174
		public static Future<T> FromResult<T>(this MonoBehaviour monoBehaviour, Func<T> factory)
		{
			return monoBehaviour.StartTask(factory.Yield<T>(), null, null);
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x0007DD84 File Offset: 0x0007C184
		public static Future<T> FromResult<T>(this MonoBehaviour monoBehaviour, T result)
		{
			return monoBehaviour.StartTask(result.Yield<T>(), null, null);
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x0007DD94 File Offset: 0x0007C194
		public static Future<object> StartTask(this MonoBehaviour monoBehaviour, IEnumerator enumerator)
		{
			if (MonoBehaviourTaskRunnerExtensions._003C_003Ef__mg_0024cache0 == null)
			{
				MonoBehaviourTaskRunnerExtensions._003C_003Ef__mg_0024cache0 = new Func<object, IEnumerator<object>>(Coroutines.Yield<object>);
			}
			return monoBehaviour.StartTask(enumerator.ContinueWith(MonoBehaviourTaskRunnerExtensions._003C_003Ef__mg_0024cache0), null, null);
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x0007DDC1 File Offset: 0x0007C1C1
		public static Future<object> StartTask(this MonoBehaviour monoBehaviour, IEnumerator enumerator, ICancellationToken cancellationToken)
		{
			if (MonoBehaviourTaskRunnerExtensions._003C_003Ef__mg_0024cache1 == null)
			{
				MonoBehaviourTaskRunnerExtensions._003C_003Ef__mg_0024cache1 = new Func<object, IEnumerator<object>>(Coroutines.Yield<object>);
			}
			return monoBehaviour.StartTask(enumerator.ContinueWith(MonoBehaviourTaskRunnerExtensions._003C_003Ef__mg_0024cache1), null, cancellationToken);
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x0007DDEE File Offset: 0x0007C1EE
		public static Future<T> StartTask<T>(this MonoBehaviour monoBehaviour, IEnumerator<T> enumerator)
		{
			return monoBehaviour.StartTask(enumerator, null, null);
		}

		// Token: 0x06001D23 RID: 7459 RVA: 0x0007DDF9 File Offset: 0x0007C1F9
		public static Future<T> StartTask<T>(this MonoBehaviour monoBehaviour, IEnumerator<T> enumerator, ICancellationToken cancellationToken)
		{
			return monoBehaviour.StartTask(enumerator, null, cancellationToken);
		}

		// Token: 0x06001D24 RID: 7460 RVA: 0x0007DE04 File Offset: 0x0007C204
		public static Future<T> StartTask<T>(this MonoBehaviour monoBehaviour, IEnumerator<T> enumerator, Action<Exception> errorHandler)
		{
			return monoBehaviour.StartTask(enumerator, errorHandler, null);
		}

		// Token: 0x06001D25 RID: 7461 RVA: 0x0007DE0F File Offset: 0x0007C20F
		public static Future<object> StartTask(this MonoBehaviour monoBehaviour, IEnumerator enumerator, Action<Exception> errorHandler, ICancellationToken cancellationToken)
		{
			return new Future<object>(enumerator.Cast<object>(), errorHandler, cancellationToken).Start(monoBehaviour);
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x0007DE24 File Offset: 0x0007C224
		public static Future<T> StartTask<T>(this MonoBehaviour monoBehaviour, IEnumerator<T> enumerator, Action<Exception> errorHandler, ICancellationToken cancellationToken)
		{
			return new Future<T>(enumerator, errorHandler, cancellationToken).Start(monoBehaviour);
		}

		// Token: 0x040049C2 RID: 18882
		[CompilerGenerated]
		private static Func<object, IEnumerator<object>> _003C_003Ef__mg_0024cache0;

		// Token: 0x040049C3 RID: 18883
		[CompilerGenerated]
		private static Func<object, IEnumerator<object>> _003C_003Ef__mg_0024cache1;
	}
}
