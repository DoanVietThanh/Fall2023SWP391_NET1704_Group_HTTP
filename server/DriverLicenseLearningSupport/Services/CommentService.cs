using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly ICommentRepository _commentRepository;

        public CommentService(IMapper mapper , ICommentRepository commentRepository) 
        {
            _mapper = mapper;
            _commentRepository = commentRepository;
        }

        public async Task<CommentModel> CreateAsync(CommentModel comment)
        {
            var commentEntity = _mapper.Map<Comment>(comment);
            return await _commentRepository.CreateAsync(commentEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _commentRepository.DeleteAsync(id);
        }

        public async Task<bool> UpdateAsync(CommentModel comment)
        {
            return await _commentRepository.UpdateAsync(comment);
        }
    }
}
