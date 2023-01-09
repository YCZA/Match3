using System.Collections;

namespace Match3.Scripts1.Shared.M3Engine
{
	// Token: 0x02000AF7 RID: 2807
	public interface IFieldMap<TField> : IEnumerable
	{
		// Token: 0x0600427B RID: 17019
		bool IsSwapPossible(IntVector2 from, IntVector2 to);

		// Token: 0x0600427C RID: 17020
		void RemoveGroups(IGemFactory gemFactory);

		// Token: 0x0600427D RID: 17021
		bool IsValid(IntVector2 pos);

		// Token: 0x17000993 RID: 2451
		TField this[IntVector2 index]
		{
			get;
			set;
		}
	}
}
