using AutoMapper;

namespace Application.Common.Mapping
{
    /// <summary>
    /// Interface dùng để ánh xạ (mapping) từ lớp hiện tại tới kiểu đích T.
    /// Cung cấp một phương thức mặc định Mapping sử dụng AutoMapper.
    /// </summary>
    /// <typeparam name="T">Kiểu đích mà class sẽ ánh xạ tới</typeparam>
    public interface IBasicMapTo<T>
    {
        /// <summary>
        /// Phương thức Mapping mặc định (default interface method)
        /// - Dùng AutoMapper để ánh xạ từ kiểu hiện tại (GetType()) tới kiểu T.
        /// - Không cần override nếu không cần cấu hình thêm.
        /// </summary>
        /// <param name="profile">Đối tượng AutoMapper Profile được truyền vào để đăng ký ánh xạ</param>
        void Mapping(Profile profile) => profile.CreateMap(GetType(), typeof(T));
    }
}
