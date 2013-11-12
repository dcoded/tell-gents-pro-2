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
    * A simple test which requires all fields in Tuple to be set
    * or else automatically rejects entry in analysis chain.
    * 
    * @author Denis Coady
    * @version 0.0.1, Oct 2013
    */
    class FieldValidation : QualityFactor
    {
        /**
         * Constructor builds the analysis chain given user passed
         * QualityFactor (see QualityFactor.java) tests.
         * 
         * @param  components   The tests to be used in determining quality metrics.
         * @see                 QualityFactor
         */
        public float run(Tuple entry)
        {
            return (entry.valid()) ? 1.0f : 0.0f;
        }
    }
}
