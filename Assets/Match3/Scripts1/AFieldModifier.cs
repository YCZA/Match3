

// Token: 0x02000588 RID: 1416
namespace Match3.Scripts1
{
	public abstract class AFieldModifier : IFieldModifier
	{
		// Token: 0x06002520 RID: 9504 RVA: 0x000A5956 File Offset: 0x000A3D56
		public AFieldModifier(int count)
		{
			this.Count = count;
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06002521 RID: 9505 RVA: 0x000A5965 File Offset: 0x000A3D65
		// (set) Token: 0x06002522 RID: 9506 RVA: 0x000A596D File Offset: 0x000A3D6D
		public int Count { get; set; }
	}
}
