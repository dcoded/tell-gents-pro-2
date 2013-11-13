using ExpertSystem.Engine;
using ExpertSystem.Factor;
using ExpertSystem.Data;

using System.Collections.Generic;
using System.IO;
using System;
using System.Diagnostics;

namespace ExpertSystem
{
   /**
    * 
    * @author Denis Coady
    * @version 0.0.1, Oct 2013 
    */
    class Program
    {
        private static IngestEngine<Data.Tuple> ingest;
        private static Stopwatch stopwatch = new Stopwatch();

        private static QualityEvaluationEngine quality = new QualityEvaluationEngine(
                            new FieldValidation(),
                            new LatitudeLongitudeCheck(),
                            new GeolocationFactor(),
                            new TemperatureCheck(),
                            new HumidityCheck(),
                            new WindVelocityCheck(),
                            new RegressionAnalysis());

        private static List<QualityReport> lowq_reports = new List<QualityReport>();

        static void Main(string[] args)
        {
            //if(args.Length < 1)
            //{
            //        Console.Out.WriteLine("Usage: ExpertSystem <dataset>");
            //        return;
            //}
            string filename = "elnino.txt";

            if(!File.Exists(filename))
            {
                Console.Out.WriteLine("Dataset file not found at: " + filename);
                return;
            }

            stopwatch.Start();

            try
            {
                ingest = new IngestEngine<Data.Tuple>(filename);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }


            string path = Path.GetFullPath(filename);


            StreamWriter log = new StreamWriter(path + filename + ".log.txt");
            StreamWriter hq  = new StreamWriter(path + filename + ".hq.txt");
            StreamWriter lq  = new StreamWriter(path + filename + ".lq.txt");

      
  

            int count = 0;
            List<QualityReport> lowq_reports = new List<QualityReport>();
            foreach (var tuple in ingest)
            {
                QualityReport report = quality.ingest(tuple);
                log.WriteLine(String.Format("[{0,5}%]\t{1}",Math.Round(report.quality()*100,1), tuple.ToString()));

                float confidence = report.quality();
                if (confidence >= 0.25)
                {
                    hq.WriteLine(tuple.ToString());
                }
                else
                {
                    lowq_reports.Add(report);
                    lq.WriteLine(tuple.ToString());
                }
                count++;
            }


            hq.Close();
            lq.Close();
            log.Close();

            stopwatch.Stop();

            print_final_evaluation(filename, count, lowq_reports.ToArray());

            Console.Out.WriteLine("Done!");
            Console.Read();
        }

        private static void print_final_evaluation(String filename, int size,
                                                   QualityReport[] lq_reports)
        {
            Console.Out.WriteLine("Time    : " + stopwatch.Elapsed.ToString());
            Console.Out.WriteLine("File    : " + filename);
            Console.Out.WriteLine("Entries : " + size);
            Console.Out.WriteLine("Low Q   : " + lq_reports.Length);
            
            Console.Out.WriteLine("Rejected Entries:");
            foreach(QualityReport report in lowq_reports)
            {
                    Console.Out.WriteLine(report.tuple());
            }
        }
    }
}
