using System;

namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x02000273 RID: 627
	public class SpineAttachment : SpineAttributeBase
	{
		// Token: 0x06001303 RID: 4867 RVA: 0x0003C2DC File Offset: 0x0003A6DC
		public SpineAttachment(bool currentSkinOnly = true, bool returnAttachmentPath = false, bool placeholdersOnly = false, string slotField = "", string dataField = "")
		{
			this.currentSkinOnly = currentSkinOnly;
			this.returnAttachmentPath = returnAttachmentPath;
			this.placeholdersOnly = placeholdersOnly;
			this.slotField = slotField;
			this.dataField = dataField;
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x0003C314 File Offset: 0x0003A714
		public static SpineAttachment.Hierarchy GetHierarchy(string fullPath)
		{
			return new SpineAttachment.Hierarchy(fullPath);
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x0003C31C File Offset: 0x0003A71C
		public static Attachment GetAttachment(string attachmentPath, SkeletonData skeletonData)
		{
			SpineAttachment.Hierarchy hierarchy = SpineAttachment.GetHierarchy(attachmentPath);
			if (hierarchy.name == string.Empty)
			{
				return null;
			}
			return skeletonData.FindSkin(hierarchy.skin).GetAttachment(skeletonData.FindSlotIndex(hierarchy.slot), hierarchy.name);
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x0003C36E File Offset: 0x0003A76E
		public static Attachment GetAttachment(string attachmentPath, SkeletonDataAsset skeletonDataAsset)
		{
			return SpineAttachment.GetAttachment(attachmentPath, skeletonDataAsset.GetSkeletonData(true));
		}

		// Token: 0x04004347 RID: 17223
		public bool returnAttachmentPath;

		// Token: 0x04004348 RID: 17224
		public bool currentSkinOnly;

		// Token: 0x04004349 RID: 17225
		public bool placeholdersOnly;

		// Token: 0x0400434A RID: 17226
		public string slotField = string.Empty;

		// Token: 0x02000274 RID: 628
		public struct Hierarchy
		{
			// Token: 0x06001307 RID: 4871 RVA: 0x0003C380 File Offset: 0x0003A780
			public Hierarchy(string fullPath)
			{
				string[] array = fullPath.Split(new char[]
				{
					'/'
				}, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length == 0)
				{
					this.skin = string.Empty;
					this.slot = string.Empty;
					this.name = string.Empty;
					return;
				}
				if (array.Length < 2)
				{
					throw new Exception("Cannot generate Attachment Hierarchy from string! Not enough components! [" + fullPath + "]");
				}
				this.skin = array[0];
				this.slot = array[1];
				this.name = string.Empty;
				for (int i = 2; i < array.Length; i++)
				{
					this.name += array[i];
				}
			}

			// Token: 0x0400434B RID: 17227
			public string skin;

			// Token: 0x0400434C RID: 17228
			public string slot;

			// Token: 0x0400434D RID: 17229
			public string name;
		}
	}
}
