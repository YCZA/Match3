using System;
using Match3.Scripts1.Puzzletown.Services;

namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x02000480 RID: 1152
	[Serializable]
	public class ExceptionsConfig
	{
		// Token: 0x04004BD6 RID: 19414
		public ExceptionHandlerService.ExceptionIdentifier[] whitelist;

		// Token: 0x04004BD7 RID: 19415
		public ExceptionHandlerFlow.Rule[] handlerRules;
	}
}
