using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuestUiCanvasC : MonoBehaviour {
	public Text[] questName = new Text[5];
	public Text[] questDescription = new Text[5];
	public GameObject[] cancelButton = new GameObject[5];
	public GameObject player;

	public GameObject database;
	private QuestDataC db;

	public Text pageText;
	public GameObject pagePanel;
	private int maxPage = 1;
	private int page = 0;
	private int cPage = 0;
	private int questLength = 5;

	void SetMaxPage(){
		db = database.GetComponent<QuestDataC>();
		
		//Set Max Page
		if(!player && GlobalConditionC.mainPlayer){
			player = GlobalConditionC.mainPlayer;
		}
		if(player){
			//questLength = player.GetComponent<QuestStatC>().questSlot.Length;
			questLength = 0;
			for(int a = 0; a < player.GetComponent<QuestStatC>().questSlot.Length; a++){
				if(player.GetComponent<QuestStatC>().questSlot[a] > 0){
					questLength++;
				}
			}
		}
		maxPage = questLength / questName.Length;
		if(questLength % questName.Length != 0){
			maxPage += 1;
		}
		if(maxPage > 1 && pagePanel){
			pagePanel.SetActive(true);
		}else if( pagePanel){
			pagePanel.SetActive(false);
		}
	}

	public void UpdateQuestDetails(){
		if(!player){
			return;
		}
		db = database.GetComponent<QuestDataC>();
		QuestStatC pq = player.GetComponent<QuestStatC>();
		for(int a = 0; a < questName.Length; a++){
			questName[a].GetComponent<Text>().text = db.questData[pq.questSlot[a + cPage]].questName;
			questDescription[a].GetComponent<Text>().text = db.questData[pq.questSlot[a + cPage]].description + " (" + pq.questProgress[pq.questSlot[a + cPage]].ToString() + " / " + db.questData[pq.questSlot[a + cPage]].finishProgress + ")";
			if(a + cPage < questLength && pq.questSlot[a + cPage] > 0){
				questDescription[a].gameObject.SetActive(true);
				cancelButton[a].SetActive(true);
			}else{
				questDescription[a].gameObject.SetActive(false);
				cancelButton[a].SetActive(false);
			}
		}
	}

	public void CancelQuest(int qid){
		if(!player){
			return;
		}
		QuestStatC pq = player.GetComponent<QuestStatC>();
		pq.questProgress[pq.questSlot[qid]] = 0;
		pq.questSlot[qid] = 0;
		pq.SortQuest();
		UpdateQuestDetails();
	}

	public void CloseMenu(){
		Time.timeScale = 1.0f;
		//Screen.lockCursor = true;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		gameObject.SetActive(false);
	}

	public void NextPage(){
		if(page < maxPage - 1){
			page++;
			cPage = page * questName.Length;
		}
		if(pageText){
			int p = page + 1;
			pageText.GetComponent<Text>().text = p.ToString();
		}
		UpdateQuestDetails();
	}

	public void PreviousPage(){
		if(page > 0){
			page--;
			cPage = page * questName.Length;
		}
		if(pageText){
			int p = page + 1;
			pageText.GetComponent<Text>().text = p.ToString();
		}
		UpdateQuestDetails();
	}

	public void ResetPage(){
		page = 0;
		cPage = 0;
		if(pageText){
			int p = page + 1;
			pageText.GetComponent<Text>().text = p.ToString();
		}
		SetMaxPage();
		UpdateQuestDetails();
	}
}
