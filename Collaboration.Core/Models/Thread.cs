using System;
using System.Collections.Generic;
using System.Text;

namespace Collaboration.Core.Models
{
    public class Thread
    {
        public int ThreadId { get; set; }
        public int DocumentId { get; set; }
        public string Title { get; set; }
        public ICollection<Post> Posts;
    }
}
