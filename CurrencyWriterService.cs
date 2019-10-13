using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CurrencyWriterService
{
    public partial class CurrencyWriterService : ServiceBase
    {
        public CurrencyWriterService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            TimerCallback tm = new TimerCallback(ReadCurrencyFromFile);
            Timer timer = new Timer(tm, 0, 0, 10800000);
        }

        public static void ReadCurrencyFromFile(object obj)
        {
            try
            {
                string readFromPath = @"C:\homework\readFromSite.txt";
                string writeToPath = @"C:\homework\readFromFile.txt";
                lock (obj)
                {
                    if (File.Exists(readFromPath))
                    {
                        List<string> lines = new List<string>();
                        using (StreamReader sr = new StreamReader(readFromPath))
                        {
                            String line;
                            while ((line = sr.ReadLine()) != null)
                                lines.Add(line);
                            sr.Close();
                        }
                        using (StreamWriter sw = new StreamWriter(writeToPath, false, System.Text.Encoding.Default))
                        {
                            if (lines.Any())
                                foreach (var l in lines)
                                    sw.WriteLine(l);
                            else
                                Thread.Sleep(1000);
                            sw.Close();
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Thread.Sleep(1000);
            }

        }

        protected override void OnStop()
        {
        }
    }


}
