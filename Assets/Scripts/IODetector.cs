using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using B83.MathHelpers;
using TMPro;
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine.Android;
#endif
using UnityEngine.Networking;


[Serializable]
public class SunriseSunsetResults {
    public string sunrise;
    public string sunset;
    public string solar_noon;
    public string day_length;
    public string civil_twilight_begin;
    public string civil_twilight_end;
    public string nautical_twilight_begin;
    public string nautical_twilight_end;
    public string astronomical_twilight_begin;
    public string astronomical_twilight_end;
}

[Serializable]
public class SunriseSunsetResponse {
    public SunriseSunsetResults results;
    public string status;
}

public class IODetector : MonoBehaviour
{
    public enum ResultValue 
    {
        Unknown,
        Indoor,
        Outdoor
    }

    struct DetectionResult {

        public DetectionResult(ResultValue result = ResultValue.Unknown, float confidence = 1.0f) {
            this.Result = result;
            this.Value = 0;
            this.Confidence = confidence;
        }

        public ResultValue Result { get; set; }
        public float Confidence {get; set; }
        public float Value { get; set; }
    };

    private const float OutdoorLightThreshold = 2000;

    private LightSensor m_lightSensor;
    private MagneticFieldSensor m_magnetFieldSensor;
    private ProximitySensor m_proximitySensor;

    private AxisControl m_lightSensorControl;
    private Vector3Control m_magneticFieldSensorControl;
    private AxisControl m_proximitySensorControl;

    private DateTime m_lastDaytimeCheckDate;
    private DateTime m_sunrise;
    private DateTime m_sunset;

    private Thread m_lightSensorSamplingThread;

    private readonly Queue<Vector3> m_pastMagneticFieldValues = new Queue<Vector3>();
    private const int MaxMagneticValues = 60;
    private const float OutdoorMagnetismThreshold = 25;
    private Vector3 m_magneticFieldSum = new Vector3();

    private double m_latitude;
    private double m_longitude;

    [SerializeField] private TMP_Text m_magneticFieldResultText;
    [SerializeField] private TMP_Text m_lightResultText;

#if UNITY_ANDROID && !UNITY_EDITOR
    internal void PermissionCallbacks_PermissionGranted(string permissionName) {
         StartCoroutine(EnableLocationServices());
    }
#endif

    // Start is called before the first frame update
    void Start()
    {

        m_lightSensor       = LightSensor.current;
        m_magnetFieldSensor = MagneticFieldSensor.current;
        m_proximitySensor   = ProximitySensor.current;
 
        m_magneticFieldSensorControl    = (m_magnetFieldSensor != null) ? m_magnetFieldSensor.magneticField : new Vector3Control();

        m_lightSensorControl            = (m_lightSensor != null) ? m_lightSensor.lightLevel : new AxisControl();

        m_proximitySensorControl        = (m_proximitySensor != null) ? m_proximitySensor.distance : new AxisControl();

        InputSystem.EnableDevice(m_lightSensor);
        InputSystem.EnableDevice(m_magnetFieldSensor);
        InputSystem.EnableDevice(m_proximitySensor);

        if (m_lightSensor != null)
            m_lightSensor.samplingFrequency         = 400;

        if (m_magnetFieldSensor != null)
            m_magnetFieldSensor.samplingFrequency   = 10;

        if (m_proximitySensor != null)
            m_proximitySensor.samplingFrequency     = 2;

        //DetermineSunriseAndSet();

        //m_lightSensorSamplingThread = new Thread(new ThreadStart(LightSensorSampling));

        //m_magneticFieldResultText = GameObject.Find("MagneticFieldDetectionResult");
        //m_lightResultText = GameObject.Find("LightDetectionResult");

        Debug.Log("start location services in coroutine...");

#if UNITY_ANDROID && !UNITY_EDITOR
        var callbacks = new PermissionCallbacks();
        callbacks.PermissionGranted += PermissionCallbacks_PermissionGranted;

        if (!Permission.HasUserAuthorizedPermission(Permission.CoarseLocation))
            Permission.RequestUserPermission(Permission.CoarseLocation);
        else
#endif
            StartCoroutine(EnableLocationServices());

        
    }

    void FixedUpdate() {
        var currentMagneticFieldValue    = m_magneticFieldSensorControl.ReadValue();

        if (m_pastMagneticFieldValues.Count+1 >  MaxMagneticValues) {
            var dequeued = m_pastMagneticFieldValues.Dequeue();
            m_magneticFieldSum -= dequeued;

        }
            
        m_pastMagneticFieldValues.Enqueue(currentMagneticFieldValue);
        m_magneticFieldSum += currentMagneticFieldValue;

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("light: " + currentLight);
        //Debug.Log("proximity: " +currentProximity);

        var lightResult = LightDetection();
        var magneticFieldResult = MagnetismDetection();

        m_magneticFieldResultText.text = Enum.GetName(typeof(ResultValue), magneticFieldResult.Result) + " (" + magneticFieldResult.Value.ToString() + ")";
        m_lightResultText.text = Enum.GetName(typeof(ResultValue), lightResult.Result) + " (" + lightResult.Value.ToString() + ")";

    }

