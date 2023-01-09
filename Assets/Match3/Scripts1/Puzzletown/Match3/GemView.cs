using System;
using System.Collections.Generic;
using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006AE RID: 1710
	public class GemView : AReleasable
	{
		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06002AB4 RID: 10932 RVA: 0x000C3622 File Offset: 0x000C1A22
		public bool HasModifier
		{
			get
			{
				return this.modifierGameObject != null;
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06002AB5 RID: 10933 RVA: 0x000C3630 File Offset: 0x000C1A30
		private Animation Animation
		{
			get
			{
				if (!this._animation)
				{
					this._animation = base.GetComponent<Animation>();
				}
				return this._animation;
			}
		}

		// Token: 0x06002AB6 RID: 10934 RVA: 0x000C3654 File Offset: 0x000C1A54
		private void Start()
		{
			PoolMarker component = base.GetComponent<PoolMarker>();
			if (component != null)
			{
				component.onReleased.AddListener(new Action(this.HandleReleased));
			}
		}

		// Token: 0x06002AB7 RID: 10935 RVA: 0x000C368C File Offset: 0x000C1A8C
		public void TryTint(bool darken)
		{
			ITintableView[] componentsInChildren = base.GetComponentsInChildren<ITintableView>();
			Color tint = (!darken) ? Color.white : FieldView.BOOST_OVERLAY_INACTIVE_TINT;
			if (componentsInChildren != null)
			{
				foreach (ITintableView tintableView in componentsInChildren)
				{
					tintableView.ApplyTintColor(tint);
				}
			}
			this.sprite.color = tint;
		}

		// eli key point 显示Gem
		public void Show(Gem gem)
		{
			this.sprite.enabled = true;
			GemType type = gem.type;
			this.color = gem.color;
			Sprite sprite;
			if (gem.IsStackedGem)
			{
				sprite = this.stackedGemSpriteManager.GetSimilar(this.color + GemView.DASH_STRING + type);
			}
			else if (gem.type == GemType.ClimberGem)
			{
				sprite = this.climberGemSpriteManager.GetSimilar(this.color.ToString());
			}
			else if (this.color == GemColor.Rainbow && this.gemColorSpriteManager.animateRainbowGems)
			{
				sprite = this.gemColorSpriteManager.GetSpriteByName("dot_animated_rainbow");
			}
			else
			{
				sprite = this.gemColorSpriteManager.GetSprite(this.color);
			}
			this.UpdateMaterial();
			this.sprite.color = Color.white;
			this.sprite.sprite = sprite;
			this.DetachOverlays();
			if (gem.IsAnyBombGem())
			{
				this.overlayGameObject = this.AttachOverlay(this.bombOverlay, this.sprite.transform);
			}
			else if (gem.IsLineGem())
			{
				GameObject prefab = (gem.type != GemType.LineHorizontal) ? this.verticalLineGemOverlay : this.horizontalLineGemOverlay;
				this.overlayGameObject = this.AttachOverlay(prefab, this.sprite.transform);
			}
			bool enabled = !gem.IsClimber && !gem.IsDroppable && gem.color != GemColor.Dirt && !gem.IsCoveredByDirt && !gem.IsCannon && !gem.IsChameleon;
			this.sprite.enabled = enabled;
			if (gem.IsClimber)
			{
				this.overlayGameObject = this.AttachOverlay(this.climberPrefab, this.sprite.transform);
			}
			else if (gem.IsDroppable)
			{
				this.overlayGameObject = this.AttachOverlay(this.droppablePrefab, this.sprite.transform);
			}
			else if (gem.IsChameleon)
			{
				this.overlayGameObject = this.AttachOverlay(this.chameleonPrefab, this.sprite.transform);
				ChameleonView componentInChildren = this.overlayGameObject.GetComponentInChildren<ChameleonView>();
				componentInChildren.Initialize(gem, false);
			}
			if (this.overlayGameObject)
			{
				this.overlayView = this.overlayGameObject.GetComponent<AAnimatedGemOverlayView>();
			}
			if (gem.modifier != GemModifier.Undefined || gem.IsCannon)
			{
				int num = (int)gem.modifier;
				if (gem.IsCannon)
				{
					num = 9;
				}
				GameObject prefab2 = this.modifierViews[this.mapModifierToPrefab[num]];
				Transform parent = (!gem.IsCoveredByDirt) ? this.sprite.transform : base.transform;
				this.modifierGameObject = this.AttachOverlay(prefab2, parent);
				this.modifierView = this.modifierGameObject.GetComponentInChildren<IGemModifierView>();
				this.modifierView.ShowModifier(gem);
			}
		}

		// Token: 0x06002AB9 RID: 10937 RVA: 0x000C3A10 File Offset: 0x000C1E10
		public void UpdateModifierView<T>(Gem gem, int parameterUpdateDelta) where T : IUpdatableGemModifierView
		{
			if (this.modifierView == null)
			{
				this.Show(gem);
			}
			else
			{
				T t = (T)((object)this.modifierView);
				t.UpdateWithDelta(parameterUpdateDelta);
			}
			T t2 = (T)((object)this.modifierView);
			this.gemParameterShown = t2.ParameterShown();
		}

		// Token: 0x06002ABA RID: 10938 RVA: 0x000C3A6D File Offset: 0x000C1E6D
		public T GetModifierView<T>() where T : IGemModifierView
		{
			return (T)((object)this.modifierView);
		}

		// Token: 0x06002ABB RID: 10939 RVA: 0x000C3A7A File Offset: 0x000C1E7A
		public void StartParticles()
		{
			this.StartParticles(this.particleTrailPrefab, false);
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x000C3A8C File Offset: 0x000C1E8C
		public void StartParticles(GameObject prefab, bool hasTrailRenderer = false)
		{
			this.particleTrail = this.objectPool.Get(prefab);
			this.particleTrail.transform.SetParent(base.transform);
			this.particleTrail.transform.localPosition = Vector3.zero;
			this.particleTrail.transform.localScale = Vector3.one;
			if (hasTrailRenderer)
			{
				this.TryClearTrailRenderer(this.particleTrail);
			}
			ParticleStartValues component = this.particleTrail.GetComponent<ParticleStartValues>();
			if (component)
			{
				component.UseColor = (int)this.color;
			}
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x000C3B20 File Offset: 0x000C1F20
		private void TryClearTrailRenderer(GameObject prefab)
		{
			TrailRenderer componentInChildren = prefab.GetComponentInChildren<TrailRenderer>();
			if (componentInChildren != null)
			{
				componentInChildren.Clear();
			}
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x000C3B48 File Offset: 0x000C1F48
		public void Play(AnimationClip clip, float duration, bool killQueue = true)
		{
			if (killQueue)
			{
				this.queueAnimations.Clear();
				this.Animation.Stop();
				this.sprite.transform.localRotation = Quaternion.identity;
			}
			AnimationInfo item = new AnimationInfo(clip, duration);
			this.queueAnimations.Add(item);
		}

		// Token: 0x06002ABF RID: 10943 RVA: 0x000C3B9B File Offset: 0x000C1F9B
		public void PlayExtraMoveAnimation()
		{
			if (this.overlayGameObject && this.overlayView)
			{
				this.overlayView.SwitchState(OverlayAnimationState.Move);
			}
		}

		// Token: 0x06002AC0 RID: 10944 RVA: 0x000C3BCC File Offset: 0x000C1FCC
		public void ModifierLanded()
		{
			if (this.modifierGameObject != null && this.modifierView != null)
			{
				IModifierViewWithLanding modifierViewWithLanding = this.modifierView as IModifierViewWithLanding;
				if (modifierViewWithLanding != null)
				{
					modifierViewWithLanding.ShowModifierLanding();
				}
			}
			if (this.overlayGameObject && this.overlayView)
			{
				this.overlayView.SwitchState(OverlayAnimationState.Idle);
			}
		}

		// Token: 0x06002AC1 RID: 10945 RVA: 0x000C3C39 File Offset: 0x000C2039
		public void SwitchToHighlightMaterial()
		{
			if (this.sprite.sharedMaterial != this.animatedRainbowMaterial)
			{
				this.sprite.material = this.highlightMaterial;
			}
		}

		// Token: 0x06002AC2 RID: 10946 RVA: 0x000C3C68 File Offset: 0x000C2068
		public void UpdateMaterial()
		{
			bool flag = this.color == GemColor.Rainbow && this.gemColorSpriteManager.animateRainbowGems;
			this.sprite.sharedMaterial = ((!flag) ? this.defaultMaterial : this.animatedRainbowMaterial);
		}

		// Token: 0x06002AC3 RID: 10947 RVA: 0x000C3CB4 File Offset: 0x000C20B4
		private void StartAnimation(AnimationInfo info)
		{
			AnimationClip clip = this.Animation.GetClip(info.clip.name);
			if (!clip)
			{
				this.Animation.AddClip(info.clip, info.clip.name);
			}
			AnimationState animationState = this.Animation[info.clip.name];
			float speed = animationState.clip.length / info.duration;
			animationState.speed = speed;
			this.Animation.Play(animationState.name);
		}

		// Token: 0x06002AC4 RID: 10948 RVA: 0x000C3D48 File Offset: 0x000C2148
		private GameObject AttachOverlay(GameObject prefab, Transform parent)
		{
			GameObject gameObject = this.objectPool.Get(prefab);
			gameObject.transform.SetParent(parent);
			gameObject.transform.position = base.transform.position;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			return gameObject;
		}

		// Token: 0x06002AC5 RID: 10949 RVA: 0x000C3DB5 File Offset: 0x000C21B5
		private void DetachParticles()
		{
			if (this.particleTrail != null)
			{
				this.particleTrail.Release();
				this.particleTrail = null;
			}
		}

		// Token: 0x06002AC6 RID: 10950 RVA: 0x000C3DDA File Offset: 0x000C21DA
		private void DetachOverlays()
		{
			this.overlayGameObject = this.DetachOverlay(this.overlayGameObject);
			this.modifierGameObject = this.DetachOverlay(this.modifierGameObject);
			this.modifierView = null;
		}

		// Token: 0x06002AC7 RID: 10951 RVA: 0x000C3E07 File Offset: 0x000C2207
		private GameObject DetachOverlay(GameObject overlay)
		{
			if (overlay != null)
			{
				overlay.transform.SetParent(base.transform.parent);
				overlay.Release();
			}
			return null;
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x000C3E32 File Offset: 0x000C2232
		private void Update()
		{
			if (!this.Animation.isPlaying && this.queueAnimations.Count > 0)
			{
				this.StartAnimation(this.queueAnimations.RemoveAndGetAt(0));
			}
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x000C3E67 File Offset: 0x000C2267
		private void OnDisable()
		{
			this.queueAnimations.Clear();
			this.Animation.Stop();
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x000C3E80 File Offset: 0x000C2280
		private void HandleReleased()
		{
			this.gemParameterShown = CannonView.NOT_INITIALIZED;
			this.UpdateMaterial();
			this.sprite.enabled = true;
			this.sprite.transform.localScale = Vector3.one;
			base.transform.localScale = Vector3.one;
			this.DetachOverlays();
			this.DetachParticles();
		}

		// Token: 0x040053FD RID: 21501
		public SpriteRenderer sprite;

		// Token: 0x040053FE RID: 21502
		public Material defaultMaterial;

		// Token: 0x040053FF RID: 21503
		public Material animatedRainbowMaterial;

		// Token: 0x04005400 RID: 21504
		public Material highlightMaterial;

		// Token: 0x04005401 RID: 21505
		[HideInInspector]
		public ObjectPool objectPool;

		// Token: 0x04005402 RID: 21506
		public int gemParameterShown = CannonView.NOT_INITIALIZED;

		// Token: 0x04005403 RID: 21507
		[SerializeField]
		private GameObject bombOverlay;

		// Token: 0x04005404 RID: 21508
		[SerializeField]
		private GameObject horizontalLineGemOverlay;

		// Token: 0x04005405 RID: 21509
		[SerializeField]
		private GameObject verticalLineGemOverlay;

		// Token: 0x04005406 RID: 21510
		[SerializeField]
		private GemColorSpriteManager gemColorSpriteManager;

		// Token: 0x04005407 RID: 21511
		[SerializeField]
		private SpriteManager stackedGemSpriteManager;

		// Token: 0x04005408 RID: 21512
		[SerializeField]
		private SpriteManager climberGemSpriteManager;

		// Token: 0x04005409 RID: 21513
		[SerializeField]
		private GameObject particleTrailPrefab;

		// Token: 0x0400540A RID: 21514
		[SerializeField]
		private GameObject climberPrefab;

		// Token: 0x0400540B RID: 21515
		[SerializeField]
		private GameObject droppablePrefab;

		// Token: 0x0400540C RID: 21516
		[SerializeField]
		private GameObject chameleonPrefab;

		// Token: 0x0400540D RID: 21517
		[SerializeField]
		private List<GameObject> modifierViews;

		// Token: 0x0400540E RID: 21518
		private readonly int[] mapModifierToPrefab = new int[]
		{
			-1,
			0,
			0,
			0,
			1,
			1,
			1,
			2,
			2,
			2
		};

		// Token: 0x0400540F RID: 21519
		private List<AnimationInfo> queueAnimations = new List<AnimationInfo>();

		// Token: 0x04005410 RID: 21520
		private static string DASH_STRING = "_";

		// Token: 0x04005411 RID: 21521
		private GameObject overlayGameObject;

		// Token: 0x04005412 RID: 21522
		private AAnimatedGemOverlayView overlayView;

		// Token: 0x04005413 RID: 21523
		private GameObject modifierGameObject;

		// Token: 0x04005414 RID: 21524
		private IGemModifierView modifierView;

		// Token: 0x04005415 RID: 21525
		private GameObject particleTrail;

		// Token: 0x04005416 RID: 21526
		private GemColor color;

		// Token: 0x04005417 RID: 21527
		private Animation _animation;
	}
}
