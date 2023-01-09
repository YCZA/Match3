namespace Match3.Scripts1.Wooga.Services.Tracking.Tools.Migration
{
	// Token: 0x0200045E RID: 1118
	internal abstract class Migration
	{
		// Token: 0x0600207E RID: 8318
		public abstract string Purpose();

		// Token: 0x0600207F RID: 8319
		public abstract bool Required();

		// Token: 0x06002080 RID: 8320
		public abstract void Migrate();

		// Token: 0x06002081 RID: 8321 RVA: 0x00089BA0 File Offset: 0x00087FA0
		public void MigrateIfRequired()
		{
			if (this.Required())
			{
				this.Migrate();
			}
		}
	}
}
