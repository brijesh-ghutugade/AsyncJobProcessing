using Maersk.AsyncJobProcessing.DTO;
using Maersk.AsyncJobProcessing.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maersk.AsyncJobProcessing.Service
{
    public interface IJobService
    {

        Task Enque(JobDto job);

        Task<IEnumerable<JobDto>> GetAll();

        Task<JobDto> Get(string id);

        Task<JobDto> Execute(JobDto job);

    }
}
