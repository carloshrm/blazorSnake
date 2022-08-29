﻿using gamesPlatform.Shared;

namespace gamesPlatform.Client.Services
{
    public interface IScoreService
    {
        Task<Score> getScore(int scoreID);
        Task<IEnumerable<Score>> getLeaderboard(int appID);
        Task setScore(Score s);
    }
}