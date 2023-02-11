using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;

namespace sdir.verify.india
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = @"C:\Users\helge\source\repos\sdir.verify.india\testdata\india.html";
            string html = File.ReadAllText(url);

            var verify =  ParseData(html);

            Console.WriteLine("Name of  Seafarer: " + verify.Name);
            Console.WriteLine("Date of Birth:" + verify.DateOfBirth);
            Console.WriteLine("INDoS No.:" + verify.INDoSNo);

            Console.WriteLine("Sr. No.:" + verify.SrNo);
            Console.WriteLine("Grade:" + verify.Grade);
            Console.WriteLine("Limitation:" + verify.Limitation);
            Console.WriteLine("Distinctive Characters in the Certificate**:" + verify.CertificateNo);
            Console.WriteLine("Date Of Issue:" + verify.DateIssued);
            Console.WriteLine("Valid Till Date:" + verify.ValidTillDate);
            Console.WriteLine("Revalidation Issue Date:" + verify.RevalidationIssueDate);
            Console.WriteLine("Revalidation Valid Till Date:" + verify.RevalidationValidTillDate);
            Console.WriteLine("Verified :" + verify.Verified);
            Console.WriteLine("Status :" + verify.Status);
        }



        public static VerifyIndia ParseData(string html)
        {
            var verify = new VerifyIndia();
            DateTime parsedDate;

            HtmlDocument htmlSnippet = new HtmlDocument();
            htmlSnippet.LoadHtml(html);


            var tables = htmlSnippet.DocumentNode.Descendants("table").Where(n => n.GetAttributeValue("cellpadding", "").Equals("3", StringComparison.InvariantCultureIgnoreCase));

            var seafarerDetails = tables?.FirstOrDefault()?.Descendants("tr");
            if(seafarerDetails != null)
            {
                foreach (var row in seafarerDetails)
                {
                    var cols = row.Descendants("td");
                    if (cols.Any(x => x.InnerText.Contains("Name of  Seafarer", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        verify.Name = cols.Skip(1).FirstOrDefault()?.InnerText.Clean();
                    }
                    if (cols.Any(x => x.InnerText.Contains("Date of Birth", StringComparison.InvariantCultureIgnoreCase)))
                    {

                        if (DateTime.TryParse(cols.Skip(1).FirstOrDefault()?.InnerText, out parsedDate))
                        {
                            verify.DateOfBirth = parsedDate;
                        }
                    }
                    if (cols.Any(x => x.InnerText.Contains("INDoS No.", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        verify.INDoSNo = cols.Skip(1).FirstOrDefault()?.InnerText.Clean();
                    }
                }
            }


         
            var certificateData = tables?
                .Skip(1).FirstOrDefault()?
                .Descendants("tr").Skip(1).FirstOrDefault()?
                .Descendants("td");
            verify.SrNo = certificateData?.FirstOrDefault()?.InnerText.Clean();
            verify.Grade = certificateData?.Skip(1).FirstOrDefault()?.InnerText.Clean();
            verify.Limitation =  certificateData?.Skip(2).FirstOrDefault()?.InnerText.Clean();
            verify.CertificateNo = certificateData?.Skip(3).FirstOrDefault()?.InnerText.Clean();

     
            if (DateTime.TryParse(certificateData?.Skip(4).FirstOrDefault()?.InnerText, out parsedDate))
            {
                verify.DateIssued = parsedDate;
            }
            if (DateTime.TryParse(certificateData?.Skip(5).FirstOrDefault()?.InnerText, out parsedDate))
            {
                verify.ValidTillDate = parsedDate;
            }
            if (DateTime.TryParse(certificateData?.Skip(6).FirstOrDefault()?.InnerText, out parsedDate))
            {
                verify.RevalidationIssueDate = parsedDate;
            }
            if (DateTime.TryParse(certificateData?.Skip(7).FirstOrDefault()?.InnerText, out parsedDate))
            {
                verify.RevalidationValidTillDate = parsedDate;
            }

            verify.Verified = certificateData?.Skip(8).FirstOrDefault()?.InnerText.Clean();
            verify.Status = certificateData?.Skip(9).FirstOrDefault()?.InnerText.Clean();

            return verify;
        }



        public class VerifyIndia
        {
            public string Name { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string INDoSNo { get; set; }
            public string SrNo { get; set; }
            public string Grade { get; set; }
            public string Limitation { get; set; }
            public string CertificateNo { get; set; }
            public DateTime DateIssued { get; set; }
            public DateTime ValidTillDate { get; set; }
            public DateTime RevalidationIssueDate { get; set; }
            public DateTime RevalidationValidTillDate { get; set; }
            public string Verified { get; set; }
            public string Status { get; set; }
        }



    }
}
