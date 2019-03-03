using IoT_Hub.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IoT_Hub
{
    public static class RoutineScheduler
    {
        private static List<Routine> routines;
        public static void Run()
        {
            routines = DatabaseHandler.LoadRoutines();
            Utility.WriteTimeStamp($"Loaded {routines.Count} routines.", typeof(RoutineScheduler));
            new Thread(PollRoutines).Start();
        }

        private static void PollRoutines()
        {
            Thread.Sleep(1000);
            Dictionary<string, DriverDevice> ddList = DriverLoader.Drivers.SelectMany(x => x.Devices).ToDictionary(t => t.Id, t => t);
            foreach(Routine r in routines)
            {
                bool executeRoutine = true;
                if (r.RoutineConditions.Count == 0) continue;
                foreach (RoutineCondition rc in r.RoutineConditions)
                {
                    DriverDevice dd = ddList[rc.DeviceID];
                    dynamic att = dd.DeviceBase.DeviceAttributes.Single(x => x.Label == rc.AttributeName);
                    dynamic val = att.Get();
                    if (val != rc.DesiredValue)
                        executeRoutine = false;
                }
                if (executeRoutine == true)
                {
                    DriverDevice dd = ddList[r.TargetDeviceID];
                    dynamic att = dd.DeviceBase.DeviceAttributes.Single(x => x.Label == r.TargetAttribute);
                    att.Set(r.TargetValue);
                }
            }
        }
    }
}
