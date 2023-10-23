using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PluginTest : MonoBehaviour
{
#if UNITY_ANDROID

    [SerializeField] private TMP_Text label;
    [SerializeField] private TMP_Dropdown logsDropdown;
    [SerializeField] private Text logText;
    [SerializeField] private Button clearButton;
    private const string PackageName = "com.barra.mylibrary";
    private const string ClassName = PackageName + ".Logger";
    private AndroidJavaClass _javaClass;
    private AndroidJavaObject _pluginInstance;

    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            _javaClass = new AndroidJavaClass(ClassName);
            _pluginInstance = _javaClass.CallStatic<AndroidJavaObject>("getInstance");
        }
    }

    public void RunPlugin()
    {
        Debug.Log("RunPlugin");
        if (Application.platform == RuntimePlatform.Android)
        {
            label.text = _pluginInstance.Call<string>("getLogtag");
        }
    }

    public void GetLogs()
    {
        logsDropdown.ClearOptions();
        if (Application.platform != RuntimePlatform.Android) return;
        
        AndroidJavaObject listObject = _javaClass.Call<AndroidJavaObject>("getLogs");
        List<string> savedLogs = AndroidJNIHelper.ConvertFromJNIArray<List<string>>(listObject.GetRawObject());

        logsDropdown.AddOptions(savedLogs);
        logsDropdown.RefreshShownValue();
    }

    public void ChangeText(int key)
    {
        if (Application.platform != RuntimePlatform.Android) return;

        AndroidJavaObject listObject = _javaClass.Call<AndroidJavaObject>("getLogs");
        List<string> savedLogs = AndroidJNIHelper.ConvertFromJNIArray<List<string>>(listObject.GetRawObject());
        logText.text = savedLogs[key];
    }
    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
        clearButton.onClick.AddListener(ClearLog);
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
        clearButton.onClick.RemoveListener(ClearLog);
    }


    private void HandleLog(string logText, string stackTrace, LogType logType)  
    {
        if (Application.platform != RuntimePlatform.Android) return;

        _javaClass.Call("saveLogs", logText);
    }

    private void ClearLog()
    {
        if (Application.platform != RuntimePlatform.Android) return;

        logText.text = "";

        _javaClass.Call("deleteLogs");
    }
#endif
}