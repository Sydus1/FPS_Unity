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

        // Solo el dueño de la granada debería establecer el temporizador
        if (!photonView.IsMine)
        {
            Invoke("Explode", delay);
        }
    }

    void Explode()
    {
        exploded = true;

        // Instancia el efecto de explosión
        PhotonNetwork.Instantiate(explosionEffect.name, transform.position, transform.rotation);

        // Encuentra los objetos dentro del radio de la explosión
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in colliders)
        {
            // Aplica la fuerza de la explosión a los objetos con Rigidbody
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, radius);
            }

            // Si hay un script AI, llámalo para que reaccione a la explosión
            AI ai = collider.GetComponent<AI>();
            if (ai != null)
            {
                ai.GrenadeImpact();
            }
        }

        // Reproduce el sonido de la explosión
        if (audioSource != null && explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }

        // Desactiva el collider para evitar daños adicionales
        GetComponent<SphereCollider>().enabled = false;

        // Destruye la granada después de un breve retraso para permitir que se reproduzca el efecto de explosión
        Destroy(gameObject, 2f);
    }

    // Si la granada no explota antes de ser destruida (por ejemplo, si se lanzó y luego el jugador se desconectó), evita que se ejecute la explosión
    void OnDestroy()
    {
        if (!exploded)
        {
            CancelInvoke("Explode");
        }
    }
}
