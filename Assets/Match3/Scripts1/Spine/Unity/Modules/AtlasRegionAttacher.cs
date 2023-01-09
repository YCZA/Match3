using System;
using UnityEngine;

namespace Match3.Scripts1.Spine.Unity.Modules
{
	// Token: 0x0200023E RID: 574
	public class AtlasRegionAttacher : MonoBehaviour
	{
		// Token: 0x060011BF RID: 4543 RVA: 0x0003159D File Offset: 0x0002F99D
		private void Awake()
		{
			SkeletonRenderer component = base.GetComponent<SkeletonRenderer>();
			component.OnRebuild = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Combine(component.OnRebuild, new SkeletonRenderer.SkeletonRendererDelegate(this.Apply));
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x000315C8 File Offset: 0x0002F9C8
		private void Apply(SkeletonRenderer skeletonRenderer)
		{
			this.atlas = this.atlasAsset.GetAtlas();
			AtlasAttachmentLoader atlasAttachmentLoader = new AtlasAttachmentLoader(new Atlas[]
			{
				this.atlas
			});
			float scale = skeletonRenderer.skeletonDataAsset.Scale;
			foreach (object obj in this.attachments)
			{
				AtlasRegionAttacher.SlotRegionPair slotRegionPair = (AtlasRegionAttacher.SlotRegionPair)obj;
				RegionAttachment regionAttachment = atlasAttachmentLoader.NewRegionAttachment(null, slotRegionPair.region, slotRegionPair.region);
				regionAttachment.Width = regionAttachment.RegionOriginalWidth * scale;
				regionAttachment.Height = regionAttachment.RegionOriginalHeight * scale;
				regionAttachment.SetColor(new Color(1f, 1f, 1f, 1f));
				regionAttachment.UpdateOffset();
				Slot slot = skeletonRenderer.skeleton.FindSlot(slotRegionPair.slot);
				slot.Attachment = regionAttachment;
			}
		}

		// Token: 0x040041F6 RID: 16886
		public AtlasAsset atlasAsset;

		// Token: 0x040041F7 RID: 16887
		public AtlasRegionAttacher.SlotRegionPair[] attachments;

		// Token: 0x040041F8 RID: 16888
		private Atlas atlas;

		// Token: 0x0200023F RID: 575
		[Serializable]
		public class SlotRegionPair
		{
			// Token: 0x040041F9 RID: 16889
			[SpineSlot("", "", false)]
			public string slot;

			// Token: 0x040041FA RID: 16890
			[SpineAtlasRegion]
			public string region;
		}
	}
}
