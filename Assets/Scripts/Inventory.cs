using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject player;
    [SerializeField] List<string> collectName;
    [SerializeField] List<Sprite> collectImage;
    [SerializeField] List<GameObject> buttons;
    [SerializeField] Sprite defaultSprite;

    void Start()
    {
        canvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            canvas.SetActive(!canvas.activeSelf);
            if (canvas.activeSelf==true)
            {
                player.GetComponent<PlayerAttack>().enabled = false;
            }
            else
            {
                player.GetComponent<PlayerAttack>().enabled = true;
            }
        }


        CheckNearObjects();

        for (int i = 0; i < buttons.Count; i++)
        {
            if (i < collectName.Count)
            {
                Sprite buttonImage = buttons[i].transform.GetChild(0).GetComponent<Image>().sprite;

                int currentIndex = i;

                if (buttonImage == defaultSprite)
                {
                    for (int j = 0; j < collectImage.Count; j++)
                    {
                        if (collectImage[j].name == collectName[i])
                        {
                            buttons[i].transform.GetChild(0).GetComponent<Image>().sprite = collectImage[j];

                            buttons[i].GetComponent<Button>().onClick.AddListener(() => RestorePlayerHealth(currentIndex));

                            break;
                        }
                    }
                }
            }
            else
            {
                buttons[i].transform.GetChild(0).GetComponent<Image>().sprite = defaultSprite;
                buttons[i].GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }
    }

    void CheckNearObjects()
    {
        Collider[] objects = Physics.OverlapSphere(player.transform.position, 5);
        LayerMask collectibleLayer = LayerMask.GetMask("Collectible");

        for (int i = 0; i < objects.Length; i++)
        {
            if (collectibleLayer == (collectibleLayer | (1 << objects[i].gameObject.layer)))
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    collectName.Add(objects[i].name);
                    Debug.Log(objects[i].name);
                    Destroy(objects[i].transform.gameObject);
                }
            }
        }
    }

    void RestorePlayerHealth(int buttonIndex)
    {
        Debug.Log("PlayerHelath restore Called");
        if(PlayerAttributes.playerHP<90)
            PlayerAttributes.playerHP += 10;
        else
            PlayerAttributes.playerHP=100;
        buttons[buttonIndex].transform.GetChild(0).GetComponent<Image>().sprite = defaultSprite;

        collectName.RemoveAt(buttonIndex);
    }
}
