using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileMyCharacterController : MonoBehaviour
{
    public CharacterController characterControler;

    //Prędkość poruszania się gracza.
    public float WalkSpeed = 5.0f;
    //Predkosc biegania.
    public float RunningSpeed = 10.0f;
    //Wysokość skoku.
    public float JumpHeight = 7.0f;
    //Aktualna wysokosc skoku.
    public float ActualJumpHeight = 0f;

    //przyciski
    private bool ForwardButton = false;
    private bool BackwardButton = false;
    private bool LeftButton = false;
    private bool RightButton = false;   

    //Predkosc.
    public float Velocity = 0f;

    private float JumpMove = 0.0f;

    private bool JumpButton = false;
    private bool InAir = false;

    private Animator animator;

    private bool start;
    float count=10.0f;    

    /** Zmienna dostarcza informację o tym czy gracz bienie.*/
    public bool Run;

    // Use this for initialization
    void Start()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            //this.enabled = false;
        }
        animator = (Animator)GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ForwardButton)
        {
            MoveForward();
        }
        if (BackwardButton)
        {
            MoveBackward();
        }
        if (LeftButton)
        {
            Turn(-1);
        }
        if (RightButton)
        {
            Turn(1);
        }

        if (JumpButton)
        {
            if (!ForwardButton && !BackwardButton)
            {
                Jump(0);
                Move(0);
            }
            if (characterControler.isGrounded && InAir)
            {
                JumpButton = false;
            }
        }
        if (start)
        {
            count += Time.deltaTime;
        }
        
    }

    public void onPointerDownForwardButton()
    {
        if (count < 1)
        {
            Run = true;
            count = 10.0f;
            start = false;
        }
        else
        {
            Run = false;
            count = 10.0f;
        }
        ForwardButton = true;
    }
    public void onPointerUpForwardButton()
    {
        start = true;
        count = 0.0f;
        Animation(0);
        ForwardButton = false;
    }

    public void onPointerDownBackwardButton()
    {
        BackwardButton = true;
    }
    public void onPointerUpBackwardButton()
    {
        Animation(0);
        BackwardButton = false;
    }

    public void onPointerDownLeftButton()
    {
        LeftButton = true;
    }
    public void onPointerUpLeftButton()
    {
        Animation(0);
        LeftButton = false;
    }

    public void onPointerDownRightButton()
    {
        RightButton = true;
    }
    public void onPointerUpRightButton()
    {
        Animation(0);
        RightButton = false;
    }

    public void onClickJumpButton()
    {
        JumpButton = true;
    }

    private void Animation(float param)
    {
        //sterownie animacjami chodzenia/biegania
        animator.SetFloat("velocity", Mathf.Abs(param));
    }

    private void MoveForward()
    {
        Jump(Velocity);
        Runing();
        //Debug.Log(Velocity);
        Animation(Velocity);
        Move(Velocity);
    }

    private void MoveBackward()
    {
        Jump(-Velocity);
        Move(-Velocity);
        Animation(Velocity);
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

    private void Turn(float direction)
    {
        if (characterControler.isGrounded)
        {
            transform.Rotate(0, direction > 0 ?  5 : -5, 0);
        }
    }

    private float Jump(float move)
    {
        //Skakanie
        if (characterControler.isGrounded && JumpButton)
        {
            animator.SetTrigger("jump");
            ActualJumpHeight = JumpHeight;
            JumpMove = move;
        }
        else if (!characterControler.isGrounded)
        {
            //Jezeli jestesmy w powietrzu(skok)
            //Fizyka odpowiadająca za grawitacje (os Y).
            InAir = true;
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
}
