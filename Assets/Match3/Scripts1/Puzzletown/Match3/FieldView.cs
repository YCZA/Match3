using System;
using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// eli key point FieldView
	public class FieldView : AReleasable
	{
		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06002A77 RID: 10871 RVA: 0x000C2552 File Offset: 0x000C0952
		public IntVector2 GridPosition
		{
			get
			{
				return this.field.gridPosition;
			}
		}

		// Token: 0x06002A78 RID: 10872 RVA: 0x000C2560 File Offset: 0x000C0960
		public void Show(Field field = null)
		{
			this.CacheSpriteRendererReferences();
			if (field != null)
			{
				this.field = field;
			}
			this.SetBlocker(this.field.blockerIndex);
			this.SetTiles(this.field.numTiles);
			this.SetWatered(this.field.numTiles);
			this.SetChains(this.field.numChains);
			this.SetCrate(this.field.cratesIndex);
			this.SetDropItemExits(this.field.IsExitDropItems);
			this.SetClimberExits(this.field.IsExitClimber);
			this.SetWindows(this.field.isWindow, false);
			this.SetPortals(this.field.portalId);
			this.SetSpawner(this.field.spawnType);
			if (!this.field.isGrowingWindow)
			{
				this.DisableGrowingWindow();
			}
		}

		// Token: 0x06002A79 RID: 10873 RVA: 0x000C2641 File Offset: 0x000C0A41
		public void AssignBoardView(BoardView boardView)
		{
			this.boardView = boardView;
		}

		// Token: 0x06002A7A RID: 10874 RVA: 0x000C264A File Offset: 0x000C0A4A
		public void SetWindows(bool isWindow, bool isAboveWindow = false)
		{
			this.spriteWindow.gameObject.SetActive(isWindow);
			this.spriteWindowBridge.gameObject.SetActive(isWindow && isAboveWindow);
		}

		// Token: 0x06002A7B RID: 10875 RVA: 0x000C2677 File Offset: 0x000C0A77
		public void SetGrowingWindow(GrowingWindowView.GrowDirection growDirection)
		{
			this.growingWindow.gameObject.SetActive(true);
			this.growingWindow.AnimateGrowingWindow(this.field, growDirection);
		}

		// Token: 0x06002A7C RID: 10876 RVA: 0x000C269C File Offset: 0x000C0A9C
		public void SetGrowingWindowWithBridge()
		{
			this.growingWindow.gameObject.SetActive(true);
			this.growingWindow.AnimateGrowingWindowBridge(this.field);
		}

		// Token: 0x06002A7D RID: 10877 RVA: 0x000C26C0 File Offset: 0x000C0AC0
		public void DisableGrowingWindow()
		{
			this.growingWindow.gameObject.SetActive(false);
		}

		// Token: 0x06002A7E RID: 10878 RVA: 0x000C26D3 File Offset: 0x000C0AD3
		public void ResetAndDisableGrowingWindow()
		{
			this.growingWindow.ResetAndDisableGameObject();
		}

		// Token: 0x06002A7F RID: 10879 RVA: 0x000C26E0 File Offset: 0x000C0AE0
		public void AnimateClearGrowingWindow()
		{
			this.growingWindow.AnimateClearGrowingWindow();
		}

		// Token: 0x06002A80 RID: 10880 RVA: 0x000C26ED File Offset: 0x000C0AED
		public void SetChains(int numChains)
		{
			this.SetSprite(numChains, this.spriteChain, this.chainSpriteManager);
		}

		// Token: 0x06002A81 RID: 10881 RVA: 0x000C2704 File Offset: 0x000C0B04
		public void SetCrate(int cratesIndex)
		{
			int hp = Crate.GetHp(cratesIndex);
			GemColor color = Crate.GetColor(cratesIndex);
			bool flag = color != GemColor.Undefined;
			bool flag2 = hp != 0;
			this.spriteCrate.gameObject.SetActive(flag2 && !flag);
			this.colorCrateSprite.gameObject.SetActive(flag2 && flag);
			if (flag2)
			{
				if (!flag)
				{
					this.spriteCrate.sprite = this.crateSpriteManager.GetSprite(hp);
				}
				else
				{
					this.colorCrateSprite.sprite = this.colorCrateSpriteManager.GetSprite(hp);
					float valueForLookUpTableColumn = FieldView.GetValueForLookUpTableColumn(color);
					this.colorCrateSprite.color = new Color(valueForLookUpTableColumn, 0f, 0f, 0f);
				}
			}
			else
			{
				this.SetSpawner(this.field.spawnType);
			}
		}

		// Token: 0x06002A82 RID: 10882 RVA: 0x000C27E4 File Offset: 0x000C0BE4
		public static float GetValueForLookUpTableColumn(GemColor gemColor)
		{
			return (float)gemColor / 10f + 0.05f;
		}

		// Token: 0x06002A83 RID: 10883 RVA: 0x000C27F4 File Offset: 0x000C0BF4
		public void SetTiles(int numTiles)
		{
			this.spriteBelowTile.gameObject.SetActive(numTiles == 1 && this.field.hiddenItemId != 0);
			if (numTiles <= 2)
			{
				this.SetSprite(numTiles, this.spriteTile, this.tileSpriteManager);
			}
			else
			{
				this.spriteTile.gameObject.SetActive(false);
			}
		}

		// Token: 0x06002A84 RID: 10884 RVA: 0x000C285C File Offset: 0x000C0C5C
		public void SetBlocker(int blockerIndex)
		{
			bool flag = Stone.IsStone(blockerIndex);
			bool flag2 = ResistantBlocker.IsResistantBlocker(blockerIndex);
			this.spriteStone.gameObject.SetActive(flag);
			this.resistantBlocker.gameObject.SetActive(flag2);
			if (flag)
			{
				this.SetSprite(blockerIndex, this.spriteStone, this.stoneSpriteManager);
			}
			else if (flag2)
			{
				int hp = ResistantBlocker.GetHp(blockerIndex);
				this.resistantBlocker.AnimateIdle(hp);
			}
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x000C28D0 File Offset: 0x000C0CD0
		public void SetWatered(int numTile)
		{
			this.spriteWater.gameObject.SetActive(numTile == 3);
		}

		// Token: 0x06002A86 RID: 10886 RVA: 0x000C28E6 File Offset: 0x000C0CE6
		public void PlayDefinedSpawningAnimation(bool setClosed)
		{
			this.definedGemSpawner.AnimateShellState(setClosed);
		}

		// Token: 0x06002A87 RID: 10887 RVA: 0x000C28F4 File Offset: 0x000C0CF4
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

		// Token: 0x06002A88 RID: 10888 RVA: 0x000C297F File Offset: 0x000C0D7F
		public override void Release(float delay = 0f)
		{
			this.StopUpdatingOverlayFade();
			this.RemoveListeners();
			base.Release(delay);
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x000C2994 File Offset: 0x000C0D94
		public void HandleBoostOverlayTick(float elapsedTime)
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
					this.boostOverlayMask.enabled = this.overlayTargetState.enableBoostMask;
					this.overlayTargetState.shouldUpdate = false;
					this.overlayTargetState.elapsedTime = 0f;
				}
			}
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x000C2A57 File Offset: 0x000C0E57
		private void Awake()
		{
			this.materialPropertyBlock = new MaterialPropertyBlock();
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x000C2A64 File Offset: 0x000C0E64
		private void OnDestroy()
		{
			this.RemoveListeners();
		}

		// Token: 0x06002A8C RID: 10892 RVA: 0x000C2A6C File Offset: 0x000C0E6C
		private void CacheSpriteRendererReferences()
		{
			if (this.spritesToTintForBoostOverlay == null)
			{
				this.spritesToTintForBoostOverlay = new SpriteRenderer[]
				{
					this.spriteStone,
					this.spriteChain,
					this.spriteWindow,
					this.spriteWindowBridge,
					this.spriteWater,
					this.spriteCrate,
					this.spriteDropItemExit,
					this.spritePortalEntrance,
					this.spritePortalEntrance,
					this.spritePortalEntrance,
					this.growingWindow.spriteGrowingWindow,
					this.growingWindow.spriteGrowingWindowBridge1,
					this.growingWindow.spriteGrowingWindowBridge2
				};
			}
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x000C2B19 File Offset: 0x000C0F19
		private void RemoveListeners()
		{
			if (this.boostOverlayController != null)
			{
				this.boostOverlayController.onBoostOverlayStateChanged.RemoveListener(new Action<BoostOverlayState>(this.HandleBoostOverlayAlphaChanged));
			}
		}

		// Token: 0x06002A8E RID: 10894 RVA: 0x000C2B44 File Offset: 0x000C0F44
		private void HandleBoostOverlayAlphaChanged(BoostOverlayState boostOverlayState)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			this.StopUpdatingOverlayFade();
			bool flag = !this.field.HasGem && !this.field.IsBlocked && !this.field.HasCrates && !this.field.isWindow && !this.field.isGrowingWindow;
			if (boostOverlayState.isOn && !flag)
			{
				this.TryTurnOnBoostMask(boostOverlayState);
			}
			else
			{
				this.TurnOffBoostMask(boostOverlayState);
			}
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x000C2BDF File Offset: 0x000C0FDF
		private void StartUpdatingBoostOverlayFade()
		{
			this.overlayTargetState.shouldUpdate = true;
		}

		// Token: 0x06002A90 RID: 10896 RVA: 0x000C2BED File Offset: 0x000C0FED
		private void StopUpdatingOverlayFade()
		{
			this.overlayTargetState.shouldUpdate = false;
			this.overlayTargetState.elapsedTime = 0f;
		}

		// Token: 0x06002A91 RID: 10897 RVA: 0x000C2C0C File Offset: 0x000C100C
		private void TryTurnOnBoostMask(BoostOverlayState boostOverlayState)
		{
			bool flag = this.field.CanBeTargetedByBoost(boostOverlayState.boostType);
			int cornerFillTmp;
			int overlayMaskSpriteIndexBasedOnPosition = this.boardView.GetOverlayMaskSpriteIndexBasedOnPosition(this.GridPosition, boostOverlayState.boostType, out cornerFillTmp);
			this.boostOverlayMask.sprite = this.boostOverlaySprites[overlayMaskSpriteIndexBasedOnPosition];
			Sprite sprite = this.boostOverlayMask.sprite;
			this.overlaySpriteTextureOffsetX = sprite.textureRect.xMin / (float)sprite.texture.width;
			this.overlaySpriteTextureOffsetY = sprite.textureRect.yMin / (float)sprite.texture.height;
			this.boostOverlayMask.enabled = true;
			this.FillInCorners(cornerFillTmp);
			if (flag)
			{
				this.MoveExitIndicatorsAboveOverlay();
				float delayInSecs = (!boostOverlayState.shouldChangeInstantly) ? BoostOverlayController.BOOST_OVERLAY_FADE_DURATION_SECS : 0f;
				this.overlayTargetState.Set(delayInSecs, true, false, true);
				this.TryTintGemViewsAndColorCrates(false);
				this.TryTintResistantBlocker(false);
			}
			else
			{
				this.MoveExitIndicatorsBelowOverlay();
				float delayInSecs2 = (!boostOverlayState.shouldChangeInstantly) ? BoostOverlayController.BOOST_OVERLAY_FADE_DURATION_SECS : 0f;
				this.overlayTargetState.Set(delayInSecs2, true, true, true);
				this.TryTintGemViewsAndColorCrates(true);
				this.TryTintResistantBlocker(true);
			}
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x000C2D4C File Offset: 0x000C114C
		private void TryTintResistantBlocker(bool darken)
		{
			if (this.field.IsResistantBlocker)
			{
				Color tint = (!darken) ? Color.white : FieldView.BOOST_OVERLAY_INACTIVE_TINT;
				this.resistantBlocker.ApplyTintColor(tint);
			}
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x000C2D8C File Offset: 0x000C118C
		private void FillInCorners(int cornerFillTmp)
		{
			float r = ((cornerFillTmp & 8) <= 0) ? 0f : 1f;
			float g = ((cornerFillTmp & 4) <= 0) ? 0f : 1f;
			float b = ((cornerFillTmp & 2) <= 0) ? 0f : 1f;
			float a = ((cornerFillTmp & 1) <= 0) ? 0f : 1f;
			Color value = new Color(r, g, b, a);
			this.boostOverlayMask.GetPropertyBlock(this.materialPropertyBlock);
			this.materialPropertyBlock.SetColor("_CornerFill", value);
			this.boostOverlayMask.SetPropertyBlock(this.materialPropertyBlock);
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x000C2E3C File Offset: 0x000C123C
		private void TurnOffBoostMask(BoostOverlayState boostOverlayState)
		{
			this.MoveExitIndicatorsBelowOverlay();
			this.TryTintGemViewsAndColorCrates(false);
			this.TryTintResistantBlocker(false);
			float delayInSecs = (!boostOverlayState.shouldChangeInstantly) ? BoostOverlayController.BOOST_OVERLAY_FADE_DURATION_SECS : 0f;
			this.overlayTargetState.Set(delayInSecs, false, false, false);
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x000C2E88 File Offset: 0x000C1288
		private void MoveExitIndicatorsAboveOverlay()
		{
			this.MoveIndicatorsToLayer("UIMask");
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x000C2E95 File Offset: 0x000C1295
		private void MoveExitIndicatorsBelowOverlay()
		{
			this.MoveIndicatorsToLayer("Window");
		}

		// Token: 0x06002A97 RID: 10903 RVA: 0x000C2EA4 File Offset: 0x000C12A4
		private void MoveIndicatorsToLayer(string layerName)
		{
			if (this.spritePortalExit.isVisible)
			{
				this.spritePortalExit.sortingLayerName = layerName;
			}
			if (this.spritePortalEntrance.isVisible)
			{
				this.spritePortalEntrance.sortingLayerName = layerName;
			}
			if (this.spriteDropItemExit.isVisible)
			{
				this.spriteDropItemExit.sortingLayerName = layerName;
			}
			if (this.spriteClimberExit.isVisible)
			{
				this.spriteClimberExit.sortingLayerName = layerName;
			}
		}

		// Token: 0x06002A98 RID: 10904 RVA: 0x000C2F24 File Offset: 0x000C1324
		private void TryTintGemViewsAndColorCrates(bool darken)
		{
			GemView gemView = this.TryGetTintableGemView();
			if (gemView != null)
			{
				gemView.TryTint(darken);
			}
			this.TryTintColorCrate(darken);
		}

		// Token: 0x06002A99 RID: 10905 RVA: 0x000C2F54 File Offset: 0x000C1354
		private GemView TryGetTintableGemView()
		{
			if (this.field.HasGem && (this.field.gem.IsClimber || this.field.gem.IsDroppable || this.field.gem.IsCoveredByDirt || this.field.gem.IsCannon || this.field.gem.IsIced || this.field.gem.color == GemColor.Rainbow || this.field.gem.color == GemColor.Cannonball))
			{
				return this.boardView.GetGemView(this.GridPosition, false);
			}
			return null;
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x000C301C File Offset: 0x000C141C
		private void TryTintColorCrate(bool darken)
		{
			if (this.field.HasCrates)
			{
				GemColor color = Crate.GetColor(this.field.cratesIndex);
				if (color != GemColor.Undefined)
				{
					this.colorCrateSprite.GetPropertyBlock(this.materialPropertyBlock);
					this.materialPropertyBlock.SetColor("_Tint", (!darken) ? Color.white : FieldView.BOOST_OVERLAY_INACTIVE_TINT);
					this.colorCrateSprite.SetPropertyBlock(this.materialPropertyBlock);
				}
			}
		}

		// Token: 0x06002A9B RID: 10907 RVA: 0x000C3098 File Offset: 0x000C1498
		private void TintSpritesForBoostOverlay(bool darken, float tintRatio, bool maskDarken)
		{
			Color color = (!darken) ? FieldView.BOOST_OVERLAY_INACTIVE_TINT : Color.white;
			Color color2 = (!darken) ? Color.white : FieldView.BOOST_OVERLAY_INACTIVE_TINT;
			tintRatio = Mathf.Clamp01(tintRatio);
			Color color3 = color + (color2 - color) * tintRatio;
			float num = (!maskDarken) ? FieldView.BOOST_OVERLAY_MASK_MAX_ALPHA : 0f;
			float num2 = (!maskDarken) ? 0f : FieldView.BOOST_OVERLAY_MASK_MAX_ALPHA;
			if (!Mathf.Approximately(this.boostOverlayMask.color.a, num2))
			{
				this.boostOverlayMask.color = new Color(this.overlaySpriteTextureOffsetX, this.overlaySpriteTextureOffsetY, 0f, num + (num2 - num) * tintRatio);
			}
			foreach (SpriteRenderer spriteRenderer in this.spritesToTintForBoostOverlay)
			{
				if (spriteRenderer.color != color2)
				{
					spriteRenderer.color = color3;
				}
			}
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x000C31A8 File Offset: 0x000C15A8
		private void SetSprite(int num, SpriteRenderer sRenderer, ACountSpriteManager manager)
		{
			bool flag = num > 0;
			sRenderer.gameObject.SetActive(flag);
			if (flag)
			{
				sRenderer.sprite = manager.GetSprite(num);
			}
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x000C31D9 File Offset: 0x000C15D9
		private void SetDropItemExits(bool isExit)
		{
			this.dropItemExit.gameObject.SetActive(isExit);
		}

		// Token: 0x06002A9E RID: 10910 RVA: 0x000C31EC File Offset: 0x000C15EC
		private void SetClimberExits(bool isExit)
		{
			this.climberExit.gameObject.SetActive(isExit);
		}

		// Token: 0x06002A9F RID: 10911 RVA: 0x000C3200 File Offset: 0x000C1600
		private void SetSpawner(SpawnTypes type)
		{
			bool active = type == SpawnTypes.DefinedGem && !this.field.HasCrates;
			this.definedGemSpawner.gameObject.SetActive(active);
		}

		// Token: 0x06002AA0 RID: 10912 RVA: 0x000C3238 File Offset: 0x000C1638
		private void SetPortals(int portalId)
		{
			this.spritePortalExit.gameObject.SetActive(portalId > 0 && portalId % 2 == 0);
			this.spritePortalEntrance.gameObject.SetActive(portalId > 0 && portalId % 2 == 1);
		}

		// Token: 0x040053CD RID: 21453
		public GameObject goChain;

		// Token: 0x040053CE RID: 21454
		public SpriteRenderer spriteTile;

		// Token: 0x040053CF RID: 21455
		public SpriteRenderer spriteCrate;

		// Token: 0x040053D0 RID: 21456
		public SpriteRenderer colorCrateSprite;

		// Token: 0x040053D1 RID: 21457
		private const int COUNT_COLOR_COLUMNS_LUT = 10;

		// Token: 0x040053D2 RID: 21458
		private const float OFFSET_TO_COLUMN_MIDDLE = 0.05f;

		// Token: 0x040053D3 RID: 21459
		public const string EXIT_INDICATOR_SORTING_LAYER = "Window";

		// Token: 0x040053D4 RID: 21460
		[SerializeField]
		private SpriteRenderer spriteBelowTile;

		// Token: 0x040053D5 RID: 21461
		[SerializeField]
		private SpriteRenderer spriteStone;

		// Token: 0x040053D6 RID: 21462
		[SerializeField]
		private SpriteRenderer spriteChain;

		// Token: 0x040053D7 RID: 21463
		[SerializeField]
		private SpriteRenderer spriteWindow;

		// Token: 0x040053D8 RID: 21464
		[SerializeField]
		private SpriteRenderer spritePortalExit;

		// Token: 0x040053D9 RID: 21465
		[SerializeField]
		private SpriteRenderer spritePortalEntrance;

		// Token: 0x040053DA RID: 21466
		[SerializeField]
		private SpriteRenderer spriteWindowBridge;

		// Token: 0x040053DB RID: 21467
		[SerializeField]
		private SpriteRenderer spriteWater;

		// Token: 0x040053DC RID: 21468
		[SerializeField]
		private GameObject dropItemExit;

		// Token: 0x040053DD RID: 21469
		[SerializeField]
		private GameObject climberExit;

		// Token: 0x040053DE RID: 21470
		[SerializeField]
		private SpriteRenderer spriteDropItemExit;

		// Token: 0x040053DF RID: 21471
		[SerializeField]
		private SpriteRenderer spriteClimberExit;

		// Token: 0x040053E0 RID: 21472
		[SerializeField]
		private DefinedSpawnerView definedGemSpawner;

		// Token: 0x040053E1 RID: 21473
		[SerializeField]
		private ResistantBlockerView resistantBlocker;

		// Token: 0x040053E2 RID: 21474
		[SerializeField]
		private GrowingWindowView growingWindow;

		// Token: 0x040053E3 RID: 21475
		[SerializeField]
		private TileSpriteManager tileSpriteManager;

		// Token: 0x040053E4 RID: 21476
		[SerializeField]
		private StoneSpriteManager stoneSpriteManager;

		// Token: 0x040053E5 RID: 21477
		[SerializeField]
		private ChainSpriteManager chainSpriteManager;

		// Token: 0x040053E6 RID: 21478
		[SerializeField]
		private CrateSpriteManager crateSpriteManager;

		// Token: 0x040053E7 RID: 21479
		[SerializeField]
		private ColorCrateSpriteManager colorCrateSpriteManager;

		// Token: 0x040053E8 RID: 21480
		[SerializeField]
		private SpriteRenderer boostOverlayMask;

		// Token: 0x040053E9 RID: 21481
		[SerializeField]
		private Sprite[] boostOverlaySprites;

		// Token: 0x040053EA RID: 21482
		private Field field;

		// Token: 0x040053EB RID: 21483
		private BoostOverlayController boostOverlayController;

		// Token: 0x040053EC RID: 21484
		private OverlayTargetState overlayTargetState = default(OverlayTargetState);

		// Token: 0x040053ED RID: 21485
		private BoardView boardView;

		// Token: 0x040053EE RID: 21486
		private MaterialPropertyBlock materialPropertyBlock;

		// Token: 0x040053EF RID: 21487
		private float overlaySpriteTextureOffsetX;

		// Token: 0x040053F0 RID: 21488
		private float overlaySpriteTextureOffsetY;

		// Token: 0x040053F1 RID: 21489
		private SpriteRenderer[] spritesToTintForBoostOverlay;

		// Token: 0x040053F2 RID: 21490
		public static Color BOOST_OVERLAY_INACTIVE_TINT = new Color(0.6f, 0.6f, 0.6f, 1f);

		// Token: 0x040053F3 RID: 21491
		public static float BOOST_OVERLAY_MASK_MAX_ALPHA = 0.4f;
	}
}
