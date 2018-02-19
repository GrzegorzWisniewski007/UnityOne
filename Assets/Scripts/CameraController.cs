using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{  

    public Transform lookAt;
    
    
    private void Update()
    {
        //pobieramy komponen fizyki z kuli
        Rigidbody rigitbody = lookAt.GetComponent<Rigidbody>();
        //nowa pozycja dla kamery (obok kuli)
        Vector3 vector = new Vector3(5f, 4f, 0);
        //pobieramy predkosc kuli i przeliczmy 
        float velocity = rigitbody.velocity.sqrMagnitude;
        //zmieniamy odległosc kamery tak zeby zależała od predkości kuli
        vector = vector * (1f + velocity / 55f);
        //nowa pozycja kamery
        Vector3 newPosition = lookAt.position + vector;
        //nadajemy płynne poruszanie sie kamery (aktualna pozycja kamery, nowa pozycja kamery, przesuniecie kamery o (1sek./xFPS) *2
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 2);        
        //kamera patrzy na kulę
        transform.LookAt(lookAt);
        transform.Rotate(-20, 0, 0);
        //Debug.Log(Application.platform);

    }    
}
