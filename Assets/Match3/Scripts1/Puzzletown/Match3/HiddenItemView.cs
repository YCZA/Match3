using System.Collections;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006B5 RID: 1717
	public class HiddenItemView : MonoBehaviour
	{
		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06002ADB RID: 10971 RVA: 0x000C42E9 File Offset: 0x000C26E9
		public Animation Animation
		{
			get
			{
				if (!this._animation)
				{
					this._animation = base.GetComponent<Animation>();
				}
				return this._animation;
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06002ADC RID: 10972 RVA: 0x000C430D File Offset: 0x000C270D
		public Sprite sprite
		{
			get
			{
				return this.statues.sprite;
			}
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x000C431A File Offset: 0x000C271A
		public void Reveal(float delay)
		{
			base.StartCoroutine(this.RevealRoutine(delay));
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x000C432A File Offset: 0x000C272A
		public void Hide()
		{
			if (this.Animation != null)
			{
				this.Animation.Stop();
			}
			global::UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06002ADF RID: 10975 RVA: 0x000C4353 File Offset: 0x000C2753
		public void MarkAsRandom()
		{
			this.statues.color = Color.red;
		}

		// Token: 0x06002AE0 RID: 10976 RVA: 0x000C4368 File Offset: 0x000C2768
		public void StartAnimation(AnimationClip clip, float duration)
		{
			AnimationState animationState = this.Animation[clip.name];
			float speed = clip.length / duration;
			animationState.speed = speed;
			this.Animation.Play(animationState.name);
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x000C43AC File Offset: 0x000C27AC
		private IEnumerator RevealRoutine(float delay)
		{
			yield return new WaitForSeconds(delay);
			this.statues.sortingLayerName = "FieldsOverlay";
			yield break;
		}

		// Token: 0x0400542E RID: 21550
		[SerializeField]
		private SpriteRenderer statues;

		// Token: 0x0400542F RID: 21551
		private Animation _animation;
	}
}
