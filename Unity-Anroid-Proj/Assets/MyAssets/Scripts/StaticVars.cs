using UnityEngine;
using System.Collections;

public class StaticVars {
	
	private static string hiderName = "hider";
	private static string seekerName = "seeker";
	
	private static int hiderScore;
	private static int seekerScore;
	
	private static int seekerGold = 500;
	private static int hiderGold = 500;
	
	private static string winner;

	private static int activeTraps = 0;

	private static bool seekerCCTV = false;
	private static bool hiderCCTV = false;

	
	public void Reset(){
		hiderName = "hider";
		seekerName = "seeker";
		
		hiderScore = 0;
		seekerScore = 0;
		
		seekerGold = 500;
		hiderGold = 500;
		
		winner = "";
	}
	
	public void setHiderName(string name){
		hiderName = name;
	}
	
	public string getHiderName(){
		return hiderName;	
	}
	
	public void setSeekerName(string name){
		seekerName = name;	
	}
	
	public string getSeekerName(){
		return seekerName;	
	}
	
	public void AddScore(bool forSeeker, int ammount){
		if(forSeeker){
			seekerScore += ammount;
		} else {
			hiderScore += ammount;	
		}
	}
	
	public void RemoveScore(bool forSeeker, int ammount){
		if(forSeeker){
			seekerScore -= ammount;
		} else {
			hiderScore -= ammount;	
		}
	}
	
	public int GetScore(bool getSeekerGold){
		if(getSeekerGold){
			return seekerScore;
		} else {
			return hiderScore;
		}
	}
	
	public void AddGold(bool forSeeker, int ammount){
		if(forSeeker){
			seekerGold += ammount;
		} else {
			hiderGold += ammount;	
		}
	}
	
	public void RemoveGold(bool forSeeker, int ammount){
		if(forSeeker){
			seekerGold -= ammount;
		} else {
			hiderGold -= ammount;	
		}
	}
	
	public int GetGold(bool getSeekerGold){
		if(getSeekerGold){
			return seekerGold;
		} else {
			return hiderGold;
		}
	}
	
	public void SetWinner(string name){
		winner = name;
	}
	
	public string GetWinner(){
		return winner;	
	}

	public void AddActiveTrap(){
		activeTraps += 1;
	}
	
	public void RemoveActiveTrap(){
		activeTraps--;
	}
	
	public int GetActiveTraps(){
		return activeTraps;
	}

	public bool HiderCCTVEnabled(){
		return hiderCCTV;
	}

	public bool SeekerCCTVEnabled(){
		return seekerCCTV;
	}

	public void SetHiderCCTV(bool enable){
		hiderCCTV = enable;
	}

	public void SetSeekerCCTV(bool enable){
		seekerCCTV = enable;
	}
}
