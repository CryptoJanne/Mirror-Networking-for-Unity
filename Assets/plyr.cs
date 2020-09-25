using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class plyr : NetworkBehaviour
{
    private const float MOVE_SPEED = 3f;
    private const float ROTATION_SPEED = 2f;
    public GameObject projectilePrefab;
    public Transform projectileMount;
    private float vertical;
    private float horizontal;
    private Material playerMaterialClone;
    public TextMesh playerHealthText;
    private CharacterController controller;

    [SyncVar(hook = nameof(OnHealthChanged))]
    public float health;
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerHealthText = transform.Find("FloatingInfo/PlayerHealthText").GetComponent<TextMesh>();
    }
    void Update()
    {
        if (!isLocalPlayer) { return; }

        // Rotate around y - axis
        transform.Rotate(0, Input.GetAxis("Horizontal") * ROTATION_SPEED, 0);

        // Move forward / backward
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float curSpeed = MOVE_SPEED * Input.GetAxis("Vertical");
        controller.SimpleMove(forward * curSpeed);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }
    }
    void OnHealthChanged(float _Old, float _New)
    {
        playerHealthText.text = $"{health}";
    }
    public override void OnStartLocalPlayer()
    {
        controller.enabled = true;
        Camera.main.orthographic = false;
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0f, 3f, -8f);
        Camera.main.transform.localEulerAngles = new Vector3(10f, 0f, 0f);
        CmdInitPlayerStats();
    }

    void OnDisable()
    {
       if (isLocalPlayer && Camera.main != null)
        {
            Camera.main.orthographic = true;
            Camera.main.transform.SetParent(null);
            Camera.main.transform.localPosition = new Vector3(0f, 70f, 0f);
            Camera.main.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
        }
    }
    
    [Command]
    public void CmdTakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            health = 0f;
        }
    }

    [Command]
    private void CmdInitPlayerStats()
    {
        health = 150f;
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
