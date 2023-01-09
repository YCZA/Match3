using System;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Spine.Unity.Modules
{
	// Token: 0x02000252 RID: 594
	public class SpriteAttachmentLoader : AttachmentLoader
	{
		// Token: 0x0600123A RID: 4666 RVA: 0x000357A4 File Offset: 0x00033BA4
		public SpriteAttachmentLoader(Sprite sprite, Shader shader)
		{
			if (sprite.packed && sprite.packingMode == SpritePackingMode.Tight)
			{
				global::UnityEngine.Debug.LogError("Tight Packer Policy not supported yet!");
				return;
			}
			this.sprite = sprite;
			this.shader = shader;
			Texture2D texture = sprite.texture;
			int instanceID = texture.GetInstanceID();
			if (!SpriteAttachmentLoader.premultipliedAtlasIds.Contains(instanceID))
			{
				try
				{
					Color[] pixels = texture.GetPixels();
					for (int i = 0; i < pixels.Length; i++)
					{
						Color color = pixels[i];
						float a = color.a;
						color.r *= a;
						color.g *= a;
						color.b *= a;
						pixels[i] = color;
					}
					texture.SetPixels(pixels);
					texture.Apply();
					SpriteAttachmentLoader.premultipliedAtlasIds.Add(instanceID);
				}
				catch
				{
				}
			}
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x000358AC File Offset: 0x00033CAC
		public RegionAttachment NewRegionAttachment(Skin skin, string name, string path)
		{
			RegionAttachment regionAttachment = new RegionAttachment(name);
			Texture2D texture = this.sprite.texture;
			int instanceID = texture.GetInstanceID();
			AtlasRegion atlasRegion;
			if (SpriteAttachmentLoader.atlasTable.ContainsKey(instanceID))
			{
				atlasRegion = SpriteAttachmentLoader.atlasTable[instanceID];
			}
			else
			{
				Material material = new Material(this.shader);
				if (this.sprite.packed)
				{
					material.name = "Unity Packed Sprite Material";
				}
				else
				{
					material.name = this.sprite.name + " Sprite Material";
				}
				material.mainTexture = texture;
				atlasRegion = new AtlasRegion();
				atlasRegion.page = new AtlasPage
				{
					rendererObject = material
				};
				SpriteAttachmentLoader.atlasTable[instanceID] = atlasRegion;
			}
			Rect textureRect = this.sprite.textureRect;
			textureRect.x = Mathf.InverseLerp(0f, (float)texture.width, textureRect.x);
			textureRect.y = Mathf.InverseLerp(0f, (float)texture.height, textureRect.y);
			textureRect.width = Mathf.InverseLerp(0f, (float)texture.width, textureRect.width);
			textureRect.height = Mathf.InverseLerp(0f, (float)texture.height, textureRect.height);
			Bounds bounds = this.sprite.bounds;
			Vector3 size = bounds.size;
			bool rotate = false;
			if (this.sprite.packed)
			{
				rotate = (this.sprite.packingRotation == SpritePackingRotation.Any);
			}
			regionAttachment.SetUVs(textureRect.xMin, textureRect.yMax, textureRect.xMax, textureRect.yMin, rotate);
			regionAttachment.RendererObject = atlasRegion;
			regionAttachment.SetColor(Color.white);
			regionAttachment.ScaleX = 1f;
			regionAttachment.ScaleY = 1f;
			regionAttachment.RegionOffsetX = this.sprite.rect.width * (0.5f - Mathf.InverseLerp(bounds.min.x, bounds.max.x, 0f)) / this.sprite.pixelsPerUnit;
			regionAttachment.RegionOffsetY = this.sprite.rect.height * (0.5f - Mathf.InverseLerp(bounds.min.y, bounds.max.y, 0f)) / this.sprite.pixelsPerUnit;
			regionAttachment.Width = size.x;
			regionAttachment.Height = size.y;
			regionAttachment.RegionWidth = size.x;
			regionAttachment.RegionHeight = size.y;
			regionAttachment.RegionOriginalWidth = size.x;
			regionAttachment.RegionOriginalHeight = size.y;
			regionAttachment.UpdateOffset();
			return regionAttachment;
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x00035B82 File Offset: 0x00033F82
		public MeshAttachment NewMeshAttachment(Skin skin, string name, string path)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x00035B89 File Offset: 0x00033F89
		public WeightedMeshAttachment NewWeightedMeshAttachment(Skin skin, string name, string path)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x00035B90 File Offset: 0x00033F90
		public BoundingBoxAttachment NewBoundingBoxAttachment(Skin skin, string name)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04004283 RID: 17027
		public static Dictionary<int, AtlasRegion> atlasTable = new Dictionary<int, AtlasRegion>();

		// Token: 0x04004284 RID: 17028
		public static List<int> premultipliedAtlasIds = new List<int>();

		// Token: 0x04004285 RID: 17029
		private Sprite sprite;

		// Token: 0x04004286 RID: 17030
		private Shader shader;
	}
}
