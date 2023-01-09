using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3.Scripts1.UnityEngine.EventSystems
{
	// Token: 0x02000BE2 RID: 3042
	[AddComponentMenu("Event/Extensions/GamePad Input Module")]
	public class GamePadInputModule : BaseInputModule
	{
		// Token: 0x06004751 RID: 18257 RVA: 0x0016C0C0 File Offset: 0x0016A4C0
		protected GamePadInputModule()
		{
		}

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x06004752 RID: 18258 RVA: 0x0016C115 File Offset: 0x0016A515
		// (set) Token: 0x06004753 RID: 18259 RVA: 0x0016C11D File Offset: 0x0016A51D
		public float inputActionsPerSecond
		{
			get
			{
				return this.m_InputActionsPerSecond;
			}
			set
			{
				this.m_InputActionsPerSecond = value;
			}
		}

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x06004754 RID: 18260 RVA: 0x0016C126 File Offset: 0x0016A526
		// (set) Token: 0x06004755 RID: 18261 RVA: 0x0016C12E File Offset: 0x0016A52E
		public float repeatDelay
		{
			get
			{
				return this.m_RepeatDelay;
			}
			set
			{
				this.m_RepeatDelay = value;
			}
		}

		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x06004756 RID: 18262 RVA: 0x0016C137 File Offset: 0x0016A537
		// (set) Token: 0x06004757 RID: 18263 RVA: 0x0016C13F File Offset: 0x0016A53F
		public string horizontalAxis
		{
			get
			{
				return this.m_HorizontalAxis;
			}
			set
			{
				this.m_HorizontalAxis = value;
			}
		}

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x06004758 RID: 18264 RVA: 0x0016C148 File Offset: 0x0016A548
		// (set) Token: 0x06004759 RID: 18265 RVA: 0x0016C150 File Offset: 0x0016A550
		public string verticalAxis
		{
			get
			{
				return this.m_VerticalAxis;
			}
			set
			{
				this.m_VerticalAxis = value;
			}
		}

		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x0600475A RID: 18266 RVA: 0x0016C159 File Offset: 0x0016A559
		// (set) Token: 0x0600475B RID: 18267 RVA: 0x0016C161 File Offset: 0x0016A561
		public string submitButton
		{
			get
			{
				return this.m_SubmitButton;
			}
			set
			{
				this.m_SubmitButton = value;
			}
		}

		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x0600475C RID: 18268 RVA: 0x0016C16A File Offset: 0x0016A56A
		// (set) Token: 0x0600475D RID: 18269 RVA: 0x0016C172 File Offset: 0x0016A572
		public string cancelButton
		{
			get
			{
				return this.m_CancelButton;
			}
			set
			{
				this.m_CancelButton = value;
			}
		}

		// Token: 0x0600475E RID: 18270 RVA: 0x0016C17C File Offset: 0x0016A57C
		public override bool ShouldActivateModule()
		{
			if (!base.ShouldActivateModule())
			{
				return false;
			}
			bool flag = true;
			flag |= Input.GetButtonDown(this.m_SubmitButton);
			flag |= Input.GetButtonDown(this.m_CancelButton);
			flag |= !Mathf.Approximately(global::UnityEngine.Input.GetAxisRaw(this.m_HorizontalAxis), 0f);
			return flag | !Mathf.Approximately(global::UnityEngine.Input.GetAxisRaw(this.m_VerticalAxis), 0f);
		}

		// Token: 0x0600475F RID: 18271 RVA: 0x0016C1EC File Offset: 0x0016A5EC
		public override void ActivateModule()
		{
			StandaloneInputModule component = base.GetComponent<StandaloneInputModule>();
			if (component && component.enabled)
			{
				global::UnityEngine.Debug.LogError("StandAloneInputSystem should not be used with the GamePadInputModule, please remove it from the Event System in this scene or disable it when this module is in use");
			}
			base.ActivateModule();
			GameObject gameObject = base.eventSystem.currentSelectedGameObject;
			if (gameObject == null)
			{
				gameObject = base.eventSystem.firstSelectedGameObject;
			}
			base.eventSystem.SetSelectedGameObject(gameObject, this.GetBaseEventData());
		}

		// Token: 0x06004760 RID: 18272 RVA: 0x0016C25C File Offset: 0x0016A65C
		public override void DeactivateModule()
		{
			base.DeactivateModule();
		}

		// Token: 0x06004761 RID: 18273 RVA: 0x0016C264 File Offset: 0x0016A664
		public override void Process()
		{
			bool flag = this.SendUpdateEventToSelectedObject();
			if (base.eventSystem.sendNavigationEvents)
			{
				if (!flag)
				{
					flag |= this.SendMoveEventToSelectedObject();
				}
				if (!flag)
				{
					this.SendSubmitEventToSelectedObject();
				}
			}
		}

		// Token: 0x06004762 RID: 18274 RVA: 0x0016C2A4 File Offset: 0x0016A6A4
		protected bool SendSubmitEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			if (Input.GetButtonDown(this.m_SubmitButton))
			{
				ExecuteEvents.Execute<ISubmitHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
			}
			if (Input.GetButtonDown(this.m_CancelButton))
			{
				ExecuteEvents.Execute<ICancelHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
			}
			return baseEventData.used;
		}

		// Token: 0x06004763 RID: 18275 RVA: 0x0016C324 File Offset: 0x0016A724
		private Vector2 GetRawMoveVector()
		{
			Vector2 zero = Vector2.zero;
			zero.x = global::UnityEngine.Input.GetAxisRaw(this.m_HorizontalAxis);
			zero.y = global::UnityEngine.Input.GetAxisRaw(this.m_VerticalAxis);
			if (Input.GetButtonDown(this.m_HorizontalAxis))
			{
				if (zero.x < 0f)
				{
					zero.x = -1f;
				}
				if (zero.x > 0f)
				{
					zero.x = 1f;
				}
			}
			if (Input.GetButtonDown(this.m_VerticalAxis))
			{
				if (zero.y < 0f)
				{
					zero.y = -1f;
				}
				if (zero.y > 0f)
				{
					zero.y = 1f;
				}
			}
			return zero;
		}

		// Token: 0x06004764 RID: 18276 RVA: 0x0016C3F0 File Offset: 0x0016A7F0
		protected bool SendMoveEventToSelectedObject()
		{
			float unscaledTime = Time.unscaledTime;
			Vector2 rawMoveVector = this.GetRawMoveVector();
			if (Mathf.Approximately(rawMoveVector.x, 0f) && Mathf.Approximately(rawMoveVector.y, 0f))
			{
				this.m_ConsecutiveMoveCount = 0;
				return false;
			}
			bool flag = Input.GetButtonDown(this.m_HorizontalAxis) || Input.GetButtonDown(this.m_VerticalAxis);
			bool flag2 = Vector2.Dot(rawMoveVector, this.m_LastMoveVector) > 0f;
			if (!flag)
			{
				if (flag2 && this.m_ConsecutiveMoveCount == 1)
				{
					flag = (unscaledTime > this.m_PrevActionTime + this.m_RepeatDelay);
				}
				else
				{
					flag = (unscaledTime > this.m_PrevActionTime + 1f / this.m_InputActionsPerSecond);
				}
			}
			if (!flag)
			{
				return false;
			}
			AxisEventData axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6f);
			ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
			if (!flag2)
			{
				this.m_ConsecutiveMoveCount = 0;
			}
			this.m_ConsecutiveMoveCount++;
			this.m_PrevActionTime = unscaledTime;
			this.m_LastMoveVector = rawMoveVector;
			return axisEventData.used;
		}

		// Token: 0x06004765 RID: 18277 RVA: 0x0016C524 File Offset: 0x0016A924
		protected bool SendUpdateEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			ExecuteEvents.Execute<IUpdateSelectedHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
			return baseEventData.used;
		}

		// Token: 0x04006E6B RID: 28267
		private float m_PrevActionTime;

		// Token: 0x04006E6C RID: 28268
		private Vector2 m_LastMoveVector;

		// Token: 0x04006E6D RID: 28269
		private int m_ConsecutiveMoveCount;

		// Token: 0x04006E6E RID: 28270
		[SerializeField]
		private string m_HorizontalAxis = "Horizontal";

		// Token: 0x04006E6F RID: 28271
		[SerializeField]
		private string m_VerticalAxis = "Vertical";

		// Token: 0x04006E70 RID: 28272
		[SerializeField]
		private string m_SubmitButton = "Submit";

		// Token: 0x04006E71 RID: 28273
		[SerializeField]
		private string m_CancelButton = "Cancel";

		// Token: 0x04006E72 RID: 28274
		[SerializeField]
		private float m_InputActionsPerSecond = 10f;

		// Token: 0x04006E73 RID: 28275
		[SerializeField]
		private float m_RepeatDelay = 0.1f;
	}
}
