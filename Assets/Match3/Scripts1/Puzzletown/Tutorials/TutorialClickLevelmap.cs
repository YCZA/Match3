using System.Collections;
using Match3.Scripts1.Puzzletown.Match3;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A75 RID: 2677
	[CreateAssetMenu(fileName = "ScriptTutorialClickLevelmap", menuName = "Puzzletown/Tutorials/Create/TutorialClickLevelmap")]
	public class TutorialClickLevelmap : ATutorialScript
	{
		// Token: 0x0600400D RID: 16397 RVA: 0x00148AD0 File Offset: 0x00146ED0
		protected override IEnumerator ExecuteRoutine()
		{
			yield return null;
			M3_LevelSelectionUiRoot levelSelectionRoot = global::UnityEngine.Object.FindObjectOfType<M3_LevelSelectionUiRoot>();
			if (levelSelectionRoot != null)
			{
				levelSelectionRoot.Handle(new Level(this.level, this.tier));
			}
			yield break;
		}

		// Token: 0x040069C3 RID: 27075
		public int tier;

		// Token: 0x040069C4 RID: 27076
		public int level;
	}
}
