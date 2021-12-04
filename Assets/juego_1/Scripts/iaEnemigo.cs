using UnityEngine;
using UnityEngine.AI;

public class iaEnemigo : MonoBehaviour
{
    public float speed = 2;

    public int saludEnemigo = 100;

    public bool atacando = false;
    public bool ataca = false;
    public bool puedeAtacar = false;

    public Transform objetivo;
    public Rigidbody enemigo;
    public GameObject manoEnemigo;
    public NavMeshAgent ruta;
    public Animator animaciones;
    public TextMesh textoVidaFlotante;

    private void Awake()
    {
        enemigo = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        float dist = Vector3.Distance(transform.position, objetivo.transform.position);
        if ((dist < 10f && !atacando) && (!atacando && dist > 2f))
        {
            ruta.destination = objetivo.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(objetivo.transform.position.x - transform.position.x, 0, objetivo.transform.position.z - transform.position.z)), speed * Time.deltaTime);
        }

        if (dist < 2f && !atacando)
        {
            atacando = true;
            if (atacando)
            {
                puedeAtacar = true;
                ataca = true;
                if (puedeAtacar)
                {
                    Debug.Log("Voy a atacarte");
                    Invoke("HacerPupa", 1f);
                    puedeAtacar = false;
                }
            }
        }
        else if (dist > 2f)
        {
            atacando = false;
            ataca = false;
            puedeAtacar = false;
        }
        ControlTextoFlotante();
        ControlSalud();
    }

    void ControlSalud()
    {
        if (saludEnemigo < 1)
        {
            saludEnemigo = 0;
            Destroy(this.gameObject);
            Destroy(textoVidaFlotante);
        }
    }

    void ControlTextoFlotante()
    {
        textoVidaFlotante.transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
        textoVidaFlotante.text = "" + saludEnemigo;
    }

    public void HacerPupa()
    {
        Collider[] hitColliders = Physics.OverlapSphere(manoEnemigo.transform.position, 1f);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject.tag == "Player")
            {
                print("Jugador detectado");
                
                hitColliders[i].GetComponent<Rigidbody>().AddExplosionForce(4, transform.position, 5, 2.0F, ForceMode.Impulse);
                hitColliders[i].GetComponent<controlJugador>().saludJugador -= Random.Range(5, 20);
                hitColliders[i].GetComponent<Animator>().SetTrigger("Golpeado");
            }
            i++;
        }
        puedeAtacar = true;
    }
}
