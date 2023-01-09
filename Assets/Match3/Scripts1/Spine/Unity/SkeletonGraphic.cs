using Match3.Scripts1.Spine.Unity.MeshGeneration;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x0200024B RID: 587
	[ExecuteInEditMode]
	[RequireComponent(typeof(CanvasRenderer), typeof(RectTransform))]
	[DisallowMultipleComponent]
	[AddComponentMenu("Spine/SkeletonGraphic (Unity UI Canvas)")]
	public class SkeletonGraphic : MaskableGraphic
	{
		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06001212 RID: 4626 RVA: 0x000348DD File Offset: 0x00032CDD
		public override Texture mainTexture
		{
			get
			{
				return (!(this.skeletonDataAsset == null)) ? this.skeletonDataAsset.atlasAssets[0].materials[0].mainTexture : null;
			}
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x0003490F File Offset: 0x00032D0F
		protected override void Awake()
		{
			base.Awake();
			if (!this.IsValid)
			{
				this.Initialize(false);
				this.Rebuild(CanvasUpdate.PreRender);
			}
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x00034930 File Offset: 0x00032D30
		public override void Rebuild(CanvasUpdate update)
		{
			base.Rebuild(update);
			if (base.canvasRenderer.cull)
			{
				return;
			}
			if (update == CanvasUpdate.PreRender)
			{
				this.UpdateMesh();
			}
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x00034957 File Offset: 0x00032D57
		public virtual void Update()
		{
			if (this.freeze)
			{
				return;
			}
			this.Update(Time.deltaTime);
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x00034970 File Offset: 0x00032D70
		public virtual void Update(float deltaTime)
		{
			if (!this.IsValid)
			{
				return;
			}
			deltaTime *= this.timeScale;
			this.skeleton.Update(deltaTime);
			this.state.Update(deltaTime);
			this.state.Apply(this.skeleton);
			if (this.UpdateLocal != null)
			{
				this.UpdateLocal(this);
			}
			this.skeleton.UpdateWorldTransform();
			if (this.UpdateWorld != null)
			{
				this.UpdateWorld(this);
				this.skeleton.UpdateWorldTransform();
			}
			if (this.UpdateComplete != null)
			{
				this.UpdateComplete(this);
			}
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x00034A17 File Offset: 0x00032E17
		private void LateUpdate()
		{
			if (this.freeze)
			{
				return;
			}
			this.UpdateMesh();
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06001218 RID: 4632 RVA: 0x00034A2B File Offset: 0x00032E2B
		public Skeleton Skeleton
		{
			get
			{
				return this.skeleton;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06001219 RID: 4633 RVA: 0x00034A33 File Offset: 0x00032E33
		public SkeletonData SkeletonData
		{
			get
			{
				return (this.skeleton != null) ? this.skeleton.data : null;
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x0600121A RID: 4634 RVA: 0x00034A51 File Offset: 0x00032E51
		public bool IsValid
		{
			get
			{
				return this.skeleton != null;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x0600121B RID: 4635 RVA: 0x00034A5F File Offset: 0x00032E5F
		public AnimationState AnimationState
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x0600121C RID: 4636 RVA: 0x00034A67 File Offset: 0x00032E67
		public ISimpleMeshGenerator SpineMeshGenerator
		{
			get
			{
				return this.spineMeshGenerator;
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600121D RID: 4637 RVA: 0x00034A70 File Offset: 0x00032E70
		// (remove) Token: 0x0600121E RID: 4638 RVA: 0x00034AA8 File Offset: 0x00032EA8
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event SkeletonGraphic.UpdateDelegate UpdateLocal;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x0600121F RID: 4639 RVA: 0x00034AE0 File Offset: 0x00032EE0
		// (remove) Token: 0x06001220 RID: 4640 RVA: 0x00034B18 File Offset: 0x00032F18
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event SkeletonGraphic.UpdateDelegate UpdateWorld;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06001221 RID: 4641 RVA: 0x00034B50 File Offset: 0x00032F50
		// (remove) Token: 0x06001222 RID: 4642 RVA: 0x00034B88 File Offset: 0x00032F88
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event SkeletonGraphic.UpdateDelegate UpdateComplete;

		// Token: 0x06001223 RID: 4643 RVA: 0x00034BBE File Offset: 0x00032FBE
		public void Clear()
		{
			this.skeleton = null;
			base.canvasRenderer.Clear();
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x00034BD4 File Offset: 0x00032FD4
		public void Initialize(bool overwrite)
		{
			if (this.IsValid && !overwrite)
			{
				return;
			}
			if (this.skeletonDataAsset == null)
			{
				return;
			}
			SkeletonData skeletonData = this.skeletonDataAsset.GetSkeletonData(false);
			if (skeletonData == null)
			{
				return;
			}
			if (this.skeletonDataAsset.atlasAssets.Length <= 0 || this.skeletonDataAsset.atlasAssets[0].materials.Length <= 0)
			{
				return;
			}
			this.state = new AnimationState(this.skeletonDataAsset.GetAnimationStateData());
			if (this.state == null)
			{
				this.Clear();
				return;
			}
			this.skeleton = new Skeleton(skeletonData);
			this.spineMeshGenerator = new ArraysSimpleMeshGenerator();
			if (!string.IsNullOrEmpty(this.initialSkinName))
			{
				this.skeleton.SetSkin(this.initialSkinName);
			}
			if (!string.IsNullOrEmpty(this.startingAnimation))
			{
				this.state.SetAnimation(0, this.startingAnimation, this.startingLoop);
			}
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x00034CD0 File Offset: 0x000330D0
		public void UpdateMesh()
		{
			if (this.IsValid)
			{
				this.skeleton.SetColor(this.color);
				if (base.canvas != null)
				{
					this.spineMeshGenerator.Scale = base.canvas.referencePixelsPerUnit;
				}
				base.canvasRenderer.SetMesh(this.spineMeshGenerator.GenerateMesh(this.skeleton));
			}
		}

		// Token: 0x04004257 RID: 16983
		public SkeletonDataAsset skeletonDataAsset;

		// Token: 0x04004258 RID: 16984
		[SpineSkin("", "skeletonDataAsset")]
		public string initialSkinName = "default";

		// Token: 0x04004259 RID: 16985
		[SpineAnimation("", "skeletonDataAsset")]
		public string startingAnimation;

		// Token: 0x0400425A RID: 16986
		public bool startingLoop;

		// Token: 0x0400425B RID: 16987
		public float timeScale = 1f;

		// Token: 0x0400425C RID: 16988
		public bool freeze;

		// Token: 0x0400425D RID: 16989
		protected Skeleton skeleton;

		// Token: 0x0400425E RID: 16990
		protected AnimationState state;

		// Token: 0x0400425F RID: 16991
		protected ISimpleMeshGenerator spineMeshGenerator;

		// Token: 0x0200024C RID: 588
		// (Invoke) Token: 0x06001227 RID: 4647
		public delegate void UpdateDelegate(SkeletonGraphic skeletonGraphic);
	}
}
