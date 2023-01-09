namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200062A RID: 1578
	public class PTBoardFactory : APTBoardFactory
	{
		// Token: 0x06002827 RID: 10279 RVA: 0x000B281F File Offset: 0x000B0C1F
		public PTBoardFactory()
		{
		}

		// Token: 0x06002828 RID: 10280 RVA: 0x000B2827 File Offset: 0x000B0C27
		public PTBoardFactory(LevelConfig config) : base(config)
		{
		}

		// Token: 0x06002829 RID: 10281 RVA: 0x000B2830 File Offset: 0x000B0C30
		public override void Setup(LevelConfig config)
		{
			this.gemFactory = new GemFactory(config);
			base.Setup(config);
		}

		// Token: 0x0600282A RID: 10282 RVA: 0x000B2848 File Offset: 0x000B0C48
		public override void CreateBoard()
		{
			base.CreateBoard();
			GroupRemover groupRemover = new GroupRemover();
			groupRemover.RemoveGroups(this.fields, this.gemFactory);
		}

		// Token: 0x0600282B RID: 10283 RVA: 0x000B2874 File Offset: 0x000B0C74
		protected override void SetupGem(Field field)
		{
			base.SetupGem(field);
			if (field.gem.color == GemColor.Random)
			{
				this.gemFactory.CreateGem(field, this.fields, field.gem, false);
			}
			int parameter;
			if (Match3ConfigAmounts.modifierValueMap.TryGetValue(field.gem.modifier, out parameter))	// 该判断是针对cannon的，也就是PreCharged
			{
				field.gem.parameter = parameter;
				field.gem.modifier = GemModifier.Undefined; // 为什么要设置为未定义, 对于cannon,cannon的类型设置到上一行的parameter中?
			}
		}
	}
}
