using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    Transform FirePoint;

    [SerializeField]
    GameObject CanonProjectile;

    [SerializeField]
    Vector3 InitialVelocity;

    [SerializeField]
    GameObject ProjectilesContainer;

    [Header("Debug")]

    [SerializeField]
    bool Debug;

    [SerializeField]
    LineRenderer LineRenderer;

    [SerializeField]
    [Range(0.1f, 5)]
    float TrajectorySeconds;

    [SerializeField]
    [Range(2, 50)]
    const int TRAJECTORY_POINTS_NUMBER = 15;

    void Awake()
    {
        CanonProjectile.SetActive(false);
        ProjectilesContainer.SetActive(true);
        LineRenderer.enabled = true;
    }

    void Update()
    {
        Fire();
    }

    void OnDrawGizmos()
    {
        if (Debug)
            _DisplayTrajectory();
    }

    void _DisplayTrajectory()
    {

        float gravity = Physics.gravity.magnitude;
        float velocity = InitialVelocity.magnitude;
        float angle = Mathf.Atan2(InitialVelocity.y, InitialVelocity.x);
        Vector3 start = FirePoint.position;

        float fTime = 0f;
        float fTimeStep = TrajectorySeconds / (float) TRAJECTORY_POINTS_NUMBER;

        LineRenderer.positionCount = TRAJECTORY_POINTS_NUMBER;
        for (int i = 0; i < TRAJECTORY_POINTS_NUMBER; i++)
        {
            float dx = velocity * fTime * Mathf.Cos(angle);
            float dy = velocity * fTime * Mathf.Sin(angle) - (gravity * fTime * fTime / 2f);
            Vector3 pos = start + new Vector3(dx, dy, 0);
            LineRenderer.SetPosition(i, pos);
            fTime += fTimeStep;
        }
    }

    void Fire()
    {
        GameObject projectile = Instantiate(CanonProjectile, FirePoint.transform);
        //projectile.transform.SetParent(ProjectilesContainer.transform);
        projectile.GetComponent<Rigidbody2D>().AddForce(InitialVelocity, ForceMode2D.Impulse);
        projectile.SetActive(true);
    }


}
