using Photon.Pun;
using UnityEngine;

public class Grenade : MonoBehaviourPunCallbacks
{
    public float delay = 3f;
    public float radius = 5f;
    public float explosionForce = 70f;
    public GameObject explosionEffect;
    public AudioClip explosionSound;

    private bool exploded = false;
    private AudioSource audioSource;
    private PhotonView photonView;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        photonView = GetComponent<PhotonView>();

        // Solo el due�o de la granada deber�a establecer el temporizador
        if (!photonView.IsMine)
        {
            Invoke("Explode", delay);
        }
    }

    void Explode()
    {
        exploded = true;

        // Instancia el efecto de explosi�n
        PhotonNetwork.Instantiate(explosionEffect.name, transform.position, transform.rotation);

        // Encuentra los objetos dentro del radio de la explosi�n
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in colliders)
        {
            // Aplica la fuerza de la explosi�n a los objetos con Rigidbody
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, radius);
            }

            // Si hay un script AI, ll�malo para que reaccione a la explosi�n
            AI ai = collider.GetComponent<AI>();
            if (ai != null)
            {
                ai.GrenadeImpact();
            }
        }

        // Reproduce el sonido de la explosi�n
        if (audioSource != null && explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }

        // Desactiva el collider para evitar da�os adicionales
        GetComponent<SphereCollider>().enabled = false;

        // Destruye la granada despu�s de un breve retraso para permitir que se reproduzca el efecto de explosi�n
        Destroy(gameObject, 2f);
    }

    // Si la granada no explota antes de ser destruida (por ejemplo, si se lanz� y luego el jugador se desconect�), evita que se ejecute la explosi�n
    void OnDestroy()
    {
        if (!exploded)
        {
            CancelInvoke("Explode");
        }
    }
}
