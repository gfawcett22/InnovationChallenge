using System;

namespace Collaboration.Core.Models
{
    public class Post
    {
        public int PostId;
        public int ThreadId { get; set; }
        public Thread Thread;
        public string Content;
        public string UserName;
        public DateTime TimeStamp;
    }
}
