using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Maersk.AsyncJobProcessing.Data;
using Maersk.AsyncJobProcessing.DTO;
using Maersk.AsyncJobProcessing.Model;
using Microsoft.Extensions.Logging;

namespace Maersk.AsyncJobProcessing.Service
{
    public class JobService : IJobService
    {
        private IUnitOfWork _unitOfWork;
        private ILogger<JobService> _logger;

        public JobService(IUnitOfWork unitOfWork, ILogger<JobService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;

        }

        public async Task Enque(JobDto job)
        {
            var newJob = new Job(job);
            await _unitOfWork.JobRepository.Insert(newJob);
            await _unitOfWork.Save();
        }        


        public async Task<JobDto> Get(string id)
        {
            var job = await _unitOfWork.JobRepository.GetById(id);
            return ToDto(job); 
        }

        public async Task<IEnumerable<JobDto>> GetAll()
        {

            List<JobDto> jobDtos = new List<JobDto>();

            var jobs = await _unitOfWork.JobRepository.GetAll();

            foreach (var job in jobs)
            {
                jobDtos.Add(ToDto(job));
            }

            return jobDtos;
        }

        public static JobDto ToDto(Job job)
        {

            JobDto jobDto;
            if (job != null)
            {
                Guid jobId = new Guid(job.Id);
                JobStatus status = (JobStatus)Enum.Parse(typeof(JobStatus), job.Status);

                TimeSpan duration = new TimeSpan(job.Duration);

                int[] input = Array.ConvertAll(job.Input.Split(','), value => Convert.ToInt32(value));

                int[] output = null;

                if (!String.IsNullOrEmpty(job.Output))
                {
                    output = Array.ConvertAll(job.Output.Split(','), value => Convert.ToInt32(value));
                }

                jobDto = new JobDto(jobId, status, duration, input, output, job.ScheduledAt);
            }
            else {
                jobDto = new JobDto();
            }

            return jobDto;
        }

    }
}
