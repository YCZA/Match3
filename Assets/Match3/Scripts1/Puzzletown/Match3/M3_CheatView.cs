namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200070E RID: 1806
	public class M3_CheatView : EnumView<EnumWithDebugSettings>
	{
		// Token: 0x06002CCB RID: 11467 RVA: 0x000CFA70 File Offset: 0x000CDE70
		public override void Show(EnumWithDebugSettings cheat)
		{
			base.Show(cheat);
			M3Cheats value = cheat.value;
			if (value == M3Cheats.FreeSwapping)
			{
				string arg = EnumView<EnumWithDebugSettings>.ConvertToHumanReadable(cheat.ToString());
				bool @bool = cheat.data.GetBool(cheat.ToString());
				this.label.text = string.Format("{0}: {1}", arg, @bool);
			}
		}
	}
}
