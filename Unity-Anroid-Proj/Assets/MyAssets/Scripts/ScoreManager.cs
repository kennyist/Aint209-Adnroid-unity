using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour {
	
	private List<string> seekerList = new List<string>();
	private List<string> hiderList = new List<string>();
	private StaticVars sttvr = new StaticVars();
	private bool addedUp = false;
	
	// ==== Hider === //
	
	private static int Hscore = 0;
	private static int HTrapsUsed = 0;
	private static int HTrapsHit = 0;
	
	private static int NearMisses = 0;
	private static int VisableNearMiss = 0;
	
	// ==== Seeker === //
	private static int Sscore = 0;
	
	private static int STrapsUsed = 0;
	private static int STrapsHit = 0;

	
	// =============== //
	
	// ----- TEMP --- //
	
	public int GetTrapsUsed(bool seekers){
		if(seekers){
			return STrapsUsed;
		} else {
			return HTrapsUsed;
		}
	}
	
	// ----- ENDTEMP --- //
	
	// ==== Add methods ==== //
	
	public void AddTrapUsed(bool forSeeker){
		if(forSeeker){
			STrapsUsed++;	
		} else {
			HTrapsUsed++;
		}
	}
	
	public void AddTrapHit(bool forSeeker){
		if(forSeeker){
			STrapsHit++;	
		} else {
			HTrapsHit++;
		}
	}
	
	public void AddNearMiss(bool visable){
		if(visable){
			VisableNearMiss++;	
		} else {
			NearMisses++;
		}
	}
	
	// ==== Return methods === //
	
	private void AddUp(bool seekerWin){
		
		// --- Seeker
		
		Sscore += STrapsUsed * 25;
		if(STrapsUsed > 0){ seekerList.Add(STrapsUsed + " Traps used -- * 25SP"); }
		
		Sscore += STrapsHit * 50;
		if(STrapsHit > 0){ seekerList.Add(STrapsHit + " Traps hit -- * 50SP"); }

		if(seekerWin){
			Sscore += 500;
			seekerList.Add("Caught "+ sttvr.getHiderName()+" -- 500SP");
		}

		if(sttvr.GetGold(true) > 0){
			Sscore += sttvr.GetGold(true);
			seekerList.Add("Left over gold -- "+sttvr.GetGold(true) + "SP");
		}

		
		// --- Hider
		
		Hscore += HTrapsUsed * 25;
		if(HTrapsUsed > 0){ hiderList.Add(HTrapsUsed + " Traps used -- * 25SP"); }
		
		Hscore += HTrapsHit * 50;
		if(HTrapsHit > 0){ hiderList.Add(HTrapsHit + " Traps hit -- * 50SP"); }
		
		Hscore += NearMisses * 25;
		if(NearMisses > 0){ hiderList.Add(NearMisses + " Near misses -- * 25SP"); }
		
		Hscore += VisableNearMiss * 75;
		if(VisableNearMiss > 0){ hiderList.Add(VisableNearMiss + " Near misses while visable -- * 75SP"); }

		if(!seekerWin){
			Hscore += 500;
			hiderList.Add("Survived the whole match -- 500SP");
		}

		if(sttvr.GetGold(false) > 0){
			Hscore += sttvr.GetGold(false);
			hiderList.Add("Left over gold -- "+sttvr.GetGold(false) + "SP");
		}
		
		/// ---
		
		sttvr.AddScore(true,Sscore);
		sttvr.AddScore(false,Hscore);
	}
	
	public List<string> GetTally(bool seekers,bool seekerWin){
		if(!addedUp){
			AddUp(seekerWin);
			addedUp = true;
		}
		
		if(seekers){
			return seekerList;
		} else {
			return hiderList;	
		}
	}
	
	public int GetScore(bool seekers){
		if(seekers){
			return Sscore;
		} else {
			return Hscore;	
		}
	}
}
