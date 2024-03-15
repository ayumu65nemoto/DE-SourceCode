using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public enum CommandMode
    {
        SelectCommand,
        SelectDirectAttack,
        SelectSkill,
        SelectSkillAttackTarget,
        SelectUseSkillOnAlliesTarget,
        SelectItem,
        SelectRecoveryItemTarget
    }

    //　戦闘データ
    [SerializeField]
    private BattleData _battleData;
    //　キャラクターのベース位置
    [SerializeField]
    private Transform _battleBasePosition;
    //　現在戦闘に参加しているキャラクター
    private List<GameObject> _allCharacterList = new List<GameObject>();

    //　現在戦闘に参加している全キャラクター
    private List<GameObject> _allCharacterInBattleList = new List<GameObject>();
    //　現在戦闘に参加している味方キャラクター
    private List<GameObject> _allyCharacterInBattleList = new List<GameObject>();
    //　現在戦闘に参加している敵キャラクター
    private List<GameObject> _enemyCharacterInBattleList = new List<GameObject>();
    //　現在の攻撃の順番
    private int _currentAttackOrder;
    //　現在攻撃をしようとしている人が選択中
    private bool _isChoosing;
    //　戦闘が開始しているかどうか
    private bool _isStartBattle;
    //　戦闘シーンの最初の攻撃が始まるまでの待機時間
    [SerializeField]
    private float _firstWaitingTime = 3f;
    //　戦闘シーンのキャラ移行時の間の時間
    [SerializeField]
    private float _timeToNextCharacter = 1f;
    //　待ち時間
    private float _waitTime;
    //　戦闘シーンの最初の攻撃が始まるまでの経過時間
    private float _elapsedTime;
    //　戦闘が終了したかどうか
    private bool _battleIsOver;
    //　現在のコマンド
    [SerializeField]
    private CommandMode _currentCommand;

    //　味方パーティーのコマンドパネル
    [SerializeField]
    private Transform _commandPanel = null;
    [SerializeField]
    private GameObject _directAttackButtonObj = null;
    [SerializeField]
    private GameObject _itemButtonObj = null;
    [SerializeField]
    private GameObject _skillButtonObj = null;
    [SerializeField]
    private GameObject _getAwayButtonObj = null;

    //　戦闘用キャラクター選択アイコンプレハブ
    [SerializeField]
    private GameObject _battleCharacterSelecter = null;
    //　現在表示されているキャラクター選択アイコン
    [SerializeField]
    private GameObject _currentSelecterObject = null;
    //　選択されている戦闘用キャラクター
    [SerializeField]
    private GameObject _selectedBattleCharacter = null;
    //　何番目のキャラクターが格納されているか
    [SerializeField]
    private int _selecterNum = 0;
    //　攻撃するキャラクター
    [SerializeField]
    private GameObject _attackCharacter = null;
    //　発動するスキル
    [SerializeField]
    private Skill _activeSkill = null;
    //　攻撃ボタン１回スルー
    private bool _isAttack = false;

    //　魔法やアイテム選択パネル
    [SerializeField]
    private Transform _skillOrItemPanel = null;
    //　魔法やアイテム選択パネルのContent
    [SerializeField]
    private Transform _skillOrItemPanelContent = null;
    //　BattleItemPanelButtonプレハブ
    [SerializeField]
    private GameObject _battleItemPanelButton = null;
    //　BattleMagicPanelButtonプレハブ
    [SerializeField]
    private GameObject _battleSkillPanelButton = null;
    //　最後に選択していたゲームオブジェクトをスタック
    private Stack<GameObject> selectedGameObjectStack = new Stack<GameObject>();

    //　結果表示処理スクリプト
    [SerializeField]
    private BattleResult _battleResult;

    //　プレイヤーキャラクターを配置するTransform
    [SerializeField]
    private Transform _characterTransform;
    //　エネミーキャラクターを配置するTransform
    [SerializeField]
    private List<Transform> _enemyTransform = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        //　キャラクターインスタンスの親
        Transform characterParent = new GameObject("Character").transform;
        //　同じ名前の敵がいた場合の処理に使うリスト
        List<string> enemyNameList = new List<string>();

        GameObject ins;
        CharacterBattleScript characterBattleScript;
        string characterName;

        //　味方のプレハブをインスタンス化
        for (int i = 0; i < _battleData.BattlePartyStatus.PartyMember.Count; i++)
        {
            ins = Instantiate<GameObject>(_battleData.BattlePartyStatus.PartyMember[i], _characterTransform.position, _characterTransform.rotation, characterParent);
            characterBattleScript = ins.GetComponent<CharacterBattleScript>();
            ins.name = characterBattleScript.CharacterStatus.CharacterName;
            if (characterBattleScript.CharacterStatus.Hp > 0)
            {
                _allyCharacterInBattleList.Add(ins);
                _allCharacterList.Add(ins);
            }
        }
        //　敵のプレハブをインスタンス化
        for (int i = 0; i < _battleData.EnemyPartyStatus.PartyMembers.Count; i++)
        {
            ins = Instantiate<GameObject>(_battleData.EnemyPartyStatus.PartyMembers[i], _enemyTransform[i].position, _enemyTransform[i].rotation, characterParent);
            //　既に同じ敵が存在したら文字を付加する
            characterName = ins.GetComponent<CharacterBattleScript>().CharacterStatus.CharacterName;
            if (enemyNameList.Contains(characterName) == false)
            {
                ins.name = characterName + "A";
            }
            else
            {
                int count = enemyNameList.Count(enemyName => enemyName == characterName);
                char newChar = (char)('A' + count);
                ins.name = characterName + newChar;
            }
            enemyNameList.Add(characterName);
            _enemyCharacterInBattleList.Add(ins);
            _allCharacterList.Add(ins);
        }
        //　現在の戦闘
        _allCharacterInBattleList = _allCharacterList.ToList<GameObject>();
        //　戦闘前の待ち時間を設定
        _waitTime = _firstWaitingTime;
        //　ランダム値のシードの設定
        Random.InitState((int)Time.time);
    }

    private void Update()
    {
        //　戦闘が終了していたらこれ以降何もしない
        if (_battleIsOver == true)
        {
            return;
        }

        //　選択解除された時（マウスでUI外をクリックした）は現在のモードによって無理やり選択させる
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (_currentCommand == CommandMode.SelectCommand)
            {
                EventSystem.current.SetSelectedGameObject(_commandPanel.GetChild(1).gameObject);
            }
            else if (_currentCommand == CommandMode.SelectDirectAttack)
            {
                //EventSystem.current.SetSelectedGameObject(_selectCharacterPanel.GetChild(0).gameObject);
                EventSystem.current.SetSelectedGameObject(_commandPanel.GetChild(1).gameObject);
            }
            else if (_currentCommand == CommandMode.SelectSkill)
            {
                EventSystem.current.SetSelectedGameObject(_skillOrItemPanelContent.GetChild(0).gameObject);
            }
            else if (_currentCommand == CommandMode.SelectSkillAttackTarget)
            {
                EventSystem.current.SetSelectedGameObject(_skillOrItemPanelContent.GetChild(0).gameObject);
            }
            else if (_currentCommand == CommandMode.SelectUseSkillOnAlliesTarget)
            {
                EventSystem.current.SetSelectedGameObject(_skillOrItemPanelContent.GetChild(0).gameObject);
            }
            else if (_currentCommand == CommandMode.SelectItem)
            {
                EventSystem.current.SetSelectedGameObject(_skillOrItemPanelContent.GetChild(0).gameObject);
            }
            else if (_currentCommand == CommandMode.SelectRecoveryItemTarget)
            {
                EventSystem.current.SetSelectedGameObject(_skillOrItemPanelContent.GetChild(0).gameObject);
            }
        }

        //　戦闘開始
        if (_isStartBattle == true)
        {
            //　現在のキャラクターの攻撃が終わっている
            if (_isChoosing == false)
            {
                _elapsedTime += Time.deltaTime;
                if (_elapsedTime < _waitTime)
                {
                    return;
                }
                _elapsedTime = 0f;
                _isChoosing = true;

                //　キャラクターの攻撃の選択に移る
                MakeAttackChoise(_allCharacterInBattleList[_currentAttackOrder]);
                //　次のキャラクターのターンにする
                _currentAttackOrder++;
                //　全員攻撃が終わったら最初から
                if (_currentAttackOrder >= _allCharacterInBattleList.Count)
                {
                    _currentAttackOrder = 0;
                }
            }
            else
            {
                //　キャンセルボタンを押した時の処理
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
                {
                    if (_currentCommand == CommandMode.SelectDirectAttack)
                    {
                        _commandPanel.GetComponent<CanvasGroup>().interactable = true;
                        EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                        _currentCommand = CommandMode.SelectCommand;
                        Destroy(_currentSelecterObject);
                        _selecterNum = 0;
                        _isAttack = false;
                    }
                    else if (_currentCommand == CommandMode.SelectSkill)
                    {
                        // magicOrItemPanelにボタンがあれば全て削除
                        for (int i = _skillOrItemPanelContent.transform.childCount - 1; i >= 0; i--)
                        {
                            Destroy(_skillOrItemPanelContent.transform.GetChild(i).gameObject);
                        }
                        _skillOrItemPanel.GetComponent<CanvasGroup>().interactable = false;
                        _skillOrItemPanel.gameObject.SetActive(false);
                        _commandPanel.GetComponent<CanvasGroup>().interactable = true;
                        EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                        _currentCommand = CommandMode.SelectCommand;
                        Destroy(_currentSelecterObject);
                        _selecterNum = 0;
                        _isAttack = false;
                    }
                    else if (_currentCommand == CommandMode.SelectSkillAttackTarget)
                    {
                        _skillOrItemPanel.GetComponent<CanvasGroup>().interactable = true;
                        EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                        _currentCommand = CommandMode.SelectSkill;
                        Destroy(_currentSelecterObject);
                        _selecterNum = 0;
                        _isAttack = false;
                    }
                    else if (_currentCommand == CommandMode.SelectUseSkillOnAlliesTarget)
                    {
                        _skillOrItemPanel.GetComponent<CanvasGroup>().interactable = true;
                        EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                        _currentCommand = CommandMode.SelectSkill;
                        Destroy(_currentSelecterObject);
                        _selecterNum = 0;
                        _isAttack = false;
                    }
                    else if (_currentCommand == CommandMode.SelectItem)
                    {
                        // magicOrItemPanelにボタンがあれば全て削除
                        for (int i = _skillOrItemPanelContent.transform.childCount - 1; i >= 0; i--)
                        {
                            Destroy(_skillOrItemPanelContent.transform.GetChild(i).gameObject);
                        }
                        _skillOrItemPanel.GetComponent<CanvasGroup>().interactable = false;
                        _skillOrItemPanel.gameObject.SetActive(false);
                        _commandPanel.GetComponent<CanvasGroup>().interactable = true;
                        EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                        _currentCommand = CommandMode.SelectCommand;
                    }
                    else if (_currentCommand == CommandMode.SelectRecoveryItemTarget)
                    {
                        _skillOrItemPanel.GetComponent<CanvasGroup>().interactable = true;
                        EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                        _currentCommand = CommandMode.SelectItem;
                    }
                }

                if (_currentCommand == CommandMode.SelectDirectAttack || _currentCommand == CommandMode.SelectSkillAttackTarget || _currentCommand == CommandMode.SelectUseSkillOnAlliesTarget)
                {
                    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D))
                    {
                        if (_selecterNum == 0)
                        {
                            _selecterNum = _enemyCharacterInBattleList.Count - 1;
                        }
                        else
                        {
                            _selecterNum -= 1;
                        }

                        Destroy(_currentSelecterObject);
                        _selectedBattleCharacter = _enemyCharacterInBattleList[_selecterNum];
                        _currentSelecterObject = Instantiate(_battleCharacterSelecter, _selectedBattleCharacter.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                    }
                    else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A))
                    {
                        if (_selecterNum == _enemyCharacterInBattleList.Count - 1)
                        {
                            _selecterNum = 0;
                        }
                        else
                        {
                            _selecterNum += 1;
                        }

                        Destroy(_currentSelecterObject);
                        _selectedBattleCharacter = _enemyCharacterInBattleList[_selecterNum];
                        _currentSelecterObject = Instantiate(_battleCharacterSelecter, _selectedBattleCharacter.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                    }

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        if (_isAttack == false)
                        {
                            _isAttack = true;
                            return;
                        }

                        if (_currentCommand == CommandMode.SelectDirectAttack)
                        {
                            DirectAttack(_attackCharacter, _selectedBattleCharacter);
                            Destroy(_currentSelecterObject);
                            _selecterNum = 0;
                            _isAttack = false;
                        }
                        else if (_currentCommand == CommandMode.SelectUseSkillOnAlliesTarget)
                        {
                            if (_activeSkill == null)
                            {
                                return;
                            }
                            UseSkill(_attackCharacter, _selectedBattleCharacter, _activeSkill);
                            Destroy(_currentSelecterObject);
                            _selecterNum = 0;
                            _isAttack = false;
                        }
                    }
                }
            }
        }
        else
        {
            //　戦闘前の待機
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= _waitTime)
            {
                //　2回目以降はキャラ間の時間を設定
                _waitTime = _timeToNextCharacter;
                //　最初のキャラクターの待ち時間は0にする為にあらかじめ条件をクリアさせておく
                _elapsedTime = _timeToNextCharacter;
                _isStartBattle = true;
            }
        }
    }

    //　キャラクターの攻撃の選択処理
    public void MakeAttackChoise(GameObject character)
    {
        //　キャラのターン開始時に状態異常処理
        CharacterBattleScript characterBattleScript = _allCharacterInBattleList[_currentAttackOrder].GetComponent<CharacterBattleScript>();
        CharacterStatus characterStatus = characterBattleScript.CharacterStatus;
        bool skipCommand = false;
        if (characterBattleScript.IsPoison == true)
        {
            if (characterBattleScript.PoisonTurn > 0)
            {
                if (characterBattleScript.PoisonDamage1 == false)
                {
                    characterBattleScript.PoisonDamage();
                    characterBattleScript.PoisonTurn--;
                    characterBattleScript.PoisonDamage1 = true;
                }
            }
            else
            {
                characterBattleScript.IsPoison = false;
                characterStatus.IsPoisonStatus = false;
            }
        }
        if (characterBattleScript.IsSleep)
        {
            skipCommand = true;
            ChangeNextChara();
        }
        if (characterBattleScript.IsParalysis)
        {
            if (characterBattleScript.ParalysisTurn > 0)
            {
                skipCommand = true;
                ChangeNextChara();
                characterBattleScript.ParalysisTurn--;
            }
            else
            {
                characterBattleScript.IsParalysis = false;
                characterStatus.IsParalysisStatus = false;
            }
        }
        if (characterBattleScript.IsConfusion)
        {
            skipCommand = true;
            int rand = Random.Range(0, _allCharacterInBattleList.Count - 1);
            GameObject attackTarget = _allCharacterInBattleList[rand];
            DirectAttack(_allCharacterInBattleList[_currentAttackOrder], attackTarget);
        }

        if (skipCommand == true)
        {
            return;
        }

        CharacterStatus characterStatusScript = character.GetComponent<CharacterBattleScript>().CharacterStatus;
        //　EnemyStatusにキャスト出来る場合は敵の攻撃処理
        if (characterStatusScript as EnemyStatus != null)
        {
            Debug.Log(character.gameObject.name + "の攻撃");
            EnemyAttack(character);
        }
        else
        {
            Debug.Log(characterStatusScript.CharacterName + "の攻撃");
            PlayerAttack(character);
        }
    }

    //　プレイヤーの攻撃
    public void PlayerAttack(GameObject character)
    {
        _currentCommand = CommandMode.SelectCommand;

        // 魔法やアイテムパネルの子要素のContentにボタンがあれば全て削除
        for (int i = _skillOrItemPanelContent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_skillOrItemPanelContent.transform.GetChild(i).gameObject);
        }

        _commandPanel.GetComponent<CanvasGroup>().interactable = true;
        _skillOrItemPanel.GetComponent<CanvasGroup>().interactable = false;

        //　キャラクターがガード状態であればガードを解く
        if (character.GetComponent<Animator>().GetBool("Guard"))
        {
            character.GetComponent<CharacterBattleScript>().UnlockGuard();
        }

        var characterSkill = character.GetComponent<CharacterBattleScript>().CharacterStatus.SkillList;
        //　持っているスキルに応じてコマンドボタンの表示
        if (characterSkill.Exists(skill => skill.SkillType == Skill.Type.DirectAttack))
        {
            var directAttackButton = _directAttackButtonObj.GetComponent<Button>();
            directAttackButton.onClick.RemoveAllListeners();
            _directAttackButtonObj.GetComponent<Button>().onClick.AddListener(() => SelectDirectAttacker(character));
            _directAttackButtonObj.SetActive(true);
        }
        else
        {
            _directAttackButtonObj.SetActive(false);
        }
        if (characterSkill.Exists(skill => skill.SkillType == Skill.Type.Item))
        {
            var itemButton = _itemButtonObj.GetComponent<Button>();
            itemButton.onClick.RemoveAllListeners();
            itemButton.onClick.AddListener(() => SelectItem(character));
            _itemButtonObj.SetActive(true);
        }
        else
        {
            _itemButtonObj.SetActive(false);
        }
        if (characterSkill.Exists(skill => skill.SkillType == Skill.Type.MagicAttack)
            || characterSkill.Find(skill => skill.SkillType == Skill.Type.IncreaseAttackPowerMagic)
            || characterSkill.Find(skill => skill.SkillType == Skill.Type.IncreaseDefensePowerMagic)
            || characterSkill.Find(skill => skill.SkillType == Skill.Type.PoisonRecoveryMagic)
            || characterSkill.Find(skill => skill.SkillType == Skill.Type.SleepRecoveryMagic)
            || characterSkill.Find(skill => skill.SkillType == Skill.Type.ParalysisRecoveryMagic)
            || characterSkill.Find(skill => skill.SkillType == Skill.Type.ConfusionRecoveryMagic)
            || characterSkill.Find(skill => skill.SkillType == Skill.Type.DepressionRecoveryMagic)
            || characterSkill.Find(skill => skill.SkillType == Skill.Type.RecoveryMagic))
        {
            var magicButton = _skillButtonObj.GetComponent<Button>();
            magicButton.onClick.RemoveAllListeners();
            magicButton.onClick.AddListener(() => SelectMagic(character));

            _skillButtonObj.SetActive(true);
        }
        else
        {
            _skillButtonObj.SetActive(false);
        }
        if (characterSkill.Exists(skill => skill.SkillType == Skill.Type.GetAway))
        {
            var getAwayButton = _getAwayButtonObj.GetComponent<Button>();
            getAwayButton.onClick.RemoveAllListeners();
            getAwayButton.onClick.AddListener(() => GetAway(character));
            _getAwayButtonObj.SetActive(true);
        }
        else
        {
            _getAwayButtonObj.SetActive(false);
        }

        EventSystem.current.SetSelectedGameObject(_commandPanel.transform.GetChild(1).gameObject);
        _commandPanel.gameObject.SetActive(true);
    }

    //　敵の攻撃処理
    public void EnemyAttack(GameObject character)
    {
        CharacterBattleScript characterBattleScript = character.GetComponent<CharacterBattleScript>();
        CharacterStatus characterStatus = characterBattleScript.CharacterStatus;

        if (characterStatus.SkillList.Count <= 0)
        {
            return;
        }
        //　敵がガード状態であればガードを解く
        //if (character.GetComponent<Animator>().GetBool("Guard"))
        //{
        //    character.GetComponent<CharacterBattleScript>().UnlockGuard();
        //}

        //　敵の行動アルゴリズム
        int randomValue = (int)(Random.value * characterStatus.SkillList.Count);
        var nowSkill = characterStatus.SkillList[randomValue];

        //　テスト用（特定のスキルで確認）
        //nowSkill = characterStatus.GetSkillList()[0];

        if (nowSkill.SkillType == Skill.Type.DirectAttack)
        {
            var targetNum = (int)(Random.value * _allyCharacterInBattleList.Count);
            //　攻撃相手のCharacterBattleScript
            characterBattleScript.ChooseAttackOptions(CharacterBattleScript.BattleState.DirectAttack, _allyCharacterInBattleList[targetNum], nowSkill);
            Debug.Log(character.name + "は" + nowSkill.Name + "を行った");
        }
        else if (nowSkill.SkillType == Skill.Type.MagicAttack)
        {
            var targetNum = (int)(Random.value * _allyCharacterInBattleList.Count);
            if (characterBattleScript.Mp >= ((Magic)nowSkill).MagicPoints)
            {
                //　攻撃相手のCharacterBattleScript
                characterBattleScript.ChooseAttackOptions(CharacterBattleScript.BattleState.MagicAttack, _allyCharacterInBattleList[targetNum], nowSkill);
                Debug.Log(character.name + "は" + nowSkill.Name + "を行った");
            }
            else
            {
                Debug.Log("MPが足りない！");
                //　MPが足りない場合は直接攻撃を行う
                characterBattleScript.ChooseAttackOptions(CharacterBattleScript.BattleState.DirectAttack, _allyCharacterInBattleList[targetNum], characterStatus.SkillList.Find(skill => skill.SkillType == Skill.Type.DirectAttack));
                Debug.Log(character.name + "は攻撃を行った");
            }
        }
        else if (nowSkill.SkillType == Skill.Type.RecoveryMagic)
        {
            if (characterBattleScript.Mp >= ((Magic)nowSkill).MagicPoints)
            {
                var targetNum = (int)(Random.value * _enemyCharacterInBattleList.Count);
                //　回復相手のCharacterBattleScript
                characterBattleScript.ChooseAttackOptions(CharacterBattleScript.BattleState.Heal, _enemyCharacterInBattleList[targetNum], nowSkill);
                Debug.Log(character.name + "は" + nowSkill.Name + "を行った");
            }
            else
            {
                Debug.Log("MPが足りない！");
                var targetNum = (int)(Random.value * _allyCharacterInBattleList.Count);
                //　MPが足りない場合は直接攻撃を行う
                characterBattleScript.ChooseAttackOptions(CharacterBattleScript.BattleState.DirectAttack, _allyCharacterInBattleList[targetNum], characterStatus.SkillList.Find(skill => skill.SkillType == Skill.Type.DirectAttack));
                Debug.Log(character.name + "は攻撃を行った");
            }
        }
        else if (nowSkill.SkillType == Skill.Type.Guard)
        {
            characterBattleScript.Guard();
            // Guardアニメはboolなのでアニメーション遷移させたらすぐに次のキャラクターに移行させる
            ChangeNextChara();
            Debug.Log(character.name + "は" + nowSkill.Name + "を行った");
        }
    }

    //　次のキャラクターに移行
    public void ChangeNextChara()
    {
        _isChoosing = false;
    }

    public void DeleteAllCharacterInBattleList(GameObject deleteObj)
    {
        var deleteObjNum = _allCharacterInBattleList.IndexOf(deleteObj);
        _allCharacterInBattleList.Remove(deleteObj);
        if (deleteObjNum < _currentAttackOrder)
        {
            _currentAttackOrder--;
        }
        //　全員攻撃が終わったら最初から
        if (_currentAttackOrder >= _allCharacterInBattleList.Count)
        {
            _currentAttackOrder = 0;
        }
    }

    public void DeletePlayerCharacterInBattleList(GameObject deleteObj)
    {
        _allyCharacterInBattleList.Remove(deleteObj);
        if (_allyCharacterInBattleList.Count == 0)
        {
            Debug.Log("味方が全滅");
            _battleIsOver = true;
            CharacterBattleScript characterBattleScript;
            foreach (var character in _allyCharacterInBattleList)
            {
                //　味方キャラクターの戦闘で増減したHPとMPを通常のステータスに反映させる
                characterBattleScript = character.GetComponent<CharacterBattleScript>();
                if (characterBattleScript.CharacterStatus != null)
                {
                    characterBattleScript.CharacterStatus.Hp = characterBattleScript.Hp;
                    characterBattleScript.CharacterStatus.Mp = characterBattleScript.Mp;
                    characterBattleScript.CharacterStatus.Mot = characterBattleScript.Mot;
                    characterBattleScript.CharacterStatus.IsPoisonStatus = characterBattleScript.IsPoison;
                    characterBattleScript.CharacterStatus.IsSleepStatus = characterBattleScript.IsSleep;
                    characterBattleScript.CharacterStatus.IsParalysisStatus = characterBattleScript.IsParalysis;
                    characterBattleScript.CharacterStatus.IsConfusionStatus = characterBattleScript.IsConfusion;
                    characterBattleScript.CharacterStatus.IsDepressionStatus = characterBattleScript.IsDepression;
                }
            }
            //　敗戦時の結果表示
            _battleResult.InitialProcessingOfDefeatResult();
        }
    }

    public void DeleteEnemyCharacterInBattleList(GameObject deleteObj)
    {
        _enemyCharacterInBattleList.Remove(deleteObj);
        if (_enemyCharacterInBattleList.Count == 0)
        {
            Debug.Log("敵が全滅");
            _battleIsOver = true;
            CharacterBattleScript characterBattleScript;
            foreach (var character in _allyCharacterInBattleList)
            {
                //　味方キャラクターの戦闘で増減したHPとMPを通常のステータスに反映させる
                characterBattleScript = character.GetComponent<CharacterBattleScript>();
                if (characterBattleScript.CharacterStatus != null)
                {
                    characterBattleScript.CharacterStatus.Hp = characterBattleScript.Hp;
                    characterBattleScript.CharacterStatus.Mp = characterBattleScript.Mp;
                    characterBattleScript.CharacterStatus.Mot = characterBattleScript.Mot;
                    characterBattleScript.CharacterStatus.IsPoisonStatus = characterBattleScript.IsPoison;
                    characterBattleScript.CharacterStatus.IsSleepStatus = characterBattleScript.IsSleep;
                    characterBattleScript.CharacterStatus.IsParalysisStatus = characterBattleScript.IsParalysis;
                    characterBattleScript.CharacterStatus.IsConfusionStatus = characterBattleScript.IsConfusion;
                    characterBattleScript.CharacterStatus.IsDepressionStatus = characterBattleScript.IsDepression;
                }
            }
            //　勝利時の結果表示
            _battleResult.InitialProcessingOfVictoryResult(_allCharacterList, _allyCharacterInBattleList[0]);
        }
    }

    //　キャラクター選択
    public void SelectDirectAttacker(GameObject attackCharacter)
    {
        _currentCommand = CommandMode.SelectDirectAttack;
        _commandPanel.GetComponent<CanvasGroup>().interactable = false;
        selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

        _selectedBattleCharacter = _enemyCharacterInBattleList[_selecterNum];
        _attackCharacter = attackCharacter;
        _currentSelecterObject = Instantiate(_battleCharacterSelecter, _selectedBattleCharacter.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
    }

    //　直接攻撃
    public void DirectAttack(GameObject attackCharacter, GameObject attackTarget)
    {
        //　攻撃するキャラのDirectAttackスキルを取得する
        var characterSkill = attackCharacter.GetComponent<CharacterBattleScript>().CharacterStatus.SkillList;
        Skill directAtatck = characterSkill.Find(skill => skill.SkillType == Skill.Type.DirectAttack);
        attackCharacter.GetComponent<CharacterBattleScript>().ChooseAttackOptions(CharacterBattleScript.BattleState.DirectAttack, attackTarget, directAtatck);
        _commandPanel.gameObject.SetActive(false);
    }

    //　防御
    public void Guard(GameObject guardCharacter)
    {
        guardCharacter.GetComponent<CharacterBattleScript>().Guard();
        _commandPanel.gameObject.SetActive(false);
        ChangeNextChara();
    }

    //　使用する魔法の選択
    public void SelectMagic(GameObject character)
    {
        _currentCommand = CommandMode.SelectSkill;
        _commandPanel.GetComponent<CanvasGroup>().interactable = false;
        selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

        GameObject battleMagicPanelButtonIns;
        var skillList = character.GetComponent<CharacterBattleScript>().CharacterStatus.SkillList;

        //　MagicOrItemPanelのスクロール値の初期化
        //scrollManager.Reset();
        //int battleMagicPanelButtonNum = 0;

        foreach (var skill in skillList)
        {
            if (skill.SkillType == Skill.Type.MagicAttack
                || skill.SkillType == Skill.Type.RecoveryMagic
                || skill.SkillType == Skill.Type.IncreaseAttackPowerMagic
                || skill.SkillType == Skill.Type.IncreaseDefensePowerMagic
                || skill.SkillType == Skill.Type.PoisonRecoveryMagic
                || skill.SkillType == Skill.Type.SleepRecoveryMagic
                || skill.SkillType == Skill.Type.ParalysisRecoveryMagic
                || skill.SkillType == Skill.Type.ConfusionRecoveryMagic
                || skill.SkillType == Skill.Type.DepressionRecoveryMagic
                )
            {
                battleMagicPanelButtonIns = Instantiate<GameObject>(_battleSkillPanelButton, _skillOrItemPanelContent);
                battleMagicPanelButtonIns.transform.Find("SkillName").GetComponent<TextMeshProUGUI>().text = skill.Name;
                battleMagicPanelButtonIns.transform.Find("AmountToUseSkillPoints").GetComponent<TextMeshProUGUI>().text = ((Magic)skill).MagicPoints.ToString();

                ////　指定した番号のアイテムパネルボタンにアイテムスクロール用スクリプトを取り付ける
                //if (battleMagicPanelButtonNum != 0
                //    && (battleMagicPanelButtonNum % scrollDownButtonNum == 0
                //    || battleMagicPanelButtonNum % (scrollDownButtonNum + 1) == 0)
                //    )
                //{
                //    //　アイテムスクロールスクリプトの取り付けて設定値のセット
                //    battleMagicPanelButtonIns.AddComponent<ScrollDownScript>();
                //}
                //else if (battleMagicPanelButtonNum != 0
                //  && (battleMagicPanelButtonNum % scrollUpButtonNum == 0
                //  || battleMagicPanelButtonNum % (scrollUpButtonNum + 1) == 0)
                //  )
                //{
                //    battleMagicPanelButtonIns.AddComponent<ScrollUpScript>();
                //}

                //　MPが足りない時はボタンを押しても何もせず魔法の名前を暗くする
                if (character.GetComponent<CharacterBattleScript>().Mp < ((Magic)skill).MagicPoints)
                {
                    battleMagicPanelButtonIns.transform.Find("MagicName").GetComponent<Text>().color = new Color(0.4f, 0.4f, 0.4f);
                }
                else
                {
                    battleMagicPanelButtonIns.GetComponent<Button>().onClick.AddListener(() => SelectUseSkillTarget(character, skill));
                }
                //　ボタン番号を足す
                //battleMagicPanelButtonNum++;

                //if (battleMagicPanelButtonNum == scrollUpButtonNum + 2)
                //{
                //    battleMagicPanelButtonNum = 2;
                //}
            }
        }

        _skillOrItemPanel.GetComponent<CanvasGroup>().interactable = true;
        EventSystem.current.SetSelectedGameObject(_skillOrItemPanelContent.GetChild(0).gameObject);
        _skillOrItemPanel.gameObject.SetActive(true);

    }

    //　魔法を使う相手の選択
    public void SelectUseSkillTarget(GameObject user, Skill skill)
    {
        _currentCommand = CommandMode.SelectUseSkillOnAlliesTarget;
        _skillOrItemPanel.GetComponent<CanvasGroup>().interactable = false;
        selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

        if (skill.SkillType == Skill.Type.MagicAttack)
        {
            _selectedBattleCharacter = _enemyCharacterInBattleList[_selecterNum];
            _attackCharacter = user;
            _activeSkill = skill;
            _currentSelecterObject = Instantiate(_battleCharacterSelecter, _selectedBattleCharacter.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        }
        else
        {
            UseSkill(user, _allyCharacterInBattleList[0], skill);
        }
    }

    //　魔法を使う
    public void UseSkill(GameObject user, GameObject targetCharacter, Skill skill)
    {
        CharacterBattleScript.BattleState battleState = CharacterBattleScript.BattleState.Idle;
        //　魔法を使う相手のCharacterBattleScriptを取得しておく
        var targetCharacterBattleScript = targetCharacter.GetComponent<CharacterBattleScript>();

        //　使う魔法の種類の設定と対象に使う必要がない場合の処理
        if (skill.SkillType == Skill.Type.MagicAttack)
        {
            battleState = CharacterBattleScript.BattleState.MagicAttack;
        }
        else if (skill.SkillType == Skill.Type.RecoveryMagic)
        {
            if (targetCharacterBattleScript.Hp == targetCharacterBattleScript.CharacterStatus.MaxHp)
            {
                Debug.Log(targetCharacter.name + "は全快です。");
                _skillOrItemPanel.GetComponent<CanvasGroup>().interactable = true;
                EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                _currentCommand = CommandMode.SelectSkill;
                return;
            }
            battleState = CharacterBattleScript.BattleState.Heal;
        }
        else if (skill.SkillType == Skill.Type.IncreaseAttackPowerMagic)
        {
            battleState = CharacterBattleScript.BattleState.IncreaseAttackPowerMagic;
        }
        else if (skill.SkillType == Skill.Type.IncreaseDefensePowerMagic)
        {
            battleState = CharacterBattleScript.BattleState.IncreaseDefensePowerMagic;
        }
        else if (skill.SkillType == Skill.Type.PoisonRecoveryMagic)
        {
            if (!targetCharacterBattleScript.IsPoison)
            {
                Debug.Log(targetCharacter.name + "は毒状態ではありません。");
                return;
            }
            battleState = CharacterBattleScript.BattleState.PoisonRecoveryMagic;
        }
        else if (skill.SkillType == Skill.Type.SleepRecoveryMagic)
        {
            if (!targetCharacterBattleScript.IsSleep)
            {
                Debug.Log(targetCharacter.name + "は昏睡状態ではありません。");
                return;
            }
            battleState = CharacterBattleScript.BattleState.SleepRecoveryMagic;
        }
        else if (skill.SkillType == Skill.Type.ParalysisRecoveryMagic)
        {
            if (!targetCharacterBattleScript.IsParalysis)
            {
                Debug.Log(targetCharacter.name + "は麻痺状態ではありません。");
                return;
            }
            battleState = CharacterBattleScript.BattleState.ParalysisRecoveryMagic;
        }
        else if (skill.SkillType == Skill.Type.ConfusionRecoveryMagic)
        {
            if (!targetCharacterBattleScript.IsConfusion)
            {
                Debug.Log(targetCharacter.name + "は混乱状態ではありません。");
                return;
            }
            battleState = CharacterBattleScript.BattleState.ConfusionRecoveryMagic;
        }
        else if (skill.SkillType == Skill.Type.DepressionRecoveryMagic)
        {
            if (!targetCharacterBattleScript.IsDepression)
            {
                Debug.Log(targetCharacter.name + "は鬱状態ではありません。");
                return;
            }
            battleState = CharacterBattleScript.BattleState.DepressionRecoveryMagic;
        }
        user.GetComponent<CharacterBattleScript>().ChooseAttackOptions(battleState, targetCharacter, skill);
        _commandPanel.gameObject.SetActive(false);
        _skillOrItemPanel.gameObject.SetActive(false);
        //selectCharacterPanel.gameObject.SetActive(false);
    }

    //　使用するアイテムの選択
    public void SelectItem(GameObject character)
    {

        var itemDictionary = ((PlayerStatus)character.GetComponent<CharacterBattleScript>().CharacterStatus).ItemDictionary;

        //　MagicOrItemPanelのスクロール値の初期化
        //scrollManager.Reset();
        //var battleItemPanelButtonNum = 0;

        GameObject battleItemPanelButtonIns;

        foreach (var item in itemDictionary.Keys)
        {
            if (item.ItemType == Item.Type.HPRecovery
                || item.ItemType == Item.Type.MPRecovery
                || item.ItemType == Item.Type.MotRecovery
                || item.ItemType == Item.Type.PoisonRecovery
                || item.ItemType == Item.Type.SleepRecovery
                || item.ItemType == Item.Type.ParalysisRecovery
                || item.ItemType == Item.Type.ConfusionRecovery
                || item.ItemType == Item.Type.DepressionRecovery
                || item.ItemType == Item.Type.PowerUp
                || item.ItemType == Item.Type.DefenseUp
                )
            {
                battleItemPanelButtonIns = Instantiate<GameObject>(_battleItemPanelButton, _skillOrItemPanelContent);
                battleItemPanelButtonIns.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = item.ItemName;
                battleItemPanelButtonIns.transform.Find("Num").GetComponent<TextMeshProUGUI>().text = ((PlayerStatus)character.GetComponent<CharacterBattleScript>().CharacterStatus).ItemDictionary[item].ToString();
                battleItemPanelButtonIns.GetComponent<Button>().onClick.AddListener(() => SelectItemTarget(character, item));

                //　指定した番号のアイテムパネルボタンにアイテムスクロール用スクリプトを取り付ける
                //if (battleItemPanelButtonNum != 0
                //    && (battleItemPanelButtonNum % scrollDownButtonNum == 0
                //    || battleItemPanelButtonNum % (scrollDownButtonNum + 1) == 0)
                //    )
                //{
                //    //　アイテムスクロールスクリプトの取り付けて設定値のセット
                //    battleItemPanelButtonIns.AddComponent<ScrollDownScript>();
                //}
                //else if (battleItemPanelButtonNum != 0
                //  && (battleItemPanelButtonNum % scrollUpButtonNum == 0
                //  || battleItemPanelButtonNum % (scrollUpButtonNum + 1) == 0)
                //  )
                //{
                //    battleItemPanelButtonIns.AddComponent<ScrollUpScript>();
                //}
                ////　ボタン番号を足す
                //battleItemPanelButtonNum++;

                //if (battleItemPanelButtonNum == scrollUpButtonNum + 2)
                //{
                //    battleItemPanelButtonNum = 2;
                //}
            }
        }

        if (_skillOrItemPanelContent.childCount > 0)
        {
            _currentCommand = CommandMode.SelectItem;
            _commandPanel.GetComponent<CanvasGroup>().interactable = false;
            selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            _skillOrItemPanel.GetComponent<CanvasGroup>().interactable = true;
            EventSystem.current.SetSelectedGameObject(_skillOrItemPanelContent.GetChild(0).gameObject);
            _skillOrItemPanel.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("使えるアイテムがありません。");
        }
    }

    //　アイテムを使用する相手を選択
    public void SelectItemTarget(GameObject user, Item item)
    {
        _currentCommand = CommandMode.SelectRecoveryItemTarget;
        _skillOrItemPanel.GetComponent<CanvasGroup>().interactable = false;
        selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

        //GameObject battleCharacterButtonIns;

        //foreach (var allyCharacter in _allyCharacterInBattleList)
        //{
        //    battleCharacterButtonIns = Instantiate<GameObject>(battleCharacterButton, selectCharacterPanel);
        //    battleCharacterButtonIns.transform.Find("Text").GetComponent<Text>().text = allyCharacter.gameObject.name;
        //    battleCharacterButtonIns.GetComponent<Button>().onClick.AddListener(() => UseItem(user, allyCharacter, item));
        //}
        UseItem(user, _allyCharacterInBattleList[0], item);

        //selectCharacterPanel.GetComponent<CanvasGroup>().interactable = true;
        //EventSystem.current.SetSelectedGameObject(selectCharacterPanel.GetChild(0).gameObject);
        //selectCharacterPanel.gameObject.SetActive(true);
    }

    //　アイテム使用
    public void UseItem(GameObject user, GameObject targetCharacter, Item item)
    {
        var userCharacterBattleScript = user.GetComponent<CharacterBattleScript>();
        var targetCharacterBattleScript = targetCharacter.GetComponent<CharacterBattleScript>();
        var skill = userCharacterBattleScript.CharacterStatus.SkillList.Find(skills => skills.SkillType == Skill.Type.Item);

        CharacterBattleScript.BattleState battleState = CharacterBattleScript.BattleState.Idle;

        if (item.ItemType == Item.Type.HPRecovery)
        {
            if (targetCharacterBattleScript.Hp == targetCharacterBattleScript.CharacterStatus.MaxHp)
            {
                Debug.Log(targetCharacter.name + "は全快です。");
                return;
            }
            battleState = CharacterBattleScript.BattleState.UseHPRecoveryItem;
        }
        else if (item.ItemType == Item.Type.MPRecovery)
        {
            if (targetCharacterBattleScript.Mp == targetCharacterBattleScript.CharacterStatus.MaxMp)
            {
                Debug.Log(targetCharacter.name + "はMP回復をする必要がありません。");
                return;
            }
            battleState = CharacterBattleScript.BattleState.UseMPRecoveryItem;
        }
        else if (item.ItemType == Item.Type.MotRecovery)
        {
            if (targetCharacterBattleScript.Mot == targetCharacterBattleScript.CharacterStatus.MaxMot)
            {
                Debug.Log(targetCharacter.name + "はMot回復をする必要がありません。");
                return;
            }
            battleState = CharacterBattleScript.BattleState.UseMotRecoveryItem;
        }
        else if (item.ItemType == Item.Type.PoisonRecovery)
        {
            if (!targetCharacterBattleScript.IsPoison)
            {
                Debug.Log(targetCharacter.name + "は毒状態ではありません。");
                return;
            }
            battleState = CharacterBattleScript.BattleState.UsePoisonRecoveryItem;
        }
        else if (item.ItemType == Item.Type.SleepRecovery)
        {
            if (!targetCharacterBattleScript.IsSleep)
            {
                Debug.Log(targetCharacter.name + "は昏睡状態ではありません。");
                return;
            }
            battleState = CharacterBattleScript.BattleState.UseSleepRecoveryItem;
        }
        else if (item.ItemType == Item.Type.ParalysisRecovery)
        {
            if (!targetCharacterBattleScript.IsParalysis)
            {
                Debug.Log(targetCharacter.name + "は麻痺状態ではありません。");
                return;
            }
            battleState = CharacterBattleScript.BattleState.UseParalysisRecoveryItem;
        }
        else if (item.ItemType == Item.Type.ConfusionRecovery)
        {
            if (!targetCharacterBattleScript.IsConfusion)
            {
                Debug.Log(targetCharacter.name + "は混乱状態ではありません。");
                return;
            }
            battleState = CharacterBattleScript.BattleState.UseConfusionRecoveryItem;
        }
        else if (item.ItemType == Item.Type.DepressionRecovery)
        {
            if (!targetCharacterBattleScript.IsDepression)
            {
                Debug.Log(targetCharacter.name + "は鬱状態ではありません。");
                return;
            }
            battleState = CharacterBattleScript.BattleState.UseDepressionRecoveryItem;
        }
        else if (item.ItemType == Item.Type.PowerUp)
        {
            //if (targetCharacterBattleScript.IsIncreasePower)
            //{
            //    Debug.Log("既に攻撃力を上げています。");
            //    return;
            //}
            battleState = CharacterBattleScript.BattleState.IncreaseAttackAndDecreaseDefense;
        }
        else if (item.ItemType == Item.Type.DefenseUp)
        {
            //if (targetCharacterBattleScript.IsIncreaseDefense)
            //{
            //    Debug.Log("既に防御力を上げています。");
            //    return;
            //}
            battleState = CharacterBattleScript.BattleState.IncreaseDefenseAndDecreaseAttack;
        }
        userCharacterBattleScript.ChooseAttackOptions(battleState, targetCharacter, skill, item);
        _commandPanel.gameObject.SetActive(false);
        _skillOrItemPanel.gameObject.SetActive(false);
    }

    //　逃げる
    public void GetAway(GameObject character)
    {
        var randomValue = Random.value;
        if (0f <= randomValue && randomValue <= 0.2f)
        {
            Debug.Log("逃げるのに成功した。");
            _battleIsOver = true;
            _commandPanel.gameObject.SetActive(false);
            //　戦闘終了
            _battleResult.InitialProcessingOfRanAwayResult();
        }
        else
        {
            Debug.Log("逃げるのに失敗した。");
            _commandPanel.gameObject.SetActive(false);
            ChangeNextChara();
        }
    }
}
