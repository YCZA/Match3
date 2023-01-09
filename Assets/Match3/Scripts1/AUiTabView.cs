using Match3.Scripts1.UnityEngine;
using Match3.Scripts1.Wooga.UI;
using UnityEngine.EventSystems;

// Token: 0x02000A35 RID: 2613
namespace Match3.Scripts1
{
	public class AUiTabView<T> : ATableViewReusableCell, IDataView<T>, IEditorDescription, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		// Token: 0x06003EB5 RID: 16053 RVA: 0x00136432 File Offset: 0x00134832
		public virtual void Show(T param)
		{
			this._data = param;
		}

		// Token: 0x06003EB6 RID: 16054 RVA: 0x0013643B File Offset: 0x0013483B
		public void OnPointerUp(PointerEventData evt)
		{
		}

		// Token: 0x06003EB7 RID: 16055 RVA: 0x00136440 File Offset: 0x00134840
		public void OnPointerDown(PointerEventData evt)
		{
			UiTabState uiTabState = this.state;
			if (uiTabState == UiTabState.Inactive)
			{
				this.HandleOnParent(this._data);
			}
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06003EB8 RID: 16056 RVA: 0x00136471 File Offset: 0x00134871
		public override int reusableId
		{
			get
			{
				return (int)this.state;
			}
		}

		// Token: 0x06003EB9 RID: 16057 RVA: 0x00136479 File Offset: 0x00134879
		public string GetEditorDescription()
		{
			return this.state.ToString();
		}

		// Token: 0x040067D6 RID: 26582
		protected T _data;

		// Token: 0x040067D7 RID: 26583
		public UiTabState state;
	}
}
