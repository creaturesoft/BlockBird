using UnityEngine;

public static class AppInfoProvider
{
    private static AppInfoSO _cached;

    public static AppInfoSO Get()
    {
        if (_cached == null)
        {
            _cached = Resources.Load<AppInfoSO>("AppInfo");

            if (_cached == null)
                Debug.LogError("AppInfo.asset not found in Resources folder.");
        }

        return _cached;
    }
}
