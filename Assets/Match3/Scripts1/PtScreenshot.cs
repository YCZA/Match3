using UnityEngine;

// Token: 0x02000B52 RID: 2898
namespace Match3.Scripts1
{
	public static class PtScreenshot
	{
		// Token: 0x060043CF RID: 17359 RVA: 0x0015A398 File Offset: 0x00158798
		public static Texture2D TakeScreenshot(int wantedHeight, RectTransform rect, PtScreenshot.RenderCallback render)
		{
			Rect relativeRect = rect.GetRelativeRect();
			float num = (wantedHeight <= 0) ? 1f : ((float)wantedHeight / (relativeRect.height * (float)Screen.height));
			int height = (int)((float)Screen.height * num);
			int width = (int)((float)Screen.width * num);
			RenderTexture renderTexture = new RenderTexture(width, height, 24);
			renderTexture.Create();
			render(renderTexture);
			Texture2D result = PtScreenshot.CropTexture(renderTexture, rect);
			renderTexture.Release();
			global::UnityEngine.Object.Destroy(renderTexture);
			return result;
		}

		// Token: 0x060043D0 RID: 17360 RVA: 0x0015A418 File Offset: 0x00158818
		public static Texture2D TakeScreenshot(int wantedHeight, RectTransform rect, Camera[] cameras)
		{
			return PtScreenshot.TakeScreenshot(wantedHeight, rect, delegate(RenderTexture rt)
			{
				foreach (Camera cam in cameras)
				{
					PtScreenshot.RenderToTexture(cam, rt);
				}
			});
		}

		// Token: 0x060043D1 RID: 17361 RVA: 0x0015A445 File Offset: 0x00158845
		public static void RenderToTexture(Camera cam, RenderTexture rt)
		{
			cam.targetTexture = rt;
			cam.Render();
			cam.targetTexture = null;
		}

		// Token: 0x060043D2 RID: 17362 RVA: 0x0015A45C File Offset: 0x0015885C
		private static Texture2D CropTexture(RenderTexture rt, RectTransform rect)
		{
			RenderTexture.active = rt;
			Rect relativeRect = rect.GetRelativeRect();
			int num = (int)(relativeRect.width * (float)rt.width);
			int num2 = (int)(relativeRect.height * (float)rt.height);
			Texture2D texture2D = new Texture2D(num, num2, TextureFormat.RGB24, false);
			Rect source = new Rect((float)((int)(relativeRect.x * (float)rt.width)), (float)((int)(relativeRect.y * (float)rt.height)), (float)num, (float)num2);
			texture2D.ReadPixels(source, 0, 0);
			RenderTexture.active = null;
			return texture2D;
		}

		// Token: 0x02000B53 RID: 2899
		// (Invoke) Token: 0x060043D4 RID: 17364
		public delegate void RenderCallback(RenderTexture rt);
	}
}
