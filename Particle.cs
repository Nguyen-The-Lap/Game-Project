using UnityEngine;

public class EffectsController : MonoBehaviour
{
    public ParticleSystem smokeEffect;
    public ParticleSystem fireEffect;
    public ParticleSystem flashEffect;

    void Start()
    {
        StopAllEffects();
    }

    public void PlaySmoke()
    {
        if (smokeEffect) smokeEffect.Play();
    }

    public void PlayFire()
    {
        if (fireEffect) fireEffect.Play();
    }

    public void PlayFlash()
    {
        if (flashEffect) flashEffect.Play();
    }

    public void StopAllEffects()
    {
        if (smokeEffect) smokeEffect.Stop();
        if (fireEffect) fireEffect.Stop();
        if (flashEffect) flashEffect.Stop();
    }
}
