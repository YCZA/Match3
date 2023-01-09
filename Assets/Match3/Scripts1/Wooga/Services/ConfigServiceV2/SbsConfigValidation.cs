using System;
using System.IO;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Match3.Scripts1.Wooga.Services.ConfigServiceV2
{
	// Token: 0x02000340 RID: 832
	public class SbsConfigValidation : ISbsConfigValidation
	{
		// Token: 0x06001987 RID: 6535 RVA: 0x00073332 File Offset: 0x00071732
		public static bool VerifySBSResponse(string body, string signature, string signatureAlgo, string publicKey)
		{
			return SbsConfigValidation.singleton.verifySBSResponse(body, signature, signatureAlgo, publicKey);
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x00073342 File Offset: 0x00071742
		bool ISbsConfigValidation.VerifySBSResponse(string body, string signature, string signatureAlgo, string publicKey)
		{
			return SbsConfigValidation.singleton.verifySBSResponse(body, signature, signatureAlgo, publicKey);
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x00073353 File Offset: 0x00071753
		protected virtual bool verifySBSResponse(string body, string signature, string signatureAlgo, string publicKey)
		{
			return body != null && signature != null && signatureAlgo != null && publicKey != null && signatureAlgo == "rsa.sha256.v1" && this.verifySignature(body, signature, publicKey);
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x0007338A File Offset: 0x0007178A
		public static bool VerifySignature(string data, string expectedSignature, string pubKey)
		{
			return SbsConfigValidation.singleton.verifySignature(data, expectedSignature, pubKey);
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x0007339C File Offset: 0x0007179C
		protected virtual bool verifySignature(string data, string expectedSignature, string pubKey)
		{
			RsaKeyParameters parameters = this.DecodeX509PublicKey(SbsConfigValidation.GetBytes(pubKey, true));
			ISigner signer = SignerUtilities.GetSigner("SHA-256withRSA");
			signer.Init(false, parameters);
			byte[] bytes = SbsConfigValidation.GetBytes(expectedSignature, true);
			byte[] bytes2 = SbsConfigValidation.GetBytes(data, false);
			signer.BlockUpdate(bytes2, 0, bytes2.Length);
			return signer.VerifySignature(bytes);
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x000733EC File Offset: 0x000717EC
		private static byte[] GetBytes(string str, bool base64 = true)
		{
			return (!base64) ? Encoding.UTF8.GetBytes(str) : Convert.FromBase64String(str);
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x0007340C File Offset: 0x0007180C
		private static RsaKeyParameters MakeKey(string modulusHexString, string exponentHexString)
		{
			BigInteger modulus = new BigInteger(modulusHexString, 16);
			BigInteger exponent = new BigInteger(exponentHexString, 16);
			return new RsaKeyParameters(false, modulus, exponent);
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x00073434 File Offset: 0x00071834
		private RsaKeyParameters DecodeX509PublicKey(byte[] x509key)
		{
			MemoryStream input = new MemoryStream(x509key);
			BinaryReader binaryReader = new BinaryReader(input);
			if (binaryReader.ReadByte() != 48)
			{
				return null;
			}
			this.ReadASNLength(binaryReader);
			if (binaryReader.ReadByte() == 48)
			{
				int num = this.ReadASNLength(binaryReader);
				if (binaryReader.ReadByte() == 6)
				{
					int num2 = this.ReadASNLength(binaryReader);
					byte[] array = new byte[num2];
					binaryReader.Read(array, 0, array.Length);
					if (!array.SequenceEqual(SbsConfigValidation.SeqOID))
					{
						return null;
					}
					int count = num - 2 - array.Length;
					binaryReader.ReadBytes(count);
				}
				if (binaryReader.ReadByte() == 3)
				{
					this.ReadASNLength(binaryReader);
					binaryReader.ReadByte();
					if (binaryReader.ReadByte() == 48)
					{
						this.ReadASNLength(binaryReader);
						if (binaryReader.ReadByte() == 2)
						{
							int num3 = this.ReadASNLength(binaryReader);
							byte[] array2 = new byte[num3];
							binaryReader.Read(array2, 0, array2.Length);
							if (array2[0] == 0)
							{
								byte[] array3 = new byte[array2.Length - 1];
								Array.Copy(array2, 1, array3, 0, array2.Length - 1);
								array2 = array3;
							}
							if (binaryReader.ReadByte() == 2)
							{
								int num4 = this.ReadASNLength(binaryReader);
								byte[] array4 = new byte[num4];
								binaryReader.Read(array4, 0, array4.Length);
								return SbsConfigValidation.MakeKey(BitConverter.ToString(array2).Replace("-", string.Empty), BitConverter.ToString(array4).Replace("-", string.Empty));
							}
						}
					}
				}
				return null;
			}
			return null;
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x000735C0 File Offset: 0x000719C0
		private int ReadASNLength(BinaryReader reader)
		{
			int num = (int)reader.ReadByte();
			if ((num & 128) == 128)
			{
				int num2 = num & 15;
				byte[] array = new byte[4];
				reader.Read(array, 4 - num2, num2);
				Array.Reverse(array);
				num = BitConverter.ToInt32(array, 0);
			}
			return num;
		}

		// Token: 0x04004833 RID: 18483
		public static SbsConfigValidation singleton = new SbsConfigValidation();

		// Token: 0x04004834 RID: 18484
		private static byte[] SeqOID = new byte[]
		{
			42,
			134,
			72,
			134,
			247,
			13,
			1,
			1,
			1
		};
	}
}
