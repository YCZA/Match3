using Wooga.Core.Storage;
using Match3.Scripts1.Wooga.Core.ThreadSafe;

namespace Wooga.Core.Utilities
{
	// Token: 0x020003B9 RID: 953
	public static class Util
	{
		// Token: 0x06001CBF RID: 7359 RVA: 0x0007D548 File Offset: 0x0007B948
		public static ISbsStorage Storage()
		{
			Unity3D.Init();
			ISbsStorage result;
			if (Unity3D.Paths.ApplicationDataPath != Unity3D.Paths.PersistentDataPath)
			{
				SbsFileStorage storage = new SbsFileStorage();
				result = new SbsMigratingStorage(new SbsBasePathStorage(Unity3D.Paths.ApplicationDataPath, storage), new SbsBasePathStorage(Unity3D.Paths.PersistentDataPath, storage));
			}
			else
			{
				result = new SbsBasePathStorage(Unity3D.Paths.PersistentDataPath, new SbsFileStorage());
			}
			return result;
		}
	}
}
