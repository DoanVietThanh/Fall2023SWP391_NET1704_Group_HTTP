using AutoMapper;
using DocumentFormat.OpenXml.InkML;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;
        public TagRepository(DriverLicenseLearningSupportContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TagModel> CreateAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            bool isSuccess = await _context.SaveChangesAsync()>0? true : false;
            if(isSuccess) 
            {
                var tagEntity = await _context.Tags.OrderByDescending(x => x.TagId).FirstOrDefaultAsync();
                tagEntity.TagId = Convert.ToInt32(tagEntity.TagId);
            }
            return _mapper.Map<TagModel>(tag);
        }

        
    }
}
