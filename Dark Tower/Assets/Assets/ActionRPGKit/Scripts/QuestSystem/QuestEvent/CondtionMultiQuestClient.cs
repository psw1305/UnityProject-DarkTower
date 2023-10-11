using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CondtionMultiQuestClient : MonoBehaviour {
	public int mustCompleteQuestId = 1;
	public QuestClientEV[] passConditionClient = new QuestClientEV[2];
	public EventActivator failConditionEv;
	
	private GameObject player;
	private GameObject questData;
	public int questStep = 0;
	
	public void Checking(){
		if(EventActivator.onInteracting){
			return;
		}
		//Condition Check
		questData = player.GetComponent<QuestStatC>().questDataBase;
		int qprogress = player.GetComponent<QuestStatC>().questProgress[mustCompleteQuestId]; //Check Queststep
		int finish = questData.GetComponent<QuestDataC>().questData[mustCompleteQuestId].finishProgress;
		if(qprogress >= finish + 9){
			if(passConditionClient.Length > 0){
				//passConditionClient.ActivateQuest(player);
				bool q = passConditionClient[questStep].ActivateQuest(player);
				if(q && questStep < passConditionClient.Length){
					passConditionClient[questStep].enter = false; //Reset Enter Variable of last client
					questStep++;
					if(questStep >= passConditionClient.Length){
						questStep = passConditionClient.Length -1;
						return;
					}
					passConditionClient[questStep].s = 0;
					passConditionClient[questStep].enter = true;
				}
			}
		}else{
			if(passConditionClient.Length > 0){
				passConditionClient[questStep].s = 0;
				passConditionClient[questStep].enter = false;
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
			CheckQuestSequence();

			if(passConditionClient.Length > 0){
				passConditionClient[questStep].s = 0;
				passConditionClient[questStep].enter = true;
			}
			if(player.GetComponent<AttackTriggerC>())
				player.GetComponent<AttackTriggerC>().GetActivator(this.gameObject , "Checking" , "Talk");
		}
	}
	
	void OnTriggerExit(Collider other){
		if(other.tag == "Player"){
			if(passConditionClient.Length > 0){
				passConditionClient[questStep].s = 0;
				passConditionClient[questStep].enter = false;
			}
			if(player.GetComponent<AttackTriggerC>())
				player.GetComponent<AttackTriggerC>().RemoveActivator(this.gameObject);
		}
	}

	public void CheckQuestSequence(){
		bool c = true;
		while(c == true){
			int id = passConditionClient[questStep].questId;
			questData = passConditionClient[questStep].questData;
			int qprogress = player.GetComponent<QuestStatC>().questProgress[id]; //Check Queststep
			int finish = questData.GetComponent<QuestDataC>().questData[id].finishProgress;
			if(qprogress >= finish + 9){ 
				questStep++;
				if(questStep >= passConditionClient.Length){
					questStep = passConditionClient.Length -1;
					c = false; // End Loop
				}
			}else{
				c = false; // End Loop
			}
		}
	}
}
