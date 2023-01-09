using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A96 RID: 2710
	public class ConditionalHide : MonoBehaviour
	{
		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x06004085 RID: 16517 RVA: 0x0014E2DB File Offset: 0x0014C6DB
		// (set) Token: 0x06004086 RID: 16518 RVA: 0x0014E2E3 File Offset: 0x0014C6E3
		public bool IsActive { get; protected set; }

		// Token: 0x06004087 RID: 16519 RVA: 0x0014E2EC File Offset: 0x0014C6EC
		public void GetVisibilityForProgress()
		{
			WooroutineRunner.StartCoroutine(this.GetVisibilityForProgressRoutine(), null);
		}

		// Token: 0x06004088 RID: 16520 RVA: 0x0014E2FB File Offset: 0x0014C6FB
		protected void Start()
		{
			base.gameObject.SetActive(false);
			this.GetVisibilityForProgress();
		}

		// Token: 0x06004089 RID: 16521 RVA: 0x0014E310 File Offset: 0x0014C710
		private void OnDestroy()
		{
			if (this.questManager != null)
			{
				this.questManager.OnQuestCollected.RemoveListener(new Action<QuestProgress>(this.HandleQuestCollected));
			}
			if (this.progression != null)
			{
				this.progression.onLevelUnlocked.RemoveListener(new Action<int>(this.HandleLevelChange));
			}
		}

		// Token: 0x0600408A RID: 16522 RVA: 0x0014E36C File Offset: 0x0014C76C
		private IEnumerator GetVisibilityForProgressRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			this.IsActive = (this.progression.UnlockedLevel >= this.level);
			this.progression.onLevelUnlocked.AddListener(new Action<int>(this.HandleLevelChange));
			if (!this.questId.IsNullOrEmpty())
			{
				if (this.questManager == null)
				{
					this.questManager = this.quests.questManager;
					this.questManager.OnQuestCollected.AddListener(new Action<QuestProgress>(this.HandleQuestCollected));
				}
				this.IsActive &= ((!this.requireQuestCollected && this.quests.IsCompleted(this.questId)) || this.quests.IsCollected(this.questId));
			}
			yield return this.CheckBundleAvailabilityRoutine();
			base.gameObject.SetActive(this.IsActive);
			yield break;
		}

		// Token: 0x0600408B RID: 16523 RVA: 0x0014E388 File Offset: 0x0014C788
		private IEnumerator CheckBundleAvailabilityRoutine()
		{
			if (this.requiredBundles != null)
			{
				int i = 0;
				while (i < this.requiredBundles.Length && this.IsActive)
				{
					string bundle = this.requiredBundles[i];
					Wooroutine<bool> isAvailableRoutine = this.abs.IsBundleAvailable(bundle);
					yield return isAvailableRoutine;
					if (!isAvailableRoutine.ReturnValue)
					{
						this.IsActive = false;
					}
					i++;
				}
			}
			yield break;
		}

		// Token: 0x0600408C RID: 16524 RVA: 0x0014E3A4 File Offset: 0x0014C7A4
		private void HandleQuestCollected(QuestProgress progress)
		{
			QuestData configData = progress.configData;
			if (configData.id == this.questId)
			{
				this.IsActive = true;
				this.GetVisibilityForProgress();
			}
		}

		// Token: 0x0600408D RID: 16525 RVA: 0x0014E3DB File Offset: 0x0014C7DB
		private void HandleLevelChange(int newLevel)
		{
			if (newLevel >= this.level)
			{
				this.IsActive = true;
				this.GetVisibilityForProgress();
			}
		}

		// Token: 0x04006A28 RID: 27176
		[WaitForService(true, true)]
		private QuestService quests;

		// Token: 0x04006A29 RID: 27177
		[WaitForService(true, true)]
		private ProgressionDataService.Service progression;

		// Token: 0x04006A2A RID: 27178
		[WaitForService(true, true)]
		private AssetBundleService abs;

		// Token: 0x04006A2B RID: 27179
		public bool requireQuestCollected;

		// Token: 0x04006A2D RID: 27181
		public int level;

		// Token: 0x04006A2E RID: 27182
		public string questId;

		// Token: 0x04006A2F RID: 27183
		public string[] requiredBundles;

		// Token: 0x04006A30 RID: 27184
		private QuestManager questManager;
	}
}
