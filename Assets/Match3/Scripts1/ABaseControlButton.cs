using Match3.Scripts1.Wooga.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000988 RID: 2440
namespace Match3.Scripts1
{
	[RequireComponent(typeof(Button))]
	public abstract class ABaseControlButton<T> : MonoBehaviour, IEditorDescription, ICategorised<T>
	{
		protected Button button
		{
			get
			{
				if (!this._button)
				{
					this._button = base.GetComponent<Button>();
				}
				return this._button;
			}
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x000D0BA7 File Offset: 0x000CEFA7
		private void OnEnable()
		{
			this.button.onClick.AddListener(new UnityAction(this.HandleOnClick));
		}

		// Token: 0x06003B72 RID: 15218 RVA: 0x000D0BC6 File Offset: 0x000CEFC6
		private void OnDisable()
		{
			this.button.onClick.RemoveListener(new UnityAction(this.HandleOnClick));
		}

		// Token: 0x06003B73 RID: 15219
		protected abstract void HandleOnClick();

		// Token: 0x06003B74 RID: 15220 RVA: 0x000D0BE5 File Offset: 0x000CEFE5
		public T GetCategory()
		{
			return this.operation;
		}

		// Token: 0x06003B75 RID: 15221 RVA: 0x000D0BED File Offset: 0x000CEFED
		public string GetEditorDescription()
		{
			return this.operation.ToString();
		}

		// Token: 0x04006380 RID: 25472
		public T operation;

		// Token: 0x04006381 RID: 25473
		private Button _button;
	}
}
