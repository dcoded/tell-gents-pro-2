//using System;
//using System.Linq;
//using System.Text;
using ExpertSystem.Data;
using ExpertSystem.Factor;
using System.Collections.Generic;

namespace ExpertSystem.Engine
{
    class QualityReport
    {
        private Tuple tuple_;
        private Dictionary<QualityFactor, float> factors_;

        /**
         * Constructor is given the Tuple (see Tuple.java) object
         * analyzed, the tests performed, and the resulting score.
         * 
         * @param   tuple         is the Tuple analyzed
         * 
         * @param   factors       holds the tests and individual scores
         */
        public QualityReport(Tuple tuple,  Dictionary<QualityFactor, float> factors)
        {
            tuple_   = tuple;
            factors_ = factors;
        }


        /**
         * Calculates and returns the aggregate score from individual tests
         * 
         * @return   the aggregate quality score
         */
        public float quality()
        {
            float quality = 1.0f;

            foreach(KeyValuePair<QualityFactor, float> factor in factors_)
            {
                quality *= (factor.Value < 0.5) ? 0.5f : factor.Value;
                
            }
            return quality;
        }

        /**
         * Returns the tuple object
         * 
         * @return   the tuple object
         */
        public Tuple tuple()
        {
            return tuple_;
        }


        /**
         * Returns the tests and their scores of the tuple
         * 
         * @return   a map of <test,score> for the tuple
         */
        public Dictionary<QualityFactor, float> factors()
        {
            return factors_;
        }

        /**
         * Prints component tests name and component scores
         */
        //public void print()
        //{
        //    for(Entry<Class<? extends QualityFactor>, Float> f : factors_.entrySet())
        //    {
        //        Console.Out.WriteLine(f.getKey().getName() + ": " + f.getValue());
        //    }
        //}

    }
}
