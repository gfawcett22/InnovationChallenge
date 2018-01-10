using Collaboration.Api.Models;
using Collaboration.Core.Data;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Collaboration.Api.Hubs
{
    public class CollaborationService : ICollaborationService
    {
        private readonly IHubContext<CollaborationHub> _hubContext;
        private IThreadRepository _threadRepo;
        private IPostRepository _postRepo;
        public CollaborationService(IHubContext<CollaborationHub> hubContext, IThreadRepository threadRepo, IPostRepository postRepo)
        {
            _hubContext = hubContext;
            _threadRepo = threadRepo;
            _postRepo = postRepo;
        }

        public Task PushOutNewThreadsForDocument(int docId)
        {
            var threadsForDocument = _threadRepo.GetThreadsForDocument(docId);
            if (threadsForDocument != null)
            {
                return _hubContext.Clients.All.InvokeAsync("ThreadsUpdate", new ThreadsDto(docId, threadsForDocument));
            }
            return Task.CompletedTask;
        }

        public Task PushOutPostsForThread(int threadId)
        {
            var postsForThread = _postRepo.GetPostsForThread(threadId);
            if (postsForThread != null)
            {
                return _hubContext.Clients.All.InvokeAsync("PostsUpdate", new PostsDto(threadId, postsForThread));
            }
            return Task.CompletedTask;
        }
    }

    public interface ICollaborationService
    {
        Task PushOutNewThreadsForDocument(int docId);
        Task PushOutPostsForThread(int threadId);
    }
}
