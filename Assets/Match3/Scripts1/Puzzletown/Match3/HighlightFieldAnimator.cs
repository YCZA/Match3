using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000673 RID: 1651
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Board/HighlightFieldAnimator")]
	public class HighlightFieldAnimator : AAnimator<IHighlightPattern>
	{
		// Token: 0x06002947 RID: 10567 RVA: 0x000B9B1C File Offset: 0x000B7F1C
		protected override void DoAppend(IHighlightPattern matchResult)
		{
			for (int i = 0; i < matchResult.HighlightPositions.Count; i++)
			{
				this.HighlightField(matchResult.HighlightPositions[i]);
			}
		}

		// Token: 0x06002948 RID: 10568 RVA: 0x000B9B58 File Offset: 0x000B7F58
		private void HighlightField(IntVector2 position)
		{
			FieldView fieldView = this.boardView.GetFieldView(position);
			float delay = this.animController.fieldDelays[position];
			fieldView.GetComponent<HighlightField>().Show(base.ModifiedDuration, delay, false);
		}
	}
}
