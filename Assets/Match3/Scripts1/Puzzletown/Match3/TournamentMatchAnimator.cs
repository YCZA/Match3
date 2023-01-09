using System.Collections;
using Match3.Scripts1.Puzzletown.Config;
using Shared.Pooling;
using Wooga.Coroutines;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000689 RID: 1673
	[CreateAssetMenu(menuName = "Puzzletown/Animators/TournamentScoreAnimator")]
	public class TournamentMatchAnimator : AAnimator<ITournamentScoreMatch>
	{
		// Token: 0x060029A0 RID: 10656 RVA: 0x000BD198 File Offset: 0x000BB598
		protected override void DoAppend(ITournamentScoreMatch matchResult)
		{
			WooroutineRunner.StartCoroutine(this.ShowTournamentItemCollectedDoober(matchResult.ReleasePosition, matchResult.Type), null);
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x000BD1B4 File Offset: 0x000BB5B4
		private IEnumerator ShowTournamentItemCollectedDoober(Vector3 startPosition, TournamentType type)
		{
			TournamentItemCollectedView item = this.boardView.GetTournamentCollectedItemView(startPosition, type);
			this.boardView.onTournamentItemCollected.Dispatch(item);
			yield return null;
			item.gameObject.Release();
			yield break;
		}
	}
}
