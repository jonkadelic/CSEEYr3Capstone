using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routine_Analyser
{
    class Program
    {
        static void Main(string[] args)
        {
            string dbLocale = "mongodb://127.0.0.1:27017/?gssapiServiceName=mongodb";
            List<string> argsList = new List<string>(args);
            int a = argsList.IndexOf("-d");
            int argsPos = a == -1 ? argsList.IndexOf("--database") : a;
            if (argsPos != -1)
            {
                if (argsList.Count - 1 > argsPos) dbLocale = argsList[argsPos + 1];
            }
            DatabaseHandler.Startup(dbLocale);

            double[][] d = DatabaseHandler.GetObservation();

            foreach (double[] c in d)
            {
                Console.WriteLine(string.Join(",", c));
            }

            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
