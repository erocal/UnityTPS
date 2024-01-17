using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [Header("���ݹ�"), Tooltip("���������ɪ����ݵe��")]
    [SerializeField] Image loadingImage;

    #region -- �ѼưѦҰ� --

    InputController input;

    int sceneIndex = -1;

    #endregion

    void Awake()
    {
        input = GameManagerSingleton.Instance.InputController;

        // �קK���������}�a
        DontDestroyOnLoad(input);
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    private async void Update()
    {

        // �p�G���U�ƹ�����A�B�b�}�l�e���A�N�[��Game
        if (input.GetClick() && SceneManager.GetActiveScene().buildIndex == 0)
        {
            input.CursorStateLocked();
            ShowLoadingImage(0);

            await AddrssableAsync.LoadSceneAsync("samplescene", LoadSceneMode.Single);

        }

        // �p�G�w�b�����������A�BLoading���٦b�Ұ�
        if (SceneManager.GetActiveScene().buildIndex != sceneIndex && loadingImage.IsActive())
        {
            HideLoadingImage();
        }
    }

    #region -- ��k�ѦҰ� --

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

    private void HideLoadingImage()
    {
        if (loadingImage != null)
        {
            loadingImage.gameObject.SetActive(false);
        }
    }

    #endregion
}
