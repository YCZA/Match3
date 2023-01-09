using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BB8 RID: 3000
	[RequireComponent(typeof(Canvas))]
	[AddComponentMenu("UI/Extensions/Selection Box")]
	public class SelectionBox : MonoBehaviour
	{
		// Token: 0x06004656 RID: 18006 RVA: 0x00164698 File Offset: 0x00162A98
		private void ValidateCanvas()
		{
			Canvas component = base.gameObject.GetComponent<Canvas>();
			if (component.renderMode != RenderMode.ScreenSpaceOverlay)
			{
				throw new Exception("SelectionBox component must be placed on a canvas in Screen Space Overlay mode.");
			}
			CanvasScaler component2 = base.gameObject.GetComponent<CanvasScaler>();
			if (component2 && component2.enabled && (!Mathf.Approximately(component2.scaleFactor, 1f) || component2.uiScaleMode != CanvasScaler.ScaleMode.ConstantPixelSize))
			{
				global::UnityEngine.Object.Destroy(component2);
				global::UnityEngine.Debug.LogWarning("SelectionBox component is on a gameObject with a Canvas Scaler component. As of now, Canvas Scalers without the default settings throw off the coordinates of the selection box. Canvas Scaler has been removed.");
			}
		}

		// Token: 0x06004657 RID: 18007 RVA: 0x0016471C File Offset: 0x00162B1C
		private void SetSelectableGroup(IEnumerable<MonoBehaviour> behaviourCollection)
		{
			if (behaviourCollection == null)
			{
				this.selectableGroup = null;
				return;
			}
			List<MonoBehaviour> list = new List<MonoBehaviour>();
			foreach (MonoBehaviour monoBehaviour in behaviourCollection)
			{
				if (monoBehaviour is IBoxSelectable)
				{
					list.Add(monoBehaviour);
				}
			}
			this.selectableGroup = list.ToArray();
		}

		// Token: 0x06004658 RID: 18008 RVA: 0x0016479C File Offset: 0x00162B9C
		private void CreateBoxRect()
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "Selection Box";
			gameObject.transform.parent = base.transform;
			gameObject.AddComponent<Image>();
			this.boxRect = (gameObject.transform as RectTransform);
		}

		// Token: 0x06004659 RID: 18009 RVA: 0x001647E4 File Offset: 0x00162BE4
		private void ResetBoxRect()
		{
			Image component = this.boxRect.GetComponent<Image>();
			component.color = this.color;
			component.sprite = this.art;
			this.origin = Vector2.zero;
			this.boxRect.anchoredPosition = Vector2.zero;
			this.boxRect.sizeDelta = Vector2.zero;
			this.boxRect.anchorMax = Vector2.zero;
			this.boxRect.anchorMin = Vector2.zero;
			this.boxRect.pivot = Vector2.zero;
			this.boxRect.gameObject.SetActive(false);
		}

		// Token: 0x0600465A RID: 18010 RVA: 0x00164884 File Offset: 0x00162C84
		private void BeginSelection()
		{
			if (!Input.GetMouseButtonDown(0))
			{
				return;
			}
			this.boxRect.gameObject.SetActive(true);
			this.origin = new Vector2(global::UnityEngine.Input.mousePosition.x, global::UnityEngine.Input.mousePosition.y);
			if (!this.PointIsValidAgainstSelectionMask(this.origin))
			{
				this.ResetBoxRect();
				return;
			}
			this.boxRect.anchoredPosition = this.origin;
			MonoBehaviour[] array;
			if (this.selectableGroup == null)
			{
				array = global::UnityEngine.Object.FindObjectsOfType<MonoBehaviour>();
			}
			else
			{
				array = this.selectableGroup;
			}
			List<IBoxSelectable> list = new List<IBoxSelectable>();
			foreach (MonoBehaviour monoBehaviour in array)
			{
				IBoxSelectable boxSelectable = monoBehaviour as IBoxSelectable;
				if (boxSelectable != null)
				{
					list.Add(boxSelectable);
					if (!Input.GetKey(KeyCode.LeftShift))
					{
						boxSelectable.selected = false;
					}
				}
			}
			this.selectables = list.ToArray();
			this.clickedBeforeDrag = this.GetSelectableAtMousePosition();
		}

		// Token: 0x0600465B RID: 18011 RVA: 0x0016498C File Offset: 0x00162D8C
		private bool PointIsValidAgainstSelectionMask(Vector2 screenPoint)
		{
			if (!this.selectionMask)
			{
				return true;
			}
			Camera screenPointCamera = this.GetScreenPointCamera(this.selectionMask);
			return RectTransformUtility.RectangleContainsScreenPoint(this.selectionMask, screenPoint, screenPointCamera);
		}

		// Token: 0x0600465C RID: 18012 RVA: 0x001649C8 File Offset: 0x00162DC8
		private IBoxSelectable GetSelectableAtMousePosition()
		{
			if (!this.PointIsValidAgainstSelectionMask(global::UnityEngine.Input.mousePosition))
			{
				return null;
			}
			foreach (IBoxSelectable boxSelectable in this.selectables)
			{
				RectTransform rectTransform = boxSelectable.transform as RectTransform;
				if (rectTransform)
				{
					Camera screenPointCamera = this.GetScreenPointCamera(rectTransform);
					if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, global::UnityEngine.Input.mousePosition, screenPointCamera))
					{
						return boxSelectable;
					}
				}
				else
				{
					float magnitude = boxSelectable.transform.GetComponent<Renderer>().bounds.extents.magnitude;
					Vector2 screenPointOfSelectable = this.GetScreenPointOfSelectable(boxSelectable);
					if (Vector2.Distance(screenPointOfSelectable, global::UnityEngine.Input.mousePosition) <= magnitude)
					{
						return boxSelectable;
					}
				}
			}
			return null;
		}

		// Token: 0x0600465D RID: 18013 RVA: 0x00164A94 File Offset: 0x00162E94
		private void DragSelection()
		{
			if (!Input.GetMouseButton(0) || !this.boxRect.gameObject.activeSelf)
			{
				return;
			}
			Vector2 a = new Vector2(global::UnityEngine.Input.mousePosition.x, global::UnityEngine.Input.mousePosition.y);
			Vector2 sizeDelta = a - this.origin;
			Vector2 anchoredPosition = this.origin;
			if (sizeDelta.x < 0f)
			{
				anchoredPosition.x = a.x;
				sizeDelta.x = -sizeDelta.x;
			}
			if (sizeDelta.y < 0f)
			{
				anchoredPosition.y = a.y;
				sizeDelta.y = -sizeDelta.y;
			}
			this.boxRect.anchoredPosition = anchoredPosition;
			this.boxRect.sizeDelta = sizeDelta;
			foreach (IBoxSelectable boxSelectable in this.selectables)
			{
				Vector3 v = this.GetScreenPointOfSelectable(boxSelectable);
				boxSelectable.preSelected = (RectTransformUtility.RectangleContainsScreenPoint(this.boxRect, v, null) && this.PointIsValidAgainstSelectionMask(v));
			}
			if (this.clickedBeforeDrag != null)
			{
				this.clickedBeforeDrag.preSelected = true;
			}
		}

		// Token: 0x0600465E RID: 18014 RVA: 0x00164BEC File Offset: 0x00162FEC
		private void ApplySingleClickDeselection()
		{
			if (this.clickedBeforeDrag == null)
			{
				return;
			}
			if (this.clickedAfterDrag != null && this.clickedBeforeDrag.selected && this.clickedBeforeDrag.transform == this.clickedAfterDrag.transform)
			{
				this.clickedBeforeDrag.selected = false;
				this.clickedBeforeDrag.preSelected = false;
			}
		}

		// Token: 0x0600465F RID: 18015 RVA: 0x00164C58 File Offset: 0x00163058
		private void ApplyPreSelections()
		{
			foreach (IBoxSelectable boxSelectable in this.selectables)
			{
				if (boxSelectable.preSelected)
				{
					boxSelectable.selected = true;
					boxSelectable.preSelected = false;
				}
			}
		}

		// Token: 0x06004660 RID: 18016 RVA: 0x00164CA0 File Offset: 0x001630A0
		private Vector2 GetScreenPointOfSelectable(IBoxSelectable selectable)
		{
			RectTransform rectTransform = selectable.transform as RectTransform;
			if (rectTransform)
			{
				Camera screenPointCamera = this.GetScreenPointCamera(rectTransform);
				return RectTransformUtility.WorldToScreenPoint(screenPointCamera, selectable.transform.position);
			}
			return Camera.main.WorldToScreenPoint(selectable.transform.position);
		}

		// Token: 0x06004661 RID: 18017 RVA: 0x00164CF8 File Offset: 0x001630F8
		private Camera GetScreenPointCamera(RectTransform rectTransform)
		{
			RectTransform rectTransform2 = rectTransform;
			Canvas canvas;
			do
			{
				canvas = rectTransform2.GetComponent<Canvas>();
				if (canvas && !canvas.isRootCanvas)
				{
					canvas = null;
				}
				rectTransform2 = (RectTransform)rectTransform2.parent;
			}
			while (canvas == null);
			switch (canvas.renderMode)
			{
			case RenderMode.ScreenSpaceOverlay:
				return null;
			case RenderMode.ScreenSpaceCamera:
				return (!canvas.worldCamera) ? Camera.main : canvas.worldCamera;
			}
			return Camera.main;
		}

		// Token: 0x06004662 RID: 18018 RVA: 0x00164D88 File Offset: 0x00163188
		public IBoxSelectable[] GetAllSelected()
		{
			if (this.selectables == null)
			{
				return new IBoxSelectable[0];
			}
			List<IBoxSelectable> list = new List<IBoxSelectable>();
			foreach (IBoxSelectable boxSelectable in this.selectables)
			{
				if (boxSelectable.selected)
				{
					list.Add(boxSelectable);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06004663 RID: 18019 RVA: 0x00164DE4 File Offset: 0x001631E4
		private void EndSelection()
		{
			if (!Input.GetMouseButtonUp(0) || !this.boxRect.gameObject.activeSelf)
			{
				return;
			}
			this.clickedAfterDrag = this.GetSelectableAtMousePosition();
			this.ApplySingleClickDeselection();
			this.ApplyPreSelections();
			this.ResetBoxRect();
			this.onSelectionChange.Invoke(this.GetAllSelected());
		}

		// Token: 0x06004664 RID: 18020 RVA: 0x00164E41 File Offset: 0x00163241
		private void Start()
		{
			this.ValidateCanvas();
			this.CreateBoxRect();
			this.ResetBoxRect();
		}

		// Token: 0x06004665 RID: 18021 RVA: 0x00164E55 File Offset: 0x00163255
		private void Update()
		{
			this.BeginSelection();
			this.DragSelection();
			this.EndSelection();
		}

		// Token: 0x04006DB8 RID: 28088
		public Color color;

		// Token: 0x04006DB9 RID: 28089
		public Sprite art;

		// Token: 0x04006DBA RID: 28090
		private Vector2 origin;

		// Token: 0x04006DBB RID: 28091
		public RectTransform selectionMask;

		// Token: 0x04006DBC RID: 28092
		private RectTransform boxRect;

		// Token: 0x04006DBD RID: 28093
		private IBoxSelectable[] selectables;

		// Token: 0x04006DBE RID: 28094
		private MonoBehaviour[] selectableGroup;

		// Token: 0x04006DBF RID: 28095
		private IBoxSelectable clickedBeforeDrag;

		// Token: 0x04006DC0 RID: 28096
		private IBoxSelectable clickedAfterDrag;

		// Token: 0x04006DC1 RID: 28097
		public SelectionBox.SelectionEvent onSelectionChange = new SelectionBox.SelectionEvent();

		// Token: 0x02000BB9 RID: 3001
		public class SelectionEvent : UnityEvent<IBoxSelectable[]>
		{
		}
	}
}
