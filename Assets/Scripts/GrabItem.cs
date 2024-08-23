using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class GrabItem : MonoBehaviour
{
    public enum AttachMode
    {
        FixedJoint,
        Force,
    }
	public GameObject EnableDisable;
    public float attachForce = 800.0f;
    public float attachForceDamper = 25.0f;
    public AttachMode attachMode = AttachMode.FixedJoint;

    [EnumFlags]
    public Hand.AttachmentFlags attachmentFlags = 0;

    private List<Hand> holdingHands = new List<Hand>();
	private List<Rigidbody> holdingBodies = new List<Rigidbody>();
	private List<Vector3> holdingPoints = new List<Vector3>();

	private List<Rigidbody> rigidBodies = new List<Rigidbody>();

	//-------------------------------------------------
	void Awake()
	{
		GetComponentsInChildren<Rigidbody>(rigidBodies);
	}
	void Update()
	{
		for (int i = 0; i < holdingHands.Count; i++)
		{
			if (holdingHands[i].IsGrabEnding(this.gameObject))
			{
				/// Object is being released
				PhysicsDetach(holdingHands[i]);
				//On realesing object change the tag to tools tags
				transform.tag = AnimHash.TOOL;
				
			}
		}
	}

	private bool PhysicsDetach(Hand hand)
	{
		int i = holdingHands.IndexOf(hand);

		if (i != -1)
		{
			// Detach this object from the hand
			holdingHands[i].DetachObject(this.gameObject, false);

			// Allow the hand to do other things
			holdingHands[i].HoverUnlock(null);

			// Delete any existing joints from the hand
			if (attachMode == AttachMode.FixedJoint)
			{
				Destroy(holdingHands[i].GetComponent<FixedJoint>());
			}

			Util.FastRemove(holdingHands, i);
			Util.FastRemove(holdingBodies, i);
			Util.FastRemove(holdingPoints, i);

			return true;
		}

		return false;
	}
	void FixedUpdate()
	{
		if (attachMode == AttachMode.Force)
		{
			for (int i = 0; i < holdingHands.Count; i++)
			{
				Vector3 targetPoint = holdingBodies[i].transform.TransformPoint(holdingPoints[i]);
				Vector3 vdisplacement = holdingHands[i].transform.position - targetPoint;

				holdingBodies[i].AddForceAtPosition(attachForce * vdisplacement, targetPoint, ForceMode.Acceleration);
				holdingBodies[i].AddForceAtPosition(-attachForceDamper * holdingBodies[i].GetPointVelocity(targetPoint), targetPoint, ForceMode.Acceleration);
			}
		}
	}

	//-------------------------------------------------
	private void OnHandHoverBegin(Hand hand)
	{
		if (holdingHands.IndexOf(hand) == -1)
		{
			if (hand.isActive)
			{
				hand.TriggerHapticPulse(800);				
			}
		}
	}


	//-------------------------------------------------
	private void OnHandHoverEnd(Hand hand)
	{
		if (holdingHands.IndexOf(hand) == -1)
		{
			if (hand.isActive)
			{
				hand.TriggerHapticPulse(500);
				
			}
		}
	}


	//-------------------------------------------------
	private void HandHoverUpdate(Hand hand)
	{
		GrabTypes startingGrabType = hand.GetGrabStarting();

		if (startingGrabType != GrabTypes.None)
		{
			/// Object is being grabbed
			PhysicsAttach(hand, startingGrabType);
			//Change the tag of the transform
			transform.tag = AnimHash.UNTAG;
			//When we grab the object it should not be kinamatic 
			GetComponent<Rigidbody>().isKinematic = false;
			
		}
	}

	private void PhysicsAttach(Hand hand, GrabTypes startingGrabType)
	{		
		PhysicsDetach(hand);

		Rigidbody holdingBody = null;
		Vector3 holdingPoint = Vector3.zero;

		// The hand should grab onto the nearest rigid body
		float closestDistance = float.MaxValue;
		for (int i = 0; i < rigidBodies.Count; i++)
		{
			float distance = Vector3.Distance(rigidBodies[i].worldCenterOfMass, hand.transform.position);
			if (distance < closestDistance)
			{
				holdingBody = rigidBodies[i];
				closestDistance = distance;
			}
		}

		// Couldn't grab onto a body
		if (holdingBody == null)
			return;

		// Create a fixed joint from the hand to the holding body
		if (attachMode == AttachMode.FixedJoint)
		{
			Rigidbody handRigidbody = Util.FindOrAddComponent<Rigidbody>(hand.gameObject);
			handRigidbody.isKinematic = true;

			FixedJoint handJoint = hand.gameObject.AddComponent<FixedJoint>();
			handJoint.connectedBody = holdingBody;
		}

		// Don't let the hand interact with other things while it's holding us
		hand.HoverLock(null);

		// Affix this point
		Vector3 offset = hand.transform.position - holdingBody.worldCenterOfMass;
		offset = Mathf.Min(offset.magnitude, 1.0f) * offset.normalized;
		holdingPoint = holdingBody.transform.InverseTransformPoint(holdingBody.worldCenterOfMass + offset);

		hand.AttachObject(this.gameObject, startingGrabType, attachmentFlags);

		// Update holding list
		holdingHands.Add(hand);
		holdingBodies.Add(holdingBody);
		holdingPoints.Add(holdingPoint);
	}
}