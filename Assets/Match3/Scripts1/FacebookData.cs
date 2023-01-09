using System;
using System.Collections.Generic;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using UnityEngine;
using Wooga.Foundation.Json;

// Token: 0x0200077B RID: 1915
namespace Match3.Scripts1
{
	public static class FacebookData
	{
		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06002F50 RID: 12112 RVA: 0x000DD846 File Offset: 0x000DBC46
		// (set) Token: 0x06002F51 RID: 12113 RVA: 0x000DD84D File Offset: 0x000DBC4D
		public static List<FacebookData.AppRequestsResponse.Request> invalidRequests { get; private set; }

		// Token: 0x06002F52 RID: 12114 RVA: 0x000DD858 File Offset: 0x000DBC58
		public static List<FacebookData.Friend> GetFriends(string responseStr)
		{
			FacebookData.FriendListResponse friendListResponse = null;
			try
			{
				friendListResponse = JSON.Deserialize<FacebookData.FriendListResponse>(responseStr);
			}
			catch (Exception ex)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Error deserializing FB response: ",
					ex
				});
				return FacebookData.noFriends;
			}
			if (friendListResponse == null || friendListResponse.data == null)
			{
				return FacebookData.noFriends;
			}
			List<FacebookData.Friend> list = new List<FacebookData.Friend>();
			foreach (FacebookData.FriendListResponse.Friend friend in friendListResponse.data)
			{
				list.Add(new FacebookData.Friend
				{
					ID = friend.id,
					Name = friend.first_name,
					Picture = friend.picture
				});
			}
			return list;
		}

		// Token: 0x06002F53 RID: 12115 RVA: 0x000DD928 File Offset: 0x000DBD28
		public static int GetFriendCount(string responseStr)
		{
			FacebookData.FriendSummary friendSummary = null;
			try
			{
				friendSummary = JSON.Deserialize<FacebookData.FriendSummary>(responseStr);
			}
			catch (Exception ex)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Error deserializing FB response: ",
					ex
				});
				return -1;
			}
			if (friendSummary != null)
			{
				return friendSummary.summary.total_count;
			}
			return -1;
		}

		// Token: 0x06002F54 RID: 12116 RVA: 0x000DD988 File Offset: 0x000DBD88
		public static List<FacebookData.Request> GetRequests(string responseStr)
		{
			FacebookData.AppRequestsResponse appRequestsResponse = null;
			try
			{
				appRequestsResponse = JSON.Deserialize<FacebookData.AppRequestsResponse>(responseStr);
			}
			catch (Exception ex)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Error deserializing FB response: ",
					ex
				});
				return FacebookData.noRequests;
			}
			if (appRequestsResponse == null || appRequestsResponse.data == null)
			{
				WoogaDebug.Log(new object[]
				{
					"Null response or response data"
				});
				return FacebookData.noRequests;
			}
			List<FacebookData.Request> list = new List<FacebookData.Request>();
			foreach (FacebookData.AppRequestsResponse.Request request in appRequestsResponse.data)
			{
				if (request != null)
				{
					FacebookData.AppRequestsResponse.Data data2 = null;
					bool flag = false;
					try
					{
						data2 = JSON.Deserialize<FacebookData.AppRequestsResponse.Data>(request.data);
						if (data2 == null)
						{
							flag = true;
						}
					}
					catch (Exception ex2)
					{
						WoogaDebug.LogWarning(new object[]
						{
							"Failed to deserialize request data: ",
							ex2
						});
						flag = true;
					}
					if (request.data == null || request.from == null || request.to == null || flag)
					{
						if (FacebookData.invalidRequests == null)
						{
							FacebookData.invalidRequests = new List<FacebookData.AppRequestsResponse.Request>();
						}
						FacebookData.invalidRequests.Add(request);
						Dictionary<string, object> dictionary = new Dictionary<string, object>();
						dictionary.Add("request", request);
						string message = (!flag) ? "Required field (data, from, to) empty" : "Failed to desierialize data parameter";
						Log.Warning("Invalid Facebook request", message, dictionary);
					}
					else
					{
						list.Add(new FacebookData.Request
						{
							ID = request.id,
							fromID = request.from.id,
							fromName = request.from.first_name,
							type = data2.type,
							item = data2.item,
							isResponse = data2.isResponse
						});
					}
				}
			}
			return list;
		}

		// Token: 0x0400586B RID: 22635
		public static string[] KeyPagingURL = new string[]
		{
			"paging",
			"next"
		};

		// Token: 0x0400586C RID: 22636
		public static string[] KeyFriendURL = new string[]
		{
			"picture",
			"data",
			"url"
		};

		// Token: 0x0400586D RID: 22637
		private static List<FacebookData.Friend> noFriends = new List<FacebookData.Friend>();

		// Token: 0x0400586F RID: 22639
		private static List<FacebookData.Request> noRequests = new List<FacebookData.Request>();

		// Token: 0x0200077C RID: 1916
		[Serializable]
		public class Edge<TData>
		{
			// Token: 0x04005870 RID: 22640
			public TData data;
		}

		// Token: 0x0200077D RID: 1917
		[Serializable]
		public class FacebookRequestLog
		{
			// Token: 0x1700075F RID: 1887
			// (get) Token: 0x06002F58 RID: 12120 RVA: 0x000DDBF7 File Offset: 0x000DBFF7
			// (set) Token: 0x06002F59 RID: 12121 RVA: 0x000DDC04 File Offset: 0x000DC004
			public DateTime timestamp
			{
				get
				{
					return new DateTime(this.timeUtc);
				}
				set
				{
					this.timeUtc = value.Ticks;
				}
			}

			// Token: 0x04005871 RID: 22641
			public string ID;

			// Token: 0x04005872 RID: 22642
			public long timeUtc;
		}

		// Token: 0x0200077E RID: 1918
		[Serializable]
		public class FacebookGameData
		{
			// Token: 0x04005873 RID: 22643
			public List<FacebookData.FacebookRequestLog> RequestLog = new List<FacebookData.FacebookRequestLog>();

			// Token: 0x04005874 RID: 22644
			public bool receivedLoginReward;

			// Token: 0x04005875 RID: 22645
			public string loggedInUserFirstName = string.Empty;

			// Token: 0x04005876 RID: 22646
			public int lastSeenInviteFriends;

			// Token: 0x04005877 RID: 22647
			public int friendCountHighWater;
		}

		// Token: 0x0200077F RID: 1919
		[Serializable]
		public class UserInvitableFriendPicture
		{
			// Token: 0x04005878 RID: 22648
			public bool is_silhouette;

			// Token: 0x04005879 RID: 22649
			public string url;
		}

		// Token: 0x02000780 RID: 1920
		[Serializable]
		public class PictureData : FacebookData.Edge<FacebookData.UserInvitableFriendPicture>
		{
		}

		// Token: 0x02000781 RID: 1921
		public class Friend : IDisposable
		{
			// Token: 0x06002F5D RID: 12125 RVA: 0x000DDC41 File Offset: 0x000DC041
			public Friend()
			{
			}

			// Token: 0x06002F5E RID: 12126 RVA: 0x000DDC49 File Offset: 0x000DC049
			public Friend(string id, string name)
			{
				this.ID = id;
				this.Name = name;
			}

			// Token: 0x06002F5F RID: 12127 RVA: 0x000DDC5F File Offset: 0x000DC05F
			public void Dispose()
			{
				if (this.PictureSprite != null && this.PictureSprite.texture != null)
				{
					global::UnityEngine.Object.Destroy(this.PictureSprite.texture);
					this.PictureSprite = null;
				}
			}

			// Token: 0x17000760 RID: 1888
			// (get) Token: 0x06002F60 RID: 12128 RVA: 0x000DDC9F File Offset: 0x000DC09F
			public string PictureURL
			{
				get
				{
					if (this.Picture != null)
					{
						return this.Picture.data.url;
					}
					return FacebookData.Friend.PictureURLForID(this.ID);
				}
			}

			// Token: 0x06002F61 RID: 12129 RVA: 0x000DDCC8 File Offset: 0x000DC0C8
			public static string PictureURLForID(string fbID)
			{
				// return "https://graph.facebook.com/v2.8/" + fbID + "/picture/?width=256";
				return "host2333" + fbID + "/picture/?width=256";
			}

			// Token: 0x0400587A RID: 22650
			public string ID;

			// Token: 0x0400587B RID: 22651
			public string Name;

			// Token: 0x0400587C RID: 22652
			public FacebookData.PictureData Picture;

			// Token: 0x0400587D RID: 22653
			public Sprite PictureSprite;

			// Token: 0x02000782 RID: 1922
			public enum Type
			{
				// Token: 0x0400587F RID: 22655
				Invitable,
				// Token: 0x04005880 RID: 22656
				Playing,
				// Token: 0x04005881 RID: 22657
				All
			}
		}

		// Token: 0x02000783 RID: 1923
		[Serializable]
		public class FriendListResponse
		{
			// Token: 0x04005882 RID: 22658
			public FacebookData.FriendListResponse.Friend[] data;

			// Token: 0x02000784 RID: 1924
			[Serializable]
			public class Friend
			{
				// Token: 0x04005883 RID: 22659
				public string first_name = "xxxxxx";

				// Token: 0x04005884 RID: 22660
				public string id;

				// Token: 0x04005885 RID: 22661
				public FacebookData.PictureData picture;
			}
		}

		// Token: 0x02000785 RID: 1925
		[Serializable]
		private class FriendSummary
		{
			// Token: 0x04005886 RID: 22662
			public FacebookData.FriendSummary.Summary summary;

			// Token: 0x02000786 RID: 1926
			[Serializable]
			public struct Summary
			{
				// Token: 0x04005887 RID: 22663
				public int total_count;
			}
		}

		// Token: 0x02000787 RID: 1927
		[Serializable]
		public class Request
		{
			// Token: 0x04005888 RID: 22664
			public string fromID;

			// Token: 0x04005889 RID: 22665
			public string fromName;

			// Token: 0x0400588A RID: 22666
			public string type;

			// Token: 0x0400588B RID: 22667
			public string item;

			// Token: 0x0400588C RID: 22668
			public bool isResponse;

			// Token: 0x0400588D RID: 22669
			public string ID;

			// Token: 0x0400588E RID: 22670
			public bool deleted;
		}

		// Token: 0x02000788 RID: 1928
		[Serializable]
		public class AppRequestsResponse
		{
			// Token: 0x0400588F RID: 22671
			public FacebookData.AppRequestsResponse.Request[] data = new FacebookData.AppRequestsResponse.Request[0];

			// Token: 0x02000789 RID: 1929
			[Serializable]
			public class User
			{
				// Token: 0x04005890 RID: 22672
				public string first_name = "xxxxxx";

				// Token: 0x04005891 RID: 22673
				public string id = string.Empty;
			}

			// Token: 0x0200078A RID: 1930
			[Serializable]
			public class Data
			{
				// Token: 0x04005892 RID: 22674
				public string type = "request";

				// Token: 0x04005893 RID: 22675
				public string item = "lives";

				// Token: 0x04005894 RID: 22676
				public bool isResponse;

				// Token: 0x04005895 RID: 22677
				public string context1 = string.Empty;

				// Token: 0x04005896 RID: 22678
				public string context2 = string.Empty;
			}

			// Token: 0x0200078B RID: 1931
			[Serializable]
			public class Request
			{
				// Token: 0x04005897 RID: 22679
				public string id = string.Empty;

				// Token: 0x04005898 RID: 22680
				public FacebookData.AppRequestsResponse.User from = new FacebookData.AppRequestsResponse.User();

				// Token: 0x04005899 RID: 22681
				public FacebookData.AppRequestsResponse.User to = new FacebookData.AppRequestsResponse.User();

				// Token: 0x0400589A RID: 22682
				public string data = string.Empty;
			}
		}
	}
}
