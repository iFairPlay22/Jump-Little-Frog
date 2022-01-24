using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    #region Variables

    #region Serialize fields

    [Header("UI general section")]
    [SerializeField]
    GameObject UiContainer;

    [Header("UI Items Section")]

    [SerializeField]
    Image[] UiItemsImages;

    [Header("UI Item Description")]

    [SerializeField]
    GameObject UiItemWindow;

    [SerializeField]
    Image UiItemImage;

    [SerializeField]
    Text UiItemTitle;

    #endregion

    #region Private fields

    List<GameObject> _items = new List<GameObject>();

    bool show = false;

    #endregion

    #endregion

    #region Methods
    public void PickUpItem(GameObject go)
    {
        _items.Add(go);
        UpdateUI();
    }

    public bool IsActive()
    {
        return show;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            show = !show;
            UiContainer.SetActive(show);
        }
    }

    void UpdateUI()
    {
        foreach (Image img in UiItemsImages)
            img.gameObject.SetActive(false);

        for (int i = 0; i < _items.Count; i++)
        {
            UiItemsImages[i].sprite = _items[i].GetComponent<SpriteRenderer>().sprite;
            UiItemsImages[i].gameObject.SetActive(true);
        }
    }

    public void ShowItem(int id)
    {
        UiItemImage.sprite = UiItemsImages[id].sprite;
        UiItemTitle.text = _items[id].name;

        UiItemImage.gameObject.SetActive(true);
        UiItemTitle.gameObject.SetActive(true);
    }

    public void HideItem()
    {
        UiItemImage.gameObject.SetActive(false);
        UiItemTitle.gameObject.SetActive(false);
    }

    #endregion

}
