using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 3f;

    public float radius = 5f;

    public float explosionForce = 70f;

    private float countDown;

    private bool exploted = false;

    public GameObject explotionEffect;


    void Start()
    {
        countDown = delay;
    }

    void Update()
    {
        countDown -= Time.deltaTime;

        if (countDown <= 0 && exploted == false)
        {
            Explode();
            exploted = true;
        }
    }

    void Explode()
    {
        Instantiate(explotionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var rangeObjects in colliders)
        {
            Rigidbody rb = rangeObjects.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce * 10, transform.position, radius);
            }
        }

        Destroy(gameObject);
    }
}
