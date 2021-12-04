using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controlJugador : MonoBehaviour
{
    public int saludJugador = 100;

    public float velocidad = 5;
    public float fuerzaSalto = 5;

    public bool saltando = false;

    public GameObject mano;
    public TextMesh textoVidaFlotante;
    public Rigidbody personaje;
    public Animator animaciones;

    private float ejeX;
    private float ejeZ;
    private float camina;
    private bool golpeando;

    void Awake()
    {
        personaje = GetComponent<Rigidbody>();
        animaciones = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Movimiento();
        ControlTextoFlotante();
        ControlSalud();
    }

    void ControlSalud()
    {
        if (saludJugador < 0)
        {
            saludJugador = 0;
        }
    }

    void ControlTextoFlotante()
    {
        textoVidaFlotante.transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
        textoVidaFlotante.text = "" + saludJugador;
    }

    void Movimiento()
    {
        ejeX = Input.GetAxis("Horizontal");
        ejeZ = Input.GetAxis("Vertical");

        if (ejeX != 0 || ejeZ != 0)
        {
            camina = 1;
            animaciones.SetFloat("Caminando", camina);
        }
        else
        {
            camina = 0;
            animaciones.SetFloat("Caminando", camina);
        }

        if (personaje.velocity.magnitude <= 0)
        {
            personaje.velocity = Vector3.zero;
        }

        Vector3 movement = new Vector3(ejeX, 0.0f, ejeZ);

        if(!golpeando) personaje.MovePosition(new Vector3(transform.position.x + ejeX * velocidad * Time.deltaTime, transform.position.y, transform.position.z + ejeZ * velocidad * Time.deltaTime));
        
        if (movement != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.normalized), 0.2f);

        if (Input.GetKeyDown(KeyCode.Space) && !saltando)
        {
            saltando = true;
            personaje.AddForce(new Vector3(0, fuerzaSalto, 0), ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            
            animaciones.SetTrigger("Golpear");
            golpeando = true;
            Collider[] hitColliders = Physics.OverlapSphere(mano.transform.position, 1f);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if(hitColliders[i].gameObject.tag == "Enemy")
                {
                    print("Enemigo ' " + hitColliders[i] + " ' golpeado");
                    hitColliders[i].transform.Translate(Vector3.back, Space.Self);
                    hitColliders[i].GetComponent<iaEnemigo>().saludEnemigo -= Random.Range(5, 20); 
                    hitColliders[i].GetComponent<Animator>().SetTrigger("Golpeado");
                }
                i++;
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Suelo")
        {
            saltando = false;
        }
    }
}
