using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.Flows;
using Wooga.UnityFramework;

// Token: 0x02000771 RID: 1905
namespace Match3.Scripts1
{
	public abstract class BaseFacebookFlow : AFlow
	{
		// Token: 0x06002F36 RID: 12086 RVA: 0x000DCB84 File Offset: 0x000DAF84
		protected override IEnumerator FlowRoutine()
		{
			ServiceLocator.Instance.Inject(this);
			yield return this.FBFlowRoutine();
			yield break;
		}

		// Token: 0x06002F37 RID: 12087
		protected abstract IEnumerator FBFlowRoutine();

		// Token: 0x04005850 RID: 22608
		[WaitForService(true, true)]
		protected FacebookService facebook;
	}
}
