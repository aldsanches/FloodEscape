using UnityEngine;
using System.Collections;

public class Pegando: MonoBehaviour {
    
    public float pickUpDist = 1f; //Distancia do objeto que vai ser pego

    private Transform carriedObject = null;
    public GameObject[] todaMobilia;

    private void Start() {
        todaMobilia = GameObject.FindGameObjectsWithTag("Moveis");     
    }

    private void Update() {
        if (Input.GetKeyDown("f")) {
            
            if (carriedObject != null) //Se tiver algo na mão, solta
                Drop();
            else //Se não tiver nada na mão, pega
                PickUp();
        }
    }

    private void Drop() {
        carriedObject.parent = null; //Tira o parentesco
        carriedObject.gameObject.AddComponent(typeof(BoxCollider2D)); //Reestabelece collider
        carriedObject.gameObject.AddComponent(typeof(Rigidbody2D)); //Reestabelece gravidade
        float playerX = gameObject.transform.position.x;
        float playerY = gameObject.transform.position.y;
        carriedObject.transform.position = new Vector3(playerX + 1f, playerY, 0);
        carriedObject = null; //Esvazia as mãos novamente
    }

    private void PickUp() {

        //Encontra o objeto mais próximo
        float dist = Mathf.Infinity;
        for (int i = 0; i < todaMobilia.Length; i++) {
            float newDist = (gameObject.transform.position - todaMobilia[i].transform.position).sqrMagnitude;
            if (newDist < dist) {
                carriedObject = todaMobilia[i].transform;
                dist = newDist;
            }
        }

        if (carriedObject != null) //Se encontrar alguma coisa
        {
            //Coloca o objeto como parente do player
            Destroy(carriedObject.GetComponent<Rigidbody2D>());
            Destroy(carriedObject.GetComponent<Collider2D>());
            carriedObject.parent = transform;
            carriedObject.localPosition = new Vector3(0, 0.9f, 1f); //Em cima da cabeça (tem que mudar conforme o tamanho)
        }
    }
}