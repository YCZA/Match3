using System;
using System.Collections;
using UnityEngine;

namespace Wooga.Coroutines
{
	// Token: 0x02000B79 RID: 2937
	public class WooroutineRunner : MonoBehaviour
	{
		public static bool isDestroy = false;
		private static WooroutineRunner Instance
		{
			get
			{
				if (_runner != null)
				{
					return _runner;
				}
				_runner = new GameObject("WooroutineRunner").AddComponent<WooroutineRunner>();
				DontDestroyOnLoad(WooroutineRunner._runner);
				_runner.hideFlags = HideFlags.DontSaveInEditor;
				isDestroy = false;
				return _runner;
			}
		}

		// Token: 0x060044C3 RID: 17603 RVA: 0x0015D645 File Offset: 0x0015BA45
		public static Wooroutine<T> StartWooroutine<T>(IEnumerator routineWithReturnValue)
		{
			return WooroutineRunner.Instance.StartWooroutine<T>(routineWithReturnValue);
		}

		// Token: 0x060044C4 RID: 17604 RVA: 0x0015D652 File Offset: 0x0015BA52
		public static Coroutine StartCoroutine(IEnumerator routine, Action onComplete = null)
		{
			if (!isDestroy)
			{
				return WooroutineRunner.Instance.StartCoroutine(routine, onComplete);
			}

			return null;
		}

		// Token: 0x060044C5 RID: 17605 RVA: 0x0015D660 File Offset: 0x0015BA60
		public static void Stop(Coroutine coroutine)
		{
			if (WooroutineRunner._runner != null)
			{
				WooroutineRunner._runner.StopCoroutine(coroutine);
			}
		}
		
		public static void Stop(IEnumerator coroutine)
		{
			if (WooroutineRunner._runner != null)
			{
				WooroutineRunner._runner.StopCoroutine(coroutine);
			}
		}

		// Token: 0x060044C6 RID: 17606 RVA: 0x0015D67D File Offset: 0x0015BA7D
		public static void PrepareForReset()
		{
			if (WooroutineRunner._runner)
			{
				WooroutineRunner._runner.StopAllCoroutines();
			}
		}

		// Token: 0x060044C7 RID: 17607 RVA: 0x0015D698 File Offset: 0x0015BA98
		private void OnDestroy()
		{
			isDestroy = true;
			if (_runner)
			{
				_runner = null;
			}
		}

		// Token: 0x04006C8E RID: 27790
		public const string RETURNVALUE_ACCESSOR = "returnvalue_accessor_method";

		// Token: 0x04006C8F RID: 27791
		private static WooroutineRunner _runner;
	}
}
