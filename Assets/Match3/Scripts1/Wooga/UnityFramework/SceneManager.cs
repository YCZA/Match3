using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Match3.Scripts1;
using Match3.Scripts1.Puzzletown;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Puzzletown.Tutorials;
using Match3.Scripts1.Puzzletown.UI;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.AssetBundleManager;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wooga.UnityFramework
{
	// Token: 0x02000B62 RID: 2914
	public class SceneManager : ALocator<ASceneRoot, WaitForRoot>
	{
		// Token: 0x06004422 RID: 17442 RVA: 0x0015B358 File Offset: 0x00159758
		public SceneManager()
		{
			for (int i = 0; i < global::UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
			{
				string scenePathByBuildIndex = SceneUtility.GetScenePathByBuildIndex(i);
				this.scenesInBuild.Add(Path.GetFileNameWithoutExtension(scenePathByBuildIndex));
			}
			this.sceneBundleRegistry = (Resources.Load("SceneBundleRegistry", typeof(SceneBundleRegistry)) as SceneBundleRegistry);
		}

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x06004423 RID: 17443 RVA: 0x0015B3D9 File Offset: 0x001597D9
		public static SceneManager Instance
		{
			get
			{
				SceneManager result;
				if ((result = SceneManager._instance) == null)
				{
					result = (SceneManager._instance = new SceneManager());
				}
				return result;
			}
		}

		// Token: 0x06004424 RID: 17444 RVA: 0x0015B3F2 File Offset: 0x001597F2
		public void MapTypeToSceneName(Type type, string sceneName)
		{
			Debug.Log("map type to scene name: " + type.ToString() + " " + sceneName);
			this.mapTypeToSceneName[type] = sceneName;
		}

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x06004425 RID: 17445 RVA: 0x0015B401 File Offset: 0x00159801
		public static bool IsLoadingAnything
		{
			get
			{
				return SceneManager._instance != null && SceneManager._instance.scenesLoading.Count > 0;
			}
		}

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x06004426 RID: 17446 RVA: 0x0015B422 File Offset: 0x00159822
		public static bool IsPlayingMatch3
		{
			get
			{
				return SceneManager.Instance.Has(typeof(M3_LevelRoot), false);
			}
		}

		// Token: 0x06004427 RID: 17447 RVA: 0x0015B439 File Offset: 0x00159839
		public static void TryLoadQuestPopupOnIsland()
		{
			if (!SceneManager.IsLoadingScreenShown() && !SceneManager.IsPlayingMatch3)
			{
				SceneManager.Instance.LoadScene<QuestsPopupRoot>(null);
			}
			else
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Trying to load quests popup while on m3"
				});
			}
		}

		// Token: 0x06004428 RID: 17448 RVA: 0x0015B473 File Offset: 0x00159873
		public static bool IsLoadingScreenShown()
		{
			return LoadingScreenRoot.isVisible;
		}

		// Token: 0x06004429 RID: 17449 RVA: 0x0015B47C File Offset: 0x0015987C
		public static bool IsPopupOrTutorialShown()
		{
			if (BlockerManager.global.HasBlockers)
			{
				return true;
			}
			TutorialRunner tutorialRunner = global::UnityEngine.Object.FindObjectOfType<TutorialRunner>();
			if (tutorialRunner != null && tutorialRunner.IsRunning)
			{
				return true;
			}
			if (SceneManager.IsLoadingScreenShown())
			{
				return true;
			}
			AnimatedUi[] array = global::UnityEngine.Object.FindObjectsOfType<AnimatedUi>();
			if (array == null || array.Length < 1)
			{
				return false;
			}
			foreach (AnimatedUi animatedUi in array)
			{
				if (animatedUi.IsVisible && !(animatedUi is UiSpeechBubble))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600442A RID: 17450 RVA: 0x0015B51C File Offset: 0x0015991C
		public bool Register(ASceneRoot sceneRoot)
		{
			bool result = this.items.Count == 0;
			Type type = sceneRoot.GetType();
			if (!this.items.ContainsKey(type) || !this.items[type])
			{
				this.items[type] = sceneRoot;
			}
			this.lastRegistered = sceneRoot;
			return result;
		}

		// Token: 0x0600442B RID: 17451 RVA: 0x0015B57B File Offset: 0x0015997B
		public override void Unregister(IInitializable sceneRoot)
		{
			base.Unregister(sceneRoot);
			this.scenesLoading.Remove(sceneRoot.GetType());
			this.Unload((ASceneRoot)sceneRoot);
		}

		// Token: 0x0600442C RID: 17452 RVA: 0x0015B5A2 File Offset: 0x001599A2
		public override void UnregisterAll()
		{
			base.UnregisterAll();
			this.scenesLoading.Clear();
		}

		// Token: 0x0600442D RID: 17453 RVA: 0x0015B5B8 File Offset: 0x001599B8
		public void Unload(ASceneRoot sceneRoot)
		{
			GameObject gameObject = sceneRoot.gameObject;
			Scene scene = this.FindLoadedScene(gameObject);
			WooroutineRunner.StartCoroutine(this.DelayedUnload(scene), null);
		}

		// Token: 0x0600442E RID: 17454 RVA: 0x0015B5E4 File Offset: 0x001599E4
		private IEnumerator DelayedUnload(Scene scene)
		{
			yield return null;
			if (!scene.name.IsNullOrEmpty())
			{
				global::UnityEngine.SceneManagement.SceneManager.UnloadScene(scene);
			}
			yield break;
		}

		// Token: 0x0600442F RID: 17455 RVA: 0x0015B600 File Offset: 0x00159A00
		public IEnumerator UnloadAllAsync()
		{
			while (this.items.Values.Count > 0)
			{
				KeyValuePair<Type, ASceneRoot> scene = this.items.First<KeyValuePair<Type, ASceneRoot>>();
				string sceneName = ASceneRoot.GetSceneName(scene.Value.GetType());
				if (scene.Value != null)
				{
					bool isValid = global::UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).IsValid();
					global::UnityEngine.Object.DestroyImmediate(scene.Value.gameObject);
					if (isValid)
					{
						yield return global::UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
					}
					else
					{
						WoogaDebug.Log(new object[]
						{
							"SceneManager: Failed to unload ",
							sceneName
						});
					}
				}
				this.items.Remove(scene.Key);
			}
			this.lastRegistered = null;
			this.scenesLoading.Clear();
			yield break;
		}

		// Token: 0x06004430 RID: 17456 RVA: 0x0015B61B File Offset: 0x00159A1B
		public override Coroutine Inject(object injectInto)
		{
			return WooroutineRunner.StartCoroutine(this.InjectScenesRoutine(injectInto), null);
		}

		// Token: 0x06004431 RID: 17457 RVA: 0x0015B62C File Offset: 0x00159A2C
		public Wooroutine<TRoot> LoadScene<TRoot>(LoadOptions options = null) where TRoot : ASceneRoot
		{
			Type typeFromHandle = typeof(TRoot);
			options = this.GetOptions<TRoot>(options);
			if (!this.scenesLoading.ContainsKey(typeFromHandle))
			{
				this.scenesLoading[typeFromHandle] = WooroutineRunner.StartWooroutine<TRoot>(this.LoadSceneRoutine(typeFromHandle, options));
			}
			return this.Wrap<TRoot>(this.scenesLoading[typeFromHandle]);
		}

		// eli key point: 载入场景，共97个?
		public Wooroutine<TRoot> LoadSceneWithParams<TRoot, TParams>(TParams parameters, LoadOptions options = null) where TRoot : ASceneRoot<TParams>
		{
			Debug.Log("载入场景:" + typeof(TRoot));
			Type typeFromHandle = typeof(TRoot);
			options = this.GetOptions<TRoot>(options);
			if (!this.scenesLoading.ContainsKey(typeFromHandle))
			{
				this.scenesLoading[typeFromHandle] = WooroutineRunner.StartWooroutine<TRoot>(this.LoadSceneWithParamsRoutine<TRoot, TParams>(parameters, options));
			}
			return this.Wrap<TRoot>(this.scenesLoading[typeFromHandle]);
		}

		// Token: 0x06004433 RID: 17459 RVA: 0x0015B6EC File Offset: 0x00159AEC
		private Scene FindLoadedScene(GameObject rootGo)
		{
			for (int i = 0; i < global::UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
			{
				Scene sceneAt = global::UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
				if (sceneAt.isLoaded)
				{
					GameObject[] rootGameObjects = sceneAt.GetRootGameObjects();
					if (rootGameObjects.Contains(rootGo))
					{
						return sceneAt;
					}
				}
			}
			return default(Scene);
		}

		// Token: 0x06004434 RID: 17460 RVA: 0x0015B748 File Offset: 0x00159B48
		private Wooroutine<TRoot> Wrap<TRoot>(CustomYieldInstruction inst) where TRoot : ASceneRoot
		{
			if (inst is Wooroutine<TRoot>)
			{
				return (Wooroutine<TRoot>)inst;
			}
			Wooroutine<ASceneRoot> routine = (Wooroutine<ASceneRoot>)inst;
			return WooroutineRunner.StartWooroutine<TRoot>(this.WrapRoutine<TRoot>(routine));
		}

		// Token: 0x06004435 RID: 17461 RVA: 0x0015B77C File Offset: 0x00159B7C
		private IEnumerator WrapRoutine<TRoot>(Wooroutine<ASceneRoot> routine) where TRoot : ASceneRoot
		{
			yield return routine;
			yield return (TRoot)((object)routine.ReturnValue);
			yield break;
		}

		// Token: 0x06004436 RID: 17462 RVA: 0x0015B797 File Offset: 0x00159B97
		private Wooroutine<ASceneRoot> LoadScene(Type type, LoadOptions options)
		{
			this.scenesLoading[type] = WooroutineRunner.StartWooroutine<ASceneRoot>(this.LoadSceneRoutine(type, options));
			return (Wooroutine<ASceneRoot>)this.scenesLoading[type];
		}

		// Token: 0x06004437 RID: 17463 RVA: 0x0015B7C4 File Offset: 0x00159BC4
		private IEnumerator LoadSceneWithParamsRoutine<TRoot, TParams>(TParams parameters, LoadOptions options) where TRoot : ASceneRoot<TParams>
		{
			bool awaitInitialization = options.awaitInitialization;
			options.awaitInitialization = false;
			
			Wooroutine<TRoot> root = WooroutineRunner.StartWooroutine<TRoot>(this.LoadSceneRoutine(typeof(TRoot), options));
			yield return root;
			TRoot returnValue = root.ReturnValue;
			returnValue.Setup(parameters);
			if (awaitInitialization)
			{
				TRoot returnValue2 = root.ReturnValue;
				yield return returnValue2.OnInitialized;
			}
			yield return root.ReturnValue;
			yield break;
		}

		// Token: 0x06004438 RID: 17464 RVA: 0x0015B7ED File Offset: 0x00159BED
		public string GetSceneBundleName(string sceneName)
		{
			return this.sceneBundleRegistry.GetAssetBundleName(sceneName);
		}

		// Token: 0x06004439 RID: 17465 RVA: 0x0015B7FC File Offset: 0x00159BFC
		private IEnumerator LoadSceneRoutine(Type sceneRootType, LoadOptions options)
		{
			string sceneName;
			if (!this.mapTypeToSceneName.TryGetValue(sceneRootType, out sceneName))
			{
				sceneName = ASceneRoot.GetSceneName(sceneRootType);
			}
			if (!options.loadAdditively)
			{
				SceneManager.onLoadSceneNonAdditive.Dispatch();
			}

			string id = "LOAD SCENE " + sceneName;
			ASceneRoot aSceneRoot = (!options.returnExisting) ? null : (base.Get(sceneRootType, false) as ASceneRoot);
			if (aSceneRoot == null)
			{
				while (this.isLoading)
				{
					yield return null;
				}
				this.isLoading = true;
				if (!this.IsSceneInBuild(sceneName))
				{
					Debug.Log("LoadScene_IsNotInBuild:" + sceneName);
					if (this.abs == null)
					{
						yield return ServiceLocator.Instance.Inject(this);
					}
					Wooroutine<BundledScene> loadBundle = this.abs.LoadScene(this.GetSceneBundleName(sceneName), sceneName, options);
					yield return loadBundle;
					loadBundle.CheckExceptions();
				}
				else
				{
					LoadSceneMode mode = (!options.loadAdditively) ? LoadSceneMode.Single : LoadSceneMode.Additive;
					yield return global::UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, mode);
				}
				this.isLoading = false;
				aSceneRoot = this.lastRegistered;
			}
			if (options.awaitInitialization)
			{
				yield return aSceneRoot.OnInitialized;
			}
			yield return aSceneRoot;
			this.scenesLoading.Remove(sceneRootType);
		}

		// Token: 0x0600443A RID: 17466 RVA: 0x0015B825 File Offset: 0x00159C25
		public bool IsSceneInBuild(string sceneName)
		{
			return this.scenesInBuild.Contains(sceneName);
		}

		// Token: 0x0600443B RID: 17467 RVA: 0x0015B833 File Offset: 0x00159C33
		private bool HasStartedLoading(Type type)
		{
			return this.scenesLoading.ContainsKey(type) || this.items.ContainsKey(type);
		}

		// Token: 0x0600443C RID: 17468 RVA: 0x0015B858 File Offset: 0x00159C58
		private IEnumerator InjectScenesRoutine(object injectInto)
		{
			FieldInfo[] fields = base.GetFieldsForInjection(injectInto);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (!this.HasStartedLoading(fieldInfo.FieldType))
				{
					this.LoadScene(fieldInfo.FieldType, new LoadOptions(true, true, false)).ThrowImmediate = true;
				}
			}
			yield return WooroutineRunner.StartCoroutine(base.InjectRoutine(injectInto), null);
			ASceneRoot injectedRoot = injectInto as ASceneRoot;
			if (injectedRoot)
			{
				foreach (FieldInfo fieldInfo2 in fields)
				{
					ASceneRoot @object = (ASceneRoot)fieldInfo2.GetValue(injectInto);
					WaitForRoot[] array3 = (WaitForRoot[])fieldInfo2.GetCustomAttributes(typeof(WaitForRoot), true);
					if (array3.Length > 0 && array3[0].followLifeCycle)
					{
						injectedRoot.onDestroyed.AddListener(new Action(@object.Destroy));
						injectedRoot.onDisabled.AddListener(new Action(@object.Disable));
						injectedRoot.onEnabled.AddListener(new Action(@object.Enable));
					}
				}
			}
			yield break;
		}

		// Token: 0x0600443D RID: 17469 RVA: 0x0015B87C File Offset: 0x00159C7C
		private LoadOptions GetOptions<TScene>(LoadOptions options) where TScene : ASceneRoot
		{
			if (options == null)
			{
				Type typeFromHandle = typeof(TScene);
				options = (LoadOptions)Attribute.GetCustomAttribute(typeFromHandle, typeof(LoadOptions));
			}
			return options;
		}

		// Token: 0x04006C67 RID: 27751
		[WaitForService(true, true)]
		private AssetBundleService abs;

		// Token: 0x04006C68 RID: 27752
		private static SceneManager _instance;

		// Token: 0x04006C69 RID: 27753
		public static readonly Signal onLoadSceneNonAdditive = new Signal();

		// Token: 0x04006C6A RID: 27754
		private Dictionary<Type, CustomYieldInstruction> scenesLoading = new Dictionary<Type, CustomYieldInstruction>();

		// Token: 0x04006C6B RID: 27755
		private ASceneRoot lastRegistered;

		// Token: 0x04006C6C RID: 27756
		private List<string> scenesInBuild = new List<string>();

		// Token: 0x04006C6D RID: 27757
		private Dictionary<Type, string> mapTypeToSceneName = new Dictionary<Type, string>();

		// Token: 0x04006C6E RID: 27758
		private SceneBundleRegistry sceneBundleRegistry;

		// Token: 0x04006C6F RID: 27759
		private bool isLoading;
	}
}
