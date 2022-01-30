using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyAi : MonoBehaviour
{

    #region Variables

    #region Movement

    #region Serializable Fields

    [Header("Movement")]
    [SerializeField]
    List<Transform> Points = new List<Transform>();

    [SerializeField]
    [Range(0, 2)]
    float speed = 1;

    [SerializeField]
    [Range(0.05f, 0.1f)]
    float sphereCollisionRadius = 0.075f;

    #endregion

    #region Private Fields

    int _nextPointId = 0;

    int _nextPointDirection = 1;

    #endregion

    #endregion


    #region Debug

    [Header("Debug")]
    [SerializeField]
    bool DisplayGizmos = true;

    #endregion

    private static GUIStyle GUIStyle = new GUIStyle();

    #endregion

    #region Movement

    void Reset()
    {
        FindObjectOfType<Collider2D>().isTrigger = true;

        GameObject root = new GameObject(gameObject.name + "Root");
        GameObject road = new GameObject("Road");
        GameObject point1 = new GameObject("Point1");
        GameObject point2 = new GameObject("Point2");

        transform.SetParent(root.transform);
        road.transform.SetParent(root.transform);
        point1.transform.SetParent(road.transform);
        point2.transform.SetParent(road.transform);

        Vector3 pos = transform.position;

        transform.position = Vector3.zero;
        road.transform.position = Vector3.zero;
        point1.transform.position = Vector3.zero;
        point2.transform.position = new Vector3(5, 0, 0);
        root.transform.position = pos;

        Points.Add(point1.transform);
        Points.Add(point2.transform);
    }

    void Awake()
    {
        transform.position = Points[0].position;
        NextPoint();

        GUIStyle.normal.textColor = Color.black;
        GUIStyle.fontSize = 20;
    }

    void OnDrawGizmos()
    {
        if (DisplayGizmos)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(transform.position, sphereCollisionRadius);

            Gizmos.color = Color.green;

            for (int i = 0; i < Points.Count; i++)
            {
                Gizmos.DrawSphere(Points[i].position, sphereCollisionRadius);
                Handles.Label(Points[i].position + new Vector3(sphereCollisionRadius * 1.1f, 0f, 0f), "" + i, GUIStyle);
            }
        }   
    }

    void FixedUpdate()
    {
        _Move();
    }

    void _Move()
    {
        if (Points.Count <= 1)
            return;

        Vector3 fromPos = transform.position;
        Vector3 toPos = Points[_nextPointId].position;

        // Check distance
        if (Vector3.Distance(fromPos, toPos) < 0.2f)
            NextPoint();

        // Rotate
        if (fromPos.x <= toPos.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);

        // 1 gauche -1 droite

        // Move
        transform.position = Vector3.MoveTowards(fromPos, toPos, speed * Time.fixedDeltaTime);
    }

    void NextPoint()
    {
        if (_nextPointId == 0)
            _nextPointDirection = 1;

        else if (_nextPointId == Points.Count - 1)
            _nextPointDirection = -1;

        _nextPointId += _nextPointDirection;
    }

    #endregion

    #region Collisions

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Collision between ennemy and player!");
        }
    }

    #endregion
}
