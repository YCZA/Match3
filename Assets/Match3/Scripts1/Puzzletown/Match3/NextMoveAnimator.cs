using DG.Tweening;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200067C RID: 1660
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Board/NextMoveAnimator")]
	public class NextMoveAnimator : AAnimator<MatchCandidate>
	{
		// Token: 0x06002976 RID: 10614 RVA: 0x000BB4E0 File Offset: 0x000B98E0
		protected override void DoAppend(MatchCandidate candidate)
		{
			IntVector2 target = candidate.target;
			int count = candidate.Group.Count;
			IntVector2 start = candidate.start;
			float duration = base.ModifiedDuration / 2f;
			for (int i = 0; i < count; i++)
			{
				if (candidate.Group[i].position != target)
				{
					this.HighlightGem(candidate.Group[i].position, duration);
				}
			}
			this.HighlightGem(start, duration);
			if (count == 1)
			{
				this.HighlightGem(target, duration);
			}
			GemView gemView = base.GetGemView(start, true);
			Vector3 a = (Vector3)(target - start);
			a.Normalize();
			this.sequence.Insert(0f, gemView.sprite.transform.DOPunchPosition(a * this.shortenDir, base.ModifiedDuration, 0, 0f, false));
		}

		// Token: 0x06002977 RID: 10615 RVA: 0x000BB5DC File Offset: 0x000B99DC
		private void HighlightGem(IntVector2 pos, float duration)
		{
			GemView gemView = base.GetGemView(pos, true);
			gemView.SwitchToHighlightMaterial();
			this.sequence.InsertCallback(duration, delegate
			{
				gemView.UpdateMaterial();
			});
		}

		// Token: 0x0400530A RID: 21258
		public float shortenDir = 0.3f;
	}
}
