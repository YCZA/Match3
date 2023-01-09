using System;
using System.Collections;
using System.Collections.Generic;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000755 RID: 1877
	public class BlockerManager
	{
		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06002E79 RID: 11897 RVA: 0x000D9637 File Offset: 0x000D7A37
		private EventSystemRoot eventSystem
		{
			get
			{
				if (this.m_eventSystem == null)
				{
					this.m_eventSystem = global::UnityEngine.Object.FindObjectOfType<EventSystemRoot>();
				}
				return this.m_eventSystem;
			}
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x000D965C File Offset: 0x000D7A5C
		private IEnumerator Init()
		{
			yield return SceneManager.Instance.Inject(this);
			yield break;
		}

		// Token: 0x06002E7B RID: 11899 RVA: 0x000D9677 File Offset: 0x000D7A77
		public static void Clear()
		{
			if (BlockerManager.global != null)
			{
				if (BlockerManager.global.currentlyRunningBlocker != null)
				{
					WooroutineRunner.Stop(BlockerManager.global.currentlyRunningBlocker);
				}
				Debug.Log("20220325清空");
				BlockerManager.global.m_blockers.Clear();
			}
		}

		// Token: 0x06002E7C RID: 11900 RVA: 0x000D96B0 File Offset: 0x000D7AB0
		public void Append(Func<IEnumerator> coroutine, bool blockInput = false)
		{
			this.Append(new CoroutineWrapper(coroutine, blockInput));
		}

		// Token: 0x06002E7D RID: 11901 RVA: 0x000D96C4 File Offset: 0x000D7AC4
		public void Append(IBlocker blocker)
		{
			// Debug.LogError("eventsystem:Append Blocker");			

			if (!this.HasBlockers)
			{
				// Debug.Log("20220325直接执行: " + m_blockers.Count + this.GetHashCode());
				this.m_blockers.Enqueue(blocker);
				// Debug.Log("20220325count: " + m_blockers.Count);
				WooroutineRunner.StartCoroutine(this.Start(), null);
				// Debug.Log("20220325count: " + m_blockers.Count);
			}
			else
			{
				// Debug.Log("20220325等待执行" + + this.GetHashCode());
				this.m_blockers.Enqueue(blocker);
			}
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x000D96FC File Offset: 0x000D7AFC
		private IEnumerator Start()
		{
			if (!this.initialized)
			{
				yield return this.Init();
				this.initialized = true;
			}
			while (this.HasBlockers)
			{
				IBlocker func = this.m_blockers.Peek();
				this.currentlyRunningBlocker = WooroutineRunner.StartCoroutine(func.ExecuteRoutine(), null);
				this.SetInputEnabled(!func.BlockInput);
				yield return this.currentlyRunningBlocker;
				this.SetInputEnabled(true);
				this.m_blockers.Dequeue();
				// Debug.Log("20220325执行完毕" + GetHashCode());
			}
			this.currentlyRunningBlocker = null;
			yield break;
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x000D9717 File Offset: 0x000D7B17
		private void SetInputEnabled(bool enabled)
		{
			if (this.eventSystem != null)
			{
				if (enabled)
				{
					this.eventSystem.Enable();
				}
				else
				{
					this.eventSystem.Disable();
				}
				BackButtonManager.Instance.SetEnabled(enabled);
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06002E80 RID: 11904 RVA: 0x000D9756 File Offset: 0x000D7B56
		public bool HasBlockers
		{
			get
			{
				return this.m_blockers.Count > 0;
			}
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06002E81 RID: 11905 RVA: 0x000D9766 File Offset: 0x000D7B66
		public IEnumerable<IBlocker> Blockers
		{
			get
			{
				return this.m_blockers;
			}
		}

		// Token: 0x040057C3 RID: 22467
		private readonly Queue<IBlocker> m_blockers = new Queue<IBlocker>();

		// Token: 0x040057C4 RID: 22468
		private Coroutine currentlyRunningBlocker;

		// Token: 0x040057C5 RID: 22469
		public static readonly BlockerManager global = new BlockerManager();

		// Token: 0x040057C6 RID: 22470
		private EventSystemRoot m_eventSystem;

		// Token: 0x040057C7 RID: 22471
		private bool initialized;
	}
}
