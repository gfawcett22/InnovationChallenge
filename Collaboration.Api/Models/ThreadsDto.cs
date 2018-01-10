using Collaboration.Core.Models;
using System.Collections.Generic;

namespace Collaboration.Api.Models
{
    public class ThreadsDto
    {
        public ThreadsDto(int documentId, IEnumerable<Thread> threads)
        {
            DocumentId = documentId;
            Threads = threads;
        }
        public int DocumentId { get; set; }
        IEnumerable<Thread> Threads { get; set; }
    }
}
