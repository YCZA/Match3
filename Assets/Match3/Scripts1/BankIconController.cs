using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;

// Token: 0x02000978 RID: 2424
namespace Match3.Scripts1
{
	public class BankIconController : MonoBehaviour
	{
		// Token: 0x06003B1C RID: 15132 RVA: 0x0012484D File Offset: 0x00122C4D
		public void Init()
		{
			WooroutineRunner.StartCoroutine(this.SetupRoutine(), null);
		}

		// Token: 0x06003B1D RID: 15133 RVA: 0x0012485C File Offset: 0x00122C5C
		private IEnumerator SetupRoutine()
		{
			if (!this.initialized)
			{
				yield return ServiceLocator.Instance.Inject(this);
				this.initialized = true;
			}
			this.Refresh();
			this.AddSlowUpdate(new SlowUpdate(this.Refresh), 3);
			yield break;
		}

		// Token: 0x06003B1E RID: 15134 RVA: 0x00124878 File Offset: 0x00122C78
		private void Refresh()
		{
			if (this.bankService.BankFeatureEnabled())
			{
				base.gameObject.SetActive(true);
				bool active = this.NotificationVisible();
				this.badge.SetActive(active);
				bool flag = this.bankService.EventABTestEnabled();
				if (flag)
				{
					this.timer.SetTargetTime(this.gameState.Bank.EndTime, false, null);
				}
				else
				{
					this.amountLabel.text = string.Format("{0}/{1}", this.gameState.Bank.NumberOfBankedDiamonds, this.sbsService.SbsConfig.bank.balancing.full_amount);
				}
				this.timer.gameObject.SetActive(flag);
				this.AmountContainer.SetActive(!flag);
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x06003B1F RID: 15135 RVA: 0x00124964 File Offset: 0x00122D64
		private bool NotificationVisible()
		{
			int numberOfDiamondsLastSeen = this.gameState.Bank.NumberOfDiamondsLastSeen;
			int numberOfBankedDiamonds = this.gameState.Bank.NumberOfBankedDiamonds;
			int open_threshold = this.sbsService.SbsConfig.bank.balancing.open_threshold;
			int full_amount = this.sbsService.SbsConfig.bank.balancing.full_amount;
			if (numberOfBankedDiamonds < open_threshold)
			{
				return false;
			}
			bool flag = numberOfDiamondsLastSeen < open_threshold;
			bool flag2 = numberOfDiamondsLastSeen < full_amount && numberOfBankedDiamonds == full_amount;
			if (flag || flag2)
			{
				return true;
			}
			bool result = true;
			if (this.gameState.IsSeenFlagTimestampSet("bankSeen"))
			{
				DateTime t = this.gameState.GetTimeStamp("bankSeen").AddSeconds((double)this.configService.general.notifications.attention_indicator_cooldown);
				result = (DateTime.UtcNow > t);
			}
			return result;
		}

		// Token: 0x040062F5 RID: 25333
		[WaitForService(true, true)]
		private GameStateService gameState;

		// Token: 0x040062F6 RID: 25334
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x040062F7 RID: 25335
		[WaitForService(true, true)]
		private BankService bankService;

		// Token: 0x040062F8 RID: 25336
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x040062F9 RID: 25337
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x040062FA RID: 25338
		[SerializeField]
		private TextMeshProUGUI amountLabel;

		// Token: 0x040062FB RID: 25339
		[SerializeField]
		private GameObject badge;

		// Token: 0x040062FC RID: 25340
		[Header("Time Limited Event")]
		[SerializeField]
		private CountdownTimer timer;

		// Token: 0x040062FD RID: 25341
		[SerializeField]
		private GameObject AmountContainer;

		// Token: 0x040062FE RID: 25342
		private bool initialized;
	}
}
