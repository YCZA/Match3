using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200066C RID: 1644
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Supergems/FishMatchAnimator")]
	public class FishMatchAnimator : AAnimator<FishMatch>
	{
		// Token: 0x06002934 RID: 10548 RVA: 0x000B8FD8 File Offset: 0x000B73D8
		protected override void DoAppend(FishMatch match)
		{
			foreach (Gem gem in match.Group)
			{
				GemView gemView = base.GetGemView(gem.position, true);
				this.PlayMatchWithGemAnimation(gemView, gem, match.fishOrigin);
			}
		}
	}
}
