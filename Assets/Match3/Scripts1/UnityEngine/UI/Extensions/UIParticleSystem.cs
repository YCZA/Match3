using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BE0 RID: 3040
	[ExecuteInEditMode]
	[RequireComponent(typeof(CanvasRenderer), typeof(ParticleSystem))]
	[AddComponentMenu("UI/Effects/Extensions/UIParticleSystem")]
	public class UIParticleSystem : MaskableGraphic
	{
		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x06004745 RID: 18245 RVA: 0x0016B31E File Offset: 0x0016971E
		public override Texture mainTexture
		{
			get
			{
				return this.currentTexture;
			}
		}

		// Token: 0x06004746 RID: 18246 RVA: 0x0016B328 File Offset: 0x00169728
		protected bool Initialize()
		{
			if (this._transform == null)
			{
				this._transform = base.transform;
			}
			if (this.pSystem == null)
			{
				this.pSystem = base.GetComponent<ParticleSystem>();
				if (this.pSystem == null)
				{
					return false;
				}
				this.mainModule = this.pSystem.main;
				if (this.pSystem.main.maxParticles > 14000)
				{
					this.mainModule.maxParticles = 14000;
				}
				this.pRenderer = this.pSystem.GetComponent<ParticleSystemRenderer>();
				if (this.pRenderer != null)
				{
					this.pRenderer.enabled = false;
				}
				Shader shader = Shader.Find("UI Extensions/Particles/Additive");
				Material material = new Material(shader);
				if (this.material == null)
				{
					this.material = material;
				}
				this.currentMaterial = this.material;
				if (this.currentMaterial && this.currentMaterial.HasProperty("_MainTex"))
				{
					this.currentTexture = this.currentMaterial.mainTexture;
					if (this.currentTexture == null)
					{
						this.currentTexture = Texture2D.whiteTexture;
					}
				}
				this.material = this.currentMaterial;
				this.mainModule.scalingMode = ParticleSystemScalingMode.Hierarchy;
				this.particles = null;
			}
			if (this.particles == null)
			{
				this.particles = new ParticleSystem.Particle[this.pSystem.main.maxParticles];
			}
			this.imageUV = new Vector4(0f, 0f, 1f, 1f);
			this.textureSheetAnimation = this.pSystem.textureSheetAnimation;
			this.textureSheetAnimationFrames = 0;
			this.textureSheetAnimationFrameSize = Vector2.zero;
			if (this.textureSheetAnimation.enabled)
			{
				this.textureSheetAnimationFrames = this.textureSheetAnimation.numTilesX * this.textureSheetAnimation.numTilesY;
				this.textureSheetAnimationFrameSize = new Vector2(1f / (float)this.textureSheetAnimation.numTilesX, 1f / (float)this.textureSheetAnimation.numTilesY);
			}
			return true;
		}

		// Token: 0x06004747 RID: 18247 RVA: 0x0016B55D File Offset: 0x0016995D
		protected override void Awake()
		{
			base.Awake();
			if (!this.Initialize())
			{
				base.enabled = false;
			}
		}

		// Token: 0x06004748 RID: 18248 RVA: 0x0016B578 File Offset: 0x00169978
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			Vector2 zero3 = Vector2.zero;
			int num = this.pSystem.GetParticles(this.particles);
			for (int i = 0; i < num; i++)
			{
				ParticleSystem.Particle particle = this.particles[i];
				Vector2 a = (this.mainModule.simulationSpace != ParticleSystemSimulationSpace.Local) ? this._transform.InverseTransformPoint(particle.position) : particle.position;
				float num2 = -particle.rotation * 0.017453292f;
				float f = num2 + 1.5707964f;
				Color32 currentColor = particle.GetCurrentColor(this.pSystem);
				float num3 = particle.GetCurrentSize(this.pSystem) * 0.5f;
				if (this.mainModule.scalingMode == ParticleSystemScalingMode.Shape)
				{
					a /= base.canvas.scaleFactor;
				}
				Vector4 vector = this.imageUV;
				if (this.textureSheetAnimation.enabled)
				{
					float num4 = 1f - particle.remainingLifetime / particle.startLifetime;
					if (this.textureSheetAnimation.frameOverTime.curveMin != null)
					{
						num4 = this.textureSheetAnimation.frameOverTime.curveMin.Evaluate(1f - particle.remainingLifetime / particle.startLifetime);
					}
					else if (this.textureSheetAnimation.frameOverTime.curve != null)
					{
						num4 = this.textureSheetAnimation.frameOverTime.curve.Evaluate(1f - particle.remainingLifetime / particle.startLifetime);
					}
					else if (this.textureSheetAnimation.frameOverTime.constant > 0f)
					{
						num4 = this.textureSheetAnimation.frameOverTime.constant - particle.remainingLifetime / particle.startLifetime;
					}
					num4 = Mathf.Repeat(num4 * (float)this.textureSheetAnimation.cycleCount, 1f);
					int num5 = 0;
					ParticleSystemAnimationType animation = this.textureSheetAnimation.animation;
					if (animation != ParticleSystemAnimationType.WholeSheet)
					{
						if (animation == ParticleSystemAnimationType.SingleRow)
						{
							num5 = Mathf.FloorToInt(num4 * (float)this.textureSheetAnimation.numTilesX);
							int rowIndex = this.textureSheetAnimation.rowIndex;
							num5 += rowIndex * this.textureSheetAnimation.numTilesX;
						}
					}
					else
					{
						num5 = Mathf.FloorToInt(num4 * (float)this.textureSheetAnimationFrames);
					}
					num5 %= this.textureSheetAnimationFrames;
					vector.x = (float)(num5 % this.textureSheetAnimation.numTilesX) * this.textureSheetAnimationFrameSize.x;
					vector.y = (float)Mathf.FloorToInt((float)(num5 / this.textureSheetAnimation.numTilesX)) * this.textureSheetAnimationFrameSize.y;
					vector.z = vector.x + this.textureSheetAnimationFrameSize.x;
					vector.w = vector.y + this.textureSheetAnimationFrameSize.y;
				}
				zero.x = vector.x;
				zero.y = vector.y;
				this._quad[0] = UIVertex.simpleVert;
				this._quad[0].color = currentColor;
				this._quad[0].uv0 = zero;
				zero.x = vector.x;
				zero.y = vector.w;
				this._quad[1] = UIVertex.simpleVert;
				this._quad[1].color = currentColor;
				this._quad[1].uv0 = zero;
				zero.x = vector.z;
				zero.y = vector.w;
				this._quad[2] = UIVertex.simpleVert;
				this._quad[2].color = currentColor;
				this._quad[2].uv0 = zero;
				zero.x = vector.z;
				zero.y = vector.y;
				this._quad[3] = UIVertex.simpleVert;
				this._quad[3].color = currentColor;
				this._quad[3].uv0 = zero;
				if (num2 == 0f)
				{
					zero2.x = a.x - num3;
					zero2.y = a.y - num3;
					zero3.x = a.x + num3;
					zero3.y = a.y + num3;
					zero.x = zero2.x;
					zero.y = zero2.y;
					this._quad[0].position = zero;
					zero.x = zero2.x;
					zero.y = zero3.y;
					this._quad[1].position = zero;
					zero.x = zero3.x;
					zero.y = zero3.y;
					this._quad[2].position = zero;
					zero.x = zero3.x;
					zero.y = zero2.y;
					this._quad[3].position = zero;
				}
				else
				{
					Vector2 b = new Vector2(Mathf.Cos(num2), Mathf.Sin(num2)) * num3;
					Vector2 b2 = new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * num3;
					this._quad[0].position = a - b - b2;
					this._quad[1].position = a - b + b2;
					this._quad[2].position = a + b + b2;
					this._quad[3].position = a + b - b2;
				}
				vh.AddUIVertexQuad(this._quad);
			}
		}

		// Token: 0x06004749 RID: 18249 RVA: 0x0016BC04 File Offset: 0x0016A004
		private void Update()
		{
			if (!this.fixedTime && Application.isPlaying)
			{
				this.pSystem.Simulate(Time.unscaledDeltaTime, false, false, true);
				this.SetAllDirty();
				if ((this.currentMaterial != null && this.currentTexture != this.currentMaterial.mainTexture) || (this.material != null && this.currentMaterial != null && this.material.shader != this.currentMaterial.shader))
				{
					this.pSystem = null;
					this.Initialize();
				}
			}
		}

		// Token: 0x0600474A RID: 18250 RVA: 0x0016BCBC File Offset: 0x0016A0BC
		private void LateUpdate()
		{
			if (!Application.isPlaying)
			{
				this.SetAllDirty();
			}
			else if (this.fixedTime)
			{
				this.pSystem.Simulate(Time.unscaledDeltaTime, false, false, true);
				this.SetAllDirty();
				if ((this.currentMaterial != null && this.currentTexture != this.currentMaterial.mainTexture) || (this.material != null && this.currentMaterial != null && this.material.shader != this.currentMaterial.shader))
				{
					this.pSystem = null;
					this.Initialize();
				}
			}
			if (this.material == this.currentMaterial)
			{
				return;
			}
			this.pSystem = null;
			this.Initialize();
		}

		// Token: 0x04006E5B RID: 28251
		[Tooltip("Having this enabled run the system in LateUpdate rather than in Update making it faster but less precise (more clunky)")]
		public bool fixedTime = true;

		// Token: 0x04006E5C RID: 28252
		private Transform _transform;

		// Token: 0x04006E5D RID: 28253
		private ParticleSystem pSystem;

		// Token: 0x04006E5E RID: 28254
		private ParticleSystem.Particle[] particles;

		// Token: 0x04006E5F RID: 28255
		private UIVertex[] _quad = new UIVertex[4];

		// Token: 0x04006E60 RID: 28256
		private Vector4 imageUV = Vector4.zero;

		// Token: 0x04006E61 RID: 28257
		private ParticleSystem.TextureSheetAnimationModule textureSheetAnimation;

		// Token: 0x04006E62 RID: 28258
		private int textureSheetAnimationFrames;

		// Token: 0x04006E63 RID: 28259
		private Vector2 textureSheetAnimationFrameSize;

		// Token: 0x04006E64 RID: 28260
		private ParticleSystemRenderer pRenderer;

		// Token: 0x04006E65 RID: 28261
		private Material currentMaterial;

		// Token: 0x04006E66 RID: 28262
		private Texture currentTexture;

		// Token: 0x04006E67 RID: 28263
		private ParticleSystem.MainModule mainModule;
	}
}
