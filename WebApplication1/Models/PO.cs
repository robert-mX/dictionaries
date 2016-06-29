using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace WebApplication1.Models
{
    public class PO
    {
        public IEnumerable<HttpPostedFileBase> Files { get; set; }

        public void upload(PO po)
        {
            try
            {
                foreach (var file in po.Files)
                {
                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/dictionaries"), fileName);
                        file.SaveAs(path);

                        po.createDict(file.FileName, "pl");
                        po.createDict(file.FileName, "fr");
                        po.createDict(file.FileName, "de");
                    }
                }
            }
            catch(NullReferenceException)
            {
                // throw...
            }
            
        }

        private string translate(string word, string destLanguage)
        {
            string url = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + word + "&from=&to="+ destLanguage + "&appId=6CE9C85A41571C050C379F60DA173D286384E0F2";
            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Method = "GET";
            WebResponse webResp = webRequest.GetResponse();

            Stream strm = webResp.GetResponseStream();
            StreamReader reader = new System.IO.StreamReader(strm);

            return Regex.Replace(reader.ReadToEnd().ToString(), @"<[^>]+>|&nbsp;", "").Trim();
        }

        public void createDict(string sourcefileName, string destLanguage)
        {
            var fileContents = System.IO.File.ReadAllLines(HttpContext.Current.Server.MapPath(@"~/Content/dictionaries/"+ sourcefileName));

            string[] newFile = new string[fileContents.Length];

            //Dictionary<string, string> dictionary;

            for (int i = 0; i < fileContents.Length; i++)
            {
                if (fileContents[i][0].Equals('"'))
                    newFile[i] = fileContents[i];
                else
                {
                    // msgid
                    if (fileContents[i].Contains("msgid"))
                        newFile[i] = "msgid \"" + fileContents[i].Substring(7).Trim('"') + "\"";
                    // msgstr 
                    else
                        newFile[i] = "msgstr \"" + translate(fileContents[i].Substring(8).Trim('"'), destLanguage) + "\"";
                }

                //Debug.WriteLine(newFile[i]);
            }

            System.IO.File.WriteAllLines(HttpContext.Current.Server.MapPath(@"~/Content/dictionaries/"+ destLanguage + "-source_" +sourcefileName), newFile);
        }

    }
}