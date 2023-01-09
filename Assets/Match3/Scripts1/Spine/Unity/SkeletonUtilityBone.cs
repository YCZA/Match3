using System;
using UnityEngine;

namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x0200026A RID: 618
	[ExecuteInEditMode]
	[AddComponentMenu("Spine/SkeletonUtilityBone")]
	public class SkeletonUtilityBone : MonoBehaviour
	{
		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x060012F0 RID: 4848 RVA: 0x0003BB26 File Offset: 0x00039F26
		public bool DisableInheritScaleWarning
		{
			get
			{
				return this.disableInheritScaleWarning;
			}
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x0003BB30 File Offset: 0x00039F30
		public void Reset()
		{
			this.bone = null;
			this.cachedTransform = base.transform;
			this.valid = (this.skeletonUtility != null && this.skeletonUtility.skeletonRenderer != null && this.skeletonUtility.skeletonRenderer.valid);
			if (!this.valid)
			{
				return;
			}
			this.skeletonTransform = this.skeletonUtility.transform;
			this.skeletonUtility.OnReset -= this.HandleOnReset;
			this.skeletonUtility.OnReset += this.HandleOnReset;
			this.DoUpdate();
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x0003BBE4 File Offset: 0x00039FE4
		private void OnEnable()
		{
			this.skeletonUtility = SkeletonUtility.GetInParent<SkeletonUtility>(base.transform);
			if (this.skeletonUtility == null)
			{
				return;
			}
			this.skeletonUtility.RegisterBone(this);
			this.skeletonUtility.OnReset += this.HandleOnReset;
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x0003BC37 File Offset: 0x0003A037
		private void HandleOnReset()
		{
			this.Reset();
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x0003BC3F File Offset: 0x0003A03F
		private void OnDisable()
		{
			if (this.skeletonUtility != null)
			{
				this.skeletonUtility.OnReset -= this.HandleOnReset;
				this.skeletonUtility.UnregisterBone(this);
			}
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x0003BC78 File Offset: 0x0003A078
		public void DoUpdate()
		{
			if (!this.valid)
			{
				this.Reset();
				return;
			}
			Skeleton skeleton = this.skeletonUtility.skeletonRenderer.skeleton;
			if (this.bone == null)
			{
				if (this.boneName == null || this.boneName.Length == 0)
				{
					return;
				}
				this.bone = skeleton.FindBone(this.boneName);
				if (this.bone == null)
				{
					global::UnityEngine.Debug.LogError("Bone not found: " + this.boneName, this);
					return;
				}
			}
			float num = (!(skeleton.flipX ^ skeleton.flipY)) ? 1f : -1f;
			if (this.mode == SkeletonUtilityBone.Mode.Follow)
			{
				if (this.position)
				{
					this.cachedTransform.localPosition = new Vector3(this.bone.x, this.bone.y, 0f);
				}
				if (this.rotation)
				{
					if (this.bone.Data.InheritRotation)
					{
						this.cachedTransform.localRotation = Quaternion.Euler(0f, 0f, this.bone.AppliedRotation);
					}
					else
					{
						Vector3 eulerAngles = this.skeletonTransform.rotation.eulerAngles;
						this.cachedTransform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z + this.bone.WorldRotationX * num);
					}
				}
				if (this.scale)
				{
					this.cachedTransform.localScale = new Vector3(this.bone.scaleX, this.bone.scaleY, this.bone.WorldSignX);
					this.disableInheritScaleWarning = !this.bone.data.inheritScale;
				}
			}
			else if (this.mode == SkeletonUtilityBone.Mode.Override)
			{
				if (this.transformLerpComplete)
				{
					return;
				}
				if (this.parentReference == null)
				{
					if (this.position)
					{
						this.bone.x = Mathf.Lerp(this.bone.x, this.cachedTransform.localPosition.x, this.overrideAlpha);
						this.bone.y = Mathf.Lerp(this.bone.y, this.cachedTransform.localPosition.y, this.overrideAlpha);
					}
					if (this.rotation)
					{
						float appliedRotation = Mathf.LerpAngle(this.bone.Rotation, this.cachedTransform.localRotation.eulerAngles.z, this.overrideAlpha);
						this.bone.Rotation = appliedRotation;
						this.bone.AppliedRotation = appliedRotation;
					}
					if (this.scale)
					{
						this.bone.scaleX = Mathf.Lerp(this.bone.scaleX, this.cachedTransform.localScale.x, this.overrideAlpha);
						this.bone.scaleY = Mathf.Lerp(this.bone.scaleY, this.cachedTransform.localScale.y, this.overrideAlpha);
					}
				}
				else
				{
					if (this.transformLerpComplete)
					{
						return;
					}
					if (this.position)
					{
						Vector3 vector = this.parentReference.InverseTransformPoint(this.cachedTransform.position);
						this.bone.x = Mathf.Lerp(this.bone.x, vector.x, this.overrideAlpha);
						this.bone.y = Mathf.Lerp(this.bone.y, vector.y, this.overrideAlpha);
					}
					if (this.rotation)
					{
						float appliedRotation2 = Mathf.LerpAngle(this.bone.Rotation, Quaternion.LookRotation((!this.flipX) ? Vector3.forward : (Vector3.forward * -1f), this.parentReference.InverseTransformDirection(this.cachedTransform.up)).eulerAngles.z, this.overrideAlpha);
						this.bone.Rotation = appliedRotation2;
						this.bone.AppliedRotation = appliedRotation2;
					}
					if (this.scale)
					{
						this.bone.scaleX = Mathf.Lerp(this.bone.scaleX, this.cachedTransform.localScale.x, this.overrideAlpha);
						this.bone.scaleY = Mathf.Lerp(this.bone.scaleY, this.cachedTransform.localScale.y, this.overrideAlpha);
					}
					this.disableInheritScaleWarning = !this.bone.data.inheritScale;
				}
				this.transformLerpComplete = true;
			}
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x0003C165 File Offset: 0x0003A565
		public void AddBoundingBox(string skinName, string slotName, string attachmentName)
		{
			SkeletonUtility.AddBoundingBox(this.bone.skeleton, skinName, slotName, attachmentName, base.transform, true);
		}

		// Token: 0x04004328 RID: 17192
		[NonSerialized]
		public bool valid;

		// Token: 0x04004329 RID: 17193
		[NonSerialized]
		public SkeletonUtility skeletonUtility;

		// Token: 0x0400432A RID: 17194
		[NonSerialized]
		public Bone bone;

		// Token: 0x0400432B RID: 17195
		public SkeletonUtilityBone.Mode mode;

		// Token: 0x0400432C RID: 17196
		public bool zPosition = true;

		// Token: 0x0400432D RID: 17197
		public bool position;

		// Token: 0x0400432E RID: 17198
		public bool rotation;

		// Token: 0x0400432F RID: 17199
		public bool scale;

		// Token: 0x04004330 RID: 17200
		public bool flip;

		// Token: 0x04004331 RID: 17201
		public bool flipX;

		// Token: 0x04004332 RID: 17202
		[Range(0f, 1f)]
		public float overrideAlpha = 1f;

		// Token: 0x04004333 RID: 17203
		public string boneName;

		// Token: 0x04004334 RID: 17204
		public Transform parentReference;

		// Token: 0x04004335 RID: 17205
		[NonSerialized]
		public bool transformLerpComplete;

		// Token: 0x04004336 RID: 17206
		protected Transform cachedTransform;

		// Token: 0x04004337 RID: 17207
		protected Transform skeletonTransform;

		// Token: 0x04004338 RID: 17208
		private bool disableInheritScaleWarning;

		// Token: 0x0200026B RID: 619
		public enum Mode
		{
			// Token: 0x0400433A RID: 17210
			Follow,
			// Token: 0x0400433B RID: 17211
			Override
		}
	}
}
