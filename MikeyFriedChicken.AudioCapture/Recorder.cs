using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using CSCore;
using CSCore.Codecs.WAV;
using CSCore.CoreAudioAPI;
using CSCore.SoundIn;

namespace MikeyFriedChicken.AudioCapture
{
    public class Recorder
    {
        public Recorder()
        {

        }

        public void Record(string waveFileName, string mp3FileName, bool includeMp3, bool prompt)
        {

            int latency = 5;
            int sampleRate = 320000;//44100;
            int bits = 32;
            int channels = 2;
            var encoding = AudioEncoding.MpegLayer3;

            WaveFormat waveFormat = new WaveFormat(sampleRate,bits,channels);

            using (WasapiCapture capture = new WasapiLoopbackCapture(latency, waveFormat,ThreadPriority.Highest))
            {
                if (prompt)
                {
                    Dictionary<int, MMDevice> devices = new Dictionary<int, MMDevice>();

                    int i = 1;
                    foreach (MMDevice device in MMDeviceEnumerator.EnumerateDevices(DataFlow.Render)) 
                    {
                        devices.Add(i, device);
                        i++;
                    }

                    foreach (var x in devices)
                    {
                        if (x.Value.FriendlyName == capture.Device.FriendlyName)
                        {
                            ColorConsole.WriteLine(x.Key + ". " + x.Value.FriendlyName + " [default]", ConsoleColor.Cyan);
                        }
                        else
                        {
                            ColorConsole.WriteLine(x.Key + ". " + x.Value.FriendlyName, ConsoleColor.DarkMagenta);
                        }
                        
                    }

                    bool optionSelected = false;
                    while (!optionSelected)
                    {
                        ColorConsole.Write("Select which device above to record from 1,2,3... (Enter for default - ", ConsoleColor.White, capture.Device.FriendlyName);
                        ColorConsole.Write("{0}", ConsoleColor.Cyan, capture.Device.FriendlyName);
                        ColorConsole.Write(") $", ConsoleColor.White, capture.Device.FriendlyName);

                        ConsoleKeyInfo key = Console.ReadKey();
                        Console.WriteLine();

                        string keystring = key.KeyChar.ToString();

                        if (key.Key == ConsoleKey.Enter)
                        {
                            optionSelected = true;
                        }
                        else if (int.TryParse(keystring, out var result))
                        {
                            capture.Device = devices[result];
                            optionSelected = true;
                        }
                    }

                }

                ColorConsole.WriteLine("Recording initialising", ConsoleColor.Blue);
                capture.Initialize();

                using (WaveRecorder waveRecorder = new WaveRecorder(waveFileName, capture))
                {
                    ColorConsole.Write("Press ENTER start recording $", ConsoleColor.White);
                    if (prompt) WaitForEnter();

                    ColorConsole.WriteLine("Writing wave to file '{0}'", ConsoleColor.Blue, waveFileName);
                    waveRecorder.StartRecording();

                    ColorConsole.Write("Recording... Press ENTER to end recording $", ConsoleColor.White);
                    WaitForEnter();

                    waveRecorder.EndRecording();
                }

                ColorConsole.WriteLine("Finished recording. '{0}' written to disk.", ConsoleColor.DarkMagenta, Path.GetFullPath(waveFileName));

                if (includeMp3)
                {
                    ToMp3 toMp3 = new ToMp3();
                    toMp3.ConvertFromWave(waveFileName, mp3FileName);

                    ColorConsole.WriteLine("Finished creating mp3. '{0}'", ConsoleColor.DarkMagenta, Path.GetFullPath(mp3FileName));
                    ColorConsole.Write("Press ENTER to exit application $", ConsoleColor.White, Path.GetFullPath(mp3FileName));
                    WaitForEnter();
                }
            }
        }

        private static void WaitForEnter()
        {
            bool enterPressed = false;
            while (!enterPressed)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Enter)
                {
                    enterPressed = true;
                }
            }

            Console.WriteLine();
        }
    }

 
}