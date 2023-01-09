using System.Collections;
using Wooga.Coroutines;
using UnityEngine;

namespace Wooga.UnityFramework
{
	// Token: 0x0200083A RID: 2106
	public static class YieldInstructionExtensions
	{
		// Token: 0x06003458 RID: 13400 RVA: 0x000FA4C9 File Offset: 0x000F88C9
		public static void ShowLoadingScreen(this YieldInstruction inst)
		{
			inst.Yield().ShowLoadingScreen();
		}

		// Token: 0x06003459 RID: 13401 RVA: 0x000FA4D6 File Offset: 0x000F88D6
		public static CustomYieldInstruction Yield(this YieldInstruction inst)
		{
			return new YieldInstructionExtensions.YieldInstructionWrapper(inst);
		}

		// Token: 0x0200083B RID: 2107
		public class YieldInstructionWrapper : CustomYieldInstruction
		{
			// Token: 0x0600345A RID: 13402 RVA: 0x000FA4DE File Offset: 0x000F88DE
			public YieldInstructionWrapper(YieldInstruction inst)
			{
				WooroutineRunner.StartCoroutine(this.WrapRoutine(inst), null);
			}

			// Token: 0x1700083F RID: 2111
			// (get) Token: 0x0600345B RID: 13403 RVA: 0x000FA4F4 File Offset: 0x000F88F4
			public override bool keepWaiting
			{
				get
				{
					return !this.isDone;
				}
			}

			// Token: 0x0600345C RID: 13404 RVA: 0x000FA500 File Offset: 0x000F8900
			private IEnumerator WrapRoutine(YieldInstruction inst)
			{
				this.isDone = false;
				yield return inst;
				this.isDone = true;
				yield break;
			}

			// Token: 0x04005C3B RID: 23611
			private bool isDone;
		}
	}
}
