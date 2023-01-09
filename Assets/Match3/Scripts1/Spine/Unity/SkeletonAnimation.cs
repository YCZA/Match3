using UnityEngine;

namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x02000256 RID: 598
	[ExecuteInEditMode]
	[AddComponentMenu("Spine/SkeletonAnimation")]
	[HelpURL("http://esotericsoftware.com/spine-unity-documentation#Controlling-Animation")]
	public class SkeletonAnimation : SkeletonRenderer, ISkeletonAnimation
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600125B RID: 4699 RVA: 0x000378C4 File Offset: 0x00035CC4
		// (remove) Token: 0x0600125C RID: 4700 RVA: 0x000378CD File Offset: 0x00035CCD
		public event UpdateBonesDelegate UpdateLocal;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x0600125D RID: 4701 RVA: 0x000378D6 File Offset: 0x00035CD6
		// (remove) Token: 0x0600125E RID: 4702 RVA: 0x000378DF File Offset: 0x00035CDF
		public event UpdateBonesDelegate UpdateWorld;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600125F RID: 4703 RVA: 0x000378E8 File Offset: 0x00035CE8
		// (remove) Token: 0x06001260 RID: 4704 RVA: 0x000378F1 File Offset: 0x00035CF1
		public event UpdateBonesDelegate UpdateComplete;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06001261 RID: 4705 RVA: 0x000378FC File Offset: 0x00035CFC
		// (remove) Token: 0x06001262 RID: 4706 RVA: 0x00037934 File Offset: 0x00035D34
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		protected event UpdateBonesDelegate _UpdateLocal;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06001263 RID: 4707 RVA: 0x0003796C File Offset: 0x00035D6C
		// (remove) Token: 0x06001264 RID: 4708 RVA: 0x000379A4 File Offset: 0x00035DA4
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		protected event UpdateBonesDelegate _UpdateWorld;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06001265 RID: 4709 RVA: 0x000379DC File Offset: 0x00035DDC
		// (remove) Token: 0x06001266 RID: 4710 RVA: 0x00037A14 File Offset: 0x00035E14
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		protected event UpdateBonesDelegate _UpdateComplete;

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06001267 RID: 4711 RVA: 0x00037A4A File Offset: 0x00035E4A
		public Skeleton Skeleton
		{
			get
			{
				this.Initialize(false);
				return this.skeleton;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06001268 RID: 4712 RVA: 0x00037A5C File Offset: 0x00035E5C
		// (set) Token: 0x06001269 RID: 4713 RVA: 0x00037AA4 File Offset: 0x00035EA4
		public string AnimationName
		{
			get
			{
				if (!this.valid)
				{
					global::UnityEngine.Debug.LogWarning("You tried access AnimationName but the SkeletonAnimation was not valid. Try checking your Skeleton Data for errors.");
					return null;
				}
				TrackEntry current = this.state.GetCurrent(0);
				return (current != null) ? current.Animation.Name : null;
			}
			set
			{
				if (this._animationName == value)
				{
					return;
				}
				this._animationName = value;
				if (!this.valid)
				{
					global::UnityEngine.Debug.LogWarning("You tried to change AnimationName but the SkeletonAnimation was not valid. Try checking your Skeleton Data for errors.");
					return;
				}
				if (value == null || value.Length == 0)
				{
					this.state.ClearTrack(0);
				}
				else
				{
					this.state.SetAnimation(0, value, this.loop);
				}
			}
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x00037B16 File Offset: 0x00035F16
		public static SkeletonAnimation AddToGameObject(GameObject gameObject, SkeletonDataAsset skeletonDataAsset)
		{
			return SkeletonRenderer.AddSpineComponent<SkeletonAnimation>(gameObject, skeletonDataAsset);
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x00037B1F File Offset: 0x00035F1F
		public static SkeletonAnimation NewSkeletonAnimationGameObject(SkeletonDataAsset skeletonDataAsset)
		{
			return SkeletonRenderer.NewSpineGameObject<SkeletonAnimation>(skeletonDataAsset);
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x00037B28 File Offset: 0x00035F28
		public override void Initialize(bool overwrite)
		{
			if (this.valid && !overwrite)
			{
				return;
			}
			base.Initialize(overwrite);
			if (!this.valid)
			{
				return;
			}
			this.state = new AnimationState(this.skeletonDataAsset.GetAnimationStateData());
			if (this.deactivateWhenIdle)
			{
				this.state.Start += delegate(AnimationState state, int trackIndex)
				{
					this.Activate(state.GetCurrent(trackIndex).Animation.name != "idle");
				};
			}
			if (!string.IsNullOrEmpty(this._animationName))
			{
				this.state.SetAnimation(0, this._animationName, this.loop);
				this.UpdateDelta(0f);
			}
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x00037BC6 File Offset: 0x00035FC6
		public override void FixedUpdate()
		{
			if (Shader.globalMaximumLOD < 500)
			{
				return;
			}
			this.UpdateDelta(Time.fixedDeltaTime);
			base.FixedUpdate();
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x00037BEC File Offset: 0x00035FEC
		public virtual void UpdateDelta(float deltaTime)
		{
			if (!this.valid)
			{
				return;
			}
			deltaTime *= this.timeScale;
			this.skeleton.Update(deltaTime);
			this.state.Update(deltaTime);
			this.state.Apply(this.skeleton);
			if (this._UpdateLocal != null)
			{
				this._UpdateLocal(this);
			}
			this.skeleton.UpdateWorldTransform();
			if (this._UpdateWorld != null)
			{
				this._UpdateWorld(this);
				this.skeleton.UpdateWorldTransform();
			}
			if (this._UpdateComplete != null)
			{
				this._UpdateComplete(this);
			}
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x00037C93 File Offset: 0x00036093
		private void Activate(bool active)
		{
			if (!active)
			{
				base.UpdateAnimation(true);
			}
			base.enabled = active;
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x00037CAC File Offset: 0x000360AC
		private void OnBecameVisible()
		{
			if (this.state == null || this.state.GetCurrent(0) == null)
			{
				return;
			}
			this.Activate(!this.deactivateWhenIdle || this.state.GetCurrent(0).Animation.name != "idle");
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x00037D0A File Offset: 0x0003610A
		private void OnBecameInvisible()
		{
			this.Activate(false);
		}

		// Token: 0x0400428D RID: 17037
		public AnimationState state;

		// Token: 0x04004291 RID: 17041
		public bool deactivateWhenIdle = true;

		// Token: 0x04004292 RID: 17042
		[SerializeField]
		[SpineAnimation("", "")]
		private string _animationName;

		// Token: 0x04004293 RID: 17043
		[Tooltip("Whether or not an animation should loop. This only applies to the initial animation specified in the inspector, or any subsequent Animations played through .AnimationName. Animations set through state.SetAnimation are unaffected.")]
		public bool loop;

		// Token: 0x04004294 RID: 17044
		[Tooltip("The rate at which animations progress over time. 1 means 100%. 0.5 means 50%.")]
		public float timeScale = 1f;
	}
}
