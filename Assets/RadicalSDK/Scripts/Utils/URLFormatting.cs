using System.Collections.Generic;
using System.Text;

namespace Radical
{
    public class URLFormatting
    {
        public static string AddQuery(string url, string path, Dictionary<string, string> query)
        {
            StringBuilder sb = new StringBuilder(url);
            sb.Append(path);
            bool isFirstArgument = true;
            if (query != null)
            {
                foreach (var entry in query)
                {
                    char separator = isFirstArgument ? '?' : '&';
                    sb.Append(separator);
                    sb.Append(entry.Key);
                    sb.Append('=');
                    sb.Append(entry.Value);
                    isFirstArgument = false;
                }
            }
            return sb.ToString();
        }
    }
}