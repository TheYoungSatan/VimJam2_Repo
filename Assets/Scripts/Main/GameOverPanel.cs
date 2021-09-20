using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private Text explenationtext;

    public void SetActive(bool val = true) => gameObject.SetActive(val);

    public void SetText(PlayerInfo info)
    {
        string quitText = $"Hunger: {info.HungerPercentage}% \nThirst: {info.ThurstPercentage}% \nTime awake: {info.AwakeTime}h";

        if (info.AwakeTime >= 30)
            quitText += "\ndue to over exhaustion";
        if (info.HungerPercentage >= 100)
            quitText += "\ndue to being to hungry";
        if (info.ThurstPercentage >= 100)
            quitText += "\ndue to being to thirsty";

        explenationtext.text = quitText;
    }
}