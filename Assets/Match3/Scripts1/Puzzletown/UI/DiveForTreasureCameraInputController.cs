using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020009C8 RID: 2504
	public class DiveForTreasureCameraInputController : AUiAdjuster, IEndDragHandler, IDragHandler, IPointerDownHandler, IEventSystemHandler
	{
		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06003CA8 RID: 15528 RVA: 0x001302EC File Offset: 0x0012E6EC
		private Camera MainCamera
		{
			get
			{
				if (this._mainCamera == null)
				{
					this._mainCamera = Camera.main;
				}
				return this._mainCamera;
			}
		}

		// Token: 0x06003CA9 RID: 15529 RVA: 0x00130310 File Offset: 0x0012E710
		private new void Start()
		{
			CameraControllerSettings matchingSetting = base.GetMatchingSetting<CameraControllerSettings>(this.cameraSettings);
			if (matchingSetting != null)
			{
				this.MainCamera.transform.position = matchingSetting.cameraInitialPosition;
				this.minY = matchingSetting.minY;
				this.maxY = matchingSetting.maxY;
			}
		}

		// Token: 0x06003CAA RID: 15530 RVA: 0x00130360 File Offset: 0x0012E760
		public void ScrollToLevel(int level)
		{
			float targetYPosition = this.GetTargetYPosition(level);
			base.StartCoroutine(this.ScrollCameraToRoutine(targetYPosition, 0.25f));
			this.level = level;
		}

		// Token: 0x06003CAB RID: 15531 RVA: 0x0013038F File Offset: 0x0012E78F
		public void OnPointerDown(PointerEventData eventData)
		{
			this.intertia = false;
		}

		// Token: 0x06003CAC RID: 15532 RVA: 0x00130398 File Offset: 0x0012E798
		public void OnEndDrag(PointerEventData eventData)
		{
			if ((double)eventData.delta.magnitude < 0.01)
			{
				return;
			}
			this.intertia = true;
			base.StartCoroutine(this.TweenVelocityRoutine(this.dragDelta));
		}

		// Token: 0x06003CAD RID: 15533 RVA: 0x001303DD File Offset: 0x0012E7DD
		public void OnDrag(PointerEventData eventData)
		{
			this.dragDelta = this.GetWorldDelta(eventData);
			this.movableContent.transform.position = this.ClampPosition(this.movableContent.transform.position + this.dragDelta);
		}

		// Token: 0x06003CAE RID: 15534 RVA: 0x00130420 File Offset: 0x0012E820
		private IEnumerator TweenVelocityRoutine(Vector3 delta)
		{
			while (delta.magnitude > 0.001f && this.intertia)
			{
				delta *= Mathf.Pow(0.075f, Time.deltaTime);
				this.movableContent.transform.position = this.ClampPosition(this.movableContent.transform.position + delta);
				yield return null;
			}
			yield break;
		}

		// Token: 0x06003CAF RID: 15535 RVA: 0x00130444 File Offset: 0x0012E844
		private IEnumerator ScrollCameraToRoutine(float targetYPos, float time)
		{
			float elapsedTime = 0f;
			float ratio = 0f;
			Vector3 startPosition = this.movableContent.transform.position;
			float startPositionY = startPosition.y;
			while (ratio < 1f && this.intertia)
			{
				elapsedTime += Time.deltaTime;
				ratio = elapsedTime / time;
				float deltaYPosition = Mathf.Lerp(startPositionY, targetYPos, ratio);
				this.movableContent.transform.position = new Vector3(startPosition.x, deltaYPosition, startPosition.z);
				yield return null;
			}
			this.movableContent.transform.position = new Vector3(startPosition.x, targetYPos, startPosition.z);
			yield break;
		}

		// Token: 0x06003CB0 RID: 15536 RVA: 0x00130470 File Offset: 0x0012E870
		private Vector3 GetWorldDelta(PointerEventData eventData)
		{
			Vector3 worldPosition = this.GetWorldPosition(eventData.position - eventData.delta);
			Vector3 worldPosition2 = this.GetWorldPosition(eventData.position);
			return worldPosition2 - worldPosition;
		}

		// Token: 0x06003CB1 RID: 15537 RVA: 0x001304AB File Offset: 0x0012E8AB
		private Vector3 ClampPosition(Vector3 position)
		{
			return new Vector3(0f, Mathf.Clamp(position.y, this.minY, this.maxY), 0f);
		}

		// Token: 0x06003CB2 RID: 15538 RVA: 0x001304D4 File Offset: 0x0012E8D4
		private Vector3 GetWorldPosition(Vector2 screenPoint)
		{
			Ray ray = this.MainCamera.ScreenPointToRay(new Vector2(screenPoint.x, screenPoint.y));
			Plane plane = new Plane(Vector3.back, 0f);
			float distance;
			plane.Raycast(ray, out distance);
			return ray.GetPoint(distance);
		}

		// Token: 0x06003CB3 RID: 15539 RVA: 0x0013052C File Offset: 0x0012E92C
		private float GetTargetYPosition(int level)
		{
			level = Math.Min(level, 8);
			float num = (float)level / 8f;
			float num2 = this.maxY - this.minY;
			return this.minY + num2 * num;
		}

		// Token: 0x06003CB4 RID: 15540 RVA: 0x00130568 File Offset: 0x0012E968
		protected override void AdjustValues()
		{
			CameraControllerSettings matchingSetting = base.GetMatchingSetting<CameraControllerSettings>(this.cameraSettings);
			if (matchingSetting != null)
			{
				this.minY = matchingSetting.minY;
				this.maxY = matchingSetting.maxY;
				this.MainCamera.transform.position = matchingSetting.cameraInitialPosition;
				this.ScrollToLevel(this.level);
			}
		}

		// Token: 0x04006565 RID: 25957
		private const int LEVEL_COUNT = 8;

		// Token: 0x04006566 RID: 25958
		private const float SCROLL_TIME = 0.25f;

		// Token: 0x04006567 RID: 25959
		private const float DECELERATION_RATE = 0.075f;

		// Token: 0x04006568 RID: 25960
		[SerializeField]
		private Transform scrollingCenter;

		// Token: 0x04006569 RID: 25961
		[SerializeField]
		private GameObject movableContent;

		// Token: 0x0400656A RID: 25962
		[SerializeField]
		private CameraControllerSettings[] cameraSettings;

		// Token: 0x0400656B RID: 25963
		[SerializeField]
		private ChestView[] chestViews;

		// Token: 0x0400656C RID: 25964
		private float minY;

		// Token: 0x0400656D RID: 25965
		private float maxY;

		// Token: 0x0400656E RID: 25966
		private int level;

		// Token: 0x0400656F RID: 25967
		private bool intertia;

		// Token: 0x04006570 RID: 25968
		private Vector3 dragDelta;

		// Token: 0x04006571 RID: 25969
		private Camera _mainCamera;
	}
}
