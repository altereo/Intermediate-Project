using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnController : MonoBehaviour
{
    public GameObject m_BulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If we're running in the editor (debugging), then we should
        // spawn a bullet when pressing the insert key. You know,
        // so we can debug things.
        if (Application.isEditor) {
            if (Input.GetKeyDown(KeyCode.Insert)) {
                SpawnBullet(Mathf.FloorToInt(Random.Range(4, 10)));
            }
        }   
    }

    public void SpawnBullet(int bulletSpeed)
    {
        GameObject instantiatedBullet = Instantiate(m_BulletPrefab, transform.position, transform.rotation);
        
        // Set the bullet's speed to the bullet speed.
        instantiatedBullet.GetComponent<BulletController>().m_BulletSpeed = bulletSpeed;
    }
}
