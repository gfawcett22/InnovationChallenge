using Collaboration.Data;
using Collaboration.Core.Models;
using Collaboration.Data.Contexts;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using Collaboration.Core.Data;

namespace Collaboration.Data.Repositories
{
    public class PostRepository : IPostRepository
    {
        private ThreadContext _context;
        public PostRepository(ThreadContext context)
        {
            _context = context;
        }
        public IEnumerable<Post> GetPosts() => _context.Posts.ToList();

        public Post GetPost(int postId) => _context.Posts.FirstOrDefault(p => p.PostId == postId);

        public IEnumerable<Post> GetPostsForThread(int threadId) => _context.Posts.Where(p => p.ThreadId == threadId);

        public void AddPost(Post post)
        {
            _context.Posts.Add(post);
        }

        public bool PostExists(int postId) => _context.Posts.Any(d => d.PostId == postId);

        public void UpdatePost(Post post)
        {
            _context.Update(post);
            _context.Entry(post).State = EntityState.Modified;
        }

        public void DeletePost(Post post)
        {
            _context.Posts.Remove(post);
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
