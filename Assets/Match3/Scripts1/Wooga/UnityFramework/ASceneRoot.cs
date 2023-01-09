using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Wooga.UnityFramework
{
	// Token: 0x02000B5E RID: 2910
	[LoadOptions(false, false, false)]
	public abstract class ASceneRoot : MonoBehaviour, IInitializable
	{
		// Token: 0x06004402 RID: 17410 RVA: 0x0008D3FC File Offset: 0x0008B7FC
		public ASceneRoot()
		{
			this.OnInitialized = new AwaitSignal();
		}

		// Token: 0x06004403 RID: 17411 RVA: 0x0008D430 File Offset: 0x0008B830
		public virtual void DeInit()
		{
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x06004404 RID: 17412 RVA: 0x0008D432 File Offset: 0x0008B832
		// (set) Token: 0x06004405 RID: 17413 RVA: 0x0008D43A File Offset: 0x0008B83A
		public AwaitSignal OnInitialized { get; private set; }

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06004406 RID: 17414 RVA: 0x0008D443 File Offset: 0x0008B843
		// (set) Token: 0x06004407 RID: 17415 RVA: 0x0008D44B File Offset: 0x0008B84B
		public bool registeredFirst { get; protected set; }

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x06004408 RID: 17416 RVA: 0x0008D454 File Offset: 0x0008B854
		protected virtual bool IsSetup
		{
			get
			{
				return true;
			}
		}

		private int count = 0;
		// Token: 0x06004409 RID: 17417 RVA: 0x0008D457 File Offset: 0x0008B857
		public void Enable()
		{
			if (this.GetType().ToString().StartsWith("EventSystem"))
			{
				// Debug.Log("eventsystem:Enable+++++++++");
				if (EventSystemRoot.isUsedByTutorial)
				{
					base.gameObject.SetActive(true);
				}

				return;
			}
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600440A RID: 17418 RVA: 0x0008D465 File Offset: 0x0008B865
		public void Disable()
		{
			if (this.GetType().ToString().StartsWith("Event"))
			{
				// Debug.Log("eventsystem:Disable---------");
			}
			if (this && base.gameObject)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600440B RID: 17419 RVA: 0x0008D48E File Offset: 0x0008B88E
		public void Destroy()
		{
			if (this && base.gameObject)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x0600440C RID: 17420 RVA: 0x0008D4B6 File Offset: 0x0008B8B6
		public static string GetSceneName(Type type)
		{
			return type.Name.Substring(0, type.Name.Length - ASceneRoot.LENGTH_OF_ROOT);
		}

		// Token: 0x0600440D RID: 17421 RVA: 0x0008D4D5 File Offset: 0x0008B8D5
		protected virtual void Awake()
		{
			this.registeredFirst = SceneManager.Instance.Register(this);
			WooroutineRunner.StartCoroutine(this.StartSceneRoutine(), null);
			this.EnableEventSystemIfNoneIsActive();
		}

		// Token: 0x0600440E RID: 17422 RVA: 0x0008D4FC File Offset: 0x0008B8FC
		private IEnumerator StartSceneRoutine()
		{
			if (this.multipleHideThis != null)
			{
				foreach (Behaviour behaviour in this.multipleHideThis)
				{
					behaviour.enabled = false;
				}
			}
			if (this.registeredFirst)
			{
				yield return null;
			}
			Coroutine injectScenes = SceneManager.Instance.Inject(this);
			Coroutine injectServices = ServiceLocator.Instance.Inject(this);
			yield return injectScenes;
			yield return injectServices;
			while (!this.IsSetup)
			{
				yield return null;
			}
			this.Go();
			if (this && base.gameObject.activeInHierarchy)
			{
				yield return base.StartCoroutine(this.GoRoutine());
			}
			if (this && this.multipleHideThis != null)
			{
				foreach (Behaviour behaviour2 in this.multipleHideThis)
				{
					if (behaviour2 != null)
					{
						behaviour2.enabled = true;
					}
				}
			}
			this.OnInitialized.Dispatch();
		}

		// Token: 0x0600440F RID: 17423 RVA: 0x0008D517 File Offset: 0x0008B917
		protected virtual void Go()
		{
		}

		// Token: 0x06004410 RID: 17424 RVA: 0x0008D51C File Offset: 0x0008B91C
		protected virtual IEnumerator GoRoutine()
		{
			yield break;
		}

		// Token: 0x06004411 RID: 17425 RVA: 0x0008D530 File Offset: 0x0008B930
		private void EnableEventSystemIfNoneIsActive()
		{
			if (EventSystem.current == null)
			{
				EventSystem[] componentsInChildren = base.GetComponentsInChildren<EventSystem>(true);
				if (componentsInChildren != null && componentsInChildren.Length > 0)
				{
					componentsInChildren[0].gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x06004412 RID: 17426 RVA: 0x0008D572 File Offset: 0x0008B972
		protected virtual void OnDestroy()
		{
			if (this.onDestroyed.WasDispatched)
			{
				return;
			}
			SceneManager.Instance.Unregister(this);
			this.onDestroyed.Dispatch();
		}

		// Token: 0x06004413 RID: 17427 RVA: 0x0008D59B File Offset: 0x0008B99B
		protected virtual void OnDisable()
		{
			if (this.OnInitialized.WasDispatched)
			{
				this.onDisabled.Dispatch();
			}
		}

		// Token: 0x06004414 RID: 17428 RVA: 0x0008D5B8 File Offset: 0x0008B9B8
		protected virtual void OnEnable()
		{
			this.onEnabled.Dispatch();
		}

		// Token: 0x06004415 RID: 17429 RVA: 0x0008D5C5 File Offset: 0x0008B9C5
		private string TypeToSceneName(Type type)
		{
			return type.Name.Replace("Root", string.Empty);
		}

		// Token: 0x04006C58 RID: 27736
		public readonly AwaitSignal onDestroyed = new AwaitSignal();

		// Token: 0x04006C59 RID: 27737
		public readonly Signal onDisabled = new Signal();

		// Token: 0x04006C5A RID: 27738
		public readonly Signal onEnabled = new Signal();

		// Token: 0x04006C5B RID: 27739
		[SerializeField]
		public Behaviour hideThis;

		// Token: 0x04006C5C RID: 27740
		public List<Behaviour> multipleHideThis;

		// Token: 0x04006C5F RID: 27743
		private static readonly int LENGTH_OF_ROOT = "Root".Length;
	}
}
