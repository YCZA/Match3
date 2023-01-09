using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C02 RID: 3074
	[AddComponentMenu("UI/Extensions/Menu Manager")]
	[DisallowMultipleComponent]
	public class MenuManager : MonoBehaviour
	{
		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x0600484A RID: 18506 RVA: 0x00171006 File Offset: 0x0016F406
		// (set) Token: 0x0600484B RID: 18507 RVA: 0x0017100D File Offset: 0x0016F40D
		public static MenuManager Instance { get; set; }

		// Token: 0x0600484C RID: 18508 RVA: 0x00171018 File Offset: 0x0016F418
		private void Awake()
		{
			MenuManager.Instance = this;
			if (this.MenuScreens.Length > this.StartScreen)
			{
				this.CreateInstance(this.MenuScreens[this.StartScreen].name);
				this.OpenMenu(this.MenuScreens[this.StartScreen]);
			}
			else
			{
				global::UnityEngine.Debug.LogError("Not enough Menu Screens configured");
			}
		}

		// Token: 0x0600484D RID: 18509 RVA: 0x00171078 File Offset: 0x0016F478
		private void OnDestroy()
		{
			MenuManager.Instance = null;
		}

		// Token: 0x0600484E RID: 18510 RVA: 0x00171080 File Offset: 0x0016F480
		public void CreateInstance<T>() where T : Menu
		{
			T prefab = this.GetPrefab<T>();
			Object.Instantiate<T>(prefab, base.transform);
		}

		// Token: 0x0600484F RID: 18511 RVA: 0x001710A4 File Offset: 0x0016F4A4
		public void CreateInstance(string MenuName)
		{
			GameObject prefab = this.GetPrefab(MenuName);
			Object.Instantiate<GameObject>(prefab, base.transform);
		}

		// Token: 0x06004850 RID: 18512 RVA: 0x001710C8 File Offset: 0x0016F4C8
		public void OpenMenu(Menu instance)
		{
			if (this.menuStack.Count > 0)
			{
				if (instance.DisableMenusUnderneath)
				{
					foreach (Menu menu in this.menuStack)
					{
						menu.gameObject.SetActive(false);
						if (menu.DisableMenusUnderneath)
						{
							break;
						}
					}
				}
				Canvas component = instance.GetComponent<Canvas>();
				Canvas component2 = this.menuStack.Peek().GetComponent<Canvas>();
				component.sortingOrder = component2.sortingOrder + 1;
			}
			this.menuStack.Push(instance);
		}

		// Token: 0x06004851 RID: 18513 RVA: 0x00171188 File Offset: 0x0016F588
		private GameObject GetPrefab(string PrefabName)
		{
			for (int i = 0; i < this.MenuScreens.Length; i++)
			{
				if (this.MenuScreens[i].name == PrefabName)
				{
					return this.MenuScreens[i].gameObject;
				}
			}
			throw new MissingReferenceException("Prefab not found for " + PrefabName);
		}

		// Token: 0x06004852 RID: 18514 RVA: 0x001711E4 File Offset: 0x0016F5E4
		private T GetPrefab<T>() where T : Menu
		{
			FieldInfo[] fields = base.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				T t = fieldInfo.GetValue(this) as T;
				if (t != null)
				{
					return t;
				}
			}
			throw new MissingReferenceException("Prefab not found for type " + typeof(T));
		}

		// Token: 0x06004853 RID: 18515 RVA: 0x0017125C File Offset: 0x0016F65C
		public void CloseMenu(Menu menu)
		{
			if (this.menuStack.Count == 0)
			{
				global::UnityEngine.Debug.LogErrorFormat(menu, "{0} cannot be closed because menu stack is empty", new object[]
				{
					menu.GetType()
				});
				return;
			}
			if (this.menuStack.Peek() != menu)
			{
				global::UnityEngine.Debug.LogErrorFormat(menu, "{0} cannot be closed because it is not on top of stack", new object[]
				{
					menu.GetType()
				});
				return;
			}
			this.CloseTopMenu();
		}

		// Token: 0x06004854 RID: 18516 RVA: 0x001712CC File Offset: 0x0016F6CC
		public void CloseTopMenu()
		{
			Menu menu = this.menuStack.Pop();
			if (menu.DestroyWhenClosed)
			{
				global::UnityEngine.Object.Destroy(menu.gameObject);
			}
			else
			{
				menu.gameObject.SetActive(false);
			}
			foreach (Menu menu2 in this.menuStack)
			{
				menu2.gameObject.SetActive(true);
				if (menu2.DisableMenusUnderneath)
				{
					break;
				}
			}
		}

		// Token: 0x06004855 RID: 18517 RVA: 0x00171370 File Offset: 0x0016F770
		private void Update()
		{
			if (global::UnityEngine.Input.GetKeyDown(KeyCode.Escape) && this.menuStack.Count > 0)
			{
				this.menuStack.Peek().OnBackPressed();
			}
		}

		// Token: 0x04006F16 RID: 28438
		public Menu[] MenuScreens;

		// Token: 0x04006F17 RID: 28439
		public int StartScreen;

		// Token: 0x04006F18 RID: 28440
		private Stack<Menu> menuStack = new Stack<Menu>();
	}
}
