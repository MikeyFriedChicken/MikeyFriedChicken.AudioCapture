# Audio Capture Tool
- .net console application to capture audio as a WAV and MP3 playing from any device on your computer
- This application uses the [cscore](https://github.com/filoe/cscore) library
- From your shell or command line:
`git clone https://github.com/MikeyFriedChicken/MikeyFriedChicken.AudioCapture`
- Open in Visual Studio 2017 and build or from the project folder
`dotnet build`
- To run go the the ./bin/debug folder and run
```console
 MikeyFriedChicken.AudioCapture.exe
```
- You should see somthing similar to the following
```console
Starting audio recorder...
Available devices:
1. Headphones (High Definition Audio Device)
2. DELL P2415Q (Intel(R) Display Audio) [default]
3. Digital Audio (HDMI) (High Definition Audio Device)
4. Speakers (Realtek High Definition Audio)
5. Speakers (High Definition Audio Device)
6. Internal AUX Jack (High Definition Audio Device)
Select which device above to record from 1,2,3... (Enter for default - DELL P2415Q (Intel(R) Display Audio)) $
Recording initialising
Press ENTER start recording $
Writing wave to file 'output.wav'
Recording... Press ENTER to end recording $
Finished recording
C:\Users\michael\code\GitHub\MikeyFriedChicken.AudioCapture\MikeyFriedChicken.AudioCapture\bin\Debug\output.wav written to disk.
Creating mp3 from wav...
Wav -> Mp3 Progress: 19.16%/100.00%
Wav -> Mp3 Progress: 38.31%/100.00%
Wav -> Mp3 Progress: 57.47%/100.00%
Wav -> Mp3 Progress: 76.63%/100.00%
Wav -> Mp3 Progress: 95.79%/100.00%
Wav -> Mp3 Progress: 100.00%/100.00%
Finished creating mp3
C:\Users\michael\code\GitHub\MikeyFriedChicken.AudioCapture\MikeyFriedChicken.AudioCapture\bin\Debug\output.mp3 written to disk.
Press ENTER to exit application $
End of audio recorder
```

- The following are optional arguments:
```
f:WaveFileName          Name of wave file to generate
i:IncludeMP3            If true then the the output will also include an mp3 file
m:MP3FileName           Name of mp3 file to generate
```
- For example:
```console
MikeyFriedChicken.AudioCapture.exe --WaveFileName "mywavefile.wav" --MP3FileName "myMp3File.mp3" --IncludeMP3 true
```

## How It Works
### Define the format for the wave file
```c#
WaveFormat waveFormat = new WaveFormat(sampleRate,bits,channels);
```
### Prepare the capture class
```c#
WasapiCapture capture = new WasapiLoopbackCapture(latency, waveFormat,ThreadPriority.Highest)
```
### Optionally choose one of the available devices for capturing
```c#
foreach (MMDevice device in MMDeviceEnumerator.EnumerateDevices(DataFlow.Render)) 
{
  // Check available devices
}
```
### Start and Stop capturing the audio
```c#
_waveWriter = new WaveWriter(filename, capture.WaveFormat);
capture.Start();
// app waits here
capture.Stop();
        
```
