using System.Collections.Generic;

// Token: 0x02000AB7 RID: 2743
namespace Match3.Scripts2.Env
{
	public class GameEnvironmentIos : AGameEnvironmentConfig
	{
		// Token: 0x06004114 RID: 16660 RVA: 0x00151BE8 File Offset: 0x0014FFE8
		public GameEnvironmentIos()
		{
			this.mapEnvToBundleId = new Dictionary<GameEnvironment.Environment, string>
			{
				{
					GameEnvironment.Environment.CI,
					"host2333"
					// "net.wooga.puzzletown.ci"
				},
				{
					GameEnvironment.Environment.STAGING,
					"host2333"
					// "net.wooga.puzzletown.staging"
				},
				{
					GameEnvironment.Environment.PRODUCTION,
					"host2333"
					// "net.wooga.tropicats-tropical-cats-puzzle-paradise"
				}
			};
			this.mapEnvToId = new Dictionary<GameEnvironment.Environment, string>
			{
				{
					GameEnvironment.Environment.CI,
					"zeoygxjzi2rcxzxpmk6ywdot"
				},
				{
					GameEnvironment.Environment.STAGING,
					"6f8gsozsithchkrgvoehstk0"
				},
				{
					GameEnvironment.Environment.PRODUCTION,
					"a4nrkyo57gsqz2e4558xpnvd"
				}
			};
			this.mapEnvToStoreUrl = new Dictionary<GameEnvironment.Environment, string>
			{
				{
					GameEnvironment.Environment.CI,
					"host2333"
					// "http://bit.do/pt-staging-ios"
				},
				{
					GameEnvironment.Environment.STAGING,
					"host2333"
					// "http://bit.do/pt-staging-ios"
				},
				{
					GameEnvironment.Environment.PRODUCTION,
					"host2333"
					// "http://bit.do/pt-staging-ios"
				}
			};
			this.mapEnvToKey = new Dictionary<GameEnvironment.Environment, string>
			{
				{
					GameEnvironment.Environment.CI,
					"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwEtdMu8YK2IaxAbK37c6RHFYMmNipnQtslxfH8ZU8YaQmmpTc5ho7WSGSHAJPGP9nM5ThQeacXu8FxHmDbCWldrgYRCKgaQ6JIkf2IBV/NZOJBQQt9v/mc6BlV2XVPuzRGPyZXUFYEjtcZHhsdw8a1JXhvzN3ENT8oy6PPFPLmfsaeO1iINYvhBNanKoBkeCekKtAI3Qoeo3Y+ftnhCMOhKrzMNF+t1TYFWefRKnhZaccr6dLO43Q2o7bGyEmDKyJ0R1yEledDL2i3cS2FJHY+RyycGMIXnLBTzp2o+mRXBgCGJQIleeVOQFGUspi3qBed5vEYzxvJyO2cSpMYvzzwIDAQAB"
				},
				{
					GameEnvironment.Environment.STAGING,
					"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA57KG/LFq9m5OcPejHRyXjVohknSdusU3uLBDYDQmlptJ5I8dff21oJkKhmqw6FZ/Cgp4WNX9Lgtry0AbFzXgNh5wuwOQp5r/6aKSB2fXI2k9uJAHI/x2yAhpR1XbuYCvtv60SLvFOwc4i2jQL+j/RoN1Pp6NWXv719kvN7TdHrwgmdm32tZrX5rtPYdGtzFxoVilcFk+9A0G2Afu69gpAqfGVfmRFAv9sZ4Ws1ydr0eidzfUcadmfSZRSVPflFqXoCjz2/eI03BTJ258nw9JNclS6qa8nO9hbiV/ZG2djUZ65gvzlBFD/G3PEWIX6lAYZkFE3/3y1tVgPX/3ErfUUwIDAQAB"
				},
				{
					GameEnvironment.Environment.PRODUCTION,
					"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAyq5O8q6PDw9yWJm2FLI/ilTjWXTgGJbHz5WTQHxk+r8EbZPorigdD78oC6FxK9Rt3rFH1rkdFQiUA83lTr8qslBAy70mMwBD4y8aqWxAuQ4UR3NGT7yv3byOpK/IbY6rfW801eia4JDDEZlHXUb41EOLkWIfU1lIgoGx/BlIx4QOOHaUtijO7MeufG1s7blsEX9CVcVBi3ASZrwCGR9SMvOtE0p1049jql/upZ6fCbHBx8oU/p1zrIBvGO0rVoEgsE7fdF5hKeLEu3c6gI1ur9nFM9VqQJHxZBnGWaznL1FVgUUE6wX4h0S2prFvKeCf3dY62QUTZ3ikODGH//CAaQIDAQAB"
				}
			};
		}

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06004115 RID: 16661 RVA: 0x00151CBF File Offset: 0x001500BF
		public override string PLATFORM_PREFIX
		{
			get
			{
				return "ios";
			}
		}

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x06004116 RID: 16662 RVA: 0x00151CC6 File Offset: 0x001500C6
		public override string StoreUrl
		{
			get
			{
				// return "itms-apps://itunes.apple.com/app/tropicats-puzzle-paradise/id1215220850";
				return "host2333";
			}
		}
	}
}
