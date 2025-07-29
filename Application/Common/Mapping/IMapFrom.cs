using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapping
{
    /// <summary>
    /// Interface dùng để ánh xạ từ kiểu T sang class hiện tại.
    /// Cung cấp phương thức Mapping mặc định với cấu hình điều kiện để bỏ qua các giá trị null từ source.
    /// dùng trong update và create để map data từ payload sang entity
    /// </summary>
    /// <typeparam name="T">Kiểu nguồn (source) sẽ ánh xạ từ</typeparam>
    public interface IMapFrom<T>
    {
        /// <summary>
        /// Phương thức Mapping mặc định (default interface method)
        /// - Tạo ánh xạ từ kiểu T (source) sang kiểu hiện tại (destination).
        /// - Bỏ qua ánh xạ nếu giá trị thuộc tính từ source là null.
        /// </summary>
        /// <param name="profile">Đối tượng AutoMapper Profile dùng để đăng ký ánh xạ</param>
        void Mapping(Profile profile)
        {
            profile.CreateMap(typeof(T), GetType())
                .ForAllMembers(opts =>
                {
                    // Cấu hình điều kiện ánh xạ cho mọi property:
                    // - Chỉ ánh xạ nếu srcMember (giá trị từ source) khác null
                    opts.Condition((src, dest, srcMember) =>
                    {
                        return srcMember != null;
                    });
                });
        }
    }
}
