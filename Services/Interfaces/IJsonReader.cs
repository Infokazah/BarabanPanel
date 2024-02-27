using BarabanPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarabanPanel.Services.Interfaces
{
    internal interface IJsonReader
    {
        IEnumerable<string> GetDictionaryNames();

        Melody GetMelody(string key);
    }
}
