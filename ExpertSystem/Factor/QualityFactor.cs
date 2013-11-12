using ExpertSystem.Data;

namespace ExpertSystem.Factor
{
    /**
     * A standardized interface for tests so that the quality engine
     * (QualityEvaluationEngine.java) can create an automated chain
     * of analysis.
     * 
     * @author Denis Coady
     * @version 0.0.1, Oct 2013
     */
    public interface QualityFactor
    {
       /* Should provide a value between 0 and 1 */
       /**
        * Performs a quality test on a given dataset entry.  The value
        * returned should be a confidence factor between 0.0 and 1.0
        * where 0 is no confidence and 1.0 is total confidence in the
        * quality of the data.
        * 
        * @param   entry   an entry of the dataset under analysis
        * @return          confidence level of quality for entry
        */

        float run(Tuple entry);
    }

}
