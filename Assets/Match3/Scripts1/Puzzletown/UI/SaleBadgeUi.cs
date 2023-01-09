using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A19 RID: 2585
	public class SaleBadgeUi : MonoBehaviour
	{
		// Token: 0x06003E2A RID: 15914 RVA: 0x0013B336 File Offset: 0x00139736
		public void Init(TownBottomPanelRoot townBottomPanel)
		{
			WooroutineRunner.StartCoroutine(this.SetupRoutine(townBottomPanel), null);
		}

		// Token: 0x06003E2B RID: 15915 RVA: 0x0013B348 File Offset: 0x00139748
		private IEnumerator SetupRoutine(TownBottomPanelRoot tBPRoot)
		{
			this.townBottomPanelRoot = tBPRoot;
			if (!this.initialized)
			{
				yield return ServiceLocator.Instance.Inject(this);
				this.initialized = true;
			}
			this.Refresh();
			this.AddSlowUpdate(new SlowUpdate(this.Refresh), 3);
			yield break;
		}

		// Token: 0x06003E2C RID: 15916 RVA: 0x0013B36C File Offset: 0x0013976C
		private void Refresh()
		{
			Sale currentSale = this.saleService.CurrentSale;
			if (currentSale != null)
			{
				DateTime lowDateTime = currentSale.GetLowDateTime(this.configService.general.notifications.low_time_event);
				this.timer.SetTargetTime(currentSale.endDateLocal, lowDateTime, false, null);
				this.icon.sprite = this.spriteManager.GetSimilar(currentSale.iconName);
			}
			this.container.SetActive(currentSale != null);
		}

		// Token: 0x06003E2D RID: 15917 RVA: 0x0013B3E9 File Offset: 0x001397E9
		public void OnButtonTap()
		{
			if (this.townBottomPanelRoot == null || !this.townBottomPanelRoot.IsInteractable)
			{
				return;
			}
			SceneManager.Instance.LoadSceneWithParams<SalePopupRoot, string>("player_triggered", null);
		}

		// Token: 0x04006711 RID: 26385
		[SerializeField]
		private GameObject container;

		// Token: 0x04006712 RID: 26386
		[SerializeField]
		private CountdownTimer timer;

		// Token: 0x04006713 RID: 26387
		[SerializeField]
		private Image icon;

		// Token: 0x04006714 RID: 26388
		[SerializeField]
		private SpriteManager spriteManager;

		// Token: 0x04006715 RID: 26389
		[WaitForService(true, true)]
		private SaleService saleService;

		// Token: 0x04006716 RID: 26390
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04006717 RID: 26391
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04006718 RID: 26392
		private bool initialized;

		// Token: 0x04006719 RID: 26393
		private TownBottomPanelRoot townBottomPanelRoot;
	}
}
