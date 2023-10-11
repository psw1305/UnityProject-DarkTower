using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyQuestObj : MonoBehaviour {
	public int dontHaveQuestId = 1;
	public GameObject questObject;
	public EvqAction eventAction = EvqAction.Delete;

	public bool keepUpdate = true;
	private GameObject player;

	public enum EvqAction{
		Delete = 0,
		Disable = 1
	}
	private QuestStatC qstat;

	[System.Serializable]
	public class RemoveQItem{
		public int itemId = 1;
		public int quantity = 1;
	}

	[Tooltip("Use it with Game Object that Delete or Disable itself to prevent the Remove Item loop")]
	public RemoveQItem[] removeQuestItem;
	// Use this for initialization
	void Start(){
		if(GlobalConditionC.mainPlayer){
			player = GlobalConditionC.mainPlayer;
		}else{
			player = GameObject.FindWithTag("Player");
		}
		//The Function will automatic check If player have this quest(ID) in the Quest Slot or not.
		if(player){
			qstat = player.GetComponent<QuestStatC>();
			if(qstat){
				bool haveQuest = player.GetComponent<QuestStatC>().CheckQuestSlot(dontHaveQuestId);

				if(!haveQuest){
					ActionToObject();
				}
			}
		}
	}

	public void ActionToObject(){
		if(player && removeQuestItem.Length > 0){
			for(int a = 0; a < removeQuestItem.Length; a++){
				player.GetComponent<InventoryC>().RemoveItem(removeQuestItem[a].itemId , removeQuestItem[a].quantity);
			}
		}
		if(eventAction == EvqAction.Delete){
			Destroy(questObject);
		}
		if(eventAction == EvqAction.Disable){
			questObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update(){
		if(keepUpdate){
			if(!player && GlobalConditionC.mainPlayer){
				player = GlobalConditionC.mainPlayer;
			}
			//The Function will automatic check If player have this quest(ID) in the Quest Slot or not.
			if(player){
				qstat = player.GetComponent<QuestStatC>();
				if(qstat){
					bool haveQuest = player.GetComponent<QuestStatC>().CheckQuestSlot(dontHaveQuestId);
					
					if(!haveQuest){
						ActionToObject();
					}
				}
			}
		}
	}
}
