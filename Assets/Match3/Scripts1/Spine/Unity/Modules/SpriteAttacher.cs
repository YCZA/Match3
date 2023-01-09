using UnityEngine;

namespace Match3.Scripts1.Spine.Unity.Modules
{
	// Token: 0x02000251 RID: 593
	public class SpriteAttacher : MonoBehaviour
	{
		// Token: 0x06001238 RID: 4664 RVA: 0x000356F8 File Offset: 0x00033AF8
		private void Start()
		{
			if (this.attachOnStart)
			{
				this.Attach();
			}
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x0003570C File Offset: 0x00033B0C
		public void Attach()
		{
			SkeletonRenderer component = base.GetComponent<SkeletonRenderer>();
			if (this.loader == null)
			{
				this.loader = new SpriteAttachmentLoader(this.sprite, Shader.Find("Spine/Skeleton"));
			}
			if (this.attachment == null)
			{
				this.attachment = this.loader.NewRegionAttachment(null, this.sprite.name, string.Empty);
			}
			component.skeleton.FindSlot(this.slot).Attachment = this.attachment;
			if (!this.keepLoaderInMemory)
			{
				this.loader = null;
			}
		}

		// Token: 0x0400427D RID: 17021
		public bool attachOnStart = true;

		// Token: 0x0400427E RID: 17022
		public bool keepLoaderInMemory = true;

		// Token: 0x0400427F RID: 17023
		public Sprite sprite;

		// Token: 0x04004280 RID: 17024
		[SpineSlot("", "", false)]
		public string slot;

		// Token: 0x04004281 RID: 17025
		private SpriteAttachmentLoader loader;

		// Token: 0x04004282 RID: 17026
		private RegionAttachment attachment;
	}
}
