using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camSeguirJugador : MonoBehaviour
{
    public Transform objetivo;

    private void LateUpdate()
    {
        transform.position = new Vector3(objetivo.transform.position.x, 7, -15);
    }
}
