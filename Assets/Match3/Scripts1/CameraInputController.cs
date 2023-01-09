using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.UnityEngine.EventSystems;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020008FF RID: 2303
namespace Match3.Scripts1
{
	public class CameraInputController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPinchHandler, IBeginPinchHandler
	{
		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x0600380E RID: 14350 RVA: 0x001120B1 File Offset: 0x001104B1
		public float CamDistanceMin
		{
			get
			{
				return this.zoomLimit.x;
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x0600380F RID: 14351 RVA: 0x001120BE File Offset: 0x001104BE
		public float CamDistanceMax
		{
			get
			{
				return this.zoomLimit.y;
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06003810 RID: 14352 RVA: 0x001120CB File Offset: 0x001104CB
		public float CamDistancePreferred
		{
			get
			{
				return this.zoomLimit.z;
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06003811 RID: 14353 RVA: 0x001120D8 File Offset: 0x001104D8
		public float ZoomRatioSetByUser
		{
			get
			{
				return (this.camDistanceSetByUser >= -99f) ? this.GetZoomRatioFor(this.camDistanceSetByUser) : this.PreferredZoomRatio;
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06003812 RID: 14354 RVA: 0x00112101 File Offset: 0x00110501
		public float CurrentZoomRatio
		{
			get
			{
				return this.GetZoomRatioFor(this.Zoom);
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06003813 RID: 14355 RVA: 0x0011210F File Offset: 0x0011050F
		public float PreferredZoomRatio
		{
			get
			{
				return this.GetZoomRatioFor(this.CamDistancePreferred);
			}
		}

		// Token: 0x06003814 RID: 14356 RVA: 0x0011211D File Offset: 0x0011051D
		private float GetZoomRatioFor(float camDistance)
		{
			return 1f - (camDistance - this.CamDistanceMin) / (this.CamDistanceMax - this.CamDistanceMin);
		}

		// Token: 0x06003815 RID: 14357 RVA: 0x0011213B File Offset: 0x0011053B
		private void OnValidate()
		{
		}

		// Token: 0x06003816 RID: 14358 RVA: 0x0011213D File Offset: 0x0011053D
		public void SetPositionAnchor(Vector3 location)
		{
			this.position.anchored = location;
			this.position.target = location + (CameraInputController.CameraPosition - location) / 0.5f;
		}

		// Token: 0x06003817 RID: 14359 RVA: 0x00112174 File Offset: 0x00110574
		internal void ResetVelocity()
		{
			this.position.velocity = Vector3.zero;
			this.position.anchored = CameraInputController.CameraPosition;
			this.position.target = CameraInputController.CameraPosition;
			this.Zoom = (CameraInputController.CameraPosition - this.mainCamera.transform.position).magnitude;
		}

		// Token: 0x06003818 RID: 14360 RVA: 0x001121DC File Offset: 0x001105DC
		private IEnumerator Start()
		{
			CameraInputController.current = this;
			yield return ServiceLocator.Instance.Inject(this);
			this.Zoom = ((!this.gameState.isInteractable) ? this.zoomLimit.y : this.zoomLimit.z);
			this.mainCamera.SetLayerVisible(ObjectLayer.EditorHelpers, TownCheatsRoot.EditMode);
			this.zoomLimitPortait = this.zoomLimit;
			AUiAdjuster.OnScreenOrientationChange.AddListener(new Action<ScreenOrientation>(this.OnOrientationChange));
			if (AUiAdjuster.SimilarOrientation == ScreenOrientation.LandscapeLeft)
			{
				this.OnOrientationChange(ScreenOrientation.LandscapeLeft);
			}
			else
			{
				this.previousOrientation = AUiAdjuster.SimilarOrientation;
			}
			yield break;
		}

		// Token: 0x06003819 RID: 14361 RVA: 0x001121F7 File Offset: 0x001105F7
		private void OnDestroy()
		{
			CameraInputController.current = null;
		}

		// Token: 0x0600381A RID: 14362 RVA: 0x00112200 File Offset: 0x00110600
		private void Update()
		{
			if (this.scrollDirection.sqrMagnitude > 0f)
			{
				this.scrollDirection.y = 0f;
				Vector3 b = this.scrollDirection * Time.deltaTime;
				this.position.anchored = this.position.anchored + b;
				this.position.target = this.position.target + b;
				this.mainCamera.transform.position += b;
				this.OnDrag(this.lastPointerEvent);
				this.ResetVelocity();
			}
			if (global::UnityEngine.Input.touchCount == 0 && !Input.GetMouseButton(0))
			{
				if (this.CanDragCamera)
				{
					for (int i = 0; i < 3; i++)
					{
						if (Mathf.Abs(this.position.anchored[i] - this.position.target[i]) > 0.001f)
						{
							float value = this.position.velocity[i];
							Mathf.SmoothDamp(this.position.target[i], this.position.anchored[i], ref value, this.m_Elasticity, float.PositiveInfinity, Time.deltaTime);
							this.position.velocity[i] = value;
						}
						else
						{
							Vector3 ptr = this.position.velocity;
							int index;
							this.position.velocity[index = i] = ptr[index] * Mathf.Pow(this.m_DecelerationRate, Time.deltaTime);
						}
					}
					this.position.velocity.y = 0f;
					if (this.position.velocity.sqrMagnitude > 0.1f)
					{
						this.ApplyMoveVector(this.position.velocity * Time.deltaTime, !this.IsPanning);
					}
				}
				if (this.IsZooming)
				{
					this.zoom.target = Mathf.SmoothDamp(this.zoom.target, this.zoom.anchored, ref this.zoom.velocity, this.m_Elasticity, float.PositiveInfinity, Time.deltaTime);
					float d = Mathf.Lerp(this.zoom.target, this.zoom.anchored, 0.5f);
					this.mainCamera.transform.position = CameraInputController.CameraPosition - CameraInputController.CameraRay.direction * d;
				}
			}

			// 通过键盘缩放
			if (Application.isEditor)
			{
				if (Input.mouseScrollDelta.y > 0)
				{
					PinchByKeyboard(Zoom - 3f);
				}
				else if(Input.mouseScrollDelta.y < 0)
				{
					PinchByKeyboard(Zoom + 3f);
				}
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x0600381B RID: 14363 RVA: 0x00112485 File Offset: 0x00110885
		public static Ray CameraRay
		{
			get
			{
				return CameraInputController.current.mainCamera.ViewportPointToRay(CameraInputController.SCREEN_CENTER);
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x0600381C RID: 14364 RVA: 0x001124A0 File Offset: 0x001108A0
		public static Vector3 MousePosition
		{
			get
			{
				if (global::UnityEngine.Input.touchCount >= 2)
				{
					Vector2 a = global::UnityEngine.Input.GetTouch(0).position;
					Vector2 b = global::UnityEngine.Input.GetTouch(1).position;
					return CameraInputController.Project((a + b) / 2f);
				}
				return CameraInputController.Project(global::UnityEngine.Input.mousePosition);
			}
		}

		// Token: 0x0600381D RID: 14365 RVA: 0x001124FC File Offset: 0x001108FC
		public static Vector3 ScreenPosition(IntVector2 worldPos)
		{
			return CameraInputController.current.mainCamera.WorldToScreenPoint(worldPos.ProjectToVector3XZ());
		}

		// Token: 0x0600381E RID: 14366 RVA: 0x00112514 File Offset: 0x00110914
		private static Vector3 Project(Vector2 screenPosition)
		{
			return CameraInputController.Project(CameraInputController.current.mainCamera.ScreenPointToRay(screenPosition));
		}

		// Token: 0x0600381F RID: 14367 RVA: 0x00112530 File Offset: 0x00110930
		private static Vector3 Project(Ray ray)
		{
			return ray.origin - ray.direction * (ray.origin.y / ray.direction.y);
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06003820 RID: 14368 RVA: 0x00112574 File Offset: 0x00110974
		// (set) Token: 0x06003821 RID: 14369 RVA: 0x00112580 File Offset: 0x00110980
		public static Vector3 CameraPosition
		{
			get
			{
				return CameraInputController.Project(CameraInputController.CameraRay);
			}
			set
			{
				CameraInputController.current.position.anchored = value;
				CameraInputController.current.position.target = value;
				CameraInputController.current.position.velocity = Vector3.zero;
			}
		}

		// Token: 0x06003822 RID: 14370 RVA: 0x001125B6 File Offset: 0x001109B6
		private static void SetCameraPosition(Vector3 position)
		{
			CameraInputController.current.mainCamera.transform.position += position - CameraInputController.CameraPosition;
		}

		// eli key point 和屏幕缩放相关
		public float Zoom
		{
			get
			{
				return this.zoom.target;
			}
			set
			{
				this.zoom.velocity = 0f;
				this.zoom.target = Mathf.Min(this.zoomLimit.y * this.zoomRubber.y, Mathf.Max(this.zoomLimit.x * this.zoomRubber.x, value));
				this.zoom.anchored = Mathf.Min(this.zoomLimit.y, Mathf.Max(this.zoomLimit.x, value));
				float d = Mathf.Lerp(this.zoom.target, this.zoom.anchored, 0.5f);
				if (this.mainCamera != null)
				{
					this.mainCamera.transform.position = CameraInputController.CameraPosition - CameraInputController.CameraRay.direction * d;
				}
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06003825 RID: 14373 RVA: 0x001126DC File Offset: 0x00110ADC
		public bool IsZooming
		{
			get
			{
				return Mathf.Abs(this.zoom.anchored - this.zoom.target) > 0.001f;
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06003826 RID: 14374 RVA: 0x00112704 File Offset: 0x00110B04
		public bool IsPanning
		{
			get
			{
				return (this.position.anchored - this.position.target).magnitude > 0.001f;
			}
		}

		// Token: 0x06003827 RID: 14375 RVA: 0x0011273B File Offset: 0x00110B3B
		public void OnBeginDrag(PointerEventData evt)
		{
			if (this.tapAndHoldTimerRoutine != null)
			{
				base.StopCoroutine(this.tapAndHoldTimerRoutine);
				this.tapAndHoldTimerRoutine = null;
			}
			this.dragOrigin = CameraInputController.MousePosition;
			this.Redirect<IBeginDragHandler>(evt, ExecuteEvents.beginDragHandler);
		}

		// Token: 0x06003828 RID: 14376 RVA: 0x00112772 File Offset: 0x00110B72
		public void OnEndDrag(PointerEventData evt)
		{
			if (CameraInputController.currentDragObject == this.mainCamera.gameObject)
			{
				CameraInputController.currentDragObject = null;
			}
			this.Redirect<IEndDragHandler>(evt, ExecuteEvents.endDragHandler);
		}

		// Token: 0x06003829 RID: 14377 RVA: 0x001127A0 File Offset: 0x00110BA0
		public void OnDrag(PointerEventData evt)
		{
			this.lastPointerEvent = evt;
			this.DoDrag();
			this.Redirect<IDragHandler>(evt, ExecuteEvents.dragHandler);
		}

		// Token: 0x0600382A RID: 14378 RVA: 0x001127BC File Offset: 0x00110BBC
		public void DoDrag()
		{
			Vector3 vector = this.dragOrigin - CameraInputController.MousePosition;
			if (this.CanDragCamera)
			{
				this.ApplyMoveVector(vector, true);
			}
			this.dragOrigin = CameraInputController.MousePosition;
			this.position.velocity = vector * 30f;
		}

		// Token: 0x0600382B RID: 14379 RVA: 0x00112810 File Offset: 0x00110C10
		public void ApplyMoveVector(Vector3 move, bool alsoMoveAnchor = false)
		{
			this.position.target = this.position.target + move;
			if (alsoMoveAnchor)
			{
				this.position.anchored = this.position.anchored + move;
			}
			if (this.cameraBounds)
			{
				Transform transform = this.cameraBounds.transform;
				Vector3 anchored = this.position.anchored;
				Vector3 v = transform.worldToLocalMatrix.MultiplyPoint(anchored);
				Vector3 point = this.BoundToBox(this.cameraBounds, v);
				Vector3 anchored2 = transform.localToWorldMatrix.MultiplyPoint(point);
				this.position.anchored = anchored2;
			}
			CameraInputController.SetCameraPosition(Vector3.Lerp(this.position.target, this.position.anchored, 0.5f));
			CameraInputController.currentDragObject = this.mainCamera.gameObject;
		}

		// Token: 0x0600382C RID: 14380 RVA: 0x001128F0 File Offset: 0x00110CF0
		public Vector3 BoundToBox(BoxCollider box, Vector3 v)
		{
			Vector3 vector = box.center - box.size * 0.5f;
			Vector3 vector2 = box.center + box.size * 0.5f;
			return new Vector3(Mathf.Max(vector.x, Mathf.Min(vector2.x, v.x)), v.y, Mathf.Max(vector.z, Mathf.Min(vector2.z, v.z)));
		}

		// Token: 0x0600382D RID: 14381 RVA: 0x0011297F File Offset: 0x00110D7F
		public void OnPointerDown(PointerEventData evt)
		{
			this.tapAndHoldTimerRoutine = base.StartCoroutine(this.TapAndHoldTimer(evt));
			this.Redirect<IPointerDownHandler>(evt, ExecuteEvents.pointerDownHandler);
			this.ResetVelocity();
		}

		// Token: 0x0600382E RID: 14382 RVA: 0x001129A6 File Offset: 0x00110DA6
		public void OnPointerUp(PointerEventData evt)
		{
			if (this.tapAndHoldTimerRoutine != null)
			{
				base.StopCoroutine(this.tapAndHoldTimerRoutine);
				this.tapAndHoldTimerRoutine = null;
			}
			this.Redirect<IPointerUpHandler>(evt, ExecuteEvents.pointerUpHandler);
		}

		// Token: 0x0600382F RID: 14383 RVA: 0x001129D4 File Offset: 0x00110DD4
		private void Redirect<T>(PointerEventData evt, ExecuteEvents.EventFunction<T> handler) where T : IEventSystemHandler
		{
			if (!Camera.main)
			{
				return;
			}
			if (CameraInputController.currentDragObject && CameraInputController.currentDragObject != this.mainCamera.gameObject)
			{
				ExecuteEvents.Execute<T>(CameraInputController.currentDragObject, evt, handler);
			}
			else if (this.gameState != null && this.gameState.isInteractable)
			{
				this.rr.Clear();
				this.cameraRaycaster.Raycast(evt, this.rr);
				this.rr.RemoveAll((RaycastResult r) => r.gameObject.GetComponent<T>() == null);
				if (this.rr.Count == 0)
				{
					return;
				}
				ExecuteEvents.Execute<T>(this.rr[0].gameObject, evt, handler);
			}
		}

		// eli key point 和屏幕缩放相关
		public void OnPinch(PinchEventData data)
		{
			float num = data.pinchDelta / (float)(Screen.width + Screen.height);
			num *= this.Zoom / 60f;
			this.Zoom -= num * this.pinchSensivity;
			this.camDistanceSetByUser = this.Zoom;
			this.DoDrag();
		}

		public void PinchByKeyboard(float zoom)
		{
			Zoom = zoom;
			this.dragOrigin = CameraInputController.MousePosition;
		}

		// Token: 0x06003831 RID: 14385 RVA: 0x00112B04 File Offset: 0x00110F04
		private void OnOrientationChange(ScreenOrientation screenOrientation)
		{
			float num = (float)Screen.width / (float)Screen.height;
			switch (screenOrientation)
			{
				case ScreenOrientation.Portrait:
				case ScreenOrientation.PortraitUpsideDown:
					if (this.previousOrientation != ScreenOrientation.Portrait)
					{
						this.zoomLimit = this.zoomLimitPortait;
						this.Zoom /= num;
						this.previousOrientation = ScreenOrientation.Portrait;
					}
					break;
				case ScreenOrientation.LandscapeLeft:
				case ScreenOrientation.LandscapeRight:
					if (this.previousOrientation != ScreenOrientation.LandscapeLeft)
					{
						this.zoomLimit = this.zoomLimitLandscape;
						this.Zoom /= num;
						this.previousOrientation = ScreenOrientation.LandscapeLeft;
					}
					break;
			}
		}

		// Token: 0x06003832 RID: 14386 RVA: 0x00112BA0 File Offset: 0x00110FA0
		public void OnBeginPinch(PinchEventData data)
		{
			this.dragOrigin = CameraInputController.MousePosition;
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06003833 RID: 14387 RVA: 0x00112BAD File Offset: 0x00110FAD
		private bool CanDragCamera
		{
			get
			{
				return CameraInputController.currentDragObject == null || CameraInputController.currentDragObject == this.mainCamera.gameObject;
			}
		}

		// Token: 0x06003834 RID: 14388 RVA: 0x00112BD8 File Offset: 0x00110FD8
		private IEnumerator TapAndHoldTimer(PointerEventData evt)
		{
			yield return new WaitForSeconds(0.25f);
			if (global::UnityEngine.Input.touchCount < 2)
			{
				this.Redirect<ITapAndHoldHandler>(evt, CameraInputController.tapAndHoldHandler);
			}
			this.tapAndHoldTimerRoutine = null;
			yield break;
		}

		// Token: 0x06003835 RID: 14389 RVA: 0x00112BFA File Offset: 0x00110FFA
		private static void Execute(ITapAndHoldHandler handler, BaseEventData eventData)
		{
			handler.OnTapAndHold(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06003836 RID: 14390 RVA: 0x00112C08 File Offset: 0x00111008
		public static ExecuteEvents.EventFunction<ITapAndHoldHandler> tapAndHoldHandler
		{
			get
			{
				if (CameraInputController._003C_003Ef__mg_0024cache0 == null)
				{
					CameraInputController._003C_003Ef__mg_0024cache0 = new ExecuteEvents.EventFunction<ITapAndHoldHandler>(CameraInputController.Execute);
				}
				return CameraInputController._003C_003Ef__mg_0024cache0;
			}
		}

		// Token: 0x06003837 RID: 14391 RVA: 0x00112C27 File Offset: 0x00111027
		public void StorePosition()
		{
			this.storedPosition = this.position.anchored;
		}

		// Token: 0x06003838 RID: 14392 RVA: 0x00112C3A File Offset: 0x0011103A
		public void RestorePosition()
		{
			this.SetPositionAnchor(this.storedPosition);
		}

		// Token: 0x0400602E RID: 24622
		[WaitForService(true, true)]
		private GameStateService gameState;

		// Token: 0x0400602F RID: 24623
		private const float ZOOM_AMOUNT_IDENTITY = 60f;

		// Token: 0x04006030 RID: 24624
		private const float TARGET2ANCHOR_EPS = 0.001f;

		// Token: 0x04006031 RID: 24625
		private const float TARGET2ANCHOR_RATIO = 0.5f;

		// Token: 0x04006032 RID: 24626
		private const float LANDSCAPE_TO_PORTRAIT_RATIO = 0.625f;

		// Token: 0x04006033 RID: 24627
		private const float MIN_APPLICABLE_VELOCITY_SQUARED = 0.1f;

		// Token: 0x04006034 RID: 24628
		public static readonly Vector2 SCREEN_CENTER = new Vector2(0.5f, 0.33f);

		// Token: 0x04006035 RID: 24629
		public static GameObject currentDragObject = null;

		// Token: 0x04006036 RID: 24630
		public static CameraInputController current;

		// Token: 0x04006037 RID: 24631
		private List<RaycastResult> rr = new List<RaycastResult>(100);

		// Token: 0x04006038 RID: 24632
		private Coroutine tapAndHoldTimerRoutine;

		// Token: 0x04006039 RID: 24633
		private Vector3 dragOrigin;

		// Token: 0x0400603A RID: 24634
		private Vector3 storedPosition;

		// Token: 0x0400603B RID: 24635
		public float pinchSensivity = 100f;

		// 相机缩放范围，要在场景里调
		[Tooltip("X,Y = min/max, Z = default")]
		public Vector3 zoomLimit = new Vector3(20f, 60f, 30f);

		public Vector3 zoomLimitLandscape = new Vector3(20f, 60f, 30f);

		// Token: 0x0400603E RID: 24638
		private Vector3 zoomLimitPortait;

		// Token: 0x0400603F RID: 24639
		public Vector2 zoomRubber = new Vector3(0.33f, 1.25f);

		// Token: 0x04006040 RID: 24640
		[HideInInspector]
		public Vector3 scrollDirection = Vector3.zero;

		// Token: 0x04006041 RID: 24641
		[HideInInspector]
		public BoxCollider cameraBounds;

		// Token: 0x04006042 RID: 24642
		public PointerEventData lastPointerEvent;

		// Token: 0x04006043 RID: 24643
		public Camera selectedObjectCamera;

		// Token: 0x04006044 RID: 24644
		public PhysicsRaycaster cameraRaycaster;

		// Token: 0x04006045 RID: 24645
		public Camera mainCamera;

		// Token: 0x04006046 RID: 24646
		private float camDistanceSetByUser = -100f;

		// Token: 0x04006047 RID: 24647
		private float m_DecelerationRate = 0.075f;

		// Token: 0x04006048 RID: 24648
		private float m_Elasticity = 0.2f;

		// Token: 0x04006049 RID: 24649
		private CameraInputController.Movement<Vector3> position;

		// Token: 0x0400604A RID: 24650
		private CameraInputController.Movement<float> zoom;

		// Token: 0x0400604B RID: 24651
		private ScreenOrientation previousOrientation;

		// Token: 0x0400604C RID: 24652
		[CompilerGenerated]
		private static ExecuteEvents.EventFunction<ITapAndHoldHandler> _003C_003Ef__mg_0024cache0;

		// Token: 0x02000900 RID: 2304
		private struct Movement<T>
		{
			// Token: 0x0400604D RID: 24653
			public T velocity;

			// Token: 0x0400604E RID: 24654
			public T anchored;

			// Token: 0x0400604F RID: 24655
			public T target;
		}
	}
}
