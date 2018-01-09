using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Collaboration.Core.Models;
using Collaboration.Data.Repositories;
using Collaboration.Data.Contexts;
using Collaboration.Core.Data;

namespace Collaboration.Api.Controllers
{
    [Route("api/Threads")]
    public class ThreadsController : Controller
    {
        private static IThreadRepository _threadRepo;
        private static IPostRepository _postRepo;

        public ThreadsController(IThreadRepository threadRepo, IPostRepository postRepo)
        {
            _threadRepo = threadRepo;
            _postRepo = postRepo;
        }

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
        [Route("/document/{id}")]
        public IEnumerable<Thread> GetThreads(int docId)
        {
            return _threadRepo.GetThreadsForDocument(docId);

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

        }

        // Create a post on an existing thread
        [HttpPost]
        public void CreatePost([FromBody]int threadID, string message, int userID)
        {
            Post post = new Post();
            post.ThreadId = threadID;
            post.Content = message;
            post.TimeStamp = DateTime.Now;

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
