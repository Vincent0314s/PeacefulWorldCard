using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditorTool {
    public class BuildingInitCreator : MonoBehaviour
    {
        public int buildingSize = 1;
        public int buildingFloorNum = 1;
        private int buildingFloorIndex = 0;
        private List<GameObject> floors = new List<GameObject>();
        private const float FLOORSPACING = 0.25f;
        public int maxHeight = 15;

        public GameObject baseFloor;
        public GameObject baseRoof;
        private GameObject roof;
        private BoxCollider m_boxCollider;

        public void GenerateBuilding()
        {
            if (buildingFloorIndex < buildingFloorNum)
            {
                GameObject _floor = Instantiate(baseFloor, transform);
                floors.Add(_floor);
                _floor.transform.localPosition = new Vector3(0, FLOORSPACING * buildingFloorIndex, 0);
                _floor.name = "Floor_" + buildingFloorNum;
                buildingFloorIndex++;
            }
            else if (buildingFloorIndex > buildingFloorNum)
            {
                GameObject _floorNeedsToBeRemoved = floors[floors.Count - 1];
                DestroyImmediate(_floorNeedsToBeRemoved);
                floors.Remove(_floorNeedsToBeRemoved);
                buildingFloorIndex--;
            }
            if (roof == null)
            {
                roof = Instantiate(baseRoof, transform);
            }
            else
            {
                roof.transform.localPosition = new Vector3(0, FLOORSPACING * buildingFloorIndex, 0);
                roof.transform.SetAsLastSibling();
            }

            transform.localScale = Vector3.one * buildingSize;
        }

        public void RandomGenerateBuilding(float _buildingSize)
        {
            m_boxCollider = GetComponent<BoxCollider>();
            int randomHeight = Random.Range(1, maxHeight);

            if (roof == null)
            {
                roof = Instantiate(baseRoof, transform);
            }
            for (int i = 0; i < randomHeight; i++)
            {
                GameObject _floor = Instantiate(baseFloor, transform);
                _floor.transform.localPosition = new Vector3(0, FLOORSPACING * i, 0);
                _floor.name = "Floor_" + i;
                roof.transform.localPosition = new Vector3(0, FLOORSPACING * (i + 1), 0);
                roof.transform.SetAsLastSibling();
            }
            transform.localScale = Vector3.one * _buildingSize;
            CreateBoxColliderOnBuilding();
        }

        private void CreateBoxColliderOnBuilding()
        {
            float ySize = roof.transform.localPosition.y + 0.25f;
            float yCenter = ySize / 2;

            m_boxCollider.center = new Vector3(0, yCenter, 0);
            m_boxCollider.size = new Vector3(1, ySize, 1);
        }
    }

}
