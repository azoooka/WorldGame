﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int m_alignment;
    public Dictionary<int, GameObject> m_selectedUnits;
    // Use this for initialization
    void Start()
    {
        m_selectedUnits = new Dictionary<int, GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void M_SelectUnits(List<GameObject> units)
    {
        foreach (GameObject unit in units)
        {
            int objId = unit.GetInstanceID();
            // Avoid adding the same unit twice
            if (m_selectedUnits.ContainsKey(objId))
            {
                continue;
            }
            m_selectedUnits.Add(unit.GetInstanceID(), unit);
            // If this is the human, also draw selection circle (not sure if this is the right place...)
            if(GetComponent<Human>())
            {
                DrawSelected(unit, true);
            }
        }
    }

    public void M_MoveSelectedUnits(Vector3 destination)
    {
        foreach (KeyValuePair<int, GameObject> pair in m_selectedUnits)
        {
            if (pair.Value == null)
                continue;
            GameObject obj = pair.Value;
            // Pretty hard coded for now. Have to be able to order multiple units
            BaseUnit thisUnit = obj.GetComponent<BaseUnit>();
            thisUnit.M_MoveOrder(destination);
        }
    }

    public void M_EngageWithSelectedUnits(List<GameObject> targets)
    {
        foreach (KeyValuePair<int, GameObject> pair in m_selectedUnits)
        {
            if (pair.Value == null)
                continue;
            GameObject obj = pair.Value;
            BaseUnit thisUnit = obj.GetComponent<BaseUnit>();
            if (thisUnit)
            {
                thisUnit.M_AttackOrder(targets);
            }
        }
    }

    public void M_ClearSelectedUnits()
    {
        foreach (KeyValuePair<int, GameObject> pair in m_selectedUnits)
        {
            if(GetComponent<Human>())
            {
                DrawSelected(pair.Value, false);
            }
        }
        m_selectedUnits.Clear();
    }


    private void DrawSelected(GameObject obj, bool selected)
    {
        if (selected)
        {
            obj.GetComponent<PlayerControlledEntity>().Select();
        }
        else
        {
            obj.GetComponent<PlayerControlledEntity>().DeSelect();
        }
    }
}
