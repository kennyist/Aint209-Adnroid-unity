using UnityEngine;
using System.Collections;

public class GameTimeManager{

	private float warmUpTime;
	private float gameTime;
	private float endTime;
	private float time;
	
	private bool warmup = false;
	private bool round = false;
	private bool end = false;
	private bool swtichLvl = false;
	
	// ----------------------------------------- //
	
	public GameTimeManager(){
		this.warmUpTime = 30.0f;
		this.gameTime = 300.0f;
		this.endTime = 10.0f;
	}
	
	public GameTimeManager(float timeWarmup, float timeRound, float timeEnd){
		this.warmUpTime = timeWarmup;
		this.gameTime = timeRound;
		this.endTime = timeEnd;
	}
	
	// ----------------------------------------- //
	
	public void startGame(){
		time = warmUpTime;	
		warmup = true;
	}
	
	public void SetTime(float nTime){
		time = nTime;
		
		if(time <= 0){
			if(warmup){
				warmup = false;
				time = gameTime;
				round = true;
			} else if (round){
				round = false;
			} else if (end){ 
				end = false;
				time = 0.0f;
				swtichLvl = true;
			}
		}
	}
	
	public void End(){
		time = endTime;
		end = true;
	}
	
	public int GetStateInt(){
		if(warmup){ return 0; }
		if(round){ return 1; }
		if(end){ return 2; }
		if(swtichLvl){ return 3; }
		return 2;
	}
	
	public string GetStateString(){
		if(warmup){ return "Warm Up"; }
		if(round){ return "Round"; }
		if(end){ return "Ended"; }
		return "NA";
	}
	
	public float GetTime(){
		return time;
	}
	
	public string GetTimeString(){
		return ToTimeFormat(time);
	}
	
	
	// ----------------------------------------- //
	
	private string ToTimeFormat(float time){
		
		int tTime = Mathf.RoundToInt(time);
		
		int s = tTime % 60;
		int m = tTime / 60;
		
		return string.Format("{0:00}:{1:00}",m,s);
	}
}
