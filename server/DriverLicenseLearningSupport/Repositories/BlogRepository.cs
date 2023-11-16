using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Spreadsheet;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;
using System.Web;

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
            var staff = await _context.Staffs.Where(staff => staff.StaffId.Equals(blog.StaffId)).FirstOrDefaultAsync();
            blog.Staff = staff;
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

            blog.Content = HttpUtility.HtmlDecode(blog.Content);
            blog.Title = HttpUtility.HtmlDecode(blog.Title);
            return _mapper.Map<BlogModel>(blog);
        }

        public async Task<bool> DeleteBlogAsync(int blogId)
        {
            var blog = await _context.Blogs.Include(x => x.Comments)
                                            .Include(x => x.Tags).FirstOrDefaultAsync(x => x.BlogId == blogId);
            if (blog != null)
            {
                blog.Tags.Clear();
                blog.Comments.Clear();
                await _context.SaveChangesAsync();
            }

            _context.Blogs.Remove(blog);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<IEnumerable<BlogModel>> GetAllAsync()
        {
            var Blogs = await _context.Blogs.Include(blog => blog.Comments)
                                            .Include(blog => blog.Tags).Include(blog => blog.Staff).ToListAsync();
            foreach (var blog in Blogs)
            {
                blog.Title = HttpUtility.HtmlDecode(blog.Title);
                blog.Content = HttpUtility.HtmlDecode(blog.Content);
            }
            return _mapper.Map<IEnumerable<BlogModel>>(Blogs);
        }

        public async Task<IEnumerable<BlogModel>> GetAllBlogWithoutCmt()
        {
            var Blogs = await _context.Blogs.Include(blog => blog.Tags).Include(blog => blog.Staff).OrderByDescending(x => x.CreateDate).ToListAsync();
            foreach (var blog in Blogs)
            {
                blog.Title = HttpUtility.HtmlDecode(blog.Title);
                blog.Content = HttpUtility.HtmlDecode(blog.Content);
            }

            return _mapper.Map<IEnumerable<BlogModel>>(Blogs);
        }

        public async Task<IEnumerable<BlogModel>> GetBlogByIdAsync(int id)
        {
            var blogs = await _context.Blogs.Include(blog => blog.Tags)
                                            .Include(blog => blog.Comments).Where(x => x.BlogId == id).ToListAsync();
            foreach (var blog in blogs)
            {
                blog.Title = HttpUtility.HtmlDecode(blog.Title);
                blog.Content = HttpUtility.HtmlDecode(blog.Content);
            }
            return _mapper.Map<IEnumerable<BlogModel>>(blogs);
        }

        public async Task<IEnumerable<BlogModel>> GetBlogsByStaffId(Guid id)
        {
            var blogs = await _context.Blogs.Include(blog => blog.Tags)
                                            .Include(blog => blog.Comments).Where(x => x.StaffId.Equals(id.ToString())).ToListAsync();
            foreach (var blog in blogs)
            {
                blog.Title = HttpUtility.HtmlDecode(blog.Title);
                blog.Content = HttpUtility.HtmlDecode(blog.Content);
            }
            return _mapper.Map<IEnumerable<BlogModel>>(blogs);
        }

        public async Task<IEnumerable<BlogModel>> SearchBlogByAuthorAsync(string author)
        {
            var blogs = await _context.Blogs.Where(x => (x.Staff.FirstName + " " + x.Staff.LastName).Contains(author)).ToListAsync();
            foreach (var blog in blogs)
            {
                blog.Title = HttpUtility.HtmlDecode(blog.Title);
                blog.Content = HttpUtility.HtmlDecode(blog.Content);
            }
            return _mapper.Map<IEnumerable<BlogModel>>(blogs);
        }

        public async Task<IEnumerable<BlogModel>> SearchBlogByNameAsync(string name)
        {
            var blogs = await _context.Blogs.Where(x => x.Title.Contains(name)).ToListAsync();
            foreach (var blog in blogs)
            {
                blog.Title = HttpUtility.HtmlDecode(blog.Title);
                blog.Content = HttpUtility.HtmlDecode(blog.Content);
            }
            return _mapper.Map<IEnumerable<BlogModel>>(blogs);
        }

        public async Task<IEnumerable<BlogModel>> SearchBlogByTagId(int tagId)
        {
            var blogs = await _context.Blogs.Include(x => x.Tags.Where(y => y.TagId == tagId)).ToListAsync();
            if (blogs is null)
            {
                return null;
            }
            else
            {
                foreach (var blog in blogs)
                {
                    blog.Title = HttpUtility.HtmlDecode(blog.Title);
                    blog.Content = HttpUtility.HtmlDecode(blog.Content);
                }
                return _mapper.Map<IEnumerable<BlogModel>>(blogs);
            }
        }

        public async Task<bool> UpdateBlogAsync(BlogModel blog)
        {
            var tagList = blog.Tags;
            blog.Tags = new List<TagModel>();

            var blogToUpdate = await _context.Blogs.Include(x => x.Tags).FirstOrDefaultAsync(x => x.BlogId == blog.BlogId);
            blogToUpdate.Content = blog.Content;
            blogToUpdate.LastModifiedDate = blog.LastModifiedDate;
            blogToUpdate.Title = blog.Title;
            foreach (var tag in tagList)
            {
                if (!blogToUpdate.Tags.Contains(_mapper.Map<Tag>(tag)))
                {
                    var t = await _context.Tags.Where(x => x.TagId == tag.TagId).FirstOrDefaultAsync();
                    blogToUpdate.Tags.Add(t);
                }
            }
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
