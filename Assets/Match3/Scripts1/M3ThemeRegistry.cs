using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B60 RID: 2912
namespace Match3.Scripts1
{
	[CreateAssetMenu(fileName = "M3ThemeRegistry", menuName = "Puzzletown/M3ThemeRegistry")]
	public class M3ThemeRegistry : ScriptableObject
	{
		// Token: 0x0600441A RID: 17434 RVA: 0x0015AC08 File Offset: 0x00159008
		public void AddEntry(string themeName)
		{
			this.themes.AddIfNotAlreadyPresent(themeName, false);
		}

		// Token: 0x0600441B RID: 17435 RVA: 0x0015AC18 File Offset: 0x00159018
		public void ClearRegistry()
		{
			this.themes.Clear();
		}

		// Token: 0x04006C63 RID: 27747
		public List<string> themes;
	}
}
