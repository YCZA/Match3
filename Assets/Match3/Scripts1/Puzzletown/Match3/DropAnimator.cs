using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000668 RID: 1640
	public class DropAnimator : MonoBehaviour
	{
		// Token: 0x0600292C RID: 10540 RVA: 0x000B8D78 File Offset: 0x000B7178
		public void PlayDropAnimation(GemView view, bool isFinal, float duration, float landDuration = 0f)
		{
			view.Play(this.clipDropping, duration, true);
			if (isFinal)
			{
				view.Play(this.clipLanding, landDuration, false);
				view.ModifierLanded();
			}
		}

		// Token: 0x040052DB RID: 21211
		public AnimationClip clipDropping;

		// Token: 0x040052DC RID: 21212
		public AnimationClip clipLanding;
	}
}
