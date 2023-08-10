using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitListener.Application.Interfaces
{
    public interface IUrlProcessor
    {
        Task ProcessUrlAsync(string url);
    }
}
