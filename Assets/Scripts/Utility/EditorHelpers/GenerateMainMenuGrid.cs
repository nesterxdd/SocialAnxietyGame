using UnityEngine;
using UnityEngine.Serialization;

namespace Utility.EditorHelpers
{
    public class GenerateMainMenuGrid : MonoBehaviour
    {
        [SerializeField] private int _sizeX;
        [SerializeField] private int _sizeZ;
        [SerializeField] private Vector2 _scale;
        [SerializeField] private Vector2 _height;
        [SerializeField] private GameObject _gridPrefab;
        [SerializeField] private Material[] _materials;

        [ContextMenu("Generate")]
        private void Generate()
        {
            Transform gridTransform = transform;
            int childCount = gridTransform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(gridTransform.GetChild(i).gameObject);
            }

            float minX = (-_sizeX * _scale.x / 2.0f) + 0.5f * _scale.x;
            float minZ = (-_sizeZ * _scale.y / 2.0f) + 0.5f * _scale.y;

            for (int x = 0; x < _sizeX; x++)
            {
                for (int z = 0; z < _sizeZ; z++)
                {
                    float height = Random.Range(_height.x, _height.y);

                    GameObject created
                        = Instantiate(
                            _gridPrefab,
                            new Vector3(minX + x * _scale.x, height / 2.0f, minZ + z * _scale.y),
                            Quaternion.identity);

                    created.transform.localScale = new Vector3(_scale.x, height, _scale.y);
                    created.transform.parent = gridTransform;
                    created.GetComponent<MeshRenderer>().material = _materials[Random.Range(0, _materials.Length)];
                }
            }
        }
    }
}
