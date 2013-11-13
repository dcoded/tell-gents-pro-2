using System.Reflection;
using ExpertSystem.Data;
using MathNet.Numerics;
using System;

namespace ExpertSystem.Factor
{
   /* 2.2.3 Regression Analysis and Field Correlation Checks
    * 
    * The fields of the dataset may be calculated for Pearson product-moment
    * correlation coefficients. These coefficients will be used to check
    * incoming tuples independent of the data set. If the field correlation
    * coefficients do not agree with that of the data set then it shows
    * erroneous behavior in measurement. Correlations may not exist or be
    * strong enough to determine correctness of new data points.
    */

   /**
    * RegressionAnalysis runs correlation checks on all fields
    * combinations in sets of two.  If a set of fields register a
    * Pearson correlation greater than PPMCC_STRONG_BOUNDRY then a
    * simple linear regression analysis is performed.  The regression
    * error (e) is used for the confidence returned as (1 - error).  The
    * final confidence is calculated using a multiplication chain of
    * each regression test performed.
    * 
    * @author Denis Coady
    * @version 0.0.1, Oct 2013
    */
    class RegressionAnalysis : QualityFactor
    {
        private static double PPMCC_STRONG_BOUNDRY  = 0.9;
        private static double ERROR_DILUTION_FACTOR = 0.5;
        private static double FLYWHEEL_ENABLE_WAIT  = 8;

        private SimpleRegression[] regressions_;

        private FieldInfo[] fields_;

        private int length_;
        private int combs_;
        private int count_;

        private double highest_confidence_ = 0.0;

        /**
         * Constructor inspects the tuple fields via reflection and
         * creates the required number of regression models.
         */
        public RegressionAnalysis()
        {
            fields_ = typeof(Data.Tuple).GetFields();
            length_ = fields_.Length;

            combs_ = Convert.ToInt32(Combinatorics.Combinations(length_, 2));

            regressions_ = new SimpleRegression[combs_];
            for (int i = 0; i < regressions_.Length; i++)
            {
                regressions_[i] = new SimpleRegression();
            }
        }


        /**
         * Performs correlation and regression analysis on all
         * combinations of 2 fields in the tuple.
         * 
         * @param   tuple   the dataset entry to analyze
         * @return          the level of confidence that the entry is of quality
         * @see             QualityFactor
         */
        public float run(Data.Tuple tuple)
        {
            double confidence = 0;
            double[] values = ParseValues(tuple);

            int i = 0;

            count_++;

            for (int a = 0; a < length_; a++)
            {
                for (int b = a + 1; b < length_; b++)
                {
                    confidence += FilterWeakCorrelations(a, b, values, regressions_[i++]);
                }
            }

            confidence /= length_ * length_;

            if (confidence > highest_confidence_)
                highest_confidence_ = confidence;


            confidence /= highest_confidence_;

            return (float)confidence;

    
        }

        /**
         * Checks for pairs of fields which show high correlation by
         * calculating Pearson's product moment correlation coefficient
         * of the two fields.  If a field is high enough (determined by
         * PPMCC_STRONG_BOUNDRY) then the returned confidence is
         * calculated as (1 - predicted error), otherwise 1.0 is returned
         * to signify no obvious relationship to test for or against.
         * 
         * @param   a        one of the fields
         * @param   b        one of the fields (which is not a)
         * @param   values   a linear array of field values
         * @param   reg      the specific regression model for the two fields
         * @return           the level of confidence that the entry is of quality
         */
        private double FilterWeakCorrelations(int a, int b, double[] values, SimpleRegression reg)
        {
            double confidence = 1.0;
            double pearson = reg.GetPearson();

            if (Math.Abs(pearson) > PPMCC_STRONG_BOUNDRY)
            {
                confidence = RegressionAccuracy(values[a], values[b], reg);
            }

            reg.AddData(values[a], values[b]);
            return confidence;
        }


        /**
         * Performs the regression analysis and returns a weighted
         * confidence of quality as (1 - % predicted error).
         * 
         * @param   a        one of the fields
         * @param   b        one of the fields (which is not a)
         * @param   reg      the specific regression model for the two fields
         * @return           the level of confidence that the entry is of quality
         */
        private double RegressionAccuracy(double a, double b, SimpleRegression reg)
        {
            reg.CreateModel();

            double b_hat = reg.Predict(a);
            double error = (Math.Abs(b_hat - b) / Math.Abs(b)) * ERROR_DILUTION_FACTOR;

            if (error > 1.0)
                error = 1.0;

            return 1 - error;
        }


        /**
         * Converts a POJO tuple into a linear array of field values.
         * 
         * @param   tuple   the tuple entry to extract values from
         * @return          the array of values
         */
        private double[] ParseValues(Data.Tuple tuple)
        {
            double[] values = new double[length_];

            for (int i = 1; i < length_; i++)
            {
                values[i] = Convert.ToDouble(fields_[i].GetValue(tuple));
            }

            return values;
        }
    }
}
