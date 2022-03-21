using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Shoot")]
    [SerializeField]
    GameObject shootingProjectile;

    [SerializeField]
    Transform shootingStartPosition;

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
            Gizmos.DrawSphere(shootingStartPosition.transform.position, 0.1f);
        }
    }

    private void Awake()
    {
        shootingProjectilesRoot.SetActive(true);
        shootingProjectile.SetActive(false);
    }

    public void _Shoot()
    {
        // Create projectile
        GameObject projectile = Instantiate(shootingProjectile, shootingStartPosition.transform);
        projectile.transform.SetParent(shootingProjectilesRoot.transform);
        projectile.transform.localScale = new Vector3(-transform.localScale.x, projectile.transform.localScale.y, projectile.transform.localScale.z);
        projectile.SetActive(true);
    }
}
