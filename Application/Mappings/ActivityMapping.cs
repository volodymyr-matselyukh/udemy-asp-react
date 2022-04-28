using AutoMapper;
using Domain;

namespace Application.Mappings
{
    public class ActivityMapping : Profile
    {
        public ActivityMapping()
        {
            CreateMap<Activity, Activity>();
        }
    }
}
