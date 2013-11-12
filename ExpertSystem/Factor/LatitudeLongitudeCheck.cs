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
    * A simple test which checks that coordinates are valid
    * 
    * @author Denis Coady
    * @version 0.0.1, Oct 2013
    */
    class LatitudeLongitudeCheck : QualityFactor
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
            if(entry.latitude  <= 90.0  && entry.latitude >= -90.0 &&
               entry.longitude <= 180.0 && entry.longitude >= -180.0)
            {
                    return 1.0f;
            }
            else
            {
                    return 0.0f;
            }
        }

    }
}
