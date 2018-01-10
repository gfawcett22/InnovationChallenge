using System.Collections.Generic;
using AutoMapper;
using Collaboration.Api.IntegrationEvents.Events;
using Collaboration.Api.Models;
using Collaboration.Core.Data;
using Collaboration.Core.Models;
using Collaboration.Messaging.Models.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Collaboration.Api.Controllers
{
    [Route("/api/[controller]")]
    public class PostsController : Controller
    {
        private IPostRepository _postRepo;
        private IThreadRepository _threadRepo;
        private IMapper _mapper;
        private IEventBus _eventBus;
        public PostsController(IPostRepository postRepo, IThreadRepository threadRepo, IMapper mapper, IEventBus ev)
        {
            _postRepo = postRepo;
            _threadRepo = threadRepo;
            _mapper = mapper;
            _eventBus = ev;
        }

        [HttpGet("{threadId}")]
        public IEnumerable<PostDto> GetPostsForThread(int threadId)
        {
            var postsFromRepo = _postRepo.GetPostsForThread(threadId);
            var mappedPosts = _mapper.Map<IEnumerable<PostDto>>(postsFromRepo);
            return mappedPosts;
        }

        [HttpPost("{threadId}")]
        public IActionResult CreatePost(int threadId,[FromBody] PostToCreateDto post)
        {
            if (!_threadRepo.ThreadExists(threadId)) return BadRequest();
            var mappedPost = _mapper.Map<Post>(post);
            mappedPost.Thread = _threadRepo.GetThread(threadId);
            _postRepo.AddPost(mappedPost);
            if (!_postRepo.Save()) return StatusCode(500);

            var eventMessage = new PostUpdateIntegrationEvent(threadId);
            _eventBus.Publish(eventMessage);

            return StatusCode(201);
        }
    }
}