using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodes_Simulation : MonoBehaviour {

    //Nodes
    public List<Node> nodes;
    public GameObject model;
    public int nodesNumbers = 0;

    float limitX = 3.5f; //Valeur limite en X lors de la création des nodes
    float limitWall = 4f; //Valeur d'impact des murs
    const float fg = 1f; //Force Gravitationnelle

    float spawnTime = 100f;


    //Use for the Double Density Relaxation
    public float H;
    public float Rho0;
    public float K;
    public float KNear;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        PoppingNodes();
        FluidGestion();

    }

    // Update is called once per frame
    void FixedUpdate() {


    }

    public void FluidGestion() {
        foreach (Node node in nodes) {

            //Debug.Log("N° " + node.number + " : " + node.vitesse.ToString());

            node.vitesse = node.CalculVitesse(-fg, node.vitesse, Time.fixedDeltaTime);
            //Debug.Log("Vitesse :" + node.vitesse);

            node.lastPosition = node.gameObject.transform.position;
            //Debug.Log("LastPosition :" + node.lastPosition);

            node.gameObject.transform.position = node.CalculPosition(node.vitesse, node.gameObject.transform.position, Time.fixedDeltaTime);
            //Debug.Log("Position :" + node.position);
        }

        CollisionGestion();

        DoubleDensityRelaxation();

        foreach (Node node in nodes) {

            node.vitesse = (node.gameObject.transform.position - node.lastPosition) / Time.fixedDeltaTime;

        }


    }

    public void PoppingNodes() {
        float timer = 0f;

        if (timer <= 0.0f) {
            if (Input.GetMouseButton(0)) {
                Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 nodePosition = new Vector3(cursorPosition.x, cursorPosition.y, 0.0f);
                GameObject node = Instantiate(model, nodePosition, Quaternion.identity, transform);
                node.GetComponent<Node>().lastPosition = node.transform.position;
                nodes.Add(node.GetComponent<Node>());

                CollisionGestion();
                timer = spawnTime;
            }
        }
        else {
            timer -= Time.fixedDeltaTime;
        }
    }

    /// <summary>
    /// ApplySpringDisplacements
    /// </summary>
    public void CollisionGestion() {

        foreach (Node node in nodes) {
            Vector3 tmp = Vector3.zero;

            //Sol
            if (node.gameObject.transform.position.y < -limitWall) {
                Debug.Log("Collision sol");
                tmp.y += -limitWall - node.gameObject.transform.position.y;
            }
            //Mur gauche
            else if (node.gameObject.transform.position.x < -limitWall) {
                Debug.Log("Collision mur gauche");
                tmp.x += -limitWall - node.gameObject.transform.position.x;
            }
            //Mur droit
            else if (node.gameObject.transform.position.x > limitWall) {
                Debug.Log("Collision mur droit");
                tmp.x -= node.gameObject.transform.position.x - limitWall;
            }
            node.gameObject.transform.position += Time.fixedDeltaTime * Time.fixedDeltaTime * tmp * 100;
        }
    }

    public void DoubleDensityRelaxation() {
        foreach (Node ni in nodes) {
            float rho = 0.0f;
            float rhoNear = 0.0f;
            foreach (Node nj in nodes) {
                if (nj != ni) {
                    Vector3 Rij = nj.transform.position - ni.transform.position;
                    float q = Rij.magnitude / H;
                    if (q < 1) {
                        rho = rho + Mathf.Pow(1 - q, 2);
                        rhoNear = rhoNear + Mathf.Pow(1 - q, 3);
                    }
                }
            }
            float P = K * (rho - Rho0);
            float PNear = KNear * rhoNear;
            Vector3 di = Vector3.zero;
            foreach (Node nj in nodes) {
                if (nj != ni) {
                    Vector3 Rij = nj.transform.position - ni.transform.position;
                    float q = Rij.magnitude / H;
                    if (q < 1) {
                        Vector3 D = Mathf.Pow(Time.fixedDeltaTime, 2) * (P * (1 - q) + PNear * Mathf.Pow(1 - q, 2)) * Rij.normalized;
                        nj.transform.position = nj.transform.position + (D / 2) / nj.mass;
                        di = di - (D / 2) / ni.mass;
                    }
                }
            }
            ni.transform.position = ni.transform.position + di;
        }
    }

}

//public void NodesGeneration() {

//    for (int i = 0; i < nodes.Length; i++) {
//        GameObject node = Instantiate(model, gameObject.transform);
//        nodes[i] = node.GetComponent<Node>();
//        node.transform.position += new Vector3(Random.Range(-limitX, limitX), 0);
//        nodes[i].lastPosition = node.transform.position;

//        nodes[i].number = i;
//    }
//}
//public void CollisionAlt() {
//    foreach (Particle p in particles) {
//        if (p.transform.position.x < -20.0f)
//            p.transform.position = new Vector3(Random.Range(-20.0f, -19.9f), p.transform.position.y, 0.0f);
//        else if (p.transform.position.x > 20.0f)
//            p.transform.position = new Vector3(Random.Range(19.9f, 20.0f), p.transform.position.y, 0.0f);

//        if (p.transform.position.y < -25.0f)
//            p.transform.position = new Vector3(p.transform.position.x, Random.Range(-25.0f, -24.9f), 0.0f);
//    }
//}