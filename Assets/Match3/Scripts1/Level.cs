using System;
using System.Text.RegularExpressions;
using Match3.Scripts1.Puzzletown.Match3;

namespace Match3.Scripts1
{
	// Token: 0x020007E5 RID: 2021
	public struct Level
	{
		// Token: 0x06003207 RID: 12807 RVA: 0x000EBD16 File Offset: 0x000EA116
		public Level(int level, int tier)
		{
			this.level = level;
			this.tier = tier;
		}

		// Token: 0x06003208 RID: 12808 RVA: 0x000EBD26 File Offset: 0x000EA126
		public override string ToString()
		{
			return string.Format("{0}{1}", this.level, (char)(97 + this.tier));
		}

		// Token: 0x06003209 RID: 12809 RVA: 0x000EBD4C File Offset: 0x000EA14C
		public static Level Parse(string name)
		{
			System.Text.RegularExpressions.Match match = Level.rex.Match(name);
			int num = int.Parse(match.Groups["level"].Value);
			int num2 = (int)((AreaConfig.Tier)Enum.Parse(typeof(AreaConfig.Tier), match.Groups["tier"].Value));
			return new Level(num, num2);
		}

		// Token: 0x04005A79 RID: 23161
		private static Regex rex = new Regex("(?<level>\\d*)(?<tier>[abc])");

		// Token: 0x04005A7A RID: 23162
		public int level;

		// Token: 0x04005A7B RID: 23163
		public int tier;
	}
}
