using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField]
    private QuestBase questAsset;

    [SerializeField]
    private QuestInterface communicate;

    public void PassQuest()
    {
        communicate
            .NewQuest?
            .Invoke(questAsset);
    }

}
