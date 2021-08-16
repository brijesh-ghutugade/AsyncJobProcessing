using Maersk.AsyncJobProcessing.Data;
using Maersk.AsyncJobProcessing.DTO;
using Maersk.AsyncJobProcessing.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Maersk.AsyncJobProcessing.Service
{
    public class JobProcessor : BackgroundService
    {
        public JobProcessor()
        {

        }
        //IUnitOfWork _unitOfWork;
        ILogger _logger;
        IServiceProvider _provider;

        public JobProcessor(IServiceProvider provider, ILogger<JobProcessor> logger)
        {
            _logger = logger;
            _provider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _provider.CreateScope())
                {
                    var _unitOfWork = (IUnitOfWork)scope.ServiceProvider.GetRequiredService(typeof(IUnitOfWork));
                    var pendingJobs = await _unitOfWork.JobRepository.FindBy(j => j.Status.Equals(JobStatus.Pending.ToString()));

                    _logger.LogInformation("Looking for Pending jobs at '{0}'", DateTime.UtcNow);
                    foreach (var job in pendingJobs)
                    {
                        await Process(_unitOfWork, job);
                    }
                }
               
            }
        }

        public async Task Process(IUnitOfWork _unitOfWork, Job job)
        {
            _logger.LogInformation("Processing job JobId:'{JobId}'.", job.Id);

            var stopwatch = Stopwatch.StartNew();
            var input = Array.ConvertAll(job.Input.Split(','), value => Convert.ToInt32(value));
            var output = input.OrderBy(n => n).ToArray();
            var duration = stopwatch.Elapsed;
            job.Status = JobStatus.Completed.ToString();
            job.Duration = duration.Ticks;
            job.Output = string.Join(',', output);

            _logger.LogInformation("Processing completed JobId: '{JobId}' | Duration: '{Duration}'.", job.Id, duration);

            _unitOfWork.JobRepository.Edit(job);
            await _unitOfWork.Save();
        }

    }
}
