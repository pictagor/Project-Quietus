using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Quickstep : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] int arrowCount;
    [SerializeField] Color color1;
    [SerializeField] Color color2;
    [SerializeField] Material arrowMat;
    public List<Image> arrows;
    public List<string> directions;
    private bool isQuickstepping;
    private int arrowIndex;

    public static Quickstep instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CombatCamera.instance.onPrecombatStart.AddListener(EndQuickstep);
    }

    private void Update()
    {
        if (!isQuickstepping) { return; }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            CheckArrow();
        }
    }

    private void CheckArrow()
    {
        if (arrowIndex >= arrows.Count) { return; }
        switch (directions[arrowIndex])
        {
            case "RIGHT":
                if (Input.GetKeyDown(KeyCode.RightArrow)) { CorrectArrow(); }
                else { IncorrectArrow(); }
                break;

            case "UP":
                if (Input.GetKeyDown(KeyCode.UpArrow)) { CorrectArrow(); }
                else { IncorrectArrow(); }
                break;

            case "LEFT":
                if (Input.GetKeyDown(KeyCode.LeftArrow)) { CorrectArrow(); }
                else { IncorrectArrow(); }
                break;

            case "DOWN":
                if (Input.GetKeyDown(KeyCode.DownArrow)) { CorrectArrow(); }
                else { IncorrectArrow(); }
                break;

            default:
                Debug.Log("Incorrect string");
                break;
        }
    }

    private void CorrectArrow()
    {

        if(!arrows[arrowIndex].gameObject.activeSelf)
        {
            arrows[arrowIndex].gameObject.SetActive(true);
            arrows[arrowIndex].transform.parent.GetChild(1).gameObject.SetActive(false);
        }

        arrows[arrowIndex].transform.DOScale(1.2f, 0.2f);
        arrows[arrowIndex].color = color1;
        arrowIndex++;
        if (arrowIndex == arrows.Count)
        {
            Debug.Log("Successful Quickstep!");
            arrowMat.DOFloat(1, "_FadeAmount", 0.5f);
            PlayerController.instance.defendState = PlayerController.DefendState.Quickstepping;
        }
    }

    private void IncorrectArrow()
    {
        for (int i = 0; i < arrows.Count; i++)
        {
            arrows[i].transform.DOScale(1f, 0.2f);
            arrows[i].color = color2;
        }
        arrowIndex = 0;
    }

    public void SpawnArrows()
    {
        isQuickstepping = true;

        arrowMat.SetFloat("_FadeAmount", -0.1f);
        for (int i = 0; i < arrowCount; i++)
        {
            int direction = Random.Range(0, 4);
            switch (direction)
            {
                case 0:
                    GameObject arrow_RIGHT = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity, this.transform);
                    Image arrow_R = arrow_RIGHT.GetComponentInChildren<Image>();
                    arrow_R.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    arrows.Add(arrow_R);
                    directions.Add("RIGHT");
                    break;

                case 1:
                    GameObject arrow_UP = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity, this.transform);
                    Image arrow_U = arrow_UP.GetComponentInChildren<Image>();
                    arrow_U.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                    arrows.Add(arrow_U);
                    directions.Add("UP");
                    break;

                case 2:
                    GameObject arrow_LEFT = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity, this.transform);
                    Image arrow_L = arrow_LEFT.GetComponentInChildren<Image>();
                    arrow_L.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                    arrows.Add(arrow_L);
                    directions.Add("LEFT");
                    break;

                case 3:
                    GameObject arrow_DOWN = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity, this.transform);
                    Image arrow_D = arrow_DOWN.GetComponentInChildren<Image>();
                    arrow_D.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 270.0f);
                    arrows.Add(arrow_D);
                    directions.Add("DOWN");
                    break;

                default:
                    Debug.Log("Incorrect Direction Index");
                    break;
            }          
        }

        RandomizeArrowGuess();
    }

    private void RandomizeArrowGuess()
    {
        int randomGuess = Random.Range(1, arrows.Count);
        arrows[randomGuess].transform.parent.GetChild(1).gameObject.SetActive(true);
        arrows[randomGuess].gameObject.SetActive(false);
    }

    private void EndQuickstep()
    {
        isQuickstepping = false;
        for (int i = 0; i < arrows.Count; i++)
        {
            Destroy(arrows[i].transform.parent.gameObject);
        }

        arrows.Clear();
        directions.Clear();
        arrowIndex = 0;

        CombatHUD.instance.HideQueueText();
    }

}
