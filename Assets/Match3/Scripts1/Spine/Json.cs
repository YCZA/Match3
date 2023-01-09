using System.IO;
using Match3.Scripts1.SharpJson;

namespace Match3.Scripts1.Spine
{
	// Token: 0x02000214 RID: 532
	public static class Json
	{
		// Token: 0x0600104B RID: 4171 RVA: 0x00027980 File Offset: 0x00025D80
		public static object Deserialize(TextReader text)
		{
			return new JsonDecoder
			{
				parseNumbersAsFloat = true
			}.Decode(text.ReadToEnd());
		}
	}
}
