using UnityEngine;

// Token: 0x02000974 RID: 2420
namespace Match3.Scripts1
{
	[RequireComponent(typeof(Camera))]
	public class BlurTextureOnScreen : MonoBehaviour
	{
		// Token: 0x06003B05 RID: 15109 RVA: 0x001242BC File Offset: 0x001226BC
		private void Start()
		{
			this.m_targetTexture = new RenderTexture(256, 256, 0)
			{
				name = "Target"
			};
			this.m_sourceTexture = new RenderTexture(Screen.width, Screen.height, 0)
			{
				name = "Source"
			};
			this.m_material = new Material(Resources.Load<Material>("iso_blur"));
			this.m_sourceInitialized = false;
		}

		// Token: 0x06003B06 RID: 15110 RVA: 0x0012432B File Offset: 0x0012272B
		private void OnDestroy()
		{
			this.ReleaseTextures();
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x00124334 File Offset: 0x00122734
		private void ReleaseTextures()
		{
			BlurTextureOnScreen.ReleaseAndDestroy(this.m_targetTexture);
			BlurTextureOnScreen.ReleaseAndDestroy(this.m_sourceTexture);
			BlurTextureOnScreen.ReleaseAndDestroy(this.m_crossfadeTarget);
			BlurTextureOnScreen.ReleaseAndDestroy(this.m_crossfadeSource);
			this.m_targetTexture = null;
			this.m_sourceTexture = null;
			this.m_crossfadeTarget = null;
			this.m_crossfadeSource = null;
		}

		// Token: 0x06003B08 RID: 15112 RVA: 0x0012438C File Offset: 0x0012278C
		public void MoveTexturesTo(BlurTextureOnScreen other)
		{
			BlurTextureOnScreen.ReleaseAndDestroy(other.m_crossfadeSource);
			BlurTextureOnScreen.ReleaseAndDestroy(other.m_crossfadeTarget);
			if (this.m_crossfadeSource)
			{
				other.m_crossfadeSource = this.m_crossfadeSource;
				this.m_crossfadeSource = null;
			}
			else
			{
				other.m_crossfadeSource = this.m_sourceTexture;
				this.m_sourceTexture = null;
			}
			if (this.m_crossfadeTarget)
			{
				other.m_crossfadeTarget = this.m_crossfadeTarget;
				this.m_crossfadeTarget = null;
			}
			else
			{
				other.m_crossfadeTarget = this.m_targetTexture;
				this.m_targetTexture = null;
			}
			other.m_crossfade = 0f;
			this.ReleaseTextures();
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x00124436 File Offset: 0x00122836
		private void Update()
		{
			this.m_crossfade = Mathf.Clamp01(this.m_crossfade + Time.deltaTime / 0.25f);
		}

		// Token: 0x06003B0A RID: 15114 RVA: 0x00124458 File Offset: 0x00122858
		private void OnPostRender()
		{
			if (!this.m_targetTexture || !this.m_sourceTexture)
			{
				return;
			}
			if (!this.m_sourceInitialized)
			{
				// source: 为null则表示把当前屏幕渲染到dest?
				// Graphics.Blit(null, this.m_sourceTexture); // 不清楚为什么这个会输出倒置的图像
				// 改用截图的方式来获取m_sourceTexture
				var t = ScreenCapture.CaptureScreenshotAsTexture();
				Graphics.Blit(t, m_sourceTexture);
				this.m_sourceInitialized = true;
			}
			this.m_material.SetFloat("_Crossfade", this.m_crossfade);
			this.m_material.SetTexture("_CrossfadeTargetTex", (!this.m_crossfadeTarget) ? this.m_targetTexture : this.m_crossfadeTarget);
			this.m_material.SetTexture("_CrossfadeSourceTex", (!this.m_crossfadeSource) ? this.m_sourceTexture : this.m_crossfadeSource);
			if (this.amount != this.previousAmount)
			{
				this.m_material.SetFloat("_Amount", this.amount);
				this.m_material.SetTexture("_HighDefTex", this.m_sourceTexture);
				RenderTexture temporary = RenderTexture.GetTemporary(this.m_targetTexture.width, this.m_targetTexture.height, 0, this.m_targetTexture.format);
				RenderTexture temporary2 = RenderTexture.GetTemporary(this.m_targetTexture.width, this.m_targetTexture.height, 0, this.m_targetTexture.format);
				Graphics.Blit(this.m_sourceTexture, temporary, this.m_material, 0);
				Graphics.Blit(temporary, temporary2, this.m_material, 1);
				this.m_targetTexture.DiscardContents();
				Graphics.Blit(temporary2, this.m_targetTexture, this.m_material, 2);
				RenderTexture.ReleaseTemporary(temporary);
				RenderTexture.ReleaseTemporary(temporary2);
				this.previousAmount = this.amount;
			}
			// dest: 目标 RenderTexture。设置为 null 将直接对屏幕执行 blit 操作
			Graphics.Blit(m_targetTexture, null, this.m_material, (this.m_crossfade >= 1f) ? 4 : 7);
		}

		// Token: 0x06003B0B RID: 15115 RVA: 0x00124630 File Offset: 0x00122A30
		private static void ReleaseAndDestroy(RenderTexture texture)
		{
			if (!texture)
			{
				return;
			}
			if (RenderTexture.active == texture)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Releasing active render texture!"
				});
				RenderTexture.active = null;
			}
			texture.Release();
			global::UnityEngine.Object.Destroy(texture);
		}

		// Token: 0x040062E2 RID: 25314
		private const int BLUR_SIZE = 256;

		// Token: 0x040062E3 RID: 25315
		private const float CROSSFADE_TIME = 0.25f;

		// Token: 0x040062E4 RID: 25316
		private Material m_material;

		// Token: 0x040062E5 RID: 25317
		private RenderTexture m_sourceTexture;

		// Token: 0x040062E6 RID: 25318
		private RenderTexture m_targetTexture;

		// Token: 0x040062E7 RID: 25319
		private RenderTexture m_crossfadeSource;

		// Token: 0x040062E8 RID: 25320
		private RenderTexture m_crossfadeTarget;

		// Token: 0x040062E9 RID: 25321
		private bool m_sourceInitialized;

		// Token: 0x040062EA RID: 25322
		private float m_crossfade = 1f;

		// Token: 0x040062EB RID: 25323
		[Range(0f, 1f)]
		public float amount = 1f;

		// Token: 0x040062EC RID: 25324
		private float previousAmount = -1f;
	}
}
