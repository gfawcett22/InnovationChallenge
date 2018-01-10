using Collaboration.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Collaboration.Api.Models
{
    public class PostDto
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public DateTime TimeStamp { get; set; }
        public int ThreadId { get; set; }
    }
}