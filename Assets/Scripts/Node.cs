using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public Vector3 vitesse = new Vector3(0,0,0);
    public Vector3 lastPosition;
    public Vector3 position;

    //Node number in the nodes list
    public int number = 0;
    public int mass = 1;


    // Use this for initialization
    void Start () {

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

    public Vector3 CalculPosition(Vector3 _vitesse, Vector3 _positionActuel) {

        Vector3 position;

        return position = _vitesse + _positionActuel;

    }

}
