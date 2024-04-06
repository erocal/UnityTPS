using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{

    #region -- �귽�ѦҰ� --

    [Header("Text")]
    [SerializeField, Tooltip("FPS")] private Text Text_FPS;
    [SerializeField, Tooltip("ms")] private Text Text_ms;

    #endregion

    #region -- �ܼưѦҰ� --

    float deltaTime = 0.0f;
    float fps = 0.0f;
    float msec = 0.0f;

    #endregion

    #region -- ��l��/�B�@ --

    void Awake()
    {
        // �аO�����󤣷|�b���������ɳQ����
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        Text_FPS.text = $"{fps:0} fps";
        Text_ms.text = $"{msec:0.0} ms";
    }

    #endregion
}
