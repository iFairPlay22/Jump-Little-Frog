using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    GameObject ParticleSystemModel;

    [SerializeField]
    Transform ExplosionTransform;

    [SerializeField]
    GameObject ExplosionContainer;

    [SerializeField]
    UnityEvent Callback;
    private void Awake()
    {
        ParticleSystemModel.SetActive(false);
    }

    public void Explode()
    {
        GameObject explosionGO = Instantiate(ParticleSystemModel, ExplosionContainer.transform);
        explosionGO.transform.position = ExplosionTransform.position;
        explosionGO.SetActive(true);
        explosionGO.GetComponent<ParticleSystem>().Play(true);

        Callback.Invoke();
    }
}
