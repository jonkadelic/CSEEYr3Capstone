using System;
using System.Collections.Generic;
using System.Text;

namespace IoTControllerApplication.Models
{
    public class RoutineCondition
    {
        public RoutineCondition(string routineConditionID, string driverID, string deviceID, string propertyName, string desiredValue, string comparison)
        {
            RoutineConditionID = routineConditionID;
            DriverID = driverID;
            DeviceID = deviceID;
            PropertyName = propertyName;
            DesiredValue = desiredValue;
            switch (comparison)
            {
                case "EQUAL":
                    Comparison = COMPARISON.EQUAL;
                    break;
                case "LESS":
                    Comparison = COMPARISON.LESS;
                    break;
                case "GREATER":
                    Comparison = COMPARISON.GREATER;
                    break;
            }
        }
        public RoutineCondition(string routineConditionID, string driverID, string deviceID, string propertyName, string desiredValue, COMPARISON comparison)
        {
            RoutineConditionID = routineConditionID;
            DriverID = driverID;
            DeviceID = deviceID;
            PropertyName = propertyName;
            DesiredValue = desiredValue;
            Comparison = comparison;
        }

        public RoutineCondition()
        {
            RoutineConditionID = "";
            DriverID = "";
            DeviceID = "";
            PropertyName = "";
            DesiredValue = "";
            Comparison = COMPARISON.LESS;
        }

        public enum COMPARISON
        {
            EQUAL,
            LESS,
            GREATER
        }

        public string RoutineConditionID { get; set; }

        public string DriverID { get; set; }

        public string DeviceID { get; set; }

        public string PropertyName { get; set; }

        public string DesiredValue { get; set; }

        public COMPARISON Comparison { get; set; }

    }
}
