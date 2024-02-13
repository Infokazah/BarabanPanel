using NAudio.Wave;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace BarabanPanel.Models
{
    public class SoundManager
    {
        private List<WaveOutEvent> waveOutEvents;

        public SoundManager()
        {
            waveOutEvents = new List<WaveOutEvent>();
        }

        public async Task PlaySoundAsync(string soundFilePath)
        {
            var waveOutEvent = new WaveOutEvent();
            var audioFileReader = new AudioFileReader(soundFilePath);

            waveOutEvent.Init(audioFileReader);
            waveOutEvent.Play();

            waveOutEvents.Add(waveOutEvent);

            await Task.Run(() =>
            {
                while (waveOutEvent.PlaybackState == PlaybackState.Playing)
                {
                    Task.Delay(1);
                }
            });

            waveOutEvent.Dispose();
            waveOutEvents.Remove(waveOutEvent);
        }

        public void StopAllSounds()
        {
            foreach (var waveOutEvent in waveOutEvents)
            {
                waveOutEvent.Stop();
                waveOutEvent.Dispose();
            }
            waveOutEvents.Clear();
        }
    }


}
