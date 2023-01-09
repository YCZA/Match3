using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C2D RID: 3117
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("UI/Extensions/ScrollRectTweener")]
	public class ScrollRectTweener : MonoBehaviour, IDragHandler, IEventSystemHandler
	{
		// Token: 0x06004982 RID: 18818 RVA: 0x00177AE4 File Offset: 0x00175EE4
		private void Awake()
		{
			this.scrollRect = base.GetComponent<ScrollRect>();
			this.wasHorizontal = this.scrollRect.horizontal;
			this.wasVertical = this.scrollRect.vertical;
		}

		// Token: 0x06004983 RID: 18819 RVA: 0x00177B14 File Offset: 0x00175F14
		public void ScrollHorizontal(float normalizedX)
		{
			this.Scroll(new Vector2(normalizedX, this.scrollRect.verticalNormalizedPosition));
		}

		// Token: 0x06004984 RID: 18820 RVA: 0x00177B2D File Offset: 0x00175F2D
		public void ScrollHorizontal(float normalizedX, float duration)
		{
			this.Scroll(new Vector2(normalizedX, this.scrollRect.verticalNormalizedPosition), duration);
		}

		// Token: 0x06004985 RID: 18821 RVA: 0x00177B47 File Offset: 0x00175F47
		public void ScrollVertical(float normalizedY)
		{
			this.Scroll(new Vector2(this.scrollRect.horizontalNormalizedPosition, normalizedY));
		}

		// Token: 0x06004986 RID: 18822 RVA: 0x00177B60 File Offset: 0x00175F60
		public void ScrollVertical(float normalizedY, float duration)
		{
			this.Scroll(new Vector2(this.scrollRect.horizontalNormalizedPosition, normalizedY), duration);
		}

		// Token: 0x06004987 RID: 18823 RVA: 0x00177B7A File Offset: 0x00175F7A
		public void Scroll(Vector2 normalizedPos)
		{
			this.Scroll(normalizedPos, this.GetScrollDuration(normalizedPos));
		}

		// Token: 0x06004988 RID: 18824 RVA: 0x00177B8C File Offset: 0x00175F8C
		private float GetScrollDuration(Vector2 normalizedPos)
		{
			Vector2 currentPos = this.GetCurrentPos();
			return Vector2.Distance(this.DeNormalize(currentPos), this.DeNormalize(normalizedPos)) / this.moveSpeed;
		}

		// Token: 0x06004989 RID: 18825 RVA: 0x00177BBC File Offset: 0x00175FBC
		private Vector2 DeNormalize(Vector2 normalizedPos)
		{
			return new Vector2(normalizedPos.x * this.scrollRect.content.rect.width, normalizedPos.y * this.scrollRect.content.rect.height);
		}

		// Token: 0x0600498A RID: 18826 RVA: 0x00177C0E File Offset: 0x0017600E
		private Vector2 GetCurrentPos()
		{
			return new Vector2(this.scrollRect.horizontalNormalizedPosition, this.scrollRect.verticalNormalizedPosition);
		}

		// Token: 0x0600498B RID: 18827 RVA: 0x00177C2B File Offset: 0x0017602B
		public void Scroll(Vector2 normalizedPos, float duration)
		{
			this.startPos = this.GetCurrentPos();
			this.targetPos = normalizedPos;
			if (this.disableDragWhileTweening)
			{
				this.LockScrollability();
			}
			base.StopAllCoroutines();
			base.StartCoroutine(this.DoMove(duration));
		}

		// Token: 0x0600498C RID: 18828 RVA: 0x00177C68 File Offset: 0x00176068
		private IEnumerator DoMove(float duration)
		{
			if (duration < 0.05f)
			{
				yield break;
			}
			Vector2 posOffset = this.targetPos - this.startPos;
			float currentTime = 0f;
			while (currentTime < duration)
			{
				currentTime += Time.deltaTime;
				this.scrollRect.normalizedPosition = this.EaseVector(currentTime, this.startPos, posOffset, duration);
				yield return null;
			}
			this.scrollRect.normalizedPosition = this.targetPos;
			if (this.disableDragWhileTweening)
			{
				this.RestoreScrollability();
			}
			yield break;
		}

		// Token: 0x0600498D RID: 18829 RVA: 0x00177C8C File Offset: 0x0017608C
		public Vector2 EaseVector(float currentTime, Vector2 startValue, Vector2 changeInValue, float duration)
		{
			return new Vector2(changeInValue.x * Mathf.Sin(currentTime / duration * 1.5707964f) + startValue.x, changeInValue.y * Mathf.Sin(currentTime / duration * 1.5707964f) + startValue.y);
		}

		// Token: 0x0600498E RID: 18830 RVA: 0x00177CDC File Offset: 0x001760DC
		public void OnDrag(PointerEventData eventData)
		{
			if (!this.disableDragWhileTweening)
			{
				this.StopScroll();
			}
		}

		// Token: 0x0600498F RID: 18831 RVA: 0x00177CEF File Offset: 0x001760EF
		private void StopScroll()
		{
			base.StopAllCoroutines();
			if (this.disableDragWhileTweening)
			{
				this.RestoreScrollability();
			}
		}

		// Token: 0x06004990 RID: 18832 RVA: 0x00177D08 File Offset: 0x00176108
		private void LockScrollability()
		{
			this.scrollRect.horizontal = false;
			this.scrollRect.vertical = false;
		}

		// Token: 0x06004991 RID: 18833 RVA: 0x00177D22 File Offset: 0x00176122
		private void RestoreScrollability()
		{
			this.scrollRect.horizontal = this.wasHorizontal;
			this.scrollRect.vertical = this.wasVertical;
		}

		// Token: 0x04006FD7 RID: 28631
		private ScrollRect scrollRect;

		// Token: 0x04006FD8 RID: 28632
		private Vector2 startPos;

		// Token: 0x04006FD9 RID: 28633
		private Vector2 targetPos;

		// Token: 0x04006FDA RID: 28634
		private bool wasHorizontal;

		// Token: 0x04006FDB RID: 28635
		private bool wasVertical;

		// Token: 0x04006FDC RID: 28636
		public float moveSpeed = 5000f;

		// Token: 0x04006FDD RID: 28637
		public bool disableDragWhileTweening;
	}
}
