using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GeneratoreLabirinto : MonoBehaviour
{
    [SerializeField]
    private Cella stampoCella;

    [SerializeField]
    private Material MaterialeCellaVittoria;

    [SerializeField]
    public int lato;

    [SerializeField] private Transform player;

    private Cella[,] mappa;
    private Cella cellaInizio;
    private Cella cellaVittoria;

    void Start()
    {
        CreaMappa();
    }

    public void CreaMappa()
    {
        Debug.Log("Hai vinto!");
        SvuotaMappa();

        mappa = new Cella[lato, lato];

        for (int i = 0; i < lato; i++)
        {
            for (int j = 0; j < lato; j++)
            {
                mappa[i, j] = Instantiate(stampoCella, new Vector3(i * 10, 10, j * 10), Quaternion.identity);
            }
        }

        //creiamo la prima cella(dell'inizio) e mettiamoci dentro il giocatore
        mappa[0, 0].DisattivaMuroDietro();
        cellaInizio = Instantiate(stampoCella, new Vector3(mappa[0, 0].transform.position.x, 10, mappa[0, 0].transform.position.z - 10), Quaternion.identity);
        cellaInizio.Visita();
        cellaInizio.DisattivaMuroDavanti();

        GeneraLabirinto(null, mappa[0, 0]);

        int CellaUscita = Random.Range(1, (lato * 4)); 

        if (CellaUscita <= lato)
        {
            CellaUscita -= 1;
            mappa[0, CellaUscita].DisattivaMuroSinistra();
            cellaVittoria = Instantiate(stampoCella, new Vector3(mappa[0, CellaUscita].transform.position.x - 10, 10, mappa[0, CellaUscita].transform.position.z), Quaternion.identity);
            cellaVittoria.DisattivaMuroDestra();
        }
        else if (CellaUscita <= (2 * lato))
        {
            CellaUscita -= (lato + 1);
            mappa[CellaUscita, lato - 1].DisattivaMuroDavanti();
            cellaVittoria = Instantiate(stampoCella, new Vector3(mappa[CellaUscita, lato - 1].transform.position.x, 10, mappa[CellaUscita, lato - 1].transform.position.z + 10), Quaternion.identity);
            cellaVittoria.DisattivaMuroDietro();
        }
        else if (CellaUscita <= (3 * lato))
        {
            CellaUscita -= ((2 * lato) + 1);
            mappa[lato - 1, CellaUscita].DisattivaMuroDestra();
            cellaVittoria = Instantiate(stampoCella, new Vector3(mappa[lato - 1, CellaUscita].transform.position.x + 10, 10, mappa[lato - 1, CellaUscita].transform.position.z), Quaternion.identity);
            cellaVittoria.DisattivaMuroSinistra();
        }
        else
        {
            CellaUscita -= ((3 * lato) + 1);
            mappa[CellaUscita, 0].DisattivaMuroDietro();
            cellaVittoria = Instantiate(stampoCella, new Vector3(mappa[CellaUscita, 0].transform.position.x, 10, mappa[CellaUscita, 0].transform.position.z - 10), Quaternion.identity);
            cellaVittoria.DisattivaMuroDavanti();
        }

        cellaVittoria.Visita();
        ImpostaCellaVittoria(cellaVittoria);
       
    }

    private void SvuotaMappa()
    {
        if (mappa == null)
            return;

        for (int i = 0; i < mappa.GetLength(0); i++)
        {
            for (int j = 0; j < mappa.GetLength(1); j++)
            {
                if (mappa[i, j] != null)
                {
                    Destroy(mappa[i, j].gameObject);
                }
            }
        }

        mappa = null;

        if (cellaInizio != null)
        {
            Destroy(cellaInizio.gameObject);
            cellaInizio = null;
        }

        if (cellaVittoria != null)
        {
            Destroy(cellaVittoria.gameObject);
            cellaVittoria = null;
        }
    }

    private void ImpostaCellaVittoria(Cella cella)//modifica la cella le cambia l'aspetto e aggiunge un trigger
    {
        cella.ImpostaMateriale(MaterialeCellaVittoria);

        BoxCollider col = cella.gameObject.AddComponent<BoxCollider>();
        col.isTrigger = true;

        var trigger = cella.gameObject.AddComponent<TriggerVittoria>();
        trigger.player = player;       
        trigger.generatore = this;
    }

    private void GeneraLabirinto( Cella cellaPrecedente, Cella cellaCorrente)
    {
        cellaCorrente.Visita();
        AbbattiMuro(cellaPrecedente, cellaCorrente);

        Cella prossimaCella;

        do
        {
            prossimaCella = OttieniNuovaCella(cellaCorrente);

            if (prossimaCella != null)
            {
                 GeneraLabirinto(cellaCorrente, prossimaCella);
            }

        } while (prossimaCella != null);
        
    }


    private Cella OttieniNuovaCella(Cella cellaCorrente)
    {
        var celleDaEsplorare = CelleNonVisitate(cellaCorrente);
        return celleDaEsplorare.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault() ;
    }

    private IEnumerable<Cella> CelleNonVisitate(Cella cellaCorrente)
    {
        int x = (int)cellaCorrente.transform.position.x;
        int z = (int)cellaCorrente.transform.position.z;
        int xIndice = x / 10;
        int zIndice = z / 10;

        if(xIndice+1 < lato)
        {
            var cellaDestra = mappa[xIndice + 1, zIndice];

            if(cellaDestra.IsVisited == false)
            {
                yield return cellaDestra;
            }
        }

        if(xIndice-1 >= 0)
        {
            var cellaSinistra = mappa[xIndice - 1, zIndice];

            if(cellaSinistra.IsVisited == false)
            {
                yield return cellaSinistra;
            }
        }

        if(zIndice+1 < lato)
        {
            var cellaDavanti = mappa[xIndice, zIndice+1];

            if(cellaDavanti.IsVisited == false)
            {
                yield return cellaDavanti;
            }
        } 

        if(zIndice -1 >= 0)
        {
            var cellaDietro = mappa[xIndice, zIndice - 1];

            if(cellaDietro.IsVisited == false)
            {
                yield return cellaDietro;
            }
        }
    }

    private void AbbattiMuro(Cella cellaPrecedente, Cella cellaCorrente)
    {
        if(cellaPrecedente == null)
        {
            return;
        } 

        if(cellaPrecedente.transform.position.x < cellaCorrente.transform.position.x)
        {
            cellaPrecedente.DisattivaMuroDestra();
            cellaCorrente.DisattivaMuroSinistra();
            return;
        }

        if (cellaPrecedente.transform.position.x > cellaCorrente.transform.position.x)
        {
            cellaPrecedente.DisattivaMuroSinistra();
            cellaCorrente.DisattivaMuroDestra();
            return;
        }

        if (cellaPrecedente.transform.position.z < cellaCorrente.transform.position.z)
        {
            cellaPrecedente.DisattivaMuroDavanti();
            cellaCorrente.DisattivaMuroDietro();
            return;
        }

        if (cellaPrecedente.transform.position.z > cellaCorrente.transform.position.z)
        {
            cellaPrecedente.DisattivaMuroDietro();
            cellaCorrente.DisattivaMuroDavanti();
            return;
        }
    }    

    // Update is called once per frame
    void Update()
    {
        
    }
}
