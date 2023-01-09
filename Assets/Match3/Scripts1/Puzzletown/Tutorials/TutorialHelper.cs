using Match3.Scripts1.Puzzletown.Match3;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000AA3 RID: 2723
	public static class TutorialHelper
	{
		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x060040BC RID: 16572 RVA: 0x0015035B File Offset: 0x0014E75B
		public static bool IsMatch3
		{
			get
			{
				return SceneManager.Instance.Has(typeof(M3_LevelRoot), false);
			}
		}
	}
}
