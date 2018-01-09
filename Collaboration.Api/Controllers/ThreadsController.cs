using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Collaboration.Core.Models;
using Collaboration.Data.Repositories;
using Collaboration.Data.Contexts;

namespace Collaboration.Api.Controllers
{
    [Route("api/Threads")]
    public class ThreadsController : Controller
    {
        private static DbContextOptions<ThreadContext> options = new DbContextOptions<ThreadContext>();
        private static ThreadContext context = new ThreadContext(options);
        //private static ThreadRepository threadRepo = new ThreadRepo(context);
        //private static PostRepository postRepo = new PostRepo(context);

        // Not all of these may be necessary. Some may be part of SignalR hub?

        /*
         * BASIC FUNCTIONALITY:
         * -Get all threads for a document
         * -Get all posts for a thread
         * -Create a new discussion thread
         * -Create a reply post on an existing thread
         */

        // Get all threads for a document
        [HttpGet]
        [Route("api/Threads/Get/{docID:int}")]
        public IEnumerable<Thread> GetThreads(int docID)
        {
            //return threadRepo.GetThreads(docID);
            return new List<Thread>();

        }

        // Get all posts for a thread
        [HttpGet]
        [Route("api/Threads/GetPosts/{threadID:int}")]
        public IEnumerable<Post> GetPosts(int threadID)
        {
            //return postRepo.GetPosts(threadID);
            return new List<Post>();
        }

        // Create new discussion thread
        [HttpPost]
        [Route("api/Threads/Create")]
        public void CreateThread([FromBody]int docID, string subject)
        {
            Thread thread = new Thread();
            thread.DocumentId = docID;
            thread.Title = subject;

            context.Threads.Add(thread);
        }

        // Create a post on an existing thread
        [HttpPost]
        public void CreatePost([FromBody]int threadID, string message, int userID)
        {
            Post post = new Post();
            post.ThreadId = threadID;
            post.Content = message;
            post.TimeStamp = DateTime.Now;

            context.Posts.Add(post);
        }
        
        /*
         * ADVANCED FUNCTIONALITY:
         * -Post attachments
         *      Some discussions may involve multiple documents. Collaboration provides the ability
         *      to attach one or more documents to a post. Maybe we could generate docpop links to
         *      display in the chat?
         * -Thread security
         *      Restrict which users can see your thread. For example, if HR is having a chat,
         *      it may not be appropriate for all OnBase users to be able to view their discussions.
         *      Collaboration allows the creator of the thread to create either an open discussion that
         *      anyone can join or a restricted thread that only certain users or user groups can access.
         */

    }
}
