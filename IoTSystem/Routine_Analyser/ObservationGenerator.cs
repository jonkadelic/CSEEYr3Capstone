using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routine_Analyser
{
    public static class ObservationGenerator
    {

        //public static double[][] Observations
        //{
        //    get
        //    {

        //    }
        //}

        public static double[][] GetObservations()
        {
            List<double[]> l = new List<double[]>();
            var temp = DatabaseHandler.Collection.AsQueryable().Select(x => x.Snapshot.Devices).ToList();
            List<List<dynamic>> valueList = temp.Select(x => x.SelectMany(y => y.Attributes.Select(z => z.Value)).ToList()).ToList();
            foreach (List<dynamic> values in valueList)
            {
                List<double> doubles = new List<double>();
                foreach (dynamic d in values)
                {
                    if (d is bool)
                        doubles.Add(d ? 1.0 : 0.0);
                    else if (d is int)
                        doubles.Add((double)d);
                    else if (d is double)
                        doubles.Add(d);
                }
                l.Add(doubles.ToArray());
            }
            return l.ToArray();
        }

        public class Observation
        {
            public double Value { get; private set; }
            public string DeviceID { get; private set; }
            //public string 
        }
    }
}
