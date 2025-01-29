using UnityEngine;
using UnityEngine.AI;

public class Organism : MonoBehaviour
{

    #region -- 資源參考區 --

    [SerializeField, Tooltip("玩家")] private GameObject player;
    [SerializeField, Tooltip("暴徒")] private GameObject mutant;
    [SerializeField, Tooltip("瘟疫醫生")] private GameObject plagueDoctor;

    #endregion

    #region -- 變數參考區 --

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

    #region -- Player --

    private PlayerDataStruct playerData;
    public PlayerDataStruct PlayerData
    {
        get
        {
            return playerData;
        }
    }

    public readonly struct PlayerDataStruct
    {
        private readonly GameObject _player;
        private readonly int _instanceID;
        private readonly PlayerController _playerController;
        private readonly Health _playerHealth;
        private readonly WeaponManager _playerWeaponManager;
        private readonly CharacterController _playerCharacterController;
        private readonly Animator _playerAnimator;
        private readonly AudioSource _playerAudioSource;

        // 構造函數，確保所有字段在初始化時被賦值
        public PlayerDataStruct(
            GameObject player,
            int instanceID,
            PlayerController playerController,
            Health playerHealth,
            WeaponManager playerWeaponManager,
            CharacterController playerCharacterController,
            Animator playerAnimator,
            AudioSource playerAudioSource)
        {

            _player = player;
            _instanceID = instanceID;
            _playerController = playerController;
            _playerHealth = playerHealth;
            _playerWeaponManager = playerWeaponManager;
            _playerCharacterController = playerCharacterController;
            _playerAnimator = playerAnimator;
            _playerAudioSource = playerAudioSource;

        }

        // 只讀屬性
        public GameObject Player { get { return _player; } }
        public int InstanceID { get { return _instanceID; } }
        public PlayerController PlayerController { get { return _playerController; } }
        public Health PlayerHealth { get { return _playerHealth; } }
        public WeaponManager PlayerWeaponManager { get { return _playerWeaponManager; } }
        public CharacterController PlayerCharacterController { get { return _playerCharacterController; } }
        public Animator PlayerAnimator { get { return _playerAnimator; } }
        public AudioSource PlayerAudioSource { get { return _playerAudioSource; } }

    }

    #endregion

    #region -- Mutant --

    private MutantDataStruct mutantData;
    public MutantDataStruct MutantData
    {
        get
        {
            return mutantData;
        }
    }

    public readonly struct MutantDataStruct
    {
        private readonly GameObject _mutant;
        private readonly int _instanceID;
        private readonly Health _mutantHealth;
        private readonly MutantAIController _mutantAIController;
        private readonly MutantFighter _mutantFighter;
        private readonly MutantMover _mutantMover;
        private readonly NavMeshAgent _mutantNavMeshAgent;
        private readonly Animator _mutantAnimator;
        private readonly AudioSource _mutantAudioSource;

        // 構造函數，確保所有字段在初始化時被賦值
        public MutantDataStruct(
            GameObject mutant,
            int instanceID,
            Health mutantHealth,
            MutantAIController mutantAIController,
            MutantFighter mutantFighter,
            MutantMover mutantMover,
            NavMeshAgent mutantNavMeshAgent,
            Animator mutantAnimator,
            AudioSource mutantAudioSource)
        {

            _mutant = mutant;
            _instanceID = instanceID;
            _mutantHealth = mutantHealth;
            _mutantAIController = mutantAIController;
            _mutantFighter = mutantFighter;
            _mutantMover = mutantMover;
            _mutantNavMeshAgent = mutantNavMeshAgent;
            _mutantAnimator = mutantAnimator;
            _mutantAudioSource = mutantAudioSource;

        }

        // 只讀屬性
        public GameObject Mutant { get { return _mutant; } }
        public int InstanceID { get { return _instanceID; } }
        public Health MutantHealth { get { return _mutantHealth; } }
        public MutantAIController MutantAIController { get { return _mutantAIController; } }
        public MutantFighter MutantFighter { get { return _mutantFighter; } }
        public MutantMover MutantMover { get { return _mutantMover; } }
        public NavMeshAgent MutantNavMeshAgent { get { return _mutantNavMeshAgent; } }
        public Animator MutantAnimator { get { return _mutantAnimator; } }
        public AudioSource MutantAudioSource { get { return _mutantAudioSource; } }

    }

    #endregion

    #endregion

    #region -- 初始化/運作 --

    // 防止外部實例化該類
    private Organism()
    {
    }

    private void Awake()
    {

        GetInstance();
        SetPlayerData();
        SetMutantData();

    }

    private void OnDestroy()
    {
        _instance = null;
    }

    #endregion

    #region -- 方法參考區 --

    #region -- 單例模式 --

    /// <summary>
    /// 獲取唯一實例
    /// </summary>
    private void GetInstance()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    #endregion

    /// <summary>
    /// 設置玩家資料
    /// </summary>
    private void SetPlayerData()
    {

        if(player == null)
        {
            Log.Error("玩家物件為空");
            return;
        }

        playerData = new PlayerDataStruct(
            player,
            player.GetInstanceID(),
            player.GetComponent<PlayerController>(),
            player.GetComponent<Health>(),
            player.GetComponent<WeaponManager>(),
            player.GetComponent<CharacterController>(),
            player.GetComponent<Animator>(),
            player.GetComponent<AudioSource>()
            );

    }

    /// <summary>
    /// 設置Boss : Mutant資料
    /// </summary>
    private void SetMutantData()
    {

        if (mutant == null)
        {
            Log.Error("Mutant物件為空");
            return;
        }

        mutantData = new MutantDataStruct(
            mutant,
            mutant.GetInstanceID(),
            mutant.GetComponent<Health>(),
            mutant.GetComponent<MutantAIController>(),
            mutant.GetComponent<MutantFighter>(),
            mutant.GetComponent<MutantMover>(),
            mutant.GetComponent<NavMeshAgent>(),
            mutant.GetComponent<Animator>(),
            mutant.GetComponent<AudioSource>()
            );

    }

    #region -- Get方法 --

    /// <summary>
    /// 取得暴徒Boss
    /// </summary>
    public GameObject GetMutant()
    {
        if (mutant == null) Log.Error("暴徒Boss物件為空");

        return mutant;
    }

    /// <summary>
    /// 取得瘟疫醫生Boss
    /// </summary>
    public GameObject GetPlagueDoctor()
    {
        if (plagueDoctor == null) Log.Error("瘟疫醫生Boss物件為空");

        return plagueDoctor;
    }

    #endregion

    #endregion

}
