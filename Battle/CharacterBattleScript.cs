using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBattleScript : MonoBehaviour
{
    //　戦闘中のキャラクターの状態
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

    //　元のステータスからコピー

    //　HP
    private int _hp = 0;
    //　MP
    private int _mp = 0;
    //　やる気
    private int _mot = 0;

    //　補助の力
    private int _auxiliaryPower = 0;
    //　補助の防御力
    private int _auxiliaryDefense = 0;
    //　毒状態か
    private bool _isPoison;
    //　昏睡状態か
    private bool _isSleep;
    //　麻痺状態か
    private bool _isParalysis;
    //　混乱状態か
    private bool _isConfusion;
    //　鬱状態か
    private bool _isDepression;

    //　今選択したスキル
    private Skill _currentSkill;
    //　今のターゲット
    private GameObject _currentTarget;
    //　今使用したアイテム
    private Item _currentItem;
    //　ターゲットのCharacterBattleScript
    private CharacterBattleScript _targetCharacterBattleScript;
    //　ターゲットのCharacterStatus
    private CharacterStatus _targetCharacterStatus;
    //　攻撃選択後のアニメーションが終了したかどうか
    private bool _isDoneAnimation;
    //　キャラクターが死んでいるかどうか
    private bool _isDead;

    //　攻撃力アップしているかどうか
    private bool _isIncreasePower;
    //　攻撃力アップしているポイント
    private int _increasePowerPoint;
    //　攻撃力アップしているターン
    private int _numOfTurnsIncreasePower = 3;
    //　攻撃力アップしてからのターン
    private int _numOfTurnsSinceIncreasePower = 0;
    //　これまでの攻撃力アップした数値のリスト
    private List<int> _increasePowerPointList = new List<int>();
    //　これまでの攻撃力アップしているターンのリスト
    private List<int> _numOfTurnsIncreasePowerList = new List<int>();
    //　攻撃力ダウンしているかどうか
    private bool _isDecreasePower;
    //　攻撃力ダウンしているポイント
    private int _decreasePowerPoint;
    //　攻撃力ダウンしているターン
    private int _numOfTurnsDecreasePower = 3;
    //　攻撃力ダウンしてからのターン
    private int _numOfTurnsSinceDecreasePower = 0;
    //　これまでの攻撃力ダウンした数値のリスト
    private List<int> _decreasePowerPointList = new List<int>();
    //　これまでの攻撃力ダウンしているターンのリスト
    private List<int> _numOfTurnsDecreasePowerList = new List<int>();
    //　防御力アップしているかどうか
    private bool _isIncreaseDefense;
    //　防御力アップしているポイント
    private int _increaseDefensePoint;
    //　防御力アップしているターン
    private int _numOfTurnsIncreaseDefense = 3;
    //　防御力アップしてからのターン
    private int _numOfTurnsSinceIncreaseDefense = 0;
    //　これまでの防御力アップした数値のリスト
    private List<int> _increaseDefensePointList = new List<int>();
    //　これまでの防御力アップしているターンのリスト
    private List<int> _numOfTurnsIncreaseDefenseList = new List<int>();
    //　防御力ダウンしているかどうか
    private bool _isDecreaseDefense;
    //　防御力ダウンしているポイント
    private int _decreaseDefensePoint;
    //　防御力ダウンしているターン
    private int _numOfTurnsDecreaseDefense = 3;
    //　防御力ダウンしてからのターン
    private int _numOfTurnsSinceDecreaseDefense = 0;
    //　これまでの防御力ダウンした数値のリスト
    private List<int> _decreaseDefensePointList = new List<int>();
    //　これまでの防御力ダウンしているターンのリスト
    private List<int> _numOfTurnsDecreaseDefenseList = new List<int>();
    //　ダメージUP
    private float _damageUpRate = 1;

    //　毒のダメージを受けたかどうか
    private bool _poisonDamage = false;
    //　毒のターン
    private int _poisonTurn = 3;
    //　麻痺のターン
    private int _paralysisTurn = 3;
    //　鬱で攻撃力を減少させたか
    private bool _isDepressionAttack = false;
    //　鬱で防御力を減少させたか
    private bool _isDepressionDefense = false;

    //　効果ポイント表示スクリプト
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
        //　元データから設定
        Hp = CharacterStatus.Hp;
        Mp = CharacterStatus.Mp;
        Mot = CharacterStatus.Mot;
        IsPoison = CharacterStatus.IsPoisonStatus;
        IsSleep = CharacterStatus.IsSleepStatus;
        IsParalysis = CharacterStatus.IsParalysisStatus;
        IsConfusion = CharacterStatus.IsConfusionStatus;
        IsDepression = CharacterStatus.IsDepressionStatus;

        //　状態の設定
        BattleState1 = BattleState.Idle;
        //　コンポーネントの取得
        _battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        _battleStatusScript = GameObject.Find("BattleUI/StatusPanel").GetComponent<BattleStatusScript>();
        _effectNumericalDisplayScript = _battleManager.GetComponent<EffectNumericalDisplayScript>();
        //　既に死んでいる場合は倒れている状態にする
        if (CharacterStatus.Hp <= 0)
        {
            //_animator.CrossFade("Dead", 0f, 0, 1f);
            _isDead = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //　既に死んでいたら何もしない
        if (_isDead == true)
        {
            return;
        }

        //　自分のターンでなければ何もしない
        if (BattleState1 == BattleState.Idle)
        {
            return;
        }
        //　アニメーションが終わっていなければ何もしない
        if (IsDoneAnimation == false)
        {
            //アニメーションを付けるまでオフにしておく　アニメーションができたらビヘイビアを設定する
            //return;
        }

        //　選択したアニメーションによって処理を分ける
        if (BattleState1 == BattleState.DirectAttack)
        {
            ShowEffectOnTheTarget();
            DirectAttack();
            //　自分のターンが来たので上がったパラメータのチェック
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
            //　自分のターンが来たので上がったパラメータのチェック
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
            //　自分のターンが来たので上がったパラメータのチェック
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
            //　自身の攻撃力をアップした場合はターン数をカウントしない
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
            //　自身の攻撃力をアップした場合はターン数をカウントしない
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
            //　自身の防御力をアップした場合はターン数をカウントしない
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
            //　自身の防御力をアップした場合はターン数をカウントしない
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
            //　自分のターンが来たので上がったパラメータのチェック
            CheckIncreaseAttackPower();
            CheckIncreaseDefense();
            CheckDecreaseAttackPower();
            CheckDecreaseDefense();
            DepressionEffect();
        }
        //　ターゲットのリセット
        _currentTarget = null;
        _currentSkill = null;
        _currentItem = null;
        _targetCharacterBattleScript = null;
        _targetCharacterStatus = null;
        BattleState1 = BattleState.Idle;
        //　ターン終了時の処理
        PoisonDamage1 = false;
        if (IsDepression == true)
        {
            DepressionDamage();
        }
        //　自身の選択が終了したら次のキャラクターにする
        _battleManager.ChangeNextChara();
        IsDoneAnimation = false;
    }

    //　選択肢から選んだモードを実行
    public void ChooseAttackOptions(BattleState selectOption, GameObject target, Skill skill = null, Item item = null)
    {

        //　スキルやターゲットの情報をセット
        _currentTarget = target;
        _currentSkill = skill;
        _targetCharacterBattleScript = target.GetComponent<CharacterBattleScript>();
        _targetCharacterStatus = _targetCharacterBattleScript.CharacterStatus;

        //　選択したキャラクターの状態を設定
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
            //　魔法使用者のMPを減らす
            Mp -= ((Magic)skill).MagicPoints;
            //　使用者が味方キャラクターであればStatusPanelの更新
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

    //　ターゲットエフェクトの表示
    public void ShowEffectOnTheTarget()
    {
        //Instantiate<GameObject>(_currentSkill.SkillReceivingEffect, _currentTarget.transform.position, _currentSkill.SkillReceivingEffect.transform.rotation);
    }

    public void DirectAttack()
    {
        //var targetAnimator = _currentTarget.GetComponent<Animator>();
        //targetAnimator.SetTrigger("Damage");
        var damage = 0;

        //　攻撃相手のStatus
        if (_targetCharacterStatus as PlayerStatus != null)
        {
            var castedTargetStatus = (PlayerStatus)_targetCharacterBattleScript.CharacterStatus;
            //　攻撃相手の通常の防御力＋相手のキャラの補助値
            var attackPower = _characterStatus.Power + AuxiliaryPower;
            var targetDefencePower = (castedTargetStatus.Defense + (castedTargetStatus.EquipArmor?.ItemAmount ?? 0) + _targetCharacterBattleScript.AuxiliaryDefense) * (castedTargetStatus.EquipArmor?.MoreDef ?? 0);
            damage = Mathf.Max(0, Mathf.FloorToInt(((attackPower - targetDefencePower) * DamageUpRate)));
            //　相手のステータスのHPをセット
            _targetCharacterBattleScript.Hp = _targetCharacterBattleScript.Hp - damage;
            //　昏睡解除
            _targetCharacterBattleScript.IsSleep = false;
            //　ステータスUIを更新
            _battleStatusScript.UpdateStatus(BattleStatusScript.Status.HP, _targetCharacterBattleScript.Hp);
        }
        else if (_targetCharacterStatus as EnemyStatus != null)
        {
            var attackCharacter = (PlayerStatus)_characterStatus;
            var castedTargetStatus = (EnemyStatus)_targetCharacterBattleScript.CharacterStatus;
            //　攻撃相手の通常の防御力＋相手のキャラの補助値
            var attackPower = (_characterStatus.Power + (attackCharacter.EquipWeapon?.ItemAmount ?? 0) + AuxiliaryPower) * (attackCharacter.EquipWeapon?.MoreAtk ?? 0);
            var targetDefencePower = castedTargetStatus.Defense + _targetCharacterBattleScript.AuxiliaryDefense;
            damage = Mathf.Max(0, Mathf.FloorToInt(((attackPower - targetDefencePower) * DamageUpRate) * (attackCharacter.EquipWeapon?.MoreDmg ?? 0)));
            //　敵のステータスのHPをセット
            _targetCharacterBattleScript.Hp = _targetCharacterBattleScript.Hp - damage;
            //　昏睡解除
            _targetCharacterBattleScript.IsSleep = false;
            //　ステータスUIを更新
            BattleStatusScript battleStatusScript = _currentTarget.GetComponent<BattleStatusScript>();
            battleStatusScript.UpdateEnemyStatus(BattleStatusScript.Status.HP, _targetCharacterBattleScript.Hp);
        }
        else
        {
            Debug.LogError("直接攻撃でターゲットが設定されていない");
        }

        Debug.Log(gameObject.name + "は" + _currentTarget.name + "に" + _currentSkill.Name + "をして" + damage + "を与えた。");
        _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.Damage, _currentTarget.transform, damage);
    }

    public void MagicAttack()
    {
        //var targetAnimator = _currentTarget.GetComponent<Animator>();
        //targetAnimator.SetTrigger("Damage");
        var damage = 0;

        //　攻撃相手のStatus
        if (_targetCharacterStatus as PlayerStatus != null)
        {
            var castedTargetStatus = (PlayerStatus)_targetCharacterBattleScript.CharacterStatus;
            //damage = Mathf.Max(0, ((Magic)_currentSkill).MagicAmount - targetDefencePower);
            var attackPower = _characterStatus.Power + AuxiliaryPower;
            var targetDefencePower = (castedTargetStatus.Defense + (castedTargetStatus.EquipArmor?.ItemAmount ?? 0) + _targetCharacterBattleScript.AuxiliaryDefense) * (castedTargetStatus.EquipArmor?.MoreDef ?? 0);
            damage = Mathf.Max(0, Mathf.FloorToInt(((attackPower - targetDefencePower) * DamageUpRate)));
            ////　相手のステータスのHPをセット
            _targetCharacterBattleScript.Hp = _targetCharacterBattleScript.Hp - damage;
            //　昏睡解除
            _targetCharacterBattleScript.IsSleep = false;
            //　ステータスUIを更新
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
            //　相手のステータスのHPをセット
            _targetCharacterBattleScript.Hp = _targetCharacterBattleScript.Hp - damage;
            //　昏睡解除
            _targetCharacterBattleScript.IsSleep = false;
            //　ステータスUIを更新
            BattleStatusScript battleStatusScript = _currentTarget.GetComponent<BattleStatusScript>();
            battleStatusScript.UpdateEnemyStatus(BattleStatusScript.Status.HP, _targetCharacterBattleScript.Hp);
        }
        else
        {
            Debug.LogError("魔法攻撃でターゲットが設定されていない");
        }

        Debug.Log(gameObject.name + "は" + _currentTarget.name + "に" + _currentSkill.Name + "をして" + damage + "を与えた。");
        _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.Damage, _currentTarget.transform, damage);
    }

    public void UseMagic()
    {
        //　アニメーション状態を作ってなかったのでDamageにする
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
            Debug.Log(gameObject.name + "は" + ((Magic)_currentSkill).Name + "を使って" + _currentTarget.name + "を" + recoveryPoint + "回復した。");
            _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.Heal, _currentTarget.transform, recoveryPoint);
        }
        else if (magicType == Skill.Type.IncreaseAttackPowerMagic)
        {
            _increasePowerPoint = Mathf.FloorToInt(_characterStatus.Power * (((Magic)_currentSkill).MagicAmount * 0.01f));
            _targetCharacterBattleScript.AuxiliaryPower = _targetCharacterBattleScript.AuxiliaryPower + _increasePowerPoint;
            _targetCharacterBattleScript.IsIncreasePower = true;
            _increasePowerPointList.Add(_increasePowerPoint);
            _numOfTurnsIncreasePowerList.Add(_numOfTurnsIncreasePower);
            Debug.Log(gameObject.name + "は" + ((Magic)_currentSkill).Name + "を使って" + _currentTarget.name + "の攻撃力を" + _increasePowerPoint + "増やした。");
            _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.StatusUp, _currentTarget.transform, _increasePowerPoint);
        }
        else if (magicType == Skill.Type.IncreaseDefensePowerMagic)
        {
            _increaseDefensePoint = Mathf.FloorToInt(_characterStatus.Defense * ((Magic)_currentSkill).MagicAmount);
            _targetCharacterBattleScript.AuxiliaryDefense = _targetCharacterBattleScript.AuxiliaryDefense + _increaseDefensePoint;
            _targetCharacterBattleScript.IsIncreaseDefense = true;
            _increaseDefensePointList.Add(_increaseDefensePoint);
            _numOfTurnsIncreaseDefenseList.Add(_numOfTurnsIncreaseDefense);
            Debug.Log(gameObject.name + "は" + ((Magic)_currentSkill).Name + "を使って" + _currentTarget.name + "の防御力を" + _increaseDefensePoint + "増やした。");
            _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.StatusUp, _currentTarget.transform, _increasePowerPoint);
        }
        else if (magicType == Skill.Type.PoisonRecoveryMagic)
        {
            _targetCharacterStatus.IsPoisonStatus = false;
            Debug.Log(gameObject.name + "は" + ((Magic)_currentSkill).Name + "を使って" + _currentTarget.name + "の毒を消した");
        }
        else if (magicType == Skill.Type.SleepRecoveryMagic)
        {
            _targetCharacterStatus.IsSleepStatus = false;
            Debug.Log(gameObject.name + "は" + ((Magic)_currentSkill).Name + "を使って" + _currentTarget.name + "の昏睡を消した");
        }
        else if (magicType == Skill.Type.ParalysisRecoveryMagic)
        {
            _targetCharacterStatus.IsParalysisStatus = false;
            Debug.Log(gameObject.name + "は" + ((Magic)_currentSkill).Name + "を使って" + _currentTarget.name + "の麻痺を消した");
        }
        else if (magicType == Skill.Type.ConfusionRecoveryMagic)
        {
            _targetCharacterStatus.IsConfusionStatus = false;
            Debug.Log(gameObject.name + "は" + ((Magic)_currentSkill).Name + "を使って" + _currentTarget.name + "の混乱を消した");
        }
        else if (magicType == Skill.Type.DepressionRecoveryMagic)
        {
            _targetCharacterStatus.IsDepressionStatus = false;
            Debug.Log(gameObject.name + "は" + ((Magic)_currentSkill).Name + "を使って" + _currentTarget.name + "の鬱を消した");
        }
    }

    public void UseItem()
    {
        //_currentTarget.GetComponent<Animator>().SetTrigger("Damage");

        //　キャラクターのアイテム数を減らす
        ((PlayerStatus)_characterStatus).ItemDictionary[_currentItem] -= 1;

        if (_currentItem.ItemType == Item.Type.HPRecovery)
        {
            //　回復力
            var recoveryPoint = _currentItem.ItemAmount;
            _targetCharacterBattleScript.Hp = _targetCharacterBattleScript.Hp + recoveryPoint;
            _battleStatusScript.UpdateStatus(BattleStatusScript.Status.HP, _targetCharacterBattleScript.Hp);
            Debug.Log(gameObject.name + "は" + _currentItem.ItemName + "を使って" + _currentTarget.name + "のHPを" + recoveryPoint + "回復した。");
            _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.Heal, _currentTarget.transform, recoveryPoint);
        }
        else if (_currentItem.ItemType == Item.Type.MPRecovery)
        {
            //　回復力
            var recoveryPoint = _currentItem.ItemAmount;
            _targetCharacterBattleScript.Mp = _targetCharacterBattleScript.Mp + recoveryPoint;
            _battleStatusScript.UpdateStatus(BattleStatusScript.Status.MP, _targetCharacterBattleScript.Mp);
            Debug.Log(gameObject.name + "は" + _currentItem.ItemName + "を使って" + _currentTarget.name + "のMPを" + recoveryPoint + "回復した。");
            _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.Heal, _currentTarget.transform, recoveryPoint);
        }
        else if (_currentItem.ItemType == Item.Type.MotRecovery)
        {
            //　回復力
            var recoveryPoint = _currentItem.ItemAmount;
            _targetCharacterBattleScript.Mp = _targetCharacterBattleScript.Mp + recoveryPoint;
            _battleStatusScript.UpdateStatus(BattleStatusScript.Status.Mot, _targetCharacterBattleScript.Mot);
            Debug.Log(gameObject.name + "は" + _currentItem.ItemName + "を使って" + _currentTarget.name + "のMPを" + recoveryPoint + "回復した。");
            _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.Heal, _currentTarget.transform, recoveryPoint);
        }
        else if (_currentItem.ItemType == Item.Type.PoisonRecovery)
        {
            _targetCharacterStatus.IsPoisonStatus = false;
            Debug.Log(gameObject.name + "は" + _currentItem.ItemName + "を使って" + _currentTarget.name + "の毒を消した。");
        }
        else if (_currentItem.ItemType == Item.Type.SleepRecovery)
        {
            _targetCharacterStatus.IsSleepStatus = false;
            Debug.Log(gameObject.name + "は" + _currentItem.ItemName + "を使って" + _currentTarget.name + "の昏睡を消した。");
        }
        else if (_currentItem.ItemType == Item.Type.ParalysisRecovery)
        {
            _targetCharacterStatus.IsParalysisStatus = false;
            Debug.Log(gameObject.name + "は" + _currentItem.ItemName + "を使って" + _currentTarget.name + "の麻痺を消した。");
        }
        else if (_currentItem.ItemType == Item.Type.ConfusionRecovery)
        {
            _targetCharacterStatus.IsConfusionStatus = false;
            Debug.Log(gameObject.name + "は" + _currentItem.ItemName + "を使って" + _currentTarget.name + "の混乱を消した。");
        }
        else if (_currentItem.ItemType == Item.Type.DepressionRecovery)
        {
            _targetCharacterStatus.IsDepressionStatus = false;
            Debug.Log(gameObject.name + "は" + _currentItem.ItemName + "を使って" + _currentTarget.name + "の鬱を消した。");
        }
        else if (_currentItem.ItemType == Item.Type.PowerUp)
        {
            _increasePowerPoint = Mathf.FloorToInt(_characterStatus.Power * (_currentItem.ItemAmount * 0.01f));
            _targetCharacterBattleScript.AuxiliaryPower = _targetCharacterBattleScript.AuxiliaryPower + _increasePowerPoint;
            _targetCharacterBattleScript.IsIncreasePower = true;
            _increasePowerPointList.Add(_increasePowerPoint);
            _numOfTurnsIncreasePowerList.Add(_numOfTurnsIncreasePower);
            Debug.Log(gameObject.name + "は" + _currentItem.ItemName + "を使って" + _currentTarget.name + "の攻撃力を" + _increasePowerPoint + "増やした。");
            _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.StatusUp, _currentTarget.transform, _increasePowerPoint);

            _decreaseDefensePoint = Mathf.FloorToInt(_characterStatus.Defense * (_currentItem.ItemAmount * 0.01f));
            _targetCharacterBattleScript.AuxiliaryDefense = _targetCharacterBattleScript.AuxiliaryDefense - _decreaseDefensePoint;
            _targetCharacterBattleScript.IsDecreaseDefense = true;
            _decreaseDefensePointList.Add(_decreaseDefensePoint);
            _numOfTurnsDecreaseDefenseList.Add(_numOfTurnsDecreaseDefense);
            Debug.Log(gameObject.name + "は" + _currentItem.ItemName + "を使って" + _currentTarget.name + "の防御力を" + _decreaseDefensePoint + "減らした。");
            _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.StatusDown, _currentTarget.transform, _decreaseDefensePoint);
        }
        else if (_currentItem.ItemType == Item.Type.DefenseUp)
        {
            _increaseDefensePoint = Mathf.FloorToInt(_characterStatus.Defense * (_currentItem.ItemAmount * 0.01f));
            _targetCharacterBattleScript.AuxiliaryDefense = _targetCharacterBattleScript.AuxiliaryDefense + _increaseDefensePoint;
            _targetCharacterBattleScript.IsIncreaseDefense = true;
            _increaseDefensePointList.Add(_increaseDefensePoint);
            _numOfTurnsIncreaseDefenseList.Add(_numOfTurnsIncreaseDefense);
            Debug.Log(gameObject.name + "は" + _currentItem.ItemName + "を使って" + _currentTarget.name + "の防御力を" + _increaseDefensePoint + "増やした。");
            _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.StatusUp, _currentTarget.transform, _increaseDefensePoint);

            _decreasePowerPoint = Mathf.FloorToInt(_characterStatus.Power * (_currentItem.ItemAmount * 0.01f));
            _targetCharacterBattleScript.AuxiliaryPower = _targetCharacterBattleScript.AuxiliaryPower - _decreasePowerPoint;
            _targetCharacterBattleScript.IsDecreasePower = true;
            _decreasePowerPointList.Add(_decreasePowerPoint);
            _numOfTurnsDecreaseDefenseList.Add(_numOfTurnsDecreasePower);
            Debug.Log(gameObject.name + "は" + _currentItem.ItemName + "を使って" + _currentTarget.name + "の攻撃力を" + _decreasePowerPoint + "減らした。");
            _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.StatusDown, _currentTarget.transform, _decreasePowerPoint);
        }

        //　アイテム数が0になったらItemDictionaryからそのアイテムを削除
        if (((PlayerStatus)_characterStatus).ItemDictionary[_currentItem] == 0)
        {
            ((PlayerStatus)_characterStatus).ItemDictionary.Remove(_currentItem);
        }
    }

    //　防御
    public void Guard()
    {
        //　自分のターンが来たので上がったパラメータのチェック
        CheckIncreaseAttackPower();
        CheckIncreaseDefense();
        //_animator.SetBool("Guard", true);
        AuxiliaryDefense += 10;
    }

    //　防御を解除
    public void UnlockGuard()
    {
        //_animator.SetBool("Guard", false);
        AuxiliaryDefense -= 10;
    }

    //　死んだときに実行する処理
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
        //　自分のターンが来た時に何らかの効果魔法を使ってたらターン数を増やす
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
                Debug.Log(gameObject.name + "の攻撃力アップの効果が消えた");
            }
        }
    }

    public void CheckDecreaseAttackPower()
    {
        //　自分のターンが来た時に何らかの効果魔法を使ってたらターン数を増やす
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
                Debug.Log(gameObject.name + "の攻撃力ダウンの効果が消えた");
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
                Debug.Log(gameObject.name + "の防御力アップの効果が消えた");
            }
        }
    }

    public void CheckDecreaseDefense()
    {
        //　自分のターンが来た時に何らかの効果魔法を使ってたらターン数を増やす
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
                Debug.Log(gameObject.name + "の防御力ダウンの効果が消えた");
            }
        }
    }

    //　毒ダメージ
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
        Debug.Log("毒で" + poisonDamage + "ダメージを受けた");
        _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.Damage, gameObject.transform, poisonDamage);
    }

    //　鬱ダメージ
    public void DepressionDamage()
    {
        int motDamage = Mathf.FloorToInt(Mot * 0.1f);
        Mot -= motDamage;
        _battleStatusScript.UpdateStatus(BattleStatusScript.Status.Mot, Mot);
        if (_enemyBattleStatusScript != null)
        {
            _enemyBattleStatusScript.UpdateEnemyStatus(BattleStatusScript.Status.Mot, Mot);
        }
        Debug.Log("鬱で" + motDamage + "やる気が減った");
        _effectNumericalDisplayScript.InstantiatePointText(EffectNumericalDisplayScript.NumberType.Damage, gameObject.transform, motDamage);
    }

    //　鬱デバフ
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
