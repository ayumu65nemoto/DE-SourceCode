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

    //�@�퓬�f�[�^
    [SerializeField]
    private BattleData _battleData;
    //�@�L�����N�^�[�̃x�[�X�ʒu
    [SerializeField]
    private Transform _battleBasePosition;
    //�@���ݐ퓬�ɎQ�����Ă���L�����N�^�[
    private List<GameObject> _allCharacterList = new List<GameObject>();

    //�@���ݐ퓬�ɎQ�����Ă���S�L�����N�^�[
    private List<GameObject> _allCharacterInBattleList = new List<GameObject>();
    //�@���ݐ퓬�ɎQ�����Ă��閡���L�����N�^�[
    private List<GameObject> _allyCharacterInBattleList = new List<GameObject>();
    //�@���ݐ퓬�ɎQ�����Ă���G�L�����N�^�[
    private List<GameObject> _enemyCharacterInBattleList = new List<GameObject>();
    //�@���݂̍U���̏���
    private int _currentAttackOrder;
    //�@���ݍU�������悤�Ƃ��Ă���l���I��
    private bool _isChoosing;
    //�@�퓬���J�n���Ă��邩�ǂ���
    private bool _isStartBattle;
    //�@�퓬�V�[���̍ŏ��̍U�����n�܂�܂ł̑ҋ@����
    [SerializeField]
    private float _firstWaitingTime = 3f;
    //�@�퓬�V�[���̃L�����ڍs���̊Ԃ̎���
    [SerializeField]
    private float _timeToNextCharacter = 1f;
    //�@�҂�����
    private float _waitTime;
    //�@�퓬�V�[���̍ŏ��̍U�����n�܂�܂ł̌o�ߎ���
    private float _elapsedTime;
    //�@�퓬���I���������ǂ���
    private bool _battleIsOver;
    //�@���݂̃R�}���h
    [SerializeField]
    private CommandMode _currentCommand;

    //�@�����p�[�e�B�[�̃R�}���h�p�l��
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

    //�@�퓬�p�L�����N�^�[�I���A�C�R���v���n�u
    [SerializeField]
    private GameObject _battleCharacterSelecter = null;
    //�@���ݕ\������Ă���L�����N�^�[�I���A�C�R��
    [SerializeField]
    private GameObject _currentSelecterObject = null;
    //�@�I������Ă���퓬�p�L�����N�^�[
    [SerializeField]
    private GameObject _selectedBattleCharacter = null;
    //�@���Ԗڂ̃L�����N�^�[���i�[����Ă��邩
    [SerializeField]
    private int _selecterNum = 0;
    //�@�U������L�����N�^�[
    [SerializeField]
    private GameObject _attackCharacter = null;
    //�@��������X�L��
    [SerializeField]
    private Skill _activeSkill = null;
    //�@�U���{�^���P��X���[
    private bool _isAttack = false;

    //�@���@��A�C�e���I���p�l��
    [SerializeField]
    private Transform _skillOrItemPanel = null;
    //�@���@��A�C�e���I���p�l����Content
    [SerializeField]
    private Transform _skillOrItemPanelContent = null;
    //�@BattleItemPanelButton�v���n�u
    [SerializeField]
    private GameObject _battleItemPanelButton = null;
    //�@BattleMagicPanelButton�v���n�u
    [SerializeField]
    private GameObject _battleSkillPanelButton = null;
    //�@�Ō�ɑI�����Ă����Q�[���I�u�W�F�N�g���X�^�b�N
    private Stack<GameObject> selectedGameObjectStack = new Stack<GameObject>();

    //�@���ʕ\�������X�N���v�g
    [SerializeField]
    private BattleResult _battleResult;

    //�@�v���C���[�L�����N�^�[��z�u����Transform
    [SerializeField]
    private Transform _characterTransform;
    //�@�G�l�~�[�L�����N�^�[��z�u����Transform
    [SerializeField]
    private List<Transform> _enemyTransform = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        //�@�L�����N�^�[�C���X�^���X�̐e
        Transform characterParent = new GameObject("Character").transform;
        //�@�������O�̓G�������ꍇ�̏����Ɏg�����X�g
        List<string> enemyNameList = new List<string>();

        GameObject ins;
        CharacterBattleScript characterBattleScript;
        string characterName;

        //�@�����̃v���n�u���C���X�^���X��
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
        //�@�G�̃v���n�u���C���X�^���X��
        for (int i = 0; i < _battleData.EnemyPartyStatus.PartyMembers.Count; i++)
        {
            ins = Instantiate<GameObject>(_battleData.EnemyPartyStatus.PartyMembers[i], _enemyTransform[i].position, _enemyTransform[i].rotation, characterParent);
            //�@���ɓ����G�����݂����當����t������
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
        //�@���݂̐퓬
        _allCharacterInBattleList = _allCharacterList.ToList<GameObject>();
        //�@�퓬�O�̑҂����Ԃ�ݒ�
        _waitTime = _firstWaitingTime;
        //�@�����_���l�̃V�[�h�̐ݒ�
        Random.InitState((int)Time.time);
    }

    private void Update()
    {
        //�@�퓬���I�����Ă����炱��ȍ~�������Ȃ�
        if (_battleIsOver == true)
        {
            return;
        }

        //�@�I���������ꂽ���i�}�E�X��UI�O���N���b�N�����j�͌��݂̃��[�h�ɂ���Ė������I��������
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

        //�@�퓬�J�n
        if (_isStartBattle == true)
        {
            //�@���݂̃L�����N�^�[�̍U�����I����Ă���
            if (_isChoosing == false)
            {
                _elapsedTime += Time.deltaTime;
                if (_elapsedTime < _waitTime)
                {
                    return;
                }
                _elapsedTime = 0f;
                _isChoosing = true;

                //�@�L�����N�^�[�̍U���̑I���Ɉڂ�
                MakeAttackChoise(_allCharacterInBattleList[_currentAttackOrder]);
                //�@���̃L�����N�^�[�̃^�[���ɂ���
                _currentAttackOrder++;
                //�@�S���U�����I�������ŏ�����
                if (_currentAttackOrder >= _allCharacterInBattleList.Count)
                {
                    _currentAttackOrder = 0;
                }
            }
            else
            {
                //�@�L�����Z���{�^�������������̏���
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
                        // magicOrItemPanel�Ƀ{�^��������ΑS�č폜
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
                        // magicOrItemPanel�Ƀ{�^��������ΑS�č폜
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
            //�@�퓬�O�̑ҋ@
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= _waitTime)
            {
                //�@2��ڈȍ~�̓L�����Ԃ̎��Ԃ�ݒ�
                _waitTime = _timeToNextCharacter;
                //�@�ŏ��̃L�����N�^�[�̑҂����Ԃ�0�ɂ���ׂɂ��炩���ߏ������N���A�����Ă���
                _elapsedTime = _timeToNextCharacter;
                _isStartBattle = true;
            }
        }
    }

    //�@�L�����N�^�[�̍U���̑I������
    public void MakeAttackChoise(GameObject character)
    {
        //�@�L�����̃^�[���J�n���ɏ�Ԉُ폈��
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
        //�@EnemyStatus�ɃL���X�g�o����ꍇ�͓G�̍U������
        if (characterStatusScript as EnemyStatus != null)
        {
            Debug.Log(character.gameObject.name + "�̍U��");
            EnemyAttack(character);
        }
        else
        {
            Debug.Log(characterStatusScript.CharacterName + "�̍U��");
            PlayerAttack(character);
        }
    }

    //�@�v���C���[�̍U��
    public void PlayerAttack(GameObject character)
    {
        _currentCommand = CommandMode.SelectCommand;

        // ���@��A�C�e���p�l���̎q�v�f��Content�Ƀ{�^��������ΑS�č폜
        for (int i = _skillOrItemPanelContent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_skillOrItemPanelContent.transform.GetChild(i).gameObject);
        }

        _commandPanel.GetComponent<CanvasGroup>().interactable = true;
        _skillOrItemPanel.GetComponent<CanvasGroup>().interactable = false;

        //�@�L�����N�^�[���K�[�h��Ԃł���΃K�[�h������
        if (character.GetComponent<Animator>().GetBool("Guard"))
        {
            character.GetComponent<CharacterBattleScript>().UnlockGuard();
        }

        var characterSkill = character.GetComponent<CharacterBattleScript>().CharacterStatus.SkillList;
        //�@�����Ă���X�L���ɉ����ăR�}���h�{�^���̕\��
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

    //�@�G�̍U������
    public void EnemyAttack(GameObject character)
    {
        CharacterBattleScript characterBattleScript = character.GetComponent<CharacterBattleScript>();
        CharacterStatus characterStatus = characterBattleScript.CharacterStatus;

        if (characterStatus.SkillList.Count <= 0)
        {
            return;
        }
        //�@�G���K�[�h��Ԃł���΃K�[�h������
        //if (character.GetComponent<Animator>().GetBool("Guard"))
        //{
        //    character.GetComponent<CharacterBattleScript>().UnlockGuard();
        //}

        //�@�G�̍s���A���S���Y��
        int randomValue = (int)(Random.value * characterStatus.SkillList.Count);
        var nowSkill = characterStatus.SkillList[randomValue];

        //�@�e�X�g�p�i����̃X�L���Ŋm�F�j
        //nowSkill = characterStatus.GetSkillList()[0];

        if (nowSkill.SkillType == Skill.Type.DirectAttack)
        {
            var targetNum = (int)(Random.value * _allyCharacterInBattleList.Count);
            //�@�U�������CharacterBattleScript
            characterBattleScript.ChooseAttackOptions(CharacterBattleScript.BattleState.DirectAttack, _allyCharacterInBattleList[targetNum], nowSkill);
            Debug.Log(character.name + "��" + nowSkill.Name + "���s����");
        }
        else if (nowSkill.SkillType == Skill.Type.MagicAttack)
        {
            var targetNum = (int)(Random.value * _allyCharacterInBattleList.Count);
            if (characterBattleScript.Mp >= ((Magic)nowSkill).MagicPoints)
            {
                //�@�U�������CharacterBattleScript
                characterBattleScript.ChooseAttackOptions(CharacterBattleScript.BattleState.MagicAttack, _allyCharacterInBattleList[targetNum], nowSkill);
                Debug.Log(character.name + "��" + nowSkill.Name + "���s����");
            }
            else
            {
                Debug.Log("MP������Ȃ��I");
                //�@MP������Ȃ��ꍇ�͒��ڍU�����s��
                characterBattleScript.ChooseAttackOptions(CharacterBattleScript.BattleState.DirectAttack, _allyCharacterInBattleList[targetNum], characterStatus.SkillList.Find(skill => skill.SkillType == Skill.Type.DirectAttack));
                Debug.Log(character.name + "�͍U�����s����");
            }
        }
        else if (nowSkill.SkillType == Skill.Type.RecoveryMagic)
        {
            if (characterBattleScript.Mp >= ((Magic)nowSkill).MagicPoints)
            {
                var targetNum = (int)(Random.value * _enemyCharacterInBattleList.Count);
                //�@�񕜑����CharacterBattleScript
                characterBattleScript.ChooseAttackOptions(CharacterBattleScript.BattleState.Heal, _enemyCharacterInBattleList[targetNum], nowSkill);
                Debug.Log(character.name + "��" + nowSkill.Name + "���s����");
            }
            else
            {
                Debug.Log("MP������Ȃ��I");
                var targetNum = (int)(Random.value * _allyCharacterInBattleList.Count);
                //�@MP������Ȃ��ꍇ�͒��ڍU�����s��
                characterBattleScript.ChooseAttackOptions(CharacterBattleScript.BattleState.DirectAttack, _allyCharacterInBattleList[targetNum], characterStatus.SkillList.Find(skill => skill.SkillType == Skill.Type.DirectAttack));
                Debug.Log(character.name + "�͍U�����s����");
            }
        }
        else if (nowSkill.SkillType == Skill.Type.Guard)
        {
            characterBattleScript.Guard();
            // Guard�A�j����bool�Ȃ̂ŃA�j���[�V�����J�ڂ������炷���Ɏ��̃L�����N�^�[�Ɉڍs������
            ChangeNextChara();
            Debug.Log(character.name + "��" + nowSkill.Name + "���s����");
        }
    }

    //�@���̃L�����N�^�[�Ɉڍs
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
        //�@�S���U�����I�������ŏ�����
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
            Debug.Log("�������S��");
            _battleIsOver = true;
            CharacterBattleScript characterBattleScript;
            foreach (var character in _allyCharacterInBattleList)
            {
                //�@�����L�����N�^�[�̐퓬�ő�������HP��MP��ʏ�̃X�e�[�^�X�ɔ��f������
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
            //�@�s�펞�̌��ʕ\��
            _battleResult.InitialProcessingOfDefeatResult();
        }
    }

    public void DeleteEnemyCharacterInBattleList(GameObject deleteObj)
    {
        _enemyCharacterInBattleList.Remove(deleteObj);
        if (_enemyCharacterInBattleList.Count == 0)
        {
            Debug.Log("�G���S��");
            _battleIsOver = true;
            CharacterBattleScript characterBattleScript;
            foreach (var character in _allyCharacterInBattleList)
            {
                //�@�����L�����N�^�[�̐퓬�ő�������HP��MP��ʏ�̃X�e�[�^�X�ɔ��f������
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
            //�@�������̌��ʕ\��
            _battleResult.InitialProcessingOfVictoryResult(_allCharacterList, _allyCharacterInBattleList[0]);
        }
    }

    //�@�L�����N�^�[�I��
    public void SelectDirectAttacker(GameObject attackCharacter)
    {
        _currentCommand = CommandMode.SelectDirectAttack;
        _commandPanel.GetComponent<CanvasGroup>().interactable = false;
        selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

        _selectedBattleCharacter = _enemyCharacterInBattleList[_selecterNum];
        _attackCharacter = attackCharacter;
        _currentSelecterObject = Instantiate(_battleCharacterSelecter, _selectedBattleCharacter.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
    }

    //�@���ڍU��
    public void DirectAttack(GameObject attackCharacter, GameObject attackTarget)
    {
        //�@�U������L������DirectAttack�X�L�����擾����
        var characterSkill = attackCharacter.GetComponent<CharacterBattleScript>().CharacterStatus.SkillList;
        Skill directAtatck = characterSkill.Find(skill => skill.SkillType == Skill.Type.DirectAttack);
        attackCharacter.GetComponent<CharacterBattleScript>().ChooseAttackOptions(CharacterBattleScript.BattleState.DirectAttack, attackTarget, directAtatck);
        _commandPanel.gameObject.SetActive(false);
    }

    //�@�h��
    public void Guard(GameObject guardCharacter)
    {
        guardCharacter.GetComponent<CharacterBattleScript>().Guard();
        _commandPanel.gameObject.SetActive(false);
        ChangeNextChara();
    }

    //�@�g�p���閂�@�̑I��
    public void SelectMagic(GameObject character)
    {
        _currentCommand = CommandMode.SelectSkill;
        _commandPanel.GetComponent<CanvasGroup>().interactable = false;
        selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

        GameObject battleMagicPanelButtonIns;
        var skillList = character.GetComponent<CharacterBattleScript>().CharacterStatus.SkillList;

        //�@MagicOrItemPanel�̃X�N���[���l�̏�����
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

                ////�@�w�肵���ԍ��̃A�C�e���p�l���{�^���ɃA�C�e���X�N���[���p�X�N���v�g�����t����
                //if (battleMagicPanelButtonNum != 0
                //    && (battleMagicPanelButtonNum % scrollDownButtonNum == 0
                //    || battleMagicPanelButtonNum % (scrollDownButtonNum + 1) == 0)
                //    )
                //{
                //    //�@�A�C�e���X�N���[���X�N���v�g�̎��t���Đݒ�l�̃Z�b�g
                //    battleMagicPanelButtonIns.AddComponent<ScrollDownScript>();
                //}
                //else if (battleMagicPanelButtonNum != 0
                //  && (battleMagicPanelButtonNum % scrollUpButtonNum == 0
                //  || battleMagicPanelButtonNum % (scrollUpButtonNum + 1) == 0)
                //  )
                //{
                //    battleMagicPanelButtonIns.AddComponent<ScrollUpScript>();
                //}

                //�@MP������Ȃ����̓{�^���������Ă������������@�̖��O���Â�����
                if (character.GetComponent<CharacterBattleScript>().Mp < ((Magic)skill).MagicPoints)
                {
                    battleMagicPanelButtonIns.transform.Find("MagicName").GetComponent<Text>().color = new Color(0.4f, 0.4f, 0.4f);
                }
                else
                {
                    battleMagicPanelButtonIns.GetComponent<Button>().onClick.AddListener(() => SelectUseSkillTarget(character, skill));
                }
                //�@�{�^���ԍ��𑫂�
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

    //�@���@���g������̑I��
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

    //�@���@���g��
    public void UseSkill(GameObject user, GameObject targetCharacter, Skill skill)
    {
        CharacterBattleScript.BattleState battleState = CharacterBattleScript.BattleState.Idle;
        //�@���@���g�������CharacterBattleScript���擾���Ă���
        var targetCharacterBattleScript = targetCharacter.GetComponent<CharacterBattleScript>();

        //�@�g�����@�̎�ނ̐ݒ�ƑΏۂɎg���K�v���Ȃ��ꍇ�̏���
        if (skill.SkillType == Skill.Type.MagicAttack)
        {
            battleState = CharacterBattleScript.BattleState.MagicAttack;
        }
        else if (skill.SkillType == Skill.Type.RecoveryMagic)
        {
            if (targetCharacterBattleScript.Hp == targetCharacterBattleScript.CharacterStatus.MaxHp)
            {
                Debug.Log(targetCharacter.name + "�͑S���ł��B");
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
                Debug.Log(targetCharacter.name + "�͓ŏ�Ԃł͂���܂���B");
                return;
            }
            battleState = CharacterBattleScript.BattleState.PoisonRecoveryMagic;
        }
        else if (skill.SkillType == Skill.Type.SleepRecoveryMagic)
        {
            if (!targetCharacterBattleScript.IsSleep)
            {
                Debug.Log(targetCharacter.name + "�͍�����Ԃł͂���܂���B");
                return;
            }
            battleState = CharacterBattleScript.BattleState.SleepRecoveryMagic;
        }
        else if (skill.SkillType == Skill.Type.ParalysisRecoveryMagic)
        {
            if (!targetCharacterBattleScript.IsParalysis)
            {
                Debug.Log(targetCharacter.name + "�͖�჏�Ԃł͂���܂���B");
                return;
            }
            battleState = CharacterBattleScript.BattleState.ParalysisRecoveryMagic;
        }
        else if (skill.SkillType == Skill.Type.ConfusionRecoveryMagic)
        {
            if (!targetCharacterBattleScript.IsConfusion)
            {
                Debug.Log(targetCharacter.name + "�͍�����Ԃł͂���܂���B");
                return;
            }
            battleState = CharacterBattleScript.BattleState.ConfusionRecoveryMagic;
        }
        else if (skill.SkillType == Skill.Type.DepressionRecoveryMagic)
        {
            if (!targetCharacterBattleScript.IsDepression)
            {
                Debug.Log(targetCharacter.name + "�͟T��Ԃł͂���܂���B");
                return;
            }
            battleState = CharacterBattleScript.BattleState.DepressionRecoveryMagic;
        }
        user.GetComponent<CharacterBattleScript>().ChooseAttackOptions(battleState, targetCharacter, skill);
        _commandPanel.gameObject.SetActive(false);
        _skillOrItemPanel.gameObject.SetActive(false);
        //selectCharacterPanel.gameObject.SetActive(false);
    }

    //�@�g�p����A�C�e���̑I��
    public void SelectItem(GameObject character)
    {

        var itemDictionary = ((PlayerStatus)character.GetComponent<CharacterBattleScript>().CharacterStatus).ItemDictionary;

        //�@MagicOrItemPanel�̃X�N���[���l�̏�����
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

                //�@�w�肵���ԍ��̃A�C�e���p�l���{�^���ɃA�C�e���X�N���[���p�X�N���v�g�����t����
                //if (battleItemPanelButtonNum != 0
                //    && (battleItemPanelButtonNum % scrollDownButtonNum == 0
                //    || battleItemPanelButtonNum % (scrollDownButtonNum + 1) == 0)
                //    )
                //{
                //    //�@�A�C�e���X�N���[���X�N���v�g�̎��t���Đݒ�l�̃Z�b�g
                //    battleItemPanelButtonIns.AddComponent<ScrollDownScript>();
                //}
                //else if (battleItemPanelButtonNum != 0
                //  && (battleItemPanelButtonNum % scrollUpButtonNum == 0
                //  || battleItemPanelButtonNum % (scrollUpButtonNum + 1) == 0)
                //  )
                //{
                //    battleItemPanelButtonIns.AddComponent<ScrollUpScript>();
                //}
                ////�@�{�^���ԍ��𑫂�
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
            Debug.Log("�g����A�C�e��������܂���B");
        }
    }

    //�@�A�C�e�����g�p���鑊���I��
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

    //�@�A�C�e���g�p
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
                Debug.Log(targetCharacter.name + "�͑S���ł��B");
                return;
            }
            battleState = CharacterBattleScript.BattleState.UseHPRecoveryItem;
        }
        else if (item.ItemType == Item.Type.MPRecovery)
        {
            if (targetCharacterBattleScript.Mp == targetCharacterBattleScript.CharacterStatus.MaxMp)
            {
                Debug.Log(targetCharacter.name + "��MP�񕜂�����K�v������܂���B");
                return;
            }
            battleState = CharacterBattleScript.BattleState.UseMPRecoveryItem;
        }
        else if (item.ItemType == Item.Type.MotRecovery)
        {
            if (targetCharacterBattleScript.Mot == targetCharacterBattleScript.CharacterStatus.MaxMot)
            {
                Debug.Log(targetCharacter.name + "��Mot�񕜂�����K�v������܂���B");
                return;
            }
            battleState = CharacterBattleScript.BattleState.UseMotRecoveryItem;
        }
        else if (item.ItemType == Item.Type.PoisonRecovery)
        {
            if (!targetCharacterBattleScript.IsPoison)
            {
                Debug.Log(targetCharacter.name + "�͓ŏ�Ԃł͂���܂���B");
                return;
            }
            battleState = CharacterBattleScript.BattleState.UsePoisonRecoveryItem;
        }
        else if (item.ItemType == Item.Type.SleepRecovery)
        {
            if (!targetCharacterBattleScript.IsSleep)
            {
                Debug.Log(targetCharacter.name + "�͍�����Ԃł͂���܂���B");
                return;
            }
            battleState = CharacterBattleScript.BattleState.UseSleepRecoveryItem;
        }
        else if (item.ItemType == Item.Type.ParalysisRecovery)
        {
            if (!targetCharacterBattleScript.IsParalysis)
            {
                Debug.Log(targetCharacter.name + "�͖�჏�Ԃł͂���܂���B");
                return;
            }
            battleState = CharacterBattleScript.BattleState.UseParalysisRecoveryItem;
        }
        else if (item.ItemType == Item.Type.ConfusionRecovery)
        {
            if (!targetCharacterBattleScript.IsConfusion)
            {
                Debug.Log(targetCharacter.name + "�͍�����Ԃł͂���܂���B");
                return;
            }
            battleState = CharacterBattleScript.BattleState.UseConfusionRecoveryItem;
        }
        else if (item.ItemType == Item.Type.DepressionRecovery)
        {
            if (!targetCharacterBattleScript.IsDepression)
            {
                Debug.Log(targetCharacter.name + "�͟T��Ԃł͂���܂���B");
                return;
            }
            battleState = CharacterBattleScript.BattleState.UseDepressionRecoveryItem;
        }
        else if (item.ItemType == Item.Type.PowerUp)
        {
            //if (targetCharacterBattleScript.IsIncreasePower)
            //{
            //    Debug.Log("���ɍU���͂��グ�Ă��܂��B");
            //    return;
            //}
            battleState = CharacterBattleScript.BattleState.IncreaseAttackAndDecreaseDefense;
        }
        else if (item.ItemType == Item.Type.DefenseUp)
        {
            //if (targetCharacterBattleScript.IsIncreaseDefense)
            //{
            //    Debug.Log("���ɖh��͂��グ�Ă��܂��B");
            //    return;
            //}
            battleState = CharacterBattleScript.BattleState.IncreaseDefenseAndDecreaseAttack;
        }
        userCharacterBattleScript.ChooseAttackOptions(battleState, targetCharacter, skill, item);
        _commandPanel.gameObject.SetActive(false);
        _skillOrItemPanel.gameObject.SetActive(false);
    }

    //�@������
    public void GetAway(GameObject character)
    {
        var randomValue = Random.value;
        if (0f <= randomValue && randomValue <= 0.2f)
        {
            Debug.Log("������̂ɐ��������B");
            _battleIsOver = true;
            _commandPanel.gameObject.SetActive(false);
            //�@�퓬�I��
            _battleResult.InitialProcessingOfRanAwayResult();
        }
        else
        {
            Debug.Log("������̂Ɏ��s�����B");
            _commandPanel.gameObject.SetActive(false);
            ChangeNextChara();
        }
    }
}
