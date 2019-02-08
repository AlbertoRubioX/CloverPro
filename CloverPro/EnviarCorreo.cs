using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Net;
using System.Net.Mail;
using Logica;

namespace CloverPro
{
    public class EnviarCorreo
    {
        public string Para { get; set; }
        public string Asunto { get; set; }
        public string Mensaje { get; set; }
        public int FolioMail { get; set; }

        public static string Enviar(EnviarCorreo enviar)
        {

            DataTable dtMail = new DataTable();
            dtMail = ConfigLogica.Consultar();
            if (dtMail.Rows.Count > 0)
            {
                try
                {
                    MailMessage message = new MailMessage();
                    SmtpClient smtp = new SmtpClient();

                    message.IsBodyHtml = true;
                    smtp.Host = dtMail.Rows[0]["servidor"].ToString();
                    smtp.Port = Int32.Parse(dtMail.Rows[0]["puerto"].ToString());
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(dtMail.Rows[0]["usuario"].ToString(), dtMail.Rows[0]["password"].ToString());

                    if (dtMail.Rows[0]["ind_ssl"].ToString() == "1")
                        smtp.EnableSsl = true;
                    else
                        smtp.EnableSsl = false;

                    message.From = new MailAddress(dtMail.Rows[0]["correo_sal"].ToString());
                    message.To.Add(enviar.Para);
                    
                    message.Subject = enviar.Asunto;
                    message.Body = enviar.Mensaje;

                    if (dtMail.Rows[0]["ind_html"].ToString() == "1")
                        message.IsBodyHtml = true;
                    else
                        message.IsBodyHtml = false;

                    smtp.Send(message);
                    message.Dispose();

                    string sMsg = "OK";
                    return sMsg;
                }
                catch (Exception ex)
                {
                    string sErr = "Error al intentar enviar el correo" + Environment.NewLine + ex.ToString();
                    return sErr;
                }
            }
            return "No se ha especificado la configuración SMTP";
        }

        public static string EnviaAlerta(EnviarCorreo enviar)
        {

            DataTable dtMail = new DataTable();
            dtMail = ConfigLogica.Consultar();
            if (dtMail.Rows.Count > 0)
            {
                try
                {
                    MailMessage message = new MailMessage();
                    SmtpClient smtp = new SmtpClient();

                    message.IsBodyHtml = true;
                    smtp.Host = dtMail.Rows[0]["servidor"].ToString();
                    smtp.Port = Int32.Parse(dtMail.Rows[0]["puerto"].ToString());
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(dtMail.Rows[0]["usuario"].ToString(), dtMail.Rows[0]["password"].ToString());

                    if (dtMail.Rows[0]["ind_ssl"].ToString() == "1")
                        smtp.EnableSsl = true;
                    else
                        smtp.EnableSsl = false;

                    message.From = new MailAddress(dtMail.Rows[0]["correo_sal"].ToString());
                    message.CC.Add(dtMail.Rows[0]["correo_dest"].ToString());

                    //To
                    DataTable dtTo = CorreoLogica.ConsultaDest(enviar.FolioMail,"T");
                    foreach (DataRow row in dtTo.Rows)
                    {
                        string sCorreo = Convert.ToString(row["correo"]);
                        message.To.Add(sCorreo);
                    }
                    //Cc
                    DataTable dtCc = CorreoLogica.ConsultaDest(enviar.FolioMail, "C");
                    foreach (DataRow row in dtCc.Rows)
                    {
                        string sCorreo = Convert.ToString(row["correo"]);
                        message.CC.Add(sCorreo);
                    }
                    //Body
                    string sMensaje = "";
                    DataTable dtBody = CorreoLogica.ConsultaBody(enviar.FolioMail);
                    foreach (DataRow row in dtBody.Rows)
                    {
                        string sBody = Convert.ToString(row["body"]);
                        sMensaje += sBody;
                    }

                    message.Subject = enviar.Asunto;
                    message.Body = sMensaje;

                    if (dtMail.Rows[0]["ind_html"].ToString() == "1")
                        message.IsBodyHtml = true;
                    else
                        message.IsBodyHtml = false;

                    smtp.Send(message);
                    message.Dispose();

                    string sMsg = "OK";
                    return sMsg;
                }
                catch (Exception ex)
                {
                    string sErr = "Error al intentar enviar el correo, verifique la Configuración SMTP" + Environment.NewLine + ex.ToString();
                    return sErr;
                }
            }
            return "No se ha especificado la configuración SMTP";
        }

        public static async Task EnviaAlertaAsync(EnviarCorreo enviar)
        {

            DataTable dtMail = new DataTable();
            dtMail = ConfigLogica.Consultar();
            if (dtMail.Rows.Count > 0)
            {
                try
                {
                    MailMessage message = new MailMessage();
                    SmtpClient smtp = new SmtpClient();

                    message.IsBodyHtml = true;
                    smtp.Host = dtMail.Rows[0]["servidor"].ToString();
                    smtp.Port = Int32.Parse(dtMail.Rows[0]["puerto"].ToString());
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(dtMail.Rows[0]["usuario"].ToString(), dtMail.Rows[0]["password"].ToString());

                    if (dtMail.Rows[0]["ind_ssl"].ToString() == "1")
                        smtp.EnableSsl = true;
                    else
                        smtp.EnableSsl = false;

                    message.From = new MailAddress(dtMail.Rows[0]["correo_sal"].ToString());
                    message.CC.Add(dtMail.Rows[0]["correo_dest"].ToString());

                    //To
                    DataTable dtTo = CorreoLogica.ConsultaDest(enviar.FolioMail, "T");
                    foreach (DataRow row in dtTo.Rows)
                    {
                        string sCorreo = Convert.ToString(row["correo"]);
                        message.To.Add(sCorreo);
                    }
                    //Cc
                    DataTable dtCc = CorreoLogica.ConsultaDest(enviar.FolioMail, "C");
                    foreach (DataRow row in dtCc.Rows)
                    {
                        string sCorreo = Convert.ToString(row["correo"]);
                        message.CC.Add(sCorreo);
                    }
                    //Body
                    string sMensaje = "";
                    DataTable dtBody = CorreoLogica.ConsultaBody(enviar.FolioMail);
                    foreach (DataRow row in dtBody.Rows)
                    {
                        string sBody = Convert.ToString(row["body"]);
                        sMensaje += sBody;
                    }

                    message.Subject = enviar.Asunto;
                    message.Body = sMensaje;

                    if (dtMail.Rows[0]["ind_html"].ToString() == "1")
                        message.IsBodyHtml = true;
                    else
                        message.IsBodyHtml = false;

                    await smtp.SendMailAsync(message);
                    message.Dispose();
                }
                catch
                {
                    throw;
                }
            }
         
        }
    }
}
