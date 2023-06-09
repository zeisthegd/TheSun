using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.UI;

namespace Penwyn.Tools
{
    public class TabMenu : MonoBehaviour
    {
        public readonly List<Button> Buttons = new List<Button>();
        public readonly List<ScrollRect> Contents = new List<ScrollRect>();

        public Button ButtonPrefab;
        public ScrollRect ContentPrefab;

        public Transform ButtonContainer;
        public Transform ContentsContainer;

        protected virtual void Awake()
        {
            AssignButtonsOnClick();
        }

        public virtual void ButtonPressed(Button button)
        {
            for (int i = 0; i < Contents.Count; i++)
            {
                int buttonIndex = Buttons.IndexOf(button);
                Contents[i].gameObject.SetActive(buttonIndex == i);
            }
        }

        public void EnableTab(int index)
        {
            ButtonPressed(Buttons[index]);
        }

        public void CreateButton(string name, Sprite sprite)
        {
            Button button = Instantiate(ButtonPrefab, ButtonContainer.transform);
            button.onClick.AddListener(() => { ButtonPressed(button); });
            button.GetComponentsInChildren<Image>().Last().sprite = sprite;
            Buttons.Add(button);
        }

        public void CreateContent(string name)
        {
            ScrollRect content = Instantiate(ContentPrefab, ContentsContainer.transform);
            Contents.Add(content);
        }

        private void AssignButtonsOnClick()
        {
            foreach (Button button in Buttons)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => { ButtonPressed(button); });
            }
        }

    }
}
