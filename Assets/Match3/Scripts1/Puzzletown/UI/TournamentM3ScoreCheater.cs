using System.Collections;
using System.Diagnostics;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A51 RID: 2641
	public class TournamentM3ScoreCheater : MonoBehaviour
	{
		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06003F3E RID: 16190 RVA: 0x00143320 File Offset: 0x00141720
		private ScoringController ScoringController
		{
			get
			{
				ScoringController result;
				if ((result = this.scoringController) == null)
				{
					result = (this.scoringController = global::UnityEngine.Object.FindObjectOfType<LevelLoader>().ScoringController);
				}
				return result;
			}
		}

		// Token: 0x06003F3F RID: 16191 RVA: 0x0014334D File Offset: 0x0014174D
		private void Start()
		{
			base.StartCoroutine(this.StartRoutine());
		}

		// Token: 0x06003F40 RID: 16192 RVA: 0x0014335C File Offset: 0x0014175C
		private void Update()
		{
			if (this.doubleTapStopWatch.ElapsedMilliseconds > 800L)
			{
				this.doubleTapStopWatch.Stop();
				this.doubleTapStopWatch.Reset();
			}
		}

		// Token: 0x06003F41 RID: 16193 RVA: 0x0014338C File Offset: 0x0014178C
		private IEnumerator StartRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			this.canCheat = this.gameStateService.Debug.ShowCheatMenus;
			yield break;
		}

		// Token: 0x06003F42 RID: 16194 RVA: 0x001433A8 File Offset: 0x001417A8
		public void OnButtonTap()
		{
			if (this.canCheat && this.ScoringController != null)
			{
				long elapsedMilliseconds = this.doubleTapStopWatch.ElapsedMilliseconds;
				this.doubleTapStopWatch.Stop();
				this.doubleTapStopWatch.Reset();
				if (elapsedMilliseconds > 0L && elapsedMilliseconds < 800L)
				{
					this.ScoringController.CheatIncreaseTournamentScore();
				}
				else
				{
					this.doubleTapStopWatch.Start();
				}
			}
		}

		// Token: 0x040068BC RID: 26812
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x040068BD RID: 26813
		private bool canCheat;

		// Token: 0x040068BE RID: 26814
		private Stopwatch doubleTapStopWatch = new Stopwatch();

		// Token: 0x040068BF RID: 26815
		private ScoringController scoringController;
	}
}
