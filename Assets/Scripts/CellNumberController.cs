using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cattoku
{
    public class CellNumberController : MonoBehaviour
    {

        // Instance variable
        [SerializeField] private Transform locationToPlace = default; // The cell to be placed, assigned in Unity

        private const float MinDistance = 0.001f; // min dist to detect the GameObject
        private const float MaxDistance = 0.1f; // max dist to detect the GameObject

        private bool shouldCheckPlacement;

        private AudioSource audioSource; // audio feedback on placement
        public List<Collider> colliders;
        private List<CellNumberController> cnc; // list of all number in the cell

        // For reset the puzzle
        private Transform originalParent;
        private Vector3 originalPosition;
        private Quaternion originalRotation;

        private IEnumerator checkPlacementCoroutine; // constant check if item is in place

        private bool hasAudioSource;
        private bool hasToolTip;

        private bool isPlaced; // check if GameObject is placed in the cell

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void Start()
        {
            // If the object is not at the position yet, should check placement
            if (locationToPlace != transform) shouldCheckPlacement = true;

            audioSource = GetComponent<AudioSource>();

            colliders = new List<Collider>();
            if (shouldCheckPlacement)
            {
                foreach (var col in GetComponents<Collider>())
                {
                    colliders.Add(col);
                    CheckColliderCount();
                }
            }

            cnc = new List<CellNumberController>();
            foreach (var controller in FindObjectsByType<CellNumberController>(FindObjectsSortMode.None))
                cnc.Add(controller);

            var trans = transform;
            originalParent = trans.parent;
            originalPosition = trans.localPosition;
            originalRotation = trans.localRotation;

            checkPlacementCoroutine = CheckPlacement();

            hasAudioSource = audioSource != null;

            if (shouldCheckPlacement) StartCoroutine(checkPlacementCoroutine);
        }

        /// <summary>
        ///     Triggers the placement feature.
        /// </summary>
        private void SetPlacement()
        {
            Set();
        }

        /// <summary>
        ///     Parents the part to the assembly and places the part at the target location.
        /// </summary>
        public void Set()
        {
            // Update placement state
            isPlaced = true;

            // Play audio snapping sound
            if (hasAudioSource) audioSource.Play();

            // Disable ability to manipulate object
            foreach (var col in colliders) col.enabled = false;

            // Set parent and placement of object to target
            var trans = transform;
            trans.SetParent(locationToPlace.parent);
            trans.position = locationToPlace.position;
            trans.rotation = locationToPlace.rotation;
        }

        /// <summary>
        ///     Triggers the reset placement feature.
        ///     Hooked up in Unity.
        /// </summary>
        public void ResetPlacement()
        {
            foreach (var controller in cnc)
                controller.Reset();
        }

        /// <summary>
        ///     Resets the part's parent and placement.
        /// </summary>
        public void Reset()
        {
            // Update placement state
            isPlaced = false;

            // Enable ability to manipulate object
            foreach (var col in colliders) col.enabled = true;

            // Reset parent and placement of object
            var trans = transform;
            trans.SetParent(originalParent);
            trans.localPosition = originalPosition;
            trans.localRotation = originalRotation;
        }

        /// <summary>
        ///     Checks the part's position and snaps/keeps it in place if the distance to target conditions are met.
        /// </summary>
        private IEnumerator CheckPlacement()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.01f);

                if (!isPlaced)
                {
                    if (Vector3.Distance(transform.position, locationToPlace.position) > MinDistance &&
                        Vector3.Distance(transform.position, locationToPlace.position) < MaxDistance)
                        SetPlacement();
                }
                else if (isPlaced)
                {
                    if (!(Vector3.Distance(transform.position, locationToPlace.position) > MinDistance)) continue;
                    var trans = transform;
                    trans.position = locationToPlace.position;
                    trans.rotation = locationToPlace.rotation;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Check if the Collider list is working
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public int CheckColliderCount()
        {
            return colliders.Count;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
