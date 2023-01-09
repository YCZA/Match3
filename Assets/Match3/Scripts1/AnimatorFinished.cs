using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

// Token: 0x02000B47 RID: 2887
namespace Match3.Scripts1
{
	public class AnimatorFinished : StateMachineBehaviour
	{
		// Token: 0x060043B1 RID: 17329 RVA: 0x00159BDF File Offset: 0x00157FDF
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.onFinished.Dispatch();
		}

		// Token: 0x04006C12 RID: 27666
		public readonly Signal onFinished = new Signal();
	}
}
