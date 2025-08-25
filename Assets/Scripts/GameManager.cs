using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region Models

    [SerializeField] private ModelHolder[] _models;
    //[SerializeField] private ModelHolder[] _modelsTest;
    [SerializeField] private GameObject _currentModel;

    #endregion

    #region GameObjects

    private Transform _cameraT;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Transform _modelHolder;
    //[SerializeField] private SpriteRenderer _imageHolder;
    [SerializeField] private Image _imageHolder;
    [SerializeField] private GameObject _startGamePanel;
    [SerializeField] private GameObject _inGamePanel;
    [SerializeField] private GameObject _endPanel;
    [SerializeField] private GameObject _nextBtn;
    [SerializeField] private GameObject _exitBtn;
    [SerializeField] private GameObject _toodlesBtn;
    [SerializeField] private Transform _3dArrow;

    #endregion

    #region UI

    [SerializeField] private TMP_Text _storyTxt;
    [SerializeField] private RectTransform _storyHolder;

    #endregion

    #region Variables

    private int _currentModelIndex = 0;
    private bool _imgHolderColorLerp;
    private float _lerpValue = 0f;
    [SerializeField] private float _lerpSpeed = 1f;
    private bool _objectVisibility;
    [SerializeField] private Vector3 _originalArrowRotation;
    [SerializeField] private Vector3 _tiltArrowRotation;

    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        CameraCollider.CameraInRangeToView += CameraInRangeToView;
        CheckVisibility.ObjectVisibility += CheckVisibility_ObjectVisibility;
    }


    private void CheckVisibility_ObjectVisibility(bool visibility, Transform objectT)
    {
        _objectVisibility = visibility;
        //_3dArrow.gameObject.SetActive(visibility);
    }

    private void Start()
    {
        _startGamePanel.SetActive(true);
        _cameraT = Camera.main.transform;
        _imgHolderColorLerp = false;
        _objectVisibility = false;
    }

    private void CameraInRangeToView()
    {
        // fadein imageHolder
        _imageHolder.color = new Color(1, 1, 1, 0);
        _imageHolder.gameObject.SetActive(true);
        _imgHolderColorLerp = true;
        _lerpValue = 0f;

        // remove model
        _currentModel.GetComponent<Rigidbody>().isKinematic = false;

        ShowStoryAndMoveOn();
    }

    private void ShowStoryAndMoveOn()
    {
        _storyHolder.gameObject.SetActive(true);
        Invoke(nameof(ShowNext), 1.5f);
    }

    private void ShowNext()
    {
        if (_currentModelIndex == _models.Length)
        {
            _nextBtn.SetActive(false);
            _storyHolder.gameObject.SetActive(false);
            _toodlesBtn.SetActive(true);
        }
        else
            _nextBtn.SetActive(true);
    }

    public void OnClickNextBtn()
    {
        _nextBtn.SetActive(false);
        //_currentModelIndex++;

        //_currentModel.GetComponent<Rigidbody>().isKinematic = false;
        Invoke(nameof(SpawnNextModel), 1f);
    }

    public void OnClickToodlesBtn()
    {
        _inGamePanel.SetActive(false);
        _audioSource.Stop();
        _exitBtn.SetActive(false);
        _endPanel.SetActive(true);
    }

    public void OnClickExitBtn()
    {
        Application.Quit();
    }

    public void OnClickStartBtn()
    {
        _currentModelIndex = 0;
        SpawnNextModel();
    }

    private void SpawnNextModel()
    {
        // stop the old music
        if (_currentModelIndex > 0)
            _audioSource.Stop();

        // reset the collider bool values
        CameraCollider.instance.BackToSearch();

        // hide story text
        _storyHolder.gameObject.SetActive(false);

        // show model
        if (_currentModel != null)
            Destroy(_currentModel);
        _currentModel = _models[_currentModelIndex].model;
        _currentModel.GetComponent<Rigidbody>().isKinematic = true;
        _currentModel.SetActive(true);

        // to show 2d image 
        _imageHolder.gameObject.SetActive(false);
        _imageHolder.color = new Color(1, 1, 1, 0);
        _imageHolder.sprite = _models[_currentModelIndex].modelImage;

        // for managing audio
        _audioSource.clip = _models[_currentModelIndex].audioClip;
        _audioSource.Play();

        // for story text
        _storyTxt.text = _models[_currentModelIndex].storyStr;

        // increment index for next model
        _currentModelIndex++;
    }

    private void Update()
    {
        // lerp color for imageHolder on command
        if (_imgHolderColorLerp && _lerpValue < 1.0)
        {
            _lerpValue += Time.deltaTime * _lerpSpeed; // speed = how fast it fades
            float t = Mathf.Clamp01(_lerpValue);
            _imageHolder.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, t);

            //_lerpValue += Time.deltaTime;
            //_imageHolder.color = Color.Lerp(_imageHolder.color, Color.white, _lerpValue * _lerpSpeed);
        }
        else
            _imgHolderColorLerp = false;

        /*
        // show the 3d object
        if (!_objectVisibility && _currentModel != null)
        {
            _3dArrow.LookAt(_currentModel.transform);
            if (_3dArrow.rotation.eulerAngles.y % 90 == 0)
                _3dArrow.GetChild(0).rotation = Quaternion.Euler(_tiltArrowRotation);
            else
                _3dArrow.GetChild(0).rotation = Quaternion.Euler(_originalArrowRotation);
        }
        */

        // exit the app on back button press
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void OnDisable()
    {
        CameraCollider.CameraInRangeToView -= CameraInRangeToView;
        CheckVisibility.ObjectVisibility -= CheckVisibility_ObjectVisibility;
    }
}

[Serializable]
public class ModelHolder
{
    public GameObject model;
    public Sprite modelImage;
    public AudioClip audioClip;
    public string storyStr;
}