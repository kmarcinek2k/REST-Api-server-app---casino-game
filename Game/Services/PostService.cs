using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Data;
using Game.Domain;
using Microsoft.EntityFrameworkCore;

namespace Game.Options.Services
{


    public class PostService : IPostService
    {
        private readonly DataContext _dataContext;

        public PostService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var post = await GetPostByIdAsync(postId);
            if (post == null)
            {
                return false;
            }
            _dataContext.Posts.Remove(post);
            var deleted = await _dataContext.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
         
            await _dataContext.Posts.AddAsync(post);
            var created = await _dataContext.SaveChangesAsync();

            return created > 0;
        }



        public async Task<Post> GetPostByIdAsync(Guid postId)
        {

            return await _dataContext.Posts.SingleOrDefaultAsync(x => x.Id == postId);
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            return await _dataContext.Posts.ToListAsync(); ;
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {

            if (postToUpdate == null)
            {
                return false;
            }
            _dataContext.Posts.Update(postToUpdate);
            var updated = await _dataContext.SaveChangesAsync();

            return updated > 0;
        }


        public async Task<bool> UserOwnsPostsAsync(Guid postId, string getUserId)
        {
            var post = await _dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == postId);

            if (post == null)
            {
                return false;
            }

            if (post.UserId != getUserId)
            {
                return false;
            }
            return true;
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _dataContext.Tags.ToListAsync();
        }
    }
}
