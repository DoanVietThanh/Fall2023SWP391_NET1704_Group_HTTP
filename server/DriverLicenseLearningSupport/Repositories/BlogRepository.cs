using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Spreadsheet;
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
            var tagList = blog.Tags;
            blog.Tags = new List<Tag>();
            await _context.Blogs.AddAsync(blog);
            bool isSuccess = await _context.SaveChangesAsync() > 0 ? true : false;
            if (isSuccess)
            {
                var BlogEntity = await _context.Blogs.OrderByDescending(x => x.BlogId).FirstOrDefaultAsync();
                blog.BlogId = Convert.ToInt32(BlogEntity.BlogId);
                foreach (var tag in tagList)
                {
                    var t = await _context.Tags.Where(x => x.TagId == tag.TagId).FirstOrDefaultAsync();
                    BlogEntity.Tags.Add(t);
                }
                await _context.SaveChangesAsync();
            }
            return _mapper.Map<BlogModel>(blog);
            //await _context.Blogs.AddAsync(blog);
            //bool isSuccess = await _context.SaveChangesAsync() > 0 ? true : false;
            //if(isSuccess) 
            //{
            //    var blogEntity = await _context.Blogs.OrderByDescending(x => x.BlogId).FirstOrDefaultAsync();
            //    blog.BlogId = Convert.ToInt32(blogEntity.BlogId);
            //}
            //return _mapper.Map<BlogModel>(blog);
        }



        public async Task<IEnumerable<BlogModel>> GetAllAsync()
        {
            var Blogs = await _context.Blogs.Include(blog => blog.Comments)
                                            .Include(blog => blog.Tags).ToListAsync();

            return _mapper.Map<IEnumerable<BlogModel>>(Blogs);
        }

        public async Task<IEnumerable<BlogModel>> GetBlogByIdAsync(int id)
        {
            var blogs = await _context.Blogs.Include(blog => blog.Tags)
                                            .Include(blog => blog.Comments).Where(x => x.BlogId == id).ToListAsync();
            return _mapper.Map<IEnumerable<BlogModel>>(blogs);
        }

        public async Task<IEnumerable<BlogModel>> GetBlogsByStaffId(string id)
        {
            var blogs = await _context.Blogs.Include(blog => blog.Tags)
                                            .Include(blog=>blog.Comments).Where(x => x.StaffId.Equals(id)).ToListAsync();
            return _mapper.Map<IEnumerable<BlogModel>>(blogs);
        }
    }
}
