using System.IO;

namespace Adrenak.UniDLL {
	public static class Utils {
		public static bool IsFile(this string path) {
			FileAttributes attr = File.GetAttributes(path);

			if ((attr & FileAttributes.Directory) == FileAttributes.Directory) return false;
			else return true;
		}
	}
}
