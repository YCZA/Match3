using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A62 RID: 2658
	[RequireComponent(typeof(TimeSpanView))]
	public class TournamentTimer : MonoBehaviour
	{
		// Token: 0x06003FAB RID: 16299 RVA: 0x00146463 File Offset: 0x00144863
		private void Start()
		{
			this.timeSpanView = base.GetComponent<TimeSpanView>();
			WooroutineRunner.StartCoroutine(this.StartRoutine(), null);
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x0014647E File Offset: 0x0014487E
		private void OnEnable()
		{
			this.AddListener();
		}

		// Token: 0x06003FAD RID: 16301 RVA: 0x00146486 File Offset: 0x00144886
		private void OnDisable()
		{
			this.RemoveListener();
		}

		// Token: 0x06003FAE RID: 16302 RVA: 0x0014648E File Offset: 0x0014488E
		private void RemoveListener()
		{
			if (this.setup && this.service != null)
			{
				this.service.ticker.onRemainingSecondsChanged.RemoveListener(new Action<string, TimeSpan>(this.CheckForUpdate));
			}
		}

		// Token: 0x06003FAF RID: 16303 RVA: 0x001464C8 File Offset: 0x001448C8
		private IEnumerator StartRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			if (this.shouldSignUpAfterServiceAppears)
			{
				this.shouldSignUpAfterServiceAppears = false;
				this.AddListener();
			}
			yield break;
		}

		// Token: 0x06003FB0 RID: 16304 RVA: 0x001464E3 File Offset: 0x001448E3
		public void Setup(string leagueID, TimeSpan timeLeft)
		{
			this.leagueID = leagueID;
			this.UpdateTimer(timeLeft);
			this.setup = true;
			this.AddListener();
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x00146500 File Offset: 0x00144900
		private void AddListener()
		{
			if (this.setup && this.service != null && base.enabled)
			{
				this.service.ticker.onRemainingSecondsChanged.AddListener(new Action<string, TimeSpan>(this.CheckForUpdate));
			}
			else
			{
				this.shouldSignUpAfterServiceAppears = true;
			}
		}

		// Token: 0x06003FB2 RID: 16306 RVA: 0x0014655C File Offset: 0x0014495C
		protected void CheckForUpdate(string tournamentLeagueID, TimeSpan timeLeft)
		{
			if (tournamentLeagueID == this.leagueID && this != null && base.gameObject != null && base.gameObject.activeInHierarchy)
			{
				this.UpdateTimer(timeLeft);
			}
		}

		// Token: 0x06003FB3 RID: 16307 RVA: 0x001465B0 File Offset: 0x001449B0
		private string GetLocalizedEndText()
		{
			if (this.locaService != null)
			{
				string key = "ui.tournaments.timer.end.label";
				return this.locaService.GetText(key, new LocaParam[0]);
			}
			return string.Empty;
		}

		// Token: 0x06003FB4 RID: 16308 RVA: 0x001465E8 File Offset: 0x001449E8
		protected void UpdateTimer(TimeSpan timeSpan)
		{
			if (timeSpan > TimeSpan.Zero || !this.timeSpanView.singleField)
			{
				if (base.gameObject.activeInHierarchy)
				{
					base.StartCoroutine(this.timeSpanView.SetTimeSpan(timeSpan, false));
				}
			}
			else
			{
				this.timeSpanView.singleLabel.text = this.GetLocalizedEndText();
			}
		}

		// Token: 0x04006956 RID: 26966
		[WaitForService(true, true)]
		protected TournamentService service;

		// Token: 0x04006957 RID: 26967
		[WaitForService(true, true)]
		protected ILocalizationService locaService;

		// Token: 0x04006958 RID: 26968
		private TimeSpanView timeSpanView;

		// Token: 0x04006959 RID: 26969
		private string leagueID;

		// Token: 0x0400695A RID: 26970
		private bool shouldSignUpAfterServiceAppears;

		// Token: 0x0400695B RID: 26971
		private bool setup;
	}
}
