using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{  

    public Transform lookAt;
    
    
    private void Update()
    {
        //player
        Rigidbody rigidbody = lookAt.GetComponent<Rigidbody>();
        //nowa pozycja dla kamery 
        //Vector3 vector = new Vector3(5f, 4f, 0);
        Vector3 vector = new Vector3(0, 4f, -8f);
        //pobieramy predkosc i przeliczmy 
        float velocity = rigidbody.velocity.sqrMagnitude;
        //zmieniamy odległosc kamery tak zeby zależała od predkości 
        vector = vector * (1f + velocity / 55f);
        //nowa pozycja kamery
        Vector3 newPosition = lookAt.position + vector;
        //nadajemy płynne poruszanie sie kamery (aktualna pozycja kamery, nowa pozycja kamery, przesuniecie kamery o (1sek./xFPS) *2
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 2);
        Quaternion newRotation = new Quaternion(0, 0, 0, 0);
        //transform.rotation = Quaternion.Lerp(transform.rotation, newRotation,Time.deltaTime *2);        
        //kamera patrzy na kulę
        transform.LookAt(lookAt);
        transform.rotation = newRotation;
        //transform.Rotate(-20, 0, 0);
        //Debug.Log(Application.platform);

    }    
}
