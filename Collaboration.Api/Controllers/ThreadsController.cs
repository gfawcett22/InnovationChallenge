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
    [Route("/api/[controller]")]
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

        [HttpPost("{docId}")]
        public IActionResult CreateThread(int docId, [FromBody] ThreadToCreateDto thread)
        {
            if (thread == null) return BadRequest();
            thread.DocumentId = docId;
            var mappedThread = _mapper.Map<Thread>(thread);

            _threadRepo.AddThread(mappedThread);
            if (!_threadRepo.Save()) return StatusCode(500);
            
            var eventMessage = new ThreadUpdateIntegrationEvent(thread.DocumentId);
            _eventBus.Publish(eventMessage);
            
            return StatusCode(201);
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
