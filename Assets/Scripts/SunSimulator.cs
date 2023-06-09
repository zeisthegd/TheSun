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
        public TMP_Text AzimuthAngleTxt;
        public TMP_Text ShadowLengthTxt;

        [Header("Other Input")]
        [Tooltip("Object height in meter")]
        public float ObjectHeight = 1.75f;
        public float SunObjectDistance = 15f;

        public SunData SunData;

        private void Start()
        {
            SetExampleData();
        }

        private void Update()
        {
            DrawDebugRays();
        }

        [ContextMenu("Calculate")]
        public void Calculate()
        {
            if (AllInputFieldsArePresent())
            {
                if (IsInputDateValid())
                {
                    CalculateSunData();
                    UpdateOutput();
                    MoveSunToAngle();
                }
                else
                {
                    Debug.LogError("Invalid Input Date. Please Enter A New Date!");
                }

            }
            else
            {
                Debug.LogError("Missing Input Field(s). Please Assign In Inspector!");
            }
        }

        private void CalculateSunData()
        {
            SunData.Date = new System.DateTime(ToInt(YearInp.text), ToInt(MonthInp.text), ToInt(DayInp.text), ToInt(HourInp.text), ToInt(MinuteInp.text), ToInt(SecondsInp.text));
            SunData.DeclinationAngle = SunMath.DeclinationAngle(SunData.Date);
            SunData.SunAngle = SunMath.ElevationAngle(SunData.Date, ToFloat(LatitudeInp.text), ToFloat(LongitudeInp.text));
            SunData.AzimuthAngle = SunMath.AzimuthAngle(SunData.Date, ToFloat(LatitudeInp.text), ToFloat(LongitudeInp.text));
            SunData.ShadowLength = SunMath.ShadowLength(ObjectHeight, SunData.SunAngle);
        }

        public void UpdateOutput()
        {
            SunAngleTxt.SetText(SunData.SunAngle.ToString("#0.0000" + "\u00B0"));
            AzimuthAngleTxt.SetText(SunData.AzimuthAngle.ToString("#0.0000" + "\u00B0"));
            ShadowLengthTxt.SetText(SunData.ShadowLength.ToString("#0.00") + "m");
        }

        public void MoveSunToAngle()
        {
            Vector3 sunPos = Quaternion.AngleAxis(SunData.SunAngle, Vector3.forward) * Vector3.right * SunObjectDistance;
            sunPos.z = Mathf.Cos(SunData.AzimuthAngle * Mathf.Deg2Rad) * SunObjectDistance;
            sunPos.x = Mathf.Sin(SunData.AzimuthAngle * Mathf.Deg2Rad) * SunObjectDistance;

            this.transform.position = sunPos;
            this.transform.forward = (GameObject.FindGameObjectWithTag("Player").transform.position - this.transform.position);
        }

        private void DrawDebugRays()
        {
            Debug.DrawRay(Vector3.zero, this.transform.position - Vector3.zero, Color.black);
            Debug.DrawRay(Vector3.zero, new Vector3(this.transform.position.x, 0, this.transform.position.z) - Vector3.zero, Color.black);
            Debug.DrawRay(this.transform.position, new Vector3(this.transform.position.x, 0, this.transform.position.z) - this.transform.position, Color.black);
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

        #region Date Time Utility
        private bool IsInputDateValid()
        {
            return IsDateValid(ToInt(YearInp.text), ToInt(MonthInp.text), ToInt(DayInp.text), ToInt(HourInp.text), ToInt(MinuteInp.text), ToInt(SecondsInp.text));
        }

        public bool IsDateValid(int year, int month, int day, int hour, int minute, int seconds)
        {
            try
            {
                DateTime date = new System.DateTime(year, month, day, hour, minute, seconds);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Other Utility
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

        private float ToFloat(string input)
        {
            return float.Parse(input);
        }
        #endregion
    }
}