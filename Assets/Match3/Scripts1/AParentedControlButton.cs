using Match3.Scripts1.UnityEngine;

// Token: 0x0200098A RID: 2442
namespace Match3.Scripts1
{
	public abstract class AParentedControlButton<T> : ABaseControlButton<T>
	{
		// Token: 0x06003B79 RID: 15225 RVA: 0x000D0C08 File Offset: 0x000CF008
		protected override void HandleOnClick()
		{
			this.HandleOnParent(this.operation);
			this.HandleOnParent(this.operation, base.button);
		}
	}
}