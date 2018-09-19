using System;
using DataEstate.Mailer.Enums;

namespace DataEstate.Mailer.Models.Dtos
{
    public class MailResponse
    {
        public string Message { get; set; }

        //Varies from service to service. 
        public string Id { get; set; }

        public string Status { get; set; }
    }
}
