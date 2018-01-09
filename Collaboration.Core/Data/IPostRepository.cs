using Collaboration.Core.Models;
using System.Collections.Generic;

namespace Collaboration.Core.Data
{
    public interface IPostRepository
    {
        IEnumerable<Post> GetPosts();
        Post GetPost(int postId);
        IEnumerable<Post> GetPostsForThread(int threadId);
        void AddPost(Post post);
        void DeletePost(Post post);
        void UpdatePost(Post post);
        bool PostExists(int postId);
        bool Save();
    }
}
