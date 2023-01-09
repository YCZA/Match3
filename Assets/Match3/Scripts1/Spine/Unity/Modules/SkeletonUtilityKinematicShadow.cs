using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Spine.Unity.Modules
{
	// Token: 0x0200024F RID: 591
	public class SkeletonUtilityKinematicShadow : MonoBehaviour
	{
		// Token: 0x06001234 RID: 4660 RVA: 0x000353FC File Offset: 0x000337FC
		private void Start()
		{
			this.shadowRoot = global::UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
			global::UnityEngine.Object.Destroy(this.shadowRoot.GetComponent<SkeletonUtilityKinematicShadow>());
			Transform transform = this.shadowRoot.transform;
			transform.position = base.transform.position;
			transform.rotation = base.transform.rotation;
			Vector3 b = base.transform.TransformPoint(Vector3.right);
			float d = Vector3.Distance(base.transform.position, b);
			transform.localScale = Vector3.one;
			if (!this.detachedShadow)
			{
				if (this.parent == null)
				{
					transform.parent = base.transform.root;
				}
				else
				{
					transform.parent = this.parent;
				}
			}
			if (this.hideShadow)
			{
				this.shadowRoot.hideFlags = HideFlags.HideInHierarchy;
			}
			Joint[] componentsInChildren = this.shadowRoot.GetComponentsInChildren<Joint>();
			foreach (Joint joint in componentsInChildren)
			{
				joint.connectedAnchor *= d;
			}
			SkeletonUtilityBone[] componentsInChildren2 = base.GetComponentsInChildren<SkeletonUtilityBone>();
			SkeletonUtilityBone[] componentsInChildren3 = this.shadowRoot.GetComponentsInChildren<SkeletonUtilityBone>();
			foreach (SkeletonUtilityBone skeletonUtilityBone in componentsInChildren2)
			{
				if (!(skeletonUtilityBone.gameObject == base.gameObject))
				{
					foreach (SkeletonUtilityBone skeletonUtilityBone2 in componentsInChildren3)
					{
						if (skeletonUtilityBone2.GetComponent<Rigidbody>() != null && skeletonUtilityBone2.boneName == skeletonUtilityBone.boneName)
						{
							this.shadowTable.Add(new SkeletonUtilityKinematicShadow.TransformPair
							{
								dest = skeletonUtilityBone.transform,
								src = skeletonUtilityBone2.transform
							});
							break;
						}
					}
				}
			}
			SkeletonUtilityKinematicShadow.DestroyComponents(componentsInChildren3);
			SkeletonUtilityKinematicShadow.DestroyComponents(base.GetComponentsInChildren<Joint>());
			SkeletonUtilityKinematicShadow.DestroyComponents(base.GetComponentsInChildren<Rigidbody>());
			SkeletonUtilityKinematicShadow.DestroyComponents(base.GetComponentsInChildren<Collider>());
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x00035620 File Offset: 0x00033A20
		private static void DestroyComponents(Component[] components)
		{
			int i = 0;
			int num = components.Length;
			while (i < num)
			{
				global::UnityEngine.Object.Destroy(components[i]);
				i++;
			}
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x0003564C File Offset: 0x00033A4C
		private void FixedUpdate()
		{
			Rigidbody component = this.shadowRoot.GetComponent<Rigidbody>();
			component.MovePosition(base.transform.position);
			component.MoveRotation(base.transform.rotation);
			int i = 0;
			int count = this.shadowTable.Count;
			while (i < count)
			{
				SkeletonUtilityKinematicShadow.TransformPair transformPair = this.shadowTable[i];
				transformPair.dest.localPosition = transformPair.src.localPosition;
				transformPair.dest.localRotation = transformPair.src.localRotation;
				i++;
			}
		}

		// Token: 0x04004276 RID: 17014
		[Tooltip("If checked, the hinge chain can inherit your root transform's velocity or position/rotation changes.")]
		public bool detachedShadow;

		// Token: 0x04004277 RID: 17015
		public Transform parent;

		// Token: 0x04004278 RID: 17016
		public bool hideShadow = true;

		// Token: 0x04004279 RID: 17017
		private GameObject shadowRoot;

		// Token: 0x0400427A RID: 17018
		private readonly List<SkeletonUtilityKinematicShadow.TransformPair> shadowTable = new List<SkeletonUtilityKinematicShadow.TransformPair>();

		// Token: 0x02000250 RID: 592
		private struct TransformPair
		{
			// Token: 0x0400427B RID: 17019
			public Transform dest;

			// Token: 0x0400427C RID: 17020
			public Transform src;
		}
	}
}
