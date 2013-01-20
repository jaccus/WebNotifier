namespace WebNotifier.d2jsp
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Windows;
    using HtmlAgilityPack;

    public class D2JspProcessor
    {
        private const string LoginUriString = "http://forums.d2jsp.org/login.php";
        
        private const string InboxUriString = "http://forums.d2jsp.org/pm.php";

        private const string CookieReadFailedMessage = "Failed to read Internet Explorer cookie for URL: " + LoginUriString;
        
        private HttpWebRequest _request;

        public int ReadInboxUnreadCount()
        {
            return ReadInboxUnreadCountWithHttpRequest();
        }

        private int ReadInboxUnreadCountWithHttpRequest()
        {
            CreateHttpRequest();

            var htmlContent = ReadResponseHtmlContent(GetHttpResponse());

            return ParseHtmlForInboundUnreadCount(htmlContent);
        }

        private void CreateHttpRequest()
        {
            _request = (HttpWebRequest) WebRequest.Create(InboxUriString);

            // Will use login cookie from Internet Explorer to access secured website.
            _request.Headers["Cookie"] = GetLoginCookie();
        }

        private static string GetLoginCookie()
        {
            try
            {
                return Application.GetCookie(new Uri(LoginUriString));
            }
            catch (Exception)
            {
                throw new WebNotifierException(CookieReadFailedMessage);
            }
        }

        private static string ReadResponseHtmlContent(WebResponse response)
        {
            using (var dataStream = response.GetResponseStream())
            {
                if (dataStream == null)
                {
                    throw new WebNotifierException("Failed to read response from server");
                }

                using (var reader = new StreamReader(dataStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private HttpWebResponse GetHttpResponse()
        {
            var response = (HttpWebResponse) _request.GetResponse();
            return response;
        }

        private static int ParseHtmlForInboundUnreadCount(string htmlContent)
        {
            var htmlDocument = CreateHtmlDocumentFromContent(htmlContent);

            // Inbox HTML part that we parse for unread msg count is message table element <mLT>.
            var msgTableElement = htmlDocument.GetElementbyId("mLT");

            //var messageTableContent = msgTableElement.OuterHtml;
            //File.WriteAllText(@"D:\jsp.html", messageTableContent);

            var unreadCount = 0;

            // Subject of an unread message is bold <b>Subject</b>, use that to count unread msgs.
            foreach (var tag in msgTableElement.ChildNodes)
            {
                // Ignore non-tr tags and column row (detected by <th> child tag).
                if (!IsTrTag(tag) || IsColumnRowElement(tag)) continue;

                // Subject column - 3rd or 4th column (second last).
                var secondLastIndex = tag.ChildNodes.Count - 2;
                var subjectTdElement = tag.ChildNodes.ElementAt(secondLastIndex); 

                // Message href - 1st or 2nd column, so take <a> tag.
                var subjectAElement = subjectTdElement.ChildNodes.Single(c => c.Name == "a");

                var subjectHtmlContent = subjectAElement.InnerHtml;
                var subjectTextContent = subjectAElement.InnerText;

                if (subjectHtmlContent != subjectTextContent)
                {
                    ++unreadCount;
                }
            }

            return unreadCount;
        }

        private static bool IsColumnRowElement(HtmlNode trTag)
        {
            return trTag.FirstChild.Name == "th";
        }

        private static bool IsTrTag(HtmlNode trTag)
        {
            return trTag.Name == "tr";
        }

        private static HtmlDocument CreateHtmlDocumentFromContent(string htmlContent)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.Load(new StringReader(htmlContent));
            return htmlDoc;
        }
    }
}
