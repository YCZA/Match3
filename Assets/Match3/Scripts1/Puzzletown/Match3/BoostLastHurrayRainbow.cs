using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005A5 RID: 1445
	public class BoostLastHurrayRainbow : ABoost
	{
		// Token: 0x060025D2 RID: 9682 RVA: 0x000A8BE7 File Offset: 0x000A6FE7
		public BoostLastHurrayRainbow(Fields fields, IntVector2 position) : base(fields, position)
		{
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x000A8BF4 File Offset: 0x000A6FF4
		public override List<IMatchResult> Apply()
		{
			List<IMatchResult> list = new List<IMatchResult>();
			GemColor targetColor = this.GetTargetColor();
			RainbowExplosion rainbowExplosion = new RainbowExplosion(this.fields, targetColor, this.position, this.position);
			foreach (Gem gem in rainbowExplosion.Group)
			{
				this.fields[gem.position].HasGem = false;
			}
			list.Add(rainbowExplosion);
			return list;
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x000A8C98 File Offset: 0x000A7098
		public override bool IsValid()
		{
			return this.fields[this.position].gem.color == GemColor.Rainbow;
		}

		// Token: 0x060025D5 RID: 9685 RVA: 0x000A8CBC File Offset: 0x000A70BC
		private GemColor GetTargetColor()
		{
			List<GemColor> list = new List<GemColor>();
			int i = 0;
			int size = this.fields.size;
			while (i < size)
			{
				for (int j = 0; j < size; j++)
				{
					Field field = this.fields[i, j];
					if (field != null && !field.GemBlocked && field.gem.IsMatchable)
					{
						list.AddIfNotAlreadyPresent(field.gem.color, false);
					}
				}
				i++;
			}
			return list[RandomHelper.Next(list.Count)];
		}
	}
}
