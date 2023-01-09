using System.Collections;
using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200064E RID: 1614
	public abstract class AMatchWithGemAnimator<T> : AAnimator<T> where T : IMatchWithGem
	{
		// Token: 0x060028DD RID: 10461 RVA: 0x000B6150 File Offset: 0x000B4550
		protected override void DoAppend(T match)
		{
			GemView gemView = base.GetGemView(match.Gem.position, true);
			GameObject gameObject = this.boardView.objectPool.Get(this.gemCreationFxPrefab);
			gameObject.transform.SetParent(gemView.transform);
			gameObject.transform.localPosition = Vector3.zero;
			foreach (Gem gem in match.Group)
			{
				if (gem.position != match.Gem.position)
				{
					GemView gemView2 = base.GetGemView(gem.position, true);
					this.PlayMatchWithGemAnimation(gemView2, gem, match.Gem.position);
				}
			}
			gemView.sprite.sortingOrder = 12;
			gemView.Play(this.clipSuperGemCreated, base.ModifiedDuration, true);
			this.boardView.StartCoroutine(this.PlaySuperGemAnimation(gemView, match.Gem));
			this.PlaySound();
			gameObject.Release(base.ModifiedDuration + this.fxStayDelay);
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x000B62B4 File Offset: 0x000B46B4
		private IEnumerator PlaySuperGemAnimation(GemView view, Gem superGem)
		{
			float delayToShow = base.ModifiedDuration * this.percentageOfDurationTillShow;
			yield return new WaitForSeconds(delayToShow);
			view.Show(superGem);
			yield return new WaitForSeconds(base.ModifiedDuration - delayToShow + this.fxStayDelay);
			view.sprite.sortingOrder = 0;
			yield break;
		}

		// Token: 0x060028DF RID: 10463
		protected abstract void PlaySound();

		// Token: 0x040052A9 RID: 21161
		public GameObject gemCreationFxPrefab;

		// Token: 0x040052AA RID: 21162
		public AnimationClip clipSuperGemCreated;

		// Token: 0x040052AB RID: 21163
		public float fxStayDelay = 0.5f;

		// Token: 0x040052AC RID: 21164
		public float percentageOfDurationTillShow = 0.5f;
	}
}
