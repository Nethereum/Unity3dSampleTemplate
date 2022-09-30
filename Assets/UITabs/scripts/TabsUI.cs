using UnityEngine ;
using UnityEngine.UI ;
using UnityEngine.Events ;

//------- Created by  : Hamza Herbou
//------- Email       : hamza95herbou@gmail.com

namespace EasyUI.Tabs {
   public enum TabsType{
      Horizontal,
      Vertical
   }
   public abstract class TabsUI : MonoBehaviour {

      [System.Serializable] public class TabsUIEvent : UnityEvent <int> {

      }

      [Header ("Tabs customization :")]
      [SerializeField] private Color themeColor = Color.gray ;
      [SerializeField] private float tabSpacing = 2f ;
      [Space]
      [Header ("OnTabChange event :")]
      public TabsUIEvent OnTabChange ;

      private TabButtonUI[] tabBtns ;
      private GameObject[] tabContent ;

      #if UNITY_EDITOR
      private LayoutGroup layoutGroup ;
      #endif

      private Color tabColorActive, tabColorInactive ;
      private int current, previous ;

      private Transform parentBtns, parentContent ;

      private int tabBtnsNum, tabContentNum ;


      private void Start () {
         GetTabBtns () ;
      }

      private void GetTabBtns () {
         parentBtns = transform.GetChild (0) ;
         parentContent = transform.GetChild (1) ;
         tabBtnsNum = parentBtns.childCount ;
         tabContentNum = parentContent.childCount ;

         if (tabBtnsNum != tabContentNum) {
            Debug.LogError ("!!Number of <b>[Buttons]</b> is not the same as <b>[Contents]</b> ("
            + tabBtnsNum + " buttons & " + tabContentNum
            + " Contents)") ;
            return ;
         }

         tabBtns = new TabButtonUI[tabBtnsNum] ;
         tabContent = new GameObject[tabBtnsNum] ;
         for (int i = 0; i < tabBtnsNum; i++) {
            tabBtns [ i ] = parentBtns.GetChild (i).GetComponent <TabButtonUI> () ;
            int i_copy = i ;
            tabBtns [ i ].uiButton.onClick.RemoveAllListeners () ;
            tabBtns [ i ].uiButton.onClick.AddListener (() => OnTabButtonClicked (i_copy)) ;

            tabContent [ i ] = parentContent.GetChild (i).gameObject ;
            tabContent [ i ].SetActive (false) ;
         }

         previous = current = 0 ;

         tabColorActive = tabBtns [ 0 ].uiImage.color ;
         tabColorInactive = tabBtns [ 1 ].uiImage.color ;

         tabBtns [ 0 ].uiButton.interactable = false ;
         tabContent [ 0 ].SetActive (true) ;
      }

      public void OnTabButtonClicked (int tabIndex) {
         if (current != tabIndex) {
            if (OnTabChange != null)
               OnTabChange.Invoke (tabIndex) ;

            previous = current ;
            current = tabIndex ;

            tabContent [ previous ].SetActive (false) ;
            tabContent [ current ].SetActive (true) ;

            tabBtns [ previous ].uiImage.color = tabColorInactive ;
            tabBtns [ current ].uiImage.color = tabColorActive ;

            tabBtns [ previous ].uiButton.interactable = true ;
            tabBtns [ current ].uiButton.interactable = false ;
         }
      }


      #if UNITY_EDITOR
      public void UpdateThemeColor (Color color) {
         tabBtns [ 0 ].uiImage.color = color ;
         Color colorDark = DarkenColor (color, 0.3f) ;
         for (int i = 1; i < tabBtnsNum; i++)
            tabBtns [ i ].uiImage.color = colorDark ;

         parentContent.GetComponent <Image> ().color = color ;
      }

      private Color DarkenColor (Color color, float amount) {
         float h, s, v ;
         Color.RGBToHSV (color, out h, out s, out v) ;
         v = Mathf.Max (0f, v - amount) ;
         return Color.HSVToRGB (h, s, v) ;
      }

      public void Validate (TabsType type) {
         parentBtns = transform.GetChild (0) ;
         parentContent = transform.GetChild (1) ;
         tabBtnsNum = parentBtns.childCount ;
         tabContentNum = parentContent.childCount ;

         tabBtns = new TabButtonUI[tabBtnsNum] ;
         tabContent = new GameObject[tabBtnsNum] ;

         for (int i = 0; i < tabBtnsNum; i++) {
            tabBtns [ i ] = parentBtns.GetChild (i).GetComponent <TabButtonUI> () ;
            tabContent [ i ] = parentContent.GetChild (i).gameObject ;
         }

         UpdateThemeColor (themeColor) ;

         if (layoutGroup == null)
            layoutGroup = parentBtns.GetComponent <LayoutGroup> () ;

         if (type == TabsType.Horizontal)
            ((HorizontalLayoutGroup)layoutGroup).spacing = tabSpacing ;
         else if (type == TabsType.Vertical)
            ((VerticalLayoutGroup)layoutGroup).spacing = tabSpacing ;

      }
      #endif

   }
}