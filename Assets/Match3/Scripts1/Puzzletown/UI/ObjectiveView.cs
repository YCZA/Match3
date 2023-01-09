using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020006D3 RID: 1747
	public class ObjectiveView : MaterialAmountView, IMaterialAmountDisplay
	{
		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06002B7E RID: 11134 RVA: 0x000C8023 File Offset: 0x000C6423
		public bool IsComplete
		{
			get
			{
				return this.decremental && this.DisplayedValue <= 0;
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06002B7F RID: 11135 RVA: 0x000C803F File Offset: 0x000C643F
		protected AudioService AudioService
		{
			get
			{
				if (this._audioService == null)
				{
					this._audioService = base.GetComponentInParent<M3_ObjectivesUiRoot>().audioService;
				}
				return this._audioService;
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06002B80 RID: 11136 RVA: 0x000C8063 File Offset: 0x000C6463
		private int DisplayedValue
		{
			get
			{
				return this.currentValue + ((!this.decremental) ? (-this.pendingValue) : this.pendingValue);
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06002B81 RID: 11137 RVA: 0x000C808C File Offset: 0x000C648C
		private MaterialAmount DisplayedMaterialAmount
		{
			get
			{
				return new MaterialAmount(base.Data.type, this.DisplayedValue, MaterialAmountUsage.Undefined, 0);
			}
		}

		// Token: 0x06002B82 RID: 11138 RVA: 0x000C80B4 File Offset: 0x000C64B4
		public override void Refresh(MaterialAmount mat)
		{
			if (this.loc == null)
			{
				return;
			}
			if (base.gameObject == null)
			{
				return;
			}
			base.gameObject.SetActive(true);
			this.currentValue = mat.amount;
			this.RefreshAmount();
		}

		// Token: 0x06002B83 RID: 11139 RVA: 0x000C80F3 File Offset: 0x000C64F3
		public void ReserveAmount(int value)
		{
			this.pendingValue += value;
		}

		// Token: 0x06002B84 RID: 11140 RVA: 0x000C8104 File Offset: 0x000C6504
		private void RefreshAmount()
		{
			if (this.decremental)
			{
				this.label.gameObject.SetActive(!this.IsComplete);
				this.successfull.SetActive(this.IsComplete);
			}
			base.Refresh(this.DisplayedMaterialAmount);
			if (this.decremental)
			{
				if (this.IsComplete && !this.wasComplete)
				{
					this.AudioService.PlaySFX(AudioId.ObjectiveCompleted, false, false, false);
					this.wasComplete = true;
				}
				else
				{
					AudioId similar = this.AudioService.GetSimilar(base.Data.type + "Collected");
					this.AudioService.PlaySFX((similar == AudioId.Default) ? AudioId.GemCollected : similar, false, false, false);
				}
			}
		}

		// Token: 0x06002B85 RID: 11141 RVA: 0x000C81D7 File Offset: 0x000C65D7
		public void AcceptDoober(int amount)
		{
			this.pendingValue -= amount;
			this.RefreshAmount();
			if (this.animator)
			{
				this.animator.SetTrigger(this.DooberArrived);
			}
		}

		// Token: 0x04005496 RID: 21654
		private readonly int DooberArrived = Animator.StringToHash("DooberArrived");

		// Token: 0x04005497 RID: 21655
		[SerializeField]
		private GameObject successfull;

		// Token: 0x04005498 RID: 21656
		[SerializeField]
		private Animator animator;

		// Token: 0x04005499 RID: 21657
		[SerializeField]
		private bool decremental;

		// Token: 0x0400549A RID: 21658
		private int currentValue;

		// Token: 0x0400549B RID: 21659
		private int pendingValue;

		// Token: 0x0400549C RID: 21660
		private AudioService _audioService;

		// Token: 0x0400549D RID: 21661
		private bool wasComplete;
	}
}
