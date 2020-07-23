using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazog.Models;

namespace Blazog.Services
{
    public class PostService : IPostService
    {
        public async Task<IEnumerable<PostInfo>> GetPostsAsync(int page)
        {
            var posts = await LoadPostInfosAsync();
            return posts;
        }

        public async Task<string> GetPostAsync(string label)
        {
            var post = await LoadPostAsync(label);
            return post;
        }

        public async Task<IEnumerable<TagInfo>> GetTagsAsync()
        {
            var tags = await LoadTagsAsync();
            return tags;
        }

        public async Task<IEnumerable<PostInfo>> GetTagAsync(string tag)
        {
            var posts = await LoadTagAsync(tag);
            return posts;
        }

        private async Task<IEnumerable<PostInfo>> LoadPostInfosAsync()
        {
            var posts = new List<PostInfo> {
                new PostInfo { 
                    Title = "First Post", 
                    Label = "first-post", 
                    Blurb = "This is the first post",
                    Tags = new string[] { "About", "Housekeeping" }
                },
                new PostInfo {
                    Title = "Second Post",
                    Label = "second-post",
                    Blurb = "This is the second post",
                    Tags = new string[] {}
                }
            };

            return await Task.FromResult(posts); 
        }

        private async Task<string> LoadPostAsync(string label)
        {
            var content = "Post not found";

            switch(label)
            {
                case "first-post":
                    content = @"<h2>First Post</h2>
                                <div>Hello, First Post!</div>";
                    break;
                case "second-post":
                    content = @"<h2>Second Post</h2>
                                <div>Yeah, I know it. I'm the second post.</div>";
                    break;
            }

            return await Task.FromResult(content);
        }

        private async Task<IEnumerable<TagInfo>> LoadTagsAsync()
        {
            var tags = new List<TagInfo> {
                new TagInfo { 
                    Title = "About",
                    PostCount = 1,
                    About = "Info about the site"
                },
                new TagInfo { 
                    Title = "Housekeeping",
                    PostCount = 1,
                    About = "Posts on general housekeeping"
                },
                new TagInfo { 
                    Title = "Blazor",
                    PostCount = 0,
                    About = "Blazor web development"
                }
            };

            return await Task.FromResult(tags);
        }

        private async Task<IEnumerable<PostInfo>> LoadTagAsync(string tag)
        {
            var posts = (await LoadPostInfosAsync()).Where(p => p.Tags.Contains(tag));
            return posts; 
        }
    }
}