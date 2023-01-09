using Match3.Scripts1.Wooga.Core.Data;

namespace Wooga.Core.Storage
{
	// Token: 0x02000393 RID: 915
	public interface ISbsStorage
	{
		// Token: 0x06001BCA RID: 7114
		SbsRichData Load(string name);

		// Token: 0x06001BCB RID: 7115
		void Save(string name, string contents, int format_version);

		// Token: 0x06001BCC RID: 7116
		void Save(string name, SbsRichData data);

		// Token: 0x06001BCD RID: 7117
		void Delete(string name);

		// Token: 0x06001BCE RID: 7118
		bool Exists(string name);
	}
}
