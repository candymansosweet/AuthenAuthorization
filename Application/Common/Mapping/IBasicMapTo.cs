using AutoMapper;

namespace Application.Common.Mapping
{
    public interface IBasicMapTo<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(GetType(), typeof(T));
    }
}