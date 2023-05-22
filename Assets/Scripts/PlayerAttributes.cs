using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerAttributes : MonoBehaviour
{
    [SerializeField] Transform resurrectionPoint;
    [SerializeField] string playerName = "Sir Lance";
    [SerializeField] static int playerXP = 10;
    [SerializeField] public static int playerHP = 100;
    [SerializeField] Slider healthBar;
    [SerializeField] public Canvas healthBarCanvas;
    [SerializeField] TextMeshProUGUI playerNameDisplay;
    [SerializeField] TextMeshProUGUI playerXPDisplay;
    [SerializeField] TextMeshProUGUI questList;
    [SerializeField] public List<GameObject> npcList;
    [SerializeField] Canvas deathScreen;
    [SerializeField] Button resurrection;
    [SerializeField] Button exit;

    public GameObject closestNPC;
    float minDistance = float.MaxValue;
    static int killCount=0;
    public int magicReset=3;
    static List<Quest> quests = new List<Quest>();

    void Start()
    {
        questList.text = "No mission";
        playerNameDisplay.text = playerName;
        playerXPDisplay.text = playerXP + " XP";
        GetComponentInChildren<SpawnEffectOnClick>().enabled = false;
        deathScreen.enabled = false;
    }

    void Update()
    {
        healthBar.value = playerHP;
        minDistance = float.MaxValue;
        for (int i = 0; i < npcList.Count; i++)
        {
            GameObject npc = npcList[i];
            float distance = Vector3.Distance(transform.position, npc.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestNPC = npc;
            }
        }
        //Debug.Log(closestNPC.name);
        if (quests.Count > 0)
        {
            questList.text = "";
            for (int i = 0;i < quests.Count; i++)
            {
                questList.text += "° " + quests[i].questAction + " " + quests[i].numberToEliminate + " " + quests[i].enemyType +"\n";
            }
        }
        else
        {
            questList.text = "No mission";
        }
        playerXPDisplay.text = playerXP + " XP";

        if (killCount >= magicReset)
        {
            GetComponentInChildren<SpawnEffectOnClick>().enabled=true;
        }

        if(playerHP<=0 && deathScreen.enabled==false)
        {
            Debug.Log("GAME OVER");
            deathScreen.enabled = true;
            deathScreen.GetComponent<CanvasFader>().StartFadeIn();
            deathScreen.GetComponentInChildren<TextMeshProUGUI>().text = playerName;
            resurrection.onClick.AddListener(()=> ResurrectPlayer());
            exit.onClick.AddListener(()=> Application.Quit()); 
        }
    }

    public static void GainXP(int number)
    {
        playerXP += number;
    }

    public static void KillCountRaise()
    {
        killCount += 1;
    }

    public void KillCountNull()
    {
        killCount = 0;
    }

    public int GetKillCount()
    {
        return killCount;
    }

    public static void AddQuest(Quest quest)
    {
        quests.Add(quest);
    }

    public static void LastKilled(string enemy)
    {
        if (quests.Count > 0)
        {
            for (int i = 0; i < quests.Count; i++)
            {
                if (quests[i].enemyType == enemy)
                {
                    quests[i].numberToEliminate--;
                    if (quests[i].numberToEliminate == 0)
                    {
                        quests[i].npcGiver.GetComponentInChildren<Collider>().enabled = true;
                        quests[i].npcGiver.GetComponentInChildren<NPCDetect>().heroReturn = true;
                        quests.Remove(quests[i]);
                    }
                }
            }
        }
    }

    public void ResurrectPlayer()
    {
        GetComponent<Transform>().position = resurrectionPoint.position;
        playerHP = 100;
        deathScreen.enabled = false;
        Time.timeScale = 1;
    }
}
