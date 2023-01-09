using Match3.Scripts1.Puzzletown.Services;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200064C RID: 1612
	public class AnimatorBase : ScriptableObject
	{
		// Token: 0x040052A1 RID: 21153
		[HideInInspector]
		public BoardView boardView;

		// Token: 0x040052A2 RID: 21154
		[HideInInspector]
		public AudioService audioService;

		// Token: 0x040052A3 RID: 21155
		[HideInInspector]
		public BoardAnimationController animController;

		// Token: 0x040052A4 RID: 21156
		[HideInInspector]
		public DropAnimator dropAnimator;
	}
}
