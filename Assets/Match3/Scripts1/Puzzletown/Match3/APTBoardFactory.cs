using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000629 RID: 1577
	public abstract class APTBoardFactory : IBoardFactory
	{
		// Token: 0x0600281E RID: 10270 RVA: 0x000B2580 File Offset: 0x000B0980
		public APTBoardFactory()
		{
		}

		// Token: 0x0600281F RID: 10271 RVA: 0x000B2588 File Offset: 0x000B0988
		public APTBoardFactory(LevelConfig config)
		{
			this.Setup(config);
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06002820 RID: 10272 RVA: 0x000B2597 File Offset: 0x000B0997
		public Fields Fields
		{
			get
			{
				if (this.fields == null)
				{
					this.CreateBoard();
				}
				return this.fields;
			}
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06002821 RID: 10273 RVA: 0x000B25B0 File Offset: 0x000B09B0
		public GemFactory GemFactory
		{
			get
			{
				return this.gemFactory;
			}
		}

		// Token: 0x06002822 RID: 10274 RVA: 0x000B25B8 File Offset: 0x000B09B8
		public virtual void Setup(LevelConfig config)
		{
			this.config = config;
			this.fieldFactory = new PTFieldFactory();
		}

		// Token: 0x06002823 RID: 10275 RVA: 0x000B25CC File Offset: 0x000B09CC
		public virtual void CreateBoard()
		{
			int fieldSize = this.config.layout.FieldSize;
			if (this.fields == null)
			{
				this.fields = new Fields(fieldSize);
			}
			for (int i = 0; i < fieldSize; i++)
			{
				for (int j = 0; j < fieldSize; j++)
				{
					Field field = this.fieldFactory.CreateField(this.config.layout, i, j);
					if (field.isOn && !field.IsBlocked)
					{
						this.SetupGem(field);
					}
					this.fields[field.gridPosition] = field;
				}
			}
		}

		// Token: 0x06002824 RID: 10276 RVA: 0x000B2670 File Offset: 0x000B0A70
		protected virtual void SetupGem(Field field)
		{
			Gem gemFromConfig = this.GetGemFromConfig(field.gridPosition, true);
			field.AssignGem(gemFromConfig);
		}

		// Token: 0x06002825 RID: 10277 RVA: 0x000B2694 File Offset: 0x000B0A94
		public Gem GetGemFromConfig(IntVector2 gridPosition, bool collectPrePositions = false)
		{
			Gem result = default(Gem);
			if (!this.config.layout.gemColors.IsNullOrEmptyCollection())
			{
				int index = LevelLayout.GetIndex(gridPosition, this.config.layout.FieldSize);
				GemColor gemColor = (GemColor)this.config.layout.gemColors[index];
				result = new Gem(gemColor);
				if (collectPrePositions && gemColor != GemColor.Undefined && gemColor != GemColor.Random)
				{
					this.fields.prePositionedGems.Add(gridPosition);
				}
				if (!this.config.layout.gemTypes.IsNullOrEmptyCollection())
				{
					result.type = (GemType)this.config.layout.gemTypes[index];
				}
				if (!this.config.layout.gemModifier.IsNullOrEmptyCollection())
				{
					result.modifier = (GemModifier)this.config.layout.gemModifier[index];
				}
				if (!this.config.layout.gemDirection.IsNullOrEmptyCollection())
				{
					result.direction = (GemDirection)this.config.layout.gemDirection[index];
				}
			}
			return result;
		}

		// Token: 0x06002826 RID: 10278 RVA: 0x000B27B8 File Offset: 0x000B0BB8
		public void LoadBoard(Fields fields)
		{
			if (this.fields == null)
			{
				this.fields = fields;
			}
			else
			{
				for (int i = 0; i < fields.size; i++)
				{
					for (int j = 0; j < fields.size; j++)
					{
						this.fields[i, j] = fields[i, j];
					}
				}
			}
		}

		// Token: 0x0400523E RID: 21054
		protected Fields fields;

		// Token: 0x0400523F RID: 21055
		protected GemFactory gemFactory;

		// Token: 0x04005240 RID: 21056
		private LevelConfig config;

		// Token: 0x04005241 RID: 21057
		private PTFieldFactory fieldFactory;
	}
}
