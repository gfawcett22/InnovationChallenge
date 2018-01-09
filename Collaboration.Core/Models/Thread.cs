using System;
using System.Collections.Generic;
using System.Text;

namespace Collaboration.Core.Models
{
    public class Thread
    {
        public int ThreadId;
        public int DocumentId { get; set; }
        public string Title;
        public ICollection<Post> Posts;
    }
}
