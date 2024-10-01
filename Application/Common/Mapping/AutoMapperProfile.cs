using AutoMapper;
using System.Reflection;
///
// Reflection một kỹ thuật giúp truy cập vào các thành phần của 1 class tại thời điểm chạy
// - thích hợp sử dụng khi không biết trước kiểu dữ liệu tại thời điểm biên dịch
// các thành phần
// - Type: đại diện cho kiểu 1 đối tượng, không thể tạo một đối tượng trực tiếp mà phải thông qua typeof() hoặc GetType() // Type type = typeof(MyClass);
// - MethodInfo: Cung cấp thông tin về các phương thức của lớp hoặc giao diện ( tham, số, kiểu trả về,... ) // MethodInfo methodInfo = type.GetMethod("MethodName");
// - PropertyInfo: Cung cấp thông tin về các thuộc tính của lớp hoặc giao diện ( get , set ) // PropertyInfo propertyInfo = type.GetProperty("PropertyName");
// - FieldInfo: Cung cấp thông tin về các trường (fields) của lớp ( không có get , set ) // FieldInfo fieldInfo = type.GetField("FieldName");
// - ConstructorInfo: cung cấp thông tin Constructor // ConstructorInfo constructorInfo = type.GetConstructor(new Type[] { typeof(int) });
// - Assembly: cung cấp thông tin của assembly hiện tại ( thường là các file dll, các file này thường đại diện cho một dự án )
///
namespace Application.Common.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly()); // tập hợp các tài nguyên của mã đang chạy
        }
        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes() // lấy ra các kiểu dữ liệu ( class ) trong assembly
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && // kiểm tra xem class có triển khai bất cứ interface nào có kiểu generic
                    (i.GetGenericTypeDefinition() == typeof(IMapFrom<>) ||
                     i.GetGenericTypeDefinition() == typeof(IMapTo<>) ||
                     i.GetGenericTypeDefinition() == typeof(IBasicMapTo<>))))
                .ToList();
            // => lấy ra types là tập các lớp có triển khai interface IMapFrom, IMapTo, IBasicMapTo
            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                //MethodInfo là một lớp 
                MethodInfo? methodInfo = type.GetMethod("Mapping") ??
                                 type.GetInterface("IMapFrom`1")?.GetMethod("Mapping") ??
                                 type.GetInterface("IMapTo`1")?.GetMethod("Mapping") ??
                                 type.GetInterface("IBasicMapTo`1")?.GetMethod("Mapping");
                // => lấy ra được phương thức Mapping tương ứng

                //@instance: xác định phương thức của class nào được kích hoạt 
                //@ new object[] { this }: truyền vào Profile hiện tại
                methodInfo?.Invoke(instance, new object[] { this }); // Invoke( gọi ): sẽ thực thi phương thức Mapping được tìm thấy để CreateMap
            }
        }
    }
}