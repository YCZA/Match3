using UnityEngine;

// Token: 0x020008DF RID: 2271
namespace Match3.Scripts1
{
	[AddComponentMenu(null)]
	public class AssetDestroyTrigger : MonoBehaviour
	{
		// Token: 0x06003742 RID: 14146 RVA: 0x0010DC72 File Offset: 0x0010C072
		public void OnAnimationEnd(Spine.AnimationState state, int trackIndex)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06003743 RID: 14147 RVA: 0x0010DC7F File Offset: 0x0010C07F
		public void OnTweenEnd()
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
