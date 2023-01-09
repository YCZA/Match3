using System;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000584 RID: 1412
	[Serializable]
	public class Field : AField, ICloneable
	{
		// Token: 0x060024EE RID: 9454 RVA: 0x000A51C0 File Offset: 0x000A35C0
		public Field()
		{
		}

		// Token: 0x060024EF RID: 9455 RVA: 0x000A51C8 File Offset: 0x000A35C8
		public Field(int x, int y) : this()
		{
			this.gridPosition = new IntVector2(x, y);
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x060024F0 RID: 9456 RVA: 0x000A51DD File Offset: 0x000A35DD
		public bool IsSpawner
		{
			get
			{
				return this.spawnType == SpawnTypes.NormalSpawn;
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x060024F1 RID: 9457 RVA: 0x000A51E8 File Offset: 0x000A35E8
		public bool IsDefinedGemSpawner
		{
			get
			{
				return this.spawnType == SpawnTypes.DefinedGem;
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x060024F2 RID: 9458 RVA: 0x000A51F3 File Offset: 0x000A35F3
		public bool HasCrates
		{
			get
			{
				return Crate.GetHp(this.cratesIndex) > 0;
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x060024F3 RID: 9459 RVA: 0x000A5203 File Offset: 0x000A3603
		public bool CanExplode
		{
			get
			{
				return !this.isExploded && !this.willExplode;
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x060024F4 RID: 9460 RVA: 0x000A521C File Offset: 0x000A361C
		public override bool IsBlocked
		{
			get
			{
				return this.blockerIndex > 0;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x060024F5 RID: 9461 RVA: 0x000A5227 File Offset: 0x000A3627
		public override bool GemBlocked
		{
			get
			{
				return this.HasCrates;
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x060024F6 RID: 9462 RVA: 0x000A522F File Offset: 0x000A362F
		public override bool CanMove
		{
			get
			{
				return this.numChains == 0 && !this.IsBlocked && !this.GemBlocked && !this.gem.IsCannon && !this.IsDefinedGemSpawner;
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x060024F7 RID: 9463 RVA: 0x000A526E File Offset: 0x000A366E
		public override bool CanBeCharged
		{
			get
			{
				return this.gem.IsCannon && this.numChains == 0 && !this.GemBlocked && this.gem.parameter < 45;
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x060024F8 RID: 9464 RVA: 0x000A52A8 File Offset: 0x000A36A8
		public override bool CanSwap
		{
			get
			{
				return this.CanMove && base.HasGem && !this.gem.IsCovered;
			}
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x060024F9 RID: 9465 RVA: 0x000A52D4 File Offset: 0x000A36D4
		public override bool NeedsGem
		{
			get
			{
				return this.isOn && !base.HasGem && !this.IsBlocked && !this.isWindow && !this.isGrowingWindow && !this.GemBlocked && !this.IsDefinedGemSpawner;
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x060024FA RID: 9466 RVA: 0x000A5330 File Offset: 0x000A3730
		public bool CanTrickle
		{
			get
			{
				return this.isOn && !this.isWindow && !this.isGrowingWindow && !this.GemBlocked && this.gem.CanTrickle && !this.IsDefinedGemSpawner && !this.IsBlocked;
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x060024FB RID: 9467 RVA: 0x000A5390 File Offset: 0x000A3790
		public bool CanSpawnInTrickling
		{
			get
			{
				return this.IsSpawner || this.CanSpawnDropItems || this.CanSpawnChameleons;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x060024FC RID: 9468 RVA: 0x000A53B1 File Offset: 0x000A37B1
		public bool CanSpawnDropItems
		{
			get
			{
				return this.isDropSpawner;
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x060024FD RID: 9469 RVA: 0x000A53B9 File Offset: 0x000A37B9
		public bool CanSpawnClimber
		{
			get
			{
				return this.isClimberSpawner;
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x060024FE RID: 9470 RVA: 0x000A53C1 File Offset: 0x000A37C1
		public bool CanSpawnChameleons
		{
			get
			{
				return this.spawnType == SpawnTypes.ChameleonSpawn;
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x060024FF RID: 9471 RVA: 0x000A53CC File Offset: 0x000A37CC
		public bool IsExitDropItems
		{
			get
			{
				return this.isDropExit;
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06002500 RID: 9472 RVA: 0x000A53D4 File Offset: 0x000A37D4
		public bool IsExitClimber
		{
			get
			{
				return this.isClimberExit;
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06002501 RID: 9473 RVA: 0x000A53DC File Offset: 0x000A37DC
		public bool CanBeWatered
		{
			get
			{
				return this.numTiles == 0 && this.isOn && !this.isWindow;
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06002502 RID: 9474 RVA: 0x000A5400 File Offset: 0x000A3800
		public bool IsWatered
		{
			get
			{
				return this.numTiles == 3;
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06002503 RID: 9475 RVA: 0x000A540B File Offset: 0x000A380B
		public bool IsTile
		{
			get
			{
				return this.numTiles > 0 && this.numTiles <= 2;
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06002504 RID: 9476 RVA: 0x000A5428 File Offset: 0x000A3828
		public bool BlocksClimber
		{
			get
			{
				return this.gem.IsCannon || this.IsDefinedGemSpawner || this.IsColorWheel;
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06002505 RID: 9477 RVA: 0x000A544E File Offset: 0x000A384E
		public bool IsStone
		{
			get
			{
				return Stone.IsStone(this.blockerIndex);
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06002506 RID: 9478 RVA: 0x000A545B File Offset: 0x000A385B
		public bool IsResistantBlocker
		{
			get
			{
				return ResistantBlocker.IsResistantBlocker(this.blockerIndex);
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06002507 RID: 9479 RVA: 0x000A5468 File Offset: 0x000A3868
		public bool IsColorWheel
		{
			get
			{
				return ColorWheel.IsColorWheel(this.blockerIndex);
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06002508 RID: 9480 RVA: 0x000A5475 File Offset: 0x000A3875
		public bool IsColorWheelCorner
		{
			get
			{
				return ColorWheel.IsColorWheelCorner(this.blockerIndex);
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06002509 RID: 9481 RVA: 0x000A5482 File Offset: 0x000A3882
		public bool IsColorWheelVariant
		{
			get
			{
				return ColorWheel.IsColorWheelVariant(this.blockerIndex);
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x0600250A RID: 9482 RVA: 0x000A5490 File Offset: 0x000A3890
		public bool CanPlaceSpawner
		{
			get
			{
				return this.isOn && !this.IsSpawner && !this.IsDefinedGemSpawner && !this.isWindow && !this.isGrowingWindow && !this.IsColorWheel && !this.IsResistantBlocker && !this.CanSpawnClimber;
			}
		}

		// Token: 0x0600250B RID: 9483 RVA: 0x000A54F6 File Offset: 0x000A38F6
		public void SetCrates(GemColor color, int hp)
		{
			this.cratesIndex = Crate.GetIndex(color, hp);
		}

		// Token: 0x0600250C RID: 9484 RVA: 0x000A5505 File Offset: 0x000A3905
		public void WaterField()
		{
			this.numTiles = 3;
		}

		// Token: 0x0600250D RID: 9485 RVA: 0x000A550E File Offset: 0x000A390E
		public bool CanExplosionHitGem()
		{
			return this.CanExplode && this.numChains == 0 && !this.GemBlocked;
		}

		// Token: 0x0600250E RID: 9486 RVA: 0x000A5532 File Offset: 0x000A3932
		public bool BehavesLikeWindow()
		{
			return this.isWindow || this.gem.IsClimber || this.isGrowingWindow;
		}

		// Token: 0x0600250F RID: 9487 RVA: 0x000A5558 File Offset: 0x000A3958
		public override string ToString()
		{
			string text = string.Empty;
			if (this.isWindow)
			{
				text += string.Format("[{0}: {1}]", "fields", 2);
			}
			if (this.spawnType > SpawnTypes.None)
			{
				text += string.Format("[{0}: {1}]", "spawning", this.spawnType);
			}
			if (this.isDropSpawner || this.isDropExit)
			{
				int num = (!this.isDropSpawner) ? 2 : 1;
				text += string.Format("[{0}: {1}]", "dropItems", num);
			}
			if (this.numChains > 0)
			{
				text += string.Format("[{0}: {1}]", "chains", this.numChains);
			}
			if (this.cratesIndex > 0)
			{
				text += string.Format("[{0}: {1}]", "crates", this.cratesIndex);
			}
			if (Stone.IsStone(this.blockerIndex))
			{
				text += string.Format("[{0}: {1}]", "stones", this.blockerIndex);
			}
			if (ResistantBlocker.IsResistantBlocker(this.blockerIndex))
			{
				text += string.Format("[{0}: {1}]", "resistantBlocker", ResistantBlocker.GetHp(this.blockerIndex));
			}
			if (this.numTiles > 0 && this.numTiles < 3)
			{
				text += string.Format("[{0}: {1}]", "tiles", this.numTiles);
			}
			if (this.numTiles == 3)
			{
				text += string.Format("[{0}: {1}]", "water", 1);
			}
			if (this.hiddenItemId > 0)
			{
				text += string.Format("[{0}: {1}]", "hiddenItems", this.hiddenItemId);
			}
			if (this.portalId > 0)
			{
				text += string.Format("[{0}: {1}]", "portals", this.portalId);
			}
			if (ColorWheel.IsColorWheel(this.blockerIndex))
			{
				text += string.Format("[{0}: {1}: {2}]", "colorWheel", (!ColorWheel.IsColorWheelCorner(this.blockerIndex)) ? "noCorner" : "corner", ColorWheel.IsColorWheelVariant(this.blockerIndex));
			}
			if (this.isGrowingWindow)
			{
				text += string.Format("[{0}: {1}]", "fields", 3);
			}
			return string.Format("[Field: IsBlocked={0}, CanMove={1}, CanSwap={2}, Modifiers={3}]", new object[]
			{
				this.IsBlocked,
				this.CanMove,
				this.CanSwap,
				text
			});
		}

		// Token: 0x06002510 RID: 9488 RVA: 0x000A583C File Offset: 0x000A3C3C
		public object Clone()
		{
			Field field = new Field(this.gridPosition.x, this.gridPosition.y);
			field.isOn = this.isOn;
			if (base.HasGem)
			{
				field.AssignGem(this.gem);
			}
			field.isWindow = this.isWindow;
			field.isGrowingWindow = this.isGrowingWindow;
			field.spawnType = this.spawnType;
			field.isDropSpawner = this.isDropSpawner;
			field.isDropExit = this.isDropExit;
			field.portalId = this.portalId;
			field.numChains = this.numChains;
			field.blockerIndex = this.blockerIndex;
			field.cratesIndex = this.cratesIndex;
			field.numTiles = this.numTiles;
			field.hiddenItemId = this.hiddenItemId;
			return field;
		}

		// Token: 0x06002511 RID: 9489 RVA: 0x000A590D File Offset: 0x000A3D0D
		public void ResetPortal()
		{
			if (base.HasGem)
			{
				this.gem.lastPortalUsed = -1;
			}
		}

		// Token: 0x06002512 RID: 9490 RVA: 0x000A5926 File Offset: 0x000A3D26
		public void ResetProcessedGem()
		{
			if (base.HasGem)
			{
				this.gem.processed = false;
			}
		}

		// Token: 0x06002513 RID: 9491 RVA: 0x000A593F File Offset: 0x000A3D3F
		public void ResetExplosion()
		{
			this.isExploded = false;
			this.willExplode = false;
			this.removedModifier = false;
		}

		// Token: 0x0400506F RID: 20591
		public const int TILE_AMOUNT_FOR_WATERED = 3;

		// Token: 0x04005070 RID: 20592
		[NonSerialized]
		public bool isWindow;

		// Token: 0x04005071 RID: 20593
		[NonSerialized]
		public bool isGrowingWindow;

		// Token: 0x04005072 RID: 20594
		[NonSerialized]
		public bool isDropSpawner;

		// Token: 0x04005073 RID: 20595
		[NonSerialized]
		public bool isDropExit;

		// Token: 0x04005074 RID: 20596
		[NonSerialized]
		public int portalId;

		// Token: 0x04005075 RID: 20597
		[NonSerialized]
		public int numChains;

		// Token: 0x04005076 RID: 20598
		[NonSerialized]
		public int blockerIndex;

		// Token: 0x04005077 RID: 20599
		[NonSerialized]
		public int cratesIndex;

		// Token: 0x04005078 RID: 20600
		[NonSerialized]
		public int numTiles;

		// Token: 0x04005079 RID: 20601
		[NonSerialized]
		public int hiddenItemId;

		// Token: 0x0400507A RID: 20602
		[NonSerialized]
		public bool isClimberSpawner;

		// Token: 0x0400507B RID: 20603
		[NonSerialized]
		public bool isClimberExit;

		// Token: 0x0400507C RID: 20604
		[NonSerialized]
		public SpawnTypes spawnType;

		// Token: 0x0400507D RID: 20605
		public bool isExploded;

		// Token: 0x0400507E RID: 20606
		public bool removedModifier;

		// Token: 0x0400507F RID: 20607
		public bool willExplode;
	}
}
