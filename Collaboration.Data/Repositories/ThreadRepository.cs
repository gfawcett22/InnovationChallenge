using Collaboration.Data;
using Collaboration.Core.Models;
using Collaboration.Data.Contexts;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using Collaboration.Core.Data;

namespace Collaboration.Data.Repositories
{
    public class ThreadRepository : IThreadRepository
    {
        private ThreadContext _context;
        public ThreadRepository(ThreadContext context)
        {
            _context = context;
        }
        public IEnumerable<Thread> GetThreads() => _context.Threads.ToList();

        public void AddThread(Thread post)
        {
            _context.Threads.Add(post);
        }

        public Thread GetThread(int documentId) => _context.Threads.FirstOrDefault(c => c.DocumentId == documentId);

        public void UpdateThread(Thread post)
        {
            _context.Update(post);
            _context.Entry(post).State = EntityState.Modified;
        }

        public void DeleteThread(Thread post)
        {
            _context.Threads.Remove(post);
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public bool ThreadExists(int threadId)
        {
            return _context.Threads.Any(t => t.ThreadId == threadId);
        }
    }
}
