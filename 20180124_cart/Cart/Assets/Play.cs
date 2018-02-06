using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play : MonoBehaviour {
	public GameObject tire_fl;
	public GameObject tire_fr;
	public GameObject tire_rl;
	public GameObject tire_rr;

	private HingeJoint hinge_fl;
	private HingeJoint hinge_fr;
	private HingeJoint hinge_rl;
	private HingeJoint hinge_rr;

	private JointMotor motor_fl;
	private JointMotor motor_fr;
	private JointMotor motor_rl;
	private JointMotor motor_rr;

	// Use this for initialization
	void Start () {
		hinge_fl = tire_fl.GetComponent<HingeJoint> ();
		hinge_fr = tire_fr.GetComponent<HingeJoint> ();
		hinge_rl = tire_rl.GetComponent<HingeJoint> ();
		hinge_rr = tire_rr.GetComponent<HingeJoint> ();

		hinge_fl.useMotor = true;
		hinge_rl.useMotor = true;
		hinge_fr.useMotor = true;
		hinge_rr.useMotor = true;

		motor_fl = hinge_fl.motor;
		motor_fr = hinge_fr.motor;
		motor_rl = hinge_rl.motor;
		motor_rr = hinge_rr.motor;

		motor_fl.freeSpin = false;
		motor_rl.freeSpin = false;
		motor_fr.freeSpin = false;
		motor_rr.freeSpin = false;

		hinge_fl.motor = motor_fl;
		hinge_fr.motor = motor_fr;
		hinge_rl.motor = motor_rl;
		hinge_rr.motor = motor_rr;
	}
	
	// Update is called once per frame
	void Update () {
		motor_fl.force = 0;
		motor_rl.force = 0;
		motor_fr.force = 0;
		motor_rr.force = 0;
		int Target = 5000;
		int force = 100;

		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.S)) {
			motor_fl.force = force;
			motor_rl.force = force;
			motor_fr.force = force;
			motor_rr.force = force;
		}

		if (Input.GetKey (KeyCode.W)) {
			motor_fl.targetVelocity = -1 * Target;
			motor_rl.targetVelocity = -1 * Target;
			motor_fr.targetVelocity = Target;
			motor_rr.targetVelocity = Target;
		}

		if (Input.GetKey (KeyCode.S)) {
			motor_fl.targetVelocity = Target;
			motor_rl.targetVelocity = Target;
			motor_fr.targetVelocity = -1 * Target;
			motor_rr.targetVelocity = -1 * Target;
		}

		if (Input.GetKey (KeyCode.D)) {
			motor_fl.targetVelocity = -1 * Target;
			motor_rl.targetVelocity = -1 * Target;
			motor_fr.targetVelocity = -1 * Target;
			motor_rr.targetVelocity = -1 * Target;
		}

		if (Input.GetKey (KeyCode.A)) {
			motor_fl.targetVelocity = Target;
			motor_rl.targetVelocity = Target;
			motor_fr.targetVelocity = Target;
			motor_rr.targetVelocity = Target;
		}

		hinge_fl.motor = motor_fl;
		hinge_rl.motor = motor_rl;
		hinge_fr.motor = motor_fr;
		hinge_rr.motor = motor_rr;
	}

	//オブジェクトが衝突したとき
	void OnTriggerEnter(Collider other) {
	}
}
