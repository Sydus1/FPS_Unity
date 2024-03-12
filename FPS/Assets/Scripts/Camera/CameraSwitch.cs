using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviourPunCallbacks
{
    public Camera thirdPersonCamera;

    public Camera firstPersonCamera;

    private bool firstPersonEnabled = true;

    // Weapons Change View
    public Transform[] weaponsTransformFirstPerson;
    public Transform[] weaponsTransformThirdPerson;

    public GameObject[] weapons;

    public bool disableMeshPlayerFirstPerson = true;

    public SkinnedMeshRenderer meshPlayer;


    private void Start()
    {
        if (disableMeshPlayerFirstPerson)
        {
            meshPlayer.enabled = false;
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                firstPersonEnabled = !firstPersonEnabled;
                ChangeCamera();
            }
        }
    }

    public void ChangeCamera()
    {
        // First Person Camera
        if (firstPersonEnabled)
        {
            if (disableMeshPlayerFirstPerson)
            {
                meshPlayer.enabled = false;
            }

            firstPersonCamera.enabled = true;
            thirdPersonCamera.enabled = false;
            ChangeWeaponsFirstPerson();
        }

        // Third Person Camera
        else
        {
            meshPlayer.enabled = true;

            firstPersonCamera.enabled = false;
            thirdPersonCamera.enabled = true;
            ChangeWeaponsThirdPerson();
        }
    }

    public void ChangeWeaponsFirstPerson()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].transform.position = weaponsTransformFirstPerson[i].transform.position;
            weapons[i].transform.rotation = weaponsTransformFirstPerson[i].transform.rotation;
            weapons[i].transform.localScale = weaponsTransformFirstPerson[i].transform.localScale;
        }
    }

    public void ChangeWeaponsThirdPerson()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].transform.position = weaponsTransformThirdPerson[i].transform.position;
            weapons[i].transform.rotation = weaponsTransformThirdPerson[i].transform.rotation;
            weapons[i].transform.localScale = weaponsTransformThirdPerson[i].transform.localScale;
        }
    }
}
