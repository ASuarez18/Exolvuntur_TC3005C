using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource behaviourAudioSource;
    [SerializeField] private AudioSource idleAudioSource;
    [SerializeField] private AudioClip[] _stepsSFX;
    [SerializeField] private AudioClip[] _attackSFX;
    [SerializeField] private AudioClip[] _damageSFX;
    [SerializeField] private AudioClip _agressiveSFX;
    [SerializeField] private AudioClip _deathSFX;
    [SerializeField] private AudioClip _scratchingSFX;
    [SerializeField] private AudioClip _inhaleSFX;
    [SerializeField] private AudioClip _exhaleSFX;

    public void PlayStepsSFX()
    {
        AudioClip _currentStep = _stepsSFX[Random.Range(0, _stepsSFX.Length)];
        PlayClip(_currentStep);
    }

    public void PlayAttackSFX()
    {
        AudioClip _currentAttack = _attackSFX[Random.Range(0, _attackSFX.Length)];
        PlayAttackClip(_currentAttack);
    }

    public void PlayDamageSFX()
    {
        AudioClip _currentDamage = _damageSFX[Random.Range(0, _damageSFX.Length)];
        PlayDamageClip(_currentDamage);
    }

    public void PlayClip(AudioClip clip)
    {
        if (clip.name == "healing_wail")
        {
            idleAudioSource.Pause();
        }
        else
        {
            idleAudioSource.UnPause();
        }

        behaviourAudioSource.PlayOneShot(clip);
    }

    public void PlayAttackClip(AudioClip clip)
    {   
        idleAudioSource.PlayOneShot(clip);
    }


    public void PlayAgressiveSFX()
    {
        PlayClip(_agressiveSFX);
    }

    public void PlayDeathSFX()
    {
        PlayClip(_deathSFX);
    }

    public void PlayScratchingSFX()
    {
        PlayClip(_scratchingSFX);
    }

    public void PlayInhaleSFX()
    {
        PlayClip(_inhaleSFX);
    }

    public void PlayExhaleSFX()
    {
        PlayClip(_exhaleSFX);
    }

    public void PlayDamageClip(AudioClip clip)
    {
        idleAudioSource.PlayOneShot(clip);
    }
}