    DetectionResult LightDetection() {
        var result              = new DetectionResult();
        var currentLight        = m_lightSensorControl.ReadValue();
        result.Value            = currentLight;
        var currentProximity    = m_proximitySensorControl.ReadValue();
        if (currentProximity > 0) {
            if (currentLight < OutdoorLightThreshold) {
                var currentTime = DateTime.UtcNow;
                
                // if sun is up
                //if (m_lastDaytimeCheckDate.Day != currentTime.Day || m_lastDaytimeCheckDate.Month != currentTime.Month)
                    //StartCoroutine(DetermineSunriseAndSet());
                    //DetermineSunriseAndSet();
                if (currentTime > m_sunrise && currentTime < m_sunset) {
                    result.Result = ResultValue.Indoor;
                }
                else 
                {
                    // use FFT and check for frequency pattern caused by indoor lights on electrical circuits
                }

            }
            else {
                result.Result = ResultValue.Outdoor;
            }
        }

        return result;
    }

    DetectionResult MagnetismDetection() {
        var result      = new DetectionResult();

        var variance    = ComputeMagnetismVariance();
        // TODO deal with movement detection
        result.Value    = variance;
        result.Result   = (variance <= OutdoorMagnetismThreshold) ? ResultValue.Unknown : ResultValue.Indoor;

        return result;
    }

    float ComputeMagnetismVariance() {
        float variance = 0.0f; 

        if (m_pastMagneticFieldValues.Count >= MaxMagneticValues) {
            var mean = m_magneticFieldSum / m_pastMagneticFieldValues.Count;

            foreach(var value in m_pastMagneticFieldValues) {
                var diff = value - mean;
                variance += diff.sqrMagnitude;
            }
            variance /= m_pastMagneticFieldValues.Count;
        }
        return variance;
    }

    IEnumerator  EnableLocationServices() {
       
        // Check if the user has location service enabled.

        yield return new WaitForSeconds(3);

        Debug.Log("location enabled by user: " + Input.location.isEnabledByUser);

        if (!Input.location.isEnabledByUser)
            yield break;


        Debug.Log("starting location services...");
        // Starts the location service.
        Input.location.Start(500f, 500f);

        // Waits until the location service initializes
        int maxWait = 60;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            Debug.Log("waiting for initialization");
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        else if (Input.location.status == LocationServiceStatus.Stopped)
        {
            Debug.Log("Stopped prematurely");
        }
        else {
            Debug.Log("Status: " + Input.location.status);
            if (Input.location.status == LocationServiceStatus.Running) {
                Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude);
                m_latitude  = Input.location.lastData.latitude;
                m_longitude = Input.location.lastData.longitude;
                DetermineSunriseAndSet();
            }
        }

        Debug.Log("stopping location services");
            // Stops the location service if there is no need to query location updates continuously.
        Input.location.Stop();
        
    }

    void DetermineSunriseAndSet() { 
   
        
        //Debug.Log("timezone: " + TimeZoneInfo.Local.DisplayName);
        var now = DateTime.Now;

        Sunriset.SunriseSunset(now, m_latitude, m_longitude, out DateTime sunrise, out DateTime sunset);

        m_sunrise = sunrise;
        m_sunset = sunset;
        
        Debug.Log("sunrise: " + sunrise);
        Debug.Log("sunset: " + sunset);
        Debug.Log("now: " + DateTime.UtcNow);

        /*UnityWebRequest serviceRequest = UnityWebRequest.Get("https://api.sunrise-sunset.org/json?lat=" + m_latitude + "&lng=" + m_longitude);
        yield return serviceRequest.SendWebRequest();

        if (serviceRequest.result != UnityWebRequest.Result.Success) {
            Debug.Log(serviceRequest.error);
        }
        else {
            // Show results as text
            Debug.Log(serviceRequest.downloadHandler.text);
            try {
                var response = JsonUtility.FromJson<SunriseSunsetResponse>(serviceRequest.downloadHandler.text);

                if (response.status == "OK") {
                    var sunrise = DateTime.Parse(response.results.sunrise);
                    var sunset = DateTime.Parse(response.results.sunset);

                    m_sunrise = sunrise;
                    m_sunset = sunset;
                    
                    var currentTime =  DateTime.UtcNow;
                    var isDay = (currentTime > m_sunrise && currentTime < m_sunset);

                    Debug.Log("sunrise: " + sunrise);
                    Debug.Log("sunset: " + sunset);
                    Debug.Log("now: " + currentTime);
                    Debug.Log("is day: " + isDay);
                }
            } catch (Exception e) {
                Debug.Log(e);
            }
        }*/


        m_lastDaytimeCheckDate = DateTime.Now;
    }

    void OnApplicationQuit() {
        InputSystem.DisableDevice(m_lightSensor);
        InputSystem.DisableDevice(m_magnetFieldSensor);
        Input.location.Stop();
    }
}
