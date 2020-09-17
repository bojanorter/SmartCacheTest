using System;
using System.Collections.Generic;


namespace Grains
{
    [Serializable]
    public class MailListState
    {
        public MailListState()
        {
            mailList = new List<string>();
        }
        public List<string> mailList { get; set; }
    }
}
