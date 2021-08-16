using Maersk.AsyncJobProcessing.Data;
using Maersk.AsyncJobProcessing.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Maersk.AsyncJobProcessing.Data
{
    public partial class UnitOfWork : IUnitOfWork
    {
        private DbContext _context;
        private IRepository<Job> _jobRepository;
        public IRepository<Job> JobRepository
        {
            get
            {

                if (_jobRepository == null)
                    _jobRepository = new Repository<Job>(_context);
                return _jobRepository;
            }
            set
            {
                if (value != null)
                    _jobRepository = value;
            }
        }

        
        public UnitOfWork(JobContext context)
        {
            _context = context;
        }

        public async Task Save()
        {
           await _context.SaveChangesAsync();
        }

        private bool disposed = false;
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this._context.Dispose();
                }
            }

            this.disposed = true;
        }
    }
}