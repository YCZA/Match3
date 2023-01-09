using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using UnityEngine;

// Token: 0x0200078E RID: 1934
namespace Match3.Scripts1
{
	public class FacebookImageRunner
	{
		// Token: 0x06002F7F RID: 12159 RVA: 0x000DE034 File Offset: 0x000DC434
		public void Start()
		{
			for (int i = 0; i < 4; i++)
			{
				this.coroutines.Add(WooroutineRunner.StartCoroutine(this.RunDownloadProcessor(), null));
			}
		}

		// Token: 0x06002F80 RID: 12160 RVA: 0x000DE06C File Offset: 0x000DC46C
		public void Stop()
		{
			foreach (Coroutine coroutine in this.coroutines)
			{
				WooroutineRunner.Stop(coroutine);
			}
			this.coroutines.Clear();
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06002F81 RID: 12161 RVA: 0x000DE0D4 File Offset: 0x000DC4D4
		private string PictureCachePath
		{
			get
			{
				if (this._pictureCachePath == null)
				{
					this._pictureCachePath = Application.persistentDataPath + "/fb-image-cache/";
				}
				return this._pictureCachePath;
			}
		}

		// Token: 0x06002F82 RID: 12162 RVA: 0x000DE0FC File Offset: 0x000DC4FC
		private IEnumerator RunDownloadProcessor()
		{
			if (!Directory.Exists(this.PictureCachePath))
			{
				WoogaDebug.Log(new object[]
				{
					"Creating picture cache path: ",
					this.PictureCachePath
				});
				Directory.CreateDirectory(this.PictureCachePath);
			}
			for (;;)
			{
				if (this.DownloadImageForID.Count == 0)
				{
					yield return null;
				}
				else
				{
					FacebookImageRunner.ImageDownloadRequest info = this.DownloadImageForID.Dequeue();
					WaitForSeconds delay = new WaitForSeconds(1f);
					if (info.downloadAfter > DateTime.UtcNow)
					{
						this.DownloadImageForID.Enqueue(info);
						yield return delay;
					}
					else
					{
						info.status = FacebookImageRunner.ImageDownloadRequest.Status.downloading;
						info.imageFileName = this.PictureCachePath + info.friend.ID;
						if (File.Exists(info.imageFileName))
						{
							info.status = FacebookImageRunner.ImageDownloadRequest.Status.complete;
						}
						else
						{
							string imageURL = info.friend.PictureURL;
							WWW www = new WWW(imageURL);
							yield return www;
							bool failed = false;
							if (!string.IsNullOrEmpty(www.error))
							{
								failed = true;
								WoogaDebug.LogWarning(new object[]
								{
									"Failed to download FB image from: " + imageURL,
									www.error
								});
							}
							try
							{
								File.WriteAllBytes(info.imageFileName, www.bytes);
								byte[] data = File.ReadAllBytes(info.imageFileName);
								Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false);
								texture2D.LoadImage(data);
								if (texture2D.width == 8 && texture2D.height == 8)
								{
									File.Delete(info.imageFileName);
									throw new Exception("Looks like the default 8x8 ? texture");
								}
							}
							catch (Exception)
							{
								failed = true;
								WoogaDebug.LogWarning(new object[]
								{
									"Failed to save FB image to: " + info.imageFileName,
									www.error
								});
							}
							if (failed)
							{
								WoogaDebug.LogWarning(new object[]
								{
									"Image failed, requeuing",
									www.error
								});
								info.downloadAfter = DateTime.UtcNow + TimeSpan.FromSeconds(30.0);
								this.DownloadImageForID.Enqueue(info);
							}
							else
							{
								info.status = FacebookImageRunner.ImageDownloadRequest.Status.complete;
								yield return null;
							}
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06002F83 RID: 12163 RVA: 0x000DE118 File Offset: 0x000DC518
		public IEnumerator LoadOrDownloadProfileImages(IEnumerable<FacebookData.Friend> friends)
		{
			List<FacebookImageRunner.ImageDownloadRequest> userInfo = new List<FacebookImageRunner.ImageDownloadRequest>();
			foreach (FacebookData.Friend friend in friends)
			{
				FacebookImageRunner.ImageDownloadRequest item = new FacebookImageRunner.ImageDownloadRequest
				{
					friend = friend
				};
				userInfo.Add(item);
				this.DownloadImageForID.Enqueue(item);
			}
			for (;;)
			{
				bool allComplete = true;
				foreach (FacebookImageRunner.ImageDownloadRequest imageDownloadRequest in userInfo)
				{
					allComplete &= (imageDownloadRequest.status == FacebookImageRunner.ImageDownloadRequest.Status.complete);
				}
				if (allComplete)
				{
					break;
				}
				yield return null;
			}
			yield return null;
			yield break;
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x000DE13C File Offset: 0x000DC53C
		public IEnumerator LoadOrDownloadProfileImage(FacebookData.Friend friend)
		{
			FacebookImageRunner.ImageDownloadRequest userInfo = new FacebookImageRunner.ImageDownloadRequest
			{
				friend = friend
			};
			this.DownloadImageForID.Enqueue(userInfo);
			while (userInfo.status != FacebookImageRunner.ImageDownloadRequest.Status.complete)
			{
				yield return new FacebookService.BoxedSprite(null);
			}
			Sprite nSprite = null;
			try
			{
				byte[] data = File.ReadAllBytes(userInfo.imageFileName);
				Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false);
				texture2D.LoadImage(data);
				nSprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), Vector2.zero);
			}
			catch (Exception)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Failed to load a local FB image from disk: " + userInfo.imageFileName
				});
			}
			yield return new FacebookService.BoxedSprite(nSprite);
			yield break;
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x000DE160 File Offset: 0x000DC560
		public void DeleteLocalProfileImageForOthers(HashSet<string> ownAndActualFriendIDs)
		{
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(this.PictureCachePath);
				FileInfo[] files = directoryInfo.GetFiles();
				foreach (FileInfo fileInfo in files)
				{
					string name = fileInfo.Name;
					if (!ownAndActualFriendIDs.Contains(name))
					{
						File.Delete(fileInfo.FullName);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06002F86 RID: 12166 RVA: 0x000DE1DC File Offset: 0x000DC5DC
		public Sprite LoadLocalProfileImage(string friendID)
		{
			string path = this.PictureCachePath + friendID;
			try
			{
				byte[] data = File.ReadAllBytes(path);
				Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false);
				texture2D.LoadImage(data);
				return Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), Vector2.zero);
			}
			catch (Exception)
			{
			}
			return null;
		}

		// Token: 0x0400589F RID: 22687
		private const int CONCURRENT_DOWNLOADS = 4;

		// Token: 0x040058A0 RID: 22688
		private readonly List<Coroutine> coroutines = new List<Coroutine>();

		// Token: 0x040058A1 RID: 22689
		private string _pictureCachePath;

		// Token: 0x040058A2 RID: 22690
		public readonly Queue<FacebookImageRunner.ImageDownloadRequest> DownloadImageForID = new Queue<FacebookImageRunner.ImageDownloadRequest>();

		// Token: 0x0200078F RID: 1935
		public class ImageDownloadRequest
		{
			// Token: 0x040058A3 RID: 22691
			public FacebookData.Friend friend;

			// Token: 0x040058A4 RID: 22692
			public string imageFileName;

			// Token: 0x040058A5 RID: 22693
			public DateTime downloadAfter;

			// Token: 0x040058A6 RID: 22694
			public FacebookImageRunner.ImageDownloadRequest.Status status;

			// Token: 0x02000790 RID: 1936
			public enum Status
			{
				// Token: 0x040058A8 RID: 22696
				pending,
				// Token: 0x040058A9 RID: 22697
				downloading,
				// Token: 0x040058AA RID: 22698
				complete
			}
		}
	}
}
