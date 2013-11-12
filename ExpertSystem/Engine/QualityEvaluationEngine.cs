using System.Collections.Generic;
using ExpertSystem.Factor;
using ExpertSystem.Data;


namespace ExpertSystem.Engine
{
    class QualityEvaluationEngine
    {
        private QualityFactor[] components_;
        private Dictionary<QualityFactor, float> factors_;
 
        /**
         * Constructor builds the analysis chain given user passed
         * QualityFactor (see QualityFactor.java) tests.
         * 
         * @param   components   The tests to be used in determining
         *                       quality metrics.
         */
        public QualityEvaluationEngine(params QualityFactor[] components)
        {
            components_ = components;
            factors_    = new Dictionary<QualityFactor, float>();
        }

        /**
         * Parses and returns the next entry in dataset as POJO using a
         * generator pattern.
         * 
         * @param   entry         is a Tuple (see Tuple.java) object
         * @return                QualityReport
         */
        public QualityReport ingest(Tuple entry)
        { 
            QualityReport report = null;
            
            // run tuple through each test
            foreach(QualityFactor factor in components_)
            {
                factors_[factor] = factor.run(entry);
            }
        
            report = new QualityReport(entry, factors_);
 
            return report;
        }

    }
}
