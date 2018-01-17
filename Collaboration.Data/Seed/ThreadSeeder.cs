using Collaboration.Core.Models;
using Collaboration.Data.Contexts;
using System;
using System.Collections.Generic;

namespace Collaboration.Data.Seed
{
    public static class ThreadSeeder
    {
        public static void Seed(ThreadContext db)
        {
            db.Database.EnsureCreated();
            //Threads
            var threads = new List<Thread>
            {
                new Thread { DocumentId = 1, Title = "Title for doc 0"},
                new Thread { DocumentId = 1, Title = "Title for doc 1"},
                new Thread { DocumentId = 1, Title = "Title for doc 2"},
                new Thread { DocumentId = 2, Title = "Title for doc 3"},
                new Thread { DocumentId = 2, Title = "Title for doc 4"},
            };
            db.Threads.AddRange(threads);
            
            //Posts
            var posts = new List<Post>
            {
                new Post { Content = "1. Test Content 0", Thread = threads[0], UserName="Test", TimeStamp = DateTime.Now},
                new Post { Content = "2. Test Content 1", Thread = threads[0], UserName = "Test 1", TimeStamp = DateTime.Now},
                new Post { Content = "3. Test Content 2", Thread = threads[0], UserName = "Test 2", TimeStamp = DateTime.Now},
                new Post { Content = "4. Test Content 3", Thread = threads[1], UserName = "Test 3", TimeStamp = DateTime.Now},
                new Post { Content = "5. Test Content 4", Thread = threads[1], UserName="Test", TimeStamp = DateTime.Now},
                new Post { Content = "6. Test Content 5", Thread = threads[3], UserName="Test", TimeStamp = DateTime.Now},
                new Post { Content = "7. Test Content 6", Thread = threads[3], UserName="Test", TimeStamp = DateTime.Now},
                new Post { Content = "8. Test Content 7", Thread = threads[3], UserName="Test", TimeStamp = DateTime.Now},
                new Post { Content = "9. Test Content 8", Thread = threads[4], UserName="Test", TimeStamp = DateTime.Now},
                new Post { Content = "10. Test Content 9", Thread = threads[4], UserName="Test", TimeStamp = DateTime.Now},
                new Post { Content = "11. Test Content 10", Thread = threads[4], UserName="Test", TimeStamp = DateTime.Now},
            };
            db.Posts.AddRange(posts);

           

            db.SaveChanges();
        }
    }
}
