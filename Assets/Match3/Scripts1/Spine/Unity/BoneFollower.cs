using System;
using UnityEngine;

namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x0200022A RID: 554
	[ExecuteInEditMode]
	[AddComponentMenu("Spine/BoneFollower")]
	public class BoneFollower : MonoBehaviour
	{
		// Token: 0x1700027D RID: 637
		// (get) Token: 0x0600117A RID: 4474 RVA: 0x0002F250 File Offset: 0x0002D650
		// (set) Token: 0x0600117B RID: 4475 RVA: 0x0002F258 File Offset: 0x0002D658
		public SkeletonRenderer SkeletonRenderer
		{
			get
			{
				return this.skeletonRenderer;
			}
			set
			{
				this.skeletonRenderer = value;
				this.Reset();
			}
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x0002F267 File Offset: 0x0002D667
		public void HandleResetRenderer(SkeletonRenderer skeletonRenderer)
		{
			this.Reset();
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x0002F270 File Offset: 0x0002D670
		public void Reset()
		{
			this.bone = null;
			this.valid = (this.skeletonRenderer != null && this.skeletonRenderer.valid);
			if (!this.valid)
			{
				return;
			}
			this.skeletonTransform = this.skeletonRenderer.transform;
			SkeletonRenderer skeletonRenderer = this.skeletonRenderer;
			skeletonRenderer.OnRebuild = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Remove(skeletonRenderer.OnRebuild, new SkeletonRenderer.SkeletonRendererDelegate(this.HandleResetRenderer));
			SkeletonRenderer skeletonRenderer2 = this.skeletonRenderer;
			skeletonRenderer2.OnRebuild = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Combine(skeletonRenderer2.OnRebuild, new SkeletonRenderer.SkeletonRendererDelegate(this.HandleResetRenderer));
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x0002F314 File Offset: 0x0002D714
		private void OnDestroy()
		{
			if (this.skeletonRenderer != null)
			{
				SkeletonRenderer skeletonRenderer = this.skeletonRenderer;
				skeletonRenderer.OnRebuild = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Remove(skeletonRenderer.OnRebuild, new SkeletonRenderer.SkeletonRendererDelegate(this.HandleResetRenderer));
			}
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x0002F34E File Offset: 0x0002D74E
		public void Awake()
		{
			if (this.resetOnAwake)
			{
				this.Reset();
			}
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x0002F361 File Offset: 0x0002D761
		private void LateUpdate()
		{
			this.DoUpdate();
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x0002F36C File Offset: 0x0002D76C
		public void DoUpdate()
		{
			if (!this.valid)
			{
				this.Reset();
				return;
			}
			if (this.bone == null)
			{
				if (this.boneName == null || this.boneName.Length == 0)
				{
					return;
				}
				this.bone = this.skeletonRenderer.skeleton.FindBone(this.boneName);
				if (this.bone == null)
				{
					global::UnityEngine.Debug.LogError("Bone not found: " + this.boneName, this);
					return;
				}
			}
			Skeleton skeleton = this.skeletonRenderer.skeleton;
			float num = (!(skeleton.flipX ^ skeleton.flipY)) ? 1f : -1f;
			Transform transform = base.transform;
			if (transform.parent == this.skeletonTransform)
			{
				transform.localPosition = new Vector3(this.bone.worldX, this.bone.worldY, (!this.followZPosition) ? transform.localPosition.z : 0f);
				if (this.followBoneRotation)
				{
					Vector3 eulerAngles = transform.localRotation.eulerAngles;
					transform.localRotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, this.bone.WorldRotationX * num);
				}
			}
			else
			{
				Vector3 position = this.skeletonTransform.TransformPoint(new Vector3(this.bone.worldX, this.bone.worldY, 0f));
				if (!this.followZPosition)
				{
					position.z = transform.position.z;
				}
				transform.position = position;
				if (this.followBoneRotation)
				{
					Vector3 eulerAngles2 = this.skeletonTransform.rotation.eulerAngles;
					transform.rotation = Quaternion.Euler(eulerAngles2.x, eulerAngles2.y, this.skeletonTransform.rotation.eulerAngles.z + this.bone.WorldRotationX * num);
				}
			}
		}

		// Token: 0x040041BB RID: 16827
		public SkeletonRenderer skeletonRenderer;

		// Token: 0x040041BC RID: 16828
		[SpineBone("", "skeletonRenderer")]
		public string boneName;

		// Token: 0x040041BD RID: 16829
		public bool followZPosition = true;

		// Token: 0x040041BE RID: 16830
		public bool followBoneRotation = true;

		// Token: 0x040041BF RID: 16831
		public bool resetOnAwake = true;

		// Token: 0x040041C0 RID: 16832
		[NonSerialized]
		public bool valid;

		// Token: 0x040041C1 RID: 16833
		[NonSerialized]
		public Bone bone;

		// Token: 0x040041C2 RID: 16834
		private Transform skeletonTransform;
	}
}
