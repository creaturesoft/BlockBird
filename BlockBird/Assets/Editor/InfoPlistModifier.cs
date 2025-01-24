using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public class InfoPlistModifier
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuildProject)
    {
        if (target == BuildTarget.iOS)
        {
            // Info.plist 경로
            string plistPath = Path.Combine(pathToBuildProject, "Info.plist");

            // Info.plist 파일 읽기
            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            // Plist의 root 객체 가져오기
            PlistElementDict rootDict = plist.root;

            // 값 추가 예제: NSUserTrackingUsageDescription 추가
            rootDict.SetString("NSUserTrackingUsageDescription", "This app uses tracking to provide personalized ads.");
            rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", false);

            // 값 추가 예제: SKAdNetworkItems 추가
            PlistElementArray skAdNetworkItems = rootDict.CreateArray("SKAdNetworkItems");
            string[] skAdNetworkIds = {
                "cstr6suwn9.skadnetwork",
                "4fzdc2evr5.skadnetwork",
                "2fnua5tdw4.skadnetwork",
                "ydx93a7ass.skadnetwork",
                "p78axxw29g.skadnetwork",
                "v72qych5uu.skadnetwork",
                "ludvb6z3bs.skadnetwork",
                "cp8zw746q7.skadnetwork",
                "3sh42y64q3.skadnetwork",
                "c6k4g5qg8m.skadnetwork",
                "s39g8k73mm.skadnetwork",
                "3qy4746246.skadnetwork",
                "hs6bdukanm.skadnetwork",
                "mlmmfzh3r3.skadnetwork",
                "v4nxqhlyqp.skadnetwork",
                "wzmmz9fp6w.skadnetwork",
                "su67r6k2v3.skadnetwork",
                "yclnxrl5pm.skadnetwork",
                "7ug5zh24hu.skadnetwork",
                "gta9lk7p23.skadnetwork",
                "vutu7akeur.skadnetwork",
                "y5ghdn5j9k.skadnetwork",
                "v9wttpbfk9.skadnetwork",
                "n38lu8286q.skadnetwork",
                "47vhws6wlr.skadnetwork",
                "kbd757ywx3.skadnetwork",
                "9t245vhmpl.skadnetwork",
                "a2p9lx4jpn.skadnetwork",
                "22mmun2rn5.skadnetwork",
                "4468km3ulz.skadnetwork",
                "2u9pt9hc89.skadnetwork",
                "8s468mfl3y.skadnetwork",
                "ppxm28t8ap.skadnetwork",
                "uw77j35x4d.skadnetwork",
                "pwa73g5rt2.skadnetwork",
                "578prtvx9j.skadnetwork",
                "4dzt52r2t5.skadnetwork",
                "tl55sbb4fm.skadnetwork",
                "e5fvkxwrpn.skadnetwork",
                "8c4e2ghe7u.skadnetwork",
                "3rd42ekr43.skadnetwork",
                "3qcr597p9d.skadnetwork"
            };

            foreach (string id in skAdNetworkIds)
            {
                PlistElementDict itemDict = skAdNetworkItems.AddDict();
                itemDict.SetString("SKAdNetworkIdentifier", id);
            }

            // 수정된 plist 파일 저장
            plist.WriteToFile(plistPath);
        }
    }
}
