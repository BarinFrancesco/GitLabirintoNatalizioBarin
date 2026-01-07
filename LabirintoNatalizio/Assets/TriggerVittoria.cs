using UnityEngine;

public class TriggerVittoria : MonoBehaviour
{
    [SerializeField] public Transform player;     // Player da spostare
    [SerializeField] public GeneratoreLabirinto generatore; // Riferimento al generatore

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            // disabilitamo temporaneamente il CharacterController e spostiamo il personaggio
            CharacterController cc = other.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false;
                other.transform.position = new Vector3(0f, 10f, -10f);
                other.transform.rotation = Quaternion.identity;
            }
            if (cc != null) cc.enabled = true; //riattiviamolo

            // chiediamo al generatore di creare una nuova mappa
            if (generatore != null)
            {
                generatore.lato += 2;
                generatore.CreaMappa();
            }
        }
    }

}
