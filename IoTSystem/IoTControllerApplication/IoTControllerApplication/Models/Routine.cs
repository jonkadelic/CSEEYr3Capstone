using System;
using System.Collections.Generic;
using System.Text;

namespace IoTControllerApplication.Models
{
    public class Routine
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string TargetDeviceID { get; set; }

        public string TargetDriverID { get; set; }

        public string TargetProperty { get; set; }

        public string TargetValue { get; set; }

        public List<RoutineCondition> Conditions { get; set; }

        public Routine(string routineId, string routineName, string targetDeviceId, string targetDriverId, string targetProperty, string targetValue)
        {
            ID = routineId;
            Name = routineName;
            TargetDeviceID = targetDeviceId;
            TargetDriverID = targetDriverId;
            TargetProperty = targetProperty;
            TargetValue = targetValue;
            Conditions = new List<RoutineCondition>();
        }

        public Routine()
        {
            ID = "";
            Name = "New routine";
            TargetDeviceID = "";
            TargetDriverID = "";
            TargetProperty = "";
            TargetValue = "";
            Conditions = new List<RoutineCondition>();

        }
    }
}
