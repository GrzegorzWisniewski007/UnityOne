using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacterController : MonoBehaviour
{
    public CharacterController characterControler;

    //Predkosc biegania.
    public float Velocity = 0f;
    //Prędkość poruszania się gracza.
    public float WalkSpeed = 5.0f;
    //Predkosc biegania.
    public float RunningSpeed = 10.0f;
    //Wysokość skoku.
    public float JumpHeight = 7.0f;
    //Aktualna wysokosc skoku.
    public float ActualJumpHeight = 0f;

    /** 
	 * Pobranie prędkości poruszania się przód/tył.
	 * jeżeli wartość dodatnia to poruszamy się do przodu,
	 * jeżeli wartość ujemna to poruszamy się do tyłu.
	 */
    private float ForwardBackMovement;
    /** 
	 * Pobranie prędkości poruszania się lewo/prawo.
	 * jeżeli wartość dodatnia to poruszamy się w prawo,
	 * jeżeli wartość ujemna to poruszamy się w lewo.
	 */
    private float LeftRightMovement;

    /** Zmienna dostarcza informację o tym czy gracz bienie.*/
    public bool Run;

    //Czulość myszki (Sensitivity)
    public float MouseSensitivy = 4.0f;
    public float MouseUpDownMovement = 1.0f;
    //Zakres patrzenia w górę i dół.
    public float MouseUpDownRange = 90.0f;

    // Use this for initialization
    void Start()
    {
        characterControler = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        klawiatura();
        myszka();
    }

    private void klawiatura()
    {
        ForwardBackMovement = Input.GetAxis("Vertical") * Velocity;
        LeftRightMovement = Input.GetAxis("Horizontal") * Velocity;

        //Skakanie
        // Jeżeli znajdujemy się na ziemi i została naciśnięta spacja (skok)
        if (characterControler.isGrounded && Input.GetButton("Jump"))
        {
            ActualJumpHeight = JumpHeight;
        }
        else if (!characterControler.isGrounded)
        {//Jezeli jestesmy w powietrzu(skok)
         //Fizyka odpowiadająca za grawitacje (os Y).
            ActualJumpHeight += Physics.gravity.y * Time.deltaTime;
        }

        //Bieganie
        if (Input.GetKeyDown("left shift"))
        {
            Run = true;
        }
        else if (Input.GetKeyUp("left shift"))
        {
            Run = false;
        }

        if (Run)
        {
            Velocity = RunningSpeed;
        }
        else
        {
            Velocity = WalkSpeed;
        }

        //Tworzymy wektor odpowiedzialny za ruch.
        //rochLewoPrawo - odpowiada za ruch lewo/prawo,
        //aktualnaWysokoscSkoku - odpowiada za ruch góra/dół,
        //rochPrzodTyl - odpowiada za ruch przód/tył.
        Vector3 ruch = new Vector3(LeftRightMovement, ActualJumpHeight, ForwardBackMovement);
        
        //Aktualny obrót gracza razy kierunek w którym sie poruszamy (poprawka na obrót myszką abyśmy szli w kierunku w którym patrzymy).
        ruch = transform.rotation * ruch;

        characterControler.Move(ruch * Time.deltaTime);

        //Debug.Log(Input.GetAxis("Horizontal"));
        transform.Rotate(0f, Input.GetAxis("Horizontal")* 90f, 0f);

    }

    private void myszka()
    {
        //Pobranie wartości ruchu myszki lewo/prawo.
        // jeżeli wartość dodatnia to poruszamy w prawo,
        // jeżeli wartość ujemna to poruszamy w lewo.
        float myszLewoPrawo = Input.GetAxis("Mouse X") * MouseSensitivy;
        transform.Rotate(0, myszLewoPrawo, 0);

        //Pobranie wartości ruchu myszki góra/dół.
        // jeżeli wartość dodatnia to poruszamy w górę,
        // jeżeli wartość ujemna to poruszamy w dół.
        MouseUpDownMovement -= Input.GetAxis("Mouse Y") * MouseSensitivy;

        //Funkcja nie pozwala aby wartość przekroczyła dane zakresy.
        MouseUpDownMovement = Mathf.Clamp(MouseUpDownMovement, -MouseUpDownRange, MouseUpDownRange);
        //Ponieważ CharacterController nie obraca się góra/dół obracamy tylko kamerę.
        //Camera.main.transform.localRotation = Quaternion.Euler(MouseUpDownMovement, 0, 0);
    }

}
