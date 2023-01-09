using System.Collections.Generic;

// Token: 0x02000AB0 RID: 2736
namespace Match3.Scripts2.Env
{
	public static class GameEnvironment
	{
		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x060040EE RID: 16622 RVA: 0x001517E6 File Offset: 0x0014FBE6
		public static GameEnvironment.Platform CurrentPlatform
		{
			get
			{
				// return (!GameEnvironment._configIos.HasId(GameEnvironment.CurrentId)) ? GameEnvironment.Platform.Android : GameEnvironment.Platform.Ios;
				foreach (var config in envConfigList)
				{
					if (config.HasId(CurrentId))
					{
						return config.currentPlatform;
					}
				}

				return Platform.HW_Abroad;
			}
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x060040EF RID: 16623 RVA: 0x00151803 File Offset: 0x0014FC03
		public static AGameEnvironmentConfig CurrentConfig
		{
			get
			{
				// return CurrentPlatform != GameEnvironment.Platform.Ios ? _configAndroid : _configIos;
				foreach (var config in envConfigList)
				{
					if (config.HasId(CurrentId))
					{
						return config;
					}
				}

				return envConfigList[0];
			}
		}

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x060040F0 RID: 16624 RVA: 0x0015181F File Offset: 0x0014FC1F
		public static string StoreUrl
		{
			get
			{
				return GameEnvironment.CurrentConfig.StoreUrl;
			}
		}

		public static bool IsProduction
		{
			get
			{
				return GameEnvironment.CurrentEnvironment == GameEnvironment.Environment.PRODUCTION;
			}
		}

		public static bool IsStaging
		{
			get
			{
				return GameEnvironment.CurrentEnvironment == GameEnvironment.Environment.STAGING;
			}
		}

		// 获取环境id
		public static string CurrentId
		{
			get
			{
				if (string.IsNullOrEmpty(GameEnvironment._currentId))
				{
					GameEnvironment._currentId = GameEnvironmentId.Load().id;
					if (GameEnvironment._currentId == null)
					{
						// GameEnvironment._currentId = GameEnvironment._configAndroid.GetId(GameEnvironment.Environment.CI);
						GameEnvironment._currentId = envConfigList[0].GetId(Environment.PRODUCTION);
					}
				}
				return GameEnvironment._currentId;
			}
			private set
			{
				GameEnvironment._currentId = value;
				GameEnvironmentId.CreateAndSave(GameEnvironment.CurrentId);
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x060040F5 RID: 16629 RVA: 0x00151890 File Offset: 0x0014FC90
		public static Scripts1.Wooga.Services.Tracking.Parameters.Environment CurrentTrackingEnvironment
		{
			get
			{
				return GameEnvironment.mapIdToTrackingEnv[GameEnvironment.CurrentEnvironment];
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x060040F6 RID: 16630 RVA: 0x001518A1 File Offset: 0x0014FCA1
		// (set) Token: 0x060040F7 RID: 16631 RVA: 0x001518B2 File Offset: 0x0014FCB2
		public static Environment CurrentEnvironment
		{
			get
			{
				return CurrentConfig.GetEnvironment(CurrentId);
			}
			set
			{
				CurrentId = CurrentConfig.GetId(value);
			}
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x060040F8 RID: 16632 RVA: 0x001518C4 File Offset: 0x0014FCC4
		public static string CurrentBundleIdentifier
		{
			get
			{
				return GameEnvironment.CurrentConfig.GetBundleId(GameEnvironment.CurrentEnvironment);
			}
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x060040F9 RID: 16633 RVA: 0x001518D5 File Offset: 0x0014FCD5
		public static string CurrentEnvShortCode
		{
			get
			{
				return GameEnvironment.mapEnvToShortCode[GameEnvironment.CurrentEnvironment];
			}
		}

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x060040FA RID: 16634 RVA: 0x001518E6 File Offset: 0x0014FCE6
		public static string CurrentPublicKey
		{
			get
			{
				return GameEnvironment.CurrentConfig.GetPublicKey(GameEnvironment.CurrentEnvironment);
			}
		}

		// public static void Set(GameEnvironment.Platform platform, GameEnvironment.Environment environment)
		// {
		// ASbsEnvironmentConfig asbsEnvironmentConfig = (platform != GameEnvironment.Platform.Ios) ? GameEnvironment._configAndroid : GameEnvironment._configIos;
		// GameEnvironment.CurrentId = asbsEnvironmentConfig.GetId(environment);
		// }

		// Token: 0x04006AC3 RID: 27331
		public static string PLATFORM_PREFIX;

		private static AGameEnvironmentConfig _configAndroid = new GameEnvironmentAndroid();	// 弃用
		private static AGameEnvironmentConfig _configIos = new GameEnvironmentIos();	// 弃用

		private static List<AGameEnvironmentConfig> envConfigList = new List<AGameEnvironmentConfig>()
		{
			new GameEnvironmentHWAbroad(),
			new GameEnvironmentSamsungAbroad()
		};

		// Token: 0x04006AC6 RID: 27334
		private static readonly Dictionary<GameEnvironment.Environment, Scripts1.Wooga.Services.Tracking.Parameters.Environment> mapIdToTrackingEnv = new Dictionary<GameEnvironment.Environment, Scripts1.Wooga.Services.Tracking.Parameters.Environment>
		{
			{
				GameEnvironment.Environment.CI,
				Scripts1.Wooga.Services.Tracking.Parameters.Environment.CI
			},
			{
				GameEnvironment.Environment.STAGING,
				Scripts1.Wooga.Services.Tracking.Parameters.Environment.Staging
			},
			{
				GameEnvironment.Environment.PRODUCTION,
				Scripts1.Wooga.Services.Tracking.Parameters.Environment.Release
			}
		};

		// Token: 0x04006AC7 RID: 27335
		private static readonly Dictionary<GameEnvironment.Environment, string> mapEnvToShortCode = new Dictionary<GameEnvironment.Environment, string>
		{
			{
				GameEnvironment.Environment.CI,
				"CI"
			},
			{
				GameEnvironment.Environment.STAGING,
				"ST"
			},
			{
				GameEnvironment.Environment.PRODUCTION,
				"PR"
			}
		};

		// Token: 0x04006AC8 RID: 27336
		private static string _currentId;

		// Token: 0x02000AB1 RID: 2737
		public enum Environment
		{
			CI,	// 开发？
			// Token: 0x04006ACB RID: 27339
			STAGING,	// ???
			// Token: 0x04006ACC RID: 27340
			PRODUCTION	// 线上
		}

		// Token: 0x02000AB2 RID: 2738
		public enum Platform
		{
			Android,
			Ios,
			HW_Abroad,
			Samsung_Abroad,
		}
	}
}
