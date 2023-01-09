using System;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using TMPro;
using UnityEngine;

// Token: 0x020006D8 RID: 1752
namespace Match3.Scripts1
{
	public class MaterialAmountDisplayLabel : AMaterialAmountDisplay, IMaterialAmountDisplay
	{
		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06002B96 RID: 11158 RVA: 0x000C86C0 File Offset: 0x000C6AC0
		protected AudioService AudioService
		{
			get
			{
				if (this._audioService == null)
				{
					TownResourcePanelRoot[] componentsInParent = base.GetComponentsInParent<TownResourcePanelRoot>(true);
					TownResourcePanelRoot townResourcePanelRoot;
					if (componentsInParent != null && componentsInParent.Length > 0)
					{
						townResourcePanelRoot = componentsInParent[0];
					}
					else
					{
						townResourcePanelRoot = global::UnityEngine.Object.FindObjectOfType<TownResourcePanelRoot>();
					}
					if (townResourcePanelRoot != null)
					{
						this._audioService = townResourcePanelRoot.audioService;
					}
				}
				return this._audioService;
			}
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x000C871E File Offset: 0x000C6B1E
		public void InjectAudioService(AudioService audioService)
		{
			this._audioService = audioService;
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06002B98 RID: 11160 RVA: 0x000C8727 File Offset: 0x000C6B27
		private int displayedValue
		{
			get
			{
				return Mathf.Max(0, this.currentValue - this.pendingValue);
			}
		}

		// Token: 0x06002B99 RID: 11161 RVA: 0x000C873C File Offset: 0x000C6B3C
		public void SetValue(int value)
		{
			this.currentValue = value;
			this.UpdateTextLabel();
		}

		// Token: 0x06002B9A RID: 11162 RVA: 0x000C874C File Offset: 0x000C6B4C
		private void UpdateTextLabel()
		{
			if (this.label != null)
			{
				this.label.text = this.displayedValue.ToString();
			}
		}

		// Token: 0x06002B9B RID: 11163 RVA: 0x000C8789 File Offset: 0x000C6B89
		public void ReserveAmount(int value)
		{
			if (!this.allowReserving)
			{
				return;
			}
			this.pendingValue += value;
			if (this.updateViewOnReserve)
			{
				this.UpdateTextLabel();
			}
		}

		// Token: 0x06002B9C RID: 11164 RVA: 0x000C87B8 File Offset: 0x000C6BB8
		public void AcceptDoober(int amount)
		{
			this.pendingValue -= amount;
			this.UpdateTextLabel();
			if (this.AudioService != null)
			{
				this.AudioService.PlaySFX(this.dooberSound, false, false, false);
			}
			if (this.animator)
			{
				this.animator.SetTrigger(this.DooberArrived);
			}
		}

		// Token: 0x040054AC RID: 21676
		private int currentValue;

		// Token: 0x040054AD RID: 21677
		private int pendingValue;

		// Token: 0x040054AE RID: 21678
		private readonly int DooberArrived = Animator.StringToHash("DooberArrived");

		// Token: 0x040054AF RID: 21679
		public AudioId dooberSound;

		// Token: 0x040054B0 RID: 21680
		public bool updateViewOnReserve = true;

		// Token: 0x040054B1 RID: 21681
		[NonSerialized]
		public bool allowReserving = true;

		// Token: 0x040054B2 RID: 21682
		[SerializeField]
		private Animator animator;

		// Token: 0x040054B3 RID: 21683
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x040054B4 RID: 21684
		private AudioService _audioService;
	}
}
