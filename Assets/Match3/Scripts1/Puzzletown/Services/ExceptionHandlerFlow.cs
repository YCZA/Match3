using System;
using System.Collections;
using System.Threading;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Shared.Flows;
using Match3.Scripts1.Shared.UI;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics.Internal;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.Env;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200076D RID: 1901
	public class ExceptionHandlerFlow : AFlow<ExceptionHandlerFlow.Input>
	{
		// Token: 0x06002F2D RID: 12077 RVA: 0x000DC744 File Offset: 0x000DAB44
		protected override IEnumerator FlowRoutine(ExceptionHandlerFlow.Input input)
		{
			yield return ServiceLocator.Instance.Inject(this);
			this.isRunning = true;
			ExceptionHandlerFlow.Rule.ActionType aType = input.rule.actionType;
			string title = this.loca.GetText(ExceptionHandlerFlow.PopupTitle(aType), new LocaParam[0]);
			string message = this.loca.GetText(ExceptionHandlerFlow.PopupMessage(aType), new LocaParam[0]);
			if (GameEnvironment.CurrentEnvironment == GameEnvironment.Environment.PRODUCTION)
			{
				title = input.exception.errorClass;
				message = input.exception.errorMessage;
			}
			string errorMessageShort = input.exception.errorMessage;
			if (errorMessageShort.Length > 40)
			{
				errorMessageShort = errorMessageShort.Substring(0, 40);
			}
			this.trackingService.TrackUi("Exception", aType.ToString(), "open", string.Empty, new object[]
			{
				input.exception.errorClass,
				errorMessageShort
			});
			PopupSortingOrder topLayerSortingOrder = new PopupSortingOrder(UILayer.Top);
			Coroutine dialog = PopupDialogRoot.ShowOkDialog(title, message, null, topLayerSortingOrder);
			yield return dialog;
			this.trackingService.TrackUi("Exception", aType.ToString(), "close", string.Empty, new object[]
			{
				input.exception.errorClass,
				errorMessageShort
			});
			if (aType == ExceptionHandlerFlow.Rule.ActionType.Quit)
			{
				Thread.Sleep(500);
				Application.Quit();
			}
			else
			{
				input.onRestart.Dispatch();
				PTReloader.ReloadGame(this.ReloadReason(input.exception), false);
			}
			this.isRunning = false;
			yield return null;
			yield break;
		}

		// Token: 0x06002F2E RID: 12078 RVA: 0x000DC766 File Offset: 0x000DAB66
		private static string PopupTitle(ExceptionHandlerFlow.Rule.ActionType aType)
		{
			return (aType != ExceptionHandlerFlow.Rule.ActionType.Restart) ? "error.popup.title" : "error.popup.title";
		}

		// Token: 0x06002F2F RID: 12079 RVA: 0x000DC77D File Offset: 0x000DAB7D
		private static string PopupMessage(ExceptionHandlerFlow.Rule.ActionType aType)
		{
			return (aType != ExceptionHandlerFlow.Rule.ActionType.Restart) ? "error.popup.message_restart" : "error.popup.message";
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x000DC794 File Offset: 0x000DAB94
		private string ReloadReason(ParsedException exception)
		{
			string str = (!exception.stackTrace.IsNullOrEmpty()) ? exception.stackTrace.Substring(0, Mathf.Min(100, exception.stackTrace.Length)) : string.Empty;
			return exception.errorMessage + " / " + str;
		}

		// Token: 0x04005844 RID: 22596
		[WaitForService(false, true)]
		private ILocalizationService loca;

		// Token: 0x04005845 RID: 22597
		[WaitForService(false, true)]
		private TrackingService trackingService;

		// Token: 0x04005846 RID: 22598
		public bool isRunning;

		// Token: 0x0200076E RID: 1902
		[Serializable]
		public class Rule
		{
			// Token: 0x06002F32 RID: 12082 RVA: 0x000DC7F5 File Offset: 0x000DABF5
			public override string ToString()
			{
				return string.Format("||Name: {0}, Message: {1}, ActionType: {2}", this.name, this.message, this.actionType.ToString());
			}

			// Token: 0x06002F33 RID: 12083 RVA: 0x000DC820 File Offset: 0x000DAC20
			public bool Match(ParsedException exception)
			{
				return (string.IsNullOrEmpty(this.name) || this.name.Equals(exception.errorClass)) && (string.IsNullOrEmpty(this.message) || exception.errorMessage.Contains(this.message));
			}

			// Token: 0x04005847 RID: 22599
			public string name;

			// Token: 0x04005848 RID: 22600
			public string message;

			// Token: 0x04005849 RID: 22601
			public ExceptionHandlerFlow.Rule.ActionType actionType;

			// Token: 0x0200076F RID: 1903
			public enum ActionType
			{
				// Token: 0x0400584B RID: 22603
				Restart,
				// Token: 0x0400584C RID: 22604
				Quit
			}
		}

		// Token: 0x02000770 RID: 1904
		public class Input
		{
			// Token: 0x06002F34 RID: 12084 RVA: 0x000DC87E File Offset: 0x000DAC7E
			public Input(ParsedException exception, Signal onRestart, ExceptionHandlerFlow.Rule rule)
			{
				this.exception = exception;
				this.onRestart = onRestart;
				this.rule = rule;
			}

			// Token: 0x0400584D RID: 22605
			public ParsedException exception;

			// Token: 0x0400584E RID: 22606
			public Signal onRestart;

			// Token: 0x0400584F RID: 22607
			public ExceptionHandlerFlow.Rule rule;
		}
	}
}
