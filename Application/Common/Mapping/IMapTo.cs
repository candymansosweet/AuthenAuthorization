using AutoMapper;

namespace Application.Common.Mapping
{
    public interface IMapTo<T>
    {
        void Mapping(Profile profile)
        {
            profile.CreateMap(GetType(), typeof(T)).ForAllMembers(opts =>
            {
                opts.Condition((src, dest, srcMember) =>
                {
                    return srcMember != null;
                });
            });
        }
    }
}