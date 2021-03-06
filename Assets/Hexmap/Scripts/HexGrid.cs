﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OwnHexMap
{
    public class HexGrid : MonoBehaviour
    {
        public Color defaultColor = Color.white;
        public Color touchedColor = Color.magenta;

        public int width = 6;
        public int height = 6;

        public HexCell cellPrefab;
        public Text cellLabelPrefab;
        Canvas gridCanvas;
        HexMesh hexMesh;

        HexCell[] cells;

        void Awake()
        {
            gridCanvas = GetComponentInChildren<Canvas>();
            hexMesh = GetComponentInChildren<HexMesh>();

            cells = new HexCell[height * width];

            for (int z = 0, i = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    CreateCell(x, z, i++);
                }
            }
        }

        // Use this for initialization
        void Start()
        {
            hexMesh.Triangulate(cells);
        }

        //void Update()
        //{
        //    if (Input.GetMouseButton(0))
        //    {
        //        HandleInput();
        //    }
        //}

        //void HandleInput()
        //{
        //    Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(inputRay, out hit))
        //    {
        //        TouchCell(hit.point);
        //    }
        //}

        //public void ColorCell(Vector3 position, Color color)
        //{
        //    position = transform.InverseTransformPoint(position);
        //    HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        //    int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        //    HexCell cell = cells[index];
        //    cell.color = color;
        //    hexMesh.Triangulate(cells);
        //}

        public void Refresh()
        {
            hexMesh.Triangulate(cells);
        }

        public HexCell GetCell(Vector3 position)
        {
            position = transform.InverseTransformPoint(position);
            HexCoordinates coordinates = HexCoordinates.FromPosition(position);
            int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
            return cells[index];
        }

        void CreateCell(int x, int z, int i)
        {
            Vector3 position;
            position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
            position.y = 0f;
            position.z = z * (HexMetrics.outerRadius * 1.5f);

            HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
            cell.transform.SetParent(transform, false);
            cell.transform.localPosition = position;
            cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

            Text label = Instantiate<Text>(cellLabelPrefab);
            label.rectTransform.SetParent(gridCanvas.transform, false);
            label.rectTransform.anchoredPosition =
                new Vector2(position.x, position.z);
            label.text = cell.coordinates.ToStringOnSeparateLines();

            cell.color = defaultColor;
            // Connect to neighbout on the left in grid
            if (x > 0)
            {
                cell.SetNeighbor(HexDirection.W, cells[i - 1]);
            }
            // Connect to SE and SW neighbours
            if (z > 0)
            {
                if ((z & 1) == 0) // Only even rows
                {
                    // Every even row cell has a south east neighbour
                    cell.SetNeighbor(HexDirection.SE, cells[i - width]);
                    if (x > 0) // For south west neighbours
                    {
                        cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
                    }
                }
                else // Mirrored for odd rows
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - width]);
                    if (x < width - 1)
                    {
                        cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
                    }
                }
            }

            cell.uiRect = label.rectTransform;
        }
    }
}