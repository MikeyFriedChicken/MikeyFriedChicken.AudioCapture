using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLineParser.Core;

namespace MikeyFriedChicken.AudioCapture
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var p = new FluentCommandLineParser<CommandLineArgs>();

                p.Setup(arg => arg.WaveFileName)
                    .As('f', "WaveFileName") 
                    .WithDescription("Name of wave file to generate")
                    .SetDefault("output.wav");

                p.Setup(arg => arg.MP3FileName)
                    .As('m', "MP3FileName")
                    .WithDescription("Name of mp3 file to generate")
                    .SetDefault("output.mp3");

                p.Setup(arg => arg.IncludeMP3)
                    .As('i', "IncludeMP3")
                    .WithDescription("If true then the the output will also include an mp3 file")
                    .SetDefault(true);

                p.Setup(arg => arg.Prompt)
                    .As('p', "Prompt")
                    .WithDescription("If true then the application will include prompts.")
                    .SetDefault(false);

                p.SetupHelp("?", "help").Callback(text => ColorConsole.WriteLine(text,ConsoleColor.Blue));
                string helptext = p.HelpOption.ToString();

                ICommandLineParserResult parserResult = p.Parse(args);

                if (parserResult.HasErrors)
                {
                    ColorConsole.WriteLine(parserResult.ErrorText, ConsoleColor.Red);
                    p.HelpOption.ShowHelp(p.Options);
                }
                else
                {
                    var arguments = p.Object;

                    ColorConsole.WriteLine("Starting audio recorder...", ConsoleColor.Blue);
                    Recorder recorder = new Recorder();
                    recorder.Record(arguments.WaveFileName, arguments.MP3FileName, arguments.IncludeMP3, arguments.Prompt);

                    ColorConsole.WriteLine("End of audio recorder", ConsoleColor.Blue);
                }


            }
            catch (Exception exception)
            {
                ColorConsole.WriteLine(exception.ToString(), ConsoleColor.Red);
            }
        }
    }
}
