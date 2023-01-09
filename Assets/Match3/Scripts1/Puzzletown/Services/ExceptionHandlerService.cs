using System;
using System.Collections.Generic;
using System.IO;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics.Internal;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200076B RID: 1899
	public class ExceptionHandlerService : AService
	{
		// Token: 0x06002F24 RID: 12068 RVA: 0x000DC414 File Offset: 0x000DA814
		public ExceptionHandlerService()
		{
			ErrorAnalytics.SetExceptionCallback(new Action<ParsedException>(this.HandleException));
			ErrorAnalytics.RegisterExceptionFilter(new Predicate<ParsedException>(this.FilterException));
			base.OnInitialized.Dispatch();
		}

		// Token: 0x06002F25 RID: 12069 RVA: 0x000DC4AB File Offset: 0x000DA8AB
		public void Configure(ExceptionsConfig config)
		{
			this.Whitelist.AddRange(config.whitelist);
			if (config.handlerRules != null)
			{
				this.HandlerRules.AddRange(config.handlerRules);
			}
		}

		// Token: 0x06002F26 RID: 12070 RVA: 0x000DC4DC File Offset: 0x000DA8DC
		private void HandleException(ParsedException exception)
		{
			if (!this.flow.isRunning && this.GetExceptionIdentifier(exception) == null && !exception.errorClass.StartsWith("[Ignored]"))
			{
				ExceptionHandlerFlow.Rule exceptionRule = this.GetExceptionRule(exception);
				this.flow.Start(new ExceptionHandlerFlow.Input(exception, this.onRestart, exceptionRule));
			}
			else
			{
				Handler.hadFatal = false;
			}
			File.WriteAllText(SessionService.CRASH_MARKER_PATH, "application crashed");
		}

		// Token: 0x06002F27 RID: 12071 RVA: 0x000DC558 File Offset: 0x000DA958
		public ExceptionHandlerService.ExceptionIdentifier GetExceptionIdentifier(ParsedException exception)
		{
			int i = 0;
			int count = this.Whitelist.Count;
			while (i < count)
			{
				ExceptionHandlerService.ExceptionIdentifier exceptionIdentifier = this.Whitelist[i];
				if (!exceptionIdentifier.Name.IsNullOrEmpty())
				{
					if (exceptionIdentifier.RequireExactMatch)
					{
						if (exceptionIdentifier.Name != exception.errorClass)
						{
							goto IL_C9;
						}
					}
					else if (!exception.errorClass.Contains(exceptionIdentifier.Name))
					{
						goto IL_C9;
					}
					if (string.IsNullOrEmpty(exceptionIdentifier.Message) || !(exceptionIdentifier.Message != exception.errorMessage))
					{
						if (exceptionIdentifier.CallstackFragment == null || exception.stackTrace.Contains(exceptionIdentifier.CallstackFragment))
						{
							return exceptionIdentifier;
						}
					}
				}
				IL_C9:
				i++;
			}
			return null;
		}

		// Token: 0x06002F28 RID: 12072 RVA: 0x000DC63C File Offset: 0x000DAA3C
		private ExceptionHandlerFlow.Rule GetExceptionRule(ParsedException exception)
		{
			for (int i = 0; i < this.HandlerRules.Count; i++)
			{
				ExceptionHandlerFlow.Rule rule = this.HandlerRules[i];
				if (rule.Match(exception))
				{
					return rule;
				}
			}
			return new ExceptionHandlerFlow.Rule();
		}

		// Token: 0x06002F29 RID: 12073 RVA: 0x000DC688 File Offset: 0x000DAA88
		private bool FilterException(ParsedException ex)
		{
			ExceptionHandlerService.ExceptionIdentifier exceptionIdentifier = this.GetExceptionIdentifier(ex);
			if (exceptionIdentifier == null)
			{
				return false;
			}
			if (exceptionIdentifier.Report)
			{
				ex.errorClass = "[Ignored]" + ex.errorClass;
			}
			return !exceptionIdentifier.Report;
		}

		// Token: 0x0400583A RID: 22586
		private const string PREFIX_IGNORED = "[Ignored]";

		// Token: 0x0400583B RID: 22587
		public readonly Signal onRestart = new Signal();

		// Token: 0x0400583C RID: 22588
		private ExceptionHandlerFlow flow = new ExceptionHandlerFlow();

		// Token: 0x0400583D RID: 22589
		private List<ExceptionHandlerService.ExceptionIdentifier> Whitelist = new List<ExceptionHandlerService.ExceptionIdentifier>
		{
			new ExceptionHandlerService.ExceptionIdentifier
			{
				Name = typeof(GemViewNotFoundException).Name,
				CallstackFragment = null
			}
		};

		// Token: 0x0400583E RID: 22590
		private List<ExceptionHandlerFlow.Rule> HandlerRules = new List<ExceptionHandlerFlow.Rule>();

		// Token: 0x0200076C RID: 1900
		[Serializable]
		public class ExceptionIdentifier
		{
			// Token: 0x06002F2B RID: 12075 RVA: 0x000DC6E8 File Offset: 0x000DAAE8
			public override string ToString()
			{
				return string.Format("||Name: {0}, Message: {1}, CallstackFragment: {2}, Report: {3}, RequireExactMatch: {4}||", new object[]
				{
					this.Name,
					this.Message,
					this.CallstackFragment,
					this.Report,
					this.RequireExactMatch
				});
			}

			// Token: 0x0400583F RID: 22591
			public string Name;

			// Token: 0x04005840 RID: 22592
			public bool RequireExactMatch = true;

			// Token: 0x04005841 RID: 22593
			public string Message;

			// Token: 0x04005842 RID: 22594
			public string CallstackFragment;

			// Token: 0x04005843 RID: 22595
			public bool Report = true;
		}
	}
}
