using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

// Token: 0x020005B9 RID: 1465
namespace Match3.Scripts1
{
	public class ColorWheelMatcher : AInstantMatcher
	{
		// Token: 0x06002634 RID: 9780 RVA: 0x000AAD59 File Offset: 0x000A9159
		public ColorWheelMatcher(Fields fields) : base(fields)
		{
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x000AAD70 File Offset: 0x000A9170
		protected override IEnumerable<IMatchResult> FindMatches(Move move, List<Group> groups)
		{
			this.activatedExplosions.Clear();
			for (int i = 0; i < this.fields.size; i++)
			{
				for (int j = 0; j < this.fields.size; j++)
				{
					Field field = this.fields[i, j];
					if (field != null && field.IsColorWheelCorner)
					{
						ColorWheelModel colorWheelModel = this.fields.colorWheelModels[field.gridPosition];
						if (colorWheelModel.colors.Count <= 0 && !field.isExploded)
						{
							ColorWheelExplosion colorWheelExplosion = new ColorWheelExplosion(this.fields, field.gridPosition);
							this.activatedExplosions.Add(colorWheelExplosion);
							field.isExploded = true;
						}
					}
				}
			}
			return this.activatedExplosions;
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x000AAE44 File Offset: 0x000A9244
		protected override IEnumerable<IMatchResult> RemoveMatches(Move move, List<Group> groups)
		{
			List<IMatchResult> list = this.FindMatches(move, groups).ToList<IMatchResult>();
			foreach (IMatchResult matchResult in list)
			{
				ColorWheelExplosion colorWheelExplosion = (ColorWheelExplosion)matchResult;
				base.HitGems(colorWheelExplosion);
				this.fields.RemoveColorWheel(colorWheelExplosion.From);
			}
			return list;
		}

		// Token: 0x0400511F RID: 20767
		private List<IMatchResult> activatedExplosions = new List<IMatchResult>();
	}
}
