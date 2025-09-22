using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SoundType
{
    FAUCET,
    WIN,
    BUBBLE,
    // add more here as needed
}

[System.Serializable]
public class SoundDefinition
{
    public SoundType type;
    public AudioClip clip;
    public float volume = 1f;
    public bool loop = false;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Sound Library")]
    [SerializeField] private List<SoundDefinition> sounds;

    private Dictionary<SoundType, SoundDefinition> _soundLookup = new();
    private Dictionary<SoundType, AudioSource> _loopingSources = new();
    private Dictionary<SoundType, List<AudioSource>> _oneShotSources = new();
    private GameObject _audioRoot;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _audioRoot = new GameObject("AudioSources");
        _audioRoot.transform.parent = transform;

        foreach (var s in sounds)
            _soundLookup[s.type] = s;
    }

    // 🔊 Play a one-shot with optional fade-in
    public void PlayOneShot(SoundType type, float fadeInTime = 0f)
    {
        if (!_soundLookup.TryGetValue(type, out var sound) || sound.clip == null)
        {
            Debug.LogWarning($"SoundManager: Sound '{type}' not found or missing clip!");
            return;
        }

        var source = _audioRoot.AddComponent<AudioSource>();
        source.clip = sound.clip;
        source.volume = fadeInTime > 0 ? 0 : sound.volume;
        source.loop = false;
        source.Play();

        if (!_oneShotSources.ContainsKey(type))
            _oneShotSources[type] = new List<AudioSource>();
        _oneShotSources[type].Add(source);

        StartCoroutine(RemoveWhenDone(type, source));

        if (fadeInTime > 0)
            StartCoroutine(FadeVolume(source, sound.volume, fadeInTime));
    }

    // 🔁 Play a looping sound with optional fade-in
    public void PlayLoop(SoundType type, float fadeInTime = 0f)
    {
        if (_loopingSources.ContainsKey(type))
            return; // already playing

        if (!_soundLookup.TryGetValue(type, out var sound) || sound.clip == null)
        {
            Debug.LogWarning($"SoundManager: Loop sound '{type}' not found or missing clip!");
            return;
        }

        var source = _audioRoot.AddComponent<AudioSource>();
        source.clip = sound.clip;
        source.volume = fadeInTime > 0 ? 0 : sound.volume;
        source.loop = true;
        source.Play();

        _loopingSources[type] = source;

        if (fadeInTime > 0)
            StartCoroutine(FadeVolume(source, sound.volume, fadeInTime));
    }

    // ⏹ Stop *any* sound (one-shot or loop)
    public void StopSound(SoundType type, float fadeOutTime = 0f)
    {
        // Stop loop if it's playing
        if (_loopingSources.TryGetValue(type, out var loopSource))
        {
            if (fadeOutTime > 0)
                StartCoroutine(FadeOutAndStop(loopSource, fadeOutTime, () => _loopingSources.Remove(type)));
            else
            {
                loopSource.Stop();
                Destroy(loopSource);
                _loopingSources.Remove(type);
            }
        }

        // Stop all one-shots of this type
        if (_oneShotSources.TryGetValue(type, out var oneShotList))
        {
            foreach (var src in new List<AudioSource>(oneShotList))
            {
                if (fadeOutTime > 0)
                    StartCoroutine(FadeOutAndStop(src, fadeOutTime, () => oneShotList.Remove(src)));
                else
                {
                    src.Stop();
                    oneShotList.Remove(src);
                    Destroy(src);
                }
            }
            if (oneShotList.Count == 0)
                _oneShotSources.Remove(type);
        }
    }

    // ⏹ Stop everything with optional fade-out
    public void StopAll(float fadeOutTime = 0f)
    {
        foreach (var kvp in _loopingSources)
            StopSound(kvp.Key, fadeOutTime);

        foreach (var kvp in _oneShotSources)
            StopSound(kvp.Key, fadeOutTime);

        _loopingSources.Clear();
        _oneShotSources.Clear();
    }

    private IEnumerator RemoveWhenDone(SoundType type, AudioSource source)
    {
        yield return new WaitWhile(() => source != null && source.isPlaying);
        if (_oneShotSources.ContainsKey(type))
            _oneShotSources[type].Remove(source);
        Destroy(source);
    }

    private IEnumerator FadeVolume(AudioSource source, float targetVolume, float duration)
    {
        float startVolume = source.volume;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            if (source != null)
                source.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            yield return null;
        }
        if (source != null) source.volume = targetVolume;
    }

    private IEnumerator FadeOutAndStop(AudioSource source, float duration, System.Action onComplete = null)
    {
        float startVolume = source.volume;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            if (source != null)
                source.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        if (source != null)
        {
            source.Stop();
            Destroy(source);
        }
        onComplete?.Invoke();
    }
}
