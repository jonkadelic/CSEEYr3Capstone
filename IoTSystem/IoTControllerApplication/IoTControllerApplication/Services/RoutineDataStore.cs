using IoTControllerApplication.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IoTControllerApplication.Services
{
    public class RoutineDataStore : IDataStore<Routine>
    {

        public static RoutineDataStore DataStore { get; private set; } = new RoutineDataStore();

        List<Routine> routines;

        static string HttpUrl => DeviceDataStore.HttpUrl;

        public RoutineDataStore()
        {
            routines = new List<Routine>();
        }

        public async Task<bool> AddItemAsync(Routine routine)
        {
            JObject outObject = new JObject()
            {
                { "name", routine.Name },
                { "target", new JObject()
                    {
                        { "deviceId", routine.TargetDeviceID },
                        { "driverId", routine.TargetDriverID },
                        { "property", routine.TargetProperty },
                        { "value", routine.TargetValue }
                    }
                },
                { "conditions", GetRoutineConditions(routine.Conditions) }
            };
            using (WebClient client = new WebClient())
            {
                await client.UploadDataTaskAsync($"{HttpUrl}/routines/new/", "PUT", Encoding.ASCII.GetBytes(outObject.ToString()));
            }
            await GetItemsAsync(true);
            return await Task.FromResult(true);
        }

        public JArray GetRoutineConditions(List<RoutineCondition> conditions)
        {
            JArray array = new JArray();
            foreach (RoutineCondition rc in conditions)
            {
                array.Add(GetRoutineCondition(rc));
            }
            return array;
        }

        public JObject GetRoutineCondition(RoutineCondition c)
        {
            JObject outObject = new JObject()
            {
                { "deviceId", c.DeviceID },
                { "driverId", c.DriverID },
                { "property", c.PropertyName },
                { "value", c.DesiredValue },
                { "comparison", c.Comparison.ToString("g") } // format specifier "g" means to provide the literal name of the enum member
            };
            return outObject;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            using (WebClient client = new WebClient())
            {
                await client.DownloadStringTaskAsync($"{HttpUrl}/routines/{id}/delete/");
            }
            await GetItemsAsync(true);

            return await Task.FromResult(true);

        }

        public async Task<Routine> GetItemAsync(string id)
        {
            return await Task.FromResult(routines.FirstOrDefault(r => r.ID == id));
        }

        public async Task<IEnumerable<Routine>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh)
            {
                JArray ja;
                routines.Clear();
                using (WebClient client = new WebClient())
                {
                    string s = await client.DownloadStringTaskAsync($"{HttpUrl}/routines/");
                    ja = JArray.Parse(s);
                }
                foreach (JToken rt in ja)
                {
                    Routine routine = new Routine(rt["id"].Value<string>(), rt["name"].Value<string>(), rt["target"]["deviceId"].Value<string>(), rt["target"]["driverId"].Value<string>(), rt["target"]["property"].Value<string>(), rt["target"]["value"].Value<string>());
                    foreach (JToken rct in rt["conditions"])
                    {
                        routine.Conditions.Add(new RoutineCondition(rct["id"].Value<string>(), rct["driverId"].Value<string>(), rct["deviceId"].Value<string>(), rct["property"].Value<string>(), rct["value"].Value<string>(), rct["comparison"].Value<string>()));
                    }
                    routines.Add(routine);
                }
            }
            return await Task.FromResult(routines);
        }

        public async Task<bool> UpdateItemAsync(Routine item)
        {
            throw new NotImplementedException();
        }
    }
}
