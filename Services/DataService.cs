using BarabanPanel.Models;
using BarabanPanel.Services.Interfaces;
using Rep_interfases;
using System;
using System.Linq;
using System.Threading.Tasks;
using UserStatisticDb;
using UserStatisticDb.Entityes;

namespace BarabanPanel.Services
{
    public class DataService : IDataService
    {
        private readonly IRepository<MelStat> melStatRepository;
        private readonly IRepository<TempStat> tempStatRepository;
        public DataService(IRepository<MelStat> melrep, IRepository<TempStat> temprep) 
        {
            melStatRepository = melrep;
            tempStatRepository = temprep;
        }

        public async Task CheckMelodyStaticticsAsync(int MelId) 
        {
            var mel = melStatRepository.Get(MelId);
            mel.IsComplete = true;
            await melStatRepository.UpdateAsync(mel);
        }

        public async Task CheckTempStaticticsAsync(decimal temp, int score) 
        {
            var obj = tempStatRepository.Items.FirstOrDefault(name => name.Temp == temp);
            if(obj == null) 
            {
                TempStat tempStat = new TempStat()
                {
                    Temp = temp,
                    Score = score
                };
                await tempStatRepository.AddAsync(tempStat);

                return;
            }
            if(obj.Score < score)
                obj.Score = score;
                await tempStatRepository.UpdateAsync(obj);
        }
    }
}
