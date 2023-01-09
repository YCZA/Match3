using System;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x02000268 RID: 616
	[RequireComponent(typeof(ISkeletonAnimation))]
	[ExecuteInEditMode]
	public class SkeletonUtility : MonoBehaviour
	{
		// Token: 0x060012D2 RID: 4818 RVA: 0x0003B009 File Offset: 0x00039409
		public static T GetInParent<T>(Transform origin) where T : Component
		{
			return origin.GetComponentInParent<T>();
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x0003B014 File Offset: 0x00039414
		public static PolygonCollider2D AddBoundingBox(Skeleton skeleton, string skinName, string slotName, string attachmentName, Transform parent, bool isTrigger = true)
		{
			if (skinName == string.Empty)
			{
				skinName = skeleton.Data.DefaultSkin.Name;
			}
			Skin skin = skeleton.Data.FindSkin(skinName);
			if (skin == null)
			{
				global::UnityEngine.Debug.LogError("Skin " + skinName + " not found!");
				return null;
			}
			Attachment attachment = skin.GetAttachment(skeleton.FindSlotIndex(slotName), attachmentName);
			if (attachment is BoundingBoxAttachment)
			{
				PolygonCollider2D polygonCollider2D = new GameObject("[BoundingBox]" + attachmentName)
				{
					transform = 
					{
						parent = parent,
						localPosition = Vector3.zero,
						localRotation = Quaternion.identity,
						localScale = Vector3.one
					}
				}.AddComponent<PolygonCollider2D>();
				polygonCollider2D.isTrigger = isTrigger;
				BoundingBoxAttachment boundingBoxAttachment = (BoundingBoxAttachment)attachment;
				float[] vertices = boundingBoxAttachment.Vertices;
				int num = vertices.Length;
				int num2 = num / 2;
				Vector2[] array = new Vector2[num2];
				int num3 = 0;
				int i = 0;
				while (i < num)
				{
					array[num3].x = vertices[i];
					array[num3].y = vertices[i + 1];
					i += 2;
					num3++;
				}
				polygonCollider2D.SetPath(0, array);
				return polygonCollider2D;
			}
			return null;
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x0003B160 File Offset: 0x00039560
		public static PolygonCollider2D AddBoundingBoxAsComponent(BoundingBoxAttachment boundingBox, GameObject gameObject, bool isTrigger = true)
		{
			if (boundingBox == null)
			{
				return null;
			}
			PolygonCollider2D polygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
			polygonCollider2D.isTrigger = isTrigger;
			float[] vertices = boundingBox.Vertices;
			int num = vertices.Length;
			int num2 = num / 2;
			Vector2[] array = new Vector2[num2];
			int num3 = 0;
			int i = 0;
			while (i < num)
			{
				array[num3].x = vertices[i];
				array[num3].y = vertices[i + 1];
				i += 2;
				num3++;
			}
			polygonCollider2D.SetPath(0, array);
			return polygonCollider2D;
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x0003B1EC File Offset: 0x000395EC
		public static Bounds GetBoundingBoxBounds(BoundingBoxAttachment boundingBox, float depth = 0f)
		{
			float[] vertices = boundingBox.Vertices;
			int num = vertices.Length;
			Bounds result = default(Bounds);
			result.center = new Vector3(vertices[0], vertices[1], 0f);
			for (int i = 2; i < num; i += 2)
			{
				result.Encapsulate(new Vector3(vertices[i], vertices[i + 1], 0f));
			}
			Vector3 size = result.size;
			size.z = depth;
			result.size = size;
			return result;
		}

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x060012D6 RID: 4822 RVA: 0x0003B26C File Offset: 0x0003966C
		// (remove) Token: 0x060012D7 RID: 4823 RVA: 0x0003B2A4 File Offset: 0x000396A4
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event SkeletonUtility.SkeletonUtilityDelegate OnReset;

		// Token: 0x060012D8 RID: 4824 RVA: 0x0003B2DC File Offset: 0x000396DC
		private void Update()
		{
			if (this.boneRoot != null && this.skeletonRenderer.skeleton != null)
			{
				Vector3 one = Vector3.one;
				if (this.skeletonRenderer.skeleton.FlipX)
				{
					one.x = -1f;
				}
				if (this.skeletonRenderer.skeleton.FlipY)
				{
					one.y = -1f;
				}
				this.boneRoot.localScale = one;
			}
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x0003B360 File Offset: 0x00039760
		private void OnEnable()
		{
			if (this.skeletonRenderer == null)
			{
				this.skeletonRenderer = base.GetComponent<SkeletonRenderer>();
			}
			if (this.skeletonAnimation == null)
			{
				this.skeletonAnimation = base.GetComponent<SkeletonAnimation>();
				if (this.skeletonAnimation == null)
				{
					this.skeletonAnimation = base.GetComponent<SkeletonAnimator>();
				}
			}
			SkeletonRenderer skeletonRenderer = this.skeletonRenderer;
			skeletonRenderer.OnRebuild = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Remove(skeletonRenderer.OnRebuild, new SkeletonRenderer.SkeletonRendererDelegate(this.HandleRendererReset));
			SkeletonRenderer skeletonRenderer2 = this.skeletonRenderer;
			skeletonRenderer2.OnRebuild = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Combine(skeletonRenderer2.OnRebuild, new SkeletonRenderer.SkeletonRendererDelegate(this.HandleRendererReset));
			if (this.skeletonAnimation != null)
			{
				this.skeletonAnimation.UpdateLocal -= this.UpdateLocal;
				this.skeletonAnimation.UpdateLocal += this.UpdateLocal;
			}
			this.CollectBones();
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x0003B445 File Offset: 0x00039845
		private void Start()
		{
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x0003B448 File Offset: 0x00039848
		private void OnDisable()
		{
			SkeletonRenderer skeletonRenderer = this.skeletonRenderer;
			skeletonRenderer.OnRebuild = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Remove(skeletonRenderer.OnRebuild, new SkeletonRenderer.SkeletonRendererDelegate(this.HandleRendererReset));
			if (this.skeletonAnimation != null)
			{
				this.skeletonAnimation.UpdateLocal -= this.UpdateLocal;
				this.skeletonAnimation.UpdateWorld -= this.UpdateWorld;
				this.skeletonAnimation.UpdateComplete -= this.UpdateComplete;
			}
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x0003B4CC File Offset: 0x000398CC
		private void HandleRendererReset(SkeletonRenderer r)
		{
			if (this.OnReset != null)
			{
				this.OnReset();
			}
			this.CollectBones();
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x0003B4EA File Offset: 0x000398EA
		public void RegisterBone(SkeletonUtilityBone bone)
		{
			if (this.utilityBones.Contains(bone))
			{
				return;
			}
			this.utilityBones.Add(bone);
			this.needToReprocessBones = true;
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x0003B511 File Offset: 0x00039911
		public void UnregisterBone(SkeletonUtilityBone bone)
		{
			this.utilityBones.Remove(bone);
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x0003B520 File Offset: 0x00039920
		public void RegisterConstraint(SkeletonUtilityConstraint constraint)
		{
			if (this.utilityConstraints.Contains(constraint))
			{
				return;
			}
			this.utilityConstraints.Add(constraint);
			this.needToReprocessBones = true;
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x0003B547 File Offset: 0x00039947
		public void UnregisterConstraint(SkeletonUtilityConstraint constraint)
		{
			this.utilityConstraints.Remove(constraint);
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x0003B558 File Offset: 0x00039958
		public void CollectBones()
		{
			if (this.skeletonRenderer.skeleton == null)
			{
				return;
			}
			if (this.boneRoot != null)
			{
				List<string> list = new List<string>();
				ExposedList<IkConstraint> ikConstraints = this.skeletonRenderer.skeleton.IkConstraints;
				int i = 0;
				int count = ikConstraints.Count;
				while (i < count)
				{
					list.Add(ikConstraints.Items[i].Target.Data.Name);
					i++;
				}
				foreach (SkeletonUtilityBone skeletonUtilityBone in this.utilityBones)
				{
					if (skeletonUtilityBone.bone == null)
					{
						return;
					}
					if (skeletonUtilityBone.mode == SkeletonUtilityBone.Mode.Override)
					{
						this.hasTransformBones = true;
					}
					if (list.Contains(skeletonUtilityBone.bone.Data.Name))
					{
						this.hasUtilityConstraints = true;
					}
				}
				if (this.utilityConstraints.Count > 0)
				{
					this.hasUtilityConstraints = true;
				}
				if (this.skeletonAnimation != null)
				{
					this.skeletonAnimation.UpdateWorld -= this.UpdateWorld;
					this.skeletonAnimation.UpdateComplete -= this.UpdateComplete;
					if (this.hasTransformBones || this.hasUtilityConstraints)
					{
						this.skeletonAnimation.UpdateWorld += this.UpdateWorld;
					}
					if (this.hasUtilityConstraints)
					{
						this.skeletonAnimation.UpdateComplete += this.UpdateComplete;
					}
				}
				this.needToReprocessBones = false;
			}
			else
			{
				this.utilityBones.Clear();
				this.utilityConstraints.Clear();
			}
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x0003B728 File Offset: 0x00039B28
		private void UpdateLocal(ISkeletonAnimation anim)
		{
			if (this.needToReprocessBones)
			{
				this.CollectBones();
			}
			if (this.utilityBones == null)
			{
				return;
			}
			foreach (SkeletonUtilityBone skeletonUtilityBone in this.utilityBones)
			{
				skeletonUtilityBone.transformLerpComplete = false;
			}
			this.UpdateAllBones();
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x0003B7A8 File Offset: 0x00039BA8
		private void UpdateWorld(ISkeletonAnimation anim)
		{
			this.UpdateAllBones();
			foreach (SkeletonUtilityConstraint skeletonUtilityConstraint in this.utilityConstraints)
			{
				skeletonUtilityConstraint.DoUpdate();
			}
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x0003B80C File Offset: 0x00039C0C
		private void UpdateComplete(ISkeletonAnimation anim)
		{
			this.UpdateAllBones();
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x0003B814 File Offset: 0x00039C14
		private void UpdateAllBones()
		{
			if (this.boneRoot == null)
			{
				this.CollectBones();
			}
			if (this.utilityBones == null)
			{
				return;
			}
			foreach (SkeletonUtilityBone skeletonUtilityBone in this.utilityBones)
			{
				skeletonUtilityBone.DoUpdate();
			}
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x0003B894 File Offset: 0x00039C94
		public Transform GetBoneRoot()
		{
			if (this.boneRoot != null)
			{
				return this.boneRoot;
			}
			this.boneRoot = new GameObject("SkeletonUtility-Root").transform;
			this.boneRoot.parent = base.transform;
			this.boneRoot.localPosition = Vector3.zero;
			this.boneRoot.localRotation = Quaternion.identity;
			this.boneRoot.localScale = Vector3.one;
			return this.boneRoot;
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x0003B918 File Offset: 0x00039D18
		public GameObject SpawnRoot(SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
		{
			this.GetBoneRoot();
			Skeleton skeleton = this.skeletonRenderer.skeleton;
			GameObject result = this.SpawnBone(skeleton.RootBone, this.boneRoot, mode, pos, rot, sca);
			this.CollectBones();
			return result;
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x0003B958 File Offset: 0x00039D58
		public GameObject SpawnHierarchy(SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
		{
			this.GetBoneRoot();
			Skeleton skeleton = this.skeletonRenderer.skeleton;
			GameObject result = this.SpawnBoneRecursively(skeleton.RootBone, this.boneRoot, mode, pos, rot, sca);
			this.CollectBones();
			return result;
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x0003B998 File Offset: 0x00039D98
		public GameObject SpawnBoneRecursively(Bone bone, Transform parent, SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
		{
			GameObject gameObject = this.SpawnBone(bone, parent, mode, pos, rot, sca);
			ExposedList<Bone> children = bone.Children;
			int i = 0;
			int count = children.Count;
			while (i < count)
			{
				Bone bone2 = children.Items[i];
				this.SpawnBoneRecursively(bone2, gameObject.transform, mode, pos, rot, sca);
				i++;
			}
			return gameObject;
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x0003B9F8 File Offset: 0x00039DF8
		public GameObject SpawnBone(Bone bone, Transform parent, SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
		{
			GameObject gameObject = new GameObject(bone.Data.Name);
			gameObject.transform.parent = parent;
			SkeletonUtilityBone skeletonUtilityBone = gameObject.AddComponent<SkeletonUtilityBone>();
			skeletonUtilityBone.skeletonUtility = this;
			skeletonUtilityBone.position = pos;
			skeletonUtilityBone.rotation = rot;
			skeletonUtilityBone.scale = sca;
			skeletonUtilityBone.mode = mode;
			skeletonUtilityBone.zPosition = true;
			skeletonUtilityBone.Reset();
			skeletonUtilityBone.bone = bone;
			skeletonUtilityBone.boneName = bone.Data.Name;
			skeletonUtilityBone.valid = true;
			if (mode == SkeletonUtilityBone.Mode.Override)
			{
				if (rot)
				{
					gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, skeletonUtilityBone.bone.AppliedRotation);
				}
				if (pos)
				{
					gameObject.transform.localPosition = new Vector3(skeletonUtilityBone.bone.X, skeletonUtilityBone.bone.Y, 0f);
				}
				gameObject.transform.localScale = new Vector3(skeletonUtilityBone.bone.scaleX, skeletonUtilityBone.bone.scaleY, 0f);
			}
			return gameObject;
		}

		// Token: 0x04004320 RID: 17184
		public Transform boneRoot;

		// Token: 0x04004321 RID: 17185
		[HideInInspector]
		public SkeletonRenderer skeletonRenderer;

		// Token: 0x04004322 RID: 17186
		[HideInInspector]
		public ISkeletonAnimation skeletonAnimation;

		// Token: 0x04004323 RID: 17187
		[NonSerialized]
		public List<SkeletonUtilityBone> utilityBones = new List<SkeletonUtilityBone>();

		// Token: 0x04004324 RID: 17188
		[NonSerialized]
		public List<SkeletonUtilityConstraint> utilityConstraints = new List<SkeletonUtilityConstraint>();

		// Token: 0x04004325 RID: 17189
		protected bool hasTransformBones;

		// Token: 0x04004326 RID: 17190
		protected bool hasUtilityConstraints;

		// Token: 0x04004327 RID: 17191
		protected bool needToReprocessBones;

		// Token: 0x02000269 RID: 617
		// (Invoke) Token: 0x060012EC RID: 4844
		public delegate void SkeletonUtilityDelegate();
	}
}
