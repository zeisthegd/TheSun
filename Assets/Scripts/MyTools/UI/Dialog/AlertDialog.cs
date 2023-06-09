using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace Penwyn.Tools
{
    public class AlertDialog : MonoBehaviour
    {
        public TMP_Text Label;
        public TMP_Text Message;

        public Button PositiveButton;
        public TMP_Text PositiveButtonTxt;
        public Button NegativeButton;
        public TMP_Text NegativeButtonTxt;

        public Action PositiveAction;
        public Action NegativeAction;

        public void SetPositiveButton(string btnLabel, Action method)
        {
            SetButton(PositiveButton, PositiveButtonTxt, btnLabel, method);
        }

        public void SetNegativeButton(string btnLabel, Action method)
        {
            SetButton(NegativeButton, NegativeButtonTxt, btnLabel, method);
        }

        public void SetButton(Button button, TMP_Text btnTMP, string btnLabel, Action method)
        {
            btnTMP.SetText(btnLabel);
            button.onClick.AddListener(() => { method?.Invoke(); });
            button.onClick.AddListener(() => { Close(); });
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}
