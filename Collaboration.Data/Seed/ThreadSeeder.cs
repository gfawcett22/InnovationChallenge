using Collaboration.Core.Models;
using Collaboration.Data.Contexts;
using System.Collections.Generic;

namespace Collaboration.Data.Seed
{
    public static class ThreadSeeder
    {
        public static void Seed(ThreadContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            //Documents
            var docList = new List<Document>
            {
                new Document { DocumentId = 0, Name = "This is the first document"},
                new Document { DocumentId = 1, Name = "This is the second document"},
                new Document { DocumentId = 2, Name = "This is the third document"},
                new Document { DocumentId = 3, Name = "This is the fourth document"},
                new Document { DocumentId = 4, Name = "This is the fifth document"}
            };
            db.Documents.AddRange(docList);

            //Posts
            var posts = new List<Post>
            {
                new Post { Content = "1. Test Content 0", ThreadId = 0, UserId=0},
                new Post { Content = "2. Test Content 1", ThreadId = 1, UserId=1},
                new Post { Content = "3. Test Content 2", ThreadId = 2, UserId=2},
                new Post { Content = "4. Test Content 3", ThreadId = 3, UserId=3},
                new Post { Content = "5. Test Content 4", ThreadId = 4, UserId=4},
                new Post { Content = "6. Test Content 5", ThreadId = 0, UserId=4},
                new Post { Content = "7. Test Content 6", ThreadId = 1, UserId=0},
                new Post { Content = "8. Test Content 7", ThreadId = 2, UserId=1},
                new Post { Content = "9. Test Content 8", ThreadId = 3, UserId=4},
                new Post { Content = "10. Test Content 9", ThreadId = 4, UserId=3},
                new Post { Content = "11. Test Content 10", ThreadId = 0, UserId=1},
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

            //Users
            var users = new List<User>
            {
                new User { UserId=0, UserName = "Garron"},
                new User { UserId=1, UserName = "Luke"},
                new User { UserId=2, UserName = "Elizabeth"},
                new User { UserId=3, UserName = "Steven"},
                new User { UserId=4, UserName = "Maneesha"}
            };
            db.Users.AddRange(users);
        }
    }
}
