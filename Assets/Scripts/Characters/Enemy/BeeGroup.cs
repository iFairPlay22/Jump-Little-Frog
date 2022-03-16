using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeGroup : MonoBehaviour
{
    [Header("Bees")]
    [SerializeField]
    GameObject BeeModel;

    [SerializeField]
    [Range(10, 30)]
    int NumberOfBees = 20;

    [SerializeField]
    [Range(0.5f, 2f)]
    float BeeGroupRange = 0.75f;

    [Header("Attack")]
    [SerializeField]
    Transform Target;

    [SerializeField]
    [Range(0.1f, 5f)]
    float Speed = 3f;

    [SerializeField]
    [Range(1f, 5f)]
    float WaitSeconds = 2f;

    [SerializeField]
    [Range(1f, 5f)]
    float MoveSeconds = 2f;

    bool _move = false;

    [SerializeField]
    bool DisplayGizmos = true;

    private void Awake()
    {
        BeeModel.SetActive(false);
    }

    void Start()
    {
        _GenerateBees();
        StartCoroutine(_ManageMovementTime());
    }

    void _GenerateBees()
    {
        for (int i = 0; i < NumberOfBees; i++)
        {
            GameObject bee = Instantiate(BeeModel, transform);
            bee.transform.position = RandomStaticMethods.GenerateRandomPointIn2dCircle(transform.position, BeeGroupRange);
            bee.SetActive(true);
        }
    }

    IEnumerator _ManageMovementTime()
    {
        _move = false;

        yield return new WaitForSeconds(WaitSeconds);

        _move = true;

        yield return new WaitForSeconds(MoveSeconds);

        StartCoroutine(_ManageMovementTime());
    }

    void Update()
    {
        if (_move)
            _Move();
    }

    private void _Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, Target.position, Speed * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        if (DisplayGizmos)
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawSphere(transform.position, BeeGroupRange);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(Target.position, 0.5f);
        }
    }
}
