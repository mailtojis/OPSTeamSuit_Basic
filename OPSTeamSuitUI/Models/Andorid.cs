public class Android
{
    public Beacondata[] beaconData { get; set; }
    public Gpsdata[] gpsData { get; set; }
    public string optionalNotes { get; set; }
    public Recordinginfo recordingInfo { get; set; }
    public Sensordata[] sensorData { get; set; }
    public int uniqueBeacons { get; set; }
    public Wifidata[] wifiData { get; set; }
}

public class Recordinginfo
{
    public string deviceModel { get; set; }
    public string manufacturer { get; set; }
    public string os { get; set; }
    public string osVersion { get; set; }
    public string recorderAppVersion { get; set; }
    public int recordingDuration { get; set; }
    public long recordingEndTime { get; set; }
    public long recordingStartTime { get; set; }
}

public class Beacondata
{
    public int batteryLevel { get; set; }
    public int[] bytes { get; set; }
    public int firmware { get; set; }
    public string hexStringOfRawData { get; set; }
    public int major { get; set; }
    public int minor { get; set; }
    public string name { get; set; }
    public string powerLevel { get; set; }
    public int rssi { get; set; }
    public float timestamp { get; set; }
    public string uuid { get; set; }
}

public class Gpsdata
{
    public float accuracyInMeters { get; set; }
    public float altitude { get; set; }
    public float bearings { get; set; }
    public float latitude { get; set; }
    public float longitude { get; set; }
    public float speed { get; set; }
    public int speedAccuracyMetersPerSecond { get; set; }
    public float timestamp { get; set; }
    public int verticalAccuracyInMeters { get; set; }
}

public class Sensordata
{
    public float[] acceleration { get; set; }
    public string dataType { get; set; }
    public float[] magneticField { get; set; }
    public float[] quaternion { get; set; }
    public float[] rotationRate { get; set; }
    public float timestamp { get; set; }
}

public class Wifidata
{
    public string androidVersion { get; set; }
    public string macId { get; set; }
    public int rssi { get; set; }
    public float timestamp { get; set; }
}
