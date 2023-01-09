using System;
using DG.Tweening;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020006D5 RID: 1749
namespace Match3.Scripts1
{
	public class MaterialAmountDisplayGauge : AMaterialAmountDisplay, IMaterialAmountDisplay
	{
		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06002B8A RID: 11146 RVA: 0x000C8498 File Offset: 0x000C6898
		protected AudioService AudioService
		{
			get
			{
				if (this._audioService == null)
				{
					this._audioService = base.GetComponentsInParent<TownResourcePanelRoot>(true)[0].audioService;
				}
				return this._audioService;
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06002B8B RID: 11147 RVA: 0x000C84BF File Offset: 0x000C68BF
		private int displayedValue
		{
			get
			{
				return this.currentValue - this.pendingValue;
			}
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x000C84CE File Offset: 0x000C68CE
		public void SetVillageRankHarmonyObserver(VillageRankHarmonyObserver villageRankHarmonyObserver)
		{
			this.villageRankHarmonyObserver = villageRankHarmonyObserver;
		}

		// Token: 0x06002B8D RID: 11149 RVA: 0x000C84D7 File Offset: 0x000C68D7
		public void SetValue(int value)
		{
			this.currentValue = value;
			this.UpdateProgress();
		}

		// Token: 0x06002B8E RID: 11150 RVA: 0x000C84E8 File Offset: 0x000C68E8
		private void UpdateProgress()
		{
			if (this.gauge)
			{
				this.gauge.DOKill(false);
				float fillAmount = this.GetFillAmount();
				if (this.currentFillAmount > fillAmount)
				{
					this.gauge.fillAmount = fillAmount;
				}
				else
				{
					this.gauge.DOFillAmount(fillAmount, 0.2f);
				}
				this.currentFillAmount = fillAmount;
			}
			if (this.rabkLabel)
			{
				VillageRank villageRank = this.villageRankHarmonyObserver.RankForHarmony(this.displayedValue);
				this.rabkLabel.text = villageRank.village_rank.ToString();
			}
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x000C8590 File Offset: 0x000C6990
		private float GetFillAmount()
		{
			VillageRank villageRank = this.villageRankHarmonyObserver.RankForHarmony(this.displayedValue);
			VillageRank villageRank2 = this.villageRankHarmonyObserver.NextRank(villageRank);
			int harmony_goal = villageRank.harmony_goal;
			int harmony_goal2 = villageRank2.harmony_goal;
			return (float)(this.displayedValue - harmony_goal) / (float)(harmony_goal2 - harmony_goal);
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x000C85D8 File Offset: 0x000C69D8
		public void ReserveAmount(int value)
		{
			if (!this.allowReserving)
			{
				return;
			}
			this.pendingValue += value;
			this.UpdateProgress();
		}

		// Token: 0x06002B91 RID: 11153 RVA: 0x000C85FC File Offset: 0x000C69FC
		public void AcceptDoober(int amount)
		{
			this.pendingValue -= amount;
			this.UpdateProgress();
			VillageRank villageRank = this.villageRankHarmonyObserver.RankForHarmony(this.displayedValue);
			if (this.currentRank == null)
			{
				this.currentRank = villageRank;
			}
			else if (villageRank.village_rank > this.currentRank.village_rank)
			{
				this.currentRank = villageRank;
			}
			this.AudioService.PlaySFX(this.dooberSound, false, false, false);
			if (this.animator)
			{
				this.animator.SetTrigger(this.DooberArrived);
			}
		}

		// Token: 0x0400549E RID: 21662
		private int currentValue;

		// Token: 0x0400549F RID: 21663
		private int pendingValue;

		// Token: 0x040054A0 RID: 21664
		private VillageRank currentRank;

		// Token: 0x040054A1 RID: 21665
		private float currentFillAmount = 1f;

		// Token: 0x040054A2 RID: 21666
		public VillageRankHarmonyObserver villageRankHarmonyObserver;

		// Token: 0x040054A3 RID: 21667
		private readonly int DooberArrived = Animator.StringToHash("DooberArrived");

		// Token: 0x040054A4 RID: 21668
		public AudioId dooberSound;

		// Token: 0x040054A5 RID: 21669
		[NonSerialized]
		public bool allowReserving = true;

		// Token: 0x040054A6 RID: 21670
		[SerializeField]
		private Animator animator;

		// Token: 0x040054A7 RID: 21671
		[SerializeField]
		private Image gauge;

		// Token: 0x040054A8 RID: 21672
		[SerializeField]
		private TextMeshProUGUI rabkLabel;

		// Token: 0x040054A9 RID: 21673
		private AudioService _audioService;
	}
}
