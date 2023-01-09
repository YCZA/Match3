using UnityEngine;

namespace Match3.Scripts1.Spine.Unity.Modules
{
	// Token: 0x02000253 RID: 595
	public static class SpriteAttachmentExtensions
	{
		// Token: 0x06001240 RID: 4672 RVA: 0x00035BB0 File Offset: 0x00033FB0
		public static Attachment AttachUnitySprite(this Skeleton skeleton, string slotName, Sprite sprite, string shaderName = "Spine/Skeleton")
		{
			RegionAttachment regionAttachment = sprite.ToRegionAttachment(shaderName);
			skeleton.FindSlot(slotName).Attachment = regionAttachment;
			return regionAttachment;
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x00035BD4 File Offset: 0x00033FD4
		public static Attachment AddUnitySprite(this SkeletonData skeletonData, string slotName, Sprite sprite, string skinName = "", string shaderName = "Spine/Skeleton")
		{
			RegionAttachment regionAttachment = sprite.ToRegionAttachment(shaderName);
			int slotIndex = skeletonData.FindSlotIndex(slotName);
			Skin skin = skeletonData.defaultSkin;
			if (skinName != string.Empty)
			{
				skin = skeletonData.FindSkin(skinName);
			}
			skin.AddAttachment(slotIndex, regionAttachment.Name, regionAttachment);
			return regionAttachment;
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x00035C20 File Offset: 0x00034020
		public static RegionAttachment ToRegionAttachment(this Sprite sprite, string shaderName = "Spine/Skeleton")
		{
			SpriteAttachmentLoader spriteAttachmentLoader = new SpriteAttachmentLoader(sprite, Shader.Find(shaderName));
			return spriteAttachmentLoader.NewRegionAttachment(null, sprite.name, string.Empty);
		}
	}
}
