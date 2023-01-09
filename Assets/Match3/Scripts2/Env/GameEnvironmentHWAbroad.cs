using System.Collections.Generic;

// Token: 0x02000AB6 RID: 2742
namespace Match3.Scripts2.Env
{
	public class GameEnvironmentHWAbroad : AGameEnvironmentConfig
	{
		// eli key point: SbsEnvironmentAndroid()
		public GameEnvironmentHWAbroad()
		{
			currentPlatform = GameEnvironment.Platform.HW_Abroad;
		
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
					// "net.wooga.tropicats_tropical_cats_puzzle_paradise"
				}
			};
			this.mapEnvToId = new Dictionary<GameEnvironment.Environment, string>
			{
				{
					GameEnvironment.Environment.CI,
					"iq8gvx9l$ag_-34lak3bn2b3"
				},
				{
					GameEnvironment.Environment.STAGING,
					"ovsfffew0asldfk09v-a23$s"
				},
				{
					GameEnvironment.Environment.PRODUCTION,
					"igane82#algasic2025-^%ss"
				}
			};
			this.mapEnvToStoreUrl = new Dictionary<GameEnvironment.Environment, string>
			{
				{
					GameEnvironment.Environment.CI,
					"host233333"
					// "http://bit.do/pt-staging-android2"
				},
				{
					GameEnvironment.Environment.STAGING,
					"host23333"
					// "http://bit.do/pt-staging-android2"
				},
				{
					GameEnvironment.Environment.PRODUCTION,
					"host2333"
					// "http://bit.do/pt-staging-android2"
				}
			};
			this.mapEnvToKey = new Dictionary<GameEnvironment.Environment, string>
			{
				{
					GameEnvironment.Environment.CI,
					"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAp940FjwJDm3xF3KyQbfRkOumO9109Gr0ek8GLaDvekKkbCP7Ie/bTD81/bcrNWxAABBJeV3kFTzFclViGhObW0gVxbnWITRLlq7r+dkczcIIuD5ytSpmp1HJF8e+y4oBVfO4UA5CJmUVpi6Aip8fPUsxMb+oTjxcObnWZ8k2NOYOcvP8L/7WATvcbw6IjEApMfC7b2kP6gEZhb1ZXIS66erPW0LwCBMNh8sthodcDRxtBTmBEK1gx7x7owiRumDsZTmImIdi+ZoSjIPvi6wq4SY+vGvdW6nEPpNGOBArccCN/9XTvIxSmUeK2NtV/v9SmrPLJrjvyooGWJtr9l9LNwIDAQAB"
				},
				{
					GameEnvironment.Environment.STAGING,
					"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAxHHqJErg/sSENNUQ7RFqfvCpCqcfrb62+UpHoaBeE4o8eaZ+Z8ioKx+W89DvC9k40ufBPpNR3UBfMG0BLp8eJwgJTl2LtpRtrcRyEhvCCPqlsh4+1YUmkFJe7j0AyqjfJ/trJGypkOcfxRI0P1Q4uMKLpy5SsjBwK+OsReIp+dfmF6AZe0fruFrRlkn3VlYvfwVUPOpta1WsPX98xvNlHnkyiqv01zPbhQBCrLXzH5NAmwqb97P+lxlsl4EyojoIC3/A4vQhR5GDqQn3FeiIenWV8Jf2cUJiIfDHskRHw8z7Wf4pRMjGLjddnn4jTodaCV+H9FuVzmCVsqBJkVT/GQIDAQAB"
				},
				{
					GameEnvironment.Environment.PRODUCTION,
					"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwBVRN3YAnZIfKijC3RUfH0WowlLDRGHhGjV2RtJnSC4YQ5LPsqNvy7nMHx/CjKwMMPPay4fFeu0Slv5wtUXTX75Mp3ymBQtmKFm/0fwuEJDRYCrpANcG2HIIVHYgC5jYGHBDYhFZNeRFSo07kr+uTDL2SUjLwYVQDpG45GIpT6ykjKE7NLaTSAmJnnM89E17/u03i91vvUBPoOj2ZaW0Nw1BeW/I+XhLnWjJDnWs7ngLvcrl+BmhuZ7qeDLeaMnBtGZJA2xfpc3hTBblNuBKpWnaPYa66w/MoRGeWXok6EvQ8TADiedg8fZKS5Uw9W83NFsxN0tx6lmrtwzJME1UcwIDAQAB"
				}
			};
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06004112 RID: 16658 RVA: 0x00151BD7 File Offset: 0x0014FFD7
		public override string PLATFORM_PREFIX
		{
			get
			{
				return "and";
			}
		}

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06004113 RID: 16659 RVA: 0x00151BDE File Offset: 0x0014FFDE
		public override string StoreUrl
		{
			get
			{
				// return "market://details?id=net.wooga.tropicats_tropical_cats_puzzle_paradise";
				return "host23333";
			}
		}
	}
}
