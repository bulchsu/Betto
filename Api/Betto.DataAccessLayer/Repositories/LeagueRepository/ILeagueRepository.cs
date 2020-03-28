﻿using Betto.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Betto.DataAccessLayer.Repositories
{
    public interface ILeagueRepository : IBaseRepository
    {
        Task AddLeaguesAsync(IEnumerable<LeagueEntity> leagues);
        Task<LeagueEntity> GetLeagueByIdAsync(int leagueId);
        Task<IEnumerable<LeagueEntity>> GetLeaguesAsync();
        void Clear();
    }
}