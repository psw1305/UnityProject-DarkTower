using UnityEngine;
using TMPro;
using DG.Tweening;

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(Vector3 position, ItemData item)
    {
        Transform transform = Instantiate(ExploreSystem.Instance.pfItemWorld, position, Quaternion.identity);
        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);
        return itemWorld;
    }

    public static ItemWorld DropItem(Vector3 position, ItemData item)
    {
        Vector3 spawnPosition = new(position.x, position.y + 0.5f, position.z);
        Vector3 randomPos = new(Random.Range(-1.0f, 1.0f), 0, Random.Range(-0.5f, 0.0f));
        ItemWorld itemWorld = SpawnItemWorld(spawnPosition, item);
        Vector3 dropPosition = position + randomPos;
        itemWorld.tween = itemWorld.transform.DOJump(dropPosition, 2.0f, 1, 1.0f).SetEase(Ease.OutBack);
        return itemWorld;
    }

    private SpriteRenderer worldSprite;
    private ItemData item;
    private TextMeshPro textMeshPro;

    private Tween tween = null;
    private int amount = 1;

    void Awake()
    {
        worldSprite = GetComponentInChildren<SpriteRenderer>();
        textMeshPro = transform.Find("Amount").GetComponent<TextMeshPro>();
    }

    public int GetAmount() { return amount; }
    public ItemData GetItem() { return item; }

    public void SetItem(ItemData item)
    {
        this.item = item;
        amount = item.amount;
        worldSprite.sprite = item.ItemImage;
        transform.parent = ExploreSystem.Instance.GetItemWorldTrnf();

        if (item.amount > 1) textMeshPro.SetText(amount.ToString());
        else textMeshPro.SetText("");

        ExploreSystem.Instance.AddMapItem(transform);
    }

    public void DestroySelf()
    {
        ExploreSystem.Instance.RemoveMapItem(transform);
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        if (DOTween.instance != null) tween?.Kill();
    }
}
