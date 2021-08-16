using Maersk.AsyncJobProcessing.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maersk.AsyncJobProcessing.Model
{
    public class Job
    {
        public Job()
        {

        }

        public Job(JobDto job)
        {
            Id = job.Id.ToString();

            Input = string.Join(',', job.Input);

            if (job.Output != null)
                Output = string.Join(',', job.Output);
            else
                Output = string.Empty;

            Status = job.Status.ToString();

            if (job.Duration.HasValue)
                Duration = job.Duration.Value.Ticks;

            ScheduledAt = job.ScheduledAt;

        }

        public string Id { get; set; }

        public string Input { get; set; }

        public string Output { get; set; }

        public string Status { get; set; }

        public long Duration { get; set; }

        public DateTime ScheduledAt { get; set; }
    }
}
