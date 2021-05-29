using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using Utils;

namespace Utils
{
    public class SendMail
    {
        private static ILog _log = ResourceFactory.FileLogger(typeof(SendMail));
        public static void Send(string mittente, List<string> destinatari, string oggetto, string corpo)
        {
            Send(mittente, destinatari, oggetto, corpo, null);
        }

        public static void Send(string mittente, List<string> destinatari, string oggetto, string corpo, List<string> allegati)
        {
            Send(mittente, destinatari, oggetto, corpo, allegati, new List<string>(), new List<LinkedResource>());
        }

        public static void Send(string mittente, List<string> destinatari, string oggetto, string corpo, List<string> allegati, List<string> destinatariCC, List<LinkedResource> emailImages)
        {
            MailMessage message;
            try
            {
                //mittente = (mittente != null && mittente != "" ? mittente : "support.gilbarco@mminformatica.it");
                mittente = (mittente != null && mittente != "" ? mittente : "DiorService@mminformatica.it");

                string body = "<div style=\"height: 100vh\">" +
                     "<div style=\"text-align:center;width:100%;background-color: #ededed;border-bottom: 3px solid #ff7171;\"><img src=\"cid:Emailogo\" width=\"350\"></div>" +
                    "<table class=MsoNormalTable border=0 cellpadding=0 style =\"width:100%; height: 100%; background-color:#ededed\">" +
                      "<tr border=0 style=\"height: 20px; background-color:#ededed; border: none;\">" +
                        "<td border=0 style='border:none;'> &nbsp;</td>" +
                        "<td border=0 style='border:none;' colspan=2 >&nbsp;</td>" +
                      "</tr>" +
                       "<tr>" +
                           "<td border=0 style=\"width: 60px; background-color:#ededed; border: none;\"></td>" +
                           "<td style=\"background-color:#FFFFFF;font-family: 'Myriad Set Pro', 'Helvetica Neue', 'Helvetica', 'Arial', sans-serif;\">" +
                               "<div style=\"margin:20px\">";
                body += corpo;
                body += "</div> " +
                            "</td>" +
                            "<td style=\"width: 60px; background-color:#ededed; border: none;\"></td>" +
                        "</tr>" +
                     "<tr style=\"height: 20px; background-color:#ededed; border: none;\">" +
                        "<td>&nbsp;</td>" +
                        "<td>&nbsp;</td>" +
                        "<td>&nbsp;</td>" +
                     "</tr>" +
                    "</table>" +
                    "<table border=0 cellpadding=0 style=\"width:100%; text-align:center; background-color:#ededed;border-top: 3px solid #ff7171;\">" +
                        "<tr>" +
                            "<td style=\"padding:20px;\">" +
                                "<div style=\"font-family: 'Myriad Set Pro', 'Helvetica Neue', 'Helvetica', 'Arial', sans-serif;\">" +
                                    "GILBARCO INTRANET" +
                                "</div>" +
                                "<div>" +
                                    "<a href=\"" + System.Environment.GetEnvironmentVariable("AppDomain") + "/home\" style=\"cursor:pointer\"><img src=\"cid:home\" width=\"30\"></a>" +
                                "</div>" +
                            "</td>" +
                        "</tr>" +
                    "</table>" +
                    "</div>";
                using (message = new MailMessage())
                {
                    message.From = new MailAddress(mittente);

                    message.Subject = oggetto;
                    message.Body = corpo;

                    LinkedResource EmailLogo = new LinkedResource(Path.Combine(ResourceFactory.ApplicationPath, "Images\\Emailogo.png"));
                    EmailLogo.ContentId = "Emailogo";
                    //Added the patch for Thunderbird as suggested by Jorge
                    EmailLogo.ContentType = new ContentType("image/png");
                    LinkedResource HomeLogo = new LinkedResource(Path.Combine(ResourceFactory.ApplicationPath, "Images\\home_logo.png"));
                    HomeLogo.ContentId = "home";
                    //Added the patch for Thunderbird as suggested by Jorge
                    HomeLogo.ContentType = new ContentType("image/png");

                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body,
                      null, "text/html");

                    htmlView.LinkedResources.Add(EmailLogo);
                    htmlView.LinkedResources.Add(HomeLogo);

                    foreach (LinkedResource image in emailImages)
                    {
                        htmlView.LinkedResources.Add(image);
                    }

                    message.AlternateViews.Add(htmlView);

                    // Aggiungo i destinatari al messaggio
                    foreach (string destinatario in destinatari)
                    {
                        message.To.Add(destinatario);
                    }

                    // Aggiungo i destinatari  in  CC al messaggio
                    foreach (string destinatarioCC in destinatariCC)
                    {
                        message.CC.Add(destinatarioCC);
                    }

                    // Aggiungo eventuali allegati al messaggio
                    if (allegati != null && allegati.Count > 0)
                    {
                        foreach (string nomeFile in allegati)
                        {
                            message.Attachments.Add(new Attachment(nomeFile));
                        }
                    }
                    //message.Bcc.Add(new MailAddress("technical.support@ethichotel.com"));
                    SmtpClient client = new SmtpClient("smtp.office365.com", 587);
                    client.EnableSsl = true;
                    client.Credentials = new System.Net.NetworkCredential(mittente, "+EmmeLeather2018");
                    message.IsBodyHtml = true;
                    message.SubjectEncoding = System.Text.Encoding.UTF8;
                    client.Send(message);
                }
            }
            catch (Exception ex)
            {
                throw new CustomException("Errore durante l'invio della mail", ex);
            }
        }

        public static void SendError(string nomeProcedura, string errore, string user = "", string note = "")
        {
            string mittente = null;
            List<string> destinatari = new List<string>();
#if(DEBUG)
            destinatari.Add("marco.baratto@minformatica.it");
            destinatari.Add("fabio.giovinazzo@minformatica.it");
            destinatari.Add("emanuele.martinelli@minformatica.it");
#else
            destinatari.Add(ResourceFactory.GetAppSetting("SupportMail"));
#endif
            string oggetto = "Segnalazione errore automatica applicazione";
            string corpo = "";
            corpo = "Data errore: <b>" + System.DateTime.Now.ToLongDateString() + "</b> alle ore <b>" + System.DateTime.Now.ToShortTimeString() + "</b><br/><br/>";
            corpo += $"Server: <b> {Environment.MachineName} </b><br/>";
            corpo += "Procedura/Titolo: <b>" + nomeProcedura + "</b><br/>";
            if (user != "")
            {
                corpo += "User: <b>" + user + "</b><br/><br/>";
            }
            if (note != "")
            {
                corpo += "Note: <b>" + note + "</b><br/><br/>";
            }
            corpo += "Errore per esteso: <br/>" + errore + "<br/>";
            SendMail.Send(mittente, destinatari, oggetto, corpo);
        }
    }
}
