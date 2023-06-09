using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Penwyn.Tools
{
    public class AlertDialogBuilder : SingletonMonoBehaviour<AlertDialogBuilder>
    {
        public AlertDialog DialogPrefab;

        private AlertDialog _dialog;

        public AlertDialog Build(string label, string message)
        {
            AlertDialog dialog = Instantiate(DialogPrefab, this.transform.position, Quaternion.identity, this.transform);
            dialog.Label.SetText(label);
            dialog.Message.SetText(message);
            return dialog;
        }
    }

}