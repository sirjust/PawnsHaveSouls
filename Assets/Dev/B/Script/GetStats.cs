﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class GetStats : MonoBehaviour
{
    [Header("Requiered")]
    public Character character;
    public GameObject hearttemplate;


    [Header("Optional")]
    public int size = 4;
    public float gapBeforeLast = 2;
    public float multiplier = 2;

    [Header("Assigned Automatically")]
    public CharInfo charInfo;

    private BoxCollider boxCollider;
    private GameObject body;
    private GameObject[] allObj;
    private Slider healthbar;
    private GameObject heartsContainer;
    private List<GameObject> hearts = new List<GameObject>();


    private void Awake()
    {
        healthbar = GetComponentInChildren<Slider>();
        heartsContainer = GetComponentInChildren<Identify>().gameObject;
        allObj = FindObjectsOfType<GameObject>();

        foreach (GameObject _gameObject in allObj)
        {
            if (_gameObject.GetComponent<CharInfo>())
            {
                charInfo = _gameObject.GetComponent<CharInfo>();
            }
        }

        boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(size, size, size);

        var charObj = Instantiate(character.Model, this.gameObject.transform);
        charObj.transform.SetParent(this.gameObject.transform);
        body = charObj;

        if (character.healthRepresentation == HealthRepresentation.healthbar)
        {
            heartsContainer.SetActive(false);
            healthbar.maxValue = character.health;
            healthbar.value = character.currentHealth;
        }
        else
        {
            healthbar.gameObject.SetActive(false);
            for (int i = 0; i < character.hearts; i++)
            {
                InstantiateHearts(i);
            }
        }
        charInfo.DisableMenu(false);
    }

    public void InstantiateHearts(int index)
    {
        if (multiplier > 0)
        {
            var pos = new Vector3((0 + ((gapBeforeLast / multiplier) * index)), 0, 0);
            var heartObj = Instantiate(hearttemplate, pos, this.gameObject.transform.rotation);
            heartObj.transform.SetParent(heartsContainer.GetComponentInParent<Transform>().gameObject.transform);
            heartObj.transform.localPosition = pos;
            heartObj.GetComponent<Image>().color = Color.green;
            hearts.Add(heartObj);
            hearts[index].transform.localPosition -= new Vector3(gapBeforeLast, 0, 0);
        }
    }

    private void Update()
    {
        this.gameObject.transform.position = body.transform.position;
    }

    private void OnMouseDown()
    {
        charInfo.getCharID(character);
        charInfo.RefreshStats(character);
    }
}
