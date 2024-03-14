using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Web.Models.Domain;
using Proyecto_Web.Models.DTO.BlogPosts;
using Proyecto_Web.Models.DTO.Category;
using Proyecto_Web.Repositories.Interface;
using System.Diagnostics.CodeAnalysis;

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
        [Authorize(Roles = "Writer")]
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
                if (existingCategory is not null)
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
            foreach (var blogPost in blogPosts)
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



        // GET: {apiBaseUrl}/api/blogposts/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostByid([FromRoute] Guid id)
        {
            // Get the BlogPost from Repo
            var blogPost = await blogPostRepostitory.GetByIdAsync(id);
            if (blogPost is null)
            {

                return NotFound();
            }
            // Convert Domain Model to DTO
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

        // GET: {apibaseurl}/api/blogPost/{urlhandle}
        [HttpGet]
        [Route("{urlHandle}")]
        public async Task<IActionResult> GetBlogPostByUrlHandle([FromRoute] string urlHandle)
        {
            // Get Blogpost details from repository
           var blogPost = await blogPostRepostitory.GetByUrlHandleAsync(urlHandle);
            if (blogPost is null)
            {

                return NotFound();
            }
            // Convert Domain Model to DTO
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


        // PUT: {apibaseurl}/api/blogposts/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateBloPostById([FromRoute] Guid id, UpdateBlogPostRequestDto requestDTO)
        {
            // Convert DTO to Domain Model
            var blogPost = new BlogPost
            {
                Id = id,
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

            // Foreach 
            foreach (var categoryGuid in requestDTO.Categories)
            {
             var existingCategory =   await categoryRepository.GetById(categoryGuid);
                if(existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            // Call Repository To Update BlogPost Domain Model
            var updateBlogPost =  await blogPostRepostitory.UpdateAsync(blogPost);

            if (updateBlogPost == null) 
            {
                return NotFound();
            }

            // Convert Domain model back to DTO
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

        // Delete: {apibaseurl}/api/blogposts/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
            var deletedBlogPost = await blogPostRepostitory.DeleteAsync(id);

            if (deletedBlogPost == null)
            {
                return NotFound();
            }

            // Convert Domain Model Back to DTO
            var response = new BlogPostDTO
            {
                Id = deletedBlogPost.Id,
                Author = deletedBlogPost.Author,
                Content = deletedBlogPost.Content,
                FeaturedImageUrl = deletedBlogPost.FeaturedImageUrl,
                IsVisible = deletedBlogPost.IsVisible,
                PublishedDate = deletedBlogPost.PublishedDate,
                ShortDescription = deletedBlogPost.ShortDescription,
                Title = deletedBlogPost.Title,
                UrlHandle = deletedBlogPost.UrlHandle
            };
            
            return Ok(response);
        }

    }



}
