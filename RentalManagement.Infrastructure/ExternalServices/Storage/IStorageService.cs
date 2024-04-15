using RentalManagement.Domain.Enums;

namespace RentalManagement.Infrastructure.ExternalServices.Storage
{
    public interface IStorageService
    {
        void ValidateDownloadSize(long contentLenth, TypeExtension typeExtension);
        Task<string> UploadFileAsync(string nameFile, Stream file, string typeExtension);       
    }
}