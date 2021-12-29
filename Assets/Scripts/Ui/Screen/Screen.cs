using UnityEngine;


namespace HeroicOpportunity.Ui
{
    public class Screen : MonoBehaviour
    {
        #region Public methods

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }


        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        #endregion
    }
}
