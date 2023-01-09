using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Wooga.Coroutines
{
	// Token: 0x02000B78 RID: 2936
	public class Wooroutine<T> : CustomYieldInstruction
	{
		// Token: 0x060044AC RID: 17580 RVA: 0x0015D1BE File Offset: 0x0015B5BE
		public Wooroutine(IEnumerator routine, MonoBehaviour runner)
		{
			this._enumerator = routine;
			this._runner = runner;
			this.Start();
		}

		// Token: 0x060044AD RID: 17581 RVA: 0x0015D1DB File Offset: 0x0015B5DB
		private Wooroutine(T result)
		{
			this._enumerator = null;
			this._runner = null;
			this._returnValue = result;
			this._success = true;
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x060044AE RID: 17582 RVA: 0x0015D1FF File Offset: 0x0015B5FF
		public T ReturnValue
		{
			get
			{
				if (!this.Completed)
				{
					throw new Exception(typeof(Wooroutine<T>).FullName + " did not complete.");
				}
				this.CheckExceptions();
				return this._returnValue;
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x060044AF RID: 17583 RVA: 0x0015D237 File Offset: 0x0015B637
		// (set) Token: 0x060044B0 RID: 17584 RVA: 0x0015D23F File Offset: 0x0015B63F
		public Exception Exception { get; private set; }

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x060044B1 RID: 17585 RVA: 0x0015D248 File Offset: 0x0015B648
		public bool Started
		{
			get
			{
				return this._coroutine != null || this.Completed || this.Canceled;
			}
		}

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x060044B2 RID: 17586 RVA: 0x0015D269 File Offset: 0x0015B669
		public bool Completed
		{
			get
			{
				return this._success || this.HasError;
			}
		}

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x060044B3 RID: 17587 RVA: 0x0015D27F File Offset: 0x0015B67F
		public bool Canceled
		{
			get
			{
				return !this.Completed && this._enumerator == null;
			}
		}

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x060044B4 RID: 17588 RVA: 0x0015D298 File Offset: 0x0015B698
		public bool HasError
		{
			get
			{
				return this.Exception != null;
			}
		}

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x060044B5 RID: 17589 RVA: 0x0015D2A6 File Offset: 0x0015B6A6
		// (set) Token: 0x060044B6 RID: 17590 RVA: 0x0015D2AE File Offset: 0x0015B6AE
		public bool ThrowImmediate { get; set; }

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x060044B7 RID: 17591 RVA: 0x0015D2B7 File Offset: 0x0015B6B7
		public override bool keepWaiting
		{
			get
			{
				return !this.Completed;
			}
		}

		// Token: 0x060044B8 RID: 17592 RVA: 0x0015D2C2 File Offset: 0x0015B6C2
		public void CheckExceptions()
		{
			if (this.Exception != null)
			{
				this.AddAccessorToExceptionData();
				Debug.LogError("StackTrace:" + Exception.StackTrace);
				throw this.Exception;
			}
		}

		// Token: 0x060044B9 RID: 17593 RVA: 0x0015D2DC File Offset: 0x0015B6DC
		public static Wooroutine<T> CreateCompleted(T result)
		{
			return new Wooroutine<T>(result);
		}

		// Token: 0x060044BA RID: 17594 RVA: 0x0015D2E4 File Offset: 0x0015B6E4
		public Coroutine DispatchExceptionAndYieldForever(Action<Exception> dispatch)
		{
			if (this.Exception != null)
			{
				this.AddAccessorToExceptionData();
				if (this._runner)
				{
					return this._runner.StartCoroutine(this.DispatchExceptionAndYieldForeverRoutine(dispatch));
				}
				dispatch(this.Exception);
			}
			return null;
		}

		// Token: 0x060044BB RID: 17595 RVA: 0x0015D332 File Offset: 0x0015B732
		protected Wooroutine<T> Start()
		{
			this._coroutine = this._runner.StartCoroutine(this.WrapRoutine());
			return this;
		}

		// Token: 0x060044BC RID: 17596 RVA: 0x0015D34C File Offset: 0x0015B74C
		private IEnumerator DispatchExceptionAndYieldForeverRoutine(Action<Exception> dispatch)
		{
			dispatch(this.Exception);
			for (;;)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060044BD RID: 17597 RVA: 0x0015D370 File Offset: 0x0015B770
		private IEnumerator WrapRoutine()
		{
			for (;;)
			{
				try
				{
					if (!this._enumerator.MoveNext())
					{
						this._returnValue = (T)((object)this._enumerator.Current);
						this._success = true;
					}
				}
				catch (Exception exception)
				{
					this.Exception = exception;
					if (this.ThrowImmediate)
					{
						this.CheckExceptions();
					}
				}
				if (this.Completed)
				{
					break;
				}
				yield return this._enumerator.Current;
			}
			this.Clear();
		}

		// Token: 0x060044BE RID: 17598 RVA: 0x0015D38B File Offset: 0x0015B78B
		public void Stop()
		{
			if (this._coroutine != null)
			{
				this._runner.StopCoroutine(this._coroutine);
			}
			this.Clear();
		}

		// Token: 0x060044BF RID: 17599 RVA: 0x0015D3AF File Offset: 0x0015B7AF
		private void Clear()
		{
			this._coroutine = null;
			this._enumerator = null;
		}

		// Token: 0x060044C0 RID: 17600 RVA: 0x0015D3C0 File Offset: 0x0015B7C0
		private void AddAccessorToExceptionData()
		{
			StackFrame stackFrame = new StackFrame(2, false);
			this.Exception.Data["returnvalue_accessor_method"] = stackFrame.GetMethod().DeclaringType.FullName + "." + stackFrame.GetMethod().Name;
		}

		// Token: 0x04006C89 RID: 27785
		private IEnumerator _enumerator;

		// Token: 0x04006C8A RID: 27786
		private Coroutine _coroutine;

		// Token: 0x04006C8B RID: 27787
		private T _returnValue;

		// Token: 0x04006C8C RID: 27788
		private bool _success;

		// Token: 0x04006C8D RID: 27789
		private readonly MonoBehaviour _runner;
	}
}
