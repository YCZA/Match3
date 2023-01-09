using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Town;
using Wooga.Coroutines;
using UnityEngine;

// Token: 0x0200072E RID: 1838
namespace Match3.Scripts1
{
	public class PopupManager : MonoBehaviour
	{
		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06002D91 RID: 11665 RVA: 0x000D4225 File Offset: 0x000D2625
		// (set) Token: 0x06002D92 RID: 11666 RVA: 0x000D422D File Offset: 0x000D262D
		public bool SkipTriggers
		{
			get
			{
				return this.skipTriggers;
			}
			set
			{
				this.skipTriggers = value;
			}
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x000D4236 File Offset: 0x000D2636
		public void AddTrigger(PopupManager.Trigger trigger)
		{
			this.triggers.Add(trigger);
		}

		// Token: 0x06002D94 RID: 11668 RVA: 0x000D4244 File Offset: 0x000D2644
		public Coroutine Run(TownUiRoot townUi)
		{
			this.townUiRoot = townUi;
			this.runRoutine = WooroutineRunner.StartCoroutine(this.RunRoutine(), null);
			return this.runRoutine;
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x000D4265 File Offset: 0x000D2665
		public void Stop()
		{
			if (this.runRoutine != null)
			{
				WooroutineRunner.Stop(this.runRoutine);
				this.AllowTownBottomPanelInteraction(true);
			}
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x000D4284 File Offset: 0x000D2684
		private void AllowTownBottomPanelInteraction(bool state)
		{
			if (this.townUiRoot != null)
			{
				this.townUiRoot.AllowUIInteraction(state);
			}
		}

		// Token: 0x06002D97 RID: 11671 RVA: 0x000D42A4 File Offset: 0x000D26A4
		private IEnumerator RunRoutine()
		{
			this.AllowTownBottomPanelInteraction(false);
			foreach (PopupManager.Trigger trigger in this.triggers)
			{
				if (!this.skipTriggers && trigger.ShouldTrigger())
				{
					yield return trigger.Run();
				}
			}
			this.AllowTownBottomPanelInteraction(true);
			yield break;
		}

		// Token: 0x0400572A RID: 22314
		private List<PopupManager.Trigger> triggers = new List<PopupManager.Trigger>();

		// Token: 0x0400572B RID: 22315
		private Coroutine runRoutine;

		// Token: 0x0400572C RID: 22316
		private TownUiRoot townUiRoot;

		// Token: 0x0400572D RID: 22317
		private bool skipTriggers;

		// Token: 0x0200072F RID: 1839
		public abstract class Trigger
		{
			// Token: 0x06002D99 RID: 11673
			public abstract bool ShouldTrigger();

			// Token: 0x06002D9A RID: 11674
			public abstract IEnumerator Run();
		}
	}
}
