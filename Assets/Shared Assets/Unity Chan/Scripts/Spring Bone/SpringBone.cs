using UnityEngine;
using System.Collections;

public class SpringBone : MonoBehaviour
{
	//次のボーン
	public Transform child;

	//ボーンの向き
	public Vector3 boneAxis = new Vector3(0.0f, 1.0f, 0.0f);

	public float radius = 0.5f;

	//バネが戻る力
	public float stiffnessForce = 0.2f;

	//力の減衰力
	public float dragForce = 0.1f;

	public Vector3 springForce = new Vector3(0.0f, -0.05f, 0.0f);

	public SpringCollider[] colliders;

	public bool debug;

	private float springLength;
	private Quaternion localRotation;
	private Transform trs;
	private Vector3 currTipPos;
	private Vector3 prevTipPos;

	private void Awake()
	{
		trs = transform;
		localRotation = transform.localRotation;
	}

	private void Start()
	{
		springLength = Vector3.Distance(trs.position, child.position);
		currTipPos = child.position;
		prevTipPos = child.position;
	}

	public void UpdateSpring()
	{
		//回転をリセット
		trs.localRotation = Quaternion.identity * localRotation;

		float sqrDt = Time.deltaTime * Time.deltaTime;

		//stiffness
		Vector3 force = trs.rotation * (boneAxis * stiffnessForce) / sqrDt;

		//drag
		force += (prevTipPos - currTipPos) * dragForce / sqrDt;

		force += springForce / sqrDt;

		//前フレームと値が同じにならないように
		Vector3 temp = currTipPos;

		//verlet
		currTipPos = (currTipPos - prevTipPos) + currTipPos + (force * sqrDt);

		//長さを元に戻す
		currTipPos = ((currTipPos - trs.position).normalized * springLength) + trs.position;

		//衝突判定
		for (int i = 0; i < colliders.Length; i++)
		{
			if (Vector3.Distance(currTipPos, colliders[i].transform.position) <= (radius + colliders[i].radius))
			{
				Vector3 normal = (currTipPos - colliders[i].transform.position).normalized;
				currTipPos = colliders[i].transform.position + (normal * (radius + colliders[i].radius));
				currTipPos = ((currTipPos - trs.position).normalized * springLength) + trs.position;
			}
		}

		prevTipPos = temp;

		//回転を適用；
		Vector3 aimVector = trs.TransformDirection(boneAxis);
		Quaternion aimRotation = Quaternion.FromToRotation(aimVector, currTipPos - trs.position);
		trs.rotation = aimRotation * trs.rotation;
	}

	private void OnDrawGizmos()
	{
		if (debug)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(currTipPos, radius);
		}
	}

}
