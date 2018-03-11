using System.Net;

namespace SpotifyLyricsDomain.Helpers {
    public static class HttpHelpers {
        public static string GetHtml(string url, string userAgent) {
            //todo: To HttpExtensions, or something?
            //Todo: Extend to allow to set other headers than useragent? Dictionary<HttpRequestHeader,string>?
            string html;
            using (var client = new WebClient()) {
                client.Headers[HttpRequestHeader.UserAgent] = userAgent;
                html = client.DownloadString(url);
            }
            return html;
        }
    }
}