#import <StoreKit/StoreKit.h>

extern "C" {
    bool RequestReviewiOS()
    {
        if (@available(iOS 10.3, *)) {
            [SKStoreReviewController requestReview];
            return true;
        }
        return false;
    }
}
