using Match3.Scripts1.Puzzletown.Services;
using Wooga.UnityFramework;
using UnityEngine;

// Token: 0x02000852 RID: 2130
namespace Match3.Scripts1
{
	public static class ASceneRootExtensions
	{
		// Token: 0x060034A9 RID: 13481 RVA: 0x000FBEBF File Offset: 0x000FA2BF
		public static void EnsureServiceRoot(this ASceneRoot root)
		{
			if (!SceneManager.Instance.Has(typeof(ServicesRoot), false))
			{
				SceneManager.Instance.LoadScene<ServicesRoot>(null);
			}
		}

		// Token: 0x060034AA RID: 13482 RVA: 0x000FBEE8 File Offset: 0x000FA2E8
		public static void HideCanvas(this ASceneRoot root)
		{
			if (root.hideThis != null)
			{
				root.multipleHideThis.AddIfNotAlreadyPresent(root.hideThis, false);
			}
			if (root.multipleHideThis == null || root.multipleHideThis.Count < 1)
			{
				Canvas componentInChildren = root.GetComponentInChildren<Canvas>();
				if (componentInChildren != null)
				{
					root.multipleHideThis.Add(componentInChildren);
				}
			}
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x000FBF54 File Offset: 0x000FA354
		public static void ExecutePtExtensions(this ASceneRoot root)
		{
			// EAHelper.AddBreadcrumb("Awake " + root.GetType());
			root.EnsureServiceRoot();
			root.HideCanvas();
		}
	}
}
