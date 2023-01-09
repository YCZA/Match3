using System.Diagnostics;
using Wooga.Core.Utilities;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager.Internal
{
	// Token: 0x0200030F RID: 783
	public static class Tracer
	{
		// Token: 0x06001883 RID: 6275 RVA: 0x0006FADD File Offset: 0x0006DEDD
		[Conditional("ASSETBUNDES_TRACE")]
		public static void Trace(string message, params object[] objects)
		{
			Log.DebugFormatted(message, objects);
		}
	}
}
