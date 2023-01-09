using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Match3.Scripts1.Puzzletown.Build;
using Wooga.Coroutines;
using Match3.Scripts2.Android.DataStatistics;
using Match3.Scripts2.Env;
using Match3.Scripts2.Network;
using Wooga.Newtonsoft.Json;
using UnityEngine;

// using LitJson;

// Token: 0x02000AB9 RID: 2745
namespace Match3.Scripts2.PlayerData
{
	public abstract class APlayerPrefsObject<T> where T : new()
	{
		// private static JsonData lastSaveContent_jd = null;
		private static Dictionary<string, object> lastSaveContent_jd = new Dictionary<string, object>();

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x0600411A RID: 16666 RVA: 0x000D6F49 File Offset: 0x000D5349
		private static string ShadowPath
		{
			get
			{
				// eli key point 存档路径
				return Application.persistentDataPath + "/" + typeof(T).Name + ".shadow.json.txt";
			}
		}

		// Token: 0x0600411B RID: 16667 RVA: 0x000D6F70 File Offset: 0x000D5370
		public virtual void Save()
		{
			Debug.Log("保存存档: " + this.GetType());
			if (this.isSaveable)
			{
				string curContent = JsonUtility.ToJson(this);
				if (typeof(T) == typeof(GameState) && false)
				{
					Dictionary<string, object> curContent_jd = ParseJson(curContent);
					Dictionary<string, object> diffContent_jd = GetDiffPart(curContent_jd);

					// 开始上传
					string json = ToJson(diffContent_jd);
					ToServer.Instance.PushArchiveAsync(json);
					lastSaveContent_jd = curContent_jd;
					Debug.Log("上传存档，大小:" + json.Length);
					Debug.Log(json);
				}
				else
				{
					// 保存了2份存档
					PlayerPrefs.SetString(base.GetType().ToString(), curContent);
					PlayerPrefs.Save();
					try
					{
						File.WriteAllText(APlayerPrefsObject<T>.ShadowPath, curContent);
					}
					catch (Exception)
					{
						WoogaDebug.LogWarning(new object[]
						{
							"Failed to write shadow copy: " + APlayerPrefsObject<T>.ShadowPath
						});
					}
				}
			}
		}

		// 退出游戏时需要使用同步保存
		public void SaveSync()
		{
			Debug.Log("同步保存存档: " + this.GetType());
			if (this.isSaveable)
			{
				string curContent = JsonUtility.ToJson(this);
				if (typeof(T) == typeof(GameState))
				{
					Dictionary<string, object> curContent_jd = ParseJson(curContent);
					Dictionary<string, object> diffContent_jd = GetDiffPart(curContent_jd);
				
					// 开始上传
					string json = ToJson(diffContent_jd);
					ToServer.Instance.PushArchiveSync(json);
					lastSaveContent_jd = curContent_jd;
					Debug.Log("上传存档，大小:" + json.Length);
					Debug.Log(json);
				}
			}
		}

		private Dictionary<string, object> GetDiffPart(Dictionary<string, object> curContent_jd)
		{
			Dictionary<string, object> diffContent_jd = new Dictionary<string, object>();
			if (lastSaveContent_jd == null)
			{
				// 第一次上传存档
				diffContent_jd = curContent_jd;
				Debug.Log("第一次上传存档");
			}
			else
			{
				// 上传不同的部分
				foreach (var item in curContent_jd)
				{
					if (!lastSaveContent_jd.ContainsKey(item.Key))
					{
						diffContent_jd[item.Key] = item.Value;
						continue;
					}

					if (lastSaveContent_jd[item.Key].ToString() != curContent_jd[item.Key].ToString())
					{
						diffContent_jd[item.Key] = item.Value;
					}
				}
			}

			return diffContent_jd;
		}

		/// <summary>
		/// 替换存档
		/// </summary>
		/// <param name="data"></param>
		public static void ReplaceArchive(string data)
		{
			PlayerPrefs.SetString(typeof(T).ToString(), data);
			PlayerPrefs.Save();
			try
			{
				File.WriteAllText(APlayerPrefsObject<T>.ShadowPath, data);
			}
			catch (Exception)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Failed to replace shadow copy: " + APlayerPrefsObject<T>.ShadowPath
				});
			}
		}

		public static bool Exists()
		{
			return PlayerPrefs.HasKey(typeof(T).ToString()) || File.Exists(APlayerPrefsObject<T>.ShadowPath);
		}

		/// <summary>
		/// 从本地载入存档
		/// </summary>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static T Load()
		{
			Debug.Log("载入存档: " + typeof(T));
			T result;
			// string archive = PlayerPrefs.GetString(typeof(T).ToString(), null);
			string archive = "";
			try
			{
				archive = File.ReadAllText(APlayerPrefsObject<T>.ShadowPath);
			}
			catch (Exception e)
			{
				Debug.Log("创建存档");
			}

			if (string.IsNullOrEmpty(archive))
			{
				WoogaDebug.Log(new object[]
				{
					"Creating new instance of: " + typeof(T).Name
				});
				result = Activator.CreateInstance<T>();
			}
			else
			{
				result = JsonUtility.FromJson<T>(archive);
			}
			return result;
		}

		public static IEnumerator LoadFromServer()
		{
			Debug.Log("从服务器载入存档: " + typeof(T));
			DataStatistics.Instance.TriggerEnterGameEvent(4, (int) Time.time, SystemInfo.deviceUniqueIdentifier, BuildVersion.Version, GameEnvironment.CurrentPlatform.ToString());
			Wooroutine<string> pullArchive = WooroutineRunner.StartWooroutine<string>(ToServer.Instance.PullArchive());
			yield return pullArchive;
			string archive = pullArchive.ReturnValue;
			GameState result = null;
		
			if (string.IsNullOrEmpty(archive))
			{
				WoogaDebug.Log(new object[]
				{
					"Creating new instance of: " + nameof(GameState)
				});
				result = Activator.CreateInstance<GameState>();
			}
			else
			{
				result = JsonUtility.FromJson<GameState>(archive);
				lastSaveContent_jd = ParseJson(archive);
			}
			DataStatistics.Instance.TriggerEnterGameEvent(5, (int) Time.time, SystemInfo.deviceUniqueIdentifier, BuildVersion.Version, GameEnvironment.CurrentPlatform.ToString());
			yield return result;
		}

		// Token: 0x04006AD8 RID: 27352
		public bool isSaveable = true;
	
		// -- parse json --
		private static Dictionary<string, object> ParseJson(string json)
		{
			var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
			return result;
		}

		private static string ToJson(Dictionary<string, object> jsonData)
		{
			return JsonConvert.SerializeObject(jsonData);
		}
	}
}
