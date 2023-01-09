using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C14 RID: 3092
	[AddComponentMenu("UI/Extensions/HoverTooltip")]
	public class HoverTooltip : MonoBehaviour
	{
		// Token: 0x060048E4 RID: 18660 RVA: 0x00174924 File Offset: 0x00172D24
		private void Start()
		{
			this.GUICamera = GameObject.Find("GUICamera").GetComponent<Camera>();
			this.GUIMode = base.transform.parent.parent.GetComponent<Canvas>().renderMode;
			this.bgImageSource = this.bgImage.GetComponent<Image>();
			this.inside = false;
			this.HideTooltipVisibility();
			base.transform.parent.gameObject.SetActive(false);
		}

		// Token: 0x060048E5 RID: 18661 RVA: 0x0017499A File Offset: 0x00172D9A
		public void SetTooltip(string text)
		{
			this.NewTooltip();
			this.thisText.text = text;
			this.OnScreenSpaceCamera();
		}

		// Token: 0x060048E6 RID: 18662 RVA: 0x001749B4 File Offset: 0x00172DB4
		public void SetTooltip(string[] texts)
		{
			this.NewTooltip();
			string text = string.Empty;
			int num = 0;
			foreach (string text2 in texts)
			{
				if (num == 0)
				{
					text += text2;
				}
				else
				{
					text = text + "\n" + text2;
				}
				num++;
			}
			this.thisText.text = text;
			this.OnScreenSpaceCamera();
		}

		// Token: 0x060048E7 RID: 18663 RVA: 0x00174A24 File Offset: 0x00172E24
		public void SetTooltip(string text, bool test)
		{
			this.NewTooltip();
			this.thisText.text = text;
			this.OnScreenSpaceCamera();
		}

		// Token: 0x060048E8 RID: 18664 RVA: 0x00174A40 File Offset: 0x00172E40
		public void OnScreenSpaceCamera()
		{
			Vector3 position = this.GUICamera.ScreenToViewportPoint(global::UnityEngine.Input.mousePosition);
			float num = this.GUICamera.ViewportToScreenPoint(position).x + this.tooltipRealWidth * this.bgImage.pivot.x;
			if (num > this.upperRight.x)
			{
				float num2 = this.upperRight.x - num;
				float num3;
				if ((double)num2 > (double)this.defaultXOffset * 0.75)
				{
					num3 = num2;
				}
				else
				{
					num3 = this.defaultXOffset - this.tooltipRealWidth * 2f;
				}
				Vector3 position2 = new Vector3(this.GUICamera.ViewportToScreenPoint(position).x + num3, 0f, 0f);
				position.x = this.GUICamera.ScreenToViewportPoint(position2).x;
			}
			num = this.GUICamera.ViewportToScreenPoint(position).x - this.tooltipRealWidth * this.bgImage.pivot.x;
			if (num < this.lowerLeft.x)
			{
				float num4 = this.lowerLeft.x - num;
				float num3;
				if ((double)num4 < (double)this.defaultXOffset * 0.75 - (double)this.tooltipRealWidth)
				{
					num3 = -num4;
				}
				else
				{
					num3 = this.tooltipRealWidth * 2f;
				}
				Vector3 position3 = new Vector3(this.GUICamera.ViewportToScreenPoint(position).x - num3, 0f, 0f);
				position.x = this.GUICamera.ScreenToViewportPoint(position3).x;
			}
			num = this.GUICamera.ViewportToScreenPoint(position).y - (this.bgImage.sizeDelta.y * this.currentYScaleFactor * this.bgImage.pivot.y - this.tooltipRealHeight);
			if (num > this.upperRight.y)
			{
				float num5 = this.upperRight.y - num;
				float num6 = this.bgImage.sizeDelta.y * this.currentYScaleFactor * this.bgImage.pivot.y;
				if ((double)num5 > (double)this.defaultYOffset * 0.75)
				{
					num6 = num5;
				}
				else
				{
					num6 = this.defaultYOffset - this.tooltipRealHeight * 2f;
				}
				Vector3 position4 = new Vector3(position.x, this.GUICamera.ViewportToScreenPoint(position).y + num6, 0f);
				position.y = this.GUICamera.ScreenToViewportPoint(position4).y;
			}
			num = this.GUICamera.ViewportToScreenPoint(position).y - this.bgImage.sizeDelta.y * this.currentYScaleFactor * this.bgImage.pivot.y;
			if (num < this.lowerLeft.y)
			{
				float num7 = this.lowerLeft.y - num;
				float num6 = this.bgImage.sizeDelta.y * this.currentYScaleFactor * this.bgImage.pivot.y;
				if ((double)num7 < (double)this.defaultYOffset * 0.75 - (double)this.tooltipRealHeight)
				{
					num6 = num7;
				}
				else
				{
					num6 = this.tooltipRealHeight * 2f;
				}
				Vector3 position5 = new Vector3(position.x, this.GUICamera.ViewportToScreenPoint(position).y + num6, 0f);
				position.y = this.GUICamera.ScreenToViewportPoint(position5).y;
			}
			base.transform.parent.transform.position = new Vector3(this.GUICamera.ViewportToWorldPoint(position).x, this.GUICamera.ViewportToWorldPoint(position).y, 0f);
			base.transform.parent.gameObject.SetActive(true);
			this.inside = true;
		}

		// Token: 0x060048E9 RID: 18665 RVA: 0x00174E9A File Offset: 0x0017329A
		public void HideTooltip()
		{
			if (this.GUIMode == RenderMode.ScreenSpaceCamera && this != null)
			{
				base.transform.parent.gameObject.SetActive(false);
				this.inside = false;
				this.HideTooltipVisibility();
			}
		}

		// Token: 0x060048EA RID: 18666 RVA: 0x00174ED7 File Offset: 0x001732D7
		private void Update()
		{
			this.LayoutInit();
			if (this.inside && this.GUIMode == RenderMode.ScreenSpaceCamera)
			{
				this.OnScreenSpaceCamera();
			}
		}

		// Token: 0x060048EB RID: 18667 RVA: 0x00174EFC File Offset: 0x001732FC
		private void LayoutInit()
		{
			if (this.firstUpdate)
			{
				this.firstUpdate = false;
				this.bgImage.sizeDelta = new Vector2(this.hlG.preferredWidth + (float)this.horizontalPadding, this.hlG.preferredHeight + (float)this.verticalPadding);
				this.defaultYOffset = this.bgImage.sizeDelta.y * this.currentYScaleFactor * this.bgImage.pivot.y;
				this.defaultXOffset = this.bgImage.sizeDelta.x * this.currentXScaleFactor * this.bgImage.pivot.x;
				this.tooltipRealHeight = this.bgImage.sizeDelta.y * this.currentYScaleFactor;
				this.tooltipRealWidth = this.bgImage.sizeDelta.x * this.currentXScaleFactor;
				this.ActivateTooltipVisibility();
			}
		}

		// Token: 0x060048EC RID: 18668 RVA: 0x00175004 File Offset: 0x00173404
		private void NewTooltip()
		{
			this.firstUpdate = true;
			this.lowerLeft = this.GUICamera.ViewportToScreenPoint(new Vector3(0f, 0f, 0f));
			this.upperRight = this.GUICamera.ViewportToScreenPoint(new Vector3(1f, 1f, 0f));
			this.currentYScaleFactor = (float)Screen.height / base.transform.root.GetComponent<CanvasScaler>().referenceResolution.y;
			this.currentXScaleFactor = (float)Screen.width / base.transform.root.GetComponent<CanvasScaler>().referenceResolution.x;
		}

		// Token: 0x060048ED RID: 18669 RVA: 0x001750B8 File Offset: 0x001734B8
		public void ActivateTooltipVisibility()
		{
			Color color = this.thisText.color;
			this.thisText.color = new Color(color.r, color.g, color.b, 1f);
			this.bgImageSource.color = new Color(this.bgImageSource.color.r, this.bgImageSource.color.g, this.bgImageSource.color.b, 0.8f);
		}

		// Token: 0x060048EE RID: 18670 RVA: 0x0017514C File Offset: 0x0017354C
		public void HideTooltipVisibility()
		{
			Color color = this.thisText.color;
			this.thisText.color = new Color(color.r, color.g, color.b, 0f);
			this.bgImageSource.color = new Color(this.bgImageSource.color.r, this.bgImageSource.color.g, this.bgImageSource.color.b, 0f);
		}

		// Token: 0x04006F7A RID: 28538
		public int horizontalPadding;

		// Token: 0x04006F7B RID: 28539
		public int verticalPadding;

		// Token: 0x04006F7C RID: 28540
		public Text thisText;

		// Token: 0x04006F7D RID: 28541
		public HorizontalLayoutGroup hlG;

		// Token: 0x04006F7E RID: 28542
		public RectTransform bgImage;

		// Token: 0x04006F7F RID: 28543
		private Image bgImageSource;

		// Token: 0x04006F80 RID: 28544
		private bool firstUpdate;

		// Token: 0x04006F81 RID: 28545
		private bool inside;

		// Token: 0x04006F82 RID: 28546
		private RenderMode GUIMode;

		// Token: 0x04006F83 RID: 28547
		private Camera GUICamera;

		// Token: 0x04006F84 RID: 28548
		private Vector3 lowerLeft;

		// Token: 0x04006F85 RID: 28549
		private Vector3 upperRight;

		// Token: 0x04006F86 RID: 28550
		private float currentYScaleFactor;

		// Token: 0x04006F87 RID: 28551
		private float currentXScaleFactor;

		// Token: 0x04006F88 RID: 28552
		private float defaultYOffset;

		// Token: 0x04006F89 RID: 28553
		private float defaultXOffset;

		// Token: 0x04006F8A RID: 28554
		private float tooltipRealHeight;

		// Token: 0x04006F8B RID: 28555
		private float tooltipRealWidth;
	}
}
