﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float currentSpeed;

    private string currentState;
    private Animator animator;
    private int currentDirection;
    private bool isOnGround;
    private bool isStucked;
    private bool isControllerLocked;

    //Colliders
    private BoxCollider2D SquareCollider;
    private CircleCollider2D CircleCollider;
    private PolygonCollider2D TriangleCollider;

    //Sounds
    /*
    public GameObject audioPrefab;
    private AudioSource fitTargetSound;
    private AudioSource dieSound;
    private AudioSource landscapeSound;
    private AudioSource changeSound;
    private AudioSource bounceSound;*/

    //Rigidbody
    private Rigidbody2D rigidBody;

    //World
    private GameObject world;

    //Velocit Vector
    Vector3 velocity = new Vector3(1, 0, 0);

	// Use this for initialization
	void Start () {
        this.currentState = "Square"; // Game starts with Square
        this.animator = this.GetComponent<Animator>();
        this.currentDirection = 1; // 1 for RIGHT / -1 for LEFT

        this.world = GameObject.Find("World");
        this.isStucked = false;
        this.isControllerLocked = false;

        //Initialize the sounds
        /*this.fitTargetSound = this.audioPrefab.transform.GetChild(0).GetComponent<AudioSource>();
        this.dieSound = this.audioPrefab.transform.GetChild(1).GetComponent<AudioSource>();
        this.landscapeSound = this.audioPrefab.transform.GetChild(2).GetComponent<AudioSource>();
        this.changeSound = this.audioPrefab.transform.GetChild(3).GetComponent<AudioSource>();
        this.bounceSound = this.audioPrefab.transform.GetChild(4).GetComponent<AudioSource>();
        */
        //Grab the colliders
        this.SquareCollider = this.GetComponent<BoxCollider2D>();
        this.CircleCollider = this.GetComponent<CircleCollider2D>();
        this.TriangleCollider = this.GetComponent<PolygonCollider2D>();

        //Grab the Rigidbody
        this.rigidBody = this.GetComponent<Rigidbody2D>();
        this.setRigidbody();
	}
	
	// Update is called once per frame
	void Update () {
        
        this.ReceiveInput();

        switch (this.currentState)
        {
            case "Square":

                this.SquareCollider.enabled = true;
                this.CircleCollider.enabled = false;
                this.TriangleCollider.enabled = false;
                break;

            case "Triangle":

                this.SquareCollider.enabled = false;
                this.CircleCollider.enabled = false;
                this.TriangleCollider.enabled = true;
                break;

            case "Circle":

                this.SquareCollider.enabled = false;
                this.CircleCollider.enabled = true;
                this.TriangleCollider.enabled = false;
                
               // this.transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(1,0), Time.deltaTime * this.currentSpeed);
                break;
        }
	}

    void LateUpdate()
    {
        if (this.currentState == "Circle" && isOnGround)
        {
            this.rigidBody.velocity = this.currentDirection * this.currentSpeed * (this.velocity.normalized);
        }
    }

    void ReceiveInput()
    {
        if (!this.isControllerLocked)
        {
            if (Input.GetKeyDown(KeyCode.A)) //Square
            {
                this.PressSquare();
            }

            else if (Input.GetKeyDown(KeyCode.S)) //Circle
            {
                this.PressCircle();
            }

            else if (Input.GetKeyDown(KeyCode.D)) //Triangle
            {
                this.PressTriangle();
            }
        }
    }

    
    public void PressSquare()
    {
        if (!this.isControllerLocked)
        {
            //this.changeSound.Play();
            switch (this.currentState)
            {
                case "Circle":

                    this.animator.SetInteger("Status", 21);
                    break;

                case "Triangle":

                    this.animator.SetInteger("Status", 31);
                    break;
            }

            this.currentState = "Square";
            this.setRigidbody();
        }
    }

    public void PressCircle()
    {
        if (!this.isControllerLocked)
        {
           // this.changeSound.Play();
            switch (this.currentState)
            {
                case "Square":

                    this.animator.SetInteger("Status", 12);
                    break;

                case "Triangle":

                    this.animator.SetInteger("Status", 32);
                    break;
            }

            this.currentState = "Circle";
            this.setRigidbody();
        }
    }

    public void PressTriangle()
    {
        if (!this.isControllerLocked)
        {
            //this.changeSound.Play();
            switch (this.currentState)
            {
                case "Square":

                    this.animator.SetInteger("Status", 13);
                    break;

                case "Circle":

                    this.animator.SetInteger("Status", 23);
                    break;
            }

            this.currentState = "Triangle";
            this.setRigidbody();
        }
    }

	public void PressNextShape()
    {
        if (!this.isControllerLocked)
        {
            switch (this.currentState)
            {
                case "Square":
                    PressCircle();
                    break;
                case "Circle":
                    PressTriangle();
                    break;
                case "Triangle":
                    PressSquare();
                    break;
            }
        }
	}

	public void PressPreviousShape()
    {
        if (!this.isControllerLocked)
        {
            switch (this.currentState)
            {
                case "Square":
                    PressTriangle();
                    break;
                case "Circle":
                    PressSquare();
                    break;
                case "Triangle":
                    PressCircle();
                    break;
            }
        }
	}

    private void setRigidbody()
    {
        if (this.isStucked)
        {
            this.UnfixPlayer();
        }

        switch (this.currentState)
        {
            case "Square":
                this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                this.transform.eulerAngles = new Vector3(0, 0, 0);
                this.rigidBody.gravityScale = 2;
                this.rigidBody.mass = 10;
                break;

            case "Circle":
                this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                this.rigidBody.gravityScale = 1;
                this.rigidBody.mass = 5;
                break;

            case "Triangle":
                this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                this.transform.eulerAngles = new Vector3(0, 0, 0);
                this.rigidBody.gravityScale = -0.45f;
                this.rigidBody.mass = 5;
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Target"))
        {
            Config config = other.GetComponent<Config>();

            if (config.acceptedShape == this.currentState)
            {
                other.transform.GetComponent<TargetController>().TargetTrigger(this.gameObject);
                this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                FadeOut();
               // this.fitTargetSound.Play();
                this.isControllerLocked = true;
                StartCoroutine(LoadLevel(config.levelIndex, 2f));
            }

            else
            {
                if (config.isTargetText)
                {
                    LeanTween.alpha(config.targetText.gameObject, 1.0f, 0.2f);
                    config.targetText.gameObject.SetActive(true);
                }

                if (config.isThereDestroyable)
                {
                    for (int i = 0; i < config.destroyables.Length; i++)
                    {
                        if (config.destroyables[i] != null)
                        {
                            int id = LeanTween.alpha(config.destroyables[i], 0f, 0.2f).id;
                            Destroy(config.destroyables[i], 0.5f);
                        }
                    }
                }
            }
        }

        else if (other.CompareTag("Danger"))
        {
            Config config = other.GetComponent<Config>();
            FadeOut();
            LeanTween.move(this.gameObject, this.transform.position + new Vector3(0, 1), 0.5f);
            //this.dieSound.Play();
            this.isControllerLocked = true;
            StartCoroutine(LoadLevel(config.levelIndex, 0.5f));
        }

        else if (other.CompareTag("Spiky Landscape"))
        {
            Config config = other.GetComponent<Config>();
            //this.landscapeSound.Play();
            int id = LeanTween.rotate(this.world, new Vector3(0, 0, config.RotationInDeg), 0.5f).setEase(LeanTweenType.easeInOutSine).id;
            BeforeRotationEvents(config);
            this.isControllerLocked = true;
            LTDescr descr = LeanTween.descr(id);
            if (descr != null) // if the tween has already finished it will come back null
                descr.setOnComplete(() => AfterRotationEvents(config));

        }

        else if (other.CompareTag("Square Landscape"))
        {
            Config config = other.GetComponent<Config>();

            if (this.currentState == config.acceptedShape)
            {
                int id = LeanTween.rotate(this.world, new Vector3(0, 0, config.RotationInDeg), 0.5f).setEase(LeanTweenType.easeInOutSine).id;
                BeforeRotationEvents(config);
                this.isControllerLocked = true;
                //this.landscapeSound.Play();
                LTDescr descr = LeanTween.descr(id);
                if (descr != null) // if the tween has already finished it will come back null
                    descr.setOnComplete(() => AfterRotationEvents(config));
            }

            else
            {
                //this.dieSound.Play();
                FadeOut();
                this.isControllerLocked = true;
                LeanTween.move(this.gameObject, this.transform.position + new Vector3(0, 1), 0.5f);
                StartCoroutine(LoadLevel(config.levelIndex, 0.5f));
            }
           
        }

        else if (other.CompareTag("Bounce"))
        {
            this.currentDirection *= -1;
           // this.bounceSound.Play();
        }
    }

    void LoadLevelviaConf(Config config)
    {
        Application.LoadLevel(config.levelIndex);
    }

    void BeforeRotationEvents(Config config)
    {
        if (config.isThereImmediateDestroy)
        {
            for (int i = 0; i < config.immeddiatetlyDestroyables.Length; i++)
            {
                int id = LeanTween.alpha(config.immeddiatetlyDestroyables[i], 0f, 0.2f).id;
                Destroy(config.immeddiatetlyDestroyables[i], 0.1f);
            }
        }
    }

    void AfterRotationEvents(Config config)
    {
        this.FixPlayer();

        this.isStucked = true;
        this.isControllerLocked = false;

        if (config.shouldChangeDir)
        {
            this.currentDirection = -1;

        }

        if (config.isThereDestroyable)
        {
            for (int i = 0; i < config.destroyables.Length; i++)
            {
                int id = LeanTween.alpha(config.destroyables[i], 0f, 0.2f).id;
                Destroy(config.destroyables[i], 0.5f);
            }
        }

        if (config.isThereMovable)
        {
            LeanTween.move(config.movable, config.movableTarget.position, 0.5f);
        }

        if (config.isThereActivable)
        {
            for (int i = 0; i < config.activables.Length; i++)
            {
                int id = LeanTween.alpha(config.activables[i], 1.0f, 0.2f).id;
                config.activables[i].gameObject.SetActive(true);
            }
        }

        if (config.hasCameraAnimations)
        {
            LeanTween.value(Camera.allCameras[0].gameObject, Camera.allCameras[0].gameObject.GetComponent<Camera>().orthographicSize, config.camerazoomFrom5to, 0.5f).setDelay(0.1f).setOnUpdate((float val) =>
            {
                Camera.allCameras[0].gameObject.GetComponent<Camera>().orthographicSize = val;
            });

            Vector3 position = Camera.allCameras[0].gameObject.GetComponent<Transform>().position;
            LeanTween.value(Camera.allCameras[0].gameObject, position, new Vector3(position.x, position.y - config.cameraMoveonYAxis, position.z), 0.5f).setOnUpdate((Vector3 val) =>
            {
                Camera.allCameras[0].gameObject.GetComponent<Transform>().position = val;
            });
        }

        if (config.hasChangableTag)
        {
            EdgeCollider2D[] colliders = config.EdgeCarried.GetComponents<EdgeCollider2D>();

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].CompareTag("Var"))
                {
                    colliders[i].tag = config.newTag;
                }
            }
        }
    }

    void FixPlayer()
    {
        this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }

    void UnfixPlayer()
    {
        if (this.currentState != "Triangle")
        {
            this.isStucked = false;
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void OnCollisionStay2D(Collision2D coll) 
    {
        this.isOnGround = coll.transform.CompareTag("Ground");
    }

    void FixedUpdate()
    {
        isOnGround = false;

    }

    void RotateCircleAnimation(int id)
    {
        LTDescr descr = LeanTween.descr(id);
        if (descr != null) // if the tween has already finished it will come back null
            descr.setOnComplete(() => RotateCircleAnimation(this.RotateCircle()));
    }

    int RotateCircle()
    {
        return LeanTween.rotate(this.gameObject, new Vector3(0, 0, 360), 1.5f).setEase(LeanTweenType.linear).id;
    }

    IEnumerator LoadLevel(int n, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Application.LoadLevel(n);
    }

	public string GetCurrentState()
	{
		return currentState;
	}

    public void Tap()
    {
        switch (this.currentState)
        {
            case "Square":
                this.PressCircle();
                break;
            case "Circle":
                this.PressTriangle();
                break;
            case "Triangle":
                this.PressSquare();
                break;
        }
    }

    private void FadeOut()
    {
        switch (this.currentState)
        {
            case "Square":
                this.animator.SetInteger("Status", 5);
                break;
            case "Circle":
                this.animator.SetInteger("Status", 6);
                break;
            case "Triangle":
                this.animator.SetInteger("Status", 7);
                break;
        }
    }
}
