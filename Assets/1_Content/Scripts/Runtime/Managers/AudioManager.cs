using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WwiseSFXState { Gameplay, Paused, None };
public enum WwiseMusicState { Gameplay, Paused, None };
public enum WwiseEvent { MusicPlay, SFXPlay };

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;
    private bool isInitialized = false;

    [Header("Startup SoundBanks")]
    [SerializeField] private List<AK.Wwise.Bank> Soundbanks;

    [Header("Sfx State variables")]
    [SerializeField] private AK.Wwise.State SFX_Gameplay;
    [SerializeField] private AK.Wwise.State SFX_Paused;
    [SerializeField] private AK.Wwise.State SFX_None;

    private WwiseSFXState currentSFXState;

    [Header("Music State variables")]
    [SerializeField] private AK.Wwise.State Music_Gameplay;
    [SerializeField] private AK.Wwise.State Music_Paused;
    [SerializeField] private AK.Wwise.State Music_None;

    private WwiseMusicState currentMusicState;

    [Header("Wwise Music Events")]
    [SerializeField] public AK.Wwise.Event MusicPlay;
    [SerializeField] public AK.Wwise.Event SFXPlay;

    private void Awake()
    {
        Initialize();
        currentMusicState = WwiseMusicState.None;
        currentSFXState = WwiseSFXState.None;
    }
    void Start()
    {
    }
    void Initialize()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("AudioManager already exists!");
            Destroy(this);
        }

        if (!isInitialized)
            LoadSoundbanks();
        isInitialized = true;
    }
    void LoadSoundbanks()
    {
        if (Soundbanks.Count > 0)
        {
            foreach (AK.Wwise.Bank bank in Soundbanks)
                bank.Load();
            Debug.Log("Startup Soundbanks have been loaded.");
        }

        else
            Debug.LogError("Soundbanks list is Empty");
    }

    public void SetWwiseSFXState(WwiseSFXState state)
    {
        if (state == currentSFXState)
        {
            Debug.Log("SFX state is already " + state + ".");
            return;
        }
        switch (state)
        {
            default:
            case (WwiseSFXState.None):
                SFX_None.SetValue();
                break;
            case (WwiseSFXState.Gameplay):
                SFX_Gameplay.SetValue();
                break;
            case (WwiseSFXState.Paused):
                SFX_Paused.SetValue();
                break;

        }
        Debug.Log("New Wwise SFX state: " + state + ".");
        currentSFXState = state;
    }
    public void SetWwiseMusicState(WwiseMusicState state)
    {
        if (state == currentMusicState)
        {
            Debug.Log("Music state is already " + state + ".");
            return;
        }

        switch (state)
        {
            default:
            case (WwiseMusicState.None):
                Music_None.SetValue();
                break;
            case (WwiseMusicState.Gameplay):
                Music_Gameplay.SetValue();
                break;
            case (WwiseMusicState.Paused):
                Music_Paused.SetValue();
                break;

        }
        Debug.Log("New Wwise Music state: " + state + ".");
        currentMusicState = state;
    }

    public void PostWWiseEvent(AK.Wwise.Event wwiseEvent)
    {
        if (wwiseEvent == null)
        {
            Debug.LogError(wwiseEvent.Name + "WWise event is null!");
            return;
        }
        if (wwiseEvent.IsValid())
            wwiseEvent.Post(this.gameObject);
        else
            Debug.LogError(wwiseEvent.Name + "Wwise event is invalid!");
    }
    public void PostWWiseEvent(AK.Wwise.Event wwiseEvent, GameObject targetObject)
    {
        if (wwiseEvent == null)
        {
            Debug.LogError(wwiseEvent.Name + " WWise event is null!");
            return;
        }
        else if (targetObject == null)
        {
            Debug.LogError(targetObject.name + " WWise target gameobject is null!");
            return;
        }
        if (wwiseEvent.IsValid())
            wwiseEvent.Post(targetObject);
        else
            Debug.LogError(wwiseEvent.Name + " Wwise event is invalid!");

    }

}