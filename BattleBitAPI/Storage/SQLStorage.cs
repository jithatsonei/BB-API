using System.IO;
using System.Threading.Tasks;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Newtonsoft.Json;
using BattleBitAPI.Common;

namespace BattleBitAPI.Storage
{
    public class SqlStorage : IPlayerStatsDatabase
    {
        private readonly GameDbContext _dbContext;

        public SqlStorage(GameDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PlayerStats> GetPlayerStatsOf(ulong steamID)
        {
            var playerStatsEntity = await _dbContext.PlayerStats.FindAsync(steamID);
            if (playerStatsEntity != null)
            {
                return JsonConvert.DeserializeObject<PlayerStats>(Encoding.UTF8.GetString(playerStatsEntity.StatsData));
            }
            return null;
        }

        public async Task SavePlayerStatsOf(ulong steamID, PlayerStats stats)
        {
            var existingEntity = await _dbContext.PlayerStats.FindAsync(steamID);

            if (existingEntity != null)
            {
                existingEntity.StatsData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(stats));
            }
            else
            {
                var newEntity = new PlayerStatsEntity { SteamID = steamID, StatsData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(stats)) };
                _dbContext.PlayerStats.Add(newEntity);
            }

            await _dbContext.SaveChangesAsync();
        }
    }

    public class GameDbContext : DbContext
    {
        public DbSet<PlayerStatsEntity> PlayerStats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("your_connection_string_here");
        }
    }

    public class PlayerStatsEntity
    {
        public ulong SteamID { get; set; }
        public byte[] StatsData { get; set; }
    }
}
