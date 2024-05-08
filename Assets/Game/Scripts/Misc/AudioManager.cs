using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private float _fadeDuration = 2f;
    public AudioClip Background;
    public AudioClip Begin;
    public AudioClip Fighting;
    public AudioClip Treasure;
    public AudioClip Upgrade;
    public AudioClip Boss;

    private Coroutine _fadeOutCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {

    }

    public void PlayBGM(AudioClip clip)
    {
        if (_fadeOutCoroutine != null)
        {
            StopCoroutine(_fadeOutCoroutine);
        }

        if (_musicSource.clip != clip)
        {
            _musicSource.clip = clip;
            _musicSource.Play();
        }
        _musicSource.volume = 0.5f;
    }

    public void MuteBGM()
    {
        _musicSource.volume = 0;
    }

    public void MuteBGMOverTime()
    {
        _fadeOutCoroutine = StartCoroutine(FadeOutBGM(_fadeDuration));
    }

    private IEnumerator FadeOutBGM(float fadeDuration)
    {
        float initialVolume = _musicSource.volume;

        float volumeStep = initialVolume / fadeDuration;

        while (_musicSource.volume > 0)
        {
            _musicSource.volume -= volumeStep * Time.deltaTime;
            yield return null;
        }

        _musicSource.volume = 0;
    }
}
