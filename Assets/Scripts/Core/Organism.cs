using UnityEngine;

public class Organism : MonoBehaviour
{

    #region -- �귽�ѦҰ� --

    [SerializeField, Tooltip("���a")] private GameObject player;
    [SerializeField, Tooltip("�ɮ{")] private GameObject mutant;
    [SerializeField, Tooltip("�E�����")] private GameObject plagueDoctor;

    #endregion

    #region -- �ܼưѦҰ� --

    private static Organism _instance;

    public static Organism Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<Organism>();
            }

            return _instance;
        }
        private set { }
    }

    #endregion

    #region -- ��l��/�B�@ --

    // ����~����ҤƸ���
    private Organism()
    {
    }

    private void Awake()
    {
        GetInstance();
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    #endregion

    #region -- ��k�ѦҰ� --

    #region -- ��ҼҦ� --

    /// <summary>
    /// ����ߤ@���
    /// </summary>
    private void GetInstance()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    #endregion

    #region -- Get��k --

    /// <summary>
    /// ���o���a
    /// </summary>
    public GameObject GetPlayer()
    {
        if (player == null) Log.Error("���a���󬰪�");

        return player;
    }

    /// <summary>
    /// ���o�ɮ{Boss
    /// </summary>
    public GameObject GetMutant()
    {
        if (mutant == null) Log.Error("�ɮ{Boss���󬰪�");

        return mutant;
    }

    /// <summary>
    /// ���o�E�����Boss
    /// </summary>
    public GameObject GetPlagueDoctor()
    {
        if (plagueDoctor == null) Log.Error("�E�����Boss���󬰪�");

        return plagueDoctor;
    }

    #endregion

    #endregion

}
