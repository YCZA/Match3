using UnityEngine;

namespace Match3.Scripts1.Spine.Unity.Modules
{
	// Token: 0x0200024D RID: 589
	public class SkeletonUtilityEyeConstraint : SkeletonUtilityConstraint
	{
		// Token: 0x0600122B RID: 4651 RVA: 0x00034D9C File Offset: 0x0003319C
		protected override void OnEnable()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			base.OnEnable();
			Bounds bounds = new Bounds(this.eyes[0].localPosition, Vector3.zero);
			this.origins = new Vector3[this.eyes.Length];
			for (int i = 0; i < this.eyes.Length; i++)
			{
				this.origins[i] = this.eyes[i].localPosition;
				bounds.Encapsulate(this.origins[i]);
			}
			this.centerPoint = bounds.center;
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x00034E42 File Offset: 0x00033242
		protected override void OnDisable()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			base.OnDisable();
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x00034E58 File Offset: 0x00033258
		public override void DoUpdate()
		{
			if (this.target != null)
			{
				this.targetPosition = this.target.position;
			}
			Vector3 a = this.targetPosition;
			Vector3 vector = base.transform.TransformPoint(this.centerPoint);
			Vector3 a2 = a - vector;
			if (a2.magnitude > 1f)
			{
				a2.Normalize();
			}
			for (int i = 0; i < this.eyes.Length; i++)
			{
				vector = base.transform.TransformPoint(this.origins[i]);
				this.eyes[i].position = Vector3.MoveTowards(this.eyes[i].position, vector + a2 * this.radius, this.speed * Time.deltaTime);
			}
		}

		// Token: 0x04004263 RID: 16995
		public Transform[] eyes;

		// Token: 0x04004264 RID: 16996
		public float radius = 0.5f;

		// Token: 0x04004265 RID: 16997
		public Transform target;

		// Token: 0x04004266 RID: 16998
		public Vector3 targetPosition;

		// Token: 0x04004267 RID: 16999
		public float speed = 10f;

		// Token: 0x04004268 RID: 17000
		private Vector3[] origins;

		// Token: 0x04004269 RID: 17001
		private Vector3 centerPoint;
	}
}
