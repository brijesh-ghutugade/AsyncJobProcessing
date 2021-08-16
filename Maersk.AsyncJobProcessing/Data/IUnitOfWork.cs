using Maersk.AsyncJobProcessing.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maersk.AsyncJobProcessing.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Job> JobRepository { get; set; }

        Task Save();
    }
}
