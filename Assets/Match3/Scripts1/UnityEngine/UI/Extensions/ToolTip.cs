using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C15 RID: 3093
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Extensions/Tooltip")]
	public class ToolTip : MonoBehaviour
	{
		// Token: 0x060048F0 RID: 18672 RVA: 0x001751E8 File Offset: 0x001735E8
		public void Awake()
		{
			Canvas componentInParent = base.GetComponentInParent<Canvas>();
			this._guiCamera = componentInParent.worldCamera;
			this._guiMode = componentInParent.renderMode;
			this._rectTransform = base.GetComponent<RectTransform>();
			this._text = base.GetComponentInChildren<Text>();
			this._inside = false;
			this.xShift = 0f;
			this.YShift = -30f;
			base.gameObject.SetActive(false);
		}

		// Token: 0x060048F1 RID: 18673 RVA: 0x00175258 File Offset: 0x00173658
		public void SetTooltip(string ttext)
		{
			if (this._guiMode == RenderMode.ScreenSpaceCamera)
			{
				this._text.text = ttext;
				this._rectTransform.sizeDelta = new Vector2(this._text.preferredWidth + 40f, this._text.preferredHeight + 25f);
				this.OnScreenSpaceCamera();
			}
		}

		// Token: 0x060048F2 RID: 18674 RVA: 0x001752B5 File Offset: 0x001736B5
		public void HideTooltip()
		{
			if (this._guiMode == RenderMode.ScreenSpaceCamera)
			{
				base.gameObject.SetActive(false);
				this._inside = false;
			}
		}

		// Token: 0x060048F3 RID: 18675 RVA: 0x001752D6 File Offset: 0x001736D6
		private void FixedUpdate()
		{
			if (this._inside && this._guiMode == RenderMode.ScreenSpaceCamera)
			{
				this.OnScreenSpaceCamera();
			}
		}

		// Token: 0x060048F4 RID: 18676 RVA: 0x001752F8 File Offset: 0x001736F8
		public void OnScreenSpaceCamera()
		{
			Vector3 position = this._guiCamera.ScreenToViewportPoint(global::UnityEngine.Input.mousePosition - new Vector3(this.xShift, this.YShift, 0f));
			Vector3 vector = this._guiCamera.ViewportToWorldPoint(position);
			this.width = this._rectTransform.sizeDelta[0];
			this.height = this._rectTransform.sizeDelta[1];
			Vector3 vector2 = this._guiCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
			Vector3 vector3 = this._guiCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
			float num = vector.x + this.width / 2f;
			if (num > vector3.x)
			{
				Vector3 vector4 = new Vector3(num - vector3.x, 0f, 0f);
				Vector3 position2 = new Vector3(vector.x - vector4.x, position.y, 0f);
				position.x = this._guiCamera.WorldToViewportPoint(position2).x;
			}
			num = vector.x - this.width / 2f;
			if (num < vector2.x)
			{
				Vector3 vector5 = new Vector3(vector2.x - num, 0f, 0f);
				Vector3 position3 = new Vector3(vector.x + vector5.x, position.y, 0f);
				position.x = this._guiCamera.WorldToViewportPoint(position3).x;
			}
			num = vector.y + this.height / 2f;
			if (num > vector3.y)
			{
				Vector3 vector6 = new Vector3(0f, 35f + this.height / 2f, 0f);
				Vector3 position4 = new Vector3(position.x, vector.y - vector6.y, 0f);
				position.y = this._guiCamera.WorldToViewportPoint(position4).y;
			}
			num = vector.y - this.height / 2f;
			if (num < vector2.y)
			{
				Vector3 vector7 = new Vector3(0f, 35f + this.height / 2f, 0f);
				Vector3 position5 = new Vector3(position.x, vector.y + vector7.y, 0f);
				position.y = this._guiCamera.WorldToViewportPoint(position5).y;
			}
			base.transform.position = new Vector3(vector.x, vector.y, 0f);
			base.gameObject.SetActive(true);
			this._inside = true;
		}

		// Token: 0x04006F8C RID: 28556
		private Text _text;

		// Token: 0x04006F8D RID: 28557
		private RectTransform _rectTransform;

		// Token: 0x04006F8E RID: 28558
		private bool _inside;

		// Token: 0x04006F8F RID: 28559
		private float width;

		// Token: 0x04006F90 RID: 28560
		private float height;

		// Token: 0x04006F91 RID: 28561
		private float YShift;

		// Token: 0x04006F92 RID: 28562
		private float xShift;

		// Token: 0x04006F93 RID: 28563
		private RenderMode _guiMode;

		// Token: 0x04006F94 RID: 28564
		private Camera _guiCamera;
	}
}
