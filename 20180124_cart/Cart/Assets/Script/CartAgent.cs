using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartAgent : Agent {
	public GameObject Cart;
	public GameObject Goal;
	public GameObject Tire_rl;
	public GameObject Tire_rr;

	private HingeJoint hinge_rl;
	private HingeJoint hinge_rr;

	private JointMotor motor_rl;
	private JointMotor motor_rr;

	private int frames = 0;

	public const float MaxTarget = 3000f;
	public const float MaxForce = 100f;

	public override List<float> CollectState()
	{
		List<float> state = new List<float>();
		state.Add (Mathf.Abs (Goal.transform.localPosition.x - Cart.transform.localPosition.x)/ 40f);
		state.Add (Mathf.Abs (Goal.transform.localPosition.y - Cart.transform.localPosition.y)/ 40f);
		state.Add (Mathf.Abs (Goal.transform.localPosition.z - Cart.transform.localPosition.z)/ 40f);
		state.Add (Cart.transform.GetComponent<Rigidbody> ().velocity.x);
		state.Add (Cart.transform.GetComponent<Rigidbody> ().velocity.y);
		state.Add (Cart.transform.GetComponent<Rigidbody> ().velocity.z);
		state.Add (Cart.transform.localRotation.x);
		state.Add (Cart.transform.localRotation.y);
		state.Add (Cart.transform.localRotation.z);
		state.Add (Tire_rl.transform.GetComponent<Rigidbody> ().angularVelocity.x);
		state.Add (Tire_rl.transform.GetComponent<Rigidbody> ().angularVelocity.y);
		state.Add (Tire_rl.transform.GetComponent<Rigidbody> ().angularVelocity.z);
		state.Add (Tire_rr.transform.GetComponent<Rigidbody> ().angularVelocity.x);
		state.Add (Tire_rr.transform.GetComponent<Rigidbody> ().angularVelocity.y);
		state.Add (Tire_rr.transform.GetComponent<Rigidbody> ().angularVelocity.z);

		return state;
	}

	public override void InitializeAgent ()
	{
		Debug.Log ("Start");
		hinge_rl = Tire_rl.GetComponent<HingeJoint> ();
		hinge_rr = Tire_rr.GetComponent<HingeJoint> ();

		hinge_rl.useMotor = true;
		hinge_rr.useMotor = true;

		motor_rl = hinge_rl.motor;
		motor_rr = hinge_rr.motor;

		motor_rl.freeSpin = false;
		motor_rr.freeSpin = false;

		hinge_rl.motor = motor_rl;
		hinge_rr.motor = motor_rr;
	}

	public override void AgentStep(float[] act)
	{
		//actの引数を受け取ってタイヤを動かす

		float action_rl = act [0];
		float action_rr = act [1];

		if (action_rl > 1f) {
			action_rl = 1f;
		} else if (action_rl < -1) {
			action_rl = -1f;	
		}

		if (action_rr > 1f) {
			action_rr = 1f;
		} else if (action_rr < -1) {
			action_rr = -1f;	
		}
		motor_rl.force = MaxForce;
		motor_rr.force = MaxForce;
		motor_rl.targetVelocity = action_rl * MaxTarget;
		motor_rr.targetVelocity = action_rr * MaxTarget;

		hinge_rl.motor = motor_rl;
		hinge_rr.motor = motor_rr;

		frames += 1;

		reward += (1f - (Mathf.Abs (Goal.transform.localPosition.z - Cart.transform.localPosition.z) / 40f));

		//落ちたらマイナスの報酬を与える
		if (gameObject.transform.localPosition.y < -1f) {
			reward -= 10;
			done = true;
		}
		Debug.Log (reward);
	}

	public override void AgentReset()
	{
		gameObject.transform.localPosition = new Vector3 (0.5f, 0.5f, -37);

		gameObject.transform.rotation = new Quaternion (0f, 0f, 0f, 0f);

		gameObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0f, 0f, 0f);
		Cart.gameObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0f, 0f, 0f);
		Tire_rl.gameObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0f, 0f, 0f);
		Tire_rr.gameObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0f, 0f, 0f);

		Cart.gameObject.GetComponent<Rigidbody> ().angularVelocity = new Vector3 (0f, 0f, 0f);
		Tire_rl.gameObject.GetComponent<Rigidbody> ().angularVelocity = new Vector3 (0f, 0f, 0f);
		Tire_rr.gameObject.GetComponent<Rigidbody> ().angularVelocity = new Vector3 (0f, 0f, 0f);

		Cart.gameObject.transform.rotation = new Quaternion (0f, 0f, 0f, 0f);
		Tire_rl.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
		Tire_rr.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));

		frames = 0;

	}

	public override void AgentOnDone()
	{
	}

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "Goal") {
			//報酬を与える
			reward += (300f - frames) / 30f;
			done = true;
		}
	}
}

