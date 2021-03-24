using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Quickstep : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] int arrowCount;
    public List<GameObject> arrows;
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
        CombatCamera.instance.onPrecombat.AddListener(EndQuickstep);
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
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    arrows[arrowIndex].transform.DOScale(1.2f, 0.2f);
                    arrowIndex++;
                    if (arrowIndex == arrows.Count)
                    {
                        Debug.Log("Successful Quickstep!");
                        PlayerController.instance.defendState = PlayerController.DefendState.Quickstepping;
                    }
                }
                else
                {
                    Debug.Log("Incorrect Arrow!");
                    arrows[arrowIndex].transform.DOScale(1f, 0.2f);
                    arrowIndex = 0;
                }
                break;

            case "UP":
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    Debug.Log("Correct Arrow!");
                    arrows[arrowIndex].transform.DOScale(1.2f, 0.2f);
                    arrowIndex++;
                    if (arrowIndex == arrows.Count)
                    {
                        Debug.Log("Successful Quickstep!");
                        PlayerController.instance.defendState = PlayerController.DefendState.Quickstepping;
                    }
                }
                else
                {
                    Debug.Log("Incorrect Arrow!");
                    arrows[arrowIndex].transform.DOScale(1f, 0.2f);
                    arrowIndex = 0;
                }
                break;

            case "LEFT":
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    Debug.Log("Correct Arrow!");
                    arrows[arrowIndex].transform.DOScale(1.2f, 0.2f);
                    arrowIndex++;
                    if (arrowIndex == arrows.Count)
                    {
                        Debug.Log("Successful Quickstep!");
                        PlayerController.instance.defendState = PlayerController.DefendState.Quickstepping;
                    }
                }
                else
                {
                    Debug.Log("Incorrect Arrow!");
                    arrows[arrowIndex].transform.DOScale(1f, 0.2f);
                    arrowIndex = 0;
                }
                break;

            case "DOWN":
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    Debug.Log("Correct Arrow!");
                    arrows[arrowIndex].transform.DOScale(1.2f, 0.2f);
                    arrowIndex++;
                    if (arrowIndex == arrows.Count)
                    {
                        Debug.Log("Successful Quickstep!");
                        PlayerController.instance.defendState = PlayerController.DefendState.Quickstepping;
                    }
                }
                else
                {
                    Debug.Log("Incorrect Arrow!");
                    arrows[arrowIndex].transform.DOScale(1f, 0.2f);
                    arrowIndex = 0;
                }
                break;

            default:
                Debug.Log("Incorrect string");
                break;
        }
    }

    public void SpawnArrows()
    {
        isQuickstepping = true;
        for (int i = 0; i < arrowCount; i++)
        {
            int direction = Random.Range(0, 4);
            switch (direction)
            {
                case 0:
                    GameObject arrow_RIGHT = Instantiate(arrowPrefab, Vector3.zero, Quaternion.Euler(0.0f, 0.0f, 0.0f), this.transform);
                    arrows.Add(arrow_RIGHT);
                    directions.Add("RIGHT");
                    break;

                case 1:
                    GameObject arrow_UP = Instantiate(arrowPrefab, Vector3.zero, Quaternion.Euler(0.0f, 0.0f, 90.0f), this.transform);
                    arrows.Add(arrow_UP);
                    directions.Add("UP");
                    break;

                case 2:
                    GameObject arrow_LEFT = Instantiate(arrowPrefab, Vector3.zero, Quaternion.Euler(0.0f, 0.0f, 180.0f), this.transform);
                    arrows.Add(arrow_LEFT);
                    directions.Add("LEFT");
                    break;

                case 3:
                    GameObject arrow_DOWN = Instantiate(arrowPrefab, Vector3.zero, Quaternion.Euler(0.0f, 0.0f, 270.0f), this.transform);
                    arrows.Add(arrow_DOWN);
                    directions.Add("DOWN");
                    break;

                default:
                    Debug.Log("Incorrect Direction Index");
                    break;
            }          
        }
    }

    private void EndQuickstep()
    {
        isQuickstepping = false;
        for (int i = 0; i < arrows.Count; i++)
        {
            Destroy(arrows[i]);
        }

        arrows.Clear();
        directions.Clear();
        arrowIndex = 0;
        CombatMenu.instance.actionQueued = false;
    }

}
