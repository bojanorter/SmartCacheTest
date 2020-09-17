using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Service
{
    public interface IClientService
    {
        IClusterClient GetClient();
    }
}
