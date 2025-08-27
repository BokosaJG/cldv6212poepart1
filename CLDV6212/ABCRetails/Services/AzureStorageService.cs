using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text;
using ABCRetails.Models;

namespace ABCRetails.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        private readonly AzureStorageOptions _options;
        private TableServiceClient _tableServiceClient;
        private TableClient _customerTable;
        private TableClient _productTable;
        private TableClient _orderTable;
        private BlobServiceClient _blobServiceClient;
        private BlobContainerClient _blobContainer;
        private QueueClient _queueClient;
        private ShareClient _shareClient;

        public AzureStorageService(IOptions<AzureStorageOptions> options)
        {
            _options = options.Value;
            _tableServiceClient = new TableServiceClient(_options.ConnectionString);
            _blobServiceClient = new BlobServiceClient(_options.ConnectionString);
            _queueClient = new QueueClient(_options.ConnectionString, _options.Queue.Name);
            _shareClient = new ShareClient(_options.ConnectionString, _options.FileShare.Name);
            _customerTable = _tableServiceClient.GetTableClient(_options.TableNames.Customers);
            _productTable = _tableServiceClient.GetTableClient(_options.TableNames.Products);
            _orderTable = _tableServiceClient.GetTableClient(_options.TableNames.Orders);
            _blobContainer = _blobServiceClient.GetBlobContainerClient(_options.Blob.ContainerName);
        }

        public async Task InitializeAsync()
        {
            await _customerTable.CreateIfNotExistsAsync();
            await _productTable.CreateIfNotExistsAsync();
            await _orderTable.CreateIfNotExistsAsync();

            await _blobContainer.CreateIfNotExistsAsync(PublicAccessType.Blob);
            await _queueClient.CreateIfNotExistsAsync();
            await _shareClient.CreateIfNotExistsAsync();
        }

        public async Task<List<T>> GetAllAsync<T>() where T : class, ITableEntity, new()
        {
            var table = GetTableFor<T>();
            var list = new List<T>();
            await foreach (var entity in table.QueryAsync<T>())
            {
                list.Add(entity);
            }
            return list;
        }

        public async Task UpsertAsync<T>(T entity) where T : class, ITableEntity, new()
        {
            var table = GetTableFor<T>();
            await table.UpsertEntityAsync(entity);
        }

        public async Task DeleteAsync<T>(string partitionKey, string rowKey) where T : class, ITableEntity, new()
        {
            var table = GetTableFor<T>();
            await table.DeleteEntityAsync(partitionKey, rowKey);
        }

        public async Task<string?> UploadImageAsync(IFormFile file, string? fileNameHint = null)
        {
            if (file == null || file.Length == 0) return null;
            var name = (fileNameHint ?? Guid.NewGuid().ToString()) + Path.GetExtension(file.FileName);
            var blob = _blobContainer.GetBlobClient(name);
            using var stream = file.OpenReadStream();
            await blob.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
            return blob.Uri.ToString();
        }

        public async Task EnqueueMessageAsync(string messageText)
        {
            await _queueClient.SendMessageAsync(Convert.ToBase64String(Encoding.UTF8.GetBytes(messageText)));
        }

        public async Task UploadContractAsync(string fileName, Stream fileContent)
        {
            var directory = _shareClient.GetRootDirectoryClient();
            await directory.CreateIfNotExistsAsync();
            var fileClient = directory.GetFileClient(fileName);
            fileContent.Position = 0;
            await fileClient.CreateAsync(fileContent.Length);
            await fileClient.UploadAsync(fileContent);
        }

        private TableClient GetTableFor<T>() where T : class, ITableEntity, new()
        {
            if (typeof(T) == typeof(Customer)) return _customerTable;
            if (typeof(T) == typeof(Product)) return _productTable;
            if (typeof(T) == typeof(Order)) return _orderTable;
            throw new InvalidOperationException($"Table mapping missing for type {typeof(T).Name}");
        }
    }
}