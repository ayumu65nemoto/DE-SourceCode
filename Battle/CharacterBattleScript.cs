using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBattleScript : MonoBehaviour
{
    //�@�퓬���̃L�����N�^�[�̏��
    public enum BattleState
    {
        Idle,
        DirectAttack,
        MagicAttack,
        Heal,
        UseHPRecoveryItem,
        UseMPRecoveryItem,
        UseMotRecoveryItem,
        UsePoisonRecoveryItem,
        UseSleepRecoveryItem,
        UseParalysisRecoveryItem,
        UseConfusionRecoveryItem,
        UseDepressionRecoveryItem,
        IncreaseAttackPowerMagic,
        IncreaseDefensePowerMagic,
        IncreaseAttackAndDecreaseDefense,
        IncreaseDefenseAndDecreaseAttack,
        PoisonRecoveryMagic,
        SleepRecoveryMagic,
        ParalysisRecoveryMagic,
        ConfusionRecoveryMagic,
        DepressionRecoveryMagic,
        Damage,
        Guard,
        Dead,
    }

    private BattleManager _battleManager;
    private BattleStatusScript _battleStatusScript;
    [SerializeField]
    private BattleStatusScript _enemyBattleStatusScript;
    [SerializeField]
    private CharacterStatus _characterStatus = null;
    private Animator _animator;
    private BattleState _battleState;

    //�@���̃X�e�[�^�X����R�s�[

    //�@HP
    private int _hp = 0;
    //�@MP
    private int _mp = 0;
    //�@���C
    private int _mot = 0;

    //�@�⏕�̗�
    private int _auxiliaryPower = 0;
    //�@�⏕�̖h���
    private int _auxiliaryDefense = 0;
    //�@�ŏ�Ԃ�
    private bool _isPoison;
    //�@������Ԃ�
    private bool _isSleep;
    //�@��჏�Ԃ�
    private bool _isParalysis;
    //�@������Ԃ�
    private bool _isConfusion;
    //�@�T��Ԃ�
    private bool _isDepression;

    //�@���I�������X�L��
    private Skill _currentSkill;
    //�@���̃^�[�Q�b�g
    private GameObject _currentTarget;
    //�@���g�p�����A�C�e��
    private Item _currentItem;
    //�@�^�[�Q�b�g��CharacterBattleScript
    private CharacterBattleScript _targetCharacterBattleScript;
    //�@�^�[�Q�b�g��CharacterStatus
    private CharacterStatus _targetCharacterStatus;
    //�@�U���I����̃A�j���[�V�������I���������ǂ���
    private bool _isDoneAnimation;
    //�@�L�����N�^�[������ł��邩�ǂ���
    private bool _isDead;

    //�@�U���̓A�b�v���Ă��邩�ǂ���
    private bool _isIncreasePower;
    //�@�U���̓A�b�v���Ă���|�C���g
    private int _increasePowerPoint;
    //�@�U���̓A�b�v���Ă���^�[��
    private int _numOfTurnsIncreasePower = 3;
    //�@�U���̓A�b�v���Ă���̃^�[��
    private int _numOfTurnsSinceIncreasePower = 0;
    //�@����܂ł̍U���̓A�b�v�������l�̃��X�g
    private List<int> _increasePowerPointList = new List<int>();
    //�@����܂ł̍U���̓A�b�v���Ă���^�[���̃��X�g
    private List<int> _numOfTurnsIncreasePowerList = new List<int>();
    //�@�U���̓_�E�����Ă��邩�ǂ���
    private bool _isDecreasePower;
    //�@�U���̓_�E�����Ă���|�C���g
    private int _decreasePowerPoint;
    //�@�U���̓_�E�����Ă���^�[��
    private int _numOfTurnsDecreasePower = 3;
    //�@�U���̓_�E�����Ă���̃^�[��
    private int _numOfTurnsSinceDecreasePower = 0;
    //�@����܂ł̍U���̓_�E���������l�̃��X�g
    private List<int> _decreasePowerPointList = new List<int>();
    //�@����܂ł̍U���̓_�E�����Ă���^�[���̃��X�g
    private List<int> _numOfTurnsDecreasePowerList = new List<int>();
    //�@�h��̓A�b�v���Ă��邩�ǂ���
    private bool _isIncreaseDefense;
    //�@�h��̓A�b�v���Ă���|�C���g
    private int _increaseDefensePoint;
    //�@�h��̓A�b�v���Ă���^�[��
    private int _numOfTurnsIncreaseDefense = 3;
    //�@�h��̓A�b�v���Ă���̃^�[��
    private int _numOfTurnsSinceIncreaseDefense = 0;
    //�@����܂ł̖h��̓A�b�v�������l�̃��X�g
    private List<int> _increaseDefensePointList = new List<int>();
    //�@����܂ł̖h��̓A�b�v���Ă���^�[���̃��X�g
    private List<int> _numOfTurnsIncreaseDefenseList = new List<int>();
    //�@�h��̓_�E�����Ă��邩�ǂ���
    private bool _isDecreaseDefense;
    //�@�h��̓_�E�����Ă���|�C���g
    private int _decreaseDefensePoint;
    //�@�h��̓_�E�����Ă���^�[��
    private int _numOfTurnsDecreaseDefense = 3;
    //�@�h��̓_�E�����Ă���̃^�[��
    private int _numOfTurnsSinceDecreaseDefense = 0;
    //�@����܂ł̖h��̓_�E���������l�̃��X�g
    private List<int> _decreaseDefensePointList = new List<int>();
    //�@����܂ł̖h��̓_�E�����Ă���^�[���̃��X�g
    private List<int> _numOfTurnsDecreaseDefenseList = new List<int>();
    //�@�_���[�WUP
    private float _damageUpRate = 1;

    //�@�ł̃_���[�W���󂯂����ǂ���
    private bool _poisonDamage = false;
    //�@�ł̃^�[��
    private int _poisonTurn = 3;
    //�@��Ⴢ̃^�[��
    private int _paralysisTurn = 3;
    //�@�T�ōU���͂�������������
    private bool _isDepressionAttack = false;
    //�@�T�Ŗh��͂�������������
    private bool _isDepressionDefense = false;

    //�@���ʃ|�C���g�\���X�N���v�g
    private EffectNumericalDisplayScript _effectNumericalDisplayScript;

    public CharacterStatus CharacterStatus { get => _characterStatus; set => _characterStatus = value; }
    public int Hp
    {
        get { return _hp; }
        set 
        { 
            _hp = Mathf.Clamp(value, 0, CharacterStatus.MaxHp);
            if (_hp <= 0)
            {
                Dead();
            }
        }
    }
    public int Mp
    {
        get { return _mp; }
        set { _mp = Mathf.Clamp(value, 0, CharacterStatus.MaxMp); }
    }
    public int Mot
    {
        get { return _mot; }
        set { _mot = Mathf.Clamp(value, 0, CharacterStatus.MaxMot); }
    }
    public bool IsDoneAnimation { get => _isDoneAnimation; set => _isDoneAnimation = value; }
    public int AuxiliaryPower { get => _auxiliaryPower; set => _auxiliaryPower = value; }
    public int AuxiliaryDefense { get => _auxiliaryDefense; set => _auxiliaryDefense = value; }
    public bool IsPoison { get => _isPoison; set => _isPoison = value; }
    public bool IsSleep { get => _isSleep; set => _isSleep = value; }
    public bool IsParalysis { get => _isParalysis; set => _isParalysis = value; }
    public bool IsConfusion { get => _isConfusion; set => _isConfusion = value; }
    public bool IsDepression { get => _isDepression; set => _isDepression = value; }
    public bool IsIncreasePower { get => _isIncreasePower; set => _isIncreasePower = value; }
    public bool IsIncreaseDefense { get => _isIncreaseDefense; set => _isIncreaseDefense = value; }
    public BattleState BattleState1 { get => _battleState; set => _battleState = value; }
    public bool IsDecreasePower { get => _isDecreasePower; set => _isDecreasePower = value; }
    public bool IsDecreaseDefense { get => _isDecreaseDefense; set => _isDecreaseDefense = value; }
    public float DamageUpRate { get => _damageUpRate; set => _damageUpRate = value; }
    public bool PoisonDamage1 { get => _poisonDamage; set => _poisonDamage = value; }
    public int PoisonTurn { get => _poisonTurn; set => _poisonTurn = value; }
    public int ParalysisTurn { get => _paralysisTurn; set => _paralysisTurn = value; }

    // Start is called before the first frame update
    void Start()
    {
        //_animator = GetComponent<Animator>();
        //�@���f�[�^����ݒ�
        Hp = CharacterStatus.Hp;
        Mp = CharacterStatus.Mp;
        Mot = CharacterStatus.Mot;
        IsPoison = CharacterStatus.IsPoisonStatus;
        IsSleep = CharacterStatus.IsSleepStatus;
        IsParalysis = CharacterStatus.IsParalysisStatus;
        IsConfusion = CharacterStatus.IsConfusionStatus;
        IsDepression = CharacterStatus.IsDepressionStatus;

        //�@��Ԃ̐ݒ�
        BattleState1 = BattleState.Idle;
        //�@�R���|�[�l���g�̎擾
        _battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        _battleStatusScript = GameObject.Find("BattleUI/StatusPanel").GetComponent<BattleStatusScript>();
        _effectNumericalDisplayScript = _battleManager.GetComponent<EffectNumericalDisplayScript>();
        //�@���Ɏ���ł���ꍇ�͓|��Ă����Ԃɂ���
        if (CharacterStatus.Hp <= 0)
        {
            //_animator.CrossFade("Dead", 0f, 0, 1f);
            _isDead = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�@���Ɏ���ł����牽�����Ȃ�
        if (_isDead == true)
        {
            return;
        }

        //�@�����̃^�[���łȂ���Ή������Ȃ�
        if (BattleState1 == BattleState.Idle)
        {
            return;
        }
        //�@�A�j���[�V�������I����Ă��Ȃ���Ή������Ȃ�
        if (IsDoneAnimation == false)
        {
            //�A�j���[�V������t����܂ŃI�t�ɂ��Ă����@�A�j���[�V�������ł�����r�w�C�r�A��ݒ肷��
            //return;
        }

        //�@�I�������A�j���[�V�����ɂ���ď����𕪂���
        if (BattleState1 == BattleState.DirectAttack)
        {
            ShowEffectOnTheTarget();
            DirectAttack();
            //�@�����̃^�[���������̂ŏオ�����p�����[�^�̃`�F�b�N
            CheckIncreaseAttackPower();
            CheckIncreaseDefense();
            CheckDecreaseAttackPower();
            CheckDecreaseDefense();
            DepressionEffect();
        }
        else if (BattleState1 == BattleState.MagicAttack)
        {
            ShowEffectOnTheTarget();
            MagicAttack();
            //�@�����̃^�[���������̂ŏオ�����p�����[�^�̃`�F�b�N
            CheckIncreaseAttackPower();
            CheckIncreaseDefense();
            CheckDecreaseAttackPower();
            CheckDecreaseDefense();
            DepressionEffect();
        }
        else if (BattleState1 == BattleState.Heal
          || BattleState1 == BattleState.PoisonRecoveryMagic
          || BattleState1 == BattleState.SleepRecoveryMagic
          || BattleState1 == BattleState.ParalysisRecoveryMagic
          || BattleState1 == BattleState.ConfusionRecoveryMagic
          || BattleState1 == BattleState.DepressionRecoveryMagic
          )
        {
            ShowEffectOnTheTarget();
            UseMagic();
            //�@�����̃^�[���������̂ŏオ�����p�����[�^�̃`�F�b�N
            CheckIncreaseAttackPower();
            CheckIncreaseDefense();
            CheckDecreaseAttackPower();
            CheckDecreaseDefense();
            DepressionEffect();
        }
        else if (BattleState1 == BattleState.IncreaseAttackPowerMagic)
        {
            ShowEffectOnTheTarget();
            UseMagic();
            //�@���g�̍U���͂��A�b�v�����ꍇ�̓^�[�������J�E���g���Ȃ�
            if (_currentTarget == this.gameObject)
            {
                CheckIncreaseDefense();
                CheckDecreaseAttackPower();
                CheckDecreaseDefense();
                DepressionEffect();
            }
            else
            {
                CheckIncreaseAttackPower();
                CheckIncreaseDefense();
                CheckDecreaseAttackPower();
                CheckDecreaseDefense();
                DepressionEffect();
            }
        }
        else if (BattleState1 == BattleState.IncreaseAttackAndDecreaseDefense)
        {
            ShowEffectOnTheTarget();
            UseMagic();
            //�@���g�̍U���͂��A�b�v�����ꍇ�̓^�[�������J�E���g���Ȃ�
            if (_currentTarget != this.gameObject)
            {
                CheckIncreaseDefense();
                CheckDecreaseAttackPower();
                DepressionEffect();
            }
            else
            {
                CheckIncreaseAttackPower();
                CheckIncreaseDefense();
                CheckDecreaseAttackPower();
                CheckDecreaseDefense();
                DepressionEffect();
            }
        }
        else if (BattleState1 == BattleState.IncreaseDefensePowerMagic)
        {
            ShowEffectOnTheTarget();
            UseMagic();
            //�@���g�̖h��͂��A�b�v�����ꍇ�̓^�[�������J�E���g���Ȃ�
            if (_currentTarget == this.gameObject)
            {
                CheckIncreaseAttackPower();
                CheckDecreaseAttackPower();
                CheckDecreaseDefense();
                DepressionEffect();
            }
            else
            {
                CheckIncreaseAttackPower();
                CheckIncreaseDefense();
                CheckDecreaseAttackPower();
                CheckDecreaseDefense();
                DepressionEffect();
            }
        }
        else if (BattleState1 == BattleState.IncreaseDefenseAndDecreaseAttack)
        {
            ShowEffectOnTheTarget();
            UseMagic();
            //�@���g�̖h��͂��A�b�v�����ꍇ�̓^�[�������J�E���g���Ȃ�
            if (_currentTarget == this.gameObject)
            {
                CheckIncreaseAttackPower();
                CheckDecreaseDefense();
                DepressionEffect();
            }
            else
            {
                CheckIncreaseAttackPower();
                CheckIncreaseDefense();
                CheckDecreaseAttackPower();
                CheckDecreaseDefense();
                DepressionEffect();
            }
        }
        else if (BattleState1 == BattleState.UseHPRecoveryItem
          || BattleState1 == BattleState.UseMPRecoveryItem
          || BattleState1 == BattleState.UseMotRecoveryItem
          || BattleState1 == BattleState.UsePoisonRecoveryItem
          || BattleState1 == BattleState.UseSleepRecoveryItem
          || BattleState1 == BattleState.UseParalysisRecoveryItem
          || BattleState1 == BattleState.UseConfusionRecoveryItem
          || BattleState1 == BattleState.UseDepressionRecoveryItem
          )
        {
            UseItem();
            //�@�����̃^�[���������̂ŏオ�����p�����[�^�̃`�F�b�N
            CheckIncreaseAttackPower();
            CheckIncreaseDefense();
            CheckDecreaseAttackPower();
            CheckDecreaseDefense();
            DepressionEffect();
        }
        //�@�^�[�Q�b�g�̃��Z�b�g
        _currentTarget = null;
        _currentSkill = null;
        _currentItem = null;
        _targetCharacterBattleScript = null;
        _targetCharacterStatus = null;
        BattleState1 = BattleState.Idle;
        //�@�^�[���I�����̏���
        PoisonDamage1 = false;
        if (IsDepression == true)
        {
            DepressionDamage();
        }
        //�@���g�̑I�����I�������玟�̃L�����N�^�[�ɂ���
        _battleManager.ChangeNextChara();
        IsDoneAnimation = false;
    }

    //�@�I��������I�񂾃��[�h�����s
    public void ChooseAttackOptions(BattleState selectOption, GameObject target, Skill skill = null, Item item = null)
    {

        //�@�X�L����^�[�Q�b�g�̏����Z�b�g
        _currentTarget = target;
        _currentSkill = skill;
        _targetCharacterBattleScript = target.GetComponent<CharacterBattleScript>();
        _targetCharacterStatus = _targetCharacterBattleScript.CharacterStatus;

        //�@�I�������L�����N�^�[�̏�Ԃ�ݒ�
        _battleState = selectOption;

        if (selectOption == BattleState.DirectAttack)
        {
            //_animator.SetTrigger("DirectAttack");
        }
        else if (selectOption == BattleState.MagicAttack
          || selectOption == BattleState.Heal
          || selectOption == BattleState.IncreaseAttackPowerMagic
          || selectOption == BattleState.IncreaseDefensePowerMagic
          || selectOption == BattleState.PoisonRecoveryMagic
          || selectOption == BattleState.SleepRecoveryMagic
          || selectOption == BattleState.ParalysisRecoveryMagic
          || selectOption == BattleState.ConfusionRecoveryMagic
          || selectOption == BattleState.DepressionRecoveryMagic
          )
        {
            //_animator.SetTrigger("MagicAttack");
            //�@���@�g�p�҂�MP�����炷
            Mp -= ((Magic)skill).MagicPoints;
            //�@�g�p�҂������L�����N�^�[�ł����StatusPanel�̍X�V
            if (CharacterStatus as PlayerStatus != null)
            {
                _battleStatusScript.UpdateStatus(BattleStatusScript.Status.MP, Mp);
            }
            else if (CharacterStatus as EnemyStatus != null)
            {
                _enemyBattleStatusScript.UpdateEnemyStatus(BattleStatusScript.Status.MP, Mp);
            }
            //Instantiate(((Magic)skill).SkillUserEffect, transform.position, ((Magic)skill).SkillUserEffect.transform.rotation);
        }
        else if (selectOption == BattleState.UseHPRecoveryItem
          || selectOption == BattleState.UseMPRecoveryItem
          || selectOption == BattleState.UseMotRecoveryItem
          || selectOption == BattleState.UsePoisonRecoveryItem
          || selectOption == BattleState.UseSleepRecoveryItem
          || selectOption == BattleState.UseParalysisRecoveryItem
          || selectOption == BattleState.UseConfusionRecoveryItem
          || selectOption == BattleState.UseDepressionRecoveryItem
          || selectOption == BattleState.IncreaseAttackAndDecreaseDefense
          || selectOption == BattleState.IncreaseDefenseAndDecreaseAttack
          )
        {
            _currentItem = item;
            //_animator.SetTrigger("UseItem");
        }
    }

    //�@�^�[�Q�b�g�G�t�F�N�g�̕\��
    public void ShowEffectOnTheTarget()
    {
        //Instantiate<GameObject>(_currentSkill.SkillReceivingEffect, _currentTarget.transform.position, _currentSkill.SkillReceivingEffect.transform.rotation);
    }

    public void DirectAttack()
    {
        //var targetAnimator = _currentTarget.GetComponent<Animator>();
        //targetAnimator.SetTrigger("Damage");
        var damage = 0;

        //�@�U�������Status
        if (_targetCharacterStatus as PlayerStatus != null)
        {
            var castedTargetStatus = (PlayerStatus)_targetCharacterBattleScript.CharacterStatus;
            //�@�U������̒ʏ�̖h��́{����̃L�����̕⏕�l
            var attackPower = _characterStatus.Power + AuxiliaryPower;
            var targetDefencePower = (castedTargetStatus.Defense + (castedTargetStatus.EquipArmor?.ItemAmount ?? 0) + _targetCharacterBattleScript.AuxiliaryDefense) * (castedTargetStatus.EquipArmor?.MoreDef ?? 0);
            damage = Mathf.Max(0, Mathf.FloorToInt(((attackPower - targetDefencePower) * DamageUpRate)));
            //�@����̃X�e�[�^�X��HP���Z�b�g
            _targetCharacterBattleScript.Hp = _targetCharacterBattleScript.Hp - damage;
            //�@��������
            _targetCharacterBattleScript.IsSleep = false;
            //�@�X�e�[�^�XUI���X�V
            _battleStatusScript.UpdateStatus(BattleStatusScript.Status.HP, _targetCharacterBattleScript.Hp);
        }
        else if (_targetCharacterStatus as EnemyStatus != null)
        {
            var attackCharacter = (PlayerStatus)_characterStatus;
            var castedTargetStatus = (EnemyStatus)_targetCharacterBattleScript.CharacterStatus;
            //�@�U������̒ʏ�̖h��́{����̃L�����̕⏕�l
            var attackPower = (_characterStatus.Power + (attackCharacter.EquipWeapon?.ItemAmount ?? 0) + AuxiliaryPower) * (attackCharacter.EquipWeapon?.MoreAtk ?? 0);
            var targetDefencePower = castedTargetStatus.Defense + _targetCharacterBattleScript.AuxiliaryDefense;
            damage = Mathf.Max(0, Mathf.FloorToInt(((attackPower - targetDefencePower) * DamageUpRate) * (attackCharacter.EquipWeapon?.MoreDmg ?? 0)));
            //�@�G�̃X�e�[�^�X��HP���Z�b�g
            _targetCharacterBattleScript.Hp = _targetCharacterBattleScript.Hp - damage;
            //�@��������
            _targetCharacterBattleScript.IsSleep = false;
            //�@�X�e�[�^�XUI���X�V
            BattleStatusScript battleStatusScript = _currentTarget.GetComponent<BattleStatusScript>();
            battleStatusScript.UpdateEnemyStatus(BattleStatusScript.Status.HP, _targetCharacterBattleScript.Hp);
        }
        else
        {
            Debug.LogError("���ڍU���Ń^�[�Q�b�g���ݒ肳��Ă��Ȃ�");
        }

        Debug.Log(gameObject.name + "��" + _currentTarget.name + "��" + _currentSkill.Name + "������" + damage + "��^�����B");
        _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.Damage, _currentTarget.transform, damage);
    }

    public void MagicAttack()
    {
        //var targetAnimator = _currentTarget.GetComponent<Animator>();
        //targetAnimator.SetTrigger("Damage");
        var damage = 0;

        //�@�U�������Status
        if (_targetCharacterStatus as PlayerStatus != null)
        {
            var castedTargetStatus = (PlayerStatus)_targetCharacterBattleScript.CharacterStatus;
            //damage = Mathf.Max(0, ((Magic)_currentSkill).MagicAmount - targetDefencePower);
            var attackPower = _characterStatus.Power + AuxiliaryPower;
            var targetDefencePower = (castedTargetStatus.Defense + (castedTargetStatus.EquipArmor?.ItemAmount ?? 0) + _targetCharacterBattleScript.AuxiliaryDefense) * (castedTargetStatus.EquipArmor?.MoreDef ?? 0);
            damage = Mathf.Max(0, Mathf.FloorToInt(((attackPower - targetDefencePower) * DamageUpRate)));
            ////�@����̃X�e�[�^�X��HP���Z�b�g
            _targetCharacterBattleScript.Hp = _targetCharacterBattleScript.Hp - damage;
            //�@��������
            _targetCharacterBattleScript.IsSleep = false;
            //�@�X�e�[�^�XUI���X�V
            _battleStatusScript.UpdateStatus(BattleStatusScript.Status.HP, _targetCharacterBattleScript.Hp);
        }
        else if (_targetCharacterStatus as EnemyStatus != null)
        {
            var attackCharacter = (PlayerStatus)_characterStatus;
            var castedTargetStatus = (EnemyStatus)_targetCharacterBattleScript.CharacterStatus;
            //damage = Mathf.Max(0, ((Magic)_currentSkill).MagicAmount - targetDefencePower);
            var attackPower = (_characterStatus.Power + (attackCharacter.EquipWeapon?.ItemAmount ?? 0) + AuxiliaryPower) * (attackCharacter.EquipWeapon?.MoreAtk ?? 0);
            var targetDefencePower = castedTargetStatus.Defense + _targetCharacterBattleScript.AuxiliaryDefense;
            damage = Mathf.Max(0, Mathf.FloorToInt(((attackPower - targetDefencePower) * DamageUpRate) * (attackCharacter.EquipWeapon?.MoreDmg ?? 0)));
            //�@����̃X�e�[�^�X��HP���Z�b�g
            _targetCharacterBattleScript.Hp = _targetCharacterBattleScript.Hp - damage;
            //�@��������
            _targetCharacterBattleScript.IsSleep = false;
            //�@�X�e�[�^�XUI���X�V
            BattleStatusScript battleStatusScript = _currentTarget.GetComponent<BattleStatusScript>();
            battleStatusScript.UpdateEnemyStatus(BattleStatusScript.Status.HP, _targetCharacterBattleScript.Hp);
        }
        else
        {
            Debug.LogError("���@�U���Ń^�[�Q�b�g���ݒ肳��Ă��Ȃ�");
        }

        Debug.Log(gameObject.name + "��" + _currentTarget.name + "��" + _currentSkill.Name + "������" + damage + "��^�����B");
        _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.Damage, _currentTarget.transform, damage);
    }

    public void UseMagic()
    {
        //�@�A�j���[�V������Ԃ�����ĂȂ������̂�Damage�ɂ���
        //_currentTarget.GetComponent<Animator>().SetTrigger("Damage");

        var magicType = ((Magic)_currentSkill).SkillType;
        if (magicType == Skill.Type.RecoveryMagic)
        {
            var recoveryPoint = ((Magic)_currentSkill).MagicAmount + _characterStatus.Power;
            if (_targetCharacterStatus as PlayerStatus != null)
            {
                _targetCharacterBattleScript.Hp = _targetCharacterBattleScript.Hp + recoveryPoint;
                _battleStatusScript.UpdateStatus(BattleStatusScript.Status.HP, _targetCharacterBattleScript.Hp);
            }
            else
            {
                _targetCharacterBattleScript.Hp = Hp + recoveryPoint;
                _enemyBattleStatusScript.UpdateEnemyStatus(BattleStatusScript.Status.MP, Mp);
            }
            Debug.Log(gameObject.name + "��" + ((Magic)_currentSkill).Name + "���g����" + _currentTarget.name + "��" + recoveryPoint + "�񕜂����B");
            _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.Heal, _currentTarget.transform, recoveryPoint);
        }
        else if (magicType == Skill.Type.IncreaseAttackPowerMagic)
        {
            _increasePowerPoint = Mathf.FloorToInt(_characterStatus.Power * (((Magic)_currentSkill).MagicAmount * 0.01f));
            _targetCharacterBattleScript.AuxiliaryPower = _targetCharacterBattleScript.AuxiliaryPower + _increasePowerPoint;
            _targetCharacterBattleScript.IsIncreasePower = true;
            _increasePowerPointList.Add(_increasePowerPoint);
            _numOfTurnsIncreasePowerList.Add(_numOfTurnsIncreasePower);
            Debug.Log(gameObject.name + "��" + ((Magic)_currentSkill).Name + "���g����" + _currentTarget.name + "�̍U���͂�" + _increasePowerPoint + "���₵���B");
            _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.StatusUp, _currentTarget.transform, _increasePowerPoint);
        }
        else if (magicType == Skill.Type.IncreaseDefensePowerMagic)
        {
            _increaseDefensePoint = Mathf.FloorToInt(_characterStatus.Defense * ((Magic)_currentSkill).MagicAmount);
            _targetCharacterBattleScript.AuxiliaryDefense = _targetCharacterBattleScript.AuxiliaryDefense + _increaseDefensePoint;
            _targetCharacterBattleScript.IsIncreaseDefense = true;
            _increaseDefensePointList.Add(_increaseDefensePoint);
            _numOfTurnsIncreaseDefenseList.Add(_numOfTurnsIncreaseDefense);
            Debug.Log(gameObject.name + "��" + ((Magic)_currentSkill).Name + "���g����" + _currentTarget.name + "�̖h��͂�" + _increaseDefensePoint + "���₵���B");
            _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.StatusUp, _currentTarget.transform, _increasePowerPoint);
        }
        else if (magicType == Skill.Type.PoisonRecoveryMagic)
        {
            _targetCharacterStatus.IsPoisonStatus = false;
            Debug.Log(gameObject.name + "��" + ((Magic)_currentSkill).Name + "���g����" + _currentTarget.name + "�̓ł�������");
        }
        else if (magicType == Skill.Type.SleepRecoveryMagic)
        {
            _targetCharacterStatus.IsSleepStatus = false;
            Debug.Log(gameObject.name + "��" + ((Magic)_currentSkill).Name + "���g����" + _currentTarget.name + "�̍�����������");
        }
        else if (magicType == Skill.Type.ParalysisRecoveryMagic)
        {
            _targetCharacterStatus.IsParalysisStatus = false;
            Debug.Log(gameObject.name + "��" + ((Magic)_currentSkill).Name + "���g����" + _currentTarget.name + "�̖�Ⴢ�������");
        }
        else if (magicType == Skill.Type.ConfusionRecoveryMagic)
        {
            _targetCharacterStatus.IsConfusionStatus = false;
            Debug.Log(gameObject.name + "��" + ((Magic)_currentSkill).Name + "���g����" + _currentTarget.name + "�̍�����������");
        }
        else if (magicType == Skill.Type.DepressionRecoveryMagic)
        {
            _targetCharacterStatus.IsDepressionStatus = false;
            Debug.Log(gameObject.name + "��" + ((Magic)_currentSkill).Name + "���g����" + _currentTarget.name + "�̟T��������");
        }
    }

    public void UseItem()
    {
        //_currentTarget.GetComponent<Animator>().SetTrigger("Damage");

        //�@�L�����N�^�[�̃A�C�e���������炷
        ((PlayerStatus)_characterStatus).ItemDictionary[_currentItem] -= 1;

        if (_currentItem.ItemType == Item.Type.HPRecovery)
        {
            //�@�񕜗�
            var recoveryPoint = _currentItem.ItemAmount;
            _targetCharacterBattleScript.Hp = _targetCharacterBattleScript.Hp + recoveryPoint;
            _battleStatusScript.UpdateStatus(BattleStatusScript.Status.HP, _targetCharacterBattleScript.Hp);
            Debug.Log(gameObject.name + "��" + _currentItem.ItemName + "���g����" + _currentTarget.name + "��HP��" + recoveryPoint + "�񕜂����B");
            _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.Heal, _currentTarget.transform, recoveryPoint);
        }
        else if (_currentItem.ItemType == Item.Type.MPRecovery)
        {
            //�@�񕜗�
            var recoveryPoint = _currentItem.ItemAmount;
            _targetCharacterBattleScript.Mp = _targetCharacterBattleScript.Mp + recoveryPoint;
            _battleStatusScript.UpdateStatus(BattleStatusScript.Status.MP, _targetCharacterBattleScript.Mp);
            Debug.Log(gameObject.name + "��" + _currentItem.ItemName + "���g����" + _currentTarget.name + "��MP��" + recoveryPoint + "�񕜂����B");
            _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.Heal, _currentTarget.transform, recoveryPoint);
        }
        else if (_currentItem.ItemType == Item.Type.MotRecovery)
        {
            //�@�񕜗�
            var recoveryPoint = _currentItem.ItemAmount;
            _targetCharacterBattleScript.Mp = _targetCharacterBattleScript.Mp + recoveryPoint;
            _battleStatusScript.UpdateStatus(BattleStatusScript.Status.Mot, _targetCharacterBattleScript.Mot);
            Debug.Log(gameObject.name + "��" + _currentItem.ItemName + "���g����" + _currentTarget.name + "��MP��" + recoveryPoint + "�񕜂����B");
            _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.Heal, _currentTarget.transform, recoveryPoint);
        }
        else if (_currentItem.ItemType == Item.Type.PoisonRecovery)
        {
            _targetCharacterStatus.IsPoisonStatus = false;
            Debug.Log(gameObject.name + "��" + _currentItem.ItemName + "���g����" + _currentTarget.name + "�̓ł��������B");
        }
        else if (_currentItem.ItemType == Item.Type.SleepRecovery)
        {
            _targetCharacterStatus.IsSleepStatus = false;
            Debug.Log(gameObject.name + "��" + _currentItem.ItemName + "���g����" + _currentTarget.name + "�̍������������B");
        }
        else if (_currentItem.ItemType == Item.Type.ParalysisRecovery)
        {
            _targetCharacterStatus.IsParalysisStatus = false;
            Debug.Log(gameObject.name + "��" + _currentItem.ItemName + "���g����" + _currentTarget.name + "�̖�Ⴢ��������B");
        }
        else if (_currentItem.ItemType == Item.Type.ConfusionRecovery)
        {
            _targetCharacterStatus.IsConfusionStatus = false;
            Debug.Log(gameObject.name + "��" + _currentItem.ItemName + "���g����" + _currentTarget.name + "�̍������������B");
        }
        else if (_currentItem.ItemType == Item.Type.DepressionRecovery)
        {
            _targetCharacterStatus.IsDepressionStatus = false;
            Debug.Log(gameObject.name + "��" + _currentItem.ItemName + "���g����" + _currentTarget.name + "�̟T���������B");
        }
        else if (_currentItem.ItemType == Item.Type.PowerUp)
        {
            _increasePowerPoint = Mathf.FloorToInt(_characterStatus.Power * (_currentItem.ItemAmount * 0.01f));
            _targetCharacterBattleScript.AuxiliaryPower = _targetCharacterBattleScript.AuxiliaryPower + _increasePowerPoint;
            _targetCharacterBattleScript.IsIncreasePower = true;
            _increasePowerPointList.Add(_increasePowerPoint);
            _numOfTurnsIncreasePowerList.Add(_numOfTurnsIncreasePower);
            Debug.Log(gameObject.name + "��" + _currentItem.ItemName + "���g����" + _currentTarget.name + "�̍U���͂�" + _increasePowerPoint + "���₵���B");
            _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.StatusUp, _currentTarget.transform, _increasePowerPoint);

            _decreaseDefensePoint = Mathf.FloorToInt(_characterStatus.Defense * (_currentItem.ItemAmount * 0.01f));
            _targetCharacterBattleScript.AuxiliaryDefense = _targetCharacterBattleScript.AuxiliaryDefense - _decreaseDefensePoint;
            _targetCharacterBattleScript.IsDecreaseDefense = true;
            _decreaseDefensePointList.Add(_decreaseDefensePoint);
            _numOfTurnsDecreaseDefenseList.Add(_numOfTurnsDecreaseDefense);
            Debug.Log(gameObject.name + "��" + _currentItem.ItemName + "���g����" + _currentTarget.name + "�̖h��͂�" + _decreaseDefensePoint + "���炵���B");
            _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.StatusDown, _currentTarget.transform, _decreaseDefensePoint);
        }
        else if (_currentItem.ItemType == Item.Type.DefenseUp)
        {
            _increaseDefensePoint = Mathf.FloorToInt(_characterStatus.Defense * (_currentItem.ItemAmount * 0.01f));
            _targetCharacterBattleScript.AuxiliaryDefense = _targetCharacterBattleScript.AuxiliaryDefense + _increaseDefensePoint;
            _targetCharacterBattleScript.IsIncreaseDefense = true;
            _increaseDefensePointList.Add(_increaseDefensePoint);
            _numOfTurnsIncreaseDefenseList.Add(_numOfTurnsIncreaseDefense);
            Debug.Log(gameObject.name + "��" + _currentItem.ItemName + "���g����" + _currentTarget.name + "�̖h��͂�" + _increaseDefensePoint + "���₵���B");
            _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.StatusUp, _currentTarget.transform, _increaseDefensePoint);

            _decreasePowerPoint = Mathf.FloorToInt(_characterStatus.Power * (_currentItem.ItemAmount * 0.01f));
            _targetCharacterBattleScript.AuxiliaryPower = _targetCharacterBattleScript.AuxiliaryPower - _decreasePowerPoint;
            _targetCharacterBattleScript.IsDecreasePower = true;
            _decreasePowerPointList.Add(_decreasePowerPoint);
            _numOfTurnsDecreaseDefenseList.Add(_numOfTurnsDecreasePower);
            Debug.Log(gameObject.name + "��" + _currentItem.ItemName + "���g����" + _currentTarget.name + "�̍U���͂�" + _decreasePowerPoint + "���炵���B");
            _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.StatusDown, _currentTarget.transform, _decreasePowerPoint);
        }

        //�@�A�C�e������0�ɂȂ�����ItemDictionary���炻�̃A�C�e�����폜
        if (((PlayerStatus)_characterStatus).ItemDictionary[_currentItem] == 0)
        {
            ((PlayerStatus)_characterStatus).ItemDictionary.Remove(_currentItem);
        }
    }

    //�@�h��
    public void Guard()
    {
        //�@�����̃^�[���������̂ŏオ�����p�����[�^�̃`�F�b�N
        CheckIncreaseAttackPower();
        CheckIncreaseDefense();
        //_animator.SetBool("Guard", true);
        AuxiliaryDefense += 10;
    }

    //�@�h�������
    public void UnlockGuard()
    {
        //_animator.SetBool("Guard", false);
        AuxiliaryDefense -= 10;
    }

    //�@���񂾂Ƃ��Ɏ��s���鏈��
    public void Dead()
    {
        //_animator.SetTrigger("Dead");
        _battleManager.DeleteAllCharacterInBattleList(this.gameObject);
        if (CharacterStatus as PlayerStatus != null)
        {
            _battleStatusScript.UpdateStatus(BattleStatusScript.Status.HP, Hp);
            _battleManager.DeletePlayerCharacterInBattleList(this.gameObject);
        }
        else if (CharacterStatus as EnemyStatus != null)
        {
            _enemyBattleStatusScript.UpdateEnemyStatus(BattleStatusScript.Status.MP, Mp);
            _battleManager.DeleteEnemyCharacterInBattleList(this.gameObject);
        }
        _isDead = true;
    }

    public void CheckIncreaseAttackPower()
    {
        //�@�����̃^�[�����������ɉ��炩�̌��ʖ��@���g���Ă���^�[�����𑝂₷
        if (IsIncreasePower == true)
        {
            _numOfTurnsSinceIncreasePower++;
            if (_numOfTurnsSinceIncreasePower >= _numOfTurnsIncreasePowerList[0])
            {
                _numOfTurnsSinceIncreasePower = 0;
                AuxiliaryPower = AuxiliaryPower - _increasePowerPointList[0];
                _increasePowerPointList.RemoveAt(0);
                _numOfTurnsIncreasePowerList.RemoveAt(0);
                if (_increasePowerPointList.Count == 0)
                {
                    IsIncreasePower = false;
                }
                Debug.Log(gameObject.name + "�̍U���̓A�b�v�̌��ʂ�������");
            }
        }
    }

    public void CheckDecreaseAttackPower()
    {
        //�@�����̃^�[�����������ɉ��炩�̌��ʖ��@���g���Ă���^�[�����𑝂₷
        if (IsDecreasePower == true)
        {
            _numOfTurnsSinceDecreasePower++;
            if (_numOfTurnsSinceDecreasePower >= _numOfTurnsDecreasePowerList[0])
            {
                _numOfTurnsSinceDecreasePower = 0;
                AuxiliaryPower = AuxiliaryPower + _decreasePowerPointList[0];
                _decreasePowerPointList.RemoveAt(0);
                _numOfTurnsDecreasePowerList.RemoveAt(0);
                if (_decreasePowerPointList.Count == 0)
                {
                    IsDecreasePower = false;
                }
                Debug.Log(gameObject.name + "�̍U���̓_�E���̌��ʂ�������");
            }
        }
    }

    public void CheckIncreaseDefense()
    {
        if (IsIncreaseDefense == true)
        {
            _numOfTurnsSinceIncreaseDefense++;
            if (_numOfTurnsSinceIncreaseDefense >= _numOfTurnsIncreaseDefenseList[0])
            {
                _numOfTurnsSinceIncreaseDefense = 0;
                AuxiliaryDefense = AuxiliaryDefense - _increaseDefensePointList[0];
                _increaseDefensePointList.RemoveAt(0);
                _numOfTurnsIncreaseDefenseList.RemoveAt(0);
                if (_increaseDefensePointList.Count == 0)
                {
                    IsIncreaseDefense = false;
                }
                Debug.Log(gameObject.name + "�̖h��̓A�b�v�̌��ʂ�������");
            }
        }
    }

    public void CheckDecreaseDefense()
    {
        //�@�����̃^�[�����������ɉ��炩�̌��ʖ��@���g���Ă���^�[�����𑝂₷
        if (IsDecreaseDefense == true)
        {
            _numOfTurnsSinceDecreaseDefense++;
            if (_numOfTurnsSinceDecreaseDefense >= _numOfTurnsDecreaseDefenseList[0])
            {
                _numOfTurnsSinceDecreaseDefense = 0;
                AuxiliaryDefense = AuxiliaryDefense + _decreaseDefensePointList[0];
                _decreaseDefensePointList.RemoveAt(0);
                _numOfTurnsDecreaseDefenseList.RemoveAt(0);
                if (_decreaseDefensePointList.Count == 0)
                {
                    IsDecreaseDefense = false;
                }
                Debug.Log(gameObject.name + "�̖h��̓_�E���̌��ʂ�������");
            }
        }
    }

    //�@�Ń_���[�W
    public void PoisonDamage()
    {
        int poisonDamage = Mathf.FloorToInt(_characterStatus.MaxHp * 0.05f);
        Hp -= poisonDamage;
        _battleStatusScript.UpdateStatus(BattleStatusScript.Status.HP, Hp);
        if (_enemyBattleStatusScript != null)
        {
            _enemyBattleStatusScript.UpdateEnemyStatus(BattleStatusScript.Status.HP, Hp);
        }
        PoisonDamage1 = true;
        Debug.Log("�ł�" + poisonDamage + "�_���[�W���󂯂�");
        _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.Damage, gameObject.transform, poisonDamage);
    }

    //�@�T�_���[�W
    public void DepressionDamage()
    {
        int motDamage = Mathf.FloorToInt(Mot * 0.1f);
        Mot -= motDamage;
        _battleStatusScript.UpdateStatus(BattleStatusScript.Status.Mot, Mot);
        if (_enemyBattleStatusScript != null)
        {
            _enemyBattleStatusScript.UpdateEnemyStatus(BattleStatusScript.Status.Mot, Mot);
        }
        Debug.Log("�T��" + motDamage + "���C��������");
        _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.Damage, gameObject.transform, motDamage);
    }

    //�@�T�f�o�t
    public void DepressionEffect()
    {
        int power = Mathf.FloorToInt(CharacterStatus.Power * 0.25f);
        int defense = Mathf.FloorToInt(CharacterStatus.Defense * 0.25f);
        if (IsDepression == true)
        {
            if (_isDepressionAttack == false)
            {
                AuxiliaryPower -= power;
                _isDepressionAttack = true;
            }
            if (_isDepressionDefense == false)
            {
                AuxiliaryDefense -= defense;
                _isDepressionDefense = true;
            }
        }
        if (IsDepression == false)
        {
            if (_isDepressionAttack == true)
            {
                AuxiliaryPower += power;
                _isDepressionAttack = false;
            }
            if (_isDepressionDefense == false)
            {
                AuxiliaryDefense += defense;
                _isDepressionDefense = false;
            }
        }
    }
}
