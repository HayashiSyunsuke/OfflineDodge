using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem m_hitParticle;
    [SerializeField]
    private ParticleSystem m_deadParticle;

    public void HitParticle(Vector3 position)
    {
        ParticleSystem newEffect = Instantiate(m_hitParticle);
        newEffect.transform.position = position;
        newEffect.Play();
        Destroy(newEffect.gameObject, 1.0f);
    }

    public void DeadParticle(Vector3 position)
    {
        ParticleSystem newEffect = Instantiate(m_deadParticle);
        newEffect.transform.position = position;
        newEffect.Play();
        Destroy(newEffect.gameObject, 1.0f);
    }
}
