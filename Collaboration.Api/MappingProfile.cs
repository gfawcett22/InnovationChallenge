using AutoMapper;
using Collaboration.Api.Models;
using Collaboration.Core.Models;

namespace Collaboration.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Thread, ThreadDto>();
            CreateMap<ThreadToUpdateDto, Thread>();
            CreateMap<ThreadToCreateDto, Thread>();
            CreateMap<Post, PostDto>()
                .ForMember(pd => pd.ThreadId, opt => opt.MapFrom(p => p.Thread.ThreadId));
            CreateMap<PostToCreateDto, Post>();
            CreateMap<PostToUpdateDto, Post>();
        }
    }
}