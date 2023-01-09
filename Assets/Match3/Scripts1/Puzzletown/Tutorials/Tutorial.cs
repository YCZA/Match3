using System;
using System.Collections;
using System.Collections.Generic;
using Wooga.Coroutines;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A98 RID: 2712
	[CreateAssetMenu(fileName = "Tutorial", menuName = "Puzzletown/Tutorials/Tutorial")]
	public class Tutorial : ScriptableObject
	{
		// Token: 0x06004090 RID: 16528 RVA: 0x0014E7B0 File Offset: 0x0014CBB0
		public Coroutine Run(TutorialOverlayRoot overlay, TutorialRunner runner)
		{
			return WooroutineRunner.StartCoroutine(this.RunRoutine(overlay, runner), null);
		}

		// Token: 0x06004091 RID: 16529 RVA: 0x0014E7C0 File Offset: 0x0014CBC0
		public void Finish()
		{
			this.isFinished = true;
		}

		// Token: 0x06004092 RID: 16530 RVA: 0x0014E7CC File Offset: 0x0014CBCC
		private IEnumerator RunRoutine(TutorialOverlayRoot overlay, TutorialRunner runner)
		{
			this.index = 0;
			while (this.index < this.steps.Count && !this.isFinished)
			{
				Debug.Log("run routine index: " + index);
				yield return this.steps[this.index].Run(overlay, string.Format("tutorial.{0}.{1}", base.name, this.index), runner.onMarkerEnabled);
				this.index++;
			}
			// 引导结束，启用eventSystem
			EventSystemRoot.isUsedByTutorial = true;
		}

		// Token: 0x04006A31 RID: 27185
		public Tutorial.Trigger trigger;

		// Token: 0x04006A32 RID: 27186
		public int level;

		// Token: 0x04006A33 RID: 27187
		public string storyId;

		// Token: 0x04006A34 RID: 27188
		public string markerId;

		// Token: 0x04006A35 RID: 27189
		public bool forceReturnToIsland;

		// Token: 0x04006A36 RID: 27190
		[NonSerialized]
		private bool isFinished;

		// Token: 0x04006A37 RID: 27191
		[NonSerialized]
		public bool dontPersistCompletion;

		// Token: 0x04006A38 RID: 27192
		public List<TutorialStep> steps = new List<TutorialStep>();

		// Token: 0x04006A39 RID: 27193
		public int index;

		// Token: 0x02000A99 RID: 2713
		public enum Trigger
		{
			// Token: 0x04006A3B RID: 27195
			Level,
			// Token: 0x04006A3C RID: 27196
			Story,
			// Token: 0x04006A3D RID: 27197
			Marker
		}

		// Token: 0x02000A9A RID: 2714
		public enum RequiredAction
		{
			// Token: 0x04006A3F RID: 27199
			Tap,
			// Token: 0x04006A40 RID: 27200
			Swap,
			// Token: 0x04006A41 RID: 27201
			Click
		}
	}
}
