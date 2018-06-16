using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore;
using CSCore.Codecs;
using CSCore.MediaFoundation;

namespace MikeyFriedChicken.AudioCapture
{
    public class ToMp3
    {
        public void ConvertFromWave(string waveFileName, string mp3FileName)
        {
            var mediaTypes = MediaFoundationEncoder.GetEncoderMediaTypes(AudioSubTypes.MpegLayer3);
            if (!mediaTypes.Any())
            {
                ColorConsole.WriteLine("The current platform does not support mp3 encoding.", ConsoleColor.DarkRed);
                return;
            }

            IWaveSource source;
            try
            {
                source = CodecFactory.Instance.GetCodec(waveFileName);

                if (mediaTypes.All(x => x.SampleRate != source.WaveFormat.SampleRate && x.Channels == source.WaveFormat.Channels))
                {
                    int sampleRate =mediaTypes.OrderBy(x => Math.Abs(source.WaveFormat.SampleRate - x.SampleRate)).First(x => x.Channels == source.WaveFormat.Channels).SampleRate;
                    ColorConsole.WriteLine("Samplerate {0} -> {1}",ConsoleColor.DarkRed, source.WaveFormat.SampleRate, sampleRate);
                    ColorConsole.WriteLine("Channels {0} -> {1}", ConsoleColor.DarkRed, source.WaveFormat.Channels, 2);
                    source = source.ChangeSampleRate(sampleRate);
                }
            }
            catch (Exception)
            {
                ColorConsole.WriteLine("Mp3 Format Not Supported", ConsoleColor.DarkRed);
                return;
            }

            using (source)
            {
                using (var encoder = MediaFoundationEncoder.CreateMP3Encoder(source.WaveFormat, mp3FileName))
                {
                    byte[] buffer = new byte[source.WaveFormat.BytesPerSecond];
                    int read;
                    while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        encoder.Write(buffer, 0, read);
                        ColorConsole.WriteLine("Wav -> Mp3 Progress: {0:P}/{1:P}", ConsoleColor.DarkGreen, (double) source.Position / source.Length, 1);
                    }
                }
            }
        }
    }
}
