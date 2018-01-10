using Collaboration.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Collaboration.Api.Models
{
    public class PostsDto
    {

        public PostsDto(int threadId, IEnumerable<Post> posts)
        {
            ThreadId = threadId;
            Posts = posts;
        }
        public int ThreadId { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}
