namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200070F RID: 1807
	public class EnumWithData<TEnum, TData>
	{
		// Token: 0x06002CCC RID: 11468 RVA: 0x000CFAD6 File Offset: 0x000CDED6
		public EnumWithData(TEnum value, TData data)
		{
			this.value = value;
			this.data = data;
		}

		// Token: 0x06002CCD RID: 11469 RVA: 0x000CFAEC File Offset: 0x000CDEEC
		public override string ToString()
		{
			return this.value.ToString();
		}

		// Token: 0x04005643 RID: 22083
		public TEnum value;

		// Token: 0x04005644 RID: 22084
		public TData data;
	}
}
