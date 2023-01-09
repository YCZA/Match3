using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000674 RID: 1652
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Board/HighlightObjectiveAnimator")]
	public class HighlightObjectiveAnimator : AAnimator<ObjectiveHighlights>
	{
		// Token: 0x0600294A RID: 10570 RVA: 0x000B9BA0 File Offset: 0x000B7FA0
		protected override void DoAppend(ObjectiveHighlights highlights)
		{
			foreach (Gem gem in highlights.Group)
			{
				GemView gemView = this.boardView.GetGemView(gem.position, true);
				gemView.Play(this.highlightClip, base.ModifiedDuration, true);
			}
		}

		// Token: 0x040052F1 RID: 21233
		[SerializeField]
		private AnimationClip highlightClip;
	}
}
