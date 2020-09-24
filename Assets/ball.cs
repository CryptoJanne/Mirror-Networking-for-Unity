using UnityEngine;
using Mirror;
    public class ball : NetworkBehaviour
    {
        public float destroyAfter = 1;
        public Rigidbody rigidBody;
        public float force = 1000;
        public GameObject source;
        public int damage = 5;


        public override void OnStartServer()
        {
            Invoke(nameof(DestroySelf), destroyAfter);
        }

        // set velocity for server and client. this way we don't have to sync the
        // position, because both the server and the client simulate it.
        void Start()
        {
            rigidBody.AddForce(transform.forward * force);
        }

        // destroy for everyone on the server
        // destroy for everyone on the server
        [Server]
        void DestroySelf()
        {
            NetworkServer.Destroy(gameObject);
        }

        // ServerCallback because we don't want a warning if OnTriggerEnter is
        // called on the client
        [ServerCallback]
        void OnTriggerEnter(Collider co)
        {
            

            NetworkServer.Destroy(gameObject);
        }

    }
