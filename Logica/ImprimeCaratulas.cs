using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logica
{
   public class ImprimeCaratulas
    {
        public void IESetupFooter()
        {
            string strKey = "Software\\Microsoft\\Internet Explorer\\PageSetup";
            bool bolWritable = true;
            string strHName = "header";
            object hValue = "";
            string strFName = "footer";
            object fValue = "";
            string strLandName = "PageOrientation";
            object lValue = "2";
            RegistryKey oKey = Registry.CurrentUser.OpenSubKey(strKey, bolWritable);
            Console.Write(strKey);
            oKey.SetValue(strHName, hValue);
            oKey.SetValue(strFName, fValue);
            oKey.SetValue(strLandName, lValue);
            oKey.Close();
        }

        public void imprimeRotolos(string color1, string color2, string dia, string rpo, string planta, string linea, string almac, string modelo, string fecha, string cant,int tarimas)
        {
            IESetupFooter();
            if (tarimas > 0)
            {
                string html = "<!DOCTYPE html> <html>" +
                                "<head>" +
                                    "<meta charset='utf-8'/>" +
                                    "<style type'text/css'>" +
                                        "#header{" +
                                            "width:980px; height:0px;" +
                                        "}" +
                                        "#footer{" +
                                            "width:980px; height:0px;" +
                                            "height:10px;" +
                                        "}" +
                                        "#text{" +
                                            "position: relative;" +
                                            "top:-10px;" +
                                        "}" +
                                        "#text>p{" +
                                            "font-size:120px;" +
                                            "margin-top:-110px" +
                                        "}" +
                                        "#center-style{" +
                                            "width:980px;" +
                                            "text-align:center;" +
                                        "}" +
                                        "body{" +
                                            "width: 980px;" +
                                            "font-family: 'calibri', bold;" +
                                            "" +
                                        "}" +
                                        "h1{" +
                                            "font-size:155px;" +
                                            "text-align:center;" +
                                        "}" +
                                        "h2{" +
                                            "font-size:60px;" +
                                        "}" +
                                        "h3{" +
                                            "font-size:30px;" +
                                            "text-align:center;" +
                                        "}" +
                                    "</style>" +
                                "</head>" +
                                "<body>";
                for (int i = 1; i <= tarimas; i++)
                {
                    html += "<div style='border-width:20px !important; border-top:120px !important; border:solid " + color1 + ";'>" +
                                "<div id='header' style='text-align:center;'>" +
                                    "<strong id='text'><p>" + dia + "</p></strong>" +
                                "</div>" +
                           "<div style='position:relative;z-index:1;margin-left:-20px;margin-right:-20px!important;border-width:20px!important; border-right:solid " + color2 + " !important; border-left:solid " + color2 + " !important;'>" +
                                "<div style='padding-top: 0px;text-align:center;'>" +
                                    "<div style='width:980px;'><h1>RPO</h1></div>" +
                                "</div>" +
                                 "<div style='padding-top: 0px; text-align:center;'>" +
                                    "<div style='width:980px;'><h1><strong><u>" + rpo + "</u></strong><h1></div>" +
                                "</div>" +
                                "<div style='padding-top: 10px;'>" +
                                    "<div style='width:980px;'><h2>" + planta + "</h2></div>" +
                                "</div>" +
                                "<div style='padding-top: 10px;text-align:center;'>" +
                                    "<div style='width:980px;'><h1>" + linea + "</h1></div>" +
                                "</div>" +
                                "<div style='padding-top: 10px;'>" +
                                    "<div style='width:980px position:relative;z-index:2;margin-left:-20px;margin-right:-20px!important;border-width:20px!important; border-right:solid " + color1 + " !important; border-left:solid " + color1 + " !important;;text-align:center;'><h2>" + almac + "</h2></div>" +
                                "</div>" +
                                "<div style='padding-top: 10px;'>" +
                                    "<div style='width:980px;text-align:center;'><h2>MODELO</h2></div>" +
                                "</div>" +
                                "<div style='padding-top: 10px;'>" +
                                    "<div style='width:980px;text-align:center;'><h2>" + modelo + "</h2></div>" +
                                "</div>" +
                                "<div style='padding-top: 30px;'>" +
                                    "<div style='width:980px;text-align:center;'><h3>PALLETS</h3></div>" +
                                "</div>" +
                                "<div style='padding-top: 10px;'>" +
                                    "<div style='width:980px;text-align:center;'><h2>" + i + " DE " + tarimas + "</h2></div>" +
                                "</div>" +
                                "<div style='padding-top: 30px;'>" +
                                    "<div style='width:980px;text-align:center;'><h2>DATE: " + fecha + " &nbsp; &nbsp;  &nbsp; &nbsp;  &nbsp; &nbsp; &nbsp; &nbsp; QTY: " + cant + "</h2></div>" +
                                    
                                "</div>" +
                           "</div>" +
                                "<div style='padding-top: 10px;'>" +
                                "</div>" +
                           "</div>";
                }
                html += "</body>" +
                           "</html>";
                PrintHelpPage(html);
            }
        }


        public void PrintHelpPage(string web)
        {
            var br = new WebBrowser();
            var th = new Thread(() =>
            {
                br.DocumentCompleted += PrintDocument;
                br.DocumentText = web;
                Application.Run();
            });
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        private void PrintDocument(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var browser = sender as WebBrowser;
            //browser.ShowPrintDialog();
            // Print the document now that it is fully loaded.
            browser.Print();
            // Dispose the WebBrowser now that the task is complete. 
            browser.Dispose();
        }
    }
}
