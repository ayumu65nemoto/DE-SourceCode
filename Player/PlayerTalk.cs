using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class PlayerTalk : MonoBehaviour
{
    //�C���X�^���X��
    public static PlayerTalk instance;
    //��b���̏��
    public enum TalkState
    {
        Talk,
        Dictionary,
        Translation,
        Hint
    }
    private TalkState _talkState;
    //�@��b�\�ȑ���i���������������邽�߁A����Ƃ��ď�����ݒ�j
    [SerializeField]
    private GameObject _conversationPartner;
    // TalkUI�Q�[���I�u�W�F�N�g
    [SerializeField]
    private GameObject _talkUI = null;
    //�@���b�Z�[�WUI
    [SerializeField]
    private TextMeshProUGUI _messageText = null;
    //�@�w�iUI
    [SerializeField]
    private Image _bgUI;
    //�@�����GUI
    [SerializeField]
    private Image _charPictureUI;
    //�@�b���Ă���L�����̖��O
    [SerializeField]
    private TextMeshProUGUI _talkCharName = null;
    //�@1�I���������ɍŏ��ɕ\�����郁�b�Z�[�W
    private List<string> _stringFirstSelectedList = new List<string>();
    //�@2��I���������ɍŏ��ɕ\�����郁�b�Z�[�W
    private List<string> _stringSecondSelectedList = new List<string>();
    //�@3��I���������ɍŏ��ɕ\�����郁�b�Z�[�W
    private List<string> _stringThirdSelectedList = new List<string>();
    //�@�\�����郁�b�Z�[�W
    private string _allMessage = null;
    //�@�g�p���镪��������
    [SerializeField]
    private string _splitString = "<>";
    //�@���s����
    [SerializeField]
    private string _lineString = "/";
    //�@�����������b�Z�[�W
    private string[] _splitMessage;
    //�@�����������b�Z�[�W�̉��Ԗڂ�
    private int _messageNum;
    //�@�e�L�X�g�X�s�[�h
    [SerializeField]
    private float _textSpeed = 0.05f;
    //�@�o�ߎ���
    private float _elapsedTime = 0f;
    //�@�����Ă��镶���ԍ�
    private int _nowTextNum = 0;
    //�@�}�E�X�N���b�N�𑣂��A�C�R��
    [SerializeField]
    private Image _clickIcon = null;
    //�@�N���b�N�A�C�R���̓_�ŕb��
    [SerializeField]
    private float _clickFlashTime = 0.2f;
    //  �e�L�X�g�̔�\������
    //[SerializeField]
    //private float _flashTextTime = 0.5f;
    //�@1�񕪂̃��b�Z�[�W��\���������ǂ���
    private bool _isOneMessage = false;
    //�@���b�Z�[�W�����ׂĕ\���������ǂ���
    private bool _isEndMessage = false;
    //�@�I�����E�C���h�E
    [SerializeField]
    private GameObject _choiceWindow;
    //�@���݂̕�����ԍ�
    private int _numOfSelections;
    //�@�I��UI�̃e�L�X�g
    [SerializeField]
    private TextMeshProUGUI _firstButtonText;
    [SerializeField]
    private TextMeshProUGUI _secondButtonText;
    [SerializeField]
    private TextMeshProUGUI _thirdButtonText;
    //�@�I������UI�ɕ\������e�L�X�g
    private List<string> _firstTitleTextList = new List<string>();
    private List<string> _secondTitleTextList = new List<string>();
    private List<string> _thirdTitleTextList = new List<string>();
    private bool _isChoosing;
    //�@��b���I��点��t���O
    private bool _isFinishTalk;
    //�@��b�̒i�K������
    private int _convFhase;

    //�@GameManager
    private GameManager _gameManager;

    //�@�L�[�𖳌������鎞��
    [SerializeField]
    private float _disableInterval = 5.0f;
    //�@�Ō�ɃL�[�������ꂽ����
    private float lastKeyPressTime;

    //�@�I�����ɂ���Ĕ��f���������t���O
    private string _isActiveOneFlag = "";
    private string _isActiveTwoFlag = "";
    private string _isActiveThreeFlag = "";

    //�@�I�����ɂ���Ĕ��f���������D���x
    private int _favRate1 = 0;
    private int _favRate2 = 0;
    private int _favRate3 = 0;

    [SerializeField]
    private DictionaryMode _dictionaryMode;
    [SerializeField]
    private OptionCommands _optionCommands;

    //��b���ĕ\��������ۂɎg�p���镶���ԍ�
    private int _reMessageNum = 0;
    private int _reNowTextNum = 0;

    //���s���Ă����b��Chapter
    private string _nowChapter = "";

    //�ǂ̑I������I�񂾂�
    private int _choiseFlag;
    private int _choiseNum = 0;

    //�I������CanvasGroup
    [SerializeField]
    private CanvasGroup _choiseCanvasGroup;

    // TipsController
    [SerializeField]
    private TipsController _tipsController;

    // �w�i�E�����G�̏��
    public enum BG
    {
        Normal,
        Exploration,
        House
    }
    private BG _bgState;

    public enum CharPicture
    {
        None,
        Normal,
        Joy,
        Anger,
        Surprise,
        Sadness,
        Fear,
        Disgust,
    }
    private CharPicture _charPictureState;

    private bool _isCenterChar = false;
    [SerializeField]
    private List<Sprite> _bgList = new List<Sprite>();
    [SerializeField]
    private List<Sprite> _charPictureList = new List<Sprite>();

    //��b���O
    [SerializeField]
    private TalkLog _talkLog;

    public TextMeshProUGUI MessageText { get => _messageText; set => _messageText = value; }
    public TextMeshProUGUI TalkCharName { get => _talkCharName; set => _talkCharName = value; }
    public float DisableInterval { get => _disableInterval; set => _disableInterval = value; }
    public string IsActiveOneFlag { get => _isActiveOneFlag; set => _isActiveOneFlag = value; }
    public string IsActiveTwoFlag { get => _isActiveTwoFlag; set => _isActiveTwoFlag = value; }
    public string IsActiveThreeFlag { get => _isActiveThreeFlag; set => _isActiveThreeFlag = value; }
    public int FavRate1 { get => _favRate1; set => _favRate1 = value; }
    public int FavRate2 { get => _favRate2; set => _favRate2 = value; }
    public int FavRate3 { get => _favRate3; set => _favRate3 = value; }
    public BG BgState { get => _bgState; set => _bgState = value; }
    public CharPicture CharPictureState { get => _charPictureState; set => _charPictureState = value; }
    public bool IsCenterChar { get => _isCenterChar; set => _isCenterChar = value; }
    public string NowChapter { get => _nowChapter; set => _nowChapter = value; }

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _isFinishTalk = false;
        _clickIcon.enabled = false;
        _convFhase = 0;
        _gameManager = GameManager.gameManager;
        _tipsController = TipsController.tipsController;
        //Initialize();
        if (_gameManager.FirstConv == false)
        {
            _conversationPartner = null;
        }

        if (_optionCommands == null)
        {
            _optionCommands = OptionCommands.optionCommands;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_optionCommands == null)
        {
            _optionCommands = OptionCommands.optionCommands;
        }

        if (_dictionaryMode.CurrentDictionaryState != DictionaryMode.DictionaryState.None)
        {
            //�R�}���h���J������I���ł��Ȃ�����
            if (_choiseCanvasGroup.interactable == true)
            {
                _choiseCanvasGroup.interactable = false;
            }
        }
        else
        {
            if (_choiseCanvasGroup.interactable == false)
            {
                _choiseCanvasGroup.interactable = true;
            }
        }

        //�@���b�Z�[�W���I����Ă��邩�A���b�Z�[�W���Ȃ��ꍇ�͂���ȍ~�������Ȃ�
        if (_isEndMessage == true || _allMessage == null || _isChoosing == true)
        {
            return;
        }

        // Tips���o�Ă���Ԃ͉����o���Ȃ�����
        //if (_tipsController.IsActiveTips == true)
        //{
        //    return;
        //}

        //�@�R�}���h���J���Ă��鎞��return
        if (_dictionaryMode.CurrentDictionaryState != DictionaryMode.DictionaryState.None
            || _optionCommands.CurrentOption != OptionCommands.OptionMode.None)
        {
            return;
        }
        else
        {
            //�@1��ɕ\�����郁�b�Z�[�W��\�����Ă��Ȃ�	
            if (!_isOneMessage)
            {
                //�@�e�L�X�g�\�����Ԃ��o�߂����烁�b�Z�[�W��ǉ�
                if (_elapsedTime >= _textSpeed)
                {
                    MessageText.enabled = false;
                    TalkChar(MessageText.text);
                    MessageText.text += ConvFoolCoolFont(_splitMessage[_messageNum][_nowTextNum].ToString());
                    NewLine(MessageText.text);
                    _reMessageNum = _messageNum;

                    _nowTextNum++;
                    _elapsedTime = 0f;

                    //�@���b�Z�[�W��S���\���A�܂��͍s�����ő吔�\�����ꂽ
                    if (_nowTextNum >= _splitMessage[_messageNum].Length)
                    {
                        _isOneMessage = true;
                        MessageText.enabled = true;
                    }
                }
                _elapsedTime += Time.deltaTime;

                //�@���b�Z�[�W�\�����Ƀ}�E�X�̍��{�^������������ꊇ�\��
                //if (Input.GetButtonDown("Jump"))
                //{
                //    float currentTime2 = Time.time;

                //    //�@�O��̓��͂����莞�Ԍo�߂��Ă��Ȃ���Ζ���
                //    if (currentTime2 - lastKeyPressTime < disableInterval)
                //    {
                //        return;
                //    }

                //    //�@�����܂łɕ\�����Ă���e�L�X�g�Ɏc��̃��b�Z�[�W�𑫂�
                //    messageText.text += ConvFoolCoolFont(_splitMessage[_messageNum].Substring(_nowTextNum));
                //    _isOneMessage = true;
                //}
                //�@1��ɕ\�����郁�b�Z�[�W��\������
            }
            else
            {
                _elapsedTime += Time.deltaTime;

                //�@�N���b�N�A�C�R����_�ł��鎞�Ԃ𒴂������A���]������
                if (_elapsedTime >= _clickFlashTime)
                {
                    _clickIcon.enabled = !_clickIcon.enabled;
                    _elapsedTime = 0f;
                }

                //�@���݂̃��b�Z�[�W�̎����Ō�̃��b�Z�[�W�ł���A���Ō�̃��b�Z�[�W���󕶎��̏ꍇ
                if (_messageNum + 1 >= _splitMessage.Length - 1 && _splitMessage[_splitMessage.Length - 1] == "")
                {
                    DisplayTheChoiceWindow();
                }
                //�@���݂̃��b�Z�[�W�̎����Ō�̃��b�Z�[�W�ł���A���Ō�̃��b�Z�[�W���h���h�̏ꍇ
                if (_messageNum + 1 >= _splitMessage.Length - 1 && _splitMessage[_splitMessage.Length - 1] == "@")
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        _isFinishTalk = false;
                        StartTalking(messNum: _convFhase + 1);
                        _convFhase++;
                    }
                }

                //�@�}�E�X�N���b�N���ꂽ�玟�̕����\������
                if (Input.GetKeyDown(KeyCode.F))
                {
                    float currentTime = Time.time;

                    //�@�O��̓��͂����莞�Ԍo�߂��Ă��Ȃ���Ζ���
                    if (currentTime - lastKeyPressTime < DisableInterval)
                    {
                        return;
                    }

                    //�@��b���O�ɋL�^
                    if (_talkLog == null)
                    {
                        _talkLog = _optionCommands.gameObject.transform.Find("TalkLogPanel").GetComponent<TalkLog>();
                    }
                    _talkLog.AddMessage(TalkCharName.text, MessageText.text);

                    _nowTextNum = 0;
                    _messageNum++;
                    MessageText.text = "";
                    _clickIcon.enabled = false;
                    _elapsedTime = 0f;
                    _isOneMessage = false;

                    //�@���b�Z�[�W���S���\������Ă�����Q�[���I�u�W�F�N�g���̂̍폜
                    if (_messageNum >= _splitMessage.Length)
                    {
                        Initialize();
                        EndTalking();
                    }
                    else
                    {
                        MessageText.text = "";
                    }
                }
            }
        }

        //�@�I���������ꂽ���i�}�E�X��UI�O���N���b�N�����j�͌��݂̃��[�h�ɂ���Ė������I��������
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (_choiceWindow.activeSelf == true)
            {
                EventSystem.current.SetSelectedGameObject(_choiceWindow.transform.GetChild(0).gameObject);
            }
        }
    }

    //�@��b�����ݒ�
    public void SetConversationPartner(GameObject partnerObj)
    {
        _conversationPartner = partnerObj;
    }

    //�@��b��������Z�b�g
    public void ResetConversationPartner(GameObject parterObj)
    {
        //�@��b���肪���Ȃ��ꍇ�͉������Ȃ�
        if (_conversationPartner == null)
        {
            return;
        }
        //�@��b����ƈ����Ŏ󂯎�������肪�����C���X�^���XID�����Ȃ��b������Ȃ���
        if (_conversationPartner.GetInstanceID() == parterObj.GetInstanceID())
        {
            _conversationPartner = null;
        }
    }

    //�@��b�����Ԃ�
    public GameObject GetConversationPartner()
    {
        return _conversationPartner;
    }

    //�@��b���J�n����
    public void StartTalking(string message = null, int messNum = 0)
    {
        if (message != null)
        {
            this.MessageText.text = message;
        }
        
        IConversation conversation = _conversationPartner.GetComponent<IConversation>();
        
        if (_isFinishTalk == false)
        {
            _isFinishTalk = true;
            if (messNum == 0)
            {
                //if (conversation.GetConversation() == null)
                //{
                //    return;
                //}
                this._allMessage = conversation.GetConversation().GetConversationMessage();
            }
            else if (messNum == 1)
            {
                this._allMessage = conversation.GetConversation2().GetConversationMessage();
            }
            else if (messNum == 2)
            {
                this._allMessage = conversation.GetConversation3().GetConversationMessage();
            }
            //�@����������ň��ɕ\�����郁�b�Z�[�W�𕪊�����
            _splitMessage = Regex.Split(this._allMessage, @"\s*" + _splitString + @"\s*");
        }
        else
        {
            //�@����������ň��ɕ\�����郁�b�Z�[�W�𕪊�����
            _splitMessage = Regex.Split(this.MessageText.text, @"\s*" + _splitString + @"\s*");
        }
        //�@����������
        _nowTextNum = 0;
        _messageNum = 0;
        MessageText.text = "";
        _talkUI.SetActive(true);
        _isOneMessage = false;
        _isEndMessage = false;
        //�@��b�J�n���̓��͈͂�U���Z�b�g
        Input.ResetInputAxes();

        //�@�w�i�E�����G������
        ChangeBg(_bgState);
        ChangeCharPicture(_charPictureState);
    }

    //�@�I���{�^�������������Ɏ��s
    public void OnPushButton(int onPush)
    {
        //�@�I�����E�C���h�E���\���ɂ���
        _isChoosing = false;
        _choiceWindow.SetActive(false);
        IConversation conversation = _conversationPartner.GetComponent<IConversation>();
        //�@�����ꂽ�{�^���ɉ����Ď��ɕ\�����郁�b�Z�[�W��ς���
        if (onPush == 1)
        {
            //�@���f���������t���O���n����Ă�����
            if (_gameManager.AllGameFlags.ContainsKey(_isActiveOneFlag))
            {
                _gameManager.AllGameFlags[_isActiveOneFlag] = true;
            }

            //�@���f���������D���x�̒l���n����Ă�����
            if (FavRate1 != 0)
            {
                //_gameManager.Favorability += FavRate1;
            }
            
            _choiseFlag = onPush;

            if (_convFhase == 0)
            {
                StartTalking(conversation.GetConversation().ReturnOne());
                _choiseNum = 1;
            }
            else if (_convFhase == 1)
            {
                StartTalking(conversation.GetConversation2().ReturnOne());
                _choiseNum = 2;
            }
            else if (_convFhase == 2)
            {
                StartTalking(conversation.GetConversation3().ReturnOne());
                _choiseNum = 3;
            }
        }
        else if (onPush == 2)
        {
            if (_gameManager.AllGameFlags.ContainsKey(_isActiveTwoFlag))
            {
                _gameManager.AllGameFlags[_isActiveTwoFlag] = true;
            }

            if (FavRate2 != 0)
            {
                //_gameManager.Favorability += FavRate2;
            }

            _choiseFlag = onPush;

            if (_convFhase == 0)
            {
                StartTalking(conversation.GetConversation().ReturnTwo());
                _choiseNum = 1;
            }
            else if (_convFhase == 1)
            {
                StartTalking(conversation.GetConversation2().ReturnTwo());
                _choiseNum = 2;
            }
            else if (_convFhase == 2)
            {
                StartTalking(conversation.GetConversation3().ReturnTwo());
                _choiseNum = 3;
            }
        }
        else if (onPush == 3)
        {
            if (_gameManager.AllGameFlags.ContainsKey(_isActiveThreeFlag))
            {
                _gameManager.AllGameFlags[_isActiveThreeFlag] = true;
            }

            if (FavRate3 != 0)
            {
                //_gameManager.Favorability += FavRate3;
            }

            _choiseFlag = onPush;

            if (_convFhase == 0)
            {
                StartTalking(conversation.GetConversation().ReturnTwo());
                _choiseNum = 1;
            }
            else if (_convFhase == 1)
            {
                StartTalking(conversation.GetConversation2().ReturnTwo());
                _choiseNum = 2;
            }
            else if (_convFhase == 2)
            {
                StartTalking(conversation.GetConversation3().ReturnTwo());
                _choiseNum = 3;
            }
        }
        _numOfSelections++;
    }

    //�@��b���I������
    public void EndTalking()
    {
        _isFinishTalk = false;
        _isEndMessage = true;
        _talkUI.SetActive(false);
        _convFhase = 0;
        //�@�v���C���[�̏�Ԃ�ύX����
        GetComponent<PlayerController>().SetPlayerState(PlayerController.PlayerState.Normal);
        Input.ResetInputAxes();
        // ��b����ɉ�b���I���������Ƃ�Ԃ�
        IConversation conversation = _conversationPartner.GetComponent<IConversation>();
        conversation.FinishTalking();

        if (_gameManager.FirstConv == true)
        {
            _gameManager.FirstConv = false;
            _conversationPartner = null;
        }
    }

    //�@�I�����ɕ\������e�L�X�g�̐ݒ�
    public void SetTheTitleTextOfTheChoice(List<string> stringFirstSelectedList, List<string> stringSecondSelected, List<string> stringThirdSelected, List<string> firstTitleList, List<string> secondTitleList, List<string> thirdTitleList)
    {
        Initialize();
        this._stringFirstSelectedList = stringFirstSelectedList;
        this._stringSecondSelectedList = stringSecondSelected;
        this._stringThirdSelectedList = stringThirdSelected;
        this._firstTitleTextList = firstTitleList;
        this._secondTitleTextList = secondTitleList;
        this._thirdTitleTextList = thirdTitleList;
    }

    //�@�I�����E�C���h�E�̕\��
    public void DisplayTheChoiceWindow()
    {
        _firstButtonText.text = ConvFoolCoolFont(_firstTitleTextList[_numOfSelections]);
        _secondButtonText.text = ConvFoolCoolFont(_secondTitleTextList[_numOfSelections]);
        _thirdButtonText.text = ConvFoolCoolFont(_thirdTitleTextList[_numOfSelections]);
        _isChoosing = true;
        _choiceWindow.SetActive(true);
        _choiseNum = 0;
        EventSystem.current.SetSelectedGameObject(_choiceWindow.transform.GetChild(0).gameObject);
    }

    //�@�ݒ�̏�����
    public void Initialize()
    {
        _clickIcon.enabled = false;
        _stringFirstSelectedList.Clear();
        _stringSecondSelectedList.Clear();
        _stringThirdSelectedList.Clear();
        //_yesTitleTextList.Clear();
        //_noTitleTextList.Clear();
        _numOfSelections = 0;
        _isEndMessage = true;
        _isChoosing = false;
        _talkUI.SetActive(false);
        _choiceWindow.SetActive(false);
    }

    //��b���͂����s����
    public void NewLine(string message)
    {
        int index = message.IndexOf(_lineString);
        if (index != -1)
        {
            string modifiedText = message.Replace(_lineString, "\n");
            MessageText.text = modifiedText;
        }
        else
        {
            MessageText.text = message;
        }
    }

    //��b���ĕ\��������
    public void ReStartTalking()
    {
        if (_choiseNum > 0)
        {
            OnPushButton(_choiseFlag);
            _choiseNum = 0;
        }
        else
        {
            _isFinishTalk = false;
            if (NowChapter != "")
            {
                _gameManager.AllGameFlags[NowChapter] = false;
            }
            StartTalking(messNum: _convFhase);
        }
        _messageNum = _reMessageNum;
    }

    //�b���Ă���L�����N�^�[�̖��O��\������
    public void TalkChar(string str)
    {
        if (str.Contains("&"))
        {
            MessageText.text = str.Replace("&", "");
            TalkCharName.text = "";
        }
        else if (str.Contains("#"))
        {
            MessageText.text = str.Replace("#", "");
            TalkCharName.text = "�v���C���[";
        }
        else if (str.Contains("$"))
        {
            MessageText.text = str.Replace("$", "");
            TalkCharName.text = "����";
        }
        else if (str.Contains("%"))
        {
            MessageText.text = str.Replace("%", "");
            TalkCharName.text = "����";
        }
        else if (str.Contains("*"))
        {
            MessageText.text = str.Replace("*", "");
            TalkCharName.text = "���l";
        }
    }

    //�ِ��E����̕\���ϊ�
    public string ConvFoolCoolFont(string str)
    {
        string rtnStr = "";
        bool isSelect = false;
        if (str == null)
        {
            return rtnStr;
        }
        //�������1�������ϊ�
        for (int i = 0; i < str.Length; i++)
        {
            //������ϊ�
            string convStr;
            switch (str[i])
            {
                case 'A':
                    convStr = "0";
                    break;
                case 'B':
                    convStr = "1";
                    break;
                case 'C':
                    convStr = "2";
                    break;
                case 'D':
                    convStr = "3";
                    break;
                case 'E':
                    convStr = "4";
                    break;
                case 'F':
                    convStr = "5";
                    break;
                case 'G':
                    convStr = "6";
                    break;
                case 'H':
                    convStr = "7";
                    break;
                case 'I':
                    convStr = "8";
                    break;
                case 'J':
                    convStr = "9";
                    break;
                case 'K':
                    convStr = "10";
                    break;
                case 'L':
                    convStr = "11";
                    break;
                case 'M':
                    convStr = "12";
                    break;
                case 'N':
                    convStr = "13";
                    break;
                case 'O':
                    convStr = "14";
                    break;
                case 'P':
                    convStr = "15";
                    break;
                case 'Q':
                    convStr = "16";
                    break;
                case 'R':
                    convStr = "17";
                    break;
                case 'S':
                    convStr = "18";
                    break;
                case 'T':
                    convStr = "19";
                    break;
                case 'U':
                    convStr = "20";
                    break;
                case 'V':
                    convStr = "21";
                    break;
                case 'W':
                    convStr = "22";
                    break;
                case 'X':
                    convStr = "23";
                    break;
                case 'Y':
                    convStr = "24";
                    break;
                case 'Z':
                    convStr = "25";
                    break;
                //����ȊO
                default:
                    isSelect = true;
                    convStr = str[i].ToString();
                    break;
            }
            //�ϊ�
            if (isSelect == false)
            {
                rtnStr += "<sprite=" + convStr + ">";
            }
            else
            {
                rtnStr += convStr;
            }
        }
        return rtnStr;
    }

    //�w�i�̕ύX
    private void ChangeBg(BG bg)
    {
        if (bg == BG.Normal)
        {
            _bgUI.sprite = null;
            _bgUI.color = new Color32(255, 255, 255, 0);
        }
        else if (bg == BG.Exploration)
        {
            _bgUI.sprite = _bgList[0];
            _bgUI.color = new Color32(255, 255, 255, 255);
        }
        else if (bg == BG.House)
        {
            _bgUI.sprite = _bgList[1];
            _bgUI.color = new Color32(255, 255, 255, 255);
        }
    }

    private void ChangeCharPicture(CharPicture charPicture)
    {
        if (_isCenterChar == true)
        {
            _charPictureUI.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
        else
        {
            _charPictureUI.transform.localPosition = new Vector3(0f, 120f, 0f);
        }

        if (charPicture == CharPicture.None)
        {
            _charPictureUI.sprite = null;
            _charPictureUI.color = new Color32(255, 255, 255, 0);
        }
        else if (charPicture == CharPicture.Normal)
        {
            _charPictureUI.sprite = _charPictureList[0];
            _charPictureUI.color = new Color32(255, 255, 255, 255);
        }
        else if (charPicture == CharPicture.Joy)
        {
            _charPictureUI.sprite = _charPictureList[1];
            _charPictureUI.color = new Color32(255, 255, 255, 255);
        }
        else if (charPicture == CharPicture.Anger)
        {
            _charPictureUI.sprite = _charPictureList[2];
            _charPictureUI.color = new Color32(255, 255, 255, 255);
        }
        else if (charPicture == CharPicture.Surprise)
        {
            _charPictureUI.sprite = _charPictureList[3];
            _charPictureUI.color = new Color32(255, 255, 255, 255);
        }
        else if (charPicture == CharPicture.Sadness)
        {
            _charPictureUI.sprite = _charPictureList[4];
            _charPictureUI.color = new Color32(255, 255, 255, 255);
        }
        else if (charPicture == CharPicture.Fear)
        {
            _charPictureUI.sprite = _charPictureList[5];
            _charPictureUI.color = new Color32(255, 255, 255, 255);
        }
        else if (charPicture == CharPicture.Disgust)
        {
            _charPictureUI.sprite = _charPictureList[6];
            _charPictureUI.color = new Color32(255, 255, 255, 255);
        }
    }
}
