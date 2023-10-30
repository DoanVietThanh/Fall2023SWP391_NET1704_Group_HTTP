using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public BlogRepository(DriverLicenseLearningSupportContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BlogModel> CreateAsync(Blog blog)
        {
            await _context.Blogs.AddAsync(blog);
            bool isSuccess = await _context.SaveChangesAsync() > 0 ? true : false;
            if(isSuccess) 
            {
                var blogEntity = await _context.Blogs.OrderByDescending(x => x.BlogId).FirstOrDefaultAsync();
                blog.BlogId = Convert.ToInt32(blogEntity.BlogId);
            }
            return _mapper.Map<BlogModel>(blog);
        }

        public async Task<IEnumerable<BlogModel>> GetAllAsync() 
        {
            var Blogs = await _context.Blogs.Include(blog => blog.Comments)
                                            .Include(blog=>blog.Tags).ToListAsync();

            return _mapper.Map<IEnumerable<BlogModel>>(Blogs);
        }
    }
}
