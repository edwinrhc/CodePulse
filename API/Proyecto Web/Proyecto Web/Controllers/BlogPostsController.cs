using Microsoft.AspNetCore.Mvc;
using Proyecto_Web.Models.Domain;
using Proyecto_Web.Models.DTO.BlogPosts;
using Proyecto_Web.Models.DTO.Category;
using Proyecto_Web.Repositories.Interface;

namespace Proyecto_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepostitory;
        private readonly ICategoryRepository categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
        {
            this.blogPostRepostitory = blogPostRepository;
            this.categoryRepository = categoryRepository;
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
                UrlHandle = requestDTO.UrlHandle,
                Categories = new List<Category>()
            };

            foreach (var categoriesGUId in requestDTO.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoriesGUId);
                if(existingCategory is not null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

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
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };
            return Ok(response);
        }

        //GET: {apibaseurl}/api/blogposts
        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts() 
        {
           var blogPosts = await blogPostRepostitory.GetAllAsync();

            // Convert Domain model to DTO
            var response = new List<BlogPostDTO>();
            foreach(var blogPost in blogPosts)
            {
                response.Add(new BlogPostDTO
                {
                    Id = blogPost.Id,
                    Author = blogPost.Author,
                    Content = blogPost.Content,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    IsVisible = blogPost.IsVisible,
                    ShortDescription = blogPost.ShortDescription,
                    Title = blogPost.Title,
                    UrlHandle = blogPost.UrlHandle,
                    Categories = blogPost.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()
                });
            }

            return Ok(response);
        }


    }



}
