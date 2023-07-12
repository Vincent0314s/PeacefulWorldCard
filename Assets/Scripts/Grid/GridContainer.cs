using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Card
{
    public class GridContainer : MonoBehaviour
    {
        [SerializeField] private Grid _pf_Grid;
        [Min(1)]
        [SerializeField] private int _gridNumberX = 1;
        [Min(1)]
        [SerializeField] private int _gridNumberZ = 1;

        private int _totalGridNumber => _gridNumberX * _gridNumberZ;

        [SerializeField] private Vector3 _originPointToCreateGrid;
        [Min(1)]
        [SerializeField] private int xSpaceing = 1;
        [Min(1)]
        [SerializeField] private int zSpaceing = 1;
        [SerializeField] private bool _isPositiveZ = true;


        public List<Grid> grids = new List<Grid>();
        public List<Vector3> positionList = new List<Vector3>();

        public void CreateGrid()
        {
            ClearGrid();
            for (int i = 0; i < _totalGridNumber; i++)
            {
                Grid grid = Instantiate(_pf_Grid, transform);
                grids.Add(grid);
            }
            AssignGridPosition();
        }

        public void AssignGridPosition()
        {
            Vector3 newPos = _originPointToCreateGrid;
            newPos.x -= xSpaceing;
            for (int x = 0; x < _gridNumberX; x++)
            {
                newPos.x += xSpaceing;
                newPos.z = _isPositiveZ ? newPos.z = _originPointToCreateGrid.z - zSpaceing : newPos.z = _originPointToCreateGrid.z + zSpaceing;
                for (int z = 0; z < _gridNumberZ; z++)
                {
                    newPos.z = _isPositiveZ ? newPos.z += zSpaceing : newPos.z -= zSpaceing;
                    positionList.Add(newPos);
                }
            }

            for (int i = 0; i < grids.Count; i++)
            {
                grids[i].transform.localPosition = positionList[i];
            }
        }

        public void ClearGrid()
        {
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            grids.Clear();
            positionList.Clear();
        }
    }
}

