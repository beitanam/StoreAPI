using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using HtmlAgilityPack;
using System.Text;
using System.Threading.Tasks;

namespace StoreAPI
{
    public class Store
    {
        public static List<string> searchQuery(string language, string country, string arch, string query, int pagenum)
        {
            List<string> appArray = new List<string>();
            string apiSearch = "https://next-services.apps.microsoft.com/search/6.3.9600-0/788/"+language+"_"+country+"/m/ROW/c/US/il/"+country+"/cp/10005001/query/cid/0/pf/1/pc/0/pt/"+arch+"/af/0/lf/1/s/0/2/pn/"+pagenum+"/pgc/-1?phrase="+query;
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(apiSearch);
                XmlNodeList nodes = doc.DocumentElement.SelectNodes("/rslt/Ptl/Pts/Pt");
                foreach (XmlNode node in nodes)
                {
                    string appId = node.SelectSingleNode("I").InnerText;
                    string appImageLink = "http://wscont1.apps.microsoft.com/winstore/1x/" + node.SelectSingleNode("Ico").InnerText;
                    string appName = node.SelectSingleNode("T").InnerText;
                    string appDeveloper = node.SelectSingleNode("Dev").InnerText;
                    string appRatingCount = node.SelectSingleNode("Src").InnerText;
                    string appPackageName = appId;
                    if (node.SelectSingleNode("Pfn") != null)
                    {
                        appPackageName = node.SelectSingleNode("Pfn").InnerText;
                    }
                    string appInfo = appId + ";" + appImageLink + ";" + appPackageName + ";" + appName + ";" + appDeveloper + ";" + appRatingCount;
                    appArray.Add(appInfo);
                }
                return appArray;
            }
            catch (Exception ex)
            {
                CrashTracker.generateReport(ex.ToString());
                return appArray;
            }
        }
        public static List<string> browseLists(string language, string country, string arch, string list)
        {
            List<string> appArray = new List<string>();
            string apiSearch = "https://next-services.apps.microsoft.com/search/6.3.9600-0/788/"+language+"_"+country+"/c/ES/cp/10005001/BrowseLists/lts/3.4.5.11.12/cid/0/pc/0/pt/"+arch+"/af/0/lf/0";
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(apiSearch);
                XmlNodeList nodes = doc.DocumentElement.SelectNodes("/Ptls/Ptl[@lt='"+list+"']/Pts/Pt");
                foreach (XmlNode node in nodes)
                {
                    string appId = node.SelectSingleNode("I").InnerText;
                    string appImageLink = "http://wscont1.apps.microsoft.com/winstore/1x/" + node.SelectSingleNode("Ico").InnerText;
                    string appName = node.SelectSingleNode("T").InnerText;
                    string appDeveloper = node.SelectSingleNode("Dev").InnerText;
                    string appRatingCount = node.SelectSingleNode("Src").InnerText;
                    string appPackageName = appId;
                    if (node.SelectSingleNode("Pfn") != null)
                    {
                        appPackageName = node.SelectSingleNode("Pfn").InnerText;
                    }
                    string appInfo = appId + ";" + appImageLink + ";" + appPackageName + ";" + appName + ";" + appDeveloper + ";" + appRatingCount;
                    appArray.Add(appInfo);
                }
                return appArray;
            }
            catch (Exception ex)
            {
                CrashTracker.generateReport(ex.ToString());
                return appArray;
            }
        }
        public static List<string> getComments(string language, string appId, int pagenum)
        {
            List<string> appComments = new List<string>();
            string apiSearch = "https://next-services.apps.microsoft.com/4R/6.3.9600-0/788/"+language+"/m/ES/Apps/"+appId+"/Reviews/all/s/helpful/1/pn/"+ pagenum + "/";
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(apiSearch);
                XmlNodeList nodes = doc.DocumentElement.SelectNodes("/Reviews/Review");
                foreach (XmlNode node in nodes)
                {
                    string reviewId = node.SelectSingleNode("ReviewID").InnerText;
                    string reviewTitle = node.SelectSingleNode("ReviewTitle").InnerText;
                    string reviewRating = node.SelectSingleNode("Rating").InnerText;
                    string reviewComment = node.SelectSingleNode("Comment").InnerText;
                    string reviewUpdateDate = node.SelectSingleNode("LastUpdatedDate").InnerText;
                    string reviewHelpPositive = node.SelectSingleNode("Helpfulness").SelectSingleNode("Yes").InnerText;
                    string reviewHelpTotal = node.SelectSingleNode("Helpfulness").SelectSingleNode("Total").InnerText;
                    string reviewComments = reviewId + "[REVIEWDIALOG]" + reviewRating + "[REVIEWDIALOG]" + reviewTitle + "[REVIEWDIALOG]" + reviewComment + "[REVIEWDIALOG]" + reviewUpdateDate + "[REVIEWDIALOG]" + reviewHelpPositive + "[REVIEWDIALOG]" + reviewHelpTotal;
                    appComments.Add(reviewComments);
                }
                return appComments;
            }
            catch (Exception ex)
            {
                CrashTracker.generateReport(ex.ToString());
                return appComments;
            }
        }
        public static string getAppInfo(string language, string country, string appId)
        {
            string appInfo = null;
            string apiSearch = "https://next-services.apps.microsoft.com/browse/6.3.9600-0/788/"+language+"_"+country+"/c/ES/cp/10005001/Apps/"+appId;
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(apiSearch);
                XmlNodeList nodes = doc.DocumentElement.SelectNodes("/Emr/Pt/Ats/At/Imgs/Img");
                string appImageLink = doc.DocumentElement.SelectSingleNode("/Emr/Pt/Ico").InnerText;
                foreach (XmlNode info in nodes){
                    if(info.SelectSingleNode("T").InnerText == "1")
                    {
                        appImageLink = info.SelectSingleNode("U").InnerText;
                    }
                }
                string appName = doc.DocumentElement.SelectSingleNode("/Emr/Pt/T").InnerText;
                string appDesc = doc.DocumentElement.SelectSingleNode("/Emr/D").InnerText;
                string appDeveloper = doc.DocumentElement.SelectSingleNode("/Emr/Dev").InnerText;
                string appPackageName = doc.DocumentElement.SelectSingleNode("/Emr/Pt/Pfn").InnerText;
                string appRatingCount = doc.DocumentElement.SelectSingleNode("/Emr/Pt/Src").InnerText;
                string appBackgroundColor = doc.DocumentElement.SelectSingleNode("/Emr/Pt/Bg").InnerText;
                appInfo = appId + "[SEPARATORFORPARSING]" + "http://wscont1.apps.microsoft.com/winstore/1x/" + appImageLink + "[SEPARATORFORPARSING]" + appPackageName + "[SEPARATORFORPARSING]" + appName + "[SEPARATORFORPARSING]" + appDesc + "[SEPARATORFORPARSING]" + appDeveloper + "[SEPARATORFORPARSING]" + appRatingCount + "[SEPARATORFORPARSING]" + appBackgroundColor;
                return appInfo;
            }
            catch (Exception ex)
            {
                CrashTracker.generateReport(ex.ToString());
                return appInfo;
            }
        }
        public static async Task<string> getAppFile(string appId, string arch, string language, string country)
        {
            string apiSearch = "https://next-services.apps.microsoft.com/browse/6.3.9600-0/788/" + language + "_" + country + "/c/ES/cp/10005001/Apps/" + appId;
            string appFile = null;
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(apiSearch);
                string appPfn = doc.DocumentElement.SelectSingleNode("/Emr/Pt/Pfn").InnerText;
                string[] appPfnIdentifier = appPfn.Split('.');
                HttpClient client = new HttpClient();
                var dataPost = new Dictionary<string, string>
                {
                    { "type", "PackageFamilyName" },
                    { "url", appPfn },
                    { "ring", "RP" },
                    { "lang", language }
                };
                var encodedData = new FormUrlEncodedContent(dataPost);
                var rawResponse = client.PostAsync("https://store.rg-adguard.net/api/GetFiles", encodedData).Result;
                string htmlResponse = "<html xmlns='http://www.w3.org/1999/xhtml'>" + rawResponse.Content.ReadAsStringAsync().Result + "</html>";
                var readerDocument = new HtmlDocument();
                readerDocument.LoadHtml(htmlResponse);
                HtmlNodeCollection nodes = readerDocument.DocumentNode.SelectNodes("//a");
                if (nodes != null)
                {
                    foreach (HtmlNode node in nodes)
                    {
                        var linkName = node.InnerHtml;
                        if (linkName.Contains(arch) || linkName.Contains("x86") || linkName.Contains("neutral"))
                        {
                            //hey fix this cause we send cateogry id and not packg id
                            if (linkName.Contains(".appx") && linkName.Contains(appPfnIdentifier[0]))
                            {
                                appFile = node.Attributes["href"].Value;
                            }
                        }
                    }
                }
                return appFile;
            }catch(Exception ex)
            {
                CrashTracker.generateReport(ex.ToString());
                return appFile;
            }
        }
    }
}
