using UnityEngine;
using UnityEngine.UI;

public class LogMenu : MonoBehaviour
{
    [SerializeField]
    private Text m_textUI = null;

    private void Awake()
    {
        Application.logMessageReceived += OnLogMessage;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived += OnLogMessage;
    }

    private void OnLogMessage(string i_logText, string i_stackTrace, LogType i_type)
    {
        if (string.IsNullOrEmpty(i_logText))
        {
            return;
        }

        i_logText = string.Format("<color=red>{0}</color>", i_logText);

        m_textUI.text += i_logText + System.Environment.NewLine;
    }
} // class LogMenu