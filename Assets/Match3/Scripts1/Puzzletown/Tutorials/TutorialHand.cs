using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A9F RID: 2719
	public class TutorialHand : MonoBehaviour
	{
		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x060040A0 RID: 16544 RVA: 0x0014F344 File Offset: 0x0014D744
		private RectTransform MyRectTransform
		{
			get
			{
				RectTransform result;
				if ((result = this.rectTransform) == null)
				{
					result = (this.rectTransform = base.GetComponent<RectTransform>());
				}
				return result;
			}
		}

		// Token: 0x060040A1 RID: 16545 RVA: 0x0014F370 File Offset: 0x0014D770
		public void Init()
		{
			this.touchFeedback.color = new Color(this.touchFeedback.color.r, this.touchFeedback.color.g, this.touchFeedback.color.b, 0f);
			this.hand.color = new Color(this.hand.color.r, this.hand.color.g, this.hand.color.b, 0f);
			base.gameObject.SetActive(false);
		}

		// Token: 0x060040A2 RID: 16546 RVA: 0x0014F427 File Offset: 0x0014D827
		public void Reset(TutorialStep step)
		{
			if (!step.showHand && base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.FadeOut());
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x060040A3 RID: 16547 RVA: 0x0014F464 File Offset: 0x0014D864
		public void Enable(Transform container, Vector3 from, Vector3 to, float duration)
		{
			base.gameObject.SetActive(true);
			base.StartCoroutine(TutorialOverlayRoot.FadeImage(this.hand, 0f, 0f, 1f));
			base.StartCoroutine(this.AnimateHand(container, from, to, duration));
		}

		// Token: 0x060040A4 RID: 16548 RVA: 0x0014F4B0 File Offset: 0x0014D8B0
		private IEnumerator FadeOut()
		{
			base.StartCoroutine(TutorialOverlayRoot.FadeImage(this.hand, 0.25f, 1f, 0f));
			yield return base.StartCoroutine(TutorialOverlayRoot.FadeImage(this.touchFeedback, 0.25f, 1f, 0f));
			this.hand.color = new Color(this.hand.color.r, this.hand.color.g, this.hand.color.b, 1f);
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x060040A5 RID: 16549 RVA: 0x0014F4CC File Offset: 0x0014D8CC
		private IEnumerator AnimateHand(Transform container, Vector3 from, Vector3 to, float duration)
		{
			this.MyRectTransform.SetParent(container, true);
			this.MyRectTransform.position = from;
			float fadeInDuration = duration * 0.05f;
			float moveDuration = duration * 0.45f;
			for (;;)
			{
				this.timer = 0f;
				yield return base.StartCoroutine(this.ToggleTouchVisualisation(fadeInDuration, true));
				yield return this.MyRectTransform.DOMove(to, moveDuration, false).WaitForCompletion();
				yield return base.StartCoroutine(this.ToggleTouchVisualisation(fadeInDuration, false));
				yield return this.MyRectTransform.DOMove(from, moveDuration, false).WaitForCompletion();
				yield return new WaitForSeconds(2f - this.timer);
			}
			yield break;
		}

		// Token: 0x060040A6 RID: 16550 RVA: 0x0014F504 File Offset: 0x0014D904
		private void Update()
		{
			this.timer += Time.deltaTime;
		}

		// Token: 0x060040A7 RID: 16551 RVA: 0x0014F518 File Offset: 0x0014D918
		private IEnumerator ToggleTouchVisualisation(float duration, bool showFeedback)
		{
			Image fadeInImage = (!showFeedback) ? this.hand : this.touchFeedback;
			Image fadeOutImage = (!showFeedback) ? this.touchFeedback : this.hand;
			base.StartCoroutine(TutorialOverlayRoot.FadeImage(fadeInImage, duration, 0f, 1f));
			yield return base.StartCoroutine(TutorialOverlayRoot.FadeImage(fadeOutImage, duration, 1f, 0f));
			yield break;
		}

		// Token: 0x04006A4A RID: 27210
		public Image hand;

		// Token: 0x04006A4B RID: 27211
		public Image touchFeedback;

		// Token: 0x04006A4C RID: 27212
		private const float FADE_DURATION_IN_PERCENT = 0.05f;

		// Token: 0x04006A4D RID: 27213
		private const float MOVE_DURATION_IN_PERCENT = 0.45f;

		// Token: 0x04006A4E RID: 27214
		private float timer;

		// Token: 0x04006A4F RID: 27215
		private RectTransform rectTransform;
	}
}
