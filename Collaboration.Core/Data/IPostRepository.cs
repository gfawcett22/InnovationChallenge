using Collaboration.Core.Models;
using System.Collections.Generic;

namespace Collaboration.Core.Data
{
    public interface IPostRepository
    {
        IEnumerable<Post> GetPosts();
        Post GetPost(int PostId);
        void AddPost(Post Post);
        void DeletePost(Post Post);
        void UpdatePost(Post Post);
        bool PostExists(int PostId);
        bool Save();
    }
}
