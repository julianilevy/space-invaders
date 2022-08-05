using UnityEngine;
using System.Collections.Generic;

namespace SpaceInvaders
{
    public abstract class GenericIterator<T> : MonoBehaviour where T : class
    {
        public bool cyclic = false;
        public List<T> orderedItems = new List<T>();
        public bool lastItemEnabled = false;
        public T lastItem = null;

        private bool _enabled = true;
        private int _currentItemIndex = 0;

        public void GoToNextItem()
        {
            if (_enabled)
            {
                _currentItemIndex++;

                if (_currentItemIndex >= orderedItems.Count)
                {
                    if (cyclic)
                        _currentItemIndex = 0;
                    else
                    {
                        GoToLastItem();
                        return;
                    }
                }

                OnGoToNextItem(_currentItemIndex);
            }
        }

        protected abstract void OnGoToNextItem(int index);

        public void GoToLastItem()
        {
            if (_enabled)
            {
                _enabled = false;

                if (lastItemEnabled)
                    OnGoToLastItem();
            }
        }

        protected abstract void OnGoToLastItem();

        public void Restart()
        {
            _enabled = true;
            _currentItemIndex = 0;

            OnRestart();
        }

        protected abstract void OnRestart();
    }
}