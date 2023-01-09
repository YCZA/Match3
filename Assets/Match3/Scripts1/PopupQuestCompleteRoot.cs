using System.Collections;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Puzzletown.UI;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000848 RID: 2120
namespace Match3.Scripts1
{
	public class PopupQuestCompleteRoot : APtSceneRoot, IDisposableDialog, IHandler<PopupOperation>
	{
		// Token: 0x06003483 RID: 13443 RVA: 0x000FB550 File Offset: 0x000F9950
		public void Show(QuestData quest)
		{
			this.quest = quest;
			string key = "quest.completed." + quest.id;
			string text = this.localizationService.GetText(key, new LocaParam[0]);
			this.questCompleteMessage.text = text;
			this.rewardMaterials = new MaterialAmount[]
			{
				new MaterialAmount(this.quest.rewardItem, this.quest.rewardCount, MaterialAmountUsage.Undefined, 0)
			};
			this.rewards.Show(this.rewardMaterials);
			base.StartCoroutine(this.LoadAssetsRoutine());
			this.TrackFunnel(quest);
		}

		// Token: 0x06003484 RID: 13444 RVA: 0x000FB5F0 File Offset: 0x000F99F0
		private IEnumerator LoadAssetsRoutine()
		{
			Wooroutine<Sprite> spriteFlow = new QuestIllustrationSpriteFlow().Start(this.quest);
			yield return spriteFlow;
			if (spriteFlow.ReturnValue != null)
			{
				this.illustration.sprite = spriteFlow.ReturnValue;
			}
			this.dialog.Show();
			this.canvasGroup.interactable = true;
			this.audioService.PlaySFX(AudioId.PopupQuestCompleted, false, false, false);
			yield break;
		}

		// Token: 0x06003485 RID: 13445 RVA: 0x000FB60C File Offset: 0x000F9A0C
		private void TrackFunnel(QuestData quest)
		{
			if (quest.id == "chp1_q1")
			{
				this.trackingService.TrackFunnelEvent("290_q1_job_done", 290, null);
			}
			else if (quest.id == "chp2_q1")
			{
				this.trackingService.TrackFunnelEvent("500_q2_job_done", 500, null);
			}
		}

		// Token: 0x06003486 RID: 13446 RVA: 0x000FB674 File Offset: 0x000F9A74
		private void CollectAndClose()
		{
			this.dialog.Hide();
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
		}

		// Token: 0x06003487 RID: 13447 RVA: 0x000FB695 File Offset: 0x000F9A95
		public void Handle(PopupOperation op)
		{
			if (op == PopupOperation.Close)
			{
				this.canvasGroup.interactable = false;
				this.CollectAndClose();
			}
		}

		// Token: 0x04005C75 RID: 23669
		[WaitForRoot(false, false)]
		private TownUiRoot townUI;

		// Token: 0x04005C76 RID: 23670
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x04005C77 RID: 23671
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005C78 RID: 23672
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04005C79 RID: 23673
		private const string FULL_IMAGE_POSTFIX = "_0";

		// Token: 0x04005C7A RID: 23674
		public MaterialsDataSource rewards;

		// Token: 0x04005C7B RID: 23675
		public TMP_Text questCompleteMessage;

		// Token: 0x04005C7C RID: 23676
		public AnimatedUi dialog;

		// Token: 0x04005C7D RID: 23677
		public CanvasGroup canvasGroup;

		// Token: 0x04005C7E RID: 23678
		public Image illustration;

		// Token: 0x04005C7F RID: 23679
		[WaitForService(true, true)]
		private GameStateService gameState;

		// Token: 0x04005C80 RID: 23680
		private MaterialAmount[] rewardMaterials;

		// Token: 0x04005C81 RID: 23681
		private QuestData quest;
	}
}
