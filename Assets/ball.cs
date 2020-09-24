using UnityEngine;
using Mirror;
    public class ball : NetworkBehaviour
    {
        public float destroyAfter = 1;
        public Rigidbody rigidBody;
        public float force = 1000;
        public GameObject source;


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
        [Server]
        void DestroySelf()
        {
            NetworkServer.Destroy(gameObject);
        }

    }
