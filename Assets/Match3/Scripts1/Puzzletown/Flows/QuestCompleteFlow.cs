using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020004C5 RID: 1221
	public class QuestCompleteFlow : IBlocker
	{
		// Token: 0x06002235 RID: 8757 RVA: 0x00095352 File Offset: 0x00093752
		public QuestCompleteFlow(QuestData quest, bool shouldForceNewAreaBanner = false)
		{
			this.shouldForceNewAreaBanner = shouldForceNewAreaBanner;
			this.m_quest = quest;
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06002236 RID: 8758 RVA: 0x00095368 File Offset: 0x00093768
		public bool BlockInput
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x0009536C File Offset: 0x0009376C
		public IEnumerator ExecuteRoutine()
		{
			// 完成一组任务后，点领取
			Debug.Log("ExecuteRoutine");
			yield return SceneManager.Instance.Inject(this);
			this.townUiRoot.ShowUi(false);
			yield return ServiceLocator.Instance.Inject(this);
			this.adjust.TrackQuestComplete(this.m_quest.id);
			StoryController storyController = this.villagerRoot.storyController;
			int currentIsland = this.gameStateService.Buildings.CurrentIsland;
			Debug.Log("claim_button_tapped, id:" + m_quest.id);
			yield return storyController.StartAndAwaitStoryFlow(new BlockerManager(), DialogueTrigger.claim_button_tapped, this.m_quest.id);
			if (currentIsland != this.gameStateService.Buildings.CurrentIsland)
			{
				yield return SceneManager.Instance.Inject(this);
				yield return ServiceLocator.Instance.Inject(this);
			}
			this.townUiRoot.BottomPanel.Disable();
			yield return this.ShowQuestCompletePopup();
			yield return this.TryShowEndOfContentPopup();
			yield return this.TryShowDateLockedPopup();
			StartNewContentFlow newContent = new StartNewContentFlow(this.shouldForceNewAreaBanner, true);
			yield return newContent.ExecuteRoutine();
			if (this.townUiRoot == null)
			{
				yield return SceneManager.Instance.Inject(this);
			}
			this.townUiRoot.ShowUi(true);
			yield break;
		}

		// Token: 0x06002238 RID: 8760 RVA: 0x00095388 File Offset: 0x00093788
		private IEnumerator ShowQuestCompletePopup()
		{
			// 完成一组任务后，显示领奖界面
			Debug.Log("ShowQuestCompletePopup");
			Wooroutine<PopupQuestCompleteRoot> popupQuestComplete = SceneManager.Instance.LoadScene<PopupQuestCompleteRoot>(null);
			yield return popupQuestComplete;
			popupQuestComplete.ReturnValue.Show(this.m_quest);
			yield return popupQuestComplete.ReturnValue.onDestroyed;
			yield break;
		}

		// Token: 0x06002239 RID: 8761 RVA: 0x000953A4 File Offset: 0x000937A4
		private IEnumerator TryShowEndOfContentPopup()
		{
			// 领取界面点击领取后
			Debug.Log("TryShowEndOfContentPopup");
			bool notLockedByDate = !this.contentUnlockService.IsNextQuestLockedByDate();
			if (this.quests.questManager.IsLastQuest(this.m_quest.id) && notLockedByDate)
			{
				Wooroutine<BannerEndOfContentRoot> popup = SceneManager.Instance.LoadScene<BannerEndOfContentRoot>(null);
				yield return popup;
				yield return popup.ReturnValue.onDestroyed;
			}
			yield break;
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x000953C0 File Offset: 0x000937C0
		private IEnumerator TryShowDateLockedPopup()
		{
			DateTime? nextQuestUnlockDate = this.contentUnlockService.NextQuestUnlockDate();
			if (nextQuestUnlockDate != null)
			{
				Wooroutine<PopupComingSoonRoot> popup = SceneManager.Instance.LoadSceneWithParams<PopupComingSoonRoot, DateTime>(nextQuestUnlockDate.Value, null);
				yield return popup;
				yield return popup.ReturnValue.onDestroyed;
			}
			yield break;
		}

		// Token: 0x04004D9A RID: 19866
		[WaitForService(true, true)]
		private QuestService quests;

		// Token: 0x04004D9B RID: 19867
		[WaitForService(true, true)]
		private ConfigService configs;

		// Token: 0x04004D9C RID: 19868
		[WaitForService(true, true)]
		private IAdjustService adjust;

		// Token: 0x04004D9D RID: 19869
		[WaitForService(true, true)]
		private ContentUnlockService contentUnlockService;

		// Token: 0x04004D9E RID: 19870
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04004D9F RID: 19871
		[WaitForRoot(false, false)]
		private VillagersControllerRoot villagerRoot;

		// Token: 0x04004DA0 RID: 19872
		[WaitForRoot(false, false)]
		private TownUiRoot townUiRoot;

		// Token: 0x04004DA1 RID: 19873
		private readonly QuestData m_quest;

		// Token: 0x04004DA2 RID: 19874
		private bool shouldForceNewAreaBanner;
	}
}
