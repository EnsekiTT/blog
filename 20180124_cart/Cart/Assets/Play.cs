using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play : MonoBehaviour {
	public GameObject tire_fl;
	public GameObject tire_fr;
	public GameObject tire_rl;
	public GameObject tire_rr;

	// Use this for initialization
	void Start () {
		HingeJoint hinge_fl = tire_fl.GetComponent<HingeJoint> ();
		HingeJoint hinge_fr = tire_fr.GetComponent<HingeJoint> ();
		HingeJoint hinge_rl = tire_rl.GetComponent<HingeJoint> ();
		HingeJoint hinge_rr = tire_rr.GetComponent<HingeJoint> ();

		JointMotor motor_fl = hinge_fl.motor;
		JointMotor motor_fr = hinge_fr.motor;
		JointMotor motor_rl = hinge_rl.motor;
		JointMotor motor_rr = hinge_rr.motor;

		motor_fl.force = -500;
		motor_fl.targetVelocity = 900;
		motor_fl.freeSpin = false;
		hinge_fl.motor = motor_fl;
		hinge_fl.useMotor = true;

		motor_fr.force = 500;
		motor_fr.targetVelocity = 900;
		motor_fr.freeSpin = false;
		hinge_fr.motor = motor_fr;
		hinge_fr.useMotor = true;

		motor_rl.force = -500;
		motor_rl.targetVelocity = 900;
		motor_rl.freeSpin = false;
		hinge_rl.motor = motor_rl;
		hinge_rl.useMotor = true;

		motor_rr.force = 500;
		motor_rr.targetVelocity = 900;
		motor_rr.freeSpin = false;
		hinge_rr.motor = motor_rr;
		hinge_rr.useMotor = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
