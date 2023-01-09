using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wooga.Coroutines
{
	// Token: 0x020003CD RID: 973
	public class Future<T> : Future, IEnumerator<T>, IEnumerator, IDisposable
	{
		// Token: 0x06001D85 RID: 7557 RVA: 0x0007E925 File Offset: 0x0007CD25
		public Future(IEnumerator<T> enumerator, Action<Exception> errorHandler = null, ICancellationToken cancellationToken = null)
		{
			// Debug.Log("Create Future");
			this._enumerator = enumerator;
			base.SetErrorHandler(errorHandler);
			if (cancellationToken != null)
			{
				this._cancellation = cancellationToken;
			}
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x0007E94F File Offset: 0x0007CD4F
		public Future(T result) : this(null, null, null)
		{
			this._result = result;
			this.Result = this._result;
			this.Done();
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06001D87 RID: 7559 RVA: 0x0007E973 File Offset: 0x0007CD73
		// (set) Token: 0x06001D88 RID: 7560 RVA: 0x0007E97B File Offset: 0x0007CD7B
		public bool IsCancelled { get; protected set; }

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06001D89 RID: 7561 RVA: 0x0007E984 File Offset: 0x0007CD84
		// (set) Token: 0x06001D8A RID: 7562 RVA: 0x0007E98C File Offset: 0x0007CD8C
		public bool IsRanToCompletion { get; protected set; }

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06001D8B RID: 7563 RVA: 0x0007E995 File Offset: 0x0007CD95
		public bool IsStarted
		{
			get
			{
				return this._runner != null;
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06001D8C RID: 7564 RVA: 0x0007E9A3 File Offset: 0x0007CDA3
		public bool IsFaulted
		{
			get
			{
				return this.Exception != null;
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06001D8D RID: 7565 RVA: 0x0007E9B1 File Offset: 0x0007CDB1
		public bool IsCompleted
		{
			get
			{
				return this.IsRanToCompletion || this.IsCancelled || this.IsFaulted;
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06001D8E RID: 7566 RVA: 0x0007E9D2 File Offset: 0x0007CDD2
		// (set) Token: 0x06001D8F RID: 7567 RVA: 0x0007E9FD File Offset: 0x0007CDFD
		public T Result
		{
			get
			{
				if (this.IsRanToCompletion)
				{
					return this._result;
				}
				throw this.Exception ?? new Exception("Not completed");
			}
			protected set
			{
				this.IsRanToCompletion = true;
				this._result = value;
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06001D90 RID: 7568 RVA: 0x0007EA0D File Offset: 0x0007CE0D
		public Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x06001D91 RID: 7569 RVA: 0x0007EA18 File Offset: 0x0007CE18
		public Future<T> Start(object taskRunner)
		{
			if (Future.UseProbing)
			{
				try
				{
					this._probed = true;
					if (this._enumerator.Probe(out this._current))
					{
						this._result = (T)((object)this._current);
						this.Result = this._result;
						this.Done();
						return this;
					}
				}
				catch (Exception exception)
				{
					this._exception = exception;
					this.Done();
					return this;
				}
			}
			this._runner = taskRunner;
			this._coroutine = ((MonoBehaviour)this._runner).StartCoroutine(this);
			return this;
		}

		// Token: 0x06001D92 RID: 7570 RVA: 0x0007EAC4 File Offset: 0x0007CEC4
		public override void Cancel()
		{
			if (!this.IsCompleted)
			{
				this.IsCancelled = true;
				MonoBehaviour monoBehaviour = this._runner as MonoBehaviour;
				Coroutine coroutine = (Coroutine)this._coroutine;
				if (monoBehaviour != null && coroutine != null)
				{
					monoBehaviour.StopCoroutine(coroutine);
				}
				this.Cleanup();
			}
		}

		// Token: 0x06001D93 RID: 7571 RVA: 0x0007EB1C File Offset: 0x0007CF1C
		public IEnumerator<T> Await()
		{
			while (!this.IsCompleted)
			{
				yield return default(T);
			}
			if (this._exception != null)
			{
				throw this._exception;
			}
			yield return (!(this._result is Coroutine)) ? this._result : default(T);
			yield break;
		}

		// Token: 0x06001D94 RID: 7572 RVA: 0x0007EB38 File Offset: 0x0007CF38
		public override bool MoveNext()
		{
			if (this._cancellation != null && this._cancellation.IsCanceled)
			{
				this.Cancel();
				return false;
			}
			if (this._probed)
			{
				this._probed = false;
				return true;
			}
			bool flag = !this.IsCompleted;
			if (!flag)
			{
				return false;
			}
			int frameCount = Time.frameCount;
			if (frameCount == this._lastFrame)
			{
				return true;
			}
			this._lastFrame = frameCount;
			bool result;
			try
			{
				if (!Coroutines.FlattenMoveNext(ref this._stack, ref this._enumerator, ref this._current))
				{
					if (this._current is T)
					{
						this.Result = (T)((object)this._current);
					}
					else
					{
						if (typeof(T) != typeof(object))
						{
						}
						this.Result = default(T);
					}
					this.Done();
					result = false;
				}
				else
				{
					result = true;
				}
			}
			catch (Exception exception)
			{
				this._exception = exception;
				this.Done();
				result = false;
			}
			return result;
		}

		// Token: 0x06001D95 RID: 7573 RVA: 0x0007EC54 File Offset: 0x0007D054
		public override void Reset()
		{
			throw new NotImplementedException();
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06001D96 RID: 7574 RVA: 0x0007EC5B File Offset: 0x0007D05B
		public override object Current
		{
			get
			{
				if (this.IsFaulted)
				{
					throw this._exception;
				}
				return this._current;
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06001D97 RID: 7575 RVA: 0x0007EC78 File Offset: 0x0007D078
		T IEnumerator<T>.Current
		{
			get
			{
				return (!this.IsCompleted) ? default(T) : this.Result;
			}
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x0007ECA4 File Offset: 0x0007D0A4
		protected void Done()
		{
			try
			{
				if (this.IsFaulted)
				{
					try
					{
						this.OnFaulted(this._exception);
					}
					catch
					{
					}
				}
			}
			finally
			{
				this.Cleanup();
			}
		}

		// Token: 0x06001D99 RID: 7577 RVA: 0x0007ECFC File Offset: 0x0007D0FC
		protected override void Cleanup()
		{
			base.Cleanup();
			this._coroutine = null;
			this._enumerator = null;
		}

		// Token: 0x040049CC RID: 18892
		private Stack<IEnumerator> _stack;

		// Token: 0x040049CD RID: 18893
		protected T _result;

		// Token: 0x040049CE RID: 18894
		protected IEnumerator _enumerator;

		// Token: 0x040049CF RID: 18895
		protected ICancellationToken _cancellation;

		// Token: 0x040049D0 RID: 18896
		protected object _runner;

		// Token: 0x040049D1 RID: 18897
		protected object _coroutine;

		// Token: 0x040049D2 RID: 18898
		protected Exception _exception;

		// Token: 0x040049D3 RID: 18899
		private bool _probed;

		// Token: 0x040049D6 RID: 18902
		private object _current;

		// Token: 0x040049D7 RID: 18903
		private int _lastFrame = -1;
	}
}
