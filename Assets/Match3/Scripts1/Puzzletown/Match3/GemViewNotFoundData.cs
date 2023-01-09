using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Match3.Scripts1.Wooga.Services;
using UnityEngine;
using Wooga.Foundation.Json;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006B1 RID: 1713
	[Serializable]
	public class GemViewNotFoundData
	{
		// Token: 0x06002ACF RID: 10959 RVA: 0x000C3F8F File Offset: 0x000C238F
		public GemViewNotFoundData()
		{
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x000C3F98 File Offset: 0x000C2398
		public GemViewNotFoundData(IntVector2 position, SerializableFields fields, Move move, Gem gem, int level, int tier)
		{
			this.position = position;
			this.fields = fields;
			this.move = move;
			this.gem = gem;
			this.level = level;
			this.tier = tier;
			if (fields != null)
			{
				this.CreateGist();
			}
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x000C3FE4 File Offset: 0x000C23E4
		private void CreateGist()
		{
			try
			{
				ServicePointManager.ServerCertificateValidationCallback = ((object xsender, X509Certificate xcertificate, X509Chain xchain, SslPolicyErrors xerrors) => true);
				string user_id = SBS.Authentication.GetUserContext().user_id;
				Dictionary<string, object> o = new Dictionary<string, object>
				{
					{
						"public",
						false
					},
					{
						"files",
						new Dictionary<string, object>
						{
							{
								user_id,
								new Dictionary<string, object>
								{
									{
										"content",
										JsonUtility.ToJson(this.fields)
									}
								}
							}
						}
					}
				};
				string data = JSON.Serialize(o, false, 1, ' ');
				using (WebClient webClient = new WebClient())
				{
					webClient.Headers["User-Agent"] = "puzzletown-production";
					// string text = webClient.UploadString("https://api.github.com/gists", data);
					string text = webClient.UploadString("host2333", data);
					JSONNode jsonnode = JSON.Deserialize(text);
					WoogaDebug.Log(new object[]
					{
						text
					});
					this.gist = jsonnode["html_url"].ToString();
					WoogaDebug.Log(new object[]
					{
						this.gist
					});
				}
			}
			catch
			{
			}
		}

		// Token: 0x0400541C RID: 21532
		public IntVector2 position;

		// Token: 0x0400541D RID: 21533
		public string gist;

		// Token: 0x0400541E RID: 21534
		public Move move;

		// Token: 0x0400541F RID: 21535
		public Gem gem;

		// Token: 0x04005420 RID: 21536
		public int level;

		// Token: 0x04005421 RID: 21537
		public int tier;

		// Token: 0x04005422 RID: 21538
		[NonSerialized]
		public SerializableFields fields;
	}
}
