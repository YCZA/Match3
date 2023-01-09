using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Build
{
	// eli key point 版本号
	public static class BuildVersion
	{
		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x060040D8 RID: 16600 RVA: 0x00151485 File Offset: 0x0014F885
		private static string major
		{
			get
			{
				return "1";
			}
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x060040D9 RID: 16601 RVA: 0x0015148C File Offset: 0x0014F88C
		private static string minor
		{
			get
			{
				return "0";
			}
		}

		// Token: 0x060040DA RID: 16602 RVA: 0x00151493 File Offset: 0x0014F893
		public static void SetBranchName(string branchName)
		{
			BuildVersion._patch = null;
			BuildVersion._sbsConfigVersion = null;
			BuildVersion.VersionData.branchName = branchName;
		}

		// Token: 0x060040DB RID: 16603 RVA: 0x001514AC File Offset: 0x0014F8AC
		private static BuildVersion.BuildVersionData LoadVersionData()
		{
			return BuildVersion.BuildVersionData.Load();
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x060040DC RID: 16604 RVA: 0x001514B3 File Offset: 0x0014F8B3
		public static BuildVersion.BuildVersionData VersionData
		{
			get
			{
				if (BuildVersion._versionData == null)
				{
					BuildVersion._versionData = BuildVersion.LoadVersionData();
				}
				return BuildVersion._versionData;
			}
		}

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x060040DD RID: 16605 RVA: 0x001514D0 File Offset: 0x0014F8D0
		private static string patch
		{
			get
			{
				if (BuildVersion._patch == null)
				{
					BuildVersion._patch = BuildVersion.VersionData.buildNumber + "-" + BuildVersion.VersionData.branchName;
				}
				if (BuildVersion.VersionData.branchName == "develop")
				{
					return BuildVersion.VersionData.buildNumber;
				}
				return BuildVersion._patch;
			}
		}

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x060040DE RID: 16606 RVA: 0x00151534 File Offset: 0x0014F934
		public static string Version
		{
			get
			{
				// if (BuildVersion._version == null)
				// {
					// todo 暂时使用配置文件
					try
					{
						var version = Resources.Load<TextAsset>("config/build/version");
						return version.text;
					}
					catch (Exception e)
					{
						Debug.LogError(e);
						return "unknown";
					}
					// BuildVersion._version = string.Concat(new string[]
					// {
					// 	SbsEnvironment.CurrentId,
					// 	"::",
					// 	BuildVersion.major,
					// 	".",
					// 	BuildVersion.minor,
					// 	".",
					// 	BuildVersion.patch,
					// 	"-",
					// 	SbsEnvironment.PLATFORM_PREFIX,
					// 	"-",
					// 	SbsEnvironment.CurrentEnvShortCode
					// });
				// }
				// return BuildVersion._version;
			}
		}
		
		// 开始: 由工具来修改版本号，代码和注释不要动
		public const int INTERNAL_VERSION = 5;
		// 结束: 由工具来修改版本号，代码和注释不要动
		
		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x060040DF RID: 16607 RVA: 0x001515BC File Offset: 0x0014F9BC
		public static int Patch
		{
			get
			{
				int result;
				if (int.TryParse(BuildVersion.VersionData.buildNumber, out result))
				{
					return result;
				}
				return 0;
			}
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x060040E0 RID: 16608 RVA: 0x001515E4 File Offset: 0x0014F9E4
		public static string ShortVersion
		{
			get
			{
				if (BuildVersion._version == null)
				{
					BuildVersion._version = string.Concat(new string[]
					{
						BuildVersion.major,
						".",
						BuildVersion.minor,
						".",
						BuildVersion.VersionData.buildNumber
					});
				}
				return BuildVersion._version;
			}
		}

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x060040E1 RID: 16609 RVA: 0x0015163D File Offset: 0x0014FA3D
		public static Version RuntimeVersion
		{
			get
			{
				return new Version(int.Parse(BuildVersion.major), int.Parse(BuildVersion.minor));
			}
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x060040E2 RID: 16610 RVA: 0x00151658 File Offset: 0x0014FA58
		public static string ConfigVersion
		{
			get
			{
				if (BuildVersion._configVersion == null)
				{
					BuildVersion._configVersion = string.Concat(new object[]
					{
						BuildVersion.major,
						".",
						BuildVersion.minor,
						".",
						0
					});
				}
				return BuildVersion._configVersion;
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x060040E3 RID: 16611 RVA: 0x001516B0 File Offset: 0x0014FAB0
		public static string SbsConfigVersion
		{
			get
			{
				if (BuildVersion._sbsConfigVersion == null)
				{
					BuildVersion._sbsConfigVersion = BuildVersion.ConfigVersion;
					Regex regex = new Regex("[^0-9A-Za-z-]");
					string text = regex.Replace(BuildVersion.VersionData.branchName, "-");
					if (!BuildVersion.IsReleaseBranch(text))
					{
						BuildVersion._sbsConfigVersion = BuildVersion._sbsConfigVersion + "-" + text;
					}
				}
				return BuildVersion._sbsConfigVersion;
			}
		}

		// Token: 0x060040E4 RID: 16612 RVA: 0x00151717 File Offset: 0x0014FB17
		public static bool IsLowerThan(Version other)
		{
			return BuildVersion.RuntimeVersion.CompareTo(other) < 0;
		}

		// Token: 0x060040E5 RID: 16613 RVA: 0x00151727 File Offset: 0x0014FB27
		public static bool IsReleaseBranch(string branch)
		{
			return branch == "develop" || branch == "staging" || branch == "production";
		}

		// Token: 0x04006AB4 RID: 27316
		private static BuildVersion.BuildVersionData _versionData;

		// Token: 0x04006AB5 RID: 27317
		private static string _patch;

		// Token: 0x04006AB6 RID: 27318
		private static string _version;

		// Token: 0x04006AB7 RID: 27319
		private static string _configVersion;

		// Token: 0x04006AB8 RID: 27320
		private static string _sbsConfigVersion;

		// Token: 0x02000AAD RID: 2733
		[Serializable]
		public class BuildVersionData
		{
			// Token: 0x060040E8 RID: 16616 RVA: 0x00151761 File Offset: 0x0014FB61
			public override string ToString()
			{
				return string.Format("{0}-{1}", this.branchName, this.buildNumber);
			}

			// Token: 0x060040E9 RID: 16617 RVA: 0x0015177C File Offset: 0x0014FB7C
			public static BuildVersion.BuildVersionData Load()
			{
				// string text = Resources.Load<TextAsset>("buildData.config").text;
				string text = "{\"branchName\": \"production\",\"buildNumber\": \"1\"}";
				return JsonUtility.FromJson<BuildVersion.BuildVersionData>(text);
			}

			// Token: 0x04006AB9 RID: 27321
			public const string FILE_NAME = "buildData.config";

			// Token: 0x04006ABA RID: 27322
			public const string FILE_PATH = "Assets/Shared/Build/Resources/";

			// Token: 0x04006ABB RID: 27323
			public string branchName;

			// Token: 0x04006ABC RID: 27324
			public string buildNumber;
		}
	}
}
