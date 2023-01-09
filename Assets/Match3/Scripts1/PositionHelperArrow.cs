using Match3.Scripts1.Wooga.UI;
using UnityEngine;

// Token: 0x0200091F RID: 2335
namespace Match3.Scripts1
{
	public class PositionHelperArrow : AVisibleGameObject, IDataView<int>, IEditorDescription
	{
		// Token: 0x060038F6 RID: 14582 RVA: 0x00118B2C File Offset: 0x00116F2C
		public void Show(int size)
		{
			float num = (float)size * 0.5f;
			float num2 = (float)size + 0.4f;
			float num3 = -0.4f;
			switch (this.direction)
			{
				case PositionHelperArrow.Direction.PositiveX:
					base.transform.localPosition = new Vector3(num2, 0f, num);
					break;
				case PositionHelperArrow.Direction.PositiveY:
					base.transform.localPosition = new Vector3(num, 0f, num2);
					break;
				case PositionHelperArrow.Direction.NegativeX:
					base.transform.localPosition = new Vector3(num3, 0f, num);
					break;
				case PositionHelperArrow.Direction.NegativeY:
					base.transform.localPosition = new Vector3(num, 0f, num3);
					break;
			}
		}

		// Token: 0x060038F7 RID: 14583 RVA: 0x00118BE5 File Offset: 0x00116FE5
		public string GetEditorDescription()
		{
			return this.direction.ToString();
		}

		// Token: 0x0400614C RID: 24908
		private const float OFFSET = 0.4f;

		// Token: 0x0400614D RID: 24909
		public PositionHelperArrow.Direction direction;

		// Token: 0x02000920 RID: 2336
		public enum Direction
		{
			// Token: 0x0400614F RID: 24911
			Unknown,
			// Token: 0x04006150 RID: 24912
			PositiveX,
			// Token: 0x04006151 RID: 24913
			PositiveY,
			// Token: 0x04006152 RID: 24914
			NegativeX,
			// Token: 0x04006153 RID: 24915
			NegativeY
		}
	}
}
