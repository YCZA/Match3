using System;
using System.Collections;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;

namespace Match3.Scripts1.UnityEngine
{
	// Token: 0x02000B33 RID: 2867
	public static class ComponentExtensions
	{
		// Token: 0x06004340 RID: 17216 RVA: 0x00157E34 File Offset: 0x00156234
		private static void Execute<T>(this Component self, T component, Action<T> action)
		{
			if (component != null)
			{
				action(component);
			}
		}

		// Token: 0x06004341 RID: 17217 RVA: 0x00157E48 File Offset: 0x00156248
		public static void ExecuteOnChild<T>(this Component self, Action<T> action)
		{
			self.Execute(self.GetComponentInChildren<T>(), action);
		}

		// Token: 0x06004342 RID: 17218 RVA: 0x00157E57 File Offset: 0x00156257
		public static void ExecuteOnChildren<T>(this Component self, Action<T> action, bool includeInactive = false)
		{
			self.GetComponentsInChildren<T>(includeInactive).ForEach(action);
		}

		// Token: 0x06004343 RID: 17219 RVA: 0x00157E66 File Offset: 0x00156266
		public static void ExecuteOnParent<T>(this Component self, Action<T> action)
		{
			self.Execute(self.GetComponentInParent<T>(), action);
		}

		// Token: 0x06004344 RID: 17220 RVA: 0x00157E78 File Offset: 0x00156278
		public static void HandleOnParent<T>(this Component self, T value)
		{
			self.Execute(self.GetComponentInParent<IHandler<T>>(), delegate(IHandler<T> h)
			{
				h.Handle(value);
			});
		}

		// Token: 0x06004345 RID: 17221 RVA: 0x00157EAC File Offset: 0x001562AC
		public static void HandleOnParent<T1, T2>(this Component self, T1 v1, T2 v2)
		{
			self.Execute(self.GetComponentInParent<IHandler<T1, T2>>(), delegate(IHandler<T1, T2> h)
			{
				h.Handle(v1, v2);
			});
		}

		// Token: 0x06004346 RID: 17222 RVA: 0x00157EE8 File Offset: 0x001562E8
		public static void HandleOnChildren<T>(this Component self, T value, bool includeInactive)
		{
			self.GetComponentsInChildren<IHandler<T>>(includeInactive).ForEach(delegate(IHandler<T> h)
			{
				h.Handle(value);
			});
		}

		// Token: 0x06004347 RID: 17223 RVA: 0x00157F1C File Offset: 0x0015631C
		public static void ShowOnChildren<T>(this Component self, T value, bool includeInactive, bool includeSelf = true)
		{
			self.GetComponentsInChildren<IDataView<T>>(includeInactive).ForEach(delegate(IDataView<T> h)
			{
				if (includeSelf || !object.ReferenceEquals(h, self))
				{
					h.Show(value);
				}
			});
		}

		// Token: 0x06004348 RID: 17224 RVA: 0x00157F64 File Offset: 0x00156364
		public static void ExecuteOnChild<T1, T2>(this Component self, T2 category, Action<T1> action)
		{
			foreach (T1 t in self.GetComponentsInChildren<T1>())
			{
				if (t is ICategorised<T2>)
				{
					T2 category2 = ((ICategorised<T2>)((object)t)).GetCategory();
					if (category2.Equals(category))
					{
						self.Execute(t, action);
						return;
					}
				}
			}
		}

		// Token: 0x06004349 RID: 17225 RVA: 0x00157FE0 File Offset: 0x001563E0
		public static void ExecuteOnChildren<T1, T2>(this Component self, Action<T1> action, params T2[] categories)
		{
			foreach (T1 t in self.GetComponentsInChildren<T1>())
			{
				if (t is ICategorised<T2>)
				{
					if (Array.IndexOf<T2>(categories, ((ICategorised<T2>)((object)t)).GetCategory()) != -1)
					{
						self.Execute(t, action);
						return;
					}
				}
			}
		}

		// Token: 0x0600434A RID: 17226 RVA: 0x0015804F File Offset: 0x0015644F
		public static void InvokeAtEndOfFrame(this MonoBehaviour mb, Action action)
		{
			mb.StartCoroutine(ComponentExtensions.InvokeRoutine(action, new WaitForEndOfFrame()));
		}

		// Token: 0x0600434B RID: 17227 RVA: 0x00158063 File Offset: 0x00156463
		public static void InvokeAfter(this MonoBehaviour mb, Action action, YieldInstruction delay)
		{
			mb.StartCoroutine(ComponentExtensions.InvokeRoutine(action, delay));
		}

		// Token: 0x0600434C RID: 17228 RVA: 0x00158073 File Offset: 0x00156473
		public static void InovkeAfterFrames(this MonoBehaviour mb, Action action, int numFrames)
		{
			mb.StartCoroutine(ComponentExtensions.InvokeAfterFramesRoutine(action, numFrames));
		}

		// Token: 0x0600434D RID: 17229 RVA: 0x00158084 File Offset: 0x00156484
		private static IEnumerator InvokeAfterFramesRoutine(Action action, int numFrames)
		{
			for (int i = 0; i < numFrames; i++)
			{
				yield return null;
			}
			action();
			yield break;
		}

		// Token: 0x0600434E RID: 17230 RVA: 0x001580A8 File Offset: 0x001564A8
		private static IEnumerator InvokeRoutine(Action action, YieldInstruction delay)
		{
			yield return delay;
			action();
			yield break;
		}

		// Token: 0x0600434F RID: 17231 RVA: 0x001580CC File Offset: 0x001564CC
		public static string GetGameObjectPath(this Transform transform)
		{
			string text = transform.name;
			while (transform.parent != null)
			{
				transform = transform.parent;
				text = transform.name + "/" + text;
			}
			return text;
		}

		// Token: 0x06004350 RID: 17232 RVA: 0x00158114 File Offset: 0x00156514
		public static void AddSlowUpdate(this MonoBehaviour monoBehaviour, SlowUpdate slowUpdate, int updateTimeInSeconds = 1)
		{
			SlowUpdateBehaviour slowUpdateBehaviour = monoBehaviour.gameObject.AddComponent<SlowUpdateBehaviour>();
			slowUpdateBehaviour.Initialise(slowUpdate, updateTimeInSeconds);
		}
	}
}
