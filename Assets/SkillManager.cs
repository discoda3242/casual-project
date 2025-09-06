using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Skill
{
    public int id;
    public string skillName;
    public Sprite skillSprite;
}

public class SkillManager : MonoBehaviour
{
    public List<Skill> allSkills;
    public List<Image> currentSkillIcons;

    private List<Skill> currentSkills = new List<Skill>();

    // This is the function that resets all skills at the start.
    public void ResetSkills()
    {
        currentSkills.Clear();
        List<Skill> skillDeck = new List<Skill>(allSkills);

        for (int i = 0; i < currentSkillIcons.Count; i++)
        {
            if (skillDeck.Count > 0)
            {
                int randomIndex = Random.Range(0, skillDeck.Count);
                Skill newSkill = skillDeck[randomIndex];
                currentSkills.Add(newSkill);
                skillDeck.RemoveAt(randomIndex);
            }
        }
        UpdateSkillIcons();
    }

    // This is the new function that gets called to replace a single skill.
    public void ReplaceSkill(int indexToReplace)
    {
        // Get a new skill from the list of all skills.
        int randomIndex = Random.Range(0, allSkills.Count);
        Skill newSkill = allSkills[randomIndex];

        // Replace the skill at the specified index.
        currentSkills[indexToReplace] = newSkill;

        // Update the UI to show the new skill.
        currentSkillIcons[indexToReplace].sprite = newSkill.skillSprite;
    }

    // This updates all the skill icons at once.
    private void UpdateSkillIcons()
    {
        for (int i = 0; i < currentSkillIcons.Count; i++)
        {
            if (i < currentSkills.Count)
            {
                currentSkillIcons[i].sprite = currentSkills[i].skillSprite;
                currentSkillIcons[i].enabled = true;
            }
            else
            {
                currentSkillIcons[i].enabled = false;
            }
        }
    }
}