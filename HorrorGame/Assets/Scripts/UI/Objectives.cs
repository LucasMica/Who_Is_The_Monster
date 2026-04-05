using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.VisualScripting.Antlr3.Runtime.Tree.TreeWizard;

public class Objectives : MonoBehaviour
{
    public static Objectives Instance { get; private set; }

    [Header("Objective Settings")]
    public List<Objective> objectives = new List<Objective>();
    [SerializeField] private TextMeshProUGUI objectivesText;
    [SerializeField] private float transitionDuration = 0.4f;

    private CanvasGroup canvasGroup;
    private int currentIndex = 0;
    private bool isTransitioning;

    private int broomInteraction = 0;

    private AudioSource audioSource;
    public AudioClip objectiveCompleteSound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        canvasGroup = objectivesText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = objectivesText.gameObject.AddComponent<CanvasGroup>();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (currentIndex < objectives.Count)
        {
            objectivesText.text = objectives[currentIndex].description;
            canvasGroup.alpha = 1f;
        }
        else
        {
            objectivesText.text = "Go to Sleep";
            NextDay bed = GameObject.FindGameObjectWithTag("Bed").GetComponent<NextDay>();
            bed.canSkip = true;
        }
    }

    public void CompleteObjective(string id)
    {
        Objective obj = objectives.Find(o => o.id == id);

        if (obj != null && !obj.isCompleted)
        {
            obj.isCompleted = true;
            audioSource.PlayOneShot(objectiveCompleteSound);

            if (objectives[currentIndex].id == id)
            {
                currentIndex++;
                StartCoroutine(AnimateObjectiveChange());
            }
        }
    }

    private IEnumerator AnimateObjectiveChange()
    {
        isTransitioning = true;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / transitionDuration);
            yield return null;
        }

        UpdateUI();

        elapsed = 0f;
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / transitionDuration);
            yield return null;
        }

        isTransitioning = false;
    }

    public void ResetObjectives()
    {
        foreach (var o in objectives)
            o.isCompleted = false;

        currentIndex = 0;
        broomInteraction = 0;
        UpdateUI();
    }

    public void CallVisitors(){
    if (currentIndex < objectives.Count - 1)
        {
            currentIndex++;
            StartCoroutine(AnimateObjectiveChange());
            AddObjective("TalkWife", "Talk to your wife");
        }
    }

    public void AddObjective(string id, string description)
    {
        Objective objective = new Objective
        {
            id = id,
            description = description,
            isCompleted = false
        };
        objectives.Add(objective);
    }

    public string GetObjective()
    {
        return objectives[currentIndex].id;
    }

    public void incrementBroom()
    {
        broomInteraction++;
    }

    public int getBroom()
    {
        return broomInteraction;
    }
}
