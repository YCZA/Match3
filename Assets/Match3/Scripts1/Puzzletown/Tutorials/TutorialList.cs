using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000AA0 RID: 2720
	[CreateAssetMenu(fileName = "TutorialList", menuName = "Puzzletown/Tutorials/Create List")]
	public class TutorialList : ScriptableObject
	{
		// Token: 0x04006A50 RID: 27216
		public List<Tutorial> tutorials;
	}
}
