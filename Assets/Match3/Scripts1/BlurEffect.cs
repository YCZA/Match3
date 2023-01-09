using UnityEngine;

// Token: 0x02000973 RID: 2419
namespace Match3.Scripts1
{
	public class BlurEffect : MonoBehaviour
	{
		// Token: 0x06003B03 RID: 15107 RVA: 0x00124200 File Offset: 0x00122600
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(source.width / this.downsample, source.height / this.downsample, 0, source.format);
			RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / this.downsample, source.height / this.downsample, 0, source.format);
			Graphics.Blit(source, temporary, this.postProcess, 0);
			Graphics.Blit(temporary, temporary2, this.postProcess, 1);
			Graphics.Blit(temporary2, destination, this.postProcess, 2);
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
		}

		// Token: 0x040062E0 RID: 25312
		public Material postProcess;

		// Token: 0x040062E1 RID: 25313
		public int downsample = 2;
	}
}
