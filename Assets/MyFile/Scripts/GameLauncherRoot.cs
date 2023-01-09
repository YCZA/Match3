using System.Collections;
using Match3.Scripts1;
using Match3.Scripts1.Puzzletown;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts2.Match3;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// [LoadOptions(false, false, false)]
public class GameLauncherRoot : MonoBehaviour
{
	// [WaitForService(true, true)]
	// private M3ConfigService m3ConfigService;
	
    public Button startBtn;
    public Button levelBtn1;
    public Button levelBtn2;
    public Button levelBtn3;

    protected void Awake()
    {
		// StartCoroutine(StartEventSystem());
		
        startBtn.onClick.RemoveAllListeners();
        levelBtn1.onClick.RemoveAllListeners();
        startBtn.onClick.AddListener(()=>WooroutineRunner.StartCoroutine(ImmediateEnterLevel(Random.Range(1,60), AreaConfig.Tier.b)));
        levelBtn1.onClick.AddListener(()=>WooroutineRunner.StartCoroutine(EnterLevel(1, AreaConfig.Tier.a)));
        levelBtn2.onClick.AddListener(()=>WooroutineRunner.StartCoroutine(EnterLevel(10, AreaConfig.Tier.a)));
        levelBtn3.onClick.AddListener(()=>WooroutineRunner.StartCoroutine(EnterLevel(100, AreaConfig.Tier.b)));
	    
	    SceneManager.Instance.LoadScene<Match3LauncherRoot>(null);
    }

    /// <summary>
    /// 进入指定场景
    /// </summary>
    /// <param name="level"></param>
    /// <param name="tier"></param>
    /// <returns></returns>
    private IEnumerator ImmediateEnterLevel(int level, AreaConfig.Tier tier)
    {
		// Wooroutine<LevelConfig> config = m3ConfigService.GetLevelConfig(m3ConfigService.GetAreaForLevel(level), level, LevelPlayMode.Regular, tier);
		// yield return config;
		
		// CoreGameFlow.Input input = new CoreGameFlow.Input(-1, true, config.ReturnValue, LevelPlayMode.Regular);
		CoreGameFlow.Input input = new CoreGameFlow.Input(-1, true, null, LevelPlayMode.Regular);
		var coreGameFlow = new CoreGameFlow().Start(input);
		yield return coreGameFlow;
		var coreGameFlowResult = coreGameFlow.ReturnValue;
		
		// eli todo 返回初始场景(该功能可以写成flow)
		Debug.Log("载入初始场景");
	    yield return LoadingScreenRoot.PrepareAnimatedLoadingScreen(LoadingScreenConfig.Random, true);
		yield return ServiceLocator.Instance.Inject(this);
		// Wooroutine<GameLauncherRoot> townScene = SceneManager.Instance.LoadScene<GameLauncherRoot>();
		// townScene.ShowLoadingScreen();
		// yield return townScene;
		// yield return townScene.ReturnValue;
		
	    Resources.UnloadUnusedAssets();
    }

    private IEnumerator EnterLevel(int level, AreaConfig.Tier tier)
    {
		CoreGameFlow.Input input = new CoreGameFlow.Input(level, false, null, LevelPlayMode.Regular, tier);
		var coreGameFlow = new CoreGameFlow().Start(input);
		yield return coreGameFlow;
		var coreGameFlowResult = coreGameFlow.ReturnValue;
		
		// eli todo 返回初始场景(该功能可以写成flow)
		if (coreGameFlowResult != null)
		{
			Debug.Log("载入初始场景");
	    	yield return LoadingScreenRoot.PrepareAnimatedLoadingScreen(LoadingScreenConfig.Random, true);
			yield return ServiceLocator.Instance.Inject(this);
			
	    	Resources.UnloadUnusedAssets();
		}
    }
    
    /// <summary>
    /// 启动EventSystem场景
    /// </summary>
    /// <returns></returns>
    IEnumerator StartEventSystem()
	{
		Wooroutine<EventSystemRoot> eventSystem = SceneManager.Instance.LoadScene<EventSystemRoot>(null);
		yield return eventSystem;
		eventSystem.ReturnValue.Enable();
	}
}
