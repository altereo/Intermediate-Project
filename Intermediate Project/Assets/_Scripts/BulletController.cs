using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject m_ExplosionPrefab;
    public int m_BulletSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * m_BulletSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") {
            // Get the player's controller script.
            var playerController = collision.gameObject.GetComponent<PlayerGridlockController>();
            if (playerController != null) {
                // If it exists, remove one of their lives.
                playerController.HurtPlayer(1);
            }
            GameObject explosionInstance = Instantiate(m_ExplosionPrefab, gameObject.transform.position, transform.rotation);
            explosionInstance.gameObject.transform.SetParent(null);
            Destroy(gameObject);
        } else if (collision.gameObject.tag == "Wall") {
            // If we hit a wall, simply destroy the bullet. No need
            // for any special effects.
            Destroy(gameObject);
        }
    }
}
