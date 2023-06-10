using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

using Penwyn.Tools;

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
        public TMP_InputField TimeZoneInp;

        [Header("Sun Data Output")]
        public TMP_Text SunAngleTxt;
        public TMP_Text AzimuthAngleTxt;
        public TMP_Text ShadowLengthTxt;

        [Header("Other Input")]
        [Tooltip("Object height in meter")]
        public GameObject ObserverObject;
        public float SunObjectDistance = 15f;

        [Tooltip("Edit this if you want a default input value.")]
        public SunData SunData;

        private void Start()
        {
            SetSunDataToUI();
        }

        private void Update()
        {
            DrawDebugRays();
        }

        /// <summary>
        /// Validate input and start the calculation.
        /// </summary>
        public void GetDataAndCalculate()
        {
            StopAllCoroutines();
            if (AllInputFieldsArePresent())
            {
                if (IsInputDateValid() && IsCoordinateValid())
                {
                    GetInputDataFromUI();
                    CalculateSunData();
                }
            }
            else
            {
                Debug.LogError("Missing Input Field(s). Please Assign In Inspector!");
            }
        }

        /// <summary>
        /// Calculate and display output.
        /// </summary>
        public void CalculateSunData()
        {
            SunData.ComputeOutputData();
            UpdateOutput();
            SimulateSunPosition();
        }

        /// <summary>
        /// Collect the input data from the UI.
        /// </summary>
        private void GetInputDataFromUI()
        {
            SunData.Date = new System.DateTime(YearInp.text.ToInt(), MonthInp.text.ToInt(), DayInp.text.ToInt(),
                                HourInp.text.ToInt(), MinuteInp.text.ToInt(), SecondsInp.text.ToInt());
            SunData.Latitude = LatitudeInp.text.ToFloat();
            SunData.Longitude = LongitudeInp.text.ToFloat();
            SunData.TimeZone = TimeZoneInp.text.ToInt();
        }

        /// <summary>
        /// Display the output to UI.
        /// </summary>
        public void UpdateOutput()
        {
            SunAngleTxt.SetText(SunData.SunElevationAngle.ToString("#0.0000" + "\u00B0"));
            AzimuthAngleTxt.SetText(SunData.AzimuthAngle.ToString("#0.0000" + "\u00B0"));
            ShadowLengthTxt.SetText(SunData.ShadowLength.ToString("#0.00") + "m");
        }

        /// <summary>
        /// Simulate the Sun's position and rotation based on the calculations.
        /// </summary>
        public void SimulateSunPosition()
        {
            Vector3 sunPos = Quaternion.AngleAxis(SunData.SunElevationAngle, Vector3.forward) * Vector3.right * SunObjectDistance;
            Vector2 positionOnPlane = new Vector2(Mathf.Sin(SunData.AzimuthAngle * Mathf.Deg2Rad), Mathf.Cos(SunData.AzimuthAngle * Mathf.Deg2Rad)).normalized * SunObjectDistance;
            sunPos.x = ObserverObject.transform.position.x + positionOnPlane.x;
            sunPos.z = ObserverObject.transform.position.z + positionOnPlane.y;

            this.transform.position = sunPos;
            this.transform.forward = (GameObject.FindGameObjectWithTag("Player").transform.position - this.transform.position);
        }

        /// <summary>
        /// Generate a random SunData and calculate it.
        /// </summary>
        public void CalculateRandomData()
        {
            StopAllCoroutines();
            SunData.Latitude = Randomizer.Range(-90, 90);
            SunData.Longitude = Randomizer.Range(-180f, 180f);
            SunData.TimeZone = Randomizer.Range(-12, 14);
            SunData.Date = DateHelper.RandomDate();
            SetSunDataToUI();
            CalculateSunData();
        }

        /// <summary>
        /// Simulate the sun's angle and position from 0h to 23h.
        /// </summary>
        public void StartDayCircle()
        {
            StopAllCoroutines();
            StartCoroutine(DayCircleCoroutine());
        }

        private IEnumerator DayCircleCoroutine()
        {
            SunData.Date = SunData.Date.AddHours(-SunData.Date.Hour);
            for (int i = 0; i < 24; i++)
            {
                SetSunDataToUI();
                CalculateSunData();
                yield return new WaitForSeconds(1);
                SunData.Date = SunData.Date.AddHours(1);
            }
        }

        /// <summary>
        /// Draw some debug rays.
        /// </summary>
        private void DrawDebugRays()
        {
            Debug.DrawRay(ObserverObject.transform.position, this.transform.position - ObserverObject.transform.position, Color.black);
            Debug.DrawRay(ObserverObject.transform.position, new Vector3(this.transform.position.x, 0, this.transform.position.z) - ObserverObject.transform.position, Color.black);
            Debug.DrawRay(this.transform.position, new Vector3(this.transform.position.x, 0, this.transform.position.z) - this.transform.position, Color.black);
            Debug.DrawRay(ObserverObject.transform.position, new Vector3(Mathf.Sin(SunData.AzimuthAngle * Mathf.Deg2Rad), 0, Mathf.Cos(SunData.AzimuthAngle * Mathf.Deg2Rad)).normalized * SunObjectDistance, Color.green);

        }

        /// <summary>
        /// Set data to UI.
        /// </summary>
        private void SetSunDataToUI()
        {
            LatitudeInp.text = SunData.Latitude + "";
            LongitudeInp.text = SunData.Longitude + "";

            YearInp.text = SunData.Date.Year + "";
            MonthInp.text = SunData.Date.Month + "";
            DayInp.text = SunData.Date.Day + "";
            HourInp.text = SunData.Date.Hour + "";
            MinuteInp.text = SunData.Date.Minute + "";
            SecondsInp.text = SunData.Date.Second + "";
            TimeZoneInp.text = SunData.TimeZone.ToString();
        }

        #region Date Time Utility

        /// <summary>
        /// Check if the input date is valid or not.
        /// </summary>
        private bool IsInputDateValid()
        {
            if (DateHelper.IsDateValid(YearInp.text.ToInt(), MonthInp.text.ToInt(), DayInp.text.ToInt(),
                                HourInp.text.ToInt(), MinuteInp.text.ToInt(), SecondsInp.text.ToInt()))
            {
                return true;
            }
            Debug.LogError("Invalid Input Date. Please Enter A New Date!");
            return false;
        }

        /// <summary>
        /// Check if the input coordinate is valid or not. 
        /// Latitude must range from -90 to 90. Longitude must range from -180 to 180.
        /// </summary>
        private bool IsCoordinateValid()
        {
            if (-90 > LatitudeInp.text.ToFloat() || LatitudeInp.text.ToFloat() > 90
                || -180 > LongitudeInp.text.ToFloat() || LongitudeInp.text.ToFloat() > 180)
            {
                Debug.LogError("Invalid Coordinate. Please Enter New Latitude And Longitude!");
                return false;
            }
            return true;
        }


        #endregion

        #region Other Utility
        /// <summary>
        /// Check if all input field is present in the inspector.
        /// </summary>
        /// <returns></returns>
        private bool AllInputFieldsArePresent()
        {
            return LatitudeInp != null && LongitudeInp != null &&
                    YearInp != null && MonthInp != null && DayInp != null &&
                    HourInp != null && MinuteInp != null && SecondsInp != null;
        }
        #endregion
    }
}