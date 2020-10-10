using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class SinglePlayerController : MonoBehaviour
{
        public float speed = 700;
        public Rigidbody2D rigidbody2d;
        // need to use FixedUpdate for rigidbody

        private void Start() {
            if(rigidbody2d!=null)return;
            rigidbody2d =  gameObject.GetComponent<Rigidbody2D>();
        }
        void FixedUpdate()
        {
        rigidbody2d.velocity = new Vector2(Input.GetAxisRaw("Horizontal"),0) * speed * Time.fixedDeltaTime;
           
        }
}