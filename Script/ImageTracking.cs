using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
    [SerializeField] private GameObject[] placeablePrefabs;
    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager aRTrackedImageManager;
    private void Awake()
    {
        aRTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        foreach (GameObject prefab in placeablePrefabs)
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            spawnedPrefabs.Add(prefab.name, newPrefab);
        }
    }

    private void OnEnable()
    {
        aRTrackedImageManager.trackedImagesChanged += ImageChanged;
    }
    private void OnDisable()
    {
        aRTrackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage aRtackedImage in eventArgs.added)
        {
            UpdateImage(aRtackedImage);
        }
        foreach (ARTrackedImage aRtackedImage in eventArgs.updated)
        {
            UpdateImage(aRtackedImage);
        }
        foreach (ARTrackedImage aRtackedImage in eventArgs.removed)
        {
            spawnedPrefabs[aRtackedImage.name].SetActive(false);
        }

    }
    private void UpdateImage(ARTrackedImage aRTrackedImage)
    {
        string name = aRTrackedImage.referenceImage.name;
        Vector3 position = aRTrackedImage.transform.position;

        GameObject prefab = spawnedPrefabs[name];
        prefab.transform.position = position;
        prefab.SetActive(true);

        foreach (GameObject go in spawnedPrefabs.Values)
        {
            if (go.name != name)
            {
                go.SetActive(false);
            }
        }
    }
}
