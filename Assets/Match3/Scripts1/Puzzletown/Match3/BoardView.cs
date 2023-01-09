using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Shared.DataStructures;
using Match3.Scripts1.Shared.M3Engine;
using Shared.Pooling;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000694 RID: 1684
	[RequireComponent(typeof(GemViewFactory))]
	[RequireComponent(typeof(FieldViewFactory))]
	public class BoardView : MonoBehaviour
	{
		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x060029D8 RID: 10712 RVA: 0x000BF68F File Offset: 0x000BDA8F
		// (set) Token: 0x060029D9 RID: 10713 RVA: 0x000BF697 File Offset: 0x000BDA97
		public float TimeSinceLastValidSwap { get; set; }

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x060029DA RID: 10714 RVA: 0x000BF6A0 File Offset: 0x000BDAA0
		public GemColorToColor GemColorToColor
		{
			get
			{
				return this.gemColorToColor;
			}
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x060029DB RID: 10715 RVA: 0x000BF6A8 File Offset: 0x000BDAA8
		public BoardAnimationController BoardAnimationController
		{
			get
			{
				return this.animationController;
			}
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x060029DC RID: 10716 RVA: 0x000BF6B0 File Offset: 0x000BDAB0
		public IEnumerable<KeyValuePair<int, HiddenItemView>> HiddenItemViews
		{
			get
			{
				return this.hiddenItemViews;
			}
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x060029DD RID: 10717 RVA: 0x000BF6B8 File Offset: 0x000BDAB8
		public IEnumerable<KeyValuePair<IntVector2, ColorWheelView>> ColorWheelViews
		{
			get
			{
				return this.colorWheelViews;
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x060029DE RID: 10718 RVA: 0x000BF6C0 File Offset: 0x000BDAC0
		public bool IsIngameBoostSelected
		{
			get
			{
				return this.boostOverlayController != null && this.boostOverlayController.CurrentSelectedBoost != Boosts.invalid;
			}
		}

		// Token: 0x060029DF RID: 10719 RVA: 0x000BF6E4 File Offset: 0x000BDAE4
		public void Initialize(LevelConfig config, BoostOverlayController overlayController)
		{
			this.currentLevel = config.Level.level;
			this.currentTier = config.Level.tier;
			this.boostOverlayController = overlayController;
		}

		// Token: 0x060029E0 RID: 10720 RVA: 0x000BF720 File Offset: 0x000BDB20
		public static bool IsInvalidMove(List<List<IMatchResult>> results)
		{
			return (results.Count == 2 && results[0].Count == 1 && results[1].Count == 1 && results[0][0] is Move && results[1][0] is Move) || (results.Count == 1 && results[0][0] is BlockedSwap);
		}

		// Token: 0x060029E1 RID: 10721 RVA: 0x000BF7B2 File Offset: 0x000BDBB2
		public void ShowShuffleBanner(float duration)
		{
			base.StartCoroutine(this.banners.ShowShuffle(duration));
		}

		// Token: 0x060029E2 RID: 10722 RVA: 0x000BF7C7 File Offset: 0x000BDBC7
		public void SetLastHurrayStarted()
		{
			this.isInLastHurray = true;
		}

		// Token: 0x060029E3 RID: 10723 RVA: 0x000BF7D0 File Offset: 0x000BDBD0
		private void Awake()
		{
			this.animationController = base.GetComponent<BoardAnimationController>();
			this.gemViewFactory = base.GetComponent<GemViewFactory>();
			this.fieldViewFactory = base.GetComponent<FieldViewFactory>();
			this.tournamentItemCollectedViewFactory = base.GetComponent<TournamentItemCollectedViewFactory>();
			this.boardBorderFactory = base.GetComponent<BoardBorderFactory>();
			this.boardDecorationsFactory = base.GetComponent<BoardDecorationsFactory>();
			this.gemColorToColor = base.GetComponent<GemColorToColor>();
			this.animationController.onFinished.AddListener(new Action(this.HandleFinished));
			this.hintCountdown = 7f;
			this.TimeSinceLastValidSwap = -2.1474836E+09f;
			this.resizer.Resize();
		}

		// Token: 0x060029E4 RID: 10724 RVA: 0x000BF870 File Offset: 0x000BDC70
		private void Update()
		{
			if (this.isAnimating || this.isInLastHurray || this.IsIngameBoostSelected)
			{
				return;
			}
			this.TimeSinceLastValidSwap += Time.deltaTime;
			if (this.TimeSinceLastValidSwap >= this.hintCountdown)
			{
				if (this.hintCountdown == 7f)
				{
					this.onNeedPossibleMoves.Dispatch();
				}
				else
				{
					this.HighlightNextMove();
				}
				this.ResetHintTimer();
			}
		}

		// Token: 0x060029E5 RID: 10725 RVA: 0x000BF8F0 File Offset: 0x000BDCF0
		public void HandleSwapped(Move move)
		{
			if (this.expectedMove == default(Move) || move.HasSamePositions(this.expectedMove))
			{
				this.onSwapped.Dispatch(move);
			}
		}

		// Token: 0x060029E6 RID: 10726 RVA: 0x000BF934 File Offset: 0x000BDD34
		public void SetExpectedMove(Move move)
		{
			this.expectedMove = move;
		}

		// Token: 0x060029E7 RID: 10727 RVA: 0x000BF93D File Offset: 0x000BDD3D
		public void HandleHighlightNextMove(MatchCandidate candidate)
		{
			if (candidate != null)
			{
				this.highlightCandidate = candidate;
				this.hintCountdown = 2f;
				this.HighlightNextMove();
			}
			else
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Did not receive a valid candidate"
				});
			}
		}

		// Token: 0x060029E8 RID: 10728 RVA: 0x000BF975 File Offset: 0x000BDD75
		public void HandleFinished()
		{
			if (this.isOutOfSync)
			{
				this.isOutOfSync = false;
				this.CreateViews(this.fields);
			}
			this.onAnimationFinished.Dispatch();
			this.isAnimating = false;
		}

		// Token: 0x060029E9 RID: 10729 RVA: 0x000BF9A7 File Offset: 0x000BDDA7
		public void SetupFields(int numFields)
		{
			this.ReleaseExistingViews();
			this.gemViews = new List<GemView>();
			this.fieldViewMap = new Map<FieldView>(numFields);
		}

		// Token: 0x060029EA RID: 10730 RVA: 0x000BF9C8 File Offset: 0x000BDDC8
		public void CreateViews(Fields fields)
		{
			this.fields = fields;
			this.SetupFields(fields.size);
			// Debug.LogError("createViews");
			this.RecreateBorders();
			IEnumerator enumerator = fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					FieldView fieldView = this.CreateFieldView(field);
					if (fieldView && field.HasGem)
					{
						this.CreateGemView(field.gem, fieldView.transform);
					}
					if (field.isWindow)
					{
						this.CreateConnectedWindowViews(field, fields);
					}
					else if (field.isGrowingWindow)
					{
						this.ShowGrowingWindow(field, fields);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			this.UpdateDirtBorder();
			this.ResetHintTimer();
		}

		// Token: 0x060029EB RID: 10731 RVA: 0x000BFAA4 File Offset: 0x000BDEA4
		public void RecreateBorders()
		{
			if (this.boardBorderFactory)
			{
				this.boardBorderFactory.GenerateBorder(this.fields);
			}
			if (this.boardDecorationsFactory)
			{
				this.boardDecorationsFactory.GenerateDecorations(this.fields);
			}
		}

		// Token: 0x060029EC RID: 10732 RVA: 0x000BFAF4 File Offset: 0x000BDEF4
		public int GetOverlayMaskSpriteIndexBasedOnPosition(IntVector2 position, Boosts boostType, out int cornerFill)
		{
			int num = (!this.FieldHasNoDarkOverlay(position + IntVector2.Left, boostType)) ? 8 : 0;
			int num2 = (!this.FieldHasNoDarkOverlay(position + IntVector2.Up, boostType)) ? 4 : 0;
			int num3 = (!this.FieldHasNoDarkOverlay(position + IntVector2.Right, boostType)) ? 2 : 0;
			int num4 = (!this.FieldHasNoDarkOverlay(position + IntVector2.Down, boostType)) ? 1 : 0;
			bool flag = num == 0 && num4 == 0 && !this.FieldHasNoDarkOverlay(position + IntVector2.Left + IntVector2.Down, boostType);
			bool flag2 = num == 0 && num2 == 0 && !this.FieldHasNoDarkOverlay(position + IntVector2.Left + IntVector2.Up, boostType);
			bool flag3 = num3 == 0 && num2 == 0 && !this.FieldHasNoDarkOverlay(position + IntVector2.Right + IntVector2.Up, boostType);
			bool flag4 = num3 == 0 && num4 == 0 && !this.FieldHasNoDarkOverlay(position + IntVector2.Right + IntVector2.Down, boostType);
			cornerFill = 0;
			cornerFill += ((!flag) ? 0 : 8);
			cornerFill += ((!flag2) ? 0 : 4);
			cornerFill += ((!flag3) ? 0 : 2);
			cornerFill += ((!flag4) ? 0 : 1);
			return num + num2 + num3 + num4;
		}

		// Token: 0x060029ED RID: 10733 RVA: 0x000BFC94 File Offset: 0x000BE094
		private bool FieldHasNoDarkOverlay(IntVector2 pos, Boosts boostType)
		{
			return this.fields.IsValid(pos) && this.fields[pos].isOn && (this.fields[pos].CanBeTargetedByBoost(boostType) || (this.fields[pos].HasGem && this.fields[pos].gem.IsSpecialColor) || this.fields[pos].HasCrates || this.fields[pos].isWindow || this.fields[pos].IsBlocked || this.fields[pos].isGrowingWindow);
		}

		// Token: 0x060029EE RID: 10734 RVA: 0x000BFD6C File Offset: 0x000BE16C
		public void CreateConnectedWindowViews(Field field, Fields fields)
		{
			IntVector2 gridPosition = field.gridPosition;
			IntVector2 vec = field.gridPosition + IntVector2.Up;
			IntVector2 intVector = field.gridPosition + IntVector2.Down;
			bool flag = fields.IsValid(gridPosition) && field.isWindow;
			bool flag2 = fields.IsValid(vec) && fields[vec].isWindow;
			this.GetFieldView(gridPosition).SetWindows(flag, flag && flag2);
			if (fields.IsValid(intVector) && fields[intVector].isOn)
			{
				bool isWindow = fields[intVector].isWindow;
				this.GetFieldView(intVector).SetWindows(isWindow, flag && isWindow);
			}
		}

		// Token: 0x060029EF RID: 10735 RVA: 0x000BFE34 File Offset: 0x000BE234
		public void ClearGrowingWindow(Field field, Fields fields)
		{
			IntVector2 gridPosition = field.gridPosition;
			IntVector2 intVector = field.gridPosition + IntVector2.Down;
			this.GetFieldView(gridPosition).DisableGrowingWindow();
			if (fields.IsValid(intVector) && fields[intVector].isGrowingWindow)
			{
				this.GetFieldView(intVector).SetGrowingWindow(GrowingWindowView.GrowDirection.None);
			}
		}

		// Token: 0x060029F0 RID: 10736 RVA: 0x000BFE90 File Offset: 0x000BE290
		public void ShowGrowingWindow(Field field, Fields fields)
		{
			IntVector2 gridPosition = field.gridPosition;
			IntVector2 intVector = field.gridPosition + IntVector2.Down;
			IntVector2 vec = field.gridPosition + IntVector2.Up;
			if (field.isGrowingWindow)
			{
				if (fields.IsValid(intVector) && fields[intVector].isGrowingWindow)
				{
					this.GetFieldView(intVector).SetGrowingWindowWithBridge();
				}
				if (fields.IsValid(vec) && fields[vec].isGrowingWindow)
				{
					this.GetFieldView(gridPosition).SetGrowingWindowWithBridge();
				}
				else
				{
					this.GetFieldView(gridPosition).SetGrowingWindow(GrowingWindowView.GrowDirection.None);
				}
			}
		}

		// Token: 0x060029F1 RID: 10737 RVA: 0x000BFF38 File Offset: 0x000BE338
		public void CreateHiddenItems(HiddenItemInfoDict configItems, HiddenItemInfoDict currentItems)
		{
			this.hiddenItemViews = new Dictionary<int, HiddenItemView>();
			foreach (HiddenItemInfo hiddenItemInfo in configItems.Values)
			{
				if (currentItems == null || currentItems.ContainsKey(hiddenItemInfo.id))
				{
					this.hiddenItemViews[hiddenItemInfo.id] = this.fieldViewFactory.CreateHiddenItem(hiddenItemInfo);
				}
			}
		}

		// Token: 0x060029F2 RID: 10738 RVA: 0x000BFFD4 File Offset: 0x000BE3D4
		public void RemoveHiddenItems()
		{
			if (this.hiddenItemViews != null)
			{
				foreach (HiddenItemView hiddenItemView in this.hiddenItemViews.Values)
				{
					if (hiddenItemView != null)
					{
						global::UnityEngine.Object.Destroy(hiddenItemView.gameObject);
					}
				}
				this.hiddenItemViews.Clear();
			}
		}

		// Token: 0x060029F3 RID: 10739 RVA: 0x000C005C File Offset: 0x000BE45C
		public void CreateColorWheels(bool isEditorView = false)
		{
			this.colorWheelViews = new Dictionary<IntVector2, ColorWheelView>();
			IEnumerator enumerator = this.fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					if (field.IsColorWheelCorner && !this.colorWheelViews.ContainsKey(field.gridPosition))
					{
						ColorWheelModel model = (!this.fields.colorWheelModels.ContainsKey(field.gridPosition)) ? null : this.fields.colorWheelModels[field.gridPosition];
						ColorWheelView colorWheelView = this.fieldViewFactory.CreateColorWheel(field.gridPosition, model, this.boostOverlayController);
						if (isEditorView)
						{
							if (field.IsColorWheelVariant)
							{
								colorWheelView.sprite.sprite = colorWheelView.spriteManager.GetSimilar("3");
							}
							else
							{
								colorWheelView.sprite.sprite = colorWheelView.spriteManager.GetSimilar("4");
							}
						}
						this.colorWheelViews.Add(field.gridPosition, colorWheelView);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x060029F4 RID: 10740 RVA: 0x000C0198 File Offset: 0x000BE598
		public void RemoveColorWheels()
		{
			if (this.colorWheelViews != null)
			{
				foreach (ColorWheelView colorWheelView in this.colorWheelViews.Values)
				{
					if (colorWheelView != null)
					{
						global::UnityEngine.Object.Destroy(colorWheelView.gameObject);
					}
				}
			}
			this.colorWheelViews = null;
		}

		// Token: 0x060029F5 RID: 10741 RVA: 0x000C021C File Offset: 0x000BE61C
		public void InitializeChameleonViews(bool isEditorView = false)
		{
			IEnumerator enumerator = this.fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					if (field.HasGem && field.gem.IsChameleon)
					{
						Gem gem = field.gem;
						ChameleonView chameleonView = this.GetChameleonView(field.gridPosition);
						ChameleonVariant debugTypeText = (!gem.IsAllColorChameleon) ? ChameleonVariant.Reduced : ChameleonVariant.All;
						if (isEditorView)
						{
							chameleonView.SetDebugTypeText(debugTypeText);
							chameleonView.SetColor(GemColor.Red, GemColor.Blue);
						}
						else
						{
							bool isFieldChained = field.numChains > 0;
							chameleonView.Initialize(gem, isFieldChained);
							gem.color = GemColor.Red;
						}
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x060029F6 RID: 10742 RVA: 0x000C02FC File Offset: 0x000BE6FC
		public void HandleStepCompleted(List<List<IMatchResult>> results)
		{
			if (!BoardView.IsInvalidMove(results))
			{
				this.hintCountdown = 7f;
				this.ResetHintTimer();
			}
			this.isAnimating = true;
			this.animationController.StartAnimation(results);
		}

		// Token: 0x060029F7 RID: 10743 RVA: 0x000C0330 File Offset: 0x000BE730
		public void UpdateDirtBorder()
		{
			this.dirtViewPositions.Clear();
			foreach (DirtView dirtView in this.dirtViews)
			{
				this.dirtViewPositions.Add((IntVector2)dirtView.ViewTransform.position);
			}
			foreach (DirtView dirtView2 in this.dirtViews)
			{
				dirtView2.UpdateBorder(this.dirtViewPositions);
			}
		}

		// Token: 0x060029F8 RID: 10744 RVA: 0x000C03FC File Offset: 0x000BE7FC
		public void AddDirtView(DirtView view)
		{
			this.dirtViews.Add(view);
		}

		// Token: 0x060029F9 RID: 10745 RVA: 0x000C040A File Offset: 0x000BE80A
		public void RemoveDirtView(DirtView view)
		{
			this.dirtViews.Remove(view);
		}

		// Token: 0x060029FA RID: 10746 RVA: 0x000C041C File Offset: 0x000BE81C
		public void HighlightObjective(float delay, string objective)
		{
			if (this.climberHighlighter.IsValid(objective))
			{
				List<IMatchResult> highlights = this.climberHighlighter.GetHighlights(this.fields);
				base.StartCoroutine(this.DoHighlightObjective(delay, highlights));
			}
		}

		// Token: 0x060029FB RID: 10747 RVA: 0x000C045C File Offset: 0x000BE85C
		public IEnumerator DoHighlightObjective(float delay, List<IMatchResult> highlight)
		{
			yield return new WaitForSeconds(delay);
			this.animationController.StartSingleAnimation(highlight);
			yield break;
		}

		// Token: 0x060029FC RID: 10748 RVA: 0x000C0488 File Offset: 0x000BE888
		public FieldView CreateFieldView(Field field)
		{
			FieldView fieldView = this.fieldViewFactory.Create(field, this);
			if (fieldView)
			{
				InputController component = fieldView.GetComponent<InputController>();
				component.onSwapped.AddListener(new Action<Move>(this.HandleSwapped));
				component.onClicked.AddListener(new Action<IntVector2>(this.onClicked.Dispatch));
				component.onRightClick.AddListener(new Action<FieldView>(this.HandleRightClick));
				fieldView.SetupBoostOverlayListener(this.boostOverlayController);
			}
			this.fieldViewMap[field.gridPosition] = fieldView;
			return fieldView;
		}

		// Token: 0x060029FD RID: 10749 RVA: 0x000C051E File Offset: 0x000BE91E
		public GemView CreateGemView(Gem gem)
		{
			return this.CreateGemView(gem, this.GetFieldView(gem.position).transform);
		}

		// Token: 0x060029FE RID: 10750 RVA: 0x000C0539 File Offset: 0x000BE939
		public GemView CreateTemporaryGemView(Gem gem)
		{
			return this.gemViewFactory.Create(gem, this.GetFieldView(gem.position).transform);
		}

		// public static int addTimes = 0;
		// Token: 0x060029FF RID: 10751 RVA: 0x000C055C File Offset: 0x000BE95C
		public GemView CreateGemView(Gem gem, Transform fieldView)
		{
			GemView gemView = this.gemViewFactory.Create(gem, fieldView);
			// Debug.LogError("添加gameView: " + gemView.sprite.name);
			this.gemViews.Add(gemView);
				// addTimes++;
				// Debug.LogError("添加次数:" +addTimes + " 移除次数:" + removeTimes + " 当前总数:" + gemViews.Count);
			return gemView;
		}

		// Token: 0x06002A00 RID: 10752 RVA: 0x000C0584 File Offset: 0x000BE984
		public GemView CreateGemView(SpawnResult spawnResult)
		{
			FieldView fieldView = this.fieldViewMap[spawnResult.position];
			GemView gemView = this.CreateGemView(spawnResult.gem, fieldView.transform);
			IntVector2 pos = IntVector2.OppositeVector(spawnResult.direction);
			gemView.transform.position += (Vector3)pos;
			return gemView;
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x000C05E4 File Offset: 0x000BE9E4
		public GemView GetGemView(IntVector2 gridPos, bool assert = true)
		{
			foreach (GemView gemView in this.gemViews)
			{
				if (gemView)
				{
					// key point 通过位置来判断GemView
					IntVector2 a = (IntVector2)gemView.transform.localPosition;
					if (a == gridPos)
					{
						return gemView;
					}
				}
			}
			if (assert)
			{
				M3_LevelRoot componentInParent = base.GetComponentInParent<M3_LevelRoot>();
				Gem gem = this.fields[gridPos].gem;
				SerializableFields serializableFields = null;
				Move move = default(Move);
				if (componentInParent.DebugUi)
				{
					FieldSerializerProxy serializer = componentInParent.DebugUi.GetComponent<M3DebugMenu>().Serializer;
					serializableFields = serializer.PrevStepFields;
					move = serializer.PrevMove;
				}
				GemViewNotFoundData data = new GemViewNotFoundData(gridPos, serializableFields, move, gem, this.currentLevel, this.currentTier);
				this.isOutOfSync = true;
				throw new GemViewNotFoundException(data);
			}
			return null;
		}

		// Token: 0x06002A02 RID: 10754 RVA: 0x000C0700 File Offset: 0x000BEB00
		public ColorWheelView GetColorWheelView(IntVector2 gridPos)
		{
			IntVector2 colorWheelViewCorner = this.GetColorWheelViewCorner(gridPos);
			if (this.colorWheelViews.ContainsKey(colorWheelViewCorner))
			{
				return this.colorWheelViews[colorWheelViewCorner];
			}
			return null;
		}

		// Token: 0x06002A03 RID: 10755 RVA: 0x000C0734 File Offset: 0x000BEB34
		public ChameleonView GetChameleonView(IntVector2 gridPos)
		{
			GemView gemView = this.GetGemView(gridPos, true);
			return (!(gemView != null)) ? null : gemView.GetComponentInChildren<ChameleonView>();
		}

		// Token: 0x06002A04 RID: 10756 RVA: 0x000C0764 File Offset: 0x000BEB64
		public void DestroyColorWheelView(IntVector2 gridPos, float delay)
		{
			IntVector2 colorWheelViewCorner = this.GetColorWheelViewCorner(gridPos);
			if (this.colorWheelViews.ContainsKey(colorWheelViewCorner))
			{
				this.colorWheelViews[colorWheelViewCorner].Release(delay);
				global::UnityEngine.Object.Destroy(this.colorWheelViews[colorWheelViewCorner].gameObject, delay);
				this.colorWheelViews.Remove(colorWheelViewCorner);
			}
		}

		// Token: 0x06002A05 RID: 10757 RVA: 0x000C07C0 File Offset: 0x000BEBC0
		private IntVector2 GetColorWheelViewCorner(IntVector2 gridPos)
		{
			if (this.colorWheelViews.ContainsKey(gridPos))
			{
				return gridPos;
			}
			IntVector2 intVector = new IntVector2(gridPos.x - 1, gridPos.y);
			if (this.colorWheelViews.ContainsKey(intVector))
			{
				return intVector;
			}
			IntVector2 intVector2 = new IntVector2(gridPos.x, gridPos.y + 1);
			if (this.colorWheelViews.ContainsKey(intVector2))
			{
				return intVector2;
			}
			IntVector2 intVector3 = new IntVector2(gridPos.x - 1, gridPos.y + 1);
			if (this.colorWheelViews.ContainsKey(intVector3))
			{
				return intVector3;
			}
			return default(IntVector2);
		}

		// Token: 0x06002A06 RID: 10758 RVA: 0x000C0869 File Offset: 0x000BEC69
		public Field GetField(IntVector2 gridPos)
		{
			return this.fields[gridPos];
		}

		// Token: 0x06002A07 RID: 10759 RVA: 0x000C0877 File Offset: 0x000BEC77
		public FieldView GetFieldView(IntVector2 gridPos)
		{
			return this.fieldViewMap[gridPos];
		}

		// Token: 0x06002A08 RID: 10760 RVA: 0x000C0885 File Offset: 0x000BEC85
		public TournamentItemCollectedView GetTournamentCollectedItemView(Vector3 position, TournamentType type)
		{
			return this.tournamentItemCollectedViewFactory.CreateTournamentCollectedItem(position, type);
		}

		// Token: 0x06002A09 RID: 10761 RVA: 0x000C0894 File Offset: 0x000BEC94
		public HiddenItemView GetHiddenItemView(int id)
		{
			return this.hiddenItemViews.GetValueOrDefault(id);
		}

		// public static int removeTimes = 0;
		// Token: 0x06002A0A RID: 10762 RVA: 0x000C08A2 File Offset: 0x000BECA2
		public void ReleaseView(IReleasable view, float delay)
		{
			if (view is GemView)
			{
				this.gemViews.Remove((GemView)view);
				// removeTimes++;
				// Debug.LogError("添加次数:" +addTimes + " 移除次数:" + removeTimes + " 当前总数:" + gemViews.Count);
			}
			view.Release(delay);
		}

		// Token: 0x06002A0B RID: 10763 RVA: 0x000C08C8 File Offset: 0x000BECC8
		public void HighlightNextMove()
		{
			this.animationController.StartSingleAnimation(new List<IMatchResult>
			{
				this.highlightCandidate
			});
		}

		// Token: 0x06002A0C RID: 10764 RVA: 0x000C08F4 File Offset: 0x000BECF4
		private void ReleaseExistingViews()
		{
			if (this.gemViews == null)
			{
				return;
			}
			foreach (GemView gemView in this.gemViews)
			{
				gemView.gameObject.Release();
			}
			this.gemViews.Clear();
			IEnumerator enumerator2 = this.fieldViewMap.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					object obj = enumerator2.Current;
					FieldView fieldView = (FieldView)obj;
					if (fieldView)
					{
						fieldView.gameObject.Release();
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator2 as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			this.fieldViewMap = null;
		}

		// Token: 0x06002A0D RID: 10765 RVA: 0x000C09D8 File Offset: 0x000BEDD8
		private void HandleRightClick(FieldView view)
		{
			WoogaDebug.Log(new object[]
			{
				view.GridPosition
			});
			WoogaDebug.Log(new object[]
			{
				this.fields[view.GridPosition].gem
			});
		}

		// Token: 0x06002A0E RID: 10766 RVA: 0x000C0A27 File Offset: 0x000BEE27
		private void ResetHintTimer()
		{
			this.TimeSinceLastValidSwap = 0f;
		}

		// Token: 0x04005353 RID: 21331
		public static float fieldOffset = -0.5f;

		// Token: 0x04005354 RID: 21332
		public Signal<Move> onSwapped = new Signal<Move>();

		// Token: 0x04005355 RID: 21333
		public Signal<IntVector2> onClicked = new Signal<IntVector2>();

		// Token: 0x04005356 RID: 21334
		public Signal onAnimationFinished = new Signal();

		// Token: 0x04005357 RID: 21335
		public readonly System.Random viewRandomHelper = new System.Random();

		// Token: 0x04005358 RID: 21336
		public readonly Signal<Transform, GemColor, GemType> onGemCollected = new Signal<Transform, GemColor, GemType>();

		// Token: 0x04005359 RID: 21337
		public readonly Signal<Transform, string> onModifierCollected = new Signal<Transform, string>();

		// Token: 0x0400535A RID: 21338
		public readonly Signal<HiddenItemView, float> onHiddenItemFound = new Signal<HiddenItemView, float>();

		// Token: 0x0400535B RID: 21339
		public readonly Signal<TournamentItemCollectedView> onTournamentItemCollected = new Signal<TournamentItemCollectedView>();

		// Token: 0x0400535C RID: 21340
		public readonly Signal onNeedPossibleMoves = new Signal();

		// Token: 0x0400535D RID: 21341
		public const float INITIAL_HINT_COUNTDOWN = 7f;

		// Token: 0x0400535E RID: 21342
		public const float FOLLOW_UP_HINT_COUNTDOWN = 2f;

		// Token: 0x0400535F RID: 21343
		public ObjectPool objectPool;

		// Token: 0x04005360 RID: 21344
		public int currentLevel;

		// Token: 0x04005361 RID: 21345
		public int currentTier;

		// Token: 0x04005362 RID: 21346
		public Camera cam;

		// Token: 0x04005363 RID: 21347
		[AutoSet]
		public BoardResizer resizer;

		// Token: 0x04005364 RID: 21348
		public M3_BannersRoot banners;

		// Token: 0x04005365 RID: 21349
		private bool isAnimating;

		// Token: 0x04005366 RID: 21350
		private bool isOutOfSync;

		// Token: 0x04005367 RID: 21351
		private bool isInLastHurray;

		// Token: 0x04005368 RID: 21352
		private GemViewFactory gemViewFactory;

		// Token: 0x04005369 RID: 21353
		private FieldViewFactory fieldViewFactory;

		// Token: 0x0400536A RID: 21354
		private TournamentItemCollectedViewFactory tournamentItemCollectedViewFactory;

		// Token: 0x0400536B RID: 21355
		private BoardAnimationController animationController;

		// Token: 0x0400536C RID: 21356
		private BoardBorderFactory boardBorderFactory;

		// Token: 0x0400536D RID: 21357
		private BoardDecorationsFactory boardDecorationsFactory;

		// Token: 0x0400536E RID: 21358
		private GemColorToColor gemColorToColor;

		// Token: 0x0400536F RID: 21359
		private Fields fields;

		// Token: 0x04005370 RID: 21360
		private List<GemView> gemViews;

		// Token: 0x04005371 RID: 21361
		private Map<FieldView> fieldViewMap;

		// Token: 0x04005372 RID: 21362
		private Dictionary<int, HiddenItemView> hiddenItemViews;

		// Token: 0x04005373 RID: 21363
		private Dictionary<IntVector2, ColorWheelView> colorWheelViews;

		// Token: 0x04005374 RID: 21364
		private List<DirtView> dirtViews = new List<DirtView>();

		// Token: 0x04005375 RID: 21365
		private HashSet<IntVector2> dirtViewPositions = new HashSet<IntVector2>();

		// Token: 0x04005376 RID: 21366
		private ClimberHighlighter climberHighlighter = new ClimberHighlighter();

		// Token: 0x04005377 RID: 21367
		private MatchCandidate highlightCandidate;

		// Token: 0x04005378 RID: 21368
		private float hintCountdown;

		// Token: 0x04005379 RID: 21369
		private Move expectedMove;

		// Token: 0x0400537A RID: 21370
		private BoostOverlayController boostOverlayController;
	}
}
