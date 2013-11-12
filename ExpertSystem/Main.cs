using ExpertSystem.Engine;
using ExpertSystem.Factor;
using ExpertSystem.Data;

using System.Collections.Generic;
using System.IO;
using System;

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

        private static QualityEvaluationEngine quality = new QualityEvaluationEngine(
                            new FieldValidation(),
                            new LatitudeLongitudeCheck(),
                            new TemperatureCheck(),
                            new HumidityCheck(),
                            new WindVelocityCheck(),
                            new RegressionAnalysis());

        private static List<QualityReport> lowq_reports = new List<QualityReport>();

        static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                    Console.Out.WriteLine("Usage: ExpertSystem <dataset>");
                    return;
            }

            if(!File.Exists(args[0]))
            {
                Console.Out.WriteLine("Dataset file not found at: " + args[0]);
                return;
            }

            try   { ingest = new IngestEngine<Data.Tuple>(args[0]); }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }

            int count = 0;
            List<QualityReport> lowq_reports = new List<QualityReport>();
            foreach (var tuple in ingest)
            {
                QualityReport report = quality.ingest(tuple);
                //quality_log.println("[" + report.quality() + "] " + entry);
                if (report.quality() >= 0.65)
                {
                    //quality_log.println("[" + report.quality() + "] " + entry);
                }
                else
                {
                    lowq_reports.Add(report);
                    //bad_quality.println(entry);
                }
                count++;
            }

            print_final_evaluation(args[0], count, lowq_reports.ToArray());

            Console.Out.WriteLine("Done!");
            Console.Read();
        }

        private static void print_final_evaluation(String filename, int size,
                                                   QualityReport[] lq_reports)
        {
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
