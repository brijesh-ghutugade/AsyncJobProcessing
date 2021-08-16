using Maersk.AsyncJobProcessing.Data;
using Maersk.AsyncJobProcessing.DTO;
using Maersk.AsyncJobProcessing.Model;
using Maersk.AsyncJobProcessing.Service;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;

namespace Tests
{
    public class JobProcessorTests
    {
        private Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork>();
        private Mock<ILogger<JobProcessor>> _logger = new Mock<ILogger<JobProcessor>>();
        private Mock<IServiceProvider> _provider = new Mock<IServiceProvider>();

        private JobProcessor _jobProcessor;

        [SetUp]
        public void Setup()
        {
            _jobProcessor = new JobProcessor(_provider.Object, _logger.Object);
        }

        [Test]
        public void When_ProcessCalled_ShouldSortInput_AndSaveToDB()
        {

            Job job = new Job();
            job.Id = Guid.NewGuid().ToString();
            job.Status = JobStatus.Pending.ToString();
            job.Input = "1,7,3,9,4";

            _unitOfWork.Setup(s => s.JobRepository.Edit(It.IsAny<Job>()));
            _unitOfWork.Setup(s => s.Save());

            _jobProcessor.Process(_unitOfWork.Object, job).Wait();

            _unitOfWork.Verify(u => u.JobRepository.Edit(It.Is<Job>(j=>j.Output.Equals("1,3,4,7,9") && j.Status.Equals(JobStatus.Completed.ToString()))), Times.Once);

            _unitOfWork.Verify(u => u.Save(), Times.Once);
        }
    }
}