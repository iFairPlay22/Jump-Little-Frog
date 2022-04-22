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

    [SerializeField]
    Vector2 RandomInitialTimeRange = new Vector2(1f, 3f);

    [SerializeField]
    [Range(1f, 10f)]
    float TimeBetweenShoots = 2f;

    bool _canShoot = false;

    [Header("Debug")]

    [SerializeField]
    bool DisplayGizmos = true; 

    [SerializeField]
    bool DisplayLineRenderer;

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
        LineRenderer.enabled = DisplayLineRenderer;

        StartCoroutine(InitFire());
    }

    IEnumerator InitFire()
    {
        _canShoot = false;
        yield return new WaitForSeconds(Random.Range(RandomInitialTimeRange.x, RandomInitialTimeRange.y));
        _canShoot = true;
    }

    void Update()
    {
        if (_canShoot)
            StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        _canShoot = false;

        yield return new WaitForSeconds(TimeBetweenShoots);


        GameObject projectile = Instantiate(CanonProjectile);
        projectile.GetComponent<GravityShootProjectile>().SetCannon(this);
        projectile.transform.position = FirePoint.transform.position;
        projectile.transform.SetParent(ProjectilesContainer.transform);
        projectile.GetComponent<Rigidbody2D>().AddForce(InitialVelocity, ForceMode2D.Impulse);
        projectile.SetActive(true);

        _canShoot = true;
    }

    void OnDrawGizmos()
    {
        if (DisplayGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(FirePoint.position, 0.3f);
        }

        LineRenderer.enabled = DisplayLineRenderer;
        if (DisplayLineRenderer)
            _DisplayTrajectory();
    }

    void _DisplayTrajectory()
    {
        float fTime = 0f;
        float fTimeStep = TrajectorySeconds / (float) TRAJECTORY_POINTS_NUMBER;

        LineRenderer.positionCount = TRAJECTORY_POINTS_NUMBER;
        for (int i = 0; i < TRAJECTORY_POINTS_NUMBER; i++)
        {
            Vector3 pos = GetPosition(fTime);
            LineRenderer.SetPosition(i, pos);
            fTime += fTimeStep;
        }
    }

    public Vector3 GetPosition(float fTime)
    {
        float gravity = Physics.gravity.magnitude;
        float velocity = InitialVelocity.magnitude;
        float angle = Mathf.Atan2(InitialVelocity.y, InitialVelocity.x);
        Vector3 start = FirePoint.position;
        float dx = velocity * fTime * Mathf.Cos(angle);
        float dy = velocity * fTime * Mathf.Sin(angle) - (gravity * fTime * fTime / 2f);

        return start + new Vector3(dx, dy, 0);
    }
}
