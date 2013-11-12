using System;

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
    * A simple test which checks that temperatures are possible
    * 
    * @author Denis Coady
    * @version 0.0.1, Oct 2013
    */
    class WindVelocityCheck : QualityFactor
    {
        /**
         * Constructor builds the analysis chain given user passed
         * QualityFactor (see QualityFactor.java) tests.
         * 
         * @param   entry   an entry of the dataset under analysis
         * @return          confidence level of quality for entry
         * @see             QualityFactor
         */
        public float run(Data.Tuple entry)
        {
            double confidence = 1.0f;

            double abs_zon = Math.Abs(entry.zon_winds);
            double abs_mer = Math.Abs(entry.mer_winds);

            if (abs_zon > 20)
                confidence *= Math.Pow(0.5, abs_zon / 20);

            if (abs_mer > 20)
                confidence *= Math.Pow(0.5, abs_mer / 20);



            return (float)confidence;
        }
    }
}
