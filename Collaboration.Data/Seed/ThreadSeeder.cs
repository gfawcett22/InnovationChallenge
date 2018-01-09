using Collaboration.Core.Models;
using Collaboration.Data.Contexts;
using System;
using System.Collections.Generic;
using System;

namespace Collaboration.Data.Seed
{
    public static class ThreadSeeder
    {
        public static void Seed(ThreadContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            //Posts
            DateTime now = DateTime.Now.AddMinutes(120.0);
            var posts = new List<Post>
            {
                new Post { Content = "1. Test Content 0", ThreadId = 0, UserName="Test"},
                new Post { Content = "2. Test Content 1", ThreadId = 1, UserName="Test"},
                new Post { Content = "3. Test Content 2", ThreadId = 2, UserName="Test"},
                new Post { Content = "4. Test Content 3", ThreadId = 3, UserName="Test"},
                new Post { Content = "5. Test Content 4", ThreadId = 4, UserName="Test"},
                new Post { Content = "6. Test Content 5", ThreadId = 0, UserName="Test"},
                new Post { Content = "7. Test Content 6", ThreadId = 1, UserName="Test"},
                new Post { Content = "8. Test Content 7", ThreadId = 2, UserName="Test"},
                new Post { Content = "9. Test Content 8", ThreadId = 3, UserName="Test"},
                new Post { Content = "10. Test Content 9", ThreadId = 4, UserName="Test"},
                new Post { Content = "11. Test Content 10", ThreadId = 0, UserName="Test"},
            };
            db.Posts.AddRange(posts);

            //Threads
            var threads = new List<Thread>
            {
                new Thread { DocumentId = 0, ThreadId = 0, Title = "Title for doc 0"},
                new Thread { DocumentId = 1, ThreadId = 1, Title = "Title for doc 1"},
                new Thread { DocumentId = 2, ThreadId = 2, Title = "Title for doc 2"},
                new Thread { DocumentId = 3, ThreadId = 3, Title = "Title for doc 3"},
                new Thread { DocumentId = 4, ThreadId = 4, Title = "Title for doc 4"},
            };
            db.Threads.AddRange(threads);

            db.SaveChanges();
        }
    }
}
