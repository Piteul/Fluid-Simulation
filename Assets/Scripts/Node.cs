using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public Vector3 vitesse = new Vector3(0,0,0);
    public Vector3 lastPosition;

    //Node number in the nodes list
    public int number = 0;
    public float mass;


    // Use this for initialization
    void Start () {
        mass = Random.Range(1.0f, 5.0f);

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //time++;

        //vitesse = CalculVitesse(-fg, vitesse);
        //lastPosition = position;
        //position = CalculPosition(vitesse, position);
	}


    public Vector3 CalculVitesse(float _acceleration, Vector3 _vitesseActuel, float _time) {

        Vector3 vitesse;
        Vector3 vecAcceleration = new Vector3(0, _acceleration, 0);
        vitesse = _vitesseActuel + (vecAcceleration * _time);

        //Debug.Log("Vitesse" + vitesse.ToString());
        return vitesse;


    }

    public Vector3 CalculPosition(Vector3 _vitesse, Vector3 _positionActuel, float _time) {

        Vector3 position;

        return position = _positionActuel + (_vitesse * _time);

    }

}
