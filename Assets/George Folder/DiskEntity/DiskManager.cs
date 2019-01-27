using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskManager : MonoBehaviour {

	//Rotation thing
	public int BPM = 100;
	public float CurrentRotation = 0;
	public int BeadAmount = 9;

	//Display maker
	public GameObject BeadAsset;
	public GameObject Singnaler;
	public float Length = 1f;
	GameObject[] BeadArray;
	GameObject Ring;
	GameObject CompleteDisplay;

	public bool testCreation = false;

	//Click simulation
	public float MaxDamage = 100f;
	public float currDamage = 0;
	public float currDisplacement = 0;
	public int dispBead = 0;
	public float sectionPercentage = 0;
	public float outputDamage = 0f;

	//Array creator
	public GameObject[] MultiBeadArray;
	public GameObject MultiBeadSignaler;
	public float MultiBeadLength = 1f;
	GameObject[] MultiBeadDisplayArray;
	GameObject MultiBeadRing;
	GameObject MultiBeadCompleteDisplay;

	public bool multiBeadTestCreation = false;

	//Correction click
	public bool correctionClick = false;
	
	// Update is called once per frame
	void Update () {
		CurrentRotation = (CurrentRotation - (CalculateRingRotationSpeed (BPM, BeadAmount) * Time.deltaTime)) % 1f;

		//

		if (Ring != null) {
			Ring.transform.localRotation = Quaternion.AngleAxis (360 * CurrentRotation, Vector3.forward);
		}

		if (testCreation) {
			CreateRing ();
			testCreation = false;
		}

		//

		if (Input.GetMouseButtonDown (0)) {
			if (!correctionClick) {
				Click ();
			} else {
				CorrectionClick ();
			}
		}

		//

		if (MultiBeadRing != null) {
			MultiBeadRing.transform.localRotation = Quaternion.AngleAxis (360 * CurrentRotation, Vector3.forward);
		}

		if (multiBeadTestCreation) {
			CreateMultiBeadRing ();
			multiBeadTestCreation = false;
		}

	}

	float CalculateRingRotationSpeed(int bpm, int beadamount) {
		float section = CalculateSection (beadamount);
		return section * (bpm / 60f);
	}

	float CalculateSection(int beadamount) {
		float section = 1f / beadamount;
		return section;
	}

	void CreateRing() {
		if (CompleteDisplay != null) {
			Destroy (CompleteDisplay);
		}
		CompleteDisplay = new GameObject ();
		CompleteDisplay.transform.position = transform.position;
		GameObject signaler = Instantiate (Singnaler, CompleteDisplay.transform);
		signaler.transform.localScale = new Vector3 (1, Length, 1);
		signaler.transform.parent = CompleteDisplay.transform;
		Ring = new GameObject ();
		Ring.transform.position = CompleteDisplay.transform.position;
		Ring.transform.parent = CompleteDisplay.transform;
		BeadArray = new GameObject[BeadAmount];
		Vector3 simpleVector = Vector3.down * Length;
		float section = CalculateSection (BeadAmount);
		for (int i = 0; i < BeadArray.Length; i++) {
			BeadArray [i] = Instantiate (BeadAsset, (Quaternion.AngleAxis(360 * section * i, Vector3.forward) * simpleVector), transform.rotation, Ring.transform);
		}
	}

	void Click() {
		float section = CalculateSection (BeadAmount);
		currDisplacement = CurrentRotation % section;
		dispBead = (int) (CurrentRotation / section);
		sectionPercentage = (currDisplacement / section) * 100;
		if (-sectionPercentage < 10f || -sectionPercentage > 90f) {
			outputDamage = MaxDamage;
		} else {
			outputDamage = sectionPercentage;
		}

	}

	void CreateMultiBeadRing() {
		if (MultiBeadCompleteDisplay != null) {
			Destroy (MultiBeadCompleteDisplay);
		}
		MultiBeadCompleteDisplay = new GameObject ();
		MultiBeadCompleteDisplay.transform.position = transform.position;
		GameObject signaler = Instantiate (MultiBeadSignaler, MultiBeadCompleteDisplay.transform);
		signaler.transform.localScale = new Vector3 (1, Length, 1);
		signaler.transform.parent = MultiBeadCompleteDisplay.transform;
		MultiBeadRing = new GameObject ();
		MultiBeadRing.transform.position = MultiBeadCompleteDisplay.transform.position;
		MultiBeadRing.transform.parent = MultiBeadCompleteDisplay.transform;
		MultiBeadDisplayArray = new GameObject[BeadAmount];
		Vector3 simpleVector = Vector3.down * Length;
		float section = CalculateSection (BeadAmount);
		for (int i = 0; i < MultiBeadDisplayArray.Length; i++) {
			MultiBeadDisplayArray [i] = Instantiate (MultiBeadArray[i], (Quaternion.AngleAxis(360 * section * i, Vector3.forward) * simpleVector), transform.rotation, MultiBeadRing.transform);
		}
	}

	void CorrectionClick() {
		float section = CalculateSection (BeadAmount);
		currDisplacement = CurrentRotation % section;
		dispBead = (int) (CurrentRotation / section);
		sectionPercentage = (currDisplacement / section) * 100;
		if (-sectionPercentage < 10f || -sectionPercentage > 90f) {
			outputDamage = MaxDamage;
		} else {
			outputDamage = sectionPercentage;
		}
		CurrentRotation = ((dispBead - 1) * section) % 1f;
	}

}
