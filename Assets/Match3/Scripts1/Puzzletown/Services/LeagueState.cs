using System.Text;
using Match3.Scripts1.Wooga.Leagues;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000811 RID: 2065
	public class LeagueState
	{
		// Token: 0x06003303 RID: 13059 RVA: 0x000F0A5C File Offset: 0x000EEE5C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Standings for league: ");
			stringBuilder.AppendLine(this.id);
			if (this.entries == null)
			{
				stringBuilder.AppendLine("< EMPTY >");
			}
			else
			{
				foreach (LeagueEntry leagueEntry in this.entries)
				{
					stringBuilder.AppendLine(leagueEntry.ToString());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04005B4D RID: 23373
		public string id;

		// Token: 0x04005B4E RID: 23374
		public LeagueEntry[] entries;
	}
}
