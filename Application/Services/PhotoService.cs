using Domain.Core;
using Domain.EFEntities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly DataContext _dataContext;
        private readonly IPhotoAccessor _photoAccessor;
        private readonly IUserAccessor _userAccessor;

        public PhotoService(DataContext dataContext, IPhotoAccessor photoAccessor,
            IUserAccessor userAccessor)
        {
            _dataContext = dataContext;
            _photoAccessor = photoAccessor;
            _userAccessor = userAccessor;
        }
        public async Task<Result> AddAsync(IFormFile file)
        {
            var user = await _dataContext.Users.Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

            if (user == null)
            {
                return Result.SuccessNotFound();
            }

            var photoUploadResult = await _photoAccessor.AddPhoto(file);

            var photo = new Photo
            {
                Url = photoUploadResult.Url,
                Id = photoUploadResult.PublicId
            };

            if (!user.Photos.Any(x => x.IsMain))
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);
            var result = await _dataContext.SaveChangesAsync() > 0;

            if (result)
            {
                return Result.Success(photo);
            }

            return Result.Error("Problem adding photo");
        }

        public async Task<Result> DeleteAsync(string id)
        {
            var user = await _dataContext.Users.Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

            if (user == null)
            {
                return Result.SuccessNotFound();
            }

            var neededPhoto = user.Photos.FirstOrDefault(photo => photo.Id == id);

            if (neededPhoto == null)
            {
                return Result.SuccessNotFound();
            }

            if (neededPhoto.IsMain)
            {
                return Result.Error("You can't delete your main photo");
            }

            var result = await _photoAccessor.DeletePhoto(neededPhoto.Id);

            if (result == null)
            {
                return Result.Error("Error deleting photo from Cloudinary");
            }

            user.Photos.Remove(neededPhoto);

            var success = await _dataContext.SaveChangesAsync() > 0;

            if (success)
            {
                return Result.SuccessNoContent();
            }

            return Result.Error("Error deleting photo from api");
        }

        public async Task<Result> SetMain(string id)
        {
            var user = await _dataContext.Users.Include(user => user.Photos)
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

            if (user == null)
            {
                return Result.SuccessNotFound();
            }

            var photo = user.Photos.FirstOrDefault(x => x.Id == id);

            if (photo == null)
            {
                return Result.SuccessNotFound();
            }

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if (currentMain != null)
            {
                currentMain.IsMain = false;
            }

            photo.IsMain = true;

            var success = await _dataContext.SaveChangesAsync() > 0;

            if (success)
            {
                return Result.SuccessNoContent();
            }

            return Result.Error("Problem setting main photo");
        }
    }
}
