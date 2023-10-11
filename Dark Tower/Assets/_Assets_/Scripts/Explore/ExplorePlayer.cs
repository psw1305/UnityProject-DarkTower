using UnityEngine;

public class ExplorePlayer : MonoBehaviour 
{
    public static ExplorePlayer Instance;
    private float lootingTimer;
    private float lootingTimerMax = 0.1f;

    private float fillGage;
    private float keyGage;

    private Inventory inventory;
    private GameSystem gameSystem;
    private ExploreNpc _interactNpc;
    private ExploreObject _closestObject, _curClosestObject;
    private ExploreEnemy _closestEnemy, _curClosestEnemy;
    private ItemWorld _closestItem;

    void Awake() { Instance = this; }

    void Start() 
    {
        gameSystem = GameSystem.Instance;

        keyGage = 0;
        fillGage = 30;

        // 인벤토리 생성
        inventory = new Inventory(UseItem);
        UI_Inventory.Instance.SetInventory(inventory);
    }

    void Update() 
    {
        if (gameSystem.State == GameState.EXPLORE || gameSystem.State == GameState.CHECKING) 
        {
            // 캐릭터 정보창, 인벤토리 활성화 or 비활성화
            if (Input.GetKeyDown(KeyCode.I))
            {
                UI_Character.Instance.CharacterUIActive();
            }
        }

        if (gameSystem.State == GameState.EXPLORE || gameSystem.State == GameState.INTERACT)
        {
            // NPC와 상호작용
            if (_interactNpc != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _interactNpc.Interact();
                }
            }
        }

        if (gameSystem.State == GameState.EXPLORE)
        {
            // 상자 근처에 있을 시
            if (_closestObject != null && _closestObject.GetClosest())
            {
                Interact_Delay(_closestObject, KeyCode.E, 45);
            }

            // 적 근처에 있을 시
            if (_closestEnemy != null && _closestEnemy.GetClosest())
            {
                Interact_Delay(_closestEnemy, KeyCode.Q, 30);
            }

            // 아이템 근처에 있을시
            if (_closestItem != null)
            {
                lootingTimer -= Time.deltaTime;

                if (Input.GetKey(KeyCode.G) && lootingTimer <= lootingTimerMax)
                {
                    lootingTimer = lootingTimerMax;
                    AddItemToInventory(_closestItem.GetItem(), _closestItem.GetAmount());
                    _closestItem.DestroySelf();
                }
            }
        }
    }

    public void SetClosestObject(Transform closestObject) 
    {
        _closestObject = closestObject.GetComponent<ExploreObject>();
    }

    public void SetClosestEnemy(Transform closestEnemy) 
    {
        _closestEnemy = closestEnemy.GetComponent<ExploreEnemy>();
    }

    public void SetClosestItem(Transform closestItem) 
    {
        _closestItem = closestItem.GetComponent<ItemWorld>();
    }

    public void SetClosestItemNULL() 
    {
        _closestItem = null;
    }

    private void Interact_Delay(ExploreObject exploreObject, KeyCode key, float speed) {
        if (Input.GetKey(key)) 
        {
            keyGage += Time.deltaTime * speed;
            exploreObject.SetFillAmount(keyGage / fillGage);

            if (keyGage >= fillGage) {
                keyGage = 0;
                exploreObject.SetFillAmount(0);
                exploreObject.Interact();
            }
        }
        else if (Input.GetKeyUp(key))
        {
            keyGage = 0;
            exploreObject.SetFillAmount(0);
        }
    }

    private void UseItem(ItemData item)
    {
        switch (item.itemType)
        {
            case ItemType.Consumable:

                break;
        }
    }

    public void AddItemToInventory(ItemData itemData, int amount, bool inventoryCheck = true)
    {
        inventory.AddItem(itemData, amount, inventoryCheck);
    }

    public void RemoveItemToInventory(ItemData itemData, bool inventoryCheck = true)
    {
        inventory.RemoveItem(itemData, inventoryCheck);
    }

    void OnTriggerEnter(Collider col) 
    {
        if (col.CompareTag("explore_npc"))
        {
            _interactNpc = col.GetComponent<ExploreNpc>();
            _interactNpc.ContactUI(true);
        }

        //if (col.CompareTag("explore_enemy"))
        //{
        //    _exploreEnemy = col.GetComponent<ExploreEnemy>();
        //    _exploreEnemy.TriggerEnter();
        //    _exploreSystem.SetInteractObject("Enemy", _exploreEnemy);
        //    _exploreSystem.SetSituation("Battle", true);
        //}
    }

    void OnTriggerStay(Collider col) 
    {
        if (col.CompareTag("explore_enemy")) 
        {
            if (!_closestEnemy.GetClosest()) 
            {
                if (_curClosestEnemy != null) 
                {
                    keyGage = 0;
                    _curClosestEnemy.SetClosest(false);
                    _curClosestEnemy.ContactUI(false);
                }

                _curClosestEnemy = _closestEnemy;
                _curClosestEnemy.SetClosest(true);
                _curClosestEnemy.ContactUI(true);
            }
        }

        if (col.CompareTag("explore_object")) 
        {
            if (!_closestObject.GetClosest()) 
            {
                if (_curClosestObject != null) 
                {
                    keyGage = 0;
                    _curClosestObject.SetClosest(false);
                    _curClosestObject.ContactUI(false);
                }

                _curClosestObject = _closestObject;
                _curClosestObject.SetClosest(true);
                _curClosestObject.ContactUI(true);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("explore_npc"))
        {
            _interactNpc.ContactUI(false);
            _interactNpc = null;
        }

        if (col.CompareTag("explore_object")) 
        {
            if (_curClosestObject.GetClosest()) 
            {
                keyGage = 0;
                _curClosestObject.SetClosest(false);
                _curClosestObject.ContactUI(false);
                _curClosestObject = null;
            }
        }

        if (col.CompareTag("explore_enemy"))
        {
            if (_curClosestEnemy.GetClosest()) 
            {
                keyGage = 0;
                _curClosestEnemy.SetClosest(false);
                _curClosestEnemy.ContactUI(false);
                _curClosestEnemy = null;
            }
        }
    }
}
