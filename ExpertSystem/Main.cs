using ExpertSystem.Engine;
using ExpertSystem.Factor;
using ExpertSystem.Data;

namespace ExpertSystem
{
   /**
    * 
    * @author Denis Coady
    * @version 0.0.1, Oct 2013 
    */
    class Program
    {
        private static QualityEvaluationEngine quality = new QualityEvaluationEngine(
                            new FieldValidation(),
                            new LatitudeLongitudeCheck(),
                            new TemperatureCheck(),
                            new HumidityCheck(),
                            new WindVelocityCheck(),
                            new RegressionAnalysis());

        static void Main(string[] args)
        {
        }
    }
}
