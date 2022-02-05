using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyAi : RegularMovement
{
    #region Collisions

    void Reset()
    {
        FindObjectOfType<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            FindObjectOfType<FoxHealthBar>().ReduceHealth();
            Debug.Log("Collision between ennemy and player!");
        }
    }

    #endregion
}
