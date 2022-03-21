using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RegularMovement : MonoBehaviour
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

    #endregion

    #region Private Fields

    int _nextPointId = 0;

    int _nextPointDirection = 1;

    bool _stop = false;

    #endregion

    #endregion

    #region Debug

    [Header("Debug")]
    [SerializeField]
    bool DisplayGizmos = true;

    [SerializeField]
    [Range(0.05f, 0.1f)]
    float SphereRadius = 0.075f;

    private static GUIStyle GUIStyle = new GUIStyle();

    #endregion

    #endregion

    #region Movement

    void Reset()
    {
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
            Gizmos.color = Color.green;

            for (int i = 0; i < Points.Count; i++)
            {
                Gizmos.DrawSphere(Points[i].position, SphereRadius);
                Handles.Label(Points[i].position + new Vector3(SphereRadius * 1.1f, 0f, 0f), "" + i, GUIStyle);
            }
        }
    }

    void FixedUpdate()
    {
        _Move();
    }

    void _Move()
    {
        if (_stop)
            return;

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

    public void Stop()
    {
        _stop = true;
    }
    public void UnStop()
    {
        _stop = false;
    }

    #endregion
}
