namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C00 RID: 3072
	public abstract class Menu<T> : Menu where T : Menu<T>
	{
		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x06004840 RID: 18496 RVA: 0x00170EFF File Offset: 0x0016F2FF
		// (set) Token: 0x06004841 RID: 18497 RVA: 0x00170F06 File Offset: 0x0016F306
		public static T Instance { get; private set; }

		// Token: 0x06004842 RID: 18498 RVA: 0x00170F0E File Offset: 0x0016F30E
		protected virtual void Awake()
		{
			Menu<T>.Instance = (T)((object)this);
		}

		// Token: 0x06004843 RID: 18499 RVA: 0x00170F1B File Offset: 0x0016F31B
		protected virtual void OnDestroy()
		{
			Menu<T>.Instance = (T)((object)null);
		}

		// Token: 0x06004844 RID: 18500 RVA: 0x00170F28 File Offset: 0x0016F328
		protected static void Open()
		{
			if (Menu<T>.Instance == null)
			{
				MenuManager.Instance.CreateInstance(typeof(T).Name);
			}
			else
			{
				T instance = Menu<T>.Instance;
				instance.gameObject.SetActive(true);
			}
			MenuManager.Instance.OpenMenu(Menu<T>.Instance);
		}

		// Token: 0x06004845 RID: 18501 RVA: 0x00170F98 File Offset: 0x0016F398
		protected static void Close()
		{
			if (Menu<T>.Instance == null)
			{
				global::UnityEngine.Debug.LogErrorFormat("Trying to close menu {0} but Instance is null", new object[]
				{
					typeof(T)
				});
				return;
			}
			MenuManager.Instance.CloseMenu(Menu<T>.Instance);
		}

		// Token: 0x06004846 RID: 18502 RVA: 0x00170FEC File Offset: 0x0016F3EC
		public override void OnBackPressed()
		{
			Menu<T>.Close();
		}
	}
}
