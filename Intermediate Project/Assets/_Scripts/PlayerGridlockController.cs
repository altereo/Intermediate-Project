using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGridlockController : MonoBehaviour
{
    public GameManager m_GameManager;

    public GameObject m_PlayerGameObject;
    public Vector3 m_PlayerPos;
    public int m_PlayerSpeed;
    public int m_PlayerMaxJumpTime;
    public float m_JumpTime;
    public ParticleSystem m_DeathParticles;
    public ParticleSystem m_JumpParticles;
    public TrailRenderer m_JumpTrail;
    public AudioSource m_HurtSound;
    public Animator m_playerMeshAnimator;

    private int m_PlayerLives = 5;
    public int playerLives { get { return m_PlayerLives; }}

    public HealthAnimationManager m_HealthAnimationManager;

    // Start is called before the first frame update
    void Start()
    {
        if (m_PlayerGameObject == null) {
            // If the gameobject isn't assigned, assume the one we're attached
            // to is good enough for our purposes.
            m_PlayerGameObject = gameObject;
        }
    }

    public void Die()
    {
        // When the player dies, set the gamestate accordingly and destroy self.
        m_GameManager.m_GameState = GameManager.GameState.GameOver;
        var deathParticles = Instantiate(m_DeathParticles, transform.position, transform.rotation);
        deathParticles.transform.SetParent(null);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_PlayerLives <= 0) {
            // When health reaches zero, die.
            Die();
        }
        
        // Just some debug code.
        if (Application.isEditor) {
            if (Input.GetKeyDown(KeyCode.Backspace)) {
                Die();
            }
        }
        // Move one unit in a given direction, depending on the 
        // user's key presses.
        if (Input.GetKey(KeyCode.D) && transform.position == m_PlayerPos) {
            m_PlayerPos += Vector3.right;
        }
        if (Input.GetKey(KeyCode.A) && transform.position == m_PlayerPos) {
            m_PlayerPos += Vector3.left;
        }
        if (Input.GetKey(KeyCode.W) && transform.position == m_PlayerPos) {
            m_PlayerPos += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S) && transform.position == m_PlayerPos) {
            m_PlayerPos += Vector3.back;
        }

        // When the user presses the space bar, set the player's position up
        // by one, then play a particle effect...
        // Then after a predetermined number of milliseconds, set it back
        // and stop the particles.
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.FloorToInt(transform.position.y) == 1) {
            gameObject.transform.position += Vector3.up;
            m_PlayerPos += Vector3.up;

            // Create the jump particles and enable the player's trail.
            var jumpParticles = Instantiate(m_JumpParticles, transform.position, transform.rotation);
            m_JumpTrail.emitting = true;
            m_playerMeshAnimator.Play("Dodge", -1, 0f);
        } else if (Mathf.FloorToInt(transform.position.y) == 2) {
            m_JumpTime += Time.deltaTime * 1000;
            if (m_JumpTime > m_PlayerMaxJumpTime) {
                gameObject.transform.position += Vector3.down;
                m_PlayerPos += Vector3.down;
                m_JumpTime = 0;
                m_JumpTrail.emitting = false;
            }
            m_playerMeshAnimator.SetBool("HasFinishedDodge", true);
        }
        // Limit the player's range to witin the camera view...
        m_PlayerPos = new Vector3(Mathf.Clamp(m_PlayerPos.x, -11, 11), m_PlayerPos.y, Mathf.Clamp(m_PlayerPos.z, -6, 6));

        transform.position = Vector3.MoveTowards(transform.position, m_PlayerPos, Time.deltaTime * m_PlayerSpeed);
    }

    // Called by other scripts when the player is hurt...
    public void HurtPlayer(int livesToRemove)
    {
        m_PlayerLives -= livesToRemove;
        if (m_PlayerLives < 0) {
            // If we've ended up out of range, set it back to zero.
            m_PlayerLives = 0;
        }

        m_HurtSound.Play();
        m_HealthAnimationManager.SetHealth(m_PlayerLives);
    }
}
