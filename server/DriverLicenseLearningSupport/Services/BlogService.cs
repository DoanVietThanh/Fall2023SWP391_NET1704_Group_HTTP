using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepo;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public BlogService(IBlogRepository blogRepo, IMapper mapper, IImageService imageService) 
        {
            _blogRepo = blogRepo;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<BlogModel> CreateAsync(BlogModel blog)
        {
            var blogEntity =  _mapper.Map<Blog>(blog);
            return await _blogRepo.CreateAsync(blogEntity);
        }

        public async Task<bool> DeleteBlogAsync(int blogId)
        {
            return await _blogRepo.DeleteBlogAsync(blogId);
        }

        public async Task<IEnumerable<BlogModel>> GetAllAsync() 
        {
            return await _blogRepo.GetAllAsync();
        }

        public async Task<IEnumerable<BlogModel>> GetAllBlogWithoutCmt()
        {
            var blogs =  await _blogRepo.GetAllBlogWithoutCmt(); 
            foreach(var b in blogs)
            {
                if(b.Image is not null)
                {
                    b.Image = await _imageService.GetPreSignedURL(Guid.Parse(b.Image));
                }
            }

            return blogs;
        }

        public async Task<IEnumerable<BlogModel>> GetBlogByIdAsync(int id)
        {
            var blogs = await _blogRepo.GetBlogByIdAsync(id);
            foreach (var b in blogs)
            {
                if (b.Image is not null)
                {
                    b.Image = await _imageService.GetPreSignedURL(Guid.Parse(b.Image));
                }
            }

            return blogs;
        }

        public async Task<IEnumerable<BlogModel>> GetBlogsByStaffId(Guid id)
        {
            return await _blogRepo.GetBlogsByStaffId(id);
        }

        public async Task<IEnumerable<BlogModel>> SearchBlogByAuthorAsync(string author)
        {
            return await _blogRepo.SearchBlogByAuthorAsync(author);
        }

        public async Task<IEnumerable<BlogModel>> SearchBlogByNameAsync(string name)
        {
            return await _blogRepo.SearchBlogByNameAsync(name);
        }

        public async Task<IEnumerable<BlogModel>> SearchBlogByTagId(int tagId)
        {
            return await _blogRepo.SearchBlogByTagId(tagId);
        }

        public async Task<bool> UpdateBlogAsync(BlogModel blog) 
        {
            return await _blogRepo.UpdateBlogAsync(blog);
        }
    }
}
