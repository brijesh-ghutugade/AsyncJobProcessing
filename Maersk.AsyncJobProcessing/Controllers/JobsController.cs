using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maersk.AsyncJobProcessing.DTO;
using Maersk.AsyncJobProcessing.Model;
using Maersk.AsyncJobProcessing.Service;
using Microsoft.AspNetCore.Mvc;

namespace Maersk.AsyncJobProcessing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private IJobService _jobService;

        public JobsController(IJobService jobService)
        {
            _jobService = jobService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> GetJobs()
        {
            var allJobs = await _jobService.GetAll();

            return Ok(allJobs);
        }

        [HttpGet("{jobId}")]
        public async Task<ActionResult<Job>> GetJob(Guid jobId)
        {
            var job = await _jobService.Get(jobId.ToString());

            return Ok(job);
        }


        [HttpPost]
        public async Task<ActionResult<JobDto>> EnqueueJob([FromBody]int[] values)
        {
            var job = new JobDto(
                id: Guid.NewGuid(),
                status: JobStatus.Pending,
                duration: null,
                input: values,
                output: null,
                scheduledAt: DateTime.UtcNow);

            await _jobService.Enque(job);

            return Ok(job);
        }

    }
}
