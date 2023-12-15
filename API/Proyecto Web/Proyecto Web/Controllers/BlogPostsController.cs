using Microsoft.AspNetCore.Mvc;
using Proyecto_Web.Models.Domain;
using Proyecto_Web.Models.DTO.BlogPosts;
using Proyecto_Web.Repositories.Interface;

namespace Proyecto_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepostitory;

        public BlogPostsController(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepostitory = blogPostRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDTO requestDTO)
        {
            // Convert DTO to Domain
            var blogPost = new BlogPost
            {
                Author = requestDTO.Author,
                Content = requestDTO.Content,
                FeaturedImageUrl = requestDTO.FeaturedImageUrl,
                IsVisible = requestDTO.IsVisible,
                PublishedDate = requestDTO.PublishedDate,
                ShortDescription = requestDTO.ShortDescription,
                Title = requestDTO.Title,
                UrlHandle = requestDTO.UrlHandle
            };

            blogPost = await blogPostRepostitory.CreateAsync(blogPost);

            // Convert Domain Model back to DTO
            var response = new BlogPostDTO
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle
            };
            return Ok(response);
        }

    }
}
