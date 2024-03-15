using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RaiseAndLowerTextAnimationScript : MonoBehaviour
{
    //�@�ړI��Y�̒l
    [SerializeField]
    private float _destinationYValue = 0.5f;
    //�@�����̈ړ��X�s�[�h
    [SerializeField]
    private float _charSpeed = 5f;

    private TMP_Text _m_TextComponent;
    private bool _hasTextChanged;

    private VertexAnim[] _vertexAnims;
    private TMP_TextInfo _textInfo;
    private CharacterAnim[] _characterAnims;
    private TMP_MeshInfo[] _cachedMeshInfo;
    private TMP_CharacterInfo _charInfo;

    //�@���_�p�\����
    private struct VertexAnim
    {
        private float _yValue;

        public float YValue { get => _yValue; set => _yValue = value; }
    }
    //�@�������̍\����
    private struct CharacterAnim
    {
        private bool _isAnimationStart;
        private bool _isAnimationEnd;
        private bool _isFlap;

        public bool IsAnimationStart { get => _isAnimationStart; set => _isAnimationStart = value; }
        public bool IsAnimationEnd { get => _isAnimationEnd; set => _isAnimationEnd = value; }
        public bool IsFlap { get => _isFlap; set => _isFlap = value; }
    }

    void Awake()
    {
        _m_TextComponent = GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        // Subscribe to event fired when text object has been regenerated.
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
    }

    void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
    }

    void Start()
    {
        //�@�Ȃ��ƃG���[�ɂȂ�
        _m_TextComponent.ForceMeshUpdate();

        _textInfo = _m_TextComponent.textInfo;

        _hasTextChanged = true;

        //�@�����Ɏg���Ă��钸�_�p�f�[�^�̍쐬
        _vertexAnims = new VertexAnim[_textInfo.characterCount * 4];
        //�@�������̃f�[�^�쐬
        _characterAnims = new CharacterAnim[_textInfo.characterCount];

        //�@�ŏ��̕����̃A�j���[�V�������X�^�[�g������
        _characterAnims[0].IsAnimationStart = true;

        // Cache the vertex data of the text object as the Jitter FX is applied to the original position of the characters.
        _cachedMeshInfo = _textInfo.CopyMeshInfoVertexData();

        // Get new copy of vertex data if the text has changed.
        if (_hasTextChanged)
        {
            // Update the copy of the vertex data for the text object.
            _cachedMeshInfo = _textInfo.CopyMeshInfoVertexData();

            _hasTextChanged = false;
        }

    }

    void ON_TEXT_CHANGED(Object obj)
    {
        if (obj == _m_TextComponent)
            _hasTextChanged = true;
    }

    private void Update()
    {

        _m_TextComponent.ForceMeshUpdate();

        Matrix4x4 matrix;

        int characterCount = _textInfo.characterCount;

        // If No Characters then just yield and wait for some text to be added
        if (characterCount == 0)
        {
            return;
        }

        //�@�g�p����ϐ��錾
        Vector3[] charOffset = new Vector3[characterCount];
        int materialIndex;
        int vertexIndex;
        Vector3[] sourceVertices;
        Vector2 charMidBasline;
        Vector3 offset;
        Vector3[] destinationVertices;


        for (int i = 0; i < characterCount; i++)
        {
            //�@���̕����̃A�j���[�V�������X�^�[�g���ĂȂ���Ύ��̕���
            if (_characterAnims[i].IsAnimationStart == false)
            {
                continue;
            }
            //�@���̕����̃A�j���[�V�������I����Ă���Ύ��̕���
            if (_characterAnims[i].IsAnimationEnd == true)
            {
                continue;
            }

            _charInfo = _textInfo.characterInfo[i];

            // �����������Ȃ���΁i�󔒕����j���̕������A�j���[�V����������
            if (_charInfo.isVisible == false)
            {
                _characterAnims[i].IsAnimationStart = false;
                _characterAnims[i].IsAnimationEnd = true;
                //�@�Ō�̕����܂Ői��ł��Ȃ���Ύ��̕������X�^�[�g������
                if (i < characterCount - 1)
                {
                    _characterAnims[i + 1].IsAnimationStart = true;
                }
                else
                {
                    //�@�Ō�̕������I����Ă���΃Q�[���I�u�W�F�N�g��1�b��ɍ폜
                    Destroy(this.gameObject, 1f);
                }

                continue;
            }

            // Get the index of the material used by the current character.
            materialIndex = _textInfo.characterInfo[i].materialReferenceIndex;

            // Get the index of the first vertex used by this text element.
            vertexIndex = _textInfo.characterInfo[i].vertexIndex;

            // Get the cached vertices of the mesh used by this text element (character or sprite).
            sourceVertices = _cachedMeshInfo[materialIndex].vertices;

            // Determine the center point of each character at the baseline.
            //charMidBasline = new Vector2((sourceVertices[vertexIndex + 0].x + sourceVertices[vertexIndex + 2].x) / 2, charInfo.baseLine);
            // Determine the center point of each character.
            charMidBasline = (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;

            // Need to translate all 4 vertices of each quad to aligned with middle of character / baseline.
            // This is needed so the matrix TRS is applied at the origin for each character.
            offset = charMidBasline;

            destinationVertices = _textInfo.meshInfo[materialIndex].vertices;

            destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex + 0] - offset;
            destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] - offset;
            destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] - offset;
            destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] - offset;

            //�@��������܂ōs���Ă��Ȃ����͏�Ɉړ�
            if (_characterAnims[i].IsFlap == false)
            {
                charOffset[i] = new Vector3(0f, Mathf.MoveTowards(_vertexAnims[i].YValue, _destinationYValue, _charSpeed * Time.deltaTime), 0f);
                _vertexAnims[i].YValue = charOffset[i].y;
                Debug.Log(_vertexAnims[i].YValue);
                //�@�ړI�n���z�����甽�]
                if (charOffset[i].y >= _destinationYValue)
                {
                    _characterAnims[i].IsFlap = true;
                }
            }
            else
            {
                charOffset[i] = new Vector3(0f, Mathf.MoveTowards(_vertexAnims[i].YValue, 0f, _charSpeed * Time.deltaTime), 0f);
                _vertexAnims[i].YValue = charOffset[i].y;
                //�@���������̈ʒu�ɖ߂����玟�̕������X�^�[�g
                if (_vertexAnims[i].YValue <= 0f)
                {
                    _characterAnims[i].IsAnimationStart = false;
                    _characterAnims[i].IsAnimationEnd = true;
                    //�@�Ō�̕����łȂ���Ύ��̕������X�^�[�g
                    if (i < characterCount - 1)
                    {
                        _characterAnims[i + 1].IsAnimationStart = true;
                    }
                    else
                    {
                        //�@�Ō�̕����ł���΃Q�[���I�u�W�F�N�g��1�b��ɍ폜
                        Destroy(this.gameObject, 1f);
                    }
                }
            }
            //�@�ʒu�A��]�A�X�P�[������s����쐬
            matrix = Matrix4x4.TRS(charOffset[i], Quaternion.identity, Vector3.one);
            //�@�����ŗ^�������_��matrix�ϊ��s��ɏ]���ĕϊ�
            destinationVertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 0]);
            destinationVertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 1]);
            destinationVertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 2]);
            destinationVertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 3]);

            destinationVertices[vertexIndex + 0] += offset;
            destinationVertices[vertexIndex + 1] += offset;
            destinationVertices[vertexIndex + 2] += offset;
            destinationVertices[vertexIndex + 3] += offset;

        }

        // Push changes into meshes
        for (int i = 0; i < _textInfo.meshInfo.Length; i++)
        {
            _textInfo.meshInfo[i].mesh.vertices = _textInfo.meshInfo[i].vertices;
            _m_TextComponent.UpdateGeometry(_textInfo.meshInfo[i].mesh, i);
        }
    }
}
