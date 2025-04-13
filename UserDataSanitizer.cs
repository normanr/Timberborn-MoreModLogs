using System;

namespace Mods.MoreModLogs {
  public static class UserDataSanitizer {

    public static string Sanitize(string path) {
      var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
      if (path.StartsWith(myDocuments, StringComparison.InvariantCulture)) {
        path = "..." + path.Substring(myDocuments.Length);
      }
      return path;
    }

  }
}
