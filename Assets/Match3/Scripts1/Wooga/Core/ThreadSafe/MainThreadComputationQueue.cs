using System;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Core.ThreadSafe
{
	// Token: 0x020003A4 RID: 932
	public class MainThreadComputationQueue : MonoBehaviour
	{
		// Token: 0x06001C1D RID: 7197 RVA: 0x0007BD50 File Offset: 0x0007A150
		public static void Enqueue(Action a)
		{
			object inbox = MainThreadComputationQueue.Inbox;
			lock (inbox)
			{
				MainThreadComputationQueue.Inbox.Enqueue(a);
			}
		}

		// Token: 0x06001C1E RID: 7198 RVA: 0x0007BD90 File Offset: 0x0007A190
		public void Start()
		{
			MainThreadComputationQueue.Shared = this;
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x0007BD98 File Offset: 0x0007A198
		public void Update()
		{
			Action[] array = null;
			object inbox = MainThreadComputationQueue.Inbox;
			lock (inbox)
			{
				if (MainThreadComputationQueue.Inbox.Count > 0)
				{
					array = MainThreadComputationQueue.Inbox.ToArray();
					MainThreadComputationQueue.Inbox.Clear();
				}
			}
			if (array != null)
			{
				foreach (Action action in array)
				{
					action();
				}
			}
		}

		// Token: 0x04004984 RID: 18820
		public static MainThreadComputationQueue Shared;

		// Token: 0x04004985 RID: 18821
		private static readonly Queue<Action> Inbox = new Queue<Action>();
	}
}
