using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006A9 RID: 1705
	public class FieldViewFactory : MonoBehaviour
	{
		// Token: 0x06002AA3 RID: 10915 RVA: 0x000A0360 File Offset: 0x0009E760
		public virtual FieldView Create(Field field, BoardView boardView)
		{
			if (!field.isOn)
			{
				return null;
			}
			FieldView component = this.pool.Get(this.prefab.gameObject).GetComponent<FieldView>();
			component.AssignBoardView(boardView);
			component.transform.SetParent(base.transform);
			component.transform.localPosition = (Vector3)field.gridPosition;
			component.Show(field);
			component.name = field.gridPosition.ToString();
			return component;
		}

		// Token: 0x06002AA4 RID: 10916 RVA: 0x000A03E4 File Offset: 0x0009E7E4
		public HiddenItemView CreateHiddenItem(HiddenItemInfo info)
		{
			HiddenItemView hiddenItemView = global::UnityEngine.Object.Instantiate<HiddenItemView>(this.prefabHiddenItem);
			hiddenItemView.transform.SetParent(base.transform);
			hiddenItemView.transform.localScale = Vector3.one * (float)info.Size;
			Vector3 vector = (Vector3)info.bottomLeftPosition;
			float d = 0.5f * (float)(info.Size - 1);
			vector += Vector3.one * d;
			hiddenItemView.transform.localPosition = vector;
			return hiddenItemView;
		}

		// Token: 0x06002AA5 RID: 10917 RVA: 0x000A0468 File Offset: 0x0009E868
		public ColorWheelView CreateColorWheel(IntVector2 position, ColorWheelModel model, BoostOverlayController boostOverlayController)
		{
			ColorWheelView colorWheelView = global::UnityEngine.Object.Instantiate<ColorWheelView>(this.prefabColorWheel);
			colorWheelView.transform.SetParent(base.transform);
			colorWheelView.transform.localScale = Vector3.one * 2f;
			Vector3 vector = (Vector3)position;
			float num = 0.5f;
			vector += new Vector3(num, -num, 0f);
			colorWheelView.transform.localPosition = vector;
			colorWheelView.Model = model;
			colorWheelView.objectPool = this.pool;
			colorWheelView.SetupBoostOverlayListener(boostOverlayController);
			return colorWheelView;
		}

		// Token: 0x040053F4 RID: 21492
		[SerializeField]
		private FieldView prefab;

		// Token: 0x040053F5 RID: 21493
		[SerializeField]
		private ObjectPool pool;

		// Token: 0x040053F6 RID: 21494
		[SerializeField]
		private HiddenItemView prefabHiddenItem;

		// Token: 0x040053F7 RID: 21495
		[SerializeField]
		private ColorWheelView prefabColorWheel;
	}
}
