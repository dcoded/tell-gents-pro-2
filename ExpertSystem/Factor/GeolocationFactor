using System.Reflection;
using System.Colletions.Generic;
using ExpertSystem.Data;
using MathNet.Numerics;
using System;

namespace ExpertSystem.Factor
{

/* 2.2.3 Geolocation Checks
    * 
    * While the Buoys in the El Nino sensor set do drift, we must ensure
    * that they still exist within acceptable limits. As stated in 
    * http://archive.ics.uci.edu/ml/machine-learning-databases/el_nino-mld/el_nino.data.html,
    * the acceptable drift range is one degree for lattitude, and five
    * degrees for longitude.
    */

   /**
    * GeolocationFactor runs checks on all buoys to verify that they exist
    * within 1 degree lattitude and 5 degrees longitude of the average location
    * for a given buoy. 
    * 
    * @author John Dvorak
    * @version 0.0.1, Nov 2013
    */
    class GeolocationFactor : QualityFactor
    {
    
    // key is the buoy number, value is average_longitude or average_latitude
    private Dictionary<int, float > longitude = new Dictionary<int, float >();
    private Dictionary<int, float > latitude = new Dictionary<int, float >();
    
        /**
         * Performs the geolocational check on a buoy
         * @param   tuple   the dataset entry to analyze
         * @return          the level of confidence that the entry is of quality
         * @see             QualityFactor
         */
        public float run(Data.Tuple tuple)
        {
            double confidence = 1;
            
            
            if longitude.ContainsKey(tuple.buoy) {
                if Math.Abs(tuple.longitude - longitude[tuple.buoy]) > 5 {
                  confidence *= 0.5;
                }
                longitude[tuple.buoy] = (tuple.day - 1)*longitude[tuple.buoy] + tuple.longitude;
            }
            else { longitude.Add(tuple.buoy, tuple.longitude };
            
            if latitude.ContainsKey(tuple.buoy) {
                if Math.Abs(tuple.longitude - longitude[tuple.buoy]) > 1 {
                  confidence *= 0.5;
                }
                longitude[tuple.buoy] = (tuple.day - 1)*longitude[tuple.buoy] + tuple.longitude;
            }
            else { latitude.Add(tuple.buoy, tuple.latitude); }
            
            return (float)confidence;
        }

    
    
    }

}
