using Collaboration.Api.Models;
using Collaboration.Core.Data;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Collaboration.Api.Hubs
{
    public class CollaborationService : ICollaborationService
    {
        private readonly IHubContext<CollaborationHub> _hubContext;
        private IThreadRepository _threadRepo;
        private IPostRepository _postRepo;
        private IMapper _mapper;
        public CollaborationService(IHubContext<CollaborationHub> hubContext, IThreadRepository threadRepo, IPostRepository postRepo, IMapper mapper)
        {
            _hubContext = hubContext;
            _threadRepo = threadRepo;
            _postRepo = postRepo;
            _mapper = mapper;
        }

        public Task PushOutNewThreadsForDocument(int docId)
        {
            var threadsForDocument = _threadRepo.GetThreadsForDocument(docId);
            if (threadsForDocument == null) return Task.CompletedTask;
            
            var mappedThreads = _mapper.Map<IEnumerable<ThreadDto>>(threadsForDocument);
            var threadsToReturn = Newtonsoft.Json.JsonConvert.SerializeObject(mappedThreads);
            return _hubContext.Clients.All.InvokeAsync("GetThreads", threadsToReturn);
        }

        public Task PushOutPostsForThread(int threadId)
        {
            var postsForThread = _postRepo.GetPostsForThread(threadId);
            if (postsForThread == null) return Task.CompletedTask;
            
            var mappedPosts = _mapper.Map<IEnumerable<PostDto>>(postsForThread);
            var postsToReturn = Newtonsoft.Json.JsonConvert.SerializeObject(mappedPosts);
            return _hubContext.Clients.All.InvokeAsync("GetPosts", postsToReturn);
        }
    }

    public interface ICollaborationService
    {
        Task PushOutNewThreadsForDocument(int docId);
        Task PushOutPostsForThread(int threadId);
    }
}
