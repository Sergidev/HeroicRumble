using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject Trash;

    public GameObject EnemySpawnPoints;
    public Transform BossSpawnPoint;
    public Transform PlayerSpawnPoint;
    public GameObject RewardUI;
    public GameObject Rewards;

    public LifeBar LifePlayerObject;

    public GameObject[] ButtonSkills;
    public Transform[] SkillPositionSlots;

    private static GameManager instance;

    public GameObject GameCanvas;
    public GameObject MenuCanvas;

    private int EnemyAmount = 1;
    private int SecondsDelay = 3;

    public bool activeClock = false;

    [SerializeField]
    private int Rounds = 0;

    [SerializeField]
    private int Waves = 1;

    public TextMeshProUGUI RoundsText;

    public TextMeshProUGUI CountdownText;

    public TextMeshProUGUI HighScoreText;

    public GameObject EnemyParent;
    public GameObject[] EnemyPool;
    public GameObject[] BossPool;

    private GameObject spawn = null;

    public GameObject PlayerObject;

    public bool extraLoot;
    public bool extraSlot;

    public int LevelSpinToWin = 1;
    public int LevelWhirlwind = 1;
    public int LevelDash = 1;
    public int LevelLightningStrike = 1;
    public int LevelVenomIvyBlade = 1;
    public int LevelDivineSmite = 1;
    public int LevelFireFeet = 1;
    public int LevelHealingField = 1;

    public List<int> rewardsIndex;

    public int SlotsUsed = 0;

    int skillLearned1 = 0;
    int skillLearned2 = 0;
    int skillLearned3 = 0;

    public int percentageScaleEnemies = 0;


    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }
    void Start()
    {
        if (!PlayerPrefs.HasKey("HighScores")) PlayerPrefs.SetInt("HighScores", 0);
        HighScoreText.text = PlayerPrefs.GetInt("HighScores").ToString();
    }
    void Update()
    {
        if (activeClock)
        {
            if (EnemyParent.gameObject.transform.childCount == 0 && Waves == 10)
            {
                activeClock = false;

                for (int i = 0; i < Trash.transform.childCount; i++)
                    Destroy(Trash.transform.GetChild(i).gameObject);

                Rounds++;
                RoundsText.text = Rounds.ToString();
                ShowReward();         
            }
        }
    }
    public IEnumerator StartWave()
    {
        CountdownText.text = "3";
        yield return new WaitForSeconds(1);
        CountdownText.text = "2";
        yield return new WaitForSeconds(1);
        CountdownText.text = "1";
        yield return new WaitForSeconds(1);
        CountdownText.text = "";

        switch (Rounds)
        {
            case 5: EnemyAmount = 1; SecondsDelay = 7; break;
            case 10: EnemyAmount = 2; SecondsDelay = 6; break;
            case 50: EnemyAmount = 3; SecondsDelay = 5; break;

            default: break;
        }

        RoundsText.text = Rounds.ToString();

        //Spawn 9 Enemy Waves
        for (Waves = 1; Waves < 10; Waves++)
        {
            for (int i = 0; i < EnemyAmount; i++)
            {
                spawn = Instantiate(EnemyPool[Random.Range(0, EnemyPool.Length)], EnemySpawnPoints.transform.GetChild(Random.Range(0, EnemySpawnPoints.transform.childCount)).transform.position, Quaternion.identity);
                spawn.gameObject.transform.SetParent(EnemyParent.gameObject.transform);
            }

            yield return new WaitForSeconds(SecondsDelay);
        }
        //Spawn Boss
        //sound boss is coming
        RoundsText.text = "Boss is coming!\n10";
        yield return new WaitForSeconds(1);
        RoundsText.text = "Boss is coming!\n9";
        yield return new WaitForSeconds(1);
        RoundsText.text = "Boss is coming!\n8";
        yield return new WaitForSeconds(1);
        RoundsText.text = "Boss is coming!\n7";
        yield return new WaitForSeconds(1);
        RoundsText.text = "Boss is coming!\n6";
        yield return new WaitForSeconds(1);
        RoundsText.text = "Boss is coming!\n5";
        yield return new WaitForSeconds(1);
        RoundsText.text = "Boss is coming!\n4";
        yield return new WaitForSeconds(1);
        RoundsText.text = "Boss is coming!\n3";
        yield return new WaitForSeconds(1);
        RoundsText.text = "Boss is coming!\n2";
        yield return new WaitForSeconds(1);
        RoundsText.text = "Boss is coming!\n1";
        yield return new WaitForSeconds(1);
        RoundsText.text = "Boss fight!";

        spawn = Instantiate(BossPool[Random.Range(0, BossPool.Length)], BossSpawnPoint.transform.position, this.transform.rotation);
        spawn.gameObject.transform.SetParent(EnemyParent.gameObject.transform);

        activeClock = true;
    }
    public void ShowReward()
    {
        SetRewards();
        percentageScaleEnemies += 5;
    }
    public void SetRewards()
    {
        //1-3 Attribute improvements
        //4-11 Skills Order:(spintowin, divinesmite, veomwidowblade, firefeet, lightning blade, healing circle, sonic step, whirlwind)
        //12 extra loot
        //13 extra slot skill

        for (int i = 0; i < Rewards.transform.childCount; i++)
            Rewards.transform.GetChild(i).gameObject.GetComponent<RewardBehaviour>().SetSkill();

        rewardsIndex.Clear();
        for (int i = 0; i < 13; i++)
        {
            Rewards.transform.GetChild(i).gameObject.SetActive(false);
            rewardsIndex.Add(i+1);
        }

        if (LevelSpinToWin == 3 || SlotsUsed == 3) rewardsIndex.Remove(4); if (skillLearned1 == 1 || skillLearned2 == 1 || skillLearned3 == 1) rewardsIndex.Add(4);
        if (LevelDivineSmite == 3 || SlotsUsed == 3) rewardsIndex.Remove(5); if (skillLearned1 == 2 || skillLearned2 == 2 || skillLearned3 == 2) rewardsIndex.Add(5);
        if (LevelVenomIvyBlade == 3 || SlotsUsed == 3) rewardsIndex.Remove(6); if (skillLearned1 == 3 || skillLearned2 == 3 || skillLearned3 == 3) rewardsIndex.Add(6);
        if (LevelFireFeet == 3 || SlotsUsed == 3) rewardsIndex.Remove(7); if (skillLearned1 == 4 || skillLearned2 == 4 || skillLearned3 == 4) rewardsIndex.Add(7);
        if (LevelLightningStrike == 3 || SlotsUsed == 3) rewardsIndex.Remove(8); if (skillLearned1 == 5 || skillLearned2 == 5 || skillLearned3 == 5) rewardsIndex.Add(8);
        if (LevelHealingField == 3 || SlotsUsed == 3) rewardsIndex.Remove(9); if (skillLearned1 == 6 || skillLearned2 == 6 || skillLearned3 == 6) rewardsIndex.Add(9);
        if (LevelDash == 3 || SlotsUsed == 3) rewardsIndex.Remove(10); if (skillLearned1 == 7 || skillLearned2 == 7 || skillLearned3 == 7) rewardsIndex.Add(10);
        if (LevelWhirlwind == 3 || SlotsUsed == 3) rewardsIndex.Remove(11); if (skillLearned1 == 8 || skillLearned2 == 8 || skillLearned3 == 8) rewardsIndex.Add(11);

        if (extraLoot == true) rewardsIndex.Remove(12);
        if (extraSlot == true) rewardsIndex.Remove(13);

        int rnd1 = rewardsIndex[Random.Range(0, rewardsIndex.Count-1)];
        rewardsIndex.Remove(rnd1);

        int rnd2 = rewardsIndex[Random.Range(0, rewardsIndex.Count-1)];
        rewardsIndex.Remove(rnd2);

        int rnd3 = rewardsIndex[Random.Range(0, rewardsIndex.Count-1)];
        rewardsIndex.Remove(rnd3);

        int rnd4 = 0;
        if (rewardsIndex.Count > 0)
        {
            rnd4 = rewardsIndex[Random.Range(0, rewardsIndex.Count - 1)];
            rewardsIndex.Remove(rnd4);
        }

        Rewards.transform.GetChild(rnd1-1).gameObject.SetActive(true);
        Rewards.transform.GetChild(rnd2-1).gameObject.SetActive(true);
        Rewards.transform.GetChild(rnd3-1).gameObject.SetActive(true);
        if(extraLoot && rnd4 != 0) Rewards.transform.GetChild(rnd4-1).gameObject.SetActive(true);
        
        RewardUI.SetActive(true);
    }

    public void RewardChosen(string action, int rew)
    {
        switch(action)
        {
            case "learn":

                Transform slotSkillToPlace = null;

                if (skillLearned1 == 0) skillLearned1 = rew;
                else if (skillLearned2 == 0) skillLearned2 = rew;
                else if (skillLearned3 == 0) skillLearned3 = rew;

                switch (SlotsUsed)
                {
                    case 0: slotSkillToPlace = SkillPositionSlots[0].transform; break;
                    case 1: slotSkillToPlace = SkillPositionSlots[1].transform; break;
                    case 2: slotSkillToPlace = SkillPositionSlots[2].transform; break;
                    default: break;
                }

                switch (rew)
                {
                    case 1: ButtonSkills[0].gameObject.transform.position = slotSkillToPlace.position; ButtonSkills[0].gameObject.SetActive(true); LevelSpinToWin += 1; break;
                    case 2: ButtonSkills[1].gameObject.transform.position = slotSkillToPlace.position; ButtonSkills[1].gameObject.SetActive(true); LevelDivineSmite += 1; break;
                    case 3: ButtonSkills[2].gameObject.transform.position = slotSkillToPlace.position; ButtonSkills[2].gameObject.SetActive(true); LevelVenomIvyBlade += 1; break;
                    case 4: ButtonSkills[3].gameObject.transform.position = slotSkillToPlace.position; ButtonSkills[3].gameObject.SetActive(true); LevelFireFeet += 1; break;
                    case 5: ButtonSkills[4].gameObject.transform.position = slotSkillToPlace.position; ButtonSkills[4].gameObject.SetActive(true); LevelLightningStrike += 1; break;
                    case 6: ButtonSkills[5].gameObject.transform.position = slotSkillToPlace.position; ButtonSkills[5].gameObject.SetActive(true); LevelHealingField += 1; break;
                    case 7: ButtonSkills[6].gameObject.transform.position = slotSkillToPlace.position; ButtonSkills[6].gameObject.SetActive(true); LevelDash += 1; break;
                    case 8: ButtonSkills[7].gameObject.transform.position = slotSkillToPlace.position; ButtonSkills[7].gameObject.SetActive(true); LevelWhirlwind += 1; break;

                    default: break;
                }

                SlotsUsed += 1;

                break;
        }

        RewardUI.SetActive(false);
        activeClock = false;
        StartCoroutine(StartWave());
    }
    public void StartGame()
    {
        StartCoroutine(TransitionGame());
    }
    public IEnumerator TransitionGame()
    {
        MenuCanvas.SetActive(false);
        yield return new WaitForSeconds(2.25f);
        GameCanvas.SetActive(true);
        ShowReward();
    }
    public IEnumerator TransitionMenu()
    {
        GameCanvas.SetActive(false);
        yield return new WaitForSeconds(1.25f);
        if (Rounds > PlayerPrefs.GetInt("HighScores")) PlayerPrefs.SetInt("HighScores", Rounds);
        HighScoreText.text = PlayerPrefs.GetInt("HighScores").ToString();
        
        yield return new WaitForSeconds(2.0f);
        CameraRotate.Instance.ToggleCamera();
        yield return new WaitForSeconds(2.0f);

        for (int i = 0; i < Trash.transform.childCount; i++)
            Destroy(Trash.transform.GetChild(i).gameObject);

        for (int i = 0; i < EnemyParent.transform.childCount; i++)
            Destroy(EnemyParent.transform.GetChild(i).gameObject);

        LevelSpinToWin = 1;
        LevelWhirlwind = 1;
        LevelDash = 1;
        LevelLightningStrike = 1;
        LevelVenomIvyBlade = 1;
        LevelDivineSmite = 1;
        LevelFireFeet = 1;
        LevelHealingField = 1;

        ButtonSkills[0].gameObject.SetActive(false);
        ButtonSkills[1].gameObject.SetActive(false);
        ButtonSkills[2].gameObject.SetActive(false);
        ButtonSkills[3].gameObject.SetActive(false);
        ButtonSkills[4].gameObject.SetActive(false);
        ButtonSkills[5].gameObject.SetActive(false);
        ButtonSkills[6].gameObject.SetActive(false);
        ButtonSkills[7].gameObject.SetActive(false);

        extraSlot = false;
        extraLoot = false;  

        percentageScaleEnemies = 0;

        Destroy(player);
        Instantiate(PlayerObject, PlayerSpawnPoint.position, Quaternion.identity);

        MenuCanvas.SetActive(true);
    }
    public void EndGame()
    {
        StartCoroutine(TransitionMenu());
    }
}
