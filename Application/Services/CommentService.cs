﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Core;
using Domain.DTOs;
using Domain.EFEntities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public CommentService(DataContext dataContext, IMapper mapper, IUserAccessor userAccessor)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }
        public async Task<Result<CommentDto>> Create(string body, Guid activityId)
        {
            var activity = await _dataContext.Activities.FindAsync(activityId);

            if (activity == null)
            {
                return Result<CommentDto>.SuccessNotFound();
            }

            var user = await _dataContext.Users.Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

            var comment = new Comment
            {
                Author = user,
                Activity = activity,
                Body = body
            };

            activity.Comments.Add(comment);

            var success = await _dataContext.SaveChangesAsync() > 0;

            if (success)
            {
                return Result<CommentDto>.Success(_mapper.Map<CommentDto>(comment));
            }

            return Result<CommentDto>.Error("Failed to add comment");
        }

        public async Task<Result<object>> List(Guid activityId)
        {
            var comments = await _dataContext.Comments.Where(c => c.Activity.Id == activityId)
                .OrderByDescending(c => c.CreatedAt)
                .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Result.Success(comments);
        }
    }
}
