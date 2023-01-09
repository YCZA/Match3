using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000627 RID: 1575
	public class WaterProcessor : AMatchProcessor
	{
		// Token: 0x06002812 RID: 10258 RVA: 0x000B2000 File Offset: 0x000B0400
		protected override void ProcessMatch(IMatchGroup match, Fields fields)
		{
			bool flag;
			if (match is IWaterSpreadingResult)
			{
				flag = ((IWaterSpreadingResult)match).SpreadWater;
			}
			else if (match is IExplosionResult)
			{
				flag = this.IsExplosionCenterWatered((IExplosionResult)match, fields);
				if (!flag && match is ISwapResult)
				{
					flag = this.IsOnePositionOfSwapWatered((ISwapResult)match, fields);
				}
			}
			else
			{
				flag = WaterProcessor.DoesMatchContainWater(match, fields);
			}
			if (flag)
			{
				foreach (Gem gem in match.Group)
				{
					this.CheckAndWaterField(fields[gem.position]);
				}
				if (match is IMatchWithAffectedFields)
				{
					List<IntVector2> fields2 = ((IMatchWithAffectedFields)match).Fields;
					foreach (IntVector2 vec in fields2)
					{
						this.CheckAndWaterField(fields[vec]);
					}
				}
			}
			else if (match is ILinegemRotatingExplosion)
			{
				this.CheckLinesOfExplosion((ILinegemRotatingExplosion)match, match.Group, fields);
			}
		}

		// Token: 0x06002813 RID: 10259 RVA: 0x000B215C File Offset: 0x000B055C
		public static bool DoesMatchContainWater(IMatchGroup match, Fields fields)
		{
			foreach (Gem gem in match.Group)
			{
				if (fields[gem.position].IsWatered)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002814 RID: 10260 RVA: 0x000B21D4 File Offset: 0x000B05D4
		protected override void CheckField(Field field, IntVector2 createdFrom)
		{
		}

		// Token: 0x06002815 RID: 10261 RVA: 0x000B21D6 File Offset: 0x000B05D6
		private bool IsExplosionCenterWatered(IExplosionResult explosion, Fields fields)
		{
			return fields[explosion.Center].IsWatered;
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x000B21E9 File Offset: 0x000B05E9
		private bool IsOnePositionOfSwapWatered(ISwapResult swap, Fields fields)
		{
			return fields[swap.Center].IsWatered || fields[swap.From].IsWatered;
		}

		// Token: 0x06002817 RID: 10263 RVA: 0x000B2218 File Offset: 0x000B0618
		private void CheckAndWaterField(Field field)
		{
			if (!field.removedModifier && field.CanBeWatered && !field.gem.IsCovered)
			{
				field.WaterField();
				this.results.Add(new WaterExplosion(field));
			}
		}

		// Token: 0x06002818 RID: 10264 RVA: 0x000B2268 File Offset: 0x000B0668
		private void CheckLinesOfExplosion(ILinegemRotatingExplosion explosion, Group group, Fields fields)
		{
			IntVector2 center = explosion.Center;
			bool flag = explosion is LinegemBombExplosion;
			bool flag2 = flag || explosion is StarExplosion || explosion is HammerStarExplosion || explosion is CannonExplosion;
			bool flag3 = flag2 || (explosion is LineGemExplosion && ((LineGemExplosion)explosion).isHorizontal);
			bool flag4 = flag2 || (explosion is LineGemExplosion && !((LineGemExplosion)explosion).isHorizontal);
			List<IntVector2> positions = new List<IntVector2>();
			if (explosion is IMatchWithAffectedFields)
			{
				positions = ((IMatchWithAffectedFields)explosion).Fields;
			}
			if (flag3)
			{
				this.CheckForWaterInLine(center + IntVector2.Left, IntVector2.Left, group, positions, fields);
				this.CheckForWaterInLine(center + IntVector2.Right, IntVector2.Right, group, positions, fields);
			}
			if (flag4)
			{
				this.CheckForWaterInLine(center + IntVector2.Up, IntVector2.Up, group, positions, fields);
				this.CheckForWaterInLine(center + IntVector2.Down, IntVector2.Down, group, positions, fields);
			}
			if (flag)
			{
				this.CheckForWaterInLine(center + IntVector2.Up, IntVector2.Left, group, positions, fields);
				this.CheckForWaterInLine(center + IntVector2.Down, IntVector2.Left, group, positions, fields);
				this.CheckForWaterInLine(center + IntVector2.Up, IntVector2.Right, group, positions, fields);
				this.CheckForWaterInLine(center + IntVector2.Down, IntVector2.Right, group, positions, fields);
				this.CheckForWaterInLine(center + IntVector2.Left, IntVector2.Up, group, positions, fields);
				this.CheckForWaterInLine(center + IntVector2.Right, IntVector2.Up, group, positions, fields);
				this.CheckForWaterInLine(center + IntVector2.Left, IntVector2.Down, group, positions, fields);
				this.CheckForWaterInLine(center + IntVector2.Right, IntVector2.Down, group, positions, fields);
			}
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x000B246C File Offset: 0x000B086C
		private void CheckForWaterInLine(IntVector2 center, IntVector2 dir, Group group, List<IntVector2> positions, Fields fields)
		{
			bool flag = false;
			int num = center.x;
			while (num < 9 && num > -1)
			{
				int num2 = center.y;
				while (num2 < 9 && num2 > -1)
				{
					IntVector2 intVector = new IntVector2(num, num2);
					if (group.Contains(intVector) || positions.Contains(intVector))
					{
						if (flag)
						{
							this.CheckAndWaterField(fields[intVector]);
						}
						else
						{
							flag = (fields[num, num2].IsWatered && !fields[num, num2].removedModifier);
						}
					}
					if (dir.y == 0)
					{
						break;
					}
					num2 += dir.y;
				}
				if (dir.x == 0)
				{
					break;
				}
				num += dir.x;
			}
		}
	}
}
