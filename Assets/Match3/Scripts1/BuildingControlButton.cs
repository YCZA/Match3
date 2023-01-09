using Match3.Scripts1.Wooga.UI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200098C RID: 2444
namespace Match3.Scripts1
{
	[RequireComponent(typeof(Button))]
	public class BuildingControlButton : AParentedControlButton<BuildingOperation>, IDataView<BuildingOperation>
	{
		// Token: 0x06003B7E RID: 15230 RVA: 0x0012783C File Offset: 0x00125C3C
		private void Start()
		{
			this.buttonRect = base.button.GetComponent<RectTransform>();
		}

		// Token: 0x06003B7F RID: 15231 RVA: 0x00127850 File Offset: 0x00125C50
		public void Show(BuildingOperation op)
		{
			bool flag = (op & this.operation) == this.operation;
			base.button.gameObject.SetActive(flag);
			if (flag && this.buttonRect != null)
			{
				this.UpdatePosition(this.buttonRect);
			}
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x001278A4 File Offset: 0x00125CA4
		private void UpdatePosition(RectTransform rect)
		{
			Vector3 position = rect.position;
			rect.position = new Vector3(position.x, position.y, 1E-05f);
			rect.position = position;
		}

		// Token: 0x04006384 RID: 25476
		public RectTransform buttonRect;
	}
}
