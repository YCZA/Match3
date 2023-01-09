using System;
using System.IO;
using UnityEngine;

namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x02000227 RID: 551
	public class AtlasAsset : ScriptableObject
	{
		// Token: 0x0600116B RID: 4459 RVA: 0x0002EA28 File Offset: 0x0002CE28
		public void Reset()
		{
			this.atlas = null;
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x0002EA34 File Offset: 0x0002CE34
		public Atlas GetAtlas()
		{
			if (this.atlasFile == null)
			{
				global::UnityEngine.Debug.LogError("Atlas file not set for atlas asset: " + base.name, this);
				this.Reset();
				return null;
			}
			if (this.materials == null || this.materials.Length == 0)
			{
				global::UnityEngine.Debug.LogError("Materials not set for atlas asset: " + base.name, this);
				this.Reset();
				return null;
			}
			if (this.atlas != null)
			{
				return this.atlas;
			}
			Atlas result;
			try
			{
				this.atlas = new Atlas(new StringReader(this.atlasFile.text), string.Empty, new MaterialsTextureLoader(this));
				this.atlas.FlipV();
				result = this.atlas;
			}
			catch (Exception ex)
			{
				global::UnityEngine.Debug.LogError(string.Concat(new string[]
				{
					"Error reading atlas file for atlas asset: ",
					base.name,
					"\n",
					ex.Message,
					"\n",
					ex.StackTrace
				}), this);
				result = null;
			}
			return result;
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x0002EB54 File Offset: 0x0002CF54
		public Sprite GenerateSprite(string name, out Material material)
		{
			AtlasRegion atlasRegion = this.atlas.FindRegion(name);
			Sprite result = null;
			material = null;
			if (atlasRegion != null)
			{
			}
			return result;
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x0002EB7C File Offset: 0x0002CF7C
		public Mesh GenerateMesh(string name, Mesh mesh, out Material material, float scale = 0.01f)
		{
			AtlasRegion atlasRegion = this.atlas.FindRegion(name);
			material = null;
			if (atlasRegion != null)
			{
				if (mesh == null)
				{
					mesh = new Mesh();
					mesh.name = name;
				}
				Vector3[] array = new Vector3[4];
				Vector2[] array2 = new Vector2[4];
				Color[] colors = new Color[]
				{
					Color.white,
					Color.white,
					Color.white,
					Color.white
				};
				int[] triangles = new int[]
				{
					0,
					1,
					2,
					2,
					3,
					0
				};
				float num = (float)atlasRegion.width / -2f;
				float x = num * -1f;
				float num2 = (float)atlasRegion.height / 2f;
				float y = num2 * -1f;
				array[0] = new Vector3(num, y, 0f) * scale;
				array[1] = new Vector3(num, num2, 0f) * scale;
				array[2] = new Vector3(x, num2, 0f) * scale;
				array[3] = new Vector3(x, y, 0f) * scale;
				float u = atlasRegion.u;
				float v = atlasRegion.v;
				float u2 = atlasRegion.u2;
				float v2 = atlasRegion.v2;
				if (!atlasRegion.rotate)
				{
					array2[0] = new Vector2(u, v2);
					array2[1] = new Vector2(u, v);
					array2[2] = new Vector2(u2, v);
					array2[3] = new Vector2(u2, v2);
				}
				else
				{
					array2[0] = new Vector2(u2, v2);
					array2[1] = new Vector2(u, v2);
					array2[2] = new Vector2(u, v);
					array2[3] = new Vector2(u2, v);
				}
				mesh.triangles = new int[0];
				mesh.vertices = array;
				mesh.uv = array2;
				mesh.colors = colors;
				mesh.triangles = triangles;
				mesh.RecalculateNormals();
				mesh.RecalculateBounds();
				material = (Material)atlasRegion.page.rendererObject;
			}
			else
			{
				mesh = null;
			}
			return mesh;
		}

		// Token: 0x040041AC RID: 16812
		public TextAsset atlasFile;

		// Token: 0x040041AD RID: 16813
		public Material[] materials;

		// Token: 0x040041AE RID: 16814
		private Atlas atlas;
	}
}
