using System;
using System.Collections;
using System.Collections.Generic;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

// Token: 0x02000B2B RID: 2859
namespace Match3.Scripts1
{
	public class TimeSlicer
	{
		// Token: 0x0600431B RID: 17179 RVA: 0x001577EC File Offset: 0x00155BEC
		public TimeSlicer(float timeSlice = 0.01f)
		{
			this._timeSlice = timeSlice;
		}

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x0600431C RID: 17180 RVA: 0x00157827 File Offset: 0x00155C27
		// (set) Token: 0x0600431D RID: 17181 RVA: 0x0015782F File Offset: 0x00155C2F
		public float TimeSlice
		{
			get
			{
				return this._timeSlice;
			}
			set
			{
				this._timeSlice = value;
			}
		}

		// Token: 0x0600431E RID: 17182 RVA: 0x00157838 File Offset: 0x00155C38
		private IEnumerator UpdateRoutine()
		{
			this.index = 0;
			while (this.index < this.pendingOperations.Count)
			{
				this.PerFrameRoutine();
				yield return null;
			}
			this.pendingOperations.Clear();
			this.runningCoroutines.Clear();
			this.onCompleted.Dispatch();
			yield break;
		}

		// Token: 0x0600431F RID: 17183 RVA: 0x00157854 File Offset: 0x00155C54
		private void PerFrameRoutine()
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			int num = 0;
			while (this.index < this.pendingOperations.Count)
			{
				if (this.pendingOperations[this.index] != null)
				{
					this.pendingOperations[this.index]();
					this.pendingOperations[this.index] = null;
					num++;
				}
				this.index++;
				float num2 = Time.realtimeSinceStartup - realtimeSinceStartup;
				if (num2 >= this.TimeSlice)
				{
					if (TimeSlicer.LOG)
					{
						WoogaDebug.Log(new object[]
						{
							"stopped after",
							num,
							num2,
							this.index,
							this.TimeSlice
						});
					}
					return;
				}
			}
		}

		// Token: 0x06004320 RID: 17184 RVA: 0x00157934 File Offset: 0x00155D34
		public void Add(Action operation)
		{
			this.pendingOperations.Add(operation);
			if (this.pendingOperations.Count == 1)
			{
				this.Start();
			}
		}

		// Token: 0x06004321 RID: 17185 RVA: 0x0015795C File Offset: 0x00155D5C
		public void Stop()
		{
			foreach (Coroutine coroutine in this.runningCoroutines)
			{
				if (coroutine != null)
				{
					WooroutineRunner.Stop(coroutine);
				}
			}
			this.pendingOperations.Clear();
			this.runningCoroutines.Clear();
		}

		// Token: 0x06004322 RID: 17186 RVA: 0x001579D4 File Offset: 0x00155DD4
		public YieldInstruction AddAndAwait(Action operation)
		{
			Coroutine coroutine = WooroutineRunner.StartCoroutine(this.AwaitRoutine(operation), null);
			this.runningCoroutines.Add(coroutine);
			return coroutine;
		}

		// Token: 0x06004323 RID: 17187 RVA: 0x001579FC File Offset: 0x00155DFC
		private IEnumerator AwaitRoutine(Action operation)
		{
			int thisIndex = this.pendingOperations.Count;
			this.Add(operation);
			while (this.index <= thisIndex)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06004324 RID: 17188 RVA: 0x00157A1E File Offset: 0x00155E1E
		private void Start()
		{
			this.runningCoroutines.Add(WooroutineRunner.StartCoroutine(this.UpdateRoutine(), null));
		}

		// Token: 0x04006BCC RID: 27596
		public static bool LOG;

		// Token: 0x04006BCD RID: 27597
		private List<Action> pendingOperations = new List<Action>();

		// Token: 0x04006BCE RID: 27598
		private List<Coroutine> runningCoroutines = new List<Coroutine>();

		// Token: 0x04006BCF RID: 27599
		public readonly AwaitSignal onCompleted = new AwaitSignal();

		// Token: 0x04006BD0 RID: 27600
		private float _timeSlice = 0.01f;

		// Token: 0x04006BD1 RID: 27601
		private int index;
	}
}
