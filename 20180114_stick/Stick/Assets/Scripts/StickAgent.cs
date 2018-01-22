using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickAgent : Agent {
	[Header("Specific to Furiko")]
	public GameObject stick;
	private Rigidbody rb;

	public override List<float> CollectState()
	{
		List<float> state = new List<float> ();
		state.Add (gameObject.transform.localPosition.x/10f);
		state.Add (stick.transform.localRotation.z);
		state.Add (stick.GetComponent<Rigidbody> ().angularVelocity.z);
		return state;
	}

	public override void InitializeAgent(){
		rb = GetComponent<Rigidbody> ();
	}

	public override void AgentStep(float[] act)
	{
		if (brain.brainParameters.actionSpaceType == StateType.continuous) {
			float action_x = act [0];
			if (action_x > 1.0f) {
				action_x = 1.0f;
			}
			if (action_x < -1.0f) {
				action_x = -1.0f;
			}
			if ((gameObject.transform.localPosition.x < 10f && action_x > 0f) ||
				(gameObject.transform.localPosition.x > -10f && action_x < 0f)) {
				gameObject.transform.Translate (action_x, 0, 0);
			}
		}

		float angle_z;
		angle_z = stick.transform.localRotation.eulerAngles.z;
		// 真っすぐ立ってたら報酬
		if (angle_z > 180) {
			reward = 0.1f - 0.1f*((360f - angle_z) / 60f);
		}
		if (angle_z < 180) {
			reward = 0.1f - 0.1f*(angle_z / 60f);
		}
		// 中心付近にいたら報酬
		reward += 0.01f * ( 10f - Mathf.Abs(gameObject.transform.localPosition.x));

		if ((angle_z > 60f) && (angle_z < 300)) {
			done = true;
			reward = -1f;
		} else {
			done = false;
			reward += 0.05f;
		}
	}

	public override void AgentReset()
	{
		gameObject.transform.localPosition = new Vector3 (0f, -3f, 0f);
		stick.transform.localPosition = new Vector3(0f, 0f, 0f);
		stick.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		stick.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
		stick.GetComponent<Rigidbody> ().angularVelocity = new Vector3 (0f, 0f, Random.Range (-0.5f, 0.5f));
	}

	public override void AgentOnDone()
	{

	}
}
