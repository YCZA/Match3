using System;
using System.Collections.Generic;
using Match3.Scripts1.HSMiniJSON;

namespace Match3.Scripts1.Helpshift
{
	// Token: 0x020001D9 RID: 473
	public static class HelpshiftJSONUtility
	{
		// Token: 0x06000DF5 RID: 3573 RVA: 0x00020BF8 File Offset: 0x0001EFF8
		public static HelpshiftUser getHelpshiftUser(string serializedJSONHelpshiftUser)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(serializedJSONHelpshiftUser);
			string identifier = Convert.ToString(dictionary["identifier"]);
			string email = Convert.ToString(dictionary["email"]);
			string authToken = Convert.ToString(dictionary["authToken"]);
			string name = Convert.ToString(dictionary["name"]);
			HelpshiftUser.Builder builder = new HelpshiftUser.Builder(identifier, email);
			builder.setName(name);
			builder.setAuthToken(authToken);
			return builder.build();
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x00020C7C File Offset: 0x0001F07C
		public static HelpshiftAuthFailureReason getAuthFailureReason(string serializedJSONAuthFailure)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(serializedJSONAuthFailure);
			string value = Convert.ToString(dictionary["authFailureReason"]);
			HelpshiftAuthFailureReason result = HelpshiftAuthFailureReason.INVALID_AUTH_TOKEN;
			if ("0".Equals(value))
			{
				result = HelpshiftAuthFailureReason.AUTH_TOKEN_NOT_PROVIDED;
			}
			else if ("1".Equals(value))
			{
				result = HelpshiftAuthFailureReason.INVALID_AUTH_TOKEN;
			}
			return result;
		}
	}
}
