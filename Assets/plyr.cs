using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class plyr : NetworkBehaviour
{
    public GameObject projectilePrefab;
    public Transform projectileMount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }
    }

    // this is called on the server
    [Command]
    void CmdFire()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileMount.position, transform.rotation);
        projectile.GetComponent<ball>().source = gameObject;
        NetworkServer.Spawn(projectile);
        RpcOnFire();
    }

        // this is called on the tank that fired for all observers
    [ClientRpc]
    void RpcOnFire()
    {
        //animator.SetTrigger("Shoot");
    }
}
