using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace Penwyn.TheSun
{
    public class SunSimulator : MonoBehaviour
    {
        [Header("Coordinate Input")]
        public TMP_InputField LatitudeInp;
        public TMP_InputField LongitudeInp;

        [Header("Date/Time Input")]
        public TMP_InputField YearInp;
        public TMP_InputField MonthInp;
        public TMP_InputField DayInp;
        public TMP_InputField HourInp;
        public TMP_InputField MinuteInp;
        public TMP_InputField SecondsInp;

        [Header("Sun Data Output")]
        public TMP_Text SunAngleTxt;
        public TMP_Text ShadowLengthTxt;

        [Header("Other Input")]
        [Tooltip("Object height in meter")]
        public float ObjectHeight = 1.75f;

        public SunData SunData;

        private void Start()
        {
            SetExampleData();
        }

        [ContextMenu("Calculate")]
        public void Calculate()
        {
            if (AllInputFieldsArePresent())
            {
                SunData.Date = new System.DateTime(ToInt(YearInp.text), ToInt(MonthInp.text), ToInt(DayInp.text), ToInt(HourInp.text), ToInt(MinuteInp.text), ToInt(SecondsInp.text));
                Debug.Log(SunData.Date);
                SunData.DeclinationAngle = SunMath.DeclinationAngle(SunData.Date);
                SunData.SunAngle = SunMath.SunAngle(SunData.Date, ToInt(LatitudeInp.text), 0);
                SunData.ShadowLength = SunMath.ShadowLength(ObjectHeight, SunData.SunAngle);

                UpdateOutput();
                MoveSunToAngle();
            }
            else
            {
                Debug.LogError("Missing Input Field(s). Please Assign In Inspector!");
            }
        }

        public void UpdateOutput()
        {
            SunAngleTxt.SetText(SunData.SunAngle.ToString("#0.0000") + "");
            ShadowLengthTxt.SetText(SunData.ShadowLength.ToString("#0.00") + "m");
        }

        public void MoveSunToAngle()
        {
            this.transform.position = Quaternion.AngleAxis(SunData.SunAngle, Vector3.forward) * Vector3.up * 10;
        }

        private void SetExampleData()
        {
            SunData.Date = DateTime.Now;
            LatitudeInp.text = SunData.Latitude + "";
            LongitudeInp.text = SunData.Longitude + "";

            YearInp.text = DateTime.Now.Year + "";
            MonthInp.text = DateTime.Now.Month + "";
            DayInp.text = DateTime.Now.Day + "";
            HourInp.text = DateTime.Now.Hour + "";
            MinuteInp.text = DateTime.Now.Minute + "";
            SecondsInp.text = DateTime.Now.Second + "";
        }

        private bool AllInputFieldsArePresent()
        {
            return LatitudeInp != null && LongitudeInp != null &&
                    YearInp != null && MonthInp != null && DayInp != null &&
                    HourInp != null && MinuteInp != null && SecondsInp != null;
        }

        private int ToInt(string input)
        {
            return int.Parse(input);
        }
    }
}