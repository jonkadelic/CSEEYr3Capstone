namespace IoT_Hub
{
    public abstract class NetworkedSensorDataItem
    {
        public string Key { get; private set; }
        public string Text { get; private set; }
    }
    public class NetworkedSensorDataItem<T> : NetworkedSensorDataItem
    {
        public T Value { get; private set; }
	}
}
