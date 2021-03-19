﻿using Microsoft.EntityFrameworkCore;
using Mimo.EntityFrameworkCore;
using Mimo.Interfaces;
using Mimo.Models;
using Mimo.Models.Dtos.UserAchievements;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Mimo.Services
{
    public class UserAchievementsService : IUserAchievementsService
    {
        private readonly MimoDbContext _mimoDbContext = new MimoDbContext();

        public async Task<List<UserAchievementDto>> GetAllUserAchievements(int userId)
        {
            var userAchievements = await _mimoDbContext.UserAchievements
                .Include(ua => ua.AchievementFk)
                .Include(ua => ua.AchievementFk.AchievementTypeFk)
                .Where(ua => ua.UserId == userId)
                .ToListAsync();

            List<UserAchievementDto> userAchievementDtos = new List<UserAchievementDto>();

            foreach (var userAchievement in userAchievements)
            {
                UserAchievementDto userAchievementDto = new UserAchievementDto
                {
                    AchievementId = userAchievement.AchievementId,
                    AchievementTypeName = userAchievement.AchievementFk.AchievementTypeFk.AchievementTypeName,
                    Completed = userAchievement.Completed,
                    Id = userAchievement.Id,
                    Progress = userAchievement.Progress,
                    UserId = userAchievement.UserId
                };

                userAchievementDtos.Add(userAchievementDto);
            }

            return userAchievementDtos;
        }

        public async Task<HttpStatusCode> PostUserAchievement(PostUserAchievementDto input)
        {
            UserAchievement userAchievement = new UserAchievement
            {
                UserId = input.UserId,
                AchievementId = input.AchievementId,
                Completed = input.Completed,
                Progress = input.Progress
            };

            await _mimoDbContext.UserAchievements.AddAsync(userAchievement);
            await _mimoDbContext.SaveChangesAsync();

            return HttpStatusCode.OK;
        }
    }
}
