using System.Collections;
using Match3.Scripts1;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts2.PromptBar
{
	[LoadOptions(true, true, false)]
	public class PopupPromptBarRoot : APtSceneRoot<string>, IDisposableDialog
	{
		public AnimatedUi anim;
		[Range(0.5f,7)]
		public float displayDuration;

		public Text promptText;

		[WaitForService(true, true)]
		private ILocalizationService locaService;

		private float remainingDuration = 0;
	
		// 场景加载后在awake里调用Go
		protected override void Go()
		{
			anim.Show();
			WooroutineRunner.StartCoroutine(Hide());
		}

		public static void ShowPrompt(string lanKey)
		{
			WooroutineRunner.StartCoroutine(ShowPromptIE(lanKey));
		}

		private static IEnumerator ShowPromptIE(string lanKey)
		{
			var routine = SceneManager.Instance.LoadSceneWithParams<PopupPromptBarRoot, string>(lanKey);
			yield return routine;
			routine.ReturnValue.UpdateContent();
		}

		private void UpdateContent()
		{
			string lanKey = parameters;
			promptText.text = locaService.GetText(lanKey);
			remainingDuration = displayDuration;
		}

		private IEnumerator Hide()
		{
			while (true)
			{
				yield return new WaitForSeconds(0.2f);
				remainingDuration -= 0.2f;
				if (remainingDuration <= 0)
				{
					break;
				}
			}
			anim.Hide();
		}
	}
}
