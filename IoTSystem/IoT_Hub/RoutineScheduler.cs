using IoT_Hub.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IoT_Hub
{
    public static class RoutineScheduler
    {
        public static List<Routine> routines;
        public static void Run()
        {
            routines = DatabaseHandler.LoadRoutines();
            Utility.WriteTimeStamp($"Loaded {routines.Count} routines.", typeof(RoutineScheduler));
            new Thread(PollRoutines).Start();
        }

        public static void ReloadRoutines()
        {
            routines = DatabaseHandler.LoadRoutines();
            Utility.WriteTimeStamp($"Reloaded {routines.Count} routines.", typeof(RoutineScheduler));
        }

        private static void PollRoutines()
        {
            while (true)
            {
                Thread.Sleep(5000);
                Utility.WriteTimeStamp("Polling routines...", typeof(RoutineScheduler));
                Dictionary<string, DriverDevice> ddList = DriverLoader.Drivers.SelectMany(x => x.Devices).ToDictionary(t => t.Id, t => t);
                foreach (Routine r in routines)
                {
                    bool executeRoutine = false;
                    foreach (RoutineCondition rc in r.RoutineConditions)
                    {
                        DriverDevice dd;
                        try
                        {
                            dd = ddList[rc.DeviceID];
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        dynamic prop = dd.DeviceBase.DeviceProperties.Single(x => x.Label == rc.PropertyName);
                        dynamic val = prop.Get();
                        if (rc.Comparison == RoutineCondition.COMPARISON.EQUAL)
                        {
                            if (val == rc.DesiredValue)
                                executeRoutine = true;
                        }
                        else if (rc.Comparison == RoutineCondition.COMPARISON.GREATER)
                        {
                            if (val > rc.DesiredValue)
                                executeRoutine = true;
                        }
                        else if (rc.Comparison == RoutineCondition.COMPARISON.LESS)
                        {
                            if (val < rc.DesiredValue)
                                executeRoutine = true;
                        }
                    }
                    if (executeRoutine == true)
                    {
                        DriverDevice dd = ddList[r.TargetDeviceID];
                        dynamic prop = dd.DeviceBase.DeviceProperties.Single(x => x.Label == r.TargetProperty);
                        prop.Set(r.TargetValue);
                        Utility.WriteTimeStamp($"Executing {r.RoutineName}", typeof(RoutineScheduler));
                    }
                }
            }
        }
    }
}
