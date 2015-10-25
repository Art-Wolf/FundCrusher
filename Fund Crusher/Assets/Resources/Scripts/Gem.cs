using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Gem : MonoBehaviour 
{
	public GameObject sphere;
	public GameObject selector;
	private string[] gemMats ={"Red","Blue","Green","Orange","Yellow","Black","Purple"};
	string color="";
	public List<Gem> Neighbors = new List<Gem>();
	private bool isSelected =false;

	void Start () 
	{
		CreateGem();
	}

	public string getColor() {
		return color;
	}

	public void ToggleSelector()
	{
		isSelected =  !isSelected;
		selector.SetActive(isSelected);
	}

	public void CreateGem()
	{
		color = gemMats[Random.Range(0,gemMats.Length)];
		Material m =Resources.Load("Materials/"+color) as Material; 
		Renderer rend = sphere.GetComponent<Renderer> ();
		rend.material = m;
	}

	public void AddNeighbor(Gem g)
	{
		if(!Neighbors.Contains(g))
			Neighbors.Add(g);
	}

	public bool IsNeighborWith(Gem g)
	{
		if(Neighbors.Contains(g))
		{
			return true;
		}
		return false;
	}

	public void RemoveNeighbor(Gem g)
	{
		Neighbors.Remove(g);
	}

	void OnMouseDown()
	{
		if(!GameObject.Find("Board").GetComponent<Board>().isSwapping)
		{
			ToggleSelector();
			GameObject.Find("Board").GetComponent<Board>().SwapGems(this);
		}
	}
}
