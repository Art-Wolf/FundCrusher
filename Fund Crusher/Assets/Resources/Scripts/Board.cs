using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Board : MonoBehaviour 
{
	public List<Gem> gems = new List<Gem>();
	public int GridWidth;
	public int GridHeight;

	public GameObject gemPrefab;
	public Gem lastGem;

	public Vector3 gem1Start,gem1End,gem2Start,gem2End;

	public bool isSwapping = false;

	public Gem gem1,gem2;
	public float startTime;
	public float SwapRate =2;

	void Start () 
	{
		for(int y=0;y<GridHeight;y++)
		{
			for(int x=0;x<GridWidth;x++)
			{
				GameObject g = Instantiate(gemPrefab,new Vector3(x,y,0),Quaternion.identity)as GameObject;
				g.transform.parent = gameObject.transform;
				gems.Add(g.GetComponent<Gem>());
			}
		}
		gameObject.transform.position = new Vector3(-2.5f,-2f,0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isSwapping) {
			ToggleOtherPhysics(true, gem1, gem2);
			//TogglePhysics (true);
			MoveGem (gem1, gem1End, gem1Start);
			MoveNegGem (gem2, gem2End, gem2Start);
			if (Vector3.Distance (gem1.transform.position, gem1End) < .1f || Vector3.Distance (gem2.transform.position, gem2End) < .1f) {
				gem1.transform.position = gem1End;
				gem2.transform.position = gem2End;
				gem1.ToggleSelector ();
				gem2.ToggleSelector ();
				lastGem = null;
				isSwapping = false;
			}
		} else {
		//	ToggleOtherPhysics(false, gem1, gem2);
			TogglePhysics (false);
		}

		string primaryColor = "";

		for(int y=0;y<GridHeight;y++)
		{
			for(int x=0;x<GridWidth;x++)
			{
				foreach (Gem g in gems) {
					if (g.transform.position.Equals(new Vector3(x,y,0))) 
					{
						if (primaryColor == null) {
							primaryColor = g.getColor();
						} else {
							if (primaryColor == g.getColor()) {

							}
						}

					}
				}
			}
		}
	}

	public void MoveGem(Gem gemToMove,Vector3 toPos,Vector3 fromPos)
	{
		Vector3 center = (fromPos + toPos) *.5f;
		center -= new Vector3(0,0,.1f);
		Vector3 riseRelCenter = fromPos - center;
		Vector3 setRelCenter = toPos - center;
		float fracComplete = (Time.time - startTime)/SwapRate;
		gemToMove.transform.position = Vector3.Slerp(riseRelCenter,setRelCenter,fracComplete);
		gemToMove.transform.position += center;
	}

	public void MoveNegGem(Gem gemToMove,Vector3 toPos,Vector3 fromPos)
	{
		Vector3 center = (fromPos + toPos) *.5f;
		center -= new Vector3(0,0,-.1f);
		Vector3 riseRelCenter = fromPos - center;
		Vector3 setRelCenter = toPos - center;
		float fracComplete = (Time.time - startTime)/SwapRate;
		gemToMove.transform.position = Vector3.Slerp(riseRelCenter,setRelCenter,fracComplete);
		gemToMove.transform.position += center;
	}
	public void TogglePhysics(bool isOn)
	{
		foreach(Gem g in gems)
		{
			Rigidbody rb = g.GetComponent<Rigidbody>();
			rb.isKinematic = isOn;
			rb.velocity = Vector3.zero;
		}
	}

	public void ToggleOtherPhysics(bool isOn, Gem first, Gem second)
	{
		foreach(Gem g in gems)
		{
			if (g != first && g != second) {
				Rigidbody rb = g.GetComponent<Rigidbody>();
				rb.isKinematic = isOn;
			}
		}
	}

	public void SwapGems(Gem currentGem)
	{
		if(lastGem == null)
		{
			lastGem = currentGem;
		}
		else if(lastGem == currentGem)
		{
			lastGem = null;
		}
		else
		{
			if(lastGem.IsNeighborWith(currentGem))
			{
				gem1Start = lastGem.transform.position;
				gem1End = currentGem.transform.position;

				gem2Start = currentGem.transform.position;
				gem2End = lastGem.transform.position;

				startTime = Time.time;
				gem1 = lastGem;
				gem2 = currentGem;
				isSwapping = true;
			}
			else
			{
				lastGem.ToggleSelector();
				lastGem = currentGem;
			}
		}
	}
}
