using Photon.Pun;
using UnityEngine;

public class ThrowGrenade : MonoBehaviourPunCallbacks
{
    public float throwForce = 500f;
    public GameObject grenadePrefab;

    void Update()
    {
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.E) && Time.timeScale != 0)
        {
            ThrowRPC();
        }
    }

    void ThrowRPC()
    {
        // Llama al RPC para lanzar la granada
        photonView.RPC("ThrowGrenadeRPC", RpcTarget.AllViaServer, transform.position, transform.rotation);
    }

    [PunRPC]
    public void ThrowGrenadeRPC(Vector3 position, Quaternion rotation)
    {
        // Crea una instancia de la granada en la posición y rotación recibidas
        GameObject newGrenade = Instantiate(grenadePrefab, position, rotation);

        // Obtén el componente Rigidbody de la granada y aplícale una fuerza hacia adelante para simular el lanzamiento
        Rigidbody grenadeRigidbody = newGrenade.GetComponent<Rigidbody>();
        if (grenadeRigidbody != null)
        {
            grenadeRigidbody.AddForce(newGrenade.transform.forward * throwForce);
        }
    }
}
