namespace Match3.Scripts1.Localization
{
	// Token: 0x02000AE7 RID: 2791
	public class Validator
	{
		// Token: 0x060041FF RID: 16895 RVA: 0x00153F6F File Offset: 0x0015236F
		public Validator(ILocalizationProcessor[] processors, ILocalizationValidator[] validators)
		{
			this._processors = processors;
			this._validators = validators;
		}

		// Token: 0x06004200 RID: 16896 RVA: 0x00153F88 File Offset: 0x00152388
		public bool ProcessLocale(Locale locale)
		{
			for (int i = 0; i < locale.Entries.Length; i++)
			{
				foreach (ILocalizationProcessor localizationProcessor in this._processors)
				{
					locale.Entries[i].val = localizationProcessor.ProcessString(locale.Entries[i].val);
				}
			}
			bool result = true;
			foreach (ILocalizationValidator localizationValidator in this._validators)
			{
				if (!localizationValidator.Validate(locale))
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x04006B09 RID: 27401
		private readonly ILocalizationProcessor[] _processors;

		// Token: 0x04006B0A RID: 27402
		private readonly ILocalizationValidator[] _validators;
	}
}
