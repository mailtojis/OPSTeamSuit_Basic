
public class IOS
{
    public SensordataIOS[] sensorData { get; set; }
    public string optionalNotes { get; set; }
    public GpsdataIOS[] gpsData { get; set; }
    public BeacondataIOS[] beaconData { get; set; }
    public RecordinginfoIos recordingInfo { get; set; }
}

public class RecordinginfoIos
{
    public float recordingStartTime { get; set; }
    public string os { get; set; }
    public string manufacturer { get; set; }
    public float recordingEndTime { get; set; }
    public string osVersion { get; set; }
    public float recordingDuration { get; set; }
    public string recorderVersion { get; set; }
    public string[] uuids { get; set; }
    public string deviceModel { get; set; }
}

public class SensordataIOS
{
    public float[] quaternion { get; set; }
    public object[] magneticField { get; set; }
    public float[] acceleration { get; set; }
    public string dataType { get; set; }
    public float[] rotationRate { get; set; }
    public float timestamp { get; set; }
    public float[] altitudeData { get; set; }
}

public class GpsdataIOS
{
    public float timestamp { get; set; }
    public float latitude { get; set; }
    public float accuracyInMeters { get; set; }
    public float longitude { get; set; }
}

public class BeacondataIOS
{
    public int minor { get; set; }
    public int rssi { get; set; }
    public int major { get; set; }
    public float timestamp { get; set; }
    public string uuid { get; set; }
}
