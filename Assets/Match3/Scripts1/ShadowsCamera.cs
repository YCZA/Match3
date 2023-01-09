using UnityEngine;

// Token: 0x0200094E RID: 2382
namespace Match3.Scripts1
{
	[RequireComponent(typeof(Camera))]
	public class ShadowsCamera : ReplaceShader
	{
		// Token: 0x060039F5 RID: 14837 RVA: 0x0011DDA4 File Offset: 0x0011C1A4
		protected override void Awake()
		{
			base.Awake();
			this.m_camera.targetTexture = new RenderTexture(this.terrainGenrator.heightmap.width, this.terrainGenrator.heightmap.height, 0, RenderTextureFormat.ARGB32);
			Shader.SetGlobalTexture("_TerrainTex", this.m_camera.targetTexture);
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			materialPropertyBlock.SetTexture("_MainTex", this.m_camera.targetTexture);
			foreach (Renderer renderer in this.terrainGenrator.GetComponentsInChildren<Renderer>())
			{
				if (renderer.material.name.StartsWith(this.terrainGenrator.landMaterial.name))
				{
					renderer.SetPropertyBlock(materialPropertyBlock);
				}
			}
			this.tmpBlurTexture = new RenderTexture(this.terrainGenrator.heightmap.width, this.terrainGenrator.heightmap.height, 0, RenderTextureFormat.ARGB32);
		}

		// Token: 0x060039F6 RID: 14838 RVA: 0x0011DE97 File Offset: 0x0011C297
		public void OnDestroy()
		{
			if (this.tmpBlurTexture)
			{
				this.tmpBlurTexture.Release();
				this.tmpBlurTexture = null;
			}
		}

		// Token: 0x060039F7 RID: 14839 RVA: 0x0011DEBC File Offset: 0x0011C2BC
		public void OnPostRender()
		{
			// eli key point: shader错误
			this.tmpBlurTexture.DiscardContents();
			Graphics.Blit(this.m_camera.activeTexture, this.tmpBlurTexture, this.material, 5);
			this.m_camera.targetTexture.DiscardContents();
			Graphics.Blit(this.tmpBlurTexture, this.m_camera.targetTexture, this.material, 0);
			this.m_camera.targetTexture.MarkRestoreExpected();
			Graphics.Blit(this.terrainGenrator.heightmap, this.m_camera.targetTexture, this.material, 6);
		}

		// Token: 0x0400620E RID: 25102
		[Header("ShadowCamera")]
		public Material material;

		// Token: 0x0400620F RID: 25103
		public TerrainGenerator terrainGenrator;

		// Token: 0x04006210 RID: 25104
		private RenderTexture tmpBlurTexture;
	}
}
