using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000880 RID: 2176
	[LoadOptions(true, false, false)]
	public class PopupWheelRewardRoot : APtSceneRoot<List<WheelPrize>>, IHandler<PopupOperation>, IDisposableDialog
	{
		// Token: 0x06003573 RID: 13683 RVA: 0x00100648 File Offset: 0x000FEA48
		protected override void Go()
		{
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			int i;
			for (i = 0; i < this.parameters.Count; i++)
			{
				this.rewardViews[i].ShowPrize(this.parameters[i]);
			}
			while (i < this.rewardViews.Count)
			{
				this.rewardViews[i].gameObject.SetActive(false);
				i++;
			}
		}

		// Token: 0x06003574 RID: 13684 RVA: 0x001006F8 File Offset: 0x000FEAF8
		public void Close()
		{
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			this.onClose.Dispatch();
		}

		// Token: 0x06003575 RID: 13685 RVA: 0x00100748 File Offset: 0x000FEB48
		private IEnumerator ClaimRewards()
		{
			float animationTime = 0f;
			float maxAnimationTime = 1f;
			for (int i = 0; i < this.parameters.Count; i++)
			{
				animationTime = this.HandleRewards(this.parameters[i], this.rewardViews[i].rewardImage.transform);
				maxAnimationTime = ((animationTime <= maxAnimationTime) ? maxAnimationTime : animationTime);
			}
			yield return new WaitForSeconds(maxAnimationTime);
			this.Close();
			yield break;
		}

		// Token: 0x06003576 RID: 13686 RVA: 0x00100764 File Offset: 0x000FEB64
		public float HandleRewards(WheelPrize prize, Transform dooberTransform)
		{
			float result = 0f;
			MaterialAmount mat = default(MaterialAmount);
			switch (prize.prizeType)
			{
			case AdSpinPrize.UnlimitedLives:
				this.livesService.StartUnlimitedLives(prize.amount / 60);
				result = 1f;
				break;
			case AdSpinPrize.Diamonds:
				this.gameStateService.Resources.AddMaterial("diamonds", prize.amount, true, "转盘");
				mat = new MaterialAmount("diamonds", prize.amount, MaterialAmountUsage.Undefined, 0);
				result = this.townUI.CollectMaterials(mat, dooberTransform, true);
				break;
			case AdSpinPrize.Coins:
				this.gameStateService.Resources.AddMaterial("coins", prize.amount, true, "转盘");
				mat = new MaterialAmount("coins", prize.amount, MaterialAmountUsage.Undefined, 0);
				result = this.townUI.CollectMaterials(mat, dooberTransform, true);
				break;
			case AdSpinPrize.Starboost:
				this.gameStateService.Resources.AddMaterial(Boosts.boost_star.ToString(), prize.amount, true, "转盘");
				mat = new MaterialAmount(Boosts.boost_star.ToString(), prize.amount, MaterialAmountUsage.Undefined, 0);
				result = this.townUI.CollectMaterials(mat, dooberTransform, true);
				break;
			case AdSpinPrize.HammerBoost:
				this.gameStateService.Resources.AddMaterial(Boosts.boost_hammer.ToString(), prize.amount, true, "转盘");
				mat = new MaterialAmount(Boosts.boost_hammer.ToString(), prize.amount, MaterialAmountUsage.Undefined, 0);
				result = this.townUI.CollectMaterials(mat, dooberTransform, true);
				break;
			}
			return result;
		}

		// Token: 0x06003577 RID: 13687 RVA: 0x00100903 File Offset: 0x000FED03
		public void Handle(PopupOperation evt)
		{
			base.StartCoroutine(this.ClaimRewards());
			this.claimButton.interactable = false;
		}

		// Token: 0x04005D55 RID: 23893
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005D56 RID: 23894
		[WaitForService(true, true)]
		private LivesService livesService;

		// Token: 0x04005D57 RID: 23895
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04005D58 RID: 23896
		[WaitForRoot(false, false)]
		private TownUiRoot townUI;

		// Token: 0x04005D59 RID: 23897
		public List<PopupWheelRewardView> rewardViews;

		// Token: 0x04005D5A RID: 23898
		public readonly AwaitSignal onClose = new AwaitSignal();

		// Token: 0x04005D5B RID: 23899
		public AnimatedUi dialog;

		// Token: 0x04005D5C RID: 23900
		public Button claimButton;

		// Token: 0x04005D5D RID: 23901
		public TextMeshProUGUI titleLable;
	}
}
