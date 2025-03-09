// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("DfsOQlVPJILl622dO13PDo4GdJuV2TTDVP03cjWzUZur6hEw+KFSJudkamVV52RvZ+dkZGXD8b/HS8UStpW6v9Y1t6vJZCj2wvzoSLDwyKeD0GFBaMVPPPKnah5wMKtiR8CrTQrXFI7dFlaZKGa4rk53kPZaQdKFVedkR1VoY2xP4y3jkmhkZGRgZWYY4tUKh8oGz8CUNZIsfmf8gTvZNCBYutvlwCsqkYW2ePESLncppx0fwVMBcv8BM2Y71qt0+Pqag00CLohWn30t+9nDcnISWXFoHoas2hrIa/msH23aHTnJ+Ayp6D9cvXqINXzqzGPFsVbse1RnwHAQmB+gz2fzrvGO/UyE0BRoOTB0zZibqwSR0LBy8Om4rH3BwuOk6mdmZGVk");
        private static int[] order = new int[] { 6,8,8,12,5,12,6,11,11,9,12,13,13,13,14 };
        private static int key = 101;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
