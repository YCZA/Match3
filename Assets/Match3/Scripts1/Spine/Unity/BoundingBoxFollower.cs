using System;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x02000240 RID: 576
	[ExecuteInEditMode]
	public class BoundingBoxFollower : MonoBehaviour
	{
		// Token: 0x17000289 RID: 649
		// (get) Token: 0x060011C3 RID: 4547 RVA: 0x000316CD File Offset: 0x0002FACD
		public string CurrentAttachmentName
		{
			get
			{
				return this.currentAttachmentName;
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x060011C4 RID: 4548 RVA: 0x000316D5 File Offset: 0x0002FAD5
		public BoundingBoxAttachment CurrentAttachment
		{
			get
			{
				return this.currentAttachment;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x060011C5 RID: 4549 RVA: 0x000316DD File Offset: 0x0002FADD
		public PolygonCollider2D CurrentCollider
		{
			get
			{
				return this.currentCollider;
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x060011C6 RID: 4550 RVA: 0x000316E5 File Offset: 0x0002FAE5
		public Slot Slot
		{
			get
			{
				return this.slot;
			}
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x000316F0 File Offset: 0x0002FAF0
		private void OnEnable()
		{
			this.ClearColliders();
			if (this.skeletonRenderer == null)
			{
				this.skeletonRenderer = base.GetComponentInParent<SkeletonRenderer>();
			}
			if (this.skeletonRenderer != null)
			{
				SkeletonRenderer skeletonRenderer = this.skeletonRenderer;
				skeletonRenderer.OnRebuild = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Remove(skeletonRenderer.OnRebuild, new SkeletonRenderer.SkeletonRendererDelegate(this.HandleReset));
				SkeletonRenderer skeletonRenderer2 = this.skeletonRenderer;
				skeletonRenderer2.OnRebuild = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Combine(skeletonRenderer2.OnRebuild, new SkeletonRenderer.SkeletonRendererDelegate(this.HandleReset));
				if (this.hasReset)
				{
					this.HandleReset(this.skeletonRenderer);
				}
			}
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x00031796 File Offset: 0x0002FB96
		private void OnDisable()
		{
			SkeletonRenderer skeletonRenderer = this.skeletonRenderer;
			skeletonRenderer.OnRebuild = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Remove(skeletonRenderer.OnRebuild, new SkeletonRenderer.SkeletonRendererDelegate(this.HandleReset));
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x000317BF File Offset: 0x0002FBBF
		private void Start()
		{
			if (!this.hasReset && this.skeletonRenderer != null)
			{
				this.HandleReset(this.skeletonRenderer);
			}
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x000317EC File Offset: 0x0002FBEC
		public void HandleReset(SkeletonRenderer renderer)
		{
			if (this.slotName == null || this.slotName == string.Empty)
			{
				return;
			}
			this.hasReset = true;
			this.ClearColliders();
			this.colliderTable.Clear();
			if (this.skeletonRenderer.skeleton == null)
			{
				SkeletonRenderer skeletonRenderer = this.skeletonRenderer;
				skeletonRenderer.OnRebuild = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Remove(skeletonRenderer.OnRebuild, new SkeletonRenderer.SkeletonRendererDelegate(this.HandleReset));
				this.skeletonRenderer.Initialize(false);
				SkeletonRenderer skeletonRenderer2 = this.skeletonRenderer;
				skeletonRenderer2.OnRebuild = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Combine(skeletonRenderer2.OnRebuild, new SkeletonRenderer.SkeletonRendererDelegate(this.HandleReset));
			}
			Skeleton skeleton = this.skeletonRenderer.skeleton;
			this.slot = skeleton.FindSlot(this.slotName);
			int slotIndex = skeleton.FindSlotIndex(this.slotName);
			foreach (Skin skin in skeleton.Data.Skins)
			{
				List<string> list = new List<string>();
				skin.FindNamesForSlot(slotIndex, list);
				foreach (string text in list)
				{
					Attachment attachment = skin.GetAttachment(slotIndex, text);
					if (attachment is BoundingBoxAttachment)
					{
						PolygonCollider2D polygonCollider2D = SkeletonUtility.AddBoundingBoxAsComponent((BoundingBoxAttachment)attachment, base.gameObject, true);
						polygonCollider2D.enabled = false;
						polygonCollider2D.hideFlags = HideFlags.HideInInspector;
						this.colliderTable.Add((BoundingBoxAttachment)attachment, polygonCollider2D);
						this.attachmentNameTable.Add((BoundingBoxAttachment)attachment, text);
					}
				}
			}
			if (this.colliderTable.Count == 0)
			{
				this.valid = false;
			}
			else
			{
				this.valid = true;
			}
			if (!this.valid)
			{
				global::UnityEngine.Debug.LogWarning("Bounding Box Follower not valid! Slot [" + this.slotName + "] does not contain any Bounding Box Attachments!");
			}
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x00031A14 File Offset: 0x0002FE14
		private void ClearColliders()
		{
			PolygonCollider2D[] components = base.GetComponents<PolygonCollider2D>();
			if (Application.isPlaying)
			{
				foreach (PolygonCollider2D obj in components)
				{
					global::UnityEngine.Object.Destroy(obj);
				}
			}
			else
			{
				foreach (PolygonCollider2D obj2 in components)
				{
					global::UnityEngine.Object.DestroyImmediate(obj2);
				}
			}
			this.colliderTable.Clear();
			this.attachmentNameTable.Clear();
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x00031A98 File Offset: 0x0002FE98
		private void LateUpdate()
		{
			if (!this.skeletonRenderer.valid)
			{
				return;
			}
			if (this.slot != null && this.slot.Attachment != this.currentAttachment)
			{
				this.SetCurrent((BoundingBoxAttachment)this.slot.Attachment);
			}
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x00031AF0 File Offset: 0x0002FEF0
		private void SetCurrent(BoundingBoxAttachment attachment)
		{
			if (this.currentCollider)
			{
				this.currentCollider.enabled = false;
			}
			if (attachment != null)
			{
				this.currentCollider = this.colliderTable[attachment];
				this.currentCollider.enabled = true;
			}
			else
			{
				this.currentCollider = null;
			}
			this.currentAttachment = attachment;
			this.currentAttachmentName = ((this.currentAttachment != null) ? this.attachmentNameTable[attachment] : null);
		}

		// Token: 0x040041FB RID: 16891
		public SkeletonRenderer skeletonRenderer;

		// Token: 0x040041FC RID: 16892
		[SpineSlot("", "skeletonRenderer", true)]
		public string slotName;

		// Token: 0x040041FD RID: 16893
		[Tooltip("LOL JK, Someone else do it!")]
		public bool use3DMeshCollider;

		// Token: 0x040041FE RID: 16894
		private Slot slot;

		// Token: 0x040041FF RID: 16895
		private BoundingBoxAttachment currentAttachment;

		// Token: 0x04004200 RID: 16896
		private PolygonCollider2D currentCollider;

		// Token: 0x04004201 RID: 16897
		private string currentAttachmentName;

		// Token: 0x04004202 RID: 16898
		private bool valid;

		// Token: 0x04004203 RID: 16899
		private bool hasReset;

		// Token: 0x04004204 RID: 16900
		public Dictionary<BoundingBoxAttachment, PolygonCollider2D> colliderTable = new Dictionary<BoundingBoxAttachment, PolygonCollider2D>();

		// Token: 0x04004205 RID: 16901
		public Dictionary<BoundingBoxAttachment, string> attachmentNameTable = new Dictionary<BoundingBoxAttachment, string>();
	}
}
