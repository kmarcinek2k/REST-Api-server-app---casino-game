using Game.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Game.Options.Services
{
    public interface IPostService
    {
        Task<bool> CreatePostAsync(Post post);

        Task<List<Post>> GetPostsAsync();

        Task<List<Tag>> GetAllTagsAsync();

        Task<Post> GetPostByIdAsync(Guid Id);

        Task<bool> UpdatePostAsync(Post postToUpdate);

        Task<bool> DeletePostAsync(Guid postId);

        Task<bool> UserOwnsPostsAsync(Guid postId, string getUserId);
    }
}
