using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Wooga.Coroutines;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Localization.Runtime
{
	// Token: 0x02000AF2 RID: 2802
	public class PTTextLocalizer : ATextMeshProLocalizer
	{
		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x0600423F RID: 16959 RVA: 0x00155394 File Offset: 0x00153794
		public override CultureInfo CultureInfo
		{
			get
			{
				return this._localizationService.GetCultureInfo();
			}
		}

		// Token: 0x06004240 RID: 16960 RVA: 0x001553A1 File Offset: 0x001537A1
		protected override void Start()
		{
			WooroutineRunner.StartCoroutine(this.StartRoutine(), null);
		}

		// Token: 0x06004241 RID: 16961 RVA: 0x001553B0 File Offset: 0x001537B0
		private IEnumerator StartRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			base.Start();
			yield break;
		}

		// Token: 0x06004242 RID: 16962 RVA: 0x001553CC File Offset: 0x001537CC
		protected override string GetText(string key)
		{
			string text = this._localizationService.GetText(key, new LocaParam[0]);
			if (base.upperCase)
			{
				text = text.ToLocalUpper(this.CultureInfo);
			}
			return text;
		}

		// Token: 0x06004243 RID: 16963 RVA: 0x00155405 File Offset: 0x00153805
		public override List<string> GetAvailableKeys()
		{
			return this.GetAvailableKeys(this._localizationService);
		}

		// Token: 0x06004244 RID: 16964 RVA: 0x00155413 File Offset: 0x00153813
		public override List<string> GetAvailableKeys(ILocalizationService localizationService)
		{
			return localizationService.LocaleKeys;
		}

		// Token: 0x06004245 RID: 16965 RVA: 0x0015541B File Offset: 0x0015381B
		protected override void AddLanguageChangedListener()
		{
			this._localizationService.LanguageChanged.AddListener(new Action(base.Refresh));
		}

		// Token: 0x06004246 RID: 16966 RVA: 0x00155439 File Offset: 0x00153839
		protected override void RemoveLanguageChangedListener()
		{
			if (this._localizationService != null)
			{
				this._localizationService.LanguageChanged.RemoveListener(new Action(base.Refresh));
			}
		}

		// Token: 0x04006B52 RID: 27474
		[WaitForService(false, true)]
		private ILocalizationService _localizationService;
	}
}
