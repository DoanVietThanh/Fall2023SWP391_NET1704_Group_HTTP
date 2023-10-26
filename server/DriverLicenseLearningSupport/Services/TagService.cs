using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepo;
        private readonly IMapper _mapper;

        public TagService(ITagRepository tagRepo, IMapper mapper) 
        {
            _tagRepo = tagRepo;
            _mapper = mapper;
        }

        public async Task<TagModel> CreateAsync(TagModel tag)
        {
            var entity = _mapper.Map<Tag>(tag);
            return await _tagRepo.CreateAsync(entity);
        }
    }
}
