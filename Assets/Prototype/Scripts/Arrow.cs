using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class Arrow : XRGrabInteractable
{
    public float speed = 1000f;
    public Transform tip;
    bool inAir = false;
    Vector3 lastPosition = Vector3.zero;
    private Rigidbody rb;
    public Collider sphereCollider;

    [Header("Particles")]
    public ParticleSystem trailParticle;
    public ParticleSystem hitParticle;
    public TrailRenderer trailRenderer;

    [Header("Sound")]
    public AudioClip launchClip;
    public AudioClip hitClip;


    public GameObject pA1;
    public GameObject pB1;

    public GameObject pA2;
    public GameObject pB2;

    public GameObject pA3;
    public GameObject pB3;

    public GameObject pA4;
    public GameObject pB4;

    public GameObject pA5;
    public GameObject pB5;


    private Vector3 ref1;
    private Vector3 ref2;
    private Vector3 ref3;
    private Vector3 ref4;
    private Vector3 ref5;


    bool isTarget1 = false;
    bool isTarget2 = false;
    bool isTarget3 = false;
    bool isTarget4 = false;
    bool isTarget5 = false;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();

    }
    private void FixedUpdate()
    {
        if (inAir)
        {
            CheckCollision();
            lastPosition = tip.position;
        }
    }

    private void Start()
    {




    }

    void Update() {


        if (inAir) {




            float adjustment1 = 5.8f / ref1.z;
            float adjustment2 = 11.61f / ref2.z;
            float adjustment3 = 17.4f / ref3.z;
            float adjustment4 = 23.22f / ref4.z;
            float adjustment5 = 29f / ref5.z;


            Vector3 target1 = new Vector3(adjustment1 * ref1.x, adjustment1 * ref1.y, 5.8f);
            Vector3 target2 = new Vector3(adjustment2 * ref2.x, adjustment2 * ref2.y, 11.61f);
            Vector3 target3 = new Vector3(adjustment3 * ref3.x, adjustment3 * ref3.y, 17.4f);
            Vector3 target4 = new Vector3(adjustment4 * ref4.x, adjustment4 * ref4.y, 23.22f);
            Vector3 target5 = new Vector3(adjustment5 * ref5.x, adjustment5 * ref5.y, 29f);



            if (!isTarget1)
            {
                float step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, target1, step);

                // Check if the position of the cube and sphere are approximately equal.
                if (Vector3.Distance(transform.position, target1) < 0.00001f)
                {
                    // Swap the position of the cylinder.
                    isTarget1 = true;
                }
            }
            else if (!isTarget2)
            {
                float step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, target2, step);

                // Check if the position of the cube and sphere are approximately equal.
                if (Vector3.Distance(transform.position, target2) < 0.00001f)
                {
                    // Swap the position of the cylinder.
                    isTarget2 = true;
                }
            }


            else if (!isTarget3)
            {
                float step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, target3, step);

                // Check if the position of the cube and sphere are approximately equal.
                if (Vector3.Distance(transform.position, target3) < 0.001f)
                {
                    // Swap the position of the cylinder.
                    isTarget3 = true;
                }
            }

            else if (!isTarget4)
            {
                float step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, target4, step);

                // Check if the position of the cube and sphere are approximately equal.
                if (Vector3.Distance(transform.position, target4) < 0.001f)
                {
                    // Swap the position of the cylinder.
                    isTarget4 = true;
                }
            }

            else if (!isTarget5)
            {
                float step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, target5, step);

                // Check if the position of the cube and sphere are approximately equal.
                if (Vector3.Distance(transform.position, target5) < 0.001f)
                {
                    // Swap the position of the cylinder.
                    isTarget5 = true;
                }
            }





        }

    }

    private void CheckCollision()
    {
        if (Physics.Linecast(lastPosition, tip.position, out RaycastHit hitInfo))
        {
            if(hitInfo.transform.TryGetComponent(out Rigidbody body))
            {
                if (body.TryGetComponent<Lantern>(out Lantern lantern))
                    lantern.TurnOn();

                if (body.TryGetComponent<Potion>(out Potion potion))
                {
                    potion.BreakPotion();
                    return;
                }
                rb.interpolation = RigidbodyInterpolation.None;
                transform.parent = hitInfo.transform;
                body.AddForce(rb.velocity, ForceMode.Impulse);
            }
            Stop();
        }
    }
    private void Stop()
    {
        inAir = false;
        SetPhysics(false);

        ArrowParticles(false);
        ArrowSounds(hitClip, 1.5f, 2, .8f, -2);
    }

    public void Release(float value)
    {

        ref1 = pB1.transform.position - pA1.transform.position;
        ref2 = pB2.transform.position - pA2.transform.position;
        ref3 = pB3.transform.position - pA3.transform.position;
        ref4 = pB4.transform.position - pA4.transform.position;
        ref5 = pB5.transform.position - pA5.transform.position;


        inAir = true;
        SetPhysics(false);
        MaskAndFire(value);
       // StartCoroutine(RotateWithVelocity());

        lastPosition = tip.position;

        ArrowParticles(true);
        ArrowSounds(launchClip, 4.2f + (.6f*value), 4.4f + (.6f*value),Mathf.Max(.7f,value), -1);
    }

    private void SetPhysics(bool usePhysics)
    {
        rb.useGravity = usePhysics;
        rb.isKinematic = !usePhysics;
    }

    private void MaskAndFire(float power)
    {
        colliders[0].enabled = false;
        interactionLayerMask = 1 << LayerMask.NameToLayer("Ignore");



        // Vector3 force = transform.forward * power * speed;
        //  rb.AddForce(force, ForceMode.Impulse);
    }
    private IEnumerator RotateWithVelocity()
    {
        yield return new WaitForFixedUpdate();
        while (inAir)
        {
            Quaternion newRotation = Quaternion.LookRotation(rb.velocity, transform.up);
            transform.rotation = newRotation;
            yield return null;
        }
    }

    public void OnSelectEnter(XRBaseInteractor interactor)
    {
        base.OnSelectEnter(interactor);
    }

    public new void OnSelectExit(XRBaseInteractor interactor)
    {
        base.OnSelectExit(interactor);
    }

    public void ArrowHaptic(XRBaseInteractor interactor)
    {
        if (interactor is HandInteractor hand)
        {
            if (hand.TryGetComponent(out XRController controller))
                HapticManager.Impulse(.7f, .05f, controller.inputDevice);
        }
    }

    void ArrowParticles(bool release)
    {
        if (release)
        {
            trailParticle.Play();
            trailRenderer.emitting = true;
        }
        else
        {
            trailParticle.Stop(); 
            hitParticle.Play();
            trailRenderer.emitting = false;
        }
    }

    void ArrowSounds(AudioClip clip, float minPitch, float maxPitch,float volume, int id)
    {
        SFXPlayer.Instance.PlaySFX(clip, transform.position, new SFXPlayer.PlayParameters()
        {
            Pitch = Random.Range(minPitch, maxPitch),
            Volume = volume,
            SourceID = id
        });
    }

}
