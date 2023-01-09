using System;
using System.Collections.Generic;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000854 RID: 2132
namespace Match3.Scripts1
{
	public class BackButtonManager : MonoBehaviour
	{
		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x060034B4 RID: 13492 RVA: 0x000FBF8C File Offset: 0x000FA38C
		public static BackButtonManager Instance
		{
			get
			{
				if (BackButtonManager._instance == null)
				{
					BackButtonManager._instance = new GameObject().AddComponent<BackButtonManager>();
					BackButtonManager._instance.gameObject.name = "BackButtonManager";
					global::UnityEngine.Object.DontDestroyOnLoad(BackButtonManager._instance.gameObject);
				}
				return BackButtonManager._instance;
			}
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x060034B5 RID: 13493 RVA: 0x000FBFE0 File Offset: 0x000FA3E0
		public static bool HasInstance
		{
			get
			{
				return BackButtonManager._instance != null;
			}
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x060034B6 RID: 13494 RVA: 0x000FBFED File Offset: 0x000FA3ED
		// (set) Token: 0x060034B7 RID: 13495 RVA: 0x000FC003 File Offset: 0x000FA403
		public bool IsEnabled
		{
			get
			{
				return this.isEnabled && this.EventSystemActiveAndEnabled;
			}
			private set
			{
				this.isEnabled = value;
			}
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x060034B8 RID: 13496 RVA: 0x000FC00C File Offset: 0x000FA40C
		public bool EventSystemActiveAndEnabled
		{
			get
			{
				EventSystem current = EventSystem.current;
				return current && current.isActiveAndEnabled;
			}
		}

		// Token: 0x060034B9 RID: 13497 RVA: 0x000FC033 File Offset: 0x000FA433
		private void Awake()
		{
			this.IsEnabled = true;
		}

		// Token: 0x060034BA RID: 13498 RVA: 0x000FC03C File Offset: 0x000FA43C
		private void Update()
		{
			if (global::UnityEngine.Input.GetKeyDown(KeyCode.Escape))
			{
				this.CallAction();
			}
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x000FC050 File Offset: 0x000FA450
		public void AddAction(Action action)
		{
			if (!this.stack.IsNullOrEmptyCollection() && this.stack.Peek() == action)
			{
				return;
			}
			this.stack.Push(action);
		}

		// Token: 0x060034BC RID: 13500 RVA: 0x000FC085 File Offset: 0x000FA485
		public void RemoveAction(Action action)
		{
			if (!this.stack.IsNullOrEmptyCollection() && this.stack.Peek() == action)
			{
				this.stack.Pop();
			}
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x000FC0B9 File Offset: 0x000FA4B9
		public void SetEnabled(bool enable)
		{
			this.IsEnabled = enable;
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x000FC0C2 File Offset: 0x000FA4C2
		public void HandleLoadSceneNonAdditive()
		{
			this.Reset();
		}

		// Token: 0x060034BF RID: 13503 RVA: 0x000FC0CC File Offset: 0x000FA4CC
		private void CallAction()
		{
			if (this.IsEnabled && !this.stack.IsNullOrEmptyCollection())
			{
				try
				{
					this.stack.Pop()();
				}
				catch (Exception ex)
				{
					Log.Warning(ex, ex.Message, null);
				}
			}
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x000FC12C File Offset: 0x000FA52C
		private void Reset()
		{
			this.stack.Clear();
			this.IsEnabled = true;
		}

		// Token: 0x04005CA3 RID: 23715
		private static BackButtonManager _instance;

		// Token: 0x04005CA4 RID: 23716
		private Stack<Action> stack = new Stack<Action>();

		// Token: 0x04005CA5 RID: 23717
		private bool isEnabled;
	}
}
