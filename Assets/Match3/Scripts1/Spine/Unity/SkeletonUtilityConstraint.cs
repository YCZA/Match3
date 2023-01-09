using UnityEngine;

namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x0200026C RID: 620
	[RequireComponent(typeof(SkeletonUtilityBone))]
	[ExecuteInEditMode]
	public abstract class SkeletonUtilityConstraint : MonoBehaviour
	{
		// Token: 0x060012F8 RID: 4856 RVA: 0x00034D44 File Offset: 0x00033144
		protected virtual void OnEnable()
		{
			this.utilBone = base.GetComponent<SkeletonUtilityBone>();
			this.skeletonUtility = SkeletonUtility.GetInParent<SkeletonUtility>(base.transform);
			this.skeletonUtility.RegisterConstraint(this);
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x00034D6F File Offset: 0x0003316F
		protected virtual void OnDisable()
		{
			this.skeletonUtility.UnregisterConstraint(this);
		}

		// Token: 0x060012FA RID: 4858
		public abstract void DoUpdate();

		// Token: 0x0400433C RID: 17212
		protected SkeletonUtilityBone utilBone;

		// Token: 0x0400433D RID: 17213
		protected SkeletonUtility skeletonUtility;
	}
}
