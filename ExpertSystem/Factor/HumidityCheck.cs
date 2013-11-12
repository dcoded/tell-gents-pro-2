using ExpertSystem.Data;

namespace ExpertSystem.Factor
{
   /* 2.2.1 Field Validation
    * 
    * We will first attempt to invalidate the entire entry by checking for
    * missing fields, or fields containing impossible values. Otherwise our
    * system will attempt to determine if the value makes sense according
    * to the previous history of entries. A field like temperature should
    * not dramatically increase or decrease for instance.
    */

   /**
    * A simple test which checks that the humidity levels are possible
    * 
    * @author Denis Coady
    * @version 0.0.1, Oct 2013
    */
    class HumidityCheck : QualityFactor
    {
        /**
         * Constructor builds the analysis chain given user passed
         * QualityFactor (see QualityFactor.java) tests.
         * 
         * @param   entry   an entry of the dataset under analysis
         * @return          confidence level of quality for entry
         * @see             QualityFactor
         */
        public float run(Tuple entry)
        {
            return (entry.humidity >= 0.0 && entry.humidity <= 100.0) ? 1.0f : 0.0f;
        }
    }
}
