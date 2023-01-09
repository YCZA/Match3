using System;
using System.Collections.Generic;
using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006A1 RID: 1697
	public class ColorWheelView : AReleasable
	{
		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06002A48 RID: 10824 RVA: 0x000C179E File Offset: 0x000BFB9E
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

		// Token: 0x170006C2 RID: 1730
		// (set) Token: 0x06002A49 RID: 10825 RVA: 0x000C17C2 File Offset: 0x000BFBC2
		public ColorWheelModel Model
		{
			set
			{
				if (value != null)
				{
					this.model = value;
					this.SetupViewState();
				}
			}
		}

		// Token: 0x06002A4A RID: 10826 RVA: 0x000C17D8 File Offset: 0x000BFBD8
		public void RemoveColor(GemColor color)
		{
			if (this.wheelSlices.ContainsKey(color))
			{
				this._animation.Stop();
				this._animation.Play("ColorWheelClear");
				this.glowingMatchedSlices[color].gameObject.SetActive(true);
				this.wheelSlices[color].gameObject.SetActive(false);
				this.wheelEffects[color].Play();
			}
		}

		// Token: 0x06002A4B RID: 10827 RVA: 0x000C1854 File Offset: 0x000BFC54
		public void PlayRemovalAnimation()
		{
			this.backgroundSprite.gameObject.SetActive(false);
			foreach (SpriteRenderer spriteRenderer in this.glowingMatchedSlices.Values)
			{
				spriteRenderer.gameObject.SetActive(false);
			}
			this._animation.Play("ColorWheelDisappear");
			this.RemoveListeners();
		}

		// Token: 0x06002A4C RID: 10828 RVA: 0x000C18E4 File Offset: 0x000BFCE4
		private void Start()
		{
			this.materialPropertyBlock = new MaterialPropertyBlock();
			this._animation = base.GetComponent<Animation>();
		}

		// Token: 0x06002A4D RID: 10829 RVA: 0x000C1900 File Offset: 0x000BFD00
		public void StartAnimation(AnimationClip clip, float duration)
		{
			AnimationState animationState = this.Animation[clip.name];
			float speed = clip.length / duration;
			animationState.speed = speed;
			this.Animation.Play(animationState.name);
		}

		// Token: 0x06002A4E RID: 10830 RVA: 0x000C1944 File Offset: 0x000BFD44
		private void SetupViewState()
		{
			if (this.model.colors == null)
			{
				return;
			}
			this.wheelSlices = new Dictionary<GemColor, SpriteRenderer>();
			this.wheelEffects = new Dictionary<GemColor, ParticleSystem>();
			this.glowingMatchedSlices = new Dictionary<GemColor, SpriteRenderer>();
			int count = this.model.colors.Count;
			this.sprite.sprite = this.spriteManager.GetSimilar("frame_" + count);
			List<GemColor> list = new List<GemColor>();
			foreach (GemColor item in Gem.GEM_ORDER)
			{
				if (this.model.colors.Contains(item))
				{
					list.Add(item);
				}
			}
			this.spritesToTintForBoostOverlay = new SpriteRenderer[count + 3];
			this.spritesToTintForBoostOverlay[count] = this.backgroundSprite;
			this.spritesToTintForBoostOverlay[count + 1] = this.sprite;
			this.spritesToTintForBoostOverlay[count + 2] = this.faceSprite;
			this.slicesToTintForBoostOverlay = new SpriteRenderer[count];
			float num = 360f / (float)count;
			for (int i = 0; i < count; i++)
			{
				Quaternion rotation = Quaternion.Euler(0f, 0f, (float)i * -num);
				SpriteRenderer spriteRenderer = global::UnityEngine.Object.Instantiate<SpriteRenderer>(this.wheelSlicePrefab, base.transform.position, rotation, base.transform);
				SpriteRenderer spriteRenderer2 = global::UnityEngine.Object.Instantiate<SpriteRenderer>(this.colorRemovedGlowPrefab, base.transform.position, rotation, base.transform);
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.chargedEffect, base.transform.position, rotation, base.transform);
				spriteRenderer.gameObject.SetActive(true);
				spriteRenderer2.gameObject.SetActive(false);
				spriteRenderer.sprite = this.spriteManager.GetSimilar("piece_" + count);
				spriteRenderer2.sprite = this.spriteManager.GetSimilar("piece_" + count);
				this.slicesToTintForBoostOverlay[i] = spriteRenderer;
				this.spritesToTintForBoostOverlay[i] = spriteRenderer2;
				ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
				ParticleSystem.ShapeModule shape = component.shape;
				shape.arc = num;
				shape.rotation = new Vector3(0f, 0f, this.GetOffsetRotationForParticles(count));
				var go1 = component.textureSheetAnimation;
				go1.frameOverTime = this.GetFlipbookTime(list[i]);
				GemColor gemColor = list[i];
				float valueForLookUpTableColumn = FieldView.GetValueForLookUpTableColumn(gemColor);
				spriteRenderer.color = new Color(valueForLookUpTableColumn, 0f, 0f, 0f);
				this.wheelSlices.Add(gemColor, spriteRenderer);
				this.wheelEffects.Add(gemColor, component);
				this.glowingMatchedSlices.Add(gemColor, spriteRenderer2);
			}
		}

		// Token: 0x06002A4F RID: 10831 RVA: 0x000C1C30 File Offset: 0x000C0030
		private float GetOffsetRotationForParticles(int numberOfSlices)
		{
			float result = 0f;
			switch (numberOfSlices)
			{
			case 3:
				result = 30f;
				break;
			case 4:
				result = 45f;
				break;
			case 5:
				result = 55f;
				break;
			case 6:
				result = 60f;
				break;
			}
			return result;
		}

		// Token: 0x06002A50 RID: 10832 RVA: 0x000C1C90 File Offset: 0x000C0090
		public void SetupBoostOverlayListener(BoostOverlayController boostOverlayController)
		{
			if (boostOverlayController == null)
			{
				return;
			}
			this.boostOverlayController = boostOverlayController;
			this.boostOverlayController.onBoostOverlayStateChanged.RemoveListener(new Action<BoostOverlayState>(this.HandleBoostOverlayAlphaChanged));
			this.boostOverlayController.onBoostOverlayStateChanged.AddListener(new Action<BoostOverlayState>(this.HandleBoostOverlayAlphaChanged));
			this.boostOverlayController.onTick.RemoveListener(new Action<float>(this.HandleBoostOverlayTick));
			this.boostOverlayController.onTick.AddListener(new Action<float>(this.HandleBoostOverlayTick));
		}

		// Token: 0x06002A51 RID: 10833 RVA: 0x000C1D1C File Offset: 0x000C011C
		private float GetFlipbookTime(GemColor color)
		{
			int num = 0;
			switch (color)
			{
			case GemColor.Red:
				num = 2;
				break;
			case GemColor.Green:
				num = 6;
				break;
			case GemColor.Blue:
				num = 5;
				break;
			case GemColor.Yellow:
				num = 7;
				break;
			case GemColor.Purple:
				num = 4;
				break;
			case GemColor.Orange:
				num = 3;
				break;
			case GemColor.Coins:
				num = 1;
				break;
			}
			return 0.142f * (float)num;
		}

		// Token: 0x06002A52 RID: 10834 RVA: 0x000C1D98 File Offset: 0x000C0198
		private void RemoveListeners()
		{
			if (this.boostOverlayController != null)
			{
				this.boostOverlayController.onBoostOverlayStateChanged.RemoveListener(new Action<BoostOverlayState>(this.HandleBoostOverlayAlphaChanged));
				this.boostOverlayController.onTick.RemoveListener(new Action<float>(this.HandleBoostOverlayTick));
			}
		}

		// Token: 0x06002A53 RID: 10835 RVA: 0x000C1DE8 File Offset: 0x000C01E8
		private void HandleBoostOverlayTick(float elapsedTime)
		{
			if (this.overlayTargetState.shouldUpdate)
			{
				this.overlayTargetState.elapsedTime = elapsedTime;
				if (this.overlayTargetState.elapsedTime < this.overlayTargetState.delayInSecs)
				{
					this.TintSpritesForBoostOverlay(this.overlayTargetState.darken, this.overlayTargetState.ElapsedRatio, this.overlayTargetState.maskDarken);
				}
				else
				{
					this.TintSpritesForBoostOverlay(this.overlayTargetState.darken, 1f, this.overlayTargetState.maskDarken);
					this.overlayTargetState.shouldUpdate = false;
					this.overlayTargetState.elapsedTime = 0f;
				}
			}
		}

		// Token: 0x06002A54 RID: 10836 RVA: 0x000C1E98 File Offset: 0x000C0298
		private void TintSpritesForBoostOverlay(bool darken, float tintRatio, bool maskDarken)
		{
			Color color = (!darken) ? FieldView.BOOST_OVERLAY_INACTIVE_TINT : Color.white;
			Color color2 = (!darken) ? Color.white : FieldView.BOOST_OVERLAY_INACTIVE_TINT;
			tintRatio = Mathf.Clamp01(tintRatio);
			Color color3 = color + (color2 - color) * tintRatio;
			foreach (SpriteRenderer spriteRenderer in this.spritesToTintForBoostOverlay)
			{
				if (spriteRenderer.color != color2)
				{
					spriteRenderer.color = color3;
				}
			}
			this.TintColorSlices(darken);
		}

		// Token: 0x06002A55 RID: 10837 RVA: 0x000C1F34 File Offset: 0x000C0334
		private void TintColorSlices(bool darken)
		{
			foreach (SpriteRenderer spriteRenderer in this.slicesToTintForBoostOverlay)
			{
				spriteRenderer.GetPropertyBlock(this.materialPropertyBlock);
				this.materialPropertyBlock.SetColor("_Tint", (!darken) ? Color.white : FieldView.BOOST_OVERLAY_INACTIVE_TINT);
				spriteRenderer.SetPropertyBlock(this.materialPropertyBlock);
			}
		}

		// Token: 0x06002A56 RID: 10838 RVA: 0x000C1F9D File Offset: 0x000C039D
		private void HandleBoostOverlayAlphaChanged(BoostOverlayState boostOverlayState)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			this.StopUpdatingOverlayFade();
			if (boostOverlayState.isOn)
			{
				this.TryTurnOnBoostMask(boostOverlayState);
			}
			else
			{
				this.TurnOffBoostMask(boostOverlayState);
			}
		}

		// Token: 0x06002A57 RID: 10839 RVA: 0x000C1FD8 File Offset: 0x000C03D8
		private void TryTurnOnBoostMask(BoostOverlayState boostOverlayState)
		{
			float delayInSecs = (!boostOverlayState.shouldChangeInstantly) ? BoostOverlayController.BOOST_OVERLAY_FADE_DURATION_SECS : 0f;
			this.overlayTargetState.Set(delayInSecs, true, true, true);
		}

		// Token: 0x06002A58 RID: 10840 RVA: 0x000C2010 File Offset: 0x000C0410
		private void TurnOffBoostMask(BoostOverlayState boostOverlayState)
		{
			float delayInSecs = (!boostOverlayState.shouldChangeInstantly) ? BoostOverlayController.BOOST_OVERLAY_FADE_DURATION_SECS : 0f;
			this.overlayTargetState.Set(delayInSecs, false, false, false);
		}

		// Token: 0x06002A59 RID: 10841 RVA: 0x000C2048 File Offset: 0x000C0448
		private void StopUpdatingOverlayFade()
		{
			this.overlayTargetState.shouldUpdate = false;
			this.overlayTargetState.elapsedTime = 0f;
		}

		// Token: 0x040053A2 RID: 21410
		private const string WHEEL_PREFIX = "frame_";

		// Token: 0x040053A3 RID: 21411
		private const string SLICE_PREFIX = "piece_";

		// Token: 0x040053A4 RID: 21412
		[HideInInspector]
		public ObjectPool objectPool;

		// Token: 0x040053A5 RID: 21413
		[SerializeField]
		public SpriteManager spriteManager;

		// Token: 0x040053A6 RID: 21414
		[SerializeField]
		public SpriteRenderer sprite;

		// Token: 0x040053A7 RID: 21415
		[SerializeField]
		public GameObject chargedEffect;

		// Token: 0x040053A8 RID: 21416
		[SerializeField]
		private SpriteRenderer faceSprite;

		// Token: 0x040053A9 RID: 21417
		[SerializeField]
		private SpriteRenderer wheelSlicePrefab;

		// Token: 0x040053AA RID: 21418
		[SerializeField]
		private SpriteRenderer backgroundSprite;

		// Token: 0x040053AB RID: 21419
		[SerializeField]
		private SpriteRenderer colorRemovedGlowPrefab;

		// Token: 0x040053AC RID: 21420
		private MaterialPropertyBlock materialPropertyBlock;

		// Token: 0x040053AD RID: 21421
		private SpriteRenderer[] spritesToTintForBoostOverlay;

		// Token: 0x040053AE RID: 21422
		private SpriteRenderer[] slicesToTintForBoostOverlay;

		// Token: 0x040053AF RID: 21423
		private BoostOverlayController boostOverlayController;

		// Token: 0x040053B0 RID: 21424
		private OverlayTargetState overlayTargetState = default(OverlayTargetState);

		// Token: 0x040053B1 RID: 21425
		private ColorWheelModel model;

		// Token: 0x040053B2 RID: 21426
		private Dictionary<GemColor, SpriteRenderer> wheelSlices;

		// Token: 0x040053B3 RID: 21427
		private Dictionary<GemColor, SpriteRenderer> glowingMatchedSlices;

		// Token: 0x040053B4 RID: 21428
		private Dictionary<GemColor, ParticleSystem> wheelEffects;

		// Token: 0x040053B5 RID: 21429
		private Animation _animation;
	}
}
