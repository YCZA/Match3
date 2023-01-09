using System;

namespace Wooga.UnityFramework
{
	// Token: 0x02000B5F RID: 2911
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class LoadOptions : Attribute
	{
		// Token: 0x06004417 RID: 17431 RVA: 0x0015ABB6 File Offset: 0x00158FB6
		public LoadOptions(bool isAdditive, bool returnExistingScene, bool awaitInitialization)
		{
			this.loadAdditively = isAdditive;
			this.returnExisting = returnExistingScene;
			this.awaitInitialization = awaitInitialization;
		}

		// Token: 0x06004418 RID: 17432 RVA: 0x0015ABD3 File Offset: 0x00158FD3
		public override string ToString()
		{
			return string.Format("[SceneLoadOptions: loadAdditively={0}, returnExisting={1}, awaitInitialization={2}]", this.loadAdditively, this.returnExisting, this.awaitInitialization);
		}

		// Token: 0x04006C60 RID: 27744
		public bool loadAdditively;

		// Token: 0x04006C61 RID: 27745
		public bool returnExisting;

		// Token: 0x04006C62 RID: 27746
		public bool awaitInitialization;
	}
}
