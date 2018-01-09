using Collaboration.Core.Models;
using System.Collections.Generic;

namespace Collaboration.Core.Data
{
    public interface IThreadRepository
    {
        IEnumerable<Thread> GetThreads();
        Thread GetThread(int threadId);
        IEnumerable<Thread> GetThreadsForDocument(int documentId);
        void AddThread(Thread thread);
        void DeleteThread(Thread thread);
        void UpdateThread(Thread thread);
        bool ThreadExists(int threadId);
        bool Save();
    }
}
