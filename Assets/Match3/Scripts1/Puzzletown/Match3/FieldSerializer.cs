using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Match3.Scripts1.Shared.Util;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020004F8 RID: 1272
	public static class FieldSerializer
	{
		// Token: 0x06002305 RID: 8965 RVA: 0x0009B140 File Offset: 0x00099540
		public static string FieldsToJson(Fields fields)
		{
			SerializableFields obj = new SerializableFields(fields);
			return JsonUtility.ToJson(obj);
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x0009B15A File Offset: 0x0009955A
		public static void SaveToDisk(string path, Fields fields)
		{
			// string value = FieldSerializer.FieldsToJson(fields);
			// eli todo 保存关卡的fields
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x0009B15C File Offset: 0x0009955C
		public static void SaveToDisk(string path, LevelConfig config)
		{
			string json = FormatJson(JsonUtility.ToJson(config));
			File.WriteAllText(path, json);
			Debug.Log("<color=green>关卡保存完成: "+path+"</color>");
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x0009B15E File Offset: 0x0009955E
		public static void QuickSaveToDisk(string path, LevelConfig config)
		{
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x0009B160 File Offset: 0x00099560
		public static string ConfigToJson(LevelConfig config)
		{
			return JsonUtility.ToJson(config);
		}

		// Token: 0x0600230A RID: 8970 RVA: 0x0009B167 File Offset: 0x00099567
		private static void SaveFileToDisk(string path, LevelConfig config)
		{
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x0009B16C File Offset: 0x0009956C
		public static void SaveToPlayerPrefs(Fields fields)
		{
			string value = FieldSerializer.FieldsToJson(fields);
			PlayerPrefs.SetString(FIELDS_KEY, value);
		}

		// Token: 0x0600230C RID: 8972 RVA: 0x0009B18C File Offset: 0x0009958C
		public static Fields LoadFromPlayerPrefs()
		{
			string @string = PlayerPrefs.GetString(FIELDS_KEY, string.Empty);
			return FieldSerializer.LoadFieldsFromJson(@string);
		}

		// Token: 0x0600230D RID: 8973 RVA: 0x0009B1B0 File Offset: 0x000995B0
		public static Fields LoadFieldsFromPath(string path)
		{
			string json = File.ReadAllText(path);
			SerializableFields serializableFields = JsonUtility.FromJson<SerializableFields>(json);
			return serializableFields.Deserialize();
		}

		// Token: 0x0600230E RID: 8974 RVA: 0x0009B1D4 File Offset: 0x000995D4
		public static Fields LoadFieldsFromJson(string json)
		{
			SerializableFields serializableFields = JsonUtility.FromJson<SerializableFields>(json);
			return serializableFields.Deserialize();
		}

		// Token: 0x0600230F RID: 8975 RVA: 0x0009B1F0 File Offset: 0x000995F0
		public static LevelConfig LoadLevelFromPath(string path)
		{
			string json = File.ReadAllText(path);
			return JsonUtility.FromJson<LevelConfig>(json);
		}

		// Token: 0x06002310 RID: 8976 RVA: 0x0009B20A File Offset: 0x0009960A
		public static LevelConfig LoadLevelFromJson(string json)
		{
			return JsonUtility.FromJson<LevelConfig>(json);
		}

		// Token: 0x06002311 RID: 8977 RVA: 0x0009B214 File Offset: 0x00099614
		private static string FormatJson(string json)
		{
			Regex regex = new Regex("    \\d");
			json = JsonFormatter.FormatJson(json);
			StringBuilder stringBuilder = new StringBuilder();
			StringReader stringReader = new StringReader(json);
			int num = 0;
			string text = string.Empty;
			string text2;
			while (!(text2 = stringReader.ReadLine()).IsNullOrEmpty())
			{
				if (regex.IsMatch(text2))
				{
					if (num == 0)
					{
						text = "            ";
					}
					int num2 = text2.Length - 12;
					text += text2.Substring(text2.Length - num2, num2);
					num = (num + 1) % 9;
					if (num == 0)
					{
						stringBuilder.AppendLine(text);
					}
				}
				else
				{
					stringBuilder.AppendLine(text2);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04004ECC RID: 20172
		// public const string DEFAULT_FIELDS_PATH = "Assets/Puzzletown/Match3/SavedStates/fields.json";
		public const string DEFAULT_FIELDS_PATH = "Assets/New/Scenes/LevelEditor/fields.json";

		// Token: 0x04004ECD RID: 20173
		public const string FIELDS_KEY = "fields";
	}
}
