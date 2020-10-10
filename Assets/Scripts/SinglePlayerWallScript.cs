
using UnityEngine;

public class SinglePlayerWallScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name == "Ball"){
            FindObjectOfType<SinglePlayerManager>().Scored(gameObject.name);
        }
    }
}
