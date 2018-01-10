using System;

namespace Collaboration.Core.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public DateTime TimeStamp { get; set; }
        public Thread Thread { get; set; }
    }
}
