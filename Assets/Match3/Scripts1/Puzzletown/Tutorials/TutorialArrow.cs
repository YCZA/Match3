using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A9E RID: 2718
	public class TutorialArrow : MonoBehaviour
	{
		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x06004096 RID: 16534 RVA: 0x0014E93C File Offset: 0x0014CD3C
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

		// Token: 0x06004097 RID: 16535 RVA: 0x0014E968 File Offset: 0x0014CD68
		public void Init()
		{
			this.arrow.color = new Color(this.arrow.color.r, this.arrow.color.g, this.arrow.color.b, 0f);
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004098 RID: 16536 RVA: 0x0014E9CF File Offset: 0x0014CDCF
		public void Reset(TutorialStep step)
		{
			if (step.arrowPosition == ArrowPosition.none && base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.FadeOut());
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x06004099 RID: 16537 RVA: 0x0014EA0C File Offset: 0x0014CE0C
		public void Enable(ArrowPosition relativePosition, RectTransform targetTransform, Transform arrowContainerParent)
		{
			if (relativePosition != ArrowPosition.none)
			{
				base.gameObject.SetActive(true);
				this.InitArrowTargetPosition(targetTransform);
				this.SetArrowRotation(relativePosition, targetTransform);
				base.StartCoroutine(this.AnimateArrowIdle(relativePosition, arrowContainerParent));
				base.StartCoroutine(TutorialOverlayRoot.FadeImage(this.arrow, 0.25f, 0f, 1f));
			}
		}

		// Token: 0x0600409A RID: 16538 RVA: 0x0014EA6A File Offset: 0x0014CE6A
		public void AdjustPosition(RectTransform target)
		{
			if (target)
			{
				this.MyRectTransform.position = target.position;
			}
		}

		// Token: 0x0600409B RID: 16539 RVA: 0x0014EA88 File Offset: 0x0014CE88
		private IEnumerator FadeOut()
		{
			yield return base.StartCoroutine(TutorialOverlayRoot.FadeImage(this.arrow, 0.25f, 1f, 0f));
			this.arrow.color = new Color(this.arrow.color.r, this.arrow.color.g, this.arrow.color.b, 1f);
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x0600409C RID: 16540 RVA: 0x0014EAA3 File Offset: 0x0014CEA3
		private void InitArrowTargetPosition(RectTransform maskHighlight)
		{
			this.MyRectTransform.SetParent(maskHighlight, false);
			this.MyRectTransform.localPosition = Vector3.zero;
		}

		// Token: 0x0600409D RID: 16541 RVA: 0x0014EAC4 File Offset: 0x0014CEC4
		private void SetArrowRotation(ArrowPosition position, RectTransform maskHighlight)
		{
			RectTransform component = this.arrow.GetComponent<RectTransform>();
			switch (position)
			{
			case ArrowPosition.top:
				this.arrow.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
				component.anchoredPosition = new Vector2(0f, maskHighlight.rect.height / 2f + 80f);
				break;
			case ArrowPosition.right:
				this.arrow.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
				component.anchoredPosition = new Vector2(maskHighlight.rect.width / 2f + 80f, 0f);
				break;
			case ArrowPosition.bottom:
				this.arrow.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
				component.anchoredPosition = new Vector2(0f, -(maskHighlight.rect.height / 2f) - 80f);
				break;
			case ArrowPosition.left:
				this.arrow.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
				component.anchoredPosition = new Vector2(-(maskHighlight.rect.width / 2f) - 80f, 0f);
				break;
			}
		}

		// Token: 0x0600409E RID: 16542 RVA: 0x0014EC4C File Offset: 0x0014D04C
		private IEnumerator AnimateArrowIdle(ArrowPosition arrowSide, Transform arrowContainerParent)
		{
			this.MyRectTransform.SetParent(arrowContainerParent, true);
			RectTransform rect = this.arrow.GetComponent<RectTransform>();
			float idleDistance = 20f;
			float idleCycleDuration = 0.75f;
			Vector2 idleOffset = new Vector2(70f, 70f);
			Vector2 idleMin = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y);
			Vector2 idleMax = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y);
			switch (arrowSide)
			{
			case ArrowPosition.top:
				idleMin = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + idleOffset.y - idleDistance);
				idleMax = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + idleOffset.y + idleDistance);
				break;
			case ArrowPosition.right:
				idleMin = new Vector2(rect.anchoredPosition.x + idleOffset.x - idleDistance, rect.anchoredPosition.y);
				idleMax = new Vector2(rect.anchoredPosition.x + idleOffset.x + idleDistance, rect.anchoredPosition.y);
				break;
			case ArrowPosition.bottom:
				idleMin = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y - idleOffset.y + idleDistance);
				idleMax = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y - idleOffset.y - idleDistance);
				break;
			case ArrowPosition.left:
				idleMin = new Vector2(rect.anchoredPosition.x + idleDistance - idleOffset.x, rect.anchoredPosition.y);
				idleMax = new Vector2(rect.anchoredPosition.x - idleDistance - idleOffset.x, rect.anchoredPosition.y);
				break;
			}
			Vector2 startPostion = rect.anchoredPosition;
			float timePassedSoFar = 0f;
			float percentComplete = 0f;
			while (percentComplete < 1f)
			{
				timePassedSoFar += Time.deltaTime;
				percentComplete = timePassedSoFar / (idleCycleDuration / 2f);
				rect.anchoredPosition = Vector3.Slerp(startPostion, idleMin, percentComplete);
				yield return null;
			}
			for (;;)
			{
				timePassedSoFar = 0f;
				percentComplete = 0f;
				while (percentComplete < 1f)
				{
					timePassedSoFar += Time.deltaTime;
					percentComplete = timePassedSoFar / idleCycleDuration;
					rect.anchoredPosition = Vector3.Slerp(idleMin, idleMax, percentComplete);
					yield return null;
				}
				rect.anchoredPosition = idleMax;
				timePassedSoFar = 0f;
				percentComplete = 0f;
				while (percentComplete < 1f)
				{
					timePassedSoFar += Time.deltaTime;
					percentComplete = timePassedSoFar / idleCycleDuration;
					rect.anchoredPosition = Vector3.Slerp(idleMax, idleMin, percentComplete);
					yield return null;
				}
				rect.anchoredPosition = idleMin;
			}
			yield break;
		}

		// Token: 0x04006A48 RID: 27208
		public Image arrow;

		// Token: 0x04006A49 RID: 27209
		private RectTransform rectTransform;
	}
}
