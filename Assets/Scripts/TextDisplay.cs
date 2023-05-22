using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class TextDisplay : MonoBehaviour
{
    string textToDisplay = "";
    [SerializeField] float charactersPerSecond = 2f;
    string enemyType;
    int howMany;
    string actionType;

    [SerializeField] Button buttonAccept;
    [SerializeField] Button buttonDecline;
    [SerializeField] GameObject player;

    GameObject closestNPC;

    TextMeshProUGUI textMeshPro;
    float timeElapsed = 0f;
    int charactersDisplayed = 0;

    void Update()
    {
        closestNPC = player.GetComponent<PlayerAttributes>().closestNPC;
        enemyType = closestNPC.GetComponentInChildren<NPCDetect>().enemyType;
        howMany = closestNPC.GetComponentInChildren<NPCDetect>().howMany;
        actionType = closestNPC.GetComponentInChildren<NPCDetect>().actionType;

        if (closestNPC.GetComponent<NPCDetect>().isColliding && closestNPC.GetComponent<NPCDetect>().loopCount == 0)
        {
            charactersDisplayed = 0;
            textToDisplay = "Greetings, adventurer! Are you up for a challenge? A pack of " + enemyType + " has been terrorizing a nearby village, and we need someone to take care of them. Will you help us by eliminating " + howMany + " of these fierce creatures? Your reward awaits you upon your return. Good luck!";
            textMeshPro = GetComponent<TextMeshProUGUI>();
            textMeshPro.text = "";
            timeElapsed = 0f;
            buttonAccept.GetComponentInChildren<TextMeshProUGUI>().text = "ACCEPT";
            buttonAccept.enabled = true;
            buttonDecline.GetComponentInChildren<TextMeshProUGUI>().text = "DECLINE";
            buttonDecline.enabled = true;
            closestNPC.GetComponent<NPCDetect>().loopCount++;
        }
        if (closestNPC.GetComponentInChildren<NPCDetect>().heroReturn == true)
        {
            closestNPC.GetComponentInChildren<NPCDetect>().ActivateBeacon();
            if (closestNPC.GetComponent<NPCDetect>().loopCount ==1 && closestNPC.GetComponent<NPCDetect>().isColliding)
            {
                charactersDisplayed = 0;
                textToDisplay = "Ah, welcome back adventurer! I see you've completed the quest to defeat those beasts. Well done! The village is much safer now. The people are grateful for your bravery and determination. As a reward for your hard work, here's 2000 XP. Keep up the good work, hero.";
                textMeshPro = GetComponent<TextMeshProUGUI>();
                textMeshPro.text = "";
                timeElapsed = 0f;
                closestNPC.GetComponent<NPCDetect>().loopCount++;
                buttonAccept.GetComponentInChildren<TextMeshProUGUI>().text = "Thanks";
                buttonDecline.GetComponentInChildren<TextMeshProUGUI>().text = "";
                buttonDecline.enabled = false;
            }
        }

        if (charactersDisplayed < textToDisplay.Length)
        {
            timeElapsed += Time.deltaTime;
            float charactersToShow = timeElapsed * charactersPerSecond;
            int wholeCharactersToShow = Mathf.FloorToInt(charactersToShow);
            if (wholeCharactersToShow > charactersDisplayed)
            {
                charactersDisplayed = wholeCharactersToShow;
                if (charactersDisplayed > textToDisplay.Length)
                {
                    charactersDisplayed = textToDisplay.Length;
                }
                textMeshPro.text = textToDisplay.Substring(0, charactersDisplayed);
            }
        }
    }

    public void SetQuest()
    {
        Quest quest = new Quest
        {
            questAction = actionType,
            enemyType = enemyType,
            numberToEliminate = howMany,
            npcGiver = closestNPC
        };
        PlayerAttributes.AddQuest(quest);
    }

    public void ButtonAccept()
    {
        if (closestNPC.GetComponentInChildren<NPCDetect>().heroReturn == false)
        {
            Invoke("LateActivate", 1f);
            closestNPC.GetComponentInChildren<Collider>().enabled = false;
            closestNPC.GetComponentInChildren<NPCDetect>().isColliding = false;
            textToDisplay = "";
            SetQuest();
        }
        else
        {
            closestNPC.GetComponentInChildren<Collider>().enabled = false;
            Invoke("LateActivate", 1f);
            PlayerAttributes.GainXP(2000);
            closestNPC.GetComponentInChildren<NPCDetect>().heroReturn = false;
            textToDisplay = "";
            Invoke("RemoveNPCFromList", 2f);
        }

    }
    public void ButtonDecline()
    {
        Invoke("LateActivate", 1f);
        closestNPC.GetComponentInChildren<Collider>().enabled = false;
        Invoke("TurnOnCollider", 5f);
    }

    public void LateActivate()
    {
        player.GetComponentInChildren<Animator>().enabled = true;
        player.GetComponentInChildren<Animator>().SetTrigger("stop");
        player.GetComponentInChildren<Camera>().enabled = false;
        player.GetComponentInChildren<CharacterController>().enabled = true;
        closestNPC.GetComponentInChildren<NPCDetect>().light.enabled = false;
        //targetObject.GetComponentInChildren<PlayerAttack>().enabled = true;
    }

    void TurnOnCollider()
    {
        closestNPC.GetComponentInChildren<Collider>().enabled = true;
    }

    void RemoveNPCFromList()
    {
        player.GetComponent<PlayerAttributes>().npcList.Remove(closestNPC);
    }
}