using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore.Codecs.WAV;
using CSCore.SoundIn;

namespace MikeyFriedChicken.AudioCapture
{
    public class WaveRecorder : IDisposable
    {
        private readonly WaveWriter _waveWriter;
        private readonly WasapiCapture _capture;

        public WaveRecorder(string filename, WasapiCapture capture)
        {
            _waveWriter = new WaveWriter(filename, capture.WaveFormat);
            _capture = capture;
            capture.DataAvailable += OnCaptureOnDataAvailable;
        }

        private void OnCaptureOnDataAvailable(object sender, DataAvailableEventArgs e)
        {
            _waveWriter.Write(e.Data, e.Offset, e.ByteCount);
        }

        public void StartRecording()
        {
            _capture.Start();
        }

        public void EndRecording()
        {
            _capture.Stop();
        }

        public void Dispose()
        {
            _waveWriter?.Dispose();
        }
    }
}
