using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Penwyn.Game
{
    public class UniqueUIWindow : MonoBehaviour
    {
        public UnityEvent OnDisableEvent;

        public void OnEnable()
        {
            foreach (UniqueUIWindow window in FindObjectsOfType<UniqueUIWindow>())
            {
                if (window != this && window.gameObject.activeInHierarchy)
                {
                    window.gameObject.SetActive(false);
                }
            }
        }

        public void OnDisable()
        {
            OnDisableEvent?.Invoke();
        }
    }
}