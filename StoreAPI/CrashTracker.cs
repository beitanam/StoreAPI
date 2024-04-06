using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StoreAPI
{
    public class CrashTracker
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static void generateReport(string text)
        {
            string PathFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\ErrorReport" + RandomString(8) + ".txt";
            System.IO.File.WriteAllText(PathFile, text);
        }
    }
}
