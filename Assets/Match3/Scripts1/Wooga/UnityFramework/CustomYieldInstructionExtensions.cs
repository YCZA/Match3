using Match3.Scripts1.Puzzletown;
using UnityEngine;

namespace Wooga.UnityFramework
{
	// Token: 0x02000839 RID: 2105
	public static class CustomYieldInstructionExtensions
	{
		// Token: 0x06003457 RID: 13399 RVA: 0x000FA4C1 File Offset: 0x000F88C1
		public static void ShowLoadingScreen(this CustomYieldInstruction inst)
		{
			LoadingScreenRoot.Await(inst);
		}
	}
}
