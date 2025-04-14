using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputAction MoveAction;
    
    public float turnSpeed = 20f;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    // Sprinting mechanics
    public float sprintMultiplier = 2f;
    public float walkMultiplier = 1f;
    public ParticleSystem sprintDust;
    private bool forceSprint = false;


    void Start ()
    {
        m_Animator = GetComponent<Animator> ();
        m_Rigidbody = GetComponent<Rigidbody> ();
        m_AudioSource = GetComponent<AudioSource> ();

        if (sprintDust == null)
        sprintDust = GetComponentInChildren<ParticleSystem>();
        
        MoveAction.Enable();
    }

    void FixedUpdate ()
    {
        var pos = MoveAction.ReadValue<Vector2>();
        
        float horizontal = pos.x;
        float vertical = pos.y;
        
        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize ();

        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);
        bool isSprinting = forceSprint;

        
        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
            // sound chnage for sprinting - raj
            float targetPitch = isSprinting ? 1.5f : 1.0f;
            if (Mathf.Abs(m_AudioSource.pitch - targetPitch) > 0.01f)
            {
                m_AudioSource.pitch = targetPitch;
            }
        }
        else
        {
            m_AudioSource.Stop();
        }

        float speedMultiplier = isSprinting ? sprintMultiplier : walkMultiplier;
        Vector3 movementWithSpeed = m_Movement * speedMultiplier;

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation (desiredForward);
        // sprinting dust particle system - charlie
        if (isSprinting && isWalking)
        {
            if (!sprintDust.isPlaying)
            {
                sprintDust.Play();
            }
        }
        else
        {
            if (sprintDust.isPlaying)
            {
                sprintDust.Stop();
            }
        }
    }

    public void ForceSprint(bool value)
    {
        forceSprint = value;
    }

    void OnAnimatorMove ()
    {
        bool isSprinting = forceSprint;
        float speedMultiplier = isSprinting ? sprintMultiplier : walkMultiplier;
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude * speedMultiplier);
        m_Rigidbody.MoveRotation(m_Rotation);

    }
}