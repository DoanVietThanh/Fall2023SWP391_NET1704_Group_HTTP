using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IMapper _mapper;
        private readonly DriverLicenseLearningSupportContext _context;

        public CommentRepository(IMapper mapper, DriverLicenseLearningSupportContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<CommentModel> CreateAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            bool isSuccess = await _context.SaveChangesAsync() > 0 ? true : false;
            if (isSuccess)
            {
                var commentEntity = await _context.Comments.OrderByDescending(x => x.CommentId).FirstOrDefaultAsync();
                comment.CommentId = Convert.ToInt32(commentEntity.CommentId);
            }
            return _mapper.Map<CommentModel>(comment);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.CommentId == id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(CommentModel comment)
        {
            var commentEntity = await _context.Comments.FirstOrDefaultAsync(x => x.CommentId == comment.CommentId);
            if (commentEntity != null)
            {
                commentEntity.Content = comment.Content;
                return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            return false;

        }
    }
}
