using LaserGRBL;
using System;

namespace Tools
{
    public class RetainedSetting<T>
    {
        // on change event
        public event Action<RetainedSetting<T>> OnChange;
        // setting name
        private readonly string mSettingName;
        // private value
        private T mValue;
        // public value
        public T Value
        {
            // just return private value
            get => mValue;
            set
            {
                // check if changed
                if (!Equals(mValue, value))
                {
                    // assign value
                    mValue = value;
                    // update settings
                    Settings.SetObject(mSettingName, mValue);
                    // invoke event
                    OnChange?.Invoke(this);
                }
            }
        }

        public RetainedSetting(string settingName, T defaultValue) {
            // save setting name
            mSettingName = settingName;
            // read current value
            Value = Settings.GetObject<T>(settingName, defaultValue);
        }


    }
}
