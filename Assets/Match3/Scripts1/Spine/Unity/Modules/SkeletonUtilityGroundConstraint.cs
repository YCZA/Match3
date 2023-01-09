using UnityEngine;

namespace Match3.Scripts1.Spine.Unity.Modules
{
	// Token: 0x0200024E RID: 590
	[RequireComponent(typeof(SkeletonUtilityBone))]
	[ExecuteInEditMode]
	public class SkeletonUtilityGroundConstraint : SkeletonUtilityConstraint
	{
		// Token: 0x0600122F RID: 4655 RVA: 0x00034F84 File Offset: 0x00033384
		protected override void OnEnable()
		{
			base.OnEnable();
			this.lastHitY = base.transform.position.y;
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x00034FB0 File Offset: 0x000333B0
		protected override void OnDisable()
		{
			base.OnDisable();
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x00034FB8 File Offset: 0x000333B8
		public override void DoUpdate()
		{
			this.rayOrigin = base.transform.position + new Vector3(this.castOffset, this.castDistance, 0f);
			this.hitY = float.MinValue;
			if (this.use2D)
			{
				RaycastHit2D raycastHit2D;
				if (this.useRadius)
				{
					raycastHit2D = Physics2D.CircleCast(this.rayOrigin, this.castRadius, this.rayDir, this.castDistance + this.groundOffset, this.groundMask);
				}
				else
				{
					raycastHit2D = Physics2D.Raycast(this.rayOrigin, this.rayDir, this.castDistance + this.groundOffset, this.groundMask);
				}
				if (raycastHit2D.collider != null)
				{
					this.hitY = raycastHit2D.point.y + this.groundOffset;
					if (Application.isPlaying)
					{
						this.hitY = Mathf.MoveTowards(this.lastHitY, this.hitY, this.adjustSpeed * Time.deltaTime);
					}
				}
				else if (Application.isPlaying)
				{
					this.hitY = Mathf.MoveTowards(this.lastHitY, base.transform.position.y, this.adjustSpeed * Time.deltaTime);
				}
			}
			else
			{
				RaycastHit raycastHit;
				bool flag;
				if (this.useRadius)
				{
					flag = Physics.SphereCast(this.rayOrigin, this.castRadius, this.rayDir, out raycastHit, this.castDistance + this.groundOffset, this.groundMask);
				}
				else
				{
					flag = Physics.Raycast(this.rayOrigin, this.rayDir, out raycastHit, this.castDistance + this.groundOffset, this.groundMask);
				}
				if (flag)
				{
					this.hitY = raycastHit.point.y + this.groundOffset;
					if (Application.isPlaying)
					{
						this.hitY = Mathf.MoveTowards(this.lastHitY, this.hitY, this.adjustSpeed * Time.deltaTime);
					}
				}
				else if (Application.isPlaying)
				{
					this.hitY = Mathf.MoveTowards(this.lastHitY, base.transform.position.y, this.adjustSpeed * Time.deltaTime);
				}
			}
			Vector3 position = base.transform.position;
			position.y = Mathf.Clamp(position.y, Mathf.Min(this.lastHitY, this.hitY), float.MaxValue);
			base.transform.position = position;
			this.utilBone.bone.X = base.transform.localPosition.x;
			this.utilBone.bone.Y = base.transform.localPosition.y;
			this.lastHitY = this.hitY;
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x000352C0 File Offset: 0x000336C0
		private void OnDrawGizmos()
		{
			Vector3 vector = this.rayOrigin + this.rayDir * Mathf.Min(this.castDistance, this.rayOrigin.y - this.hitY);
			Vector3 to = this.rayOrigin + this.rayDir * this.castDistance;
			Gizmos.DrawLine(this.rayOrigin, vector);
			if (this.useRadius)
			{
				Gizmos.DrawLine(new Vector3(vector.x - this.castRadius, vector.y - this.groundOffset, vector.z), new Vector3(vector.x + this.castRadius, vector.y - this.groundOffset, vector.z));
				Gizmos.DrawLine(new Vector3(to.x - this.castRadius, to.y, to.z), new Vector3(to.x + this.castRadius, to.y, to.z));
			}
			Gizmos.color = Color.red;
			Gizmos.DrawLine(vector, to);
		}

		// Token: 0x0400426A RID: 17002
		[Tooltip("LayerMask for what objects to raycast against")]
		public LayerMask groundMask;

		// Token: 0x0400426B RID: 17003
		[Tooltip("The 2D")]
		public bool use2D;

		// Token: 0x0400426C RID: 17004
		[Tooltip("Uses SphereCast for 3D mode and CircleCast for 2D mode")]
		public bool useRadius;

		// Token: 0x0400426D RID: 17005
		[Tooltip("The Radius")]
		public float castRadius = 0.1f;

		// Token: 0x0400426E RID: 17006
		[Tooltip("How high above the target bone to begin casting from")]
		public float castDistance = 5f;

		// Token: 0x0400426F RID: 17007
		[Tooltip("X-Axis adjustment")]
		public float castOffset;

		// Token: 0x04004270 RID: 17008
		[Tooltip("Y-Axis adjustment")]
		public float groundOffset;

		// Token: 0x04004271 RID: 17009
		[Tooltip("How fast the target IK position adjusts to the ground.  Use smaller values to prevent snapping")]
		public float adjustSpeed = 5f;

		// Token: 0x04004272 RID: 17010
		private Vector3 rayOrigin;

		// Token: 0x04004273 RID: 17011
		private Vector3 rayDir = new Vector3(0f, -1f, 0f);

		// Token: 0x04004274 RID: 17012
		private float hitY;

		// Token: 0x04004275 RID: 17013
		private float lastHitY;
	}
}
