using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Match3.Scripts1.Spine.Unity.Modules
{
	// Token: 0x02000244 RID: 580
	public class CustomSkin : MonoBehaviour
	{
		// Token: 0x060011D8 RID: 4568 RVA: 0x00031F38 File Offset: 0x00030338
		private void Start()
		{
			this.skeletonRenderer = base.GetComponent<SkeletonRenderer>();
			Skeleton skeleton = this.skeletonRenderer.skeleton;
			this.customSkin = new Skin("CustomSkin");
			foreach (CustomSkin.SkinPair skinPair in this.skinItems)
			{
				Attachment attachment = SpineAttachment.GetAttachment(skinPair.sourceAttachmentPath, this.skinSource);
				this.customSkin.AddAttachment(skeleton.FindSlotIndex(skinPair.targetSlot), skinPair.targetAttachment, attachment);
			}
			skeleton.SetSkin(this.customSkin);
		}

		// Token: 0x0400420F RID: 16911
		public SkeletonDataAsset skinSource;

		// Token: 0x04004210 RID: 16912
		[FormerlySerializedAs("skinning")]
		public CustomSkin.SkinPair[] skinItems;

		// Token: 0x04004211 RID: 16913
		public Skin customSkin;

		// Token: 0x04004212 RID: 16914
		private SkeletonRenderer skeletonRenderer;

		// Token: 0x02000245 RID: 581
		[Serializable]
		public class SkinPair
		{
			// Token: 0x04004213 RID: 16915
			[SpineAttachment(false, true, false, "", "skinSource")]
			[FormerlySerializedAs("sourceAttachment")]
			public string sourceAttachmentPath;

			// Token: 0x04004214 RID: 16916
			[SpineSlot("", "", false)]
			public string targetSlot;

			// Token: 0x04004215 RID: 16917
			[SpineAttachment(true, false, true, "", "")]
			public string targetAttachment;
		}
	}
}
