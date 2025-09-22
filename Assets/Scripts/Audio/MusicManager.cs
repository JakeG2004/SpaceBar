using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MusicType
{
    GAMEPLAY,
    MENUS,
    NONE
}

[System.Serializable]
public class MusicDefinition
{
    public MusicType type;
    public AudioClip clip;
    public float volume = 1f;
}

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Music Library")]
    [SerializeField] private List<MusicDefinition> musicLibrary;
    [SerializeField] private float defaultCrossfadeTime = 1.5f;

    [SerializeField] private MusicType _autoplay = MusicType.NONE;
    private Dictionary<MusicType, MusicDefinition> _musicLookup = new();
    private AudioSource _activeSource;
    private AudioSource _nextSource;
    private GameObject _audioRoot;
    private MusicType _currentType;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _audioRoot = new GameObject("MusicSources");
        _audioRoot.transform.parent = transform;

        foreach (var m in musicLibrary)
            _musicLookup[m.type] = m;

        // Create two sources for crossfading
        _activeSource = _audioRoot.AddComponent<AudioSource>();
        _activeSource.loop = true;

        _nextSource = _audioRoot.AddComponent<AudioSource>();
        _nextSource.loop = true;
    }

    void Start()
    {
        switch(_autoplay)
        {
            case MusicType.GAMEPLAY:
                Play(_autoplay);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Plays the requested music track with crossfade (or immediately if first play).
    /// </summary>
    public void Play(MusicType type, float crossfadeTime = -1f)
    {
        if (!_musicLookup.TryGetValue(type, out var music) || music.clip == null)
        {
            Debug.LogWarning($"MusicManager: Music '{type}' not found or missing clip!");
            return;
        }

        if (_currentType == type && _activeSource.isPlaying)
            return; // already playing this track

        _currentType = type;
        crossfadeTime = crossfadeTime < 0 ? defaultCrossfadeTime : crossfadeTime;

        StopAllCoroutines();
        StartCoroutine(CrossfadeToTrack(music, crossfadeTime));
    }

    private IEnumerator CrossfadeToTrack(MusicDefinition newMusic, float duration)
    {
        // Set up next source
        _nextSource.clip = newMusic.clip;
        _nextSource.volume = 0f;
        _nextSource.Play();

        float startTime = Time.time;
        float initialVolume = _activeSource.volume;

        // Perform crossfade
        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            _nextSource.volume = Mathf.Lerp(0f, newMusic.volume, t);
            _activeSource.volume = Mathf.Lerp(initialVolume, 0f, t);
            yield return null;
        }

        // Ensure final volumes are correct
        _nextSource.volume = newMusic.volume;
        _activeSource.volume = 0f;
        _activeSource.Stop();

        // Swap sources
        var temp = _activeSource;
        _activeSource = _nextSource;
        _nextSource = temp;
    }

    /// <summary>
    /// Instantly stops music and clears sources (not recommended unless switching scenes abruptly).
    /// </summary>
    public void StopImmediately()
    {
        _activeSource.Stop();
        _nextSource.Stop();
        _currentType = default;
    }
}
