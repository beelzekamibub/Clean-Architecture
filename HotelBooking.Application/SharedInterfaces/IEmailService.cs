using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.SharedInterfaces
{
    public interface IEmailService
    {
        Task SendEmail(string body, string To);
    }
}
