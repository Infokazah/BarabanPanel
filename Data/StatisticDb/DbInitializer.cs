using BarabanPanel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStatisticDb.Context;
using UserStatisticDb.Entityes;

namespace BarabanPanel.Data.StatisticDb
{
    class DbInitializer
    {
        private readonly UserStatisticContext _db;
        private readonly ILogger _logger;
        public DbInitializer(UserStatisticContext db, ILogger<DbInitializer> Logger)
        {
            _db = db;
            _logger = Logger;
        }

        public async Task InitializeAsync()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Инициализация БД...");
            _db.Database.Migrate();

            if (await _db.MelStatDb.AnyAsync().ConfigureAwait(false)) return;

            await InitializeCategories().ConfigureAwait(false);
            _logger.LogInformation("Инициализация БД выполненно за {0}", timer.Elapsed.TotalSeconds);
        }
        private const int _MelodyCount = 5;

        private MelStat[] _Melody;
        private async Task InitializeCategories()
        {
            _logger.LogInformation("Инициализация мелодий");
            _Melody = new MelStat[_MelodyCount];
            for (var i = 0; i < _MelodyCount; i++)
            {
                _Melody[i] = new MelStat { Name = $"Мелодия {i + 1}" };

            }
            await _db.AddRangeAsync(_Melody);
            await _db.SaveChangesAsync();
        }
    }
}
