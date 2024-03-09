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

    private AudioSource audioSource;

    public AudioClip explosionSound;



    void Start()
    {
        countDown = delay;
        audioSource = GetComponent<AudioSource>();
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
            AI ai = rangeObjects.GetComponent<AI>();

            if (ai != null)
            {
                ai.GrenadeImpact();
            }

            Rigidbody rb = rangeObjects.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce * 10, transform.position, radius);
            }
        }

        audioSource.PlayOneShot(explosionSound);

        gameObject.GetComponent<SphereCollider>().enabled = false;
        //gameObject.GetComponent<MeshRenderer>().enabled = false;

        Destroy(gameObject, delay * 2);
    }
}
