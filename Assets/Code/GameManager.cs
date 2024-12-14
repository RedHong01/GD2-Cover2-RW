using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public List<GameObject> sequence1Objects;
    public List<GameObject> sequence2Objects;
    public List<GameObject> sequence3Objects;
    public List<GameObject> sequence4Objects;
    public List<GameObject> sequence5Objects;
    public List<GameObject> sequence6Objects;

    public List<Button> sequence1Buttons;
    public GameObject fishGameObject;
    public TextMeshProUGUI hookedText;

    public int currentSequenceIndex = 0;
    [HideInInspector] public bool isFishHooked = false;

    private bool playerReacted = false;

    private FishEscape fishEscape; // 对FishEscape脚本的引用 // Reference to FishEscape script

    void Start()
    {
        // 获取 FishEscape 脚本的引用 // Get the reference to the FishEscape script
        fishEscape = fishGameObject.GetComponent<FishEscape>();

        foreach (Button button in sequence1Buttons)
        {
            if (button != null)
            {
                button.onClick.AddListener(() => OnSequence1ButtonClicked());
            }
        }

        ActivateSequence(currentSequenceIndex);
    }

    void Update()
    {
        if (currentSequenceIndex == 1 && Input.GetKeyDown(KeyCode.Space))
        {
            NextSequence();
        }

        if (currentSequenceIndex == 2 && isFishHooked && Input.GetKeyDown(KeyCode.Space))
        {
            playerReacted = true;
            if (hookedText != null)
            {
                hookedText.gameObject.SetActive(true);
                StartCoroutine(HandleHookedTransition());
            }
        }

        // 在序列4中检测钓鱼成功或失败 // In sequence 4, check fishing success or failure
        if (currentSequenceIndex == 3 && fishEscape.fishingSucceeded)
        {
            NextSequence(); // 切换到序列5 // Switch to sequence 5
        }
    }

    public void ActivateSequence(int sequenceIndex)
    {
        DeactivateAllSequences();

        switch (sequenceIndex)
        {
            case 0:
                SetActiveStateForList(sequence1Objects, true);
                break;
            case 1:
                SetActiveStateForList(sequence2Objects, true);
                break;
            case 2:
                SetActiveStateForList(sequence3Objects, true);
                break;
            case 3:
                SetActiveStateForList(sequence4Objects, true);
                if (fishGameObject != null)
                {
                    fishGameObject.SetActive(true);
                }
                break;
            case 4:
                SetActiveStateForList(sequence5Objects, true);
                break;
            case 5:
                SetActiveStateForList(sequence6Objects, true);
                break;
            default:
                Debug.LogWarning("Invalid sequence index!");
                break;
        }

        currentSequenceIndex = sequenceIndex;
    }

    private void DeactivateAllSequences()
    {
        SetActiveStateForList(sequence1Objects, false);
        SetActiveStateForList(sequence2Objects, false);
        SetActiveStateForList(sequence3Objects, false);
        SetActiveStateForList(sequence4Objects, false);
        SetActiveStateForList(sequence5Objects, false);
        SetActiveStateForList(sequence6Objects, false);

        if (fishGameObject != null)
        {
            fishGameObject.SetActive(false);
        }

        if (hookedText != null)
        {
            hookedText.gameObject.SetActive(false);
        }

        playerReacted = false;
    }

    private void SetActiveStateForList(List<GameObject> objects, bool state)
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                obj.SetActive(state);
            }
        }
    }

    public void NextSequence()
    {
        int nextSequenceIndex = (currentSequenceIndex + 1) % 6;
        ActivateSequence(nextSequenceIndex);
    }

    private void OnSequence1ButtonClicked()
    {
        if (currentSequenceIndex == 0)
        {
            NextSequence();
        }
    }

    private IEnumerator HandleHookedTransition()
    {
        yield return new WaitForSeconds(3f);
        NextSequence();
    }
}