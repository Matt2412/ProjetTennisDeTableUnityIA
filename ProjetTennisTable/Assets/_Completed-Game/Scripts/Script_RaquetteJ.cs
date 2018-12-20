using UnityEngine;
using System.Collections;

public class Script_RaquetteJ : MonoBehaviour
{
    private Vector3 posRaquetteJ;
    public float speed;
    
    void Start()
    {

    }

    void Mouvement_RaquetteJ(Vector3 positionRaquette)
    {
        transform.position = new Vector3 (Mathf.Lerp(transform.position.x, transform.position.x + Input.GetAxis("Mouse X"), 0.2f),
                                         transform.position.y,
                                         Mathf.Lerp(transform.position.z, transform.position.z + Input.GetAxis("Mouse Y"), 0.2f) );
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Balle")
        {
            col.rigidbody.AddForce(transform.forward * 12, ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        posRaquetteJ = GameObject.Find("RaquetteJ").transform.position;//position de la raquette
        Mouvement_RaquetteJ(posRaquetteJ);
        transform.rotation = Quaternion.AngleAxis(180, new Vector3(0,1,0));//toujours garder la face // au filet
    }
}
