using System.Collections;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200065C RID: 1628
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Chameleon/ChameleonTurnAnimator")]
	public class ChameleonTurnAnimator : AAnimator<ChameleonTurn>
	{
		// Token: 0x06002909 RID: 10505 RVA: 0x000B7B28 File Offset: 0x000B5F28
		protected override void DoAppend(ChameleonTurn matchResult)
		{
			IntVector2 origin = matchResult.Origin;
			FieldView fieldView = this.boardView.GetFieldView(origin);
			float delay = this.animController.fieldDelays[origin];
			ChameleonView chameleonView = this.boardView.GetChameleonView(fieldView.GridPosition);
			base.BlockSequence(this.sequence, fieldView.gameObject, delay);
			this.boardView.StartCoroutine(this.DelayedDoAppend(delay, chameleonView, matchResult.FacingDirection));
		}

		// Token: 0x0600290A RID: 10506 RVA: 0x000B7B9C File Offset: 0x000B5F9C
		private IEnumerator DelayedDoAppend(float delay, ChameleonView view, GemDirection newFacingDirection)
		{
			yield return new WaitForSeconds(delay);
			view.SetChameleonIsStuck(false);
			view.SetFacingDirection(newFacingDirection);
			yield break;
		}
	}
}
