using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardBehaviour : MonoBehaviour
{
    public Button[] Buttons;
    public enum RewardType { health, power, speed, spintowin, divinesmite, veomwidowblade, firefeet, lightningblade, healingcircle, sonicstep, whirlwind, comodin, expandbusiness };
    public RewardType typeReward;
    public enum SelectType { skill, noskill };
    public SelectType selectReward;

    public TextMeshProUGUI textSkill;

    void Start()
    {
        SetSkill();
    }
    public void Learn()
    {
        switch (typeReward)
        {
            case RewardType.spintowin: GameManager.Instance.RewardChosen("learn", 1); break;
            case RewardType.divinesmite: GameManager.Instance.RewardChosen("learn", 2); break;
            case RewardType.veomwidowblade: GameManager.Instance.RewardChosen("learn", 3); break;
            case RewardType.firefeet: GameManager.Instance.RewardChosen("learn", 4); break;
            case RewardType.lightningblade: GameManager.Instance.RewardChosen("learn", 5); break;
            case RewardType.healingcircle: GameManager.Instance.RewardChosen("learn", 6); break;
            case RewardType.sonicstep: GameManager.Instance.RewardChosen("learn", 7); break;
            case RewardType.whirlwind: GameManager.Instance.RewardChosen("learn", 8); break;

            default: break;
        }
    }
    public void Improve()
    {
        switch (typeReward)
        {
            case RewardType.health: GameManager.Instance.player.GetComponent<PlayerController>().Life.TotalLife += GameManager.Instance.player.GetComponent<PlayerController>().Life.TotalLife / 10; break;
            case RewardType.power: GameManager.Instance.player.GetComponent<PlayerController>().power += GameManager.Instance.player.GetComponent<PlayerController>().power / 10; break;
            case RewardType.speed: GameManager.Instance.player.GetComponent<PlayerController>().power += 0.5f; break;
            case RewardType.comodin: GameManager.Instance.extraSlot = true; break;
            case RewardType.expandbusiness: GameManager.Instance.extraSlot = true; break;

            default: break;
        }

        GameManager.Instance.RewardChosen("improve", 0);
    }
    public void SetSkill()
    {
        switch (typeReward)
        {
            case RewardType.spintowin: if (GameManager.Instance.LevelSpinToWin == 2) textSkill.text = Constants.spintowin2; else if (GameManager.Instance.LevelSpinToWin == 3) textSkill.text = Constants.spintowin3; break;
            case RewardType.divinesmite: if (GameManager.Instance.LevelDivineSmite == 2) textSkill.text = Constants.divinesmite2; else if (GameManager.Instance.LevelDivineSmite == 3) textSkill.text = Constants.divinesmite3; break;
            case RewardType.veomwidowblade: if (GameManager.Instance.LevelVenomIvyBlade == 2) textSkill.text = Constants.venomwidowblade2; else if (GameManager.Instance.LevelVenomIvyBlade == 3) textSkill.text = Constants.venomwidowblade3; break;
            case RewardType.firefeet: if (GameManager.Instance.LevelFireFeet == 2) textSkill.text = Constants.firefeet2; else if (GameManager.Instance.LevelFireFeet == 3) textSkill.text = Constants.firefeet3; break;
            case RewardType.lightningblade: if (GameManager.Instance.LevelLightningStrike == 2) textSkill.text = Constants.lightningblade2; else if (GameManager.Instance.LevelLightningStrike == 3) textSkill.text = Constants.lightningblade3; break;
            case RewardType.healingcircle: if (GameManager.Instance.LevelHealingField == 2) textSkill.text = Constants.healingcircle2; else if (GameManager.Instance.LevelHealingField == 3) textSkill.text = Constants.healingcircle3; break;
            case RewardType.sonicstep: if (GameManager.Instance.LevelDash == 2) textSkill.text = Constants.sonicstep2; else if (GameManager.Instance.LevelDash == 3) textSkill.text = Constants.sonicstep3; break;
            case RewardType.whirlwind: if (GameManager.Instance.LevelWhirlwind == 2) textSkill.text = Constants.whirlwind2; else if (GameManager.Instance.LevelWhirlwind == 3) textSkill.text = Constants.whirlwind3; break;

            case RewardType.health: textSkill.text = Constants.healthimpr; break;
            case RewardType.power: textSkill.text = Constants.powerimpr; break;
            case RewardType.speed: textSkill.text = Constants.speedimpr; break;
            case RewardType.comodin: textSkill.text = Constants.wildcard; break;
            case RewardType.expandbusiness: textSkill.text = Constants.expandbusiness; break;

            default: break;
        }

        switch (selectReward)
        {
            case SelectType.skill: Buttons[0].gameObject.SetActive(true); break;
            case SelectType.noskill: Buttons[1].gameObject.SetActive(true); break;
        }
    }
}
