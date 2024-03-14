using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLogic : MonoBehaviourPunCallbacks
{
    public Transform spawnPoint;

    public GameObject bullet;

    public float shotForce = 1500f;
    public float shotRate = 0.5f;

    private float shotRateTime = 0;

    private AudioSource audioSource;

    public AudioClip shotSound;

    public bool continueShooting = false;

    private PhotonView photonView;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        photonView = GetComponent<PhotonView>();
    }


    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetButtonDown("Fire1") && Time.timeScale != 0)
            {
                if (Time.time > shotRateTime && GameManager.Instance.gunAmmo > 0)
                {
                    if (continueShooting)
                    {
                        InvokeRepeating("ShootRPC", 0.005f, shotRate);
                    }
                    else
                    {
                        //Shoot();
                        ShootRPC();
                    }
                }
            }

            else if (Input.GetButtonUp("Fire1") && continueShooting && Time.timeScale != 0)
            {
                //CancelInvoke("Shoot");
                CancelInvoke("ShootRPC");
            }
        }
    }

    public void ShootRPC()
    {

        if (GameManager.Instance.gunAmmo > 0)
        {
            if (audioSource != null)
            {
                audioSource.PlayOneShot(shotSound);
            }

            photonView.RPC("ShootMultiplayer", RpcTarget.AllViaServer, spawnPoint.position, spawnPoint.rotation);

            GameManager.Instance.gunAmmo--;

            //GameObject newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);

            //newBullet.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * shotForce);

            shotRateTime = Time.time + shotRate;

            //Destroy(newBullet, 5);
        }

        else
        {
            CancelInvoke("ShootRPC");
        }
    }

    [PunRPC]
    public void ShootMultiplayer(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    {
        GameObject newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);

        newBullet.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * shotForce);

        Destroy(newBullet, 5);
    }
}
