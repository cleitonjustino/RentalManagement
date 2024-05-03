using System.Net.Mime;
using MassTransit.Caching.Internals;
using System.Security.AccessControl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using RentalManagement.Domain.Common;
using RentalManagement.Domain.Enums;
using RentalManagement.Infrastructure.Extensions;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RentalManagement.Infrastructure.ExternalServices.Storage
{
    public class StorageService : IStorageService
    {
        private readonly IMinioClient _minioClient;
        private readonly ILogger<StorageService> _logger;
        private readonly IConfiguration _configuration;

        private const long TamanhoMaximoDownloadPdfMb = 10;
        private const long TamanhoMaximoDownloadJpgMb = 3;
        private const long TotalDiasExpiracaoUrl = 365;
        private const string BucketCnh = "cnhstore";


        public StorageService(IMinioClient minioClient, ILoggerFactory loggerFactory,
            IConfiguration configuration)
        {
            _minioClient = minioClient;
            _logger = loggerFactory.CreateLogger<StorageService>();
            _configuration = configuration;
        }

        public void ValidateDownloadSize(long contentLenth, TypeExtension anexoExtensaoContentType)
        {
            var tamanhoMaximoDownload = anexoExtensaoContentType == TypeExtension.Png
                ? TamanhoMaximoDownloadJpgMb.SizeFromMb()
                : TamanhoMaximoDownloadPdfMb.SizeFromMb();

            if (contentLenth <= tamanhoMaximoDownload)
                return;

            var mensagem = string.Format("Size Exceeed", contentLenth,
                tamanhoMaximoDownload);

            _logger.LogInformation(mensagem);

            throw new ValidationException(new ValidationItem { Message = mensagem });
        }

        public async Task<string> UploadFileAsync(string nomeArquivo, Stream file, string contentType)
        {
            UploadIsValid(nomeArquivo, file);

            var beArgs = new BucketExistsArgs()
                    .WithBucket(BucketCnh);

            bool found = await _minioClient.BucketExistsAsync(beArgs).ConfigureAwait(false);
            if (!found)
            {
                var mbArgs = new MakeBucketArgs()
                        .WithBucket(BucketCnh);
                await _minioClient.MakeBucketAsync(mbArgs).ConfigureAwait(false);
            }

            var putObjectArgs = new PutObjectArgs()
               .WithBucket(BucketCnh)
               .WithObject(nomeArquivo)
               .WithStreamData(ConvertMemoryStream(file))
               .WithObjectSize(file.Length)
               .WithContentType(contentType);

            var result = await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
            _logger.LogInformation($"criado registro{result.ObjectName} no bucket com sucesso");

            var bucket = new PresignedGetObjectArgs()
                .WithBucket(BucketCnh)
                .WithObject(string.Concat(nomeArquivo))
                .WithRequestDate(DateTime.Now)
                .WithExpiry(365);

            string fileUrl = await _minioClient.PresignedGetObjectAsync(bucket);

            return fileUrl;
        }

        public MemoryStream ConvertMemoryStream(Stream inputStream)
        {
            byte[] inputBuffer = new byte[inputStream.Length];
            inputStream.Read(inputBuffer, 0, inputBuffer.Length);

            return new MemoryStream(inputBuffer);
        }

        private void UploadIsValid(string nomeArquivo, Stream file)
        {
            if (string.IsNullOrWhiteSpace(nomeArquivo))
                throw new ArgumentNullException(nameof(nomeArquivo));

            if (string.IsNullOrWhiteSpace(BucketCnh))
                throw new ArgumentNullException(BucketCnh);

            if (file.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(file), "Empty");
        }


        private string ConverterBytesToBase64(byte[] bytes)
        {
            if (bytes.Length <= 0)
                return default;
            return Convert.ToBase64String(bytes, 0, bytes.Length);
        }


        private static string ContentType(TypeExtension anexoExtensao)
            => anexoExtensao == TypeExtension.Png
                ? MediaTypeNames.Image.Png
                : MediaTypeNames.Image.Bmp;
    }
}