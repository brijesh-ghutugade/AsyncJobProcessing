using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maersk.AsyncJobProcessing.DTO
{
    public class JobDto
    {
        public JobDto()
        {

        }

        public JobDto(Guid id, JobStatus status, TimeSpan? duration, IReadOnlyCollection<int> input, IReadOnlyCollection<int> output, DateTime scheduledAt)
        {
            Id = id;
            Status = status;
            Duration = duration;
            Input = input;
            Output = output;
            ScheduledAt = scheduledAt;
        }

        public Guid Id { get; }
        public JobStatus Status { get; }
        public TimeSpan? Duration { get; }
        public IReadOnlyCollection<int> Input { get; }
        public IReadOnlyCollection<int> Output { get; }
        public DateTime ScheduledAt { get; }
    }

    public enum JobStatus
    {
        Pending,
        Completed
    }
}
