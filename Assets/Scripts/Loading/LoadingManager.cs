using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{

    #region -- �귽�ѦҰ� --

    [Header("���ݹ�"), Tooltip("���������ɪ����ݵe��")]
    [SerializeField] Image loadingImage;

    #endregion

    #region -- �ѼưѦҰ� --

    InputController input;

    int sceneIndex = -1;

    #endregion

    #region -- ��l��/�B�@ --

    void Awake()
    {
        input = GameManagerSingleton.Instance.InputController;

        // �קK���������}�a
        DontDestroyOnLoad(input);
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {

        // �p�G�w�b�����������A�BLoading���٦b�Ұ�
        if (SceneManager.GetActiveScene().buildIndex != sceneIndex && loadingImage.IsActive())
        {
            HideLoadingImage();
        }

    }

    #endregion

    #region -- ��k�ѦҰ� --

    private void HideLoadingImage()
    {
        if (loadingImage != null)
        {
            loadingImage.gameObject.SetActive(false);
        }
    }

    #region -- onClick --

    /// <summary>
    /// Button-Start �[���U�@�i�a��
    /// </summary>
    public async void onStartGame()
    {
        // �[��Game
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            input.CursorStateLocked();
            ShowLoadingImage(0);

            await AddrssableAsync.LoadSceneAsync("samplescene", LoadSceneMode.Single);

        }
    }

    #endregion

    /// <summary>
    /// �b���������ɡA�N���ݵe����ܥX��
    /// </summary>
    /// <param name="sceneIndex">�����e����������</param>
    public void ShowLoadingImage(int sceneIndex)
    {
        if (loadingImage != null)
        {
            loadingImage.gameObject.SetActive(true);

            // �N��l��������
            GameObject.Find("UI").SetActive(false);
        }

        this.sceneIndex = sceneIndex;
    }

    #endregion
}
