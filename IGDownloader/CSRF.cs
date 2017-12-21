using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace IGDownloader
{
    public class CSRF
    {
        public string CSRF_Token;

        public CSRF() { }

        public void generateCSRF()
        {
            string source = new System.Net.WebClient().DownloadString("https://www.instagram.com/accounts/edit/?wo=1");
            string pattern = "\"csrf_token\": \"(.*)\"";

            string CSRF = string.Empty;

            foreach (Match item in (new Regex(pattern).Matches(source)))
            {
                CSRF = item.Groups[1].Value.Split('"')[0];
            }
            CSRF_Token = CSRF;
        }
    }
}
