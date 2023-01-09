using System;

namespace Match3.Scripts1.Spine
{
	// Token: 0x02000200 RID: 512
	public class AtlasAttachmentLoader : AttachmentLoader
	{
		// Token: 0x06000ED9 RID: 3801 RVA: 0x0002446B File Offset: 0x0002286B
		public AtlasAttachmentLoader(params Atlas[] atlasArray)
		{
			if (atlasArray == null)
			{
				throw new ArgumentNullException("atlas array cannot be null.");
			}
			this.atlasArray = atlasArray;
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x0002448C File Offset: 0x0002288C
		public RegionAttachment NewRegionAttachment(Skin skin, string name, string path)
		{
			AtlasRegion atlasRegion = this.FindRegion(path);
			if (atlasRegion == null)
			{
				throw new Exception(string.Concat(new string[]
				{
					"Region not found in atlas: ",
					path,
					" (region attachment: ",
					name,
					")"
				}));
			}
			RegionAttachment regionAttachment = new RegionAttachment(name);
			regionAttachment.RendererObject = atlasRegion;
			regionAttachment.SetUVs(atlasRegion.u, atlasRegion.v, atlasRegion.u2, atlasRegion.v2, atlasRegion.rotate);
			regionAttachment.regionOffsetX = atlasRegion.offsetX;
			regionAttachment.regionOffsetY = atlasRegion.offsetY;
			regionAttachment.regionWidth = (float)atlasRegion.width;
			regionAttachment.regionHeight = (float)atlasRegion.height;
			regionAttachment.regionOriginalWidth = (float)atlasRegion.originalWidth;
			regionAttachment.regionOriginalHeight = (float)atlasRegion.originalHeight;
			return regionAttachment;
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x00024558 File Offset: 0x00022958
		public MeshAttachment NewMeshAttachment(Skin skin, string name, string path)
		{
			AtlasRegion atlasRegion = this.FindRegion(path);
			if (atlasRegion == null)
			{
				throw new Exception(string.Concat(new string[]
				{
					"Region not found in atlas: ",
					path,
					" (mesh attachment: ",
					name,
					")"
				}));
			}
			return new MeshAttachment(name)
			{
				RendererObject = atlasRegion,
				RegionU = atlasRegion.u,
				RegionV = atlasRegion.v,
				RegionU2 = atlasRegion.u2,
				RegionV2 = atlasRegion.v2,
				RegionRotate = atlasRegion.rotate,
				regionOffsetX = atlasRegion.offsetX,
				regionOffsetY = atlasRegion.offsetY,
				regionWidth = (float)atlasRegion.width,
				regionHeight = (float)atlasRegion.height,
				regionOriginalWidth = (float)atlasRegion.originalWidth,
				regionOriginalHeight = (float)atlasRegion.originalHeight
			};
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x0002463C File Offset: 0x00022A3C
		public WeightedMeshAttachment NewWeightedMeshAttachment(Skin skin, string name, string path)
		{
			AtlasRegion atlasRegion = this.FindRegion(path);
			if (atlasRegion == null)
			{
				throw new Exception(string.Concat(new string[]
				{
					"Region not found in atlas: ",
					path,
					" (weighted mesh attachment: ",
					name,
					")"
				}));
			}
			return new WeightedMeshAttachment(name)
			{
				RendererObject = atlasRegion,
				RegionU = atlasRegion.u,
				RegionV = atlasRegion.v,
				RegionU2 = atlasRegion.u2,
				RegionV2 = atlasRegion.v2,
				RegionRotate = atlasRegion.rotate,
				regionOffsetX = atlasRegion.offsetX,
				regionOffsetY = atlasRegion.offsetY,
				regionWidth = (float)atlasRegion.width,
				regionHeight = (float)atlasRegion.height,
				regionOriginalWidth = (float)atlasRegion.originalWidth,
				regionOriginalHeight = (float)atlasRegion.originalHeight
			};
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x0002471F File Offset: 0x00022B1F
		public BoundingBoxAttachment NewBoundingBoxAttachment(Skin skin, string name)
		{
			return new BoundingBoxAttachment(name);
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x00024728 File Offset: 0x00022B28
		public AtlasRegion FindRegion(string name)
		{
			for (int i = 0; i < this.atlasArray.Length; i++)
			{
				AtlasRegion atlasRegion = this.atlasArray[i].FindRegion(name);
				if (atlasRegion != null)
				{
					return atlasRegion;
				}
			}
			return null;
		}

		// Token: 0x04004074 RID: 16500
		private Atlas[] atlasArray;
	}
}
