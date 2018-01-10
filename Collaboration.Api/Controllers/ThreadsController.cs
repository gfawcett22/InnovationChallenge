using System;
using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using Collaboration.Api.IntegrationEvents.Events;
using Collaboration.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Collaboration.Core.Models;
using Collaboration.Data.Repositories;
using Collaboration.Data.Contexts;
using Collaboration.Core.Data;
using Collaboration.Messaging.Models.Abstractions;

namespace Collaboration.Api.Controllers
{
    [Route("api/threads")]
    public class ThreadsController : Controller
    {
        private IThreadRepository _threadRepo;
        private IPostRepository _postRepo;
        private IMapper _mapper;
        private IEventBus _eventBus;

        public ThreadsController(IThreadRepository threadRepo, IPostRepository postRepo, IMapper mapper, IEventBus eventBus)
        {
            _threadRepo = threadRepo;
            _postRepo = postRepo;
            _mapper = mapper;
            _eventBus = eventBus;
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
        public IEnumerable<ThreadDto> GetThreads()
        {
            var threadsFromRepo = _threadRepo.GetThreads();
            var threadsToReturn = _mapper.Map<IEnumerable<ThreadDto>>(threadsFromRepo);
            return threadsToReturn;
        }
        
        [HttpGet("{docId}")]
        public IEnumerable<ThreadDto> GetThreadsForDocument(int docId)
        {
            var threadsFromRepo = _threadRepo.GetThreadsForDocument(docId);
            var threadsToReturn = _mapper.Map<IEnumerable<ThreadDto>>(threadsFromRepo);
            return threadsToReturn;
        }

        // Get all posts for a thread
        [HttpGet]
        [Route("{threadId:int}/posts")]
        public IEnumerable<PostDto> GetPosts(int threadId)
        {
            var postsFromRepo = _postRepo.GetPostsForThread(threadId);
            var mappedPosts = _mapper.Map<IEnumerable<PostDto>>(postsFromRepo);
            return mappedPosts;
        }

        [HttpPost]
        public IActionResult CreateThread(ThreadToCreateDto thread)
        {
            if (thread == null) return BadRequest();
            var mappedThread = _mapper.Map<Thread>(thread);
            mappedThread.Posts = new List<Post>();

            _threadRepo.AddThread(mappedThread);
            if (!_threadRepo.Save()) return StatusCode(500);
            
            var eventMessage = new ThreadUpdateIntegrationEvent(thread.DocumentId);
            _eventBus.Publish(eventMessage);
            
            return StatusCode(200);
        }

        // Create a post on an existing thread
        [HttpPost("{threadId:int}/posts")]
        public IActionResult CreatePost([FromBody]int threadID, PostToCreateDto post)
        {
            if (!_threadRepo.ThreadExists(threadID)) return BadRequest();
            var mappedPost = _mapper.Map<Post>(post);
            mappedPost.Thread = _threadRepo.GetThread(threadID);
            _postRepo.AddPost(mappedPost);
            if (!_postRepo.Save()) return StatusCode(500);

            var eventMessage = new PostUpdateIntegrationEvent(threadID);
            _eventBus.Publish(eventMessage);

            return StatusCode(200);
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
