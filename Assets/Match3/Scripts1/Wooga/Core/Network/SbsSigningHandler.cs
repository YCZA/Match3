using System;
using System.Security.Cryptography;
using System.Text;

namespace Match3.Scripts1.Wooga.Core.Network
{
	// Token: 0x02000367 RID: 871
	public static class SbsSigningHandler
	{
		// Token: 0x06001A5B RID: 6747 RVA: 0x00076000 File Offset: 0x00074400
		public static string BuildAuthorizationHeader(string deviceId, string signature)
		{
			return string.Format("SBS {0}:{1}", deviceId, signature);
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x00076010 File Offset: 0x00074410
		public static string BuildSignature(string password, string stringToSign)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(stringToSign);
			byte[] hmacsha = SbsSigningHandler.GetHMACSHA1(password, bytes);
			return Convert.ToBase64String(hmacsha);
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x0007603C File Offset: 0x0007443C
		public static string BuildSignedResponseString(SbsResponse response, string requestSignature)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(SbsSigningHandler.NewLine(((int)response.StatusCode).ToString()));
			stringBuilder.Append(SbsSigningHandler.NewLine(SbsSigningHandler.GetMD5Hash(response.BodyString) ?? string.Empty));
			stringBuilder.Append(SbsSigningHandler.NewLine(response.ContentType ?? string.Empty));
			stringBuilder.Append(SbsSigningHandler.NewLine("x-sbs-date:" + response.Timestamp));
			stringBuilder.Append(requestSignature);
			return stringBuilder.ToString();
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x000760DC File Offset: 0x000744DC
		public static string BuildStringToSign(HttpMethod method, string contentInJson, string contentType, string sbsId, string userId, string path, DateTime timeStamp)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(SbsSigningHandler.NewLine(method.ToString()));
			stringBuilder.Append(SbsSigningHandler.NewLine(SbsSigningHandler.GetMD5Hash(contentInJson) ?? string.Empty));
			stringBuilder.Append(SbsSigningHandler.NewLine(contentType ?? string.Empty));
			stringBuilder.Append(SbsSigningHandler.NewLine("x-sbs-date:" + SbsSigningHandler.GetSbsDate(timeStamp)));
			stringBuilder.Append(SbsSigningHandler.NewLine("x-sbs-id:" + sbsId));
			stringBuilder.Append(SbsSigningHandler.NewLine("x-sbs-user-id:" + userId));
			stringBuilder.Append(path);
			return stringBuilder.ToString();
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x0007619C File Offset: 0x0007459C
		public static void SignRequest(SbsRequest request, DateTime timeStamp, string sbsId)
		{
			string text = (!string.IsNullOrEmpty(request.Body)) ? request.Body : null;
			string contentType = (!string.IsNullOrEmpty(text)) ? "application/json" : null;
			string text2 = request.Path;
			if (request.IsUriEscaped)
			{
				text2 = Uri.UnescapeDataString(text2);
			}
			string stringToSign = SbsSigningHandler.BuildStringToSign(request.Method, text, contentType, sbsId, request.UserContext.user_id, text2, timeStamp);
			string signature = SbsSigningHandler.BuildSignature(request.UserContext.password, stringToSign);
			string value = SbsSigningHandler.BuildAuthorizationHeader(request.UserContext.device_id, signature);
			request.Headers.Add("Authorization", value);
			request.Headers.Add("X-SBS-DATE", SbsSigningHandler.GetSbsDate(timeStamp));
			request.Headers.Add("X-SBS-USER-ID", request.UserContext.user_id);
			request.Signature = signature;
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x00076284 File Offset: 0x00074684
		public static void SignRequest(SbsRequest request, string sbsId)
		{
			SbsSigningHandler.SignRequest(request, request.Timestamp, sbsId);
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x00076294 File Offset: 0x00074694
		private static byte[] GetHMACSHA1(string password, byte[] bytes)
		{
			Encoding utf = Encoding.UTF8;
			HMACSHA1 hmacsha = new HMACSHA1(utf.GetBytes(password));
			hmacsha.Initialize();
			return hmacsha.ComputeHash(bytes);
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x000762C4 File Offset: 0x000746C4
		private static string GetMD5Hash(string source)
		{
			if (string.IsNullOrEmpty(source))
			{
				return null;
			}
			MD5 md = new MD5CryptoServiceProvider();
			byte[] bytes = Encoding.UTF8.GetBytes(source);
			byte[] value = md.ComputeHash(bytes);
			string text = BitConverter.ToString(value);
			return text.Replace("-", string.Empty).ToLower();
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x00076316 File Offset: 0x00074716
		private static string NewLine(string content)
		{
			return string.Format("{0}\n", content);
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x00076323 File Offset: 0x00074723
		private static string GetSbsDate(DateTime timeStamp)
		{
			return timeStamp.ToString("R").Replace("GMT", "-0000");
		}
	}
}
