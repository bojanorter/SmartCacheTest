using GrainInterfaces;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Resources;
using System.Threading.Tasks;

namespace Grains
{
    public class DomainGrain : Grain, IDomainGrain, IRemindable
    {
        private readonly IPersistentState<MailListState> _mailList;

        public DomainGrain([PersistentState("mailList", "mailStore")] IPersistentState<MailListState> mailList)
        {
            _mailList = mailList;
        }

        public override Task OnActivateAsync()
        {
            base.OnActivateAsync();
            RegisterOrUpdateReminder("saveToStorage", TimeSpan.FromSeconds(3), TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }

        public Task AddMail(string mail)
        {
            _mailList.State.mailList.Add(mail);
            Console.WriteLine(mail + " ADDING!");
            
            return Task.CompletedTask;
        }

        public Task<bool> MailExists(string mail)
        {
            bool exists = _mailList.State.mailList.Contains(mail);
            if (exists)
            {
                Console.WriteLine(mail + " exists!");
            }
            else
            {
                Console.WriteLine(mail + " NOT NOT exists!");
            }
            return Task.FromResult(exists);
        }

        public Task WriteToStorage()
        {
            return _mailList.WriteStateAsync();
        }

        public Task ReceiveReminder(string reminderName, TickStatus status)
        {
            Console.WriteLine("Saving state to storage 'remainder' => " + reminderName);
            WriteToStorage();
            return Task.CompletedTask;
        }
    }
}
