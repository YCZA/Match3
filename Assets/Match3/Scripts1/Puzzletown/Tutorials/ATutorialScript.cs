using System.Collections;
using Wooga.Coroutines;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A95 RID: 2709
	public abstract class ATutorialScript : ScriptableObject
	{
		// Token: 0x06004081 RID: 16513 RVA: 0x0014870B File Offset: 0x00146B0B
		public Coroutine Execute(TutorialOverlayRoot overlay, TutorialStep step)
		{
			this.overlay = overlay;
			this.step = step;
			return WooroutineRunner.StartCoroutine(this.ExecuteRoutine(), null);
		}

		// Token: 0x06004082 RID: 16514 RVA: 0x00148727 File Offset: 0x00146B27
		public virtual void Tick()
		{
		}

		// Token: 0x06004083 RID: 16515
		protected abstract IEnumerator ExecuteRoutine();

		// Token: 0x04006A26 RID: 27174
		protected TutorialOverlayRoot overlay;

		// Token: 0x04006A27 RID: 27175
		protected TutorialStep step;
	}
}
