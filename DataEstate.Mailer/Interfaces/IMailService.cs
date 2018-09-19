using System;
using DataEstate.Mailer.Enums;
using DataEstate.Mailer.Models.Dtos;

namespace DataEstate.Mailer.Interfaces
{
    public interface IMailService
    {
        MailResponse Send(MailContent mailBody);
    }
}
