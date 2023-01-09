using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200067F RID: 1663
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Board/ReplaceGemAnimator")]
	public class ReplaceGemAnimator : AAnimator<ReplaceGem>
	{
		// Token: 0x06002982 RID: 10626 RVA: 0x000BBF8C File Offset: 0x000BA38C
		protected override void DoAppend(ReplaceGem matchResult)
		{
			GemView gemView = base.GetGemView(matchResult.gem.position, true);
			gemView.Show(matchResult.gem);
		}
	}
}
