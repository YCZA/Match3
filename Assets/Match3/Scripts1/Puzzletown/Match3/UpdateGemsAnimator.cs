using System.Collections;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200068B RID: 1675
	[CreateAssetMenu(menuName = "Puzzletown/Animators/Climber/UpdateGemsAnimator")]
	public class UpdateGemsAnimator : AAnimator<UpdateGems>
	{
		// Token: 0x060029A6 RID: 10662 RVA: 0x000BD434 File Offset: 0x000BB834
		protected override void DoAppend(UpdateGems gemsToBeUpdated)
		{
			foreach (Gem gem in gemsToBeUpdated.Group)
			{
				GemView gemView = this.boardView.GetGemView(gem.position, true);
				this.boardView.StartCoroutine(this.ShowFxAndUpdatedGem(gemView, gem));
			}
		}

		// Token: 0x060029A7 RID: 10663 RVA: 0x000BD4B4 File Offset: 0x000BB8B4
		private IEnumerator ShowFxAndUpdatedGem(GemView view, Gem gem)
		{
			FieldView fieldView = this.boardView.GetFieldView(gem.position);
			ParticleStartValues startValues = this.updateGemFx.GetComponent<ParticleStartValues>();
			startValues.UseColor = (int)gem.color;
			base.SetFxToFieldview(this.boardView.objectPool.Get(this.updateGemFx), fieldView);
			yield return new WaitForSeconds(base.ModifiedDuration);
			if (view != null)
			{
				view.Show(gem);
			}
			yield break;
		}

		// Token: 0x0400532C RID: 21292
		public GameObject updateGemFx;
	}
}
