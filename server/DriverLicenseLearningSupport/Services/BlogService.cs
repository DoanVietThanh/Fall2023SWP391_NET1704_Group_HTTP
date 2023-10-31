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

        public BlogService(IBlogRepository blogRepo, IMapper mapper) 
        {
            _blogRepo = blogRepo;
            _mapper = mapper;
        }

        public async Task<BlogModel> CreateAsync(BlogModel blog)
        {
            var blogEntity =  _mapper.Map<Blog>(blog);
            return await _blogRepo.CreateAsync(blogEntity);
        }

        public async Task<IEnumerable<BlogModel>> GetAllAsync() 
        {
            return await _blogRepo.GetAllAsync();
        }

        public async Task<IEnumerable<BlogModel>> GetBlogByIdAsync(int id)
        {
            return await _blogRepo.GetBlogByIdAsync(id);
        }
    }
}
