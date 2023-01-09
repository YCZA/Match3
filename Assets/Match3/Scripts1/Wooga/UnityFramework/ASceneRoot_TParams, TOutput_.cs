using Match3.Scripts1.Wooga.Signals;

namespace Wooga.UnityFramework
{
	// Token: 0x02000B5D RID: 2909
	public abstract class ASceneRoot<TParams, TOutput> : ASceneRoot<TParams>
	{
		// Token: 0x06004401 RID: 17409 RVA: 0x00097CB4 File Offset: 0x000960B4
		public override void Setup(TParams parameters)
		{
			base.Setup(parameters);
			this.onCompleted.Clear();
		}

		// onCompleted好像什么也没执行，只是改变了一下状态
		public readonly AwaitSignal<TOutput> onCompleted = new AwaitSignal<TOutput>();
	}
}
