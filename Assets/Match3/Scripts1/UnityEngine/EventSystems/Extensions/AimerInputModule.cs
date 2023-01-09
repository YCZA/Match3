using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3.Scripts1.UnityEngine.EventSystems.Extensions
{
	// Token: 0x02000BE1 RID: 3041
	[RequireComponent(typeof(EventSystem))]
	[AddComponentMenu("Event/Extensions/Aimer Input Module")]
	public class AimerInputModule : PointerInputModule
	{
		// Token: 0x0600474B RID: 18251 RVA: 0x0016BDA3 File Offset: 0x0016A1A3
		protected AimerInputModule()
		{
		}

		// Token: 0x0600474C RID: 18252 RVA: 0x0016BDCC File Offset: 0x0016A1CC
		public override void ActivateModule()
		{
			StandaloneInputModule component = base.GetComponent<StandaloneInputModule>();
			if (component != null && component.enabled)
			{
				global::UnityEngine.Debug.LogError("Aimer Input Module is incompatible with the StandAloneInputSystem, please remove it from the Event System in this scene or disable it when this module is in use");
			}
		}

		// Token: 0x0600474D RID: 18253 RVA: 0x0016BE04 File Offset: 0x0016A204
		public override void Process()
		{
			bool buttonDown = Input.GetButtonDown(this.activateAxis);
			bool buttonUp = Input.GetButtonUp(this.activateAxis);
			PointerEventData aimerPointerEventData = this.GetAimerPointerEventData();
			this.ProcessInteraction(aimerPointerEventData, buttonDown, buttonUp);
			if (!buttonUp)
			{
				this.ProcessMove(aimerPointerEventData);
			}
			else
			{
				base.RemovePointerData(aimerPointerEventData);
			}
		}

		// Token: 0x0600474E RID: 18254 RVA: 0x0016BE54 File Offset: 0x0016A254
		protected virtual PointerEventData GetAimerPointerEventData()
		{
			PointerEventData pointerEventData;
			base.GetPointerData(-2, out pointerEventData, true);
			pointerEventData.Reset();
			pointerEventData.position = new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f) + this.aimerOffset;
			base.eventSystem.RaycastAll(pointerEventData, this.m_RaycastResultCache);
			RaycastResult pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
			pointerEventData.pointerCurrentRaycast = pointerCurrentRaycast;
			this.m_RaycastResultCache.Clear();
			return pointerEventData;
		}

		// Token: 0x0600474F RID: 18255 RVA: 0x0016BED4 File Offset: 0x0016A2D4
		private void ProcessInteraction(PointerEventData pointer, bool pressed, bool released)
		{
			GameObject gameObject = pointer.pointerCurrentRaycast.gameObject;
			AimerInputModule.objectUnderAimer = ExecuteEvents.GetEventHandler<ISubmitHandler>(gameObject);
			if (pressed)
			{
				pointer.eligibleForClick = true;
				pointer.delta = Vector2.zero;
				pointer.pressPosition = pointer.position;
				pointer.pointerPressRaycast = pointer.pointerCurrentRaycast;
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<ISubmitHandler>(gameObject, pointer, ExecuteEvents.submitHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, pointer, ExecuteEvents.pointerDownHandler);
					if (gameObject2 == null)
					{
						gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
					}
				}
				else
				{
					pointer.eligibleForClick = false;
				}
				if (gameObject2 != pointer.pointerPress)
				{
					pointer.pointerPress = gameObject2;
					pointer.rawPointerPress = gameObject;
					pointer.clickCount = 0;
				}
				pointer.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (pointer.pointerDrag != null)
				{
					ExecuteEvents.Execute<IBeginDragHandler>(pointer.pointerDrag, pointer, ExecuteEvents.beginDragHandler);
				}
			}
			if (released)
			{
				ExecuteEvents.Execute<IPointerUpHandler>(pointer.pointerPress, pointer, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (pointer.pointerPress == eventHandler && pointer.eligibleForClick)
				{
					float unscaledTime = Time.unscaledTime;
					if (unscaledTime - pointer.clickTime < 0.3f)
					{
						pointer.clickCount++;
					}
					else
					{
						pointer.clickCount = 1;
					}
					pointer.clickTime = unscaledTime;
					ExecuteEvents.Execute<IPointerClickHandler>(pointer.pointerPress, pointer, ExecuteEvents.pointerClickHandler);
				}
				else if (pointer.pointerDrag != null)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, pointer, ExecuteEvents.dropHandler);
				}
				pointer.eligibleForClick = false;
				pointer.pointerPress = null;
				pointer.rawPointerPress = null;
				if (pointer.pointerDrag != null)
				{
					ExecuteEvents.Execute<IEndDragHandler>(pointer.pointerDrag, pointer, ExecuteEvents.endDragHandler);
				}
				pointer.pointerDrag = null;
			}
		}

		// Token: 0x06004750 RID: 18256 RVA: 0x0016C0B0 File Offset: 0x0016A4B0
		public override void DeactivateModule()
		{
			base.DeactivateModule();
			base.ClearSelection();
		}

		// Token: 0x04006E68 RID: 28264
		public string activateAxis = "Submit";

		// Token: 0x04006E69 RID: 28265
		public Vector2 aimerOffset = new Vector2(0f, 0f);

		// Token: 0x04006E6A RID: 28266
		public static GameObject objectUnderAimer;
	}
}
