using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
    public ParticleSystem fireEffect;
    public ParticleSystem fireEmbers;

    public Light glowEffect;

    //set warmth level between 0 and 1
    // 0 = cold, no fire effect
    // 1 = warm, full fire effect
    public float warmthLevel = 1;
    
    public float glowLevel = 1;

    private float maxGlow = 7f;

    private void Update()
    {
        //fireEffect modulation
        var fireEffectEmission = fireEffect.emission;
        fireEffectEmission.rateOverTimeMultiplier = warmthLevel * 20.0f;

        var fireEffectVelocity = fireEffect.velocityOverLifetime;
        fireEffectVelocity.radialMultiplier = Mathf.Lerp(0.2f, 0.5f, warmthLevel);

        //embers modulation
        var fireEmbersEmission = fireEmbers.emission;
        fireEmbersEmission.rateOverTimeMultiplier = warmthLevel * 20.0f;

        var fireEmbersSpeed = fireEmbers.velocityOverLifetime;
        fireEmbersSpeed.speedModifierMultiplier = Mathf.Lerp(0.5f, 1.0f, warmthLevel);

        //glow modulation
        glowEffect.range = maxGlow * glowLevel;
    }
}
