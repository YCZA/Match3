using System.Collections.Generic;
using System.Globalization;
using Match3.Scripts1.UnityEngine;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Localization.Runtime
{
	// Token: 0x02000AF0 RID: 2800
	[DisallowMultipleComponent]
	public abstract class ATextLocalizer : MonoBehaviour
	{
		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x06004229 RID: 16937 RVA: 0x00154FB9 File Offset: 0x001533B9
		// (set) Token: 0x0600422A RID: 16938 RVA: 0x00154FC1 File Offset: 0x001533C1
		public bool upperCase
		{
			get
			{
				return this._upperCase;
			}
			set
			{
				this._upperCase = value;
				this.Refresh();
			}
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x0600422B RID: 16939
		public abstract CultureInfo CultureInfo { get; }

		// Token: 0x0600422C RID: 16940 RVA: 0x00154FD0 File Offset: 0x001533D0
		public void SetKey(string key)
		{
			this.key = key;
			this.Refresh();
		}

		// Token: 0x0600422D RID: 16941 RVA: 0x00154FDF File Offset: 0x001533DF
		public string GetKey()
		{
			return this.key;
		}

		// Token: 0x0600422E RID: 16942
		protected abstract string GetText(string key);

		// Token: 0x0600422F RID: 16943
		public abstract List<string> GetAvailableKeys();

		// Token: 0x06004230 RID: 16944
		public abstract List<string> GetAvailableKeys(ILocalizationService localizationService);

		// Token: 0x06004231 RID: 16945 RVA: 0x00154FE7 File Offset: 0x001533E7
		protected virtual void AddLanguageChangedListener()
		{
		}

		// Token: 0x06004232 RID: 16946 RVA: 0x00154FE9 File Offset: 0x001533E9
		protected virtual void RemoveLanguageChangedListener()
		{
		}

		// Token: 0x06004233 RID: 16947 RVA: 0x00154FEC File Offset: 0x001533EC
		protected virtual void Start()
		{
			this.Refresh();
			this.AddLanguageChangedListener();
			if (string.IsNullOrEmpty(this.key))
			{
				ASceneRoot componentInParent = base.GetComponentInParent<ASceneRoot>();
				if (componentInParent != null)
				{
					WoogaDebug.LogWarningFormatted("Missing Loca Key. Root: {0} - Object: {1}", new object[]
					{
						componentInParent.gameObject.name,
						base.gameObject.name
					});
				}
				else
				{
					WoogaDebug.LogWarningFormatted("Missing Loca Key. - Object: {0}", new object[]
					{
						base.gameObject.transform.GetGameObjectPath()
					});
				}
				global::UnityEngine.Object.Destroy(this);
			}
		}

		// Token: 0x06004234 RID: 16948 RVA: 0x00155082 File Offset: 0x00153482
		protected void OnDestroy()
		{
			this.RemoveLanguageChangedListener();
		}

		// Token: 0x06004235 RID: 16949
		protected abstract void SetLocalizedText(string localizedText);

		// Token: 0x06004236 RID: 16950 RVA: 0x0015508C File Offset: 0x0015348C
		public void Refresh()
		{
			if (!string.IsNullOrEmpty(this.key))
			{
				string text = this.GetText(this.key);
				if (this.upperCase)
				{
					text = text.ToLocalUpper(this.CultureInfo);
				}
				this.SetLocalizedText(text);
			}
		}

		// Token: 0x04006B4C RID: 27468
		[SerializeField]
		// [HideInInspector]
		private bool _upperCase;

		// Token: 0x04006B4D RID: 27469
		[SerializeField]
		// [HideInInspector]
		protected string key;
	}
}
