using System.Text;
using System.Diagnostics;

namespace Adrenak.UniDLL {
	public class CSC {
		public string target = "library";
		public string output;
		public int warn = 1;
		public string[] references = new string[0];
		public string[] defines = new string[0];
		public string[] recurse = new string[0];

		override public string ToString() {
			var sb = new StringBuilder();
			sb.Append("csc").Append(" ^\n");

			sb.Append("/target:").Append(target).Append(" ^\n");
			sb.Append("/out:").Append("\"").Append(output.Replace("/", "\\")).Append("\" ^\n");

			for (int i = 0; i < defines.Length; i++)
				sb.Append("/define:").Append("\"").Append(defines[i]).Append("\"").Append(" ^\n");

			for (int i = 0; i < references.Length; i++) {
				references[i] = references[i].Replace('/', '\\');
				sb.Append("/r:").Append("\"").Append(references[i]).Append("\"").Append(" ^\n");
			}

			for (int i = 0; i < recurse.Length; i++) {
				recurse[i] = recurse[i].Replace('/', '\\');

				if (Utils.IsFile(recurse[i]))
					sb.Append("/recurse:").Append("\"").Append(recurse[i]).Append("\"").Append(" ^\n");
				else
					sb.Append("/recurse:").Append("\"").Append(recurse[i]).Append("\\*.cs").Append("\"").Append(" ^\n");
			}

			sb.Append("/warn:").Append(warn);
			return sb.ToString();
		}

		public string ToCommand() {
			return @"/c " + ToString().Replace("^\n", "");
		}

		public void Build() {
			Process.Start("cmd.exe", ToCommand());
		}
	}
}
