using System.IO;
using UnityEngine;

namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x02000228 RID: 552
	public class MaterialsTextureLoader : TextureLoader
	{
		// Token: 0x0600116F RID: 4463 RVA: 0x0002EE03 File Offset: 0x0002D203
		public MaterialsTextureLoader(AtlasAsset atlasAsset)
		{
			this.atlasAsset = atlasAsset;
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x0002EE14 File Offset: 0x0002D214
		public void Load(AtlasPage page, string path)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
			Material material = null;
			foreach (Material material2 in this.atlasAsset.materials)
			{
				if (material2.mainTexture == null)
				{
					global::UnityEngine.Debug.LogError("Material is missing texture: " + material2.name, material2);
					return;
				}
				if (material2.mainTexture.name == fileNameWithoutExtension)
				{
					material = material2;
					break;
				}
			}
			if (material == null)
			{
				global::UnityEngine.Debug.LogError("Material with texture name \"" + fileNameWithoutExtension + "\" not found for atlas asset: " + this.atlasAsset.name, this.atlasAsset);
				return;
			}
			page.rendererObject = material;
			if (page.width == 0 || page.height == 0)
			{
				page.width = material.mainTexture.width;
				page.height = material.mainTexture.height;
			}
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x0002EF0A File Offset: 0x0002D30A
		public void Unload(object texture)
		{
		}

		// Token: 0x040041AF RID: 16815
		private AtlasAsset atlasAsset;
	}
}
