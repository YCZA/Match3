using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006C7 RID: 1735
	public class LevelTheme : ScriptableObject
	{
		// Token: 0x04005451 RID: 21585
		public const string THEME_PATH = "Assets/Puzzletown/Match3/Art/Themes/Configs/{0}.asset";

		// Token: 0x04005452 RID: 21586
		public const string DEFAULT_THEME = "climber";

		// Token: 0x04005453 RID: 21587
		public const string FIRST_THEMES_BUNDLE_NAME = "m3_themes";

		// Token: 0x04005454 RID: 21588
		public const string THEMES_BUNDLE_NAME = "m3_themes_{0}";

		// Token: 0x04005455 RID: 21589
		public static readonly string[] THEMES_FIRST_BUNDLE = new string[]
		{
			"droppable",
			"hiddenitem",
			"recipe",
			"treasure",
			"water"
		};

		// Token: 0x04005456 RID: 21590
		public Texture2D board;

		// Token: 0x04005457 RID: 21591
		public Texture2D innerBorder;

		// Token: 0x04005458 RID: 21592
		public Texture2D outerBorder;

		// Token: 0x04005459 RID: 21593
		public Sprite background;
	}
}
