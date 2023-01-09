using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Spine.Unity.Modules
{
	// Token: 0x02000248 RID: 584
	[RequireComponent(typeof(SkeletonRenderer))]
	public class SkeletonRagdoll : MonoBehaviour
	{
		// Token: 0x1700028D RID: 653
		// (get) Token: 0x060011E8 RID: 4584 RVA: 0x00032A75 File Offset: 0x00030E75
		// (set) Token: 0x060011E9 RID: 4585 RVA: 0x00032A7D File Offset: 0x00030E7D
		public Rigidbody RootRigidbody { get; private set; }

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x060011EA RID: 4586 RVA: 0x00032A86 File Offset: 0x00030E86
		// (set) Token: 0x060011EB RID: 4587 RVA: 0x00032A8E File Offset: 0x00030E8E
		public Bone StartingBone { get; private set; }

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x060011EC RID: 4588 RVA: 0x00032A97 File Offset: 0x00030E97
		public Vector3 RootOffset
		{
			get
			{
				return this.rootOffset;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x060011ED RID: 4589 RVA: 0x00032A9F File Offset: 0x00030E9F
		public bool IsActive
		{
			get
			{
				return this.isActive;
			}
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x00032AA8 File Offset: 0x00030EA8
		private IEnumerator Start()
		{
			if (SkeletonRagdoll.parentSpaceHelper == null)
			{
				SkeletonRagdoll.parentSpaceHelper = new GameObject("Parent Space Helper").transform;
				SkeletonRagdoll.parentSpaceHelper.hideFlags = HideFlags.HideInHierarchy;
			}
			this.targetSkeletonComponent = (base.GetComponent<SkeletonRenderer>() as ISkeletonAnimation);
			if (this.targetSkeletonComponent == null)
			{
				global::UnityEngine.Debug.LogError("Attached Spine component does not implement ISkeletonAnimation. This script is not compatible.");
			}
			this.skeleton = this.targetSkeletonComponent.Skeleton;
			if (this.applyOnStart)
			{
				yield return null;
				this.Apply();
			}
			yield break;
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x060011EF RID: 4591 RVA: 0x00032AC4 File Offset: 0x00030EC4
		public Rigidbody[] RigidbodyArray
		{
			get
			{
				if (!this.isActive)
				{
					return new Rigidbody[0];
				}
				Rigidbody[] array = new Rigidbody[this.boneTable.Count];
				int num = 0;
				foreach (Transform transform in this.boneTable.Values)
				{
					array[num] = transform.GetComponent<Rigidbody>();
					num++;
				}
				return array;
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x060011F0 RID: 4592 RVA: 0x00032B54 File Offset: 0x00030F54
		public Vector3 EstimatedSkeletonPosition
		{
			get
			{
				return this.RootRigidbody.position - this.rootOffset;
			}
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x00032B6C File Offset: 0x00030F6C
		public void Apply()
		{
			this.isActive = true;
			this.mix = 1f;
			this.StartingBone = this.skeleton.FindBone(this.startingBoneName);
			this.RecursivelyCreateBoneProxies(this.StartingBone);
			this.RootRigidbody = this.boneTable[this.StartingBone].GetComponent<Rigidbody>();
			this.RootRigidbody.isKinematic = this.pinStartBone;
			this.RootRigidbody.mass = this.rootMass;
			List<Collider> list = new List<Collider>();
			foreach (KeyValuePair<Bone, Transform> keyValuePair in this.boneTable)
			{
				Bone key = keyValuePair.Key;
				Transform value = keyValuePair.Value;
				list.Add(value.GetComponent<Collider>());
				Transform transform;
				if (key == this.StartingBone)
				{
					this.ragdollRoot = new GameObject("RagdollRoot").transform;
					this.ragdollRoot.SetParent(base.transform, false);
					if (key == this.skeleton.RootBone)
					{
						this.ragdollRoot.localPosition = new Vector3(key.WorldX, key.WorldY, 0f);
						this.ragdollRoot.localRotation = Quaternion.Euler(0f, 0f, SkeletonRagdoll.GetPropagatedRotation(key));
					}
					else
					{
						this.ragdollRoot.localPosition = new Vector3(key.Parent.WorldX, key.Parent.WorldY, 0f);
						this.ragdollRoot.localRotation = Quaternion.Euler(0f, 0f, SkeletonRagdoll.GetPropagatedRotation(key.Parent));
					}
					transform = this.ragdollRoot;
					this.rootOffset = value.position - base.transform.position;
				}
				else
				{
					transform = this.boneTable[key.Parent];
				}
				Rigidbody component = transform.GetComponent<Rigidbody>();
				if (component != null)
				{
					HingeJoint hingeJoint = value.gameObject.AddComponent<HingeJoint>();
					hingeJoint.connectedBody = component;
					Vector3 connectedAnchor = transform.InverseTransformPoint(value.position);
					connectedAnchor.x *= 1f;
					hingeJoint.connectedAnchor = connectedAnchor;
					hingeJoint.axis = Vector3.forward;
					hingeJoint.GetComponent<Rigidbody>().mass = hingeJoint.connectedBody.mass * this.massFalloffFactor;
					hingeJoint.limits = new JointLimits
					{
						min = -this.rotationLimit,
						max = this.rotationLimit
					};
					hingeJoint.useLimits = true;
					hingeJoint.enableCollision = this.enableJointCollision;
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (i != j)
					{
						Physics.IgnoreCollision(list[i], list[j]);
					}
				}
			}
			SkeletonUtilityBone[] componentsInChildren = base.GetComponentsInChildren<SkeletonUtilityBone>();
			if (componentsInChildren.Length > 0)
			{
				List<string> list2 = new List<string>();
				foreach (SkeletonUtilityBone skeletonUtilityBone in componentsInChildren)
				{
					if (skeletonUtilityBone.mode == SkeletonUtilityBone.Mode.Override)
					{
						list2.Add(skeletonUtilityBone.gameObject.name);
						global::UnityEngine.Object.Destroy(skeletonUtilityBone.gameObject);
					}
				}
				if (list2.Count > 0)
				{
					string text = "Destroyed Utility Bones: ";
					for (int l = 0; l < list2.Count; l++)
					{
						text += list2[l];
						if (l != list2.Count - 1)
						{
							text += ",";
						}
					}
					global::UnityEngine.Debug.LogWarning(text);
				}
			}
			if (this.disableIK)
			{
				foreach (IkConstraint ikConstraint in this.skeleton.IkConstraints)
				{
					ikConstraint.Mix = 0f;
				}
			}
			this.targetSkeletonComponent.UpdateWorld += this.UpdateSpineSkeleton;
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x00032FF0 File Offset: 0x000313F0
		public Coroutine SmoothMix(float target, float duration)
		{
			return base.StartCoroutine(this.SmoothMixCoroutine(target, duration));
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x00033000 File Offset: 0x00031400
		private IEnumerator SmoothMixCoroutine(float target, float duration)
		{
			float startTime = Time.time;
			float startMix = this.mix;
			while (this.mix > 0f)
			{
				this.mix = Mathf.SmoothStep(startMix, target, (Time.time - startTime) / duration);
				yield return null;
			}
			yield break;
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x0003302C File Offset: 0x0003142C
		public void SetSkeletonPosition(Vector3 worldPosition)
		{
			if (!this.isActive)
			{
				global::UnityEngine.Debug.LogWarning("Can't call SetSkeletonPosition while Ragdoll is not active!");
				return;
			}
			Vector3 b = worldPosition - base.transform.position;
			base.transform.position = worldPosition;
			foreach (Transform transform in this.boneTable.Values)
			{
				transform.position -= b;
			}
			this.UpdateSpineSkeleton(null);
			this.skeleton.UpdateWorldTransform();
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x000330E0 File Offset: 0x000314E0
		public void Remove()
		{
			this.isActive = false;
			foreach (Transform transform in this.boneTable.Values)
			{
				global::UnityEngine.Object.Destroy(transform.gameObject);
			}
			global::UnityEngine.Object.Destroy(this.ragdollRoot.gameObject);
			this.boneTable.Clear();
			this.targetSkeletonComponent.UpdateWorld -= this.UpdateSpineSkeleton;
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x00033180 File Offset: 0x00031580
		public Rigidbody GetRigidbody(string boneName)
		{
			Bone bone = this.skeleton.FindBone(boneName);
			return (bone == null || !this.boneTable.ContainsKey(bone)) ? null : this.boneTable[bone].GetComponent<Rigidbody>();
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x000331C8 File Offset: 0x000315C8
		private void RecursivelyCreateBoneProxies(Bone b)
		{
			string name = b.data.name;
			if (this.stopBoneNames.Contains(name))
			{
				return;
			}
			GameObject gameObject = new GameObject(name);
			gameObject.layer = this.colliderLayer;
			Transform transform = gameObject.transform;
			this.boneTable.Add(b, transform);
			transform.parent = base.transform;
			transform.localPosition = new Vector3(b.WorldX, b.WorldY, 0f);
			transform.localRotation = Quaternion.Euler(0f, 0f, b.WorldRotationX);
			transform.localScale = new Vector3(b.WorldScaleX, b.WorldScaleY, 1f);
			List<Collider> list = this.AttachBoundingBoxRagdollColliders(b);
			if (list.Count == 0)
			{
				float length = b.Data.Length;
				if (length == 0f)
				{
					SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
					sphereCollider.radius = this.thickness * 0.5f;
				}
				else
				{
					BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
					boxCollider.size = new Vector3(length, this.thickness, this.thickness);
					boxCollider.center = new Vector3(length * 0.5f, 0f);
				}
			}
			Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
			rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
			foreach (Bone b2 in b.Children)
			{
				this.RecursivelyCreateBoneProxies(b2);
			}
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x00033364 File Offset: 0x00031764
		private void UpdateSpineSkeleton(ISkeletonAnimation skeletonRenderer)
		{
			bool flipX = this.skeleton.flipX;
			bool flipY = this.skeleton.flipY;
			bool flag = flipX ^ flipY;
			bool flag2 = flipX || flipY;
			foreach (KeyValuePair<Bone, Transform> keyValuePair in this.boneTable)
			{
				Bone key = keyValuePair.Key;
				Transform value = keyValuePair.Value;
				bool flag3 = key == this.StartingBone;
				Transform transform = (!flag3) ? this.boneTable[key.Parent] : this.ragdollRoot;
				Vector3 position = transform.position;
				Quaternion rotation = transform.rotation;
				SkeletonRagdoll.parentSpaceHelper.position = position;
				SkeletonRagdoll.parentSpaceHelper.rotation = rotation;
				SkeletonRagdoll.parentSpaceHelper.localScale = transform.localScale;
				Vector3 position2 = value.position;
				Vector3 vector = SkeletonRagdoll.parentSpaceHelper.InverseTransformDirection(value.right);
				Vector3 vector2 = SkeletonRagdoll.parentSpaceHelper.InverseTransformPoint(position2);
				float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
				if (flag2)
				{
					if (flag3)
					{
						if (flipX)
						{
							vector2.x *= -1f;
						}
						if (flipY)
						{
							vector2.y *= -1f;
						}
						num *= ((!flag) ? 1f : -1f);
						if (flipX)
						{
							num += 180f;
						}
					}
					else if (flag)
					{
						num *= -1f;
						vector2.y *= -1f;
					}
				}
				key.x = Mathf.Lerp(key.x, vector2.x, this.mix);
				key.y = Mathf.Lerp(key.y, vector2.y, this.mix);
				key.rotation = Mathf.Lerp(key.rotation, num, this.mix);
				key.appliedRotation = Mathf.Lerp(key.appliedRotation, num, this.mix);
			}
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x000335BC File Offset: 0x000319BC
		private List<Collider> AttachBoundingBoxRagdollColliders(Bone b)
		{
			List<Collider> list = new List<Collider>();
			Transform transform = this.boneTable[b];
			GameObject gameObject = transform.gameObject;
			Skin skin = this.skeleton.Skin ?? this.skeleton.Data.DefaultSkin;
			List<Attachment> list2 = new List<Attachment>();
			foreach (Slot slot in this.skeleton.Slots)
			{
				if (slot.Bone == b)
				{
					skin.FindAttachmentsForSlot(this.skeleton.Slots.IndexOf(slot), list2);
					foreach (Attachment attachment in list2)
					{
						BoundingBoxAttachment boundingBoxAttachment = attachment as BoundingBoxAttachment;
						if (boundingBoxAttachment != null)
						{
							if (attachment.Name.ToLower().Contains("ragdoll"))
							{
								BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
								Bounds boundingBoxBounds = SkeletonUtility.GetBoundingBoxBounds(boundingBoxAttachment, this.thickness);
								boxCollider.center = boundingBoxBounds.center;
								boxCollider.size = boundingBoxBounds.size;
								list.Add(boxCollider);
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x00033734 File Offset: 0x00031B34
		private static float GetPropagatedRotation(Bone b)
		{
			Bone parent = b.Parent;
			float num = b.AppliedRotation;
			while (parent != null)
			{
				num += parent.AppliedRotation;
				parent = parent.parent;
			}
			return num;
		}

		// Token: 0x0400422C RID: 16940
		private static Transform parentSpaceHelper;

		// Token: 0x0400422D RID: 16941
		[Header("Hierarchy")]
		[SpineBone("", "")]
		public string startingBoneName = string.Empty;

		// Token: 0x0400422E RID: 16942
		[SpineBone("", "")]
		public List<string> stopBoneNames = new List<string>();

		// Token: 0x0400422F RID: 16943
		[Header("Parameters")]
		public bool applyOnStart;

		// Token: 0x04004230 RID: 16944
		[Tooltip("Set RootRigidbody IsKinematic to true when Apply is called.")]
		public bool pinStartBone;

		// Token: 0x04004231 RID: 16945
		[Tooltip("Enable Collision between adjacent ragdoll elements (IE: Neck and Head)")]
		public bool enableJointCollision;

		// Token: 0x04004232 RID: 16946
		public bool useGravity = true;

		// Token: 0x04004233 RID: 16947
		[Tooltip("Warning!  You will have to re-enable and tune mix values manually if attempting to remove the ragdoll system.")]
		public bool disableIK = true;

		// Token: 0x04004234 RID: 16948
		[Tooltip("If no BoundingBox Attachment is attached to a bone, this becomes the default Width or Radius of a Bone's ragdoll Rigidbody")]
		public float thickness = 0.125f;

		// Token: 0x04004235 RID: 16949
		[Tooltip("Default rotational limit value.  Min is negative this value, Max is this value.")]
		public float rotationLimit = 20f;

		// Token: 0x04004236 RID: 16950
		public float rootMass = 20f;

		// Token: 0x04004237 RID: 16951
		[Tooltip("If your ragdoll seems unstable or uneffected by limits, try lowering this value.")]
		[Range(0.01f, 1f)]
		public float massFalloffFactor = 0.4f;

		// Token: 0x04004238 RID: 16952
		[Tooltip("The layer assigned to all of the rigidbody parts.")]
		public int colliderLayer;

		// Token: 0x04004239 RID: 16953
		[Range(0f, 1f)]
		public float mix = 1f;

		// Token: 0x0400423A RID: 16954
		private ISkeletonAnimation targetSkeletonComponent;

		// Token: 0x0400423B RID: 16955
		private Skeleton skeleton;

		// Token: 0x0400423C RID: 16956
		private Dictionary<Bone, Transform> boneTable = new Dictionary<Bone, Transform>();

		// Token: 0x0400423D RID: 16957
		private Transform ragdollRoot;

		// Token: 0x04004240 RID: 16960
		private Vector3 rootOffset;

		// Token: 0x04004241 RID: 16961
		private bool isActive;

		// Token: 0x02000249 RID: 585
		public class LayerFieldAttribute : PropertyAttribute
		{
		}
	}
}
