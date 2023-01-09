
using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3.Scripts1.UnityEngine.EventSystems
{
	// Token: 0x02000B03 RID: 2819
	public class MultiTouchInputModule : StandaloneInputModule
	{
		// Token: 0x0600429B RID: 17051 RVA: 0x00155744 File Offset: 0x00153B44
		protected override void Start()
		{
			this._pinchData = new PinchEventData(base.eventSystem, 0f);
			this._rotateData = new RotateEventData(base.eventSystem, 0f);
		}

		// Token: 0x0600429C RID: 17052 RVA: 0x00155772 File Offset: 0x00153B72
		public override void Process()
		{
			// eli key point 用户输入相关(如缩放)
			if (Input.mousePresent || global::UnityEngine.Input.touchCount == 1)
			{
				base.Process();
			}
			else
			{
				// eli key point 用户输入相关(如缩放)
				// 手机上缩放使用这个处理
				this.ProcessPinch();
			}
		}

		// Token: 0x0600429D RID: 17053 RVA: 0x0015579C File Offset: 0x00153B9C
		protected void ProcessPinch()
		{
			if (global::UnityEngine.Input.touchCount >= 2)
			{
				bool flag;
				bool flag2;
				PointerEventData touchPointerEventData = base.GetTouchPointerEventData(global::UnityEngine.Input.GetTouch(0), out flag, out flag2);
				bool flag3;
				bool flag4;
				PointerEventData touchPointerEventData2 = base.GetTouchPointerEventData(global::UnityEngine.Input.GetTouch(1), out flag3, out flag4);
				base.eventSystem.RaycastAll(touchPointerEventData, this.m_RaycastResultCache);
				RaycastResult raycastResult = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
				base.eventSystem.RaycastAll(touchPointerEventData2, this.m_RaycastResultCache);
				RaycastResult raycastResult2 = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
				if (global::UnityEngine.Input.GetTouch(0).phase == TouchPhase.Began || global::UnityEngine.Input.GetTouch(1).phase == TouchPhase.Began)
				{
					this._prevVector = global::UnityEngine.Input.GetTouch(1).position - global::UnityEngine.Input.GetTouch(0).position;
					this._touchMode = MultiTouchInputModule._MultiTouchMode.Began;
					ExecuteEvents.Execute<IBeginPinchHandler>(raycastResult.gameObject, this._pinchData, MultiTouchModuleEvents.beginPinchHandler);
				}
				if ((global::UnityEngine.Input.GetTouch(0).phase == TouchPhase.Moved || global::UnityEngine.Input.GetTouch(1).phase == TouchPhase.Moved) && raycastResult.gameObject != null && raycastResult2.gameObject != null && raycastResult.gameObject.Equals(raycastResult2.gameObject))
				{
					bool flag5 = this.DetectMultiTouchMotion();
					if (flag5)
					{
						if (this._touchMode == MultiTouchInputModule._MultiTouchMode.Pinching)
						{
							this._pinchData.data[0] = touchPointerEventData;
							this._pinchData.data[1] = touchPointerEventData2;
							ExecuteEvents.Execute<IPinchHandler>(raycastResult.gameObject, this._pinchData, MultiTouchModuleEvents.pinchHandler);
						}
						else if (this._touchMode == MultiTouchInputModule._MultiTouchMode.Rotating)
						{
							this._rotateData.data[0] = touchPointerEventData;
							this._rotateData.data[1] = touchPointerEventData2;
							ExecuteEvents.Execute<IRotateHandler>(raycastResult.gameObject, this._rotateData, MultiTouchModuleEvents.rotateHandler);
						}
					}
				}
				if (global::UnityEngine.Input.GetTouch(0).phase == TouchPhase.Ended || global::UnityEngine.Input.GetTouch(1).phase == TouchPhase.Ended || global::UnityEngine.Input.GetTouch(0).phase == TouchPhase.Canceled || global::UnityEngine.Input.GetTouch(1).phase == TouchPhase.Canceled)
				{
					this._touchMode = MultiTouchInputModule._MultiTouchMode.Idle;
					this._prevVector = Vector2.zero;
				}
				if (global::UnityEngine.Input.GetTouch(0).phase == TouchPhase.Stationary || global::UnityEngine.Input.GetTouch(1).phase == TouchPhase.Stationary)
				{
					this._stationaryTime += Time.deltaTime;
					if (this._stationaryTime > this.stationaryTimeThreshold)
					{
						this._touchMode = MultiTouchInputModule._MultiTouchMode.Began;
						this._prevVector = global::UnityEngine.Input.GetTouch(1).position - global::UnityEngine.Input.GetTouch(0).position;
						this._stationaryTime = 0f;
					}
				}
			}
		}

		// Token: 0x0600429E RID: 17054 RVA: 0x00155A74 File Offset: 0x00153E74
		private bool DetectMultiTouchMotion()
		{
			if (this._touchMode == MultiTouchInputModule._MultiTouchMode.Began)
			{
				Vector2 vector = global::UnityEngine.Input.GetTouch(1).position - global::UnityEngine.Input.GetTouch(0).position;
				float num = Vector2.Angle(this._prevVector, vector);
				if (num > this.minRotationThreshold)
				{
					this._touchMode = MultiTouchInputModule._MultiTouchMode.Rotating;
					this._prevVector = vector;
				}
				if (Mathf.Abs(vector.magnitude - this._prevVector.magnitude) > this.minPinchThreshold)
				{
					this._touchMode = MultiTouchInputModule._MultiTouchMode.Pinching;
					this._prevVector = vector;
				}
				return false;
			}
			if (this._touchMode == MultiTouchInputModule._MultiTouchMode.Rotating)
			{
				Vector2 vector2 = global::UnityEngine.Input.GetTouch(1).position - global::UnityEngine.Input.GetTouch(0).position;
				float num2 = Vector2.Angle(this._prevVector, vector2);
				Vector3 vector3 = Vector3.Cross(this._prevVector, vector2);
				this._prevVector = vector2;
				this._rotateData.rotateDelta = ((vector3.z >= 0f) ? num2 : (-num2));
				return true;
			}
			if (this._touchMode == MultiTouchInputModule._MultiTouchMode.Pinching)
			{
				Vector2 prevVector = global::UnityEngine.Input.GetTouch(1).position - global::UnityEngine.Input.GetTouch(0).position;
				float pinchDelta = prevVector.magnitude - this._prevVector.magnitude;
				this._prevVector = prevVector;
				this._pinchData.pinchDelta = pinchDelta;
				return true;
			}
			return false;
		}

		// Token: 0x0600429F RID: 17055 RVA: 0x00155BF0 File Offset: 0x00153FF0
		public override string ToString()
		{
			return string.Format("[MultiTouchInputModule]", new object[0]);
		}

		// Token: 0x04006B76 RID: 27510
		private PinchEventData _pinchData;

		// Token: 0x04006B77 RID: 27511
		private RotateEventData _rotateData;

		// Token: 0x04006B78 RID: 27512
		private MultiTouchInputModule._MultiTouchMode _touchMode;

		// Token: 0x04006B79 RID: 27513
		private Vector2 _prevVector = Vector2.zero;

		// Token: 0x04006B7A RID: 27514
		private float _stationaryTime;

		// Token: 0x04006B7B RID: 27515
		public float minRotationThreshold = 10f;

		// Token: 0x04006B7C RID: 27516
		public float minPinchThreshold = 10f;

		// Token: 0x04006B7D RID: 27517
		public float stationaryTimeThreshold = 1f;

		// Token: 0x02000B04 RID: 2820
		private enum _MultiTouchMode
		{
			// Token: 0x04006B7F RID: 27519
			Idle,
			// Token: 0x04006B80 RID: 27520
			Began,
			// Token: 0x04006B81 RID: 27521
			Pinching,
			// Token: 0x04006B82 RID: 27522
			Rotating
		}
	}
}
