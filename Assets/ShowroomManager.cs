﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using MetaUnits;
public class ShowroomManager : MonoBehaviour
{
    public Dropdown m_hullDropDown;
    public Dropdown m_moduleDropdown;
    public UnitBuilder m_unitBuilder;
    public Transform m_spawnPosition;

    private GameObject m_currentVehicle;
    private ModuleHardpoint m_currentHardpoint;
    // Use this for initialization
    void Start()
    {
        List<ModuleTypeMap> modulePrefabs = new List<ModuleTypeMap>(m_unitBuilder.m_modulePrefabs);
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        // Add all hulls to menu (this seems possibly silly)
        foreach (ModuleTypeMap modulePair in modulePrefabs)
        {
            string moduleName = modulePair.prefab.name;
            if (moduleName.ToLower().Contains("hull"))
            {
                options.Add(new Dropdown.OptionData(moduleName));
            }
        }
        m_hullDropDown.ClearOptions();
        m_hullDropDown.AddOptions(options);
        m_hullDropDown.onValueChanged.AddListener(delegate { HullSelected(m_hullDropDown); });

        m_moduleDropdown.ClearOptions();
        m_moduleDropdown.onValueChanged.AddListener(delegate { ModuleSelected(m_moduleDropdown); });
    }

    void HullSelected(Dropdown change)
    {
        if (m_currentVehicle != null)
        {
            Destroy(m_currentVehicle);
        }

        ModuleType hullType = (ModuleType)Enum.Parse(typeof(ModuleType), change.captionText.text);
        m_currentVehicle = m_unitBuilder.M_BuildUnit(hullType, m_spawnPosition);
    }

    void ModuleSelected(Dropdown change)
    {
        if (m_currentVehicle != null)
        {
            // Remove old, if it exists
            if (m_currentHardpoint.m_connectedTo)
            {
                Destroy(m_currentHardpoint.m_connectedTo);
            }

            // Create new module
            ModuleType type = (ModuleType)Enum.Parse(typeof(ModuleType), change.captionText.text);
            m_currentHardpoint.m_connectedTo = m_unitBuilder.M_BuildModule(type, m_currentHardpoint);
        }
    }

    public void M_HardpointSelected(ModuleHardpoint hardPoint)
    {
        m_currentHardpoint = hardPoint;
        m_moduleDropdown.ClearOptions();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        foreach (ModuleType moduleType in hardPoint.m_availableModules)
        {
            options.Add(new Dropdown.OptionData(moduleType.ToString()));
        }
        m_moduleDropdown.AddOptions(options);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
