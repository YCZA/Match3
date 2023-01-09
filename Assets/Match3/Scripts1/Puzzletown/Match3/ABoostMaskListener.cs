using System;
using System.Collections;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000716 RID: 1814
	public abstract class ABoostMaskListener : MonoBehaviour, IBoostMaskListener
	{
		// Token: 0x06002CEB RID: 11499 RVA: 0x000D0640 File Offset: 0x000CEA40
		protected virtual IEnumerator Start()
		{
			yield return ServiceLocator.Instance.Inject(this);
			yield return SceneManager.Instance.Inject(this);
			if (this.boostOverlayController == null)
			{
				this.Init(this.boostsUiRoot.BoostOverlayController);
			}
			this.TryShowCurrentState();
			yield break;
		}

		// Token: 0x06002CEC RID: 11500 RVA: 0x000D065B File Offset: 0x000CEA5B
		protected virtual void OnDestroy()
		{
			this.RemoveListener();
		}

		// Token: 0x06002CED RID: 11501 RVA: 0x000D0663 File Offset: 0x000CEA63
		public virtual void Init(BoostOverlayController overlayController)
		{
			this.boostOverlayController = overlayController;
			if (overlayController.IsEnabled)
			{
				this.AddListener();
			}
		}

		// Token: 0x06002CEE RID: 11502 RVA: 0x000D0680 File Offset: 0x000CEA80
		protected void TryShowCurrentState()
		{
			if (this.boostOverlayController != null && !this.boostOverlayController.IsBoostOverlayPending)
			{
				Boosts currentSelectedBoost = this.boostOverlayController.CurrentSelectedBoost;
				this.HandleBoostOverlayStateChanged(new BoostOverlayState(currentSelectedBoost != Boosts.invalid, false, currentSelectedBoost));
			}
			if (this.boostOverlayController == null || !this.boostOverlayController.IsEnabled)
			{
				this.HandleBoostOverlayStateChanged(new BoostOverlayState(false, false, Boosts.invalid));
			}
		}

		// Token: 0x06002CEF RID: 11503 RVA: 0x000D06F1 File Offset: 0x000CEAF1
		protected void AddListener()
		{
			if (this.boostOverlayController != null)
			{
				this.boostOverlayController.onBoostOverlayStateChanged.AddListener(new Action<BoostOverlayState>(this.HandleBoostOverlayStateChanged));
			}
		}

		// Token: 0x06002CF0 RID: 11504 RVA: 0x000D071B File Offset: 0x000CEB1B
		protected virtual void RemoveListener()
		{
			if (this.boostOverlayController != null)
			{
				this.boostOverlayController.onBoostOverlayStateChanged.RemoveListener(new Action<BoostOverlayState>(this.HandleBoostOverlayStateChanged));
			}
		}

		// Token: 0x06002CF1 RID: 11505
		public abstract void HandleBoostOverlayStateChanged(BoostOverlayState newState);

		// Token: 0x04005674 RID: 22132
		public static float BOOST_OVERLAY_OPACITY = 0.6f;

		// Token: 0x04005675 RID: 22133
		protected BoostOverlayController boostOverlayController;

		// Token: 0x04005676 RID: 22134
		[WaitForRoot(true, false)]
		protected BoostsUiRoot boostsUiRoot;
	}
}
