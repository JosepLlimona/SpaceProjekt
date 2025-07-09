using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] List<GameObject> spaces = new List<GameObject>();

    [SerializeField] List<GameObject> items = new List<GameObject>();
    [SerializeField] GameObject potionTextPrefab;
    [SerializeField] GameObject HelmetSlot;
    [SerializeField] GameObject ArmourSlot;
    [SerializeField] GameObject FeetSlot;
    [SerializeField] GameObject RingSlot;
    [SerializeField] GameObject WeaponSlot;
    [SerializeField] TMP_Text ItemTitle;
    [SerializeField] TMP_Text descriprionText;
    [SerializeField] TMP_Text weaponBonusText;
    [SerializeField] TMP_Text armourBonusText;
    [SerializeField] TMP_Text moneyAmount;
    [SerializeField] GameObject itemVisualizer;

    public PlayerController player;

    // Update is called once per frame
    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            AddItem(items[0]);
            Debug.Log(GetAvailableSpace());
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            AddItem(items[1]);
            Debug.Log(GetAvailableSpace());
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddItem(items[2]);
            Debug.Log(GetAvailableSpace());
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            AddItem(items[3]);
            Debug.Log(GetAvailableSpace());
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddItem(items[4]);
            Debug.Log(GetAvailableSpace());
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            AddItem(items[5]);
            Debug.Log(GetAvailableSpace());
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            AddItem(items[6]);
            Debug.Log(GetAvailableSpace());
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            AddItem(items[7]);
            Debug.Log(GetAvailableSpace());
        }
    }*/

    public void AddItem(GameObject item)
    {
        GameObject itemIcon = null;
        if (item.GetComponent<Item>().ReturnType() == "Consum")
        {
            int iPotion = SearchPotion();
            if (iPotion == -1)
            {
                itemIcon = new GameObject("Potion");
                itemIcon.AddComponent<RawImage>();
                itemIcon.GetComponent<RawImage>().texture = item.GetComponent<Item>().ReturnIcon();
                itemIcon.transform.SetParent(spaces[GetAvailableSpace()].transform);
                itemIcon.GetComponent<RectTransform>().localPosition = Vector3.zero;
                itemIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 40f);
                itemIcon.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                itemIcon.AddComponent<Button>();
                itemIcon.GetComponent<Button>().onClick.AddListener(() => { UseObject(itemIcon, item); });
                itemIcon.AddComponent<PointerEnterHandler>();
                itemIcon.GetComponent<PointerEnterHandler>().item = item;
                itemIcon.GetComponent<PointerEnterHandler>().inventoryController = this;
            }
            else
            {
                if (spaces[iPotion].transform.GetChild(0).childCount <= 0)
                {
                    GameObject potionText = Instantiate(potionTextPrefab);
                    potionText.GetComponent<TMP_Text>().text = "2";
                    potionText.transform.SetParent(spaces[iPotion].transform.GetChild(0));
                    potionText.GetComponent<RectTransform>().localPosition = new Vector3(20f, 20f, 0);
                    potionText.GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 40f);
                    potionText.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                }
                else
                {
                    GameObject potionText = spaces[iPotion].transform.GetChild(0).GetChild(0).gameObject;
                    int value = int.Parse(potionText.GetComponent<TMP_Text>().text);
                    value++;
                    potionText.GetComponent<TMP_Text>().text = value.ToString();
                }
            }
        }
        else if (item.GetComponent<Item>().ReturnType() == "Gun")
        {
            if (GetAvailableSpace() == -1) { return; }
            itemIcon = new GameObject("Sword");
            itemIcon.AddComponent<RawImage>();
            itemIcon.GetComponent<RawImage>().texture = item.GetComponent<Item>().ReturnIcon();
            itemIcon.transform.SetParent(spaces[GetAvailableSpace()].transform);
            itemIcon.GetComponent<RectTransform>().localPosition = Vector3.zero;
            itemIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 40f);
            itemIcon.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f,1f);
            itemIcon.AddComponent<Button>();
            itemIcon.GetComponent<Button>().onClick.AddListener(() => { UseObject(itemIcon, item); });
            itemIcon.AddComponent<PointerEnterHandler>();
            itemIcon.GetComponent<PointerEnterHandler>().item = item;
            itemIcon.GetComponent<PointerEnterHandler>().inventoryController = this;
        }
        else if (item.GetComponent<Item>().ReturnType() == "Sword")
        {
            if (GetAvailableSpace() == -1) { return; }
            itemIcon = new GameObject("Sword");
            itemIcon.AddComponent<RawImage>();
            itemIcon.GetComponent<RawImage>().texture = item.GetComponent<Item>().ReturnIcon();
            itemIcon.transform.SetParent(spaces[GetAvailableSpace()].transform);
            itemIcon.GetComponent<RectTransform>().localPosition = Vector3.zero;
            itemIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 40f);
            itemIcon.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            itemIcon.AddComponent<Button>();
            itemIcon.GetComponent<Button>().onClick.AddListener(() => { UseObject(itemIcon, item); });
            itemIcon.AddComponent<PointerEnterHandler>();
            itemIcon.GetComponent<PointerEnterHandler>().item = item;
            itemIcon.GetComponent<PointerEnterHandler>().inventoryController = this;
        }
        else if (item.GetComponent<Item>().ReturnType() == "Armour")
        {
            if (GetAvailableSpace() == -1) { return; }
            itemIcon = new GameObject("Armour");
            itemIcon.AddComponent<RawImage>();
            itemIcon.GetComponent<RawImage>().texture = item.GetComponent<Item>().ReturnIcon();
            itemIcon.transform.SetParent(spaces[GetAvailableSpace()].transform);
            itemIcon.GetComponent<RectTransform>().localPosition = Vector3.zero;
            itemIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 40f);
            itemIcon.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            itemIcon.AddComponent<Button>();
            itemIcon.GetComponent<Button>().onClick.AddListener(() => { UseObject(itemIcon, item); });
            itemIcon.AddComponent<PointerEnterHandler>();
            itemIcon.GetComponent<PointerEnterHandler>().item = item;
            itemIcon.GetComponent<PointerEnterHandler>().inventoryController = this;
        }
        else if (item.GetComponent<Item>().ReturnType() == "Feet")
        {
            if (GetAvailableSpace() == -1) { return; }
            itemIcon = new GameObject("Botes");
            itemIcon.AddComponent<RawImage>();
            itemIcon.GetComponent<RawImage>().texture = item.GetComponent<Item>().ReturnIcon();
            itemIcon.transform.SetParent(spaces[GetAvailableSpace()].transform);
            itemIcon.GetComponent<RectTransform>().localPosition = Vector3.zero;
            itemIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 40f);
            itemIcon.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            itemIcon.AddComponent<Button>();
            itemIcon.GetComponent<Button>().onClick.AddListener(() => { UseObject(itemIcon, item); });
            itemIcon.AddComponent<PointerEnterHandler>();
            itemIcon.GetComponent<PointerEnterHandler>().item = item;
            itemIcon.GetComponent<PointerEnterHandler>().inventoryController = this;
        }
        else if (item.GetComponent<Item>().ReturnType() == "Ring")
        {
            if (GetAvailableSpace() == -1) { return; }
            itemIcon = new GameObject("Anell");
            itemIcon.AddComponent<RawImage>();
            itemIcon.GetComponent<RawImage>().texture = item.GetComponent<Item>().ReturnIcon();
            itemIcon.transform.SetParent(spaces[GetAvailableSpace()].transform);
            itemIcon.GetComponent<RectTransform>().localPosition = Vector3.zero;
            itemIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 40f);
            itemIcon.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            itemIcon.AddComponent<Button>();
            itemIcon.GetComponent<Button>().onClick.AddListener(() => { UseObject(itemIcon, item); });
            itemIcon.AddComponent<PointerEnterHandler>();
            itemIcon.GetComponent<PointerEnterHandler>().item = item;
            itemIcon.GetComponent<PointerEnterHandler>().inventoryController = this;
        }
        else if (item.GetComponent<Item>().ReturnType() == "Helmet")
        {
            if (GetAvailableSpace() == -1) { return; }
            itemIcon = new GameObject("Helmet");
            itemIcon.AddComponent<RawImage>();
            itemIcon.GetComponent<RawImage>().texture = item.GetComponent<Item>().ReturnIcon();
            itemIcon.transform.SetParent(spaces[GetAvailableSpace()].transform);
            itemIcon.GetComponent<RectTransform>().localPosition = Vector3.zero;
            itemIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 40f);
            itemIcon.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            itemIcon.AddComponent<Button>();
            itemIcon.GetComponent<Button>().onClick.AddListener(() => { UseObject(itemIcon, item); });
            itemIcon.AddComponent<PointerEnterHandler>();
            itemIcon.GetComponent<PointerEnterHandler>().item = item;
            itemIcon.GetComponent<PointerEnterHandler>().inventoryController = this;
        }
    }

    int GetAvailableSpace()
    {
        for(int i = 0; i< spaces.Count; i++)
        {
            if (spaces[i].transform.childCount <= 0)
            {
                return i;
            }
        }

        return -1;
    }

    int SearchPotion()
    {
        for (int i = 0; i < spaces.Count; i++)
        {
            Debug.Log(i);
            if (spaces[i].transform.childCount > 0)
            {
                if (spaces[i].transform.GetChild(0).name == "Potion")
                {
                    return i;
                }
            }
        }

        return -1;
    }

    public void UseObject(GameObject itemR, GameObject item)
    {
        Debug.Log(item);
        item.GetComponent<Item>().Use(player.GetComponent<PlayerController>());
        if(item.GetComponent<Item>().ReturnType() == "Consum")
        {
            if (itemR.transform.childCount > 0)
            {
                GameObject potionText = itemR.transform.GetChild(0).gameObject;
                int value = int.Parse(potionText.GetComponent<TMP_Text>().text);
                value--;
                if (value <= 1)
                {
                    Destroy(potionText);
                }
                else
                {
                    potionText.GetComponent<TMP_Text>().text = value.ToString();
                }
            }
            else
            {
                Destroy(itemR);
            }
        }
        else if(item.GetComponent<Item>().ReturnType() == "Gun")
        {
            if(WeaponSlot.transform.childCount > 0)
            {
                Debug.Log("Hi ha un altre item :)");
                UnequipWeapon(WeaponSlot.transform.GetChild(0).gameObject, WeaponSlot.transform.GetChild(0).gameObject.GetComponent<PointerEnterHandler>().item);
            }
            itemR.transform.SetParent(WeaponSlot.transform);
            itemR.GetComponent<RectTransform>().localPosition = Vector3.zero;
            itemR.GetComponent<Button>().onClick.RemoveAllListeners();
            itemR.GetComponent<Button>().onClick.AddListener(() => { UnequipWeapon(itemR, item); }) ;
            weaponBonusText.text = "+" + item.GetComponent<Item>().ReturnBonus() + " DMG";
            player.EquipWeapon("Gun", item.GetComponent<Item>().ReturnBonus());
        }
        else if (item.GetComponent<Item>().ReturnType() == "Sword")
        {
            if (WeaponSlot.transform.childCount > 0)
            {
                UnequipWeapon(WeaponSlot.transform.GetChild(0).gameObject, WeaponSlot.transform.GetChild(0).gameObject.GetComponent<PointerEnterHandler>().item);
            }
            itemR.transform.SetParent(WeaponSlot.transform);
            itemR.GetComponent<RectTransform>().localPosition = Vector3.zero;
            itemR.GetComponent<Button>().onClick.RemoveAllListeners();
            itemR.GetComponent<Button>().onClick.AddListener(() => { UnequipWeapon(itemR, item); });
            weaponBonusText.text = "+" + item.GetComponent<Item>().ReturnBonus() + " DMG";
            player.EquipWeapon("Sword", item.GetComponent<Item>().ReturnBonus());
        }
        else if(item.GetComponent<Item>().ReturnType() == "Armour")
        {
            if(ArmourSlot.transform.childCount > 0)
            {
                UnequipArmour(ArmourSlot.transform.GetChild(0).gameObject, ArmourSlot.transform.GetChild(0).gameObject.GetComponent<PointerEnterHandler>().item);
            }
            itemR.transform.parent = ArmourSlot.transform;
            itemR.GetComponent<RectTransform>().localPosition = Vector3.zero;
            itemR.GetComponent<Button>().onClick.RemoveAllListeners();
            itemR.GetComponent<Button>().onClick.AddListener(() => { UnequipArmour(itemR, item); });
            string armourBonusString = armourBonusText.text;
            char num = armourBonusString[1];
            int value = (int)char.GetNumericValue(num);
            value += item.GetComponent<Item>().ReturnBonus();
            armourBonusText.text = "+" + value + " DEF";
            player.changeDefense(item.GetComponent<Item>().ReturnBonus()/10f);
        }
        else if (item.GetComponent<Item>().ReturnType() == "Feet")
        {
            if (FeetSlot.transform.childCount > 0)
            {
                UnequipArmour(FeetSlot.transform.GetChild(0).gameObject, FeetSlot.transform.GetChild(0).gameObject.GetComponent<PointerEnterHandler>().item);
            }
            itemR.transform.parent = FeetSlot.transform;
            itemR.GetComponent<RectTransform>().localPosition = Vector3.zero;
            itemR.GetComponent<Button>().onClick.RemoveAllListeners();
            itemR.GetComponent<Button>().onClick.AddListener(() => { UnequipArmour(itemR, item); });
            string armourBonusString = armourBonusText.text;
            char num = armourBonusString[1];
            int value = (int)char.GetNumericValue(num);
            value += item.GetComponent<Item>().ReturnBonus();
            armourBonusText.text = "+" + value + " DEF";
            player.changeDefense(item.GetComponent<Item>().ReturnBonus() / 10f);
        }
        else if (item.GetComponent<Item>().ReturnType() == "Helmet")
        {
            if (HelmetSlot.transform.childCount > 0)
            {
                UnequipArmour(HelmetSlot.transform.GetChild(0).gameObject, HelmetSlot.transform.GetChild(0).gameObject.GetComponent<PointerEnterHandler>().item);
            }
            itemR.transform.parent = HelmetSlot.transform;
            itemR.GetComponent<RectTransform>().localPosition = Vector3.zero;
            itemR.GetComponent<Button>().onClick.RemoveAllListeners();
            itemR.GetComponent<Button>().onClick.AddListener(() => { UnequipArmour(itemR, item); });
            string armourBonusString = armourBonusText.text;
            char num = armourBonusString[1];
            int value = (int)char.GetNumericValue(num);
            value += item.GetComponent<Item>().ReturnBonus();
            armourBonusText.text = "+" + value + " DEF";
            player.changeDefense(item.GetComponent<Item>().ReturnBonus() / 10f);
        }
        else if (item.GetComponent<Item>().ReturnType() == "Ring")
        {
            if (RingSlot.transform.childCount > 0)
            {
                UnequipArmour(RingSlot.transform.GetChild(0).gameObject, RingSlot.transform.GetChild(0).gameObject.GetComponent<PointerEnterHandler>().item);
            }
            itemR.transform.parent = RingSlot.transform;
            itemR.GetComponent<RectTransform>().localPosition = Vector3.zero;
            itemR.GetComponent<Button>().onClick.RemoveAllListeners();
            itemR.GetComponent<Button>().onClick.AddListener(() => { UnequipArmour(itemR, item); });
            string armourBonusString = armourBonusText.text;
            char num = armourBonusString[1];
            int value = (int)char.GetNumericValue(num);
            value += item.GetComponent<Item>().ReturnBonus();
            armourBonusText.text = "+" + value + " DEF";
            player.changeDefense(item.GetComponent<Item>().ReturnBonus() / 10f);
        }
    }

    public void UnequipWeapon(GameObject itemR, GameObject item)
    {
        itemR.transform.SetParent(spaces[GetAvailableSpace()].transform);
        itemR.GetComponent<RectTransform>().localPosition = Vector3.zero;
        itemR.GetComponent<Button>().onClick.RemoveAllListeners();
        itemR.GetComponent<Button>().onClick.AddListener(() => { UseObject(itemR, item); });
        weaponBonusText.text = "+0 DMG";
        player.UnequipWeapon();

    }
    public void UnequipArmour(GameObject itemR, GameObject item)
    {
        itemR.transform.parent = spaces[GetAvailableSpace()].transform;
        itemR.GetComponent<RectTransform>().localPosition = Vector3.zero;
        itemR.GetComponent<Button>().onClick.RemoveAllListeners();
        itemR.GetComponent<Button>().onClick.AddListener(() => { UseObject(itemR, item); });
        string armourBonusString = armourBonusText.text;
        char num = armourBonusString[1];
        int value = (int)char.GetNumericValue(num);
        value -= item.GetComponent<Item>().ReturnBonus();
        armourBonusText.text = "+" + value + " DEF";
        player.changeDefense(-(item.GetComponent<Item>().ReturnBonus()/10f));
    }

    public void changeItemVisualizer(GameObject item)
    {
        ItemTitle.text = item.GetComponent<Item>().ReturnTitle();
        descriprionText.text = item.GetComponent<Item>().ReturnDescription();
        itemVisualizer.GetComponent<RawImage>().color = Color.white;
        itemVisualizer.GetComponent<RawImage>().texture = item.GetComponent<Item>().ReturnIcon();
    }

    public void removeItemVisualizer()
    {
        ItemTitle.text = "";
        descriprionText.text = "";
        itemVisualizer.GetComponent<RawImage>().color = Color.clear;
    }

    public void changeMoneyAmount(int money)
    {
        moneyAmount.text = money + "€";
    }
}
