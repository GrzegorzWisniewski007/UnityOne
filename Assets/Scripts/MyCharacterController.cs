using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacterController : MonoBehaviour
{
    public CharacterController characterControler;

    public Camera Camera;

    private float MoveButtons;

    //Predkosc.
    public float Velocity = 0f;
    //Prędkość poruszania się gracza.
    public float WalkSpeed = 5.0f;
    //Predkosc biegania.
    public float RunningSpeed = 10.0f;
    //Wysokość skoku.
    public float JumpHeight = 7.0f;
    //Aktualna wysokosc skoku.
    public float ActualJumpHeight = 0f;

    private Animator animator;

    /** 
	 * Pobranie prędkości poruszania się przód/tył.
	 * jeżeli wartość dodatnia to poruszamy się do przodu,
	 * jeżeli wartość ujemna to poruszamy się do tyłu.
	 */
    private float JumpMove = 0.0f;


    /** Zmienna dostarcza informację o tym czy gracz bienie.*/
    public bool Run;

    //Czulość myszki 
    public float MouseSensitivy = 4.0f;

    Vector3 gestureStartPosition;
    Vector3 gestureDelta;



    // Use this for initialization
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            this.enabled = false;
        }
        characterControler = GetComponent<CharacterController>();        

        animator = (Animator)GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Mouse();
        Keyboard();
    }    

    private void Keyboard()
    {
        float ForwardBackMovement = Input.GetAxis("Vertical") * Velocity;
        //Debug.Log(Input.GetAxis("Vertical"));
        ActualJumpHeight = Jump(ForwardBackMovement);

        //Bieganie
        if (Input.GetKeyDown("left shift"))
        {
            Run = true;
        }
        else if (Input.GetKeyUp("left shift"))
        {
            Run = false;
        }

        Runing();
        //Debug.Log(ForwardBackMovement);
        Animation(ForwardBackMovement);

        Move(ForwardBackMovement);
    }

    private void Mouse()
    {
        //pobieranie wartości obrotu kamery
        float CameraAngle = Camera.transform.rotation.eulerAngles.y;

        float myszLewoPrawo = Input.GetAxis("Mouse X") * MouseSensitivy;
        //Debug.Log(myszLewoPrawo);
        transform.rotation = Quaternion.Euler(0f, CameraAngle, 0f);
    }

    private void Move(float motion)
    {
        Vector3 move = new Vector3(0f, 0f, 0f);
        if (characterControler.isGrounded)
        {
            move = new Vector3(0f, ActualJumpHeight, motion);
        }
        else
        {
            move = new Vector3(0f, ActualJumpHeight, JumpMove);
        }

        //Aktualny obrót gracza razy kierunek w którym sie poruszamy (poprawka na obrót myszką abyśmy szli w kierunku w którym patrzymy).
        move = transform.rotation * move;

        characterControler.Move(move * Time.deltaTime);
    }   

    private float Jump(float move)
    {
        //Skakanie
        if (characterControler.isGrounded && Input.GetButton("Jump"))
        {
            animator.SetTrigger("jump");
            ActualJumpHeight = JumpHeight;
            JumpMove = move;
        }
        else if (!characterControler.isGrounded)
        {
            //Jezeli jestesmy w powietrzu(skok)
            //Fizyka odpowiadająca za grawitacje (os Y).            
            ActualJumpHeight += Physics.gravity.y * Time.deltaTime * 2;
        }

        return ActualJumpHeight;
    }

    private void Runing()
    {
        if (Run)
        {
            Velocity = RunningSpeed;
        }
        else
        {
            Velocity = WalkSpeed;
        }
    }

    private void Animation(float param)
    {
        //sterownie animacjami chodzenia/biegania
        animator.SetFloat("velocity", Mathf.Abs(param));
    }
}
