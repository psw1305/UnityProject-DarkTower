using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CondtionQuestClient : MonoBehaviour {
	public int mustCompleteQuestId = 1;
	public QuestClientEV passConditionClient;
	public EventActivator failConditionEv;

	private GameObject player;
	private GameObject questData;
	
	public void Checking(){
		if(EventActivator.onInteracting){
			return;
		}

		//Condition Check
		questData = player.GetComponent<QuestStatC>().questDataBase;
		int qprogress = player.GetComponent<QuestStatC>().questProgress[mustCompleteQuestId]; //Check Queststep
		int finish = questData.GetComponent<QuestDataC>().questData[mustCompleteQuestId].finishProgress;
		if(qprogress >= finish + 9){ 
			if(passConditionClient){
				//passConditionClient.s = 0;
				//passConditionClient.enter = true;
				passConditionClient.ActivateQuest(player);
			}
		}else{
			if(passConditionClient){
				passConditionClient.s = 0;
				passConditionClient.enter = false;
			}
			if(failConditionEv){
				failConditionEv.player = player;
				failConditionEv.ActivateEvent();
			}
		}
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			player = other.gameObject;

			if(passConditionClient){
				passConditionClient.s = 0;
				passConditionClient.enter = true;
			}

			if(player.GetComponent<AttackTriggerC>())
				player.GetComponent<AttackTriggerC>().GetActivator(this.gameObject , "Checking" , "Talk");
		}
	}
	
	void OnTriggerExit(Collider other){
		if(other.tag == "Player"){

			if(passConditionClient){
				passConditionClient.s = 0;
				passConditionClient.enter = false;
			}

			if(player.GetComponent<AttackTriggerC>())
				player.GetComponent<AttackTriggerC>().RemoveActivator(this.gameObject);
		}
	}
	
}
