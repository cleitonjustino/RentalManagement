using RentalManagement.Domain.Enums;

namespace RentalManagement.Infrastructure.Extensions
{
    public static class SystemExtensions
    {
        public static long SizeFromMb(this long mb) => 1024 * 1024 * mb;

        public static TypeExtension GetExtessionTypeMedia(this string mediaType)
        {
            var data = mediaType.Trim();

            switch (data)
            {
                case "image/jpg":
                    return TypeExtension.Png;
                case "image/bmp":
                    return TypeExtension.Bmp;
                default:
                    return default;
            }
        }
    }
}
