using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.webui.EmailServices
{
    public interface IEmailSender
    {
        //smtp => gmail , hotmail  hostun verdiği ücretli
        //api => sendgrip max 100 email ücretsiz
        //biz interface ile yapıcaz
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
