using Collaboration.Data;
using Collaboration.Core.Models;
using Collaboration.Data.Contexts;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Collaboration.Data.Repositories
{
    public class PostRepo
    {
        private ThreadContext _context;
        public PostRepo(ThreadContext context)
        {
            _context = context;
        }
        public IEnumerable<Post> GetPosts(int ThreadId) => _context.Posts.Where(t => t.ThreadId == ThreadId);
        public bool PostsExists(int ThreadId) => _context.Posts.Any(t => t.ThreadId == ThreadId);
    }
}
