using System.Collections;
using UnityEngine;

namespace UI
{
    public class Loading : MonoBehaviour
    {
        public static Loading Instance;

        [SerializeField] private GameObject _loadingWindow;
        [SerializeField] private Transform _loadingImageTransform;
        [SerializeField] private float _rotatingSpeed;

        private Coroutine _loadingCoroutine;

        private void Awake()
        {
            if (Instance == null)
            {
                transform.SetParent(null);
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            StopLoadingCoroutine();
        }

        public void LoadingState(bool state)
        {
            StopLoadingCoroutine();
            if (state)
            {
                _loadingWindow.SetActive(true);
                _loadingCoroutine = StartCoroutine(LoadingCoroutine());
            }
        }

        private void StopLoadingCoroutine()
        {
            _loadingWindow.SetActive(false);
            if (_loadingCoroutine != null)
            {
                StopCoroutine(_loadingCoroutine);
                _loadingCoroutine = null;
            }
        }

        private IEnumerator LoadingCoroutine()
        {
            while (true)
            {
                _loadingImageTransform.Rotate(Vector3.forward * Time.deltaTime * _rotatingSpeed, Space.Self);
                yield return null;
            }
        }
    }
}