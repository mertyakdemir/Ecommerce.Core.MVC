using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.UI.Email
{
    public interface IEmail
    {
        Task SendEmailAsync(string email, string subject, string text);
    }
}
