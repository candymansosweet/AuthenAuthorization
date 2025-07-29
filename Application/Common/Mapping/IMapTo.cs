using AutoMapper;

namespace Application.Common.Mapping
{
    /// <summary>
    /// Interface dùng để ánh xạ từ class hiện tại sang kiểu T.
    /// Cung cấp phương thức Mapping mặc định với cấu hình điều kiện để bỏ qua các giá trị null từ source.
    /// dùng để map data từ entity sang DTO của response
    /// </summary>
    /// <typeparam name="T">Kiểu đích (destination) mà class hiện tại sẽ ánh xạ tới</typeparam>
    public interface IMapTo<T>
    {
        /// <summary>
        /// Phương thức Mapping mặc định (default interface method)
        /// - Tạo ánh xạ từ kiểu hiện tại (source) sang kiểu T (destination).
        /// - Bỏ qua ánh xạ nếu giá trị thuộc tính từ source là null.
        /// </summary>
        /// <param name="profile">Đối tượng AutoMapper Profile dùng để đăng ký ánh xạ</param>
        void Mapping(Profile profile)
        {
            profile.CreateMap(GetType(), typeof(T))
                .ForAllMembers(opts =>
                {
                    // Cấu hình cho mọi property:
                    // - Chỉ ánh xạ nếu srcMember (giá trị từ source) khác null
                    opts.Condition((src, dest, srcMember) =>
                    {
                        return srcMember != null;
                    });
                });
        }
    }
}
