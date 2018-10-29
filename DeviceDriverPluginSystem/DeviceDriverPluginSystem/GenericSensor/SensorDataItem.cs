namespace DeviceDriverPluginSystem.GenericSensor
{
    public abstract class SensorDataItem
    {
        public string Key { get; private set; }
        public string Text { get; private set; }
    }
    public class SensorDataItem<T> : SensorDataItem
    {
        public T Value { get; private set; }
	}
}
