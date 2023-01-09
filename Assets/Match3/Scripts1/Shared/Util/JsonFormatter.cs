using System.Linq;
using System.Text;

namespace Match3.Scripts1.Shared.Util
{
	// Token: 0x02000B4C RID: 2892
	public class JsonFormatter
	{
		// Token: 0x060043B7 RID: 17335 RVA: 0x00159CB8 File Offset: 0x001580B8
		public static string FormatJson(string str)
		{
			int num = 0;
			bool flag = false;
			StringBuilder sb = new StringBuilder();
			int i = 0;
			while (i < str.Length)
			{
				char c = str[i];
				switch (c)
				{
				case '[':
					goto IL_71;
				default:
					switch (c)
					{
					case '{':
						goto IL_71;
					default:
						if (c != '"')
						{
							if (c != ',')
							{
								if (c != ':')
								{
									sb.Append(c);
								}
								else
								{
									sb.Append(c);
									if (!flag)
									{
										sb.Append(" ");
									}
								}
							}
							else
							{
								sb.Append(c);
								if (!flag)
								{
									sb.AppendLine();
									Enumerable.Range(0, num).ForEach(delegate(int item)
									{
										sb.Append("    ");
									});
								}
							}
						}
						else
						{
							sb.Append(c);
							bool flag2 = false;
							int num2 = i;
							while (num2 > 0 && str[--num2] == '\\')
							{
								flag2 = !flag2;
							}
							if (!flag2)
							{
								flag = !flag;
							}
						}
						break;
					case '}':
						goto IL_B2;
					}
					break;
				case ']':
					goto IL_B2;
				}
				IL_1BA:
				i++;
				continue;
				IL_71:
				sb.Append(c);
				if (!flag)
				{
					sb.AppendLine();
					Enumerable.Range(0, ++num).ForEach(delegate(int item)
					{
						sb.Append("    ");
					});
				}
				goto IL_1BA;
				IL_B2:
				if (!flag)
				{
					sb.AppendLine();
					Enumerable.Range(0, --num).ForEach(delegate(int item)
					{
						sb.Append("    ");
					});
				}
				sb.Append(c);
				goto IL_1BA;
			}
			return sb.ToString();
		}

		// Token: 0x04006C21 RID: 27681
		private const string INDENT_STRING = "    ";
	}
}
