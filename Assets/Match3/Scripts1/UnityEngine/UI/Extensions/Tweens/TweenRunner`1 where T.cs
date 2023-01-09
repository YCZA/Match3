using System.Collections;
using UnityEngine;

namespace Match3.Scripts1.UnityEngine.UI.Extensions.Tweens
{
	// Token: 0x02000B84 RID: 2948
	internal class TweenRunner<T> where T : struct, ITweenValue
	{
		// Token: 0x060044F4 RID: 17652 RVA: 0x0015DC40 File Offset: 0x0015C040
		private static IEnumerator Start(T tweenInfo)
		{
			if (!tweenInfo.ValidTarget())
			{
				yield break;
			}
			float elapsedTime = 0f;
			while (elapsedTime < tweenInfo.duration)
			{
				elapsedTime += ((!tweenInfo.ignoreTimeScale) ? Time.deltaTime : Time.unscaledDeltaTime);
				float percentage = Mathf.Clamp01(elapsedTime / tweenInfo.duration);
				tweenInfo.TweenValue(percentage);
				yield return null;
			}
			tweenInfo.TweenValue(1f);
			tweenInfo.Finished();
			yield break;
		}

		// Token: 0x060044F5 RID: 17653 RVA: 0x0015DC5B File Offset: 0x0015C05B
		public void Init(MonoBehaviour coroutineContainer)
		{
			this.m_CoroutineContainer = coroutineContainer;
		}

		// Token: 0x060044F6 RID: 17654 RVA: 0x0015DC64 File Offset: 0x0015C064
		public void StartTween(T info)
		{
			if (this.m_CoroutineContainer == null)
			{
				global::UnityEngine.Debug.LogWarning("Coroutine container not configured... did you forget to call Init?");
				return;
			}
			if (this.m_Tween != null)
			{
				this.m_CoroutineContainer.StopCoroutine(this.m_Tween);
				this.m_Tween = null;
			}
			if (!this.m_CoroutineContainer.gameObject.activeInHierarchy)
			{
				info.TweenValue(1f);
				return;
			}
			this.m_Tween = TweenRunner<T>.Start(info);
			this.m_CoroutineContainer.StartCoroutine(this.m_Tween);
		}

		// Token: 0x04006CA6 RID: 27814
		protected MonoBehaviour m_CoroutineContainer;

		// Token: 0x04006CA7 RID: 27815
		protected IEnumerator m_Tween;
	}
}
