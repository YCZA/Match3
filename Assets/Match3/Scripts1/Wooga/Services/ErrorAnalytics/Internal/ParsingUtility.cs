using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.ErrorAnalytics.Internal
{
	// Token: 0x020003F6 RID: 1014
	public static class ParsingUtility
	{
		// Token: 0x06001E5B RID: 7771 RVA: 0x00080BF0 File Offset: 0x0007EFF0
		public static List<ParsingUtility.StackTraceElement> ParseStackTraceElements(string stackTrace, ParsingUtility.ParseType type)
		{
			List<ParsingUtility.StackTraceElement> list = new List<ParsingUtility.StackTraceElement>();
			if (stackTrace == null)
			{
				return list;
			}
			Regex regex = null;
			if (type != ParsingUtility.ParseType.EnvironmentStackTrace)
			{
				if (type != ParsingUtility.ParseType.LogStackTrace)
				{
					if (type == ParsingUtility.ParseType.UnhandledExceptionStackTrace)
					{
						regex = ParsingUtility.regexGenericStackTrace;
					}
				}
				else
				{
					regex = ParsingUtility.regexGenericStackTrace;
				}
			}
			else
			{
				regex = ParsingUtility.regexEnvironmentStackTrace;
			}
			foreach (string input in stackTrace.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
			{
				System.Text.RegularExpressions.Match match = regex.Match(input);
				if (match.Success)
				{
					ParsingUtility.StackTraceElement stackTraceElement = new ParsingUtility.StackTraceElement();
					stackTraceElement.file = match.Groups["file"].Value;
					if (match.Groups["line"].Value != string.Empty)
					{
						stackTraceElement.lineNumber = int.Parse(match.Groups["line"].Value);
					}
					stackTraceElement.method = match.Groups["method"].Value;
					list.Add(stackTraceElement);
				}
			}
			return list;
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x00080D1C File Offset: 0x0007F11C
		public static ParsedException ParseExceptionLog(string exceptionLog, ErrorAnalytics.LogSeverity severity)
		{
			ParsedException ex = new ParsedException();
			System.Text.RegularExpressions.Match match = ParsingUtility.exceptionRegEx.Match(exceptionLog);
			if (match.Success)
			{
				ex.errorClass = match.Groups["errorClass"].Value;
				ex.errorMessage = match.Groups["message"].Value.Trim();
			}
			else if (exceptionLog.Length < 200)
			{
				ex.errorClass = exceptionLog;
			}
			else
			{
				ex.errorClass = severity.ToString();
				ex.errorMessage = exceptionLog;
			}
			ex.severity = severity;
			return ex;
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x00080DC4 File Offset: 0x0007F1C4
		private static void AddMetaData(ParsedException ex)
		{
			Regex regex = new Regex("\\[\\[(?<type>.*)\\]\\](?<json>{.*})");
			System.Text.RegularExpressions.Match match = regex.Match(ex.errorMessage);
			if (match.Success)
			{
				ex.errorMessage = "for full exception see metadata";
				Type type = Type.GetType(match.Groups["type"].Value);
				object data = JsonUtility.FromJson(match.Groups["json"].Value, type);
				ex.data = data;
			}
		}

		// Token: 0x04004A30 RID: 18992
		private static readonly Regex exceptionRegEx = new Regex("^(?<errorClass>.+?):\\s*(?<message>.*)");

		// Token: 0x04004A31 RID: 18993
		private static readonly Regex regexEnvironmentStackTrace = new Regex("\\s*at (?<method>.+\\([^\\)]*\\))(.* in (?<file>.\\S+):(line )?(?<line>\\d+))?");

		// Token: 0x04004A32 RID: 18994
		private static readonly Regex regexGenericStackTrace = new Regex("^(?<method>\\S+\\s*\\((?!at )[^\\)]*\\))(\\s*(\\(at (?<file>.+):(?<line>\\d+)\\)))?");

		// Token: 0x020003F7 RID: 1015
		public enum ParseType
		{
			// Token: 0x04004A34 RID: 18996
			EnvironmentStackTrace,
			// Token: 0x04004A35 RID: 18997
			LogStackTrace,
			// Token: 0x04004A36 RID: 18998
			UnhandledExceptionStackTrace,
			// Token: 0x04004A37 RID: 18999
			Exception
		}

		// Token: 0x020003F8 RID: 1016
		public class StackTraceElement
		{
			// Token: 0x06001E5F RID: 7775 RVA: 0x00080E6C File Offset: 0x0007F26C
			public StackTraceElement(string file, int lineNumber, int columnNumber, string method, bool inProject)
			{
				this.file = file;
				this.lineNumber = lineNumber;
				this.columnNumber = columnNumber;
				this.method = method;
				this.inProject = inProject;
			}

			// Token: 0x06001E60 RID: 7776 RVA: 0x00080EA7 File Offset: 0x0007F2A7
			public StackTraceElement()
			{
			}

			// Token: 0x06001E61 RID: 7777 RVA: 0x00080EC0 File Offset: 0x0007F2C0
			public override string ToString()
			{
				return string.Format("StackTraceElement. File: {0}, ln: {1}, cl: {2}, method: {3}, inProject: {4}", new object[]
				{
					this.file,
					this.lineNumber,
					this.columnNumber,
					this.method,
					this.inProject
				});
			}

			// Token: 0x06001E62 RID: 7778 RVA: 0x00080F19 File Offset: 0x0007F319
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x06001E63 RID: 7779 RVA: 0x00080F24 File Offset: 0x0007F324
			public override bool Equals(object obj)
			{
				if (!(obj is ParsingUtility.StackTraceElement))
				{
					return false;
				}
				ParsingUtility.StackTraceElement stackTraceElement = obj as ParsingUtility.StackTraceElement;
				return this.file == stackTraceElement.file && this.lineNumber == stackTraceElement.lineNumber && this.columnNumber == stackTraceElement.columnNumber && this.method == stackTraceElement.method && this.inProject == stackTraceElement.inProject;
			}

			// Token: 0x04004A38 RID: 19000
			public int columnNumber = -1;

			// Token: 0x04004A39 RID: 19001
			public string file;

			// Token: 0x04004A3A RID: 19002
			public bool inProject;

			// Token: 0x04004A3B RID: 19003
			public int lineNumber = -1;

			// Token: 0x04004A3C RID: 19004
			public string method;
		}
	}
}
