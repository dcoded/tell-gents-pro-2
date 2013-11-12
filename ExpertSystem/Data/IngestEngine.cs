using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Reflection;
using System.Collections;

namespace ExpertSystem.Data
{
   /* Responsible to read data from file
    * 
    * Spec 2.1 - Data Source
    * 
    * We are going to attempt to use a dataset which is used to predict
    * a naturally occuring periodic weather event known as El Nino. We
    * will attempt to identify likely bad quality entries (that we might
    * have to create ourselves) using our expert system project.
    */


    /* FIELDS
    * 
    * 0: buoy
    * 1: day
    * 2: latitude
    * 3: longitude
    * 4: zon.winds
    * 5: mer.winds
    * 6: humidity
    * 7: air temp.
    * 8: s.s.temp.
    */


    /**
    * IngestEngine class reads and parses the dataset into a friendlier
    * format as a POJO (see Tuple.java). It presents an iterator
    * approach to processing and hides the file operations and data
    * conversion.
    * 
    * @author Denis Coady
    * @version 0.0.1, Oct 2013
    */
    class IngestEngine<T> : IEnumerable<T> where T : new()
    {
        private string source_ = null;

        /**
         * Constructor creates the needed file stream readers.
         * 
         * @param   dataset   the file containing the dataset
         */
        public IngestEngine(string source)
        {
            source_ = source;
        }


        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TupleEnum<T> GetEnumerator()
        {
            return new TupleEnum<T>(source_);
        }
    }








   /**
    * Parses and returns the next entry in dataset as POJO using a
    * generator pattern.
    * 
    * @return   the next parsed line as a Tuple object or
    *           null if end of file
    */
    public class TupleEnum<T> : IEnumerator<T> where T : new()
    {
        private StreamReader reader_ = null;
        private string source_ = null;
        private FieldInfo[] fields_ = null;

        public TupleEnum(string source)
        {
            source_ = source;
            fields_ = typeof(T).GetFields();

            Reset();
        }

        void IDisposable.Dispose() { }

        public bool MoveNext()
        {
            return (reader_.EndOfStream == false);
        }

        public void Reset()
        {
            reader_ = new StreamReader(source_);
        }

        public void Close()
        {
            reader_.Close();
        }

        T IEnumerator<T>.Current
        {
            get
            {
                return Current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public T Current
        {
            get
            {
                try { return ParseLine(); }
                catch (EndOfStreamException) { throw new InvalidOperationException(); }
                finally { }
            }
        }



        private T ParseLine()
        {
            T data = new T();

            string[] token = reader_.ReadLine().Split(' ');

            for (int i = 0; i < fields_.Length; i++)
            {
                switch (fields_[i].FieldType.ToString())
                {
                    case "System.Double":
                        {
                            double result;
                            if (Double.TryParse(token[i], out result))
                            {
                                fields_[i].SetValue(data, result);
                            }
                            break;
                        }
                    case "System.Int32":
                        {
                            int result;
                            if (Int32.TryParse(token[i], out result))
                            {
                                fields_[i].SetValue(data, result);
                            }
                            break;
                        }
                }
            }

            return data;
        }
    }
}
