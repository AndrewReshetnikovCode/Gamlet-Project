using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform player;

    void Update()  
    {
        Vector3 direction = (player.position - transform.position).normalized;     
        transform.position += direction * moveSpeed * Time.deltaTime;     
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Враг атакует игрока!");
        }
    }
}
