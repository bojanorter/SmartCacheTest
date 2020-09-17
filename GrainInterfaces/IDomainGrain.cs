using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrainInterfaces
{
    public interface IDomainGrain : IGrainWithStringKey
    {
        Task<bool> MailExists(string mail);
        Task AddMail(string mail);

        Task WriteToStorage();
    }
}
