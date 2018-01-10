using Collaboration.Core.Models;
using System.Collections.Generic;

namespace Collaboration.Api.Models
{
    public class ThreadDto
    {
        public int ThreadId { get; set; }
        public int DocumentId { get; set; }
        public string Title { get; set; }
        IEnumerable<PostDto> Posts { get; set; }
    }
}