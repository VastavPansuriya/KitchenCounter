using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource audioSource;

    private float warningSoundTimer;
    private bool playBurningSound;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChange += StoveCounter_OnProgressChange; ;
    }

    private void StoveCounter_OnProgressChange(object sender, IHasProgress.OnProgressChangeEventArgs e)
    {

        float burnShowProgressAmount = .5f;
        playBurningSound = e.progressNormalized <= 1 &&stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool playSound = e.currentState == StoveCounter.CokkingPattyState.Frying || e.currentState == StoveCounter.CokkingPattyState.Fried;
        if (playSound)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }

    private void OnDestroy()
    {
        stoveCounter.OnStateChanged -= StoveCounter_OnStateChanged;
    }

    private void Update()
    {
        if (!playBurningSound)
        {
            return;
        }
        warningSoundTimer -= Time.deltaTime;
        if (warningSoundTimer <= 0f)
        {
            const float warningSoundTimerMax = .2f;
            warningSoundTimer = warningSoundTimerMax;
            SoundManager.Instance.PlayWarningSound(transform.position);
        }
    }
}
