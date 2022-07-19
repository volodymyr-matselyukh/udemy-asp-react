using Domain.Core;
using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces
{
    public interface IPhotoService
    {
        public Task<Result> AddAsync(IFormFile file);
        public Task<Result> DeleteAsync(string id);
        public Task<Result> SetMain(string id);
    }
}
