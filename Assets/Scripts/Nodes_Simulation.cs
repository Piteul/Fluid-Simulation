using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodes_Simulation : MonoBehaviour {

    public Node[] nodes;
    public GameObject model;
    public int nodesNumbers = 1;
    public float limitX = 3.5f; //Valeur limite en X lors de la création des nodes
    public float limitWall = 4f; //Valeur d'impact des murs
    public const float fg = 1f; //Force Gravitationnelle

    // Use this for initialization
    void Start() {

        nodes = new Node[nodesNumbers];

        if (nodes.Length < 1) {
            Debug.Log("Erreur, chaine impossible");
            Application.Quit();
        }

        NodesGeneration();


    }

    // Update is called once per frame
    void FixedUpdate() {

        foreach (Node node in nodes) {

            //Debug.Log("N° " + node.number + " : " + node.vitesse.ToString());

            node.vitesse = node.CalculVitesse(-fg, node.vitesse, Time.deltaTime);
            //Debug.Log("Vitesse :" + node.vitesse);

            node.lastPosition = node.position;
            //Debug.Log("LastPosition :" + node.lastPosition);

            node.position = node.CalculPosition(node.vitesse, node.position);
            //Debug.Log("Position :" + node.position);


            //Collision

            //Sol
            if (node.position.y < -limitWall) {

                Debug.Log("Collision sol");
                float distanceAfterWall = -limitWall - node.gameObject.transform.position.y;
                Debug.Log(distanceAfterWall.ToString());

                //node.position += new Vector3(0, distanceAfterWall * 100 * (Time.deltaTime) * (Time.deltaTime), 0);
            }
            //Mur gauche
            else if (node.position.x < -limitWall) {
                Debug.Log("Collision mur gauche");
                float distanceAfterWall = -limitWall - node.gameObject.transform.position.y;
                Debug.Log(distanceAfterWall.ToString());

                node.position += new Vector3(0, distanceAfterWall * 1 * (Time.deltaTime) * (Time.deltaTime), 0);


            }
            //Mur droit
            else if (node.position.x > limitWall) {
                Debug.Log("Collision mur droit");
                float distanceAfterWall = -limitWall - node.gameObject.transform.position.y;
                Debug.Log(distanceAfterWall.ToString());

                node.position += new Vector3(0, distanceAfterWall * 1 * (Time.deltaTime) * (Time.deltaTime), 0);

            }

            node.gameObject.transform.position = node.position;

            //ode.vitesse = node.CalculVitesse(-fg,(node.position - node.lastPosition) / (Time.deltaTime), Time.deltaTime);


        }

        //foreach (Node node in nodes) {

        //    node.vitesse = (node.position - node.lastPosition) / Time.deltaTime;

        //}
    }


    public bool Collision(Node _node) {

        if (_node.position.y < -limitWall || _node.position.x < -limitWall || _node.position.x > limitWall) {
            return true;
        }
        else {
            return false;
        }
    }

    public void NodesGeneration() {

        for (int i = 0; i < nodes.Length; i++) {
            GameObject node = Instantiate(model, gameObject.transform);
            nodes[i] = node.GetComponent<Node>();
            node.transform.position += new Vector3(Random.Range(-limitX, limitX), 0);
            nodes[i].position = node.transform.position;
            nodes[i].lastPosition = nodes[i].position;

            nodes[i].number = i;
        }
    }
}
