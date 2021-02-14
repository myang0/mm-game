using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBoost : BaseBoost
{
    [SerializeField] private Rigidbody2D _rb;

    void Start()
    {
        _rb.velocity = new Vector2(Random.Range(-10f, -5f), 0);
    }

    void FixedUpdate()
    {
        transform.Rotate(0, 0, 4);

        if (transform.position.x < -15) Destroy(gameObject);
    }

    public override void PickUp() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null) return;

        // Heal the player
    }
}