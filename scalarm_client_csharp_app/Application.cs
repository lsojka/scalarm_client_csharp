using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Scalarm.ExperimentInput;
using RestSharp.Deserializers;
using System.Threading;

namespace Scalarm
{
	public class Application
	{	
		public static bool ShouldWait = true;

		public static void ShowResults(object sender, IList<SimulationParams> results)
		{
            Experiment experiment = sender as Experiment;
			Console.WriteLine(string.Format("Experiment with id {0} done! Results:", experiment.Id));
			foreach (var r in results) {
				Console.WriteLine(string.Format("{0} -> {1}", r.Input, r.Output));
			}

			ShouldWait = false;
		}

		private static List<ValuesMap> points = new List<ValuesMap>();

		public static string ReadPassword()
		{
			string pass = "";
			Console.Write("Enter your PL-Grid password: ");
			ConsoleKeyInfo key;

			do
			{
				key = Console.ReadKey(true);

				// Backspace Should Not Work
				if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
				{
					pass += key.KeyChar;
					Console.Write("*");
				}
				else
				{
					if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
					{
						pass = pass.Substring(0, (pass.Length - 1));
						Console.Write("\b \b");
					}
				}
			}
			// Stops Receving Keys Once Enter is Pressed
			while (key.Key != ConsoleKey.Enter);

			Console.WriteLine("\n");

			return pass;
		}

		static private Random RAND = new Random((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);

		static public int GetRandomNumber(int n)
		{
			return RAND.Next(n);
		}

        public static Client CreateClient(string configFile)
        {
            string configText = System.IO.File.ReadAllText("config.json");
            var config = JsonConvert.DeserializeAnonymousType(configText, new { base_url = "", login = "", password = "" });
            return new Client(config.base_url, config.login, config.password);
        }

		static void Main()
		{
            ExampleFullRegister.Run();
		}

	}
}

