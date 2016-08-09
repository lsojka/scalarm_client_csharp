using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

// ported
using System.Windows.Forms;
using System.Text;
using System.Linq;
using System.Reflection;

using Newtonsoft.Json;
using RestSharp.Deserializers;
using Scalarm.ExperimentInput;
using Scalarm;


namespace AppLogic
{
	public class Supervisor
	{
        // lots of fields!
        ScalarmAppConfig config;
        Client client;
        
        // methods
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

		public static void HandleNoResources(object sender)
		{
			Experiment experiment = sender as Experiment;
			Console.WriteLine(string.Format("Experiment with id {0} has no resources!", experiment.Id));

			ShouldWait = false;
		}

		// private static List<ValuesMap> points = new List<ValuesMap>();

		public static string ReadString(string prompt)
		{
			Console.Write(prompt + " ");
			return Console.ReadLine();
		}

		public static string ReadPassword(string prompt)
		{
			string pass = "";
			Console.Write(prompt + " ");
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


        // ------------------------
		public class ScalarmAppConfig
		{
			public string base_url = null;
			public string login = null;
			public string password = null;
			public string plgrid_login = null;
			public string proxy_path = null;
			public string experiment_id = null;
            /*
            private PropertyInfo[] _PropertyInfos = null;

            public override string ToString()
            {
                if (_PropertyInfos == null)
                    _PropertyInfos = this.GetType().GetProperties();

                var sb = new StringBuilder();

                foreach (var info in _PropertyInfos)
                {
                    var value = info.GetValue(this, null) ?? "(null)";
                    sb.AppendLine(info.Name + ": " + value.ToString());
                }

                return sb.ToString();
            }
             */
		}

		public static ScalarmAppConfig ReadConfig(string path)
		{
			string configText = System.IO.File.ReadAllText(path);
			return JsonConvert.DeserializeObject<ScalarmAppConfig> (configText);
            // DEBUG

		}

        public static Client CreateClient(ScalarmAppConfig config)
        {
			if (String.IsNullOrEmpty (config.proxy_path)) {
				return new BasicAuthClient (config.base_url, config.login, config.password);
			} else {
				return new ProxyCertClient (config.base_url, new FileStream (config.proxy_path, FileMode.Open));
			}					
        }
        /*
		static void Main()
		{
            // TODO - Run(configFile.Decomposed)
			new ExampleFullRegister().Run();
		}
        */

        public void register()
        {
            // register - first, with hardcoded data
            // then push to verver
            // lives only a click
            // 1. Load credentials
            // no try/catch as static metod does it safe
            config = Supervisor.ReadConfig("config.json");
            client = Supervisor.CreateClient(config);

            MessageBox.Show(config.ToString());
        }
	}
}

