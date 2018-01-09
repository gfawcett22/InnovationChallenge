using Collaboration.Data;
using Collaboration.Core.Models;
using Collaboration.Data.Contexts;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
namespace Collaboration.Data.Repositories
{
    public class ThreadRepo
    {
        private ThreadContext _context;
        public ThreadRepo(ThreadContext context)
        {
            _context = context;
        }
        public IEnumerable<Thread> GetThreads(int DocumentId) => _context.Threads.Where(d => d.DocumentId == DocumentId);
        public bool ThreadsExists(int DocumentId) => _context.Threads.Any(t => t.DocumentId == DocumentId);
    }
}
