namespace DeviceDriverPluginSystem.GenericSensor
{
    public abstract class SensorDataItem
    {
        public string Key { get; }
        public string Text { get; }

        internal SensorDataItem(string key, string text)
        {
            Key = key;
            Text = text;
        }
    }
    public class SensorDataItem<SensorDataType> : SensorDataItem
    {
        public SensorDataType Value { get; }

        public SensorDataItem(string key, string text, SensorDataType value) : base(key, text)
        {
            Value = value;
        }
	}
}
