using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Organism : MonoBehaviour
{

    #region -- 資源參考區 --

    [SerializeField, Tooltip("玩家")] private GameObject player;
    [SerializeField, Tooltip("登入時展示的玩家")] private GameObject loginPlayer;
    [SerializeField, Tooltip("暴徒")] private GameObject mutant;
    [SerializeField, Tooltip("瘟疫醫生")] private GameObject plagueDoctor;

    [Header("敵人"), Tooltip("地圖上預先生成的全部敵人")]
    public List<GameObject> fullEnemyList = new List<GameObject>();
    [Header("區域敵人")]
    [Tooltip("StartArea區域預先生成的敵人")]
    public List<GameObject> StartAreaEnemyList = new List<GameObject>();
    [Tooltip("PlainArea區域預先生成的敵人")]
    public List<GameObject> PlainAreaEnemyList = new List<GameObject>();
    [Tooltip("PlagueDoctorArea區域預先生成的敵人")]
    public List<GameObject> PlagueDoctorAreaEnemyList = new List<GameObject>();
    [Tooltip("MutantArea區域預先生成的敵人")]
    public List<GameObject> MutantAreaEnemyList = new List<GameObject>();

    #endregion

    #region -- 變數參考區 --

    #region -- Player --

    public GameObject LoginPlayer
    {
        get
        {
            return loginPlayer;
        }
    }

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

    #region -- Boss --

    public readonly struct BossDataStruct
    {

        private readonly GameObject _bossObject;
        private readonly int _instanceID;
        private readonly Health _healthComponent;
        private readonly AIController _aiController;
        private readonly MonoBehaviour _fighter;
        private readonly MonoBehaviour _mover;
        private readonly NavMeshAgent _navAgent;
        private readonly Animator _animator;
        private readonly AudioSource _audio;

        public BossDataStruct(GameObject boss, int id, Health health,
                              AIController aiController, MonoBehaviour fighter, MonoBehaviour mover,
                              NavMeshAgent navAgent, Animator animator, AudioSource audio)
        {
            _bossObject = boss;
            _instanceID = id;
            _healthComponent = health;
            _aiController = aiController;
            _fighter = fighter;
            _mover = mover;
            _navAgent = navAgent;
            _animator = animator;
            _audio = audio;
        }

        // 只讀屬性
        public GameObject Boss { get { return _bossObject; } }
        public int InstanceID { get { return _instanceID; } }
        public Health BossHealth { get { return _healthComponent; } }
        public AIController BossAIController { get { return _aiController; } }
        public MonoBehaviour BossFighter { get { return _fighter; } }
        public MonoBehaviour BossMover { get { return _mover; } }
        public NavMeshAgent BossNavMeshAgent { get { return _navAgent; } }
        public Animator BossAnimator { get { return _animator; } }
        public AudioSource BossAudioSource { get { return _audio; } }

    }

    #endregion

    #region -- Mutant --

    private BossDataStruct mutantData;
    public BossDataStruct MutantData
    {
        get
        {
            return mutantData;
        }
    }

    #endregion

    #region -- PlagueDoctor --

    private BossDataStruct plagueDoctorData;
    public BossDataStruct PlagueDoctorData
    {
        get
        {
            return plagueDoctorData;
        }
    }

    #endregion

    #endregion

    #region -- 初始化/運作 --


    private void Awake()
    {

        SetPlayerData();
        SetMutantData();
        SetPlagueDoctorData();

        player.SetActive(false);

    }

    #endregion

    #region -- 方法參考區 --

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

        SetBossData<MutantAIController, MutantFighter, MutantMover>(mutant, ref mutantData);

    }

    /// <summary>
    /// 設置Boss : PlagueDoctor資料
    /// </summary>
    private void SetPlagueDoctorData()
    {

        if (plagueDoctor == null)
        {
            Log.Error("PlagueDoctor物件為空");
            return;
        }

        SetBossData<PlagueDoctorAIController, PlagueDoctorFighter, PlagueDoctorMover>(plagueDoctor, ref plagueDoctorData);

    }

    /// <summary>
    /// 設置Boss資料
    /// </summary>
    private void SetBossData<TController, TFighter, TMover>(GameObject boss, ref BossDataStruct bossData)
        where TController : AIController
        where TFighter : MonoBehaviour
        where TMover : MonoBehaviour
    {

        if (boss == null)
        {
            Log.Error($"{typeof(TController).Name} 物件為空");
            return;
        }

        bossData = new BossDataStruct(
            boss,
            boss.GetInstanceID(),
            boss.GetComponent<Health>(),
            boss.GetComponent<TController>(),
            boss.GetComponent<TFighter>(),
            boss.GetComponent<TMover>(),
            boss.GetComponent<NavMeshAgent>(),
            boss.GetComponent<Animator>(),
            boss.GetComponent<AudioSource>()
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
