using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Shoot")]
    [SerializeField]
    Transform shootingStartPosition;

    [SerializeField]
    GameObject shootingProjectile;

    [SerializeField]
    GameObject shootingProjectilesRoot;

    [Header("Debug")]
    [SerializeField]
    bool displayGizmos = false;
    private void OnDrawGizmos()
    {
        if (displayGizmos)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(shootingStartPosition.position, 0.1f);
        }
    }

    private void Awake()
    {
        shootingProjectilesRoot.SetActive(true);
        shootingProjectile.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            _Shoot();
        }
    }

    void _Shoot()
    {
        GameObject projectile = Instantiate(shootingProjectile, shootingStartPosition);
        projectile.transform.SetParent(shootingProjectilesRoot.transform);
        projectile.SetActive(true);
    }
}
