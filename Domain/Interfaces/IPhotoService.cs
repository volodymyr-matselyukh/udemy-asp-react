using Domain.Core;
using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces
{
    public interface IPhotoService
    {
        public Task<Result<object>> AddAsync(IFormFile file);
        public Task<Result<object>> DeleteAsync(string id);
        public Task<Result<object>> SetMain(string id);
    }
}
