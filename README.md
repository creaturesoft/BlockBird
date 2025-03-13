# Block Birds

[![Google Play](https://img.shields.io/badge/Download-Google%20Play-green?style=for-the-badge&logo=google-play)](https://play.google.com/store/apps/details?id=com.Creaturesoft.BlockBird)
[![App Store](https://img.shields.io/badge/Download-App%20Store-blue?style=for-the-badge&logo=app-store)](https://apps.apple.com/app/id6741071606)

- **Block Birds**는 간단하지만 중독성 있는 아케이드 게임입니다.
- 플레이어는 블럭 장애물을 부수며 점수를 획득하며 스테이지를 클리어하는 것이 목표입니다.
- 여러가지 맵과 캐릭터를 추가 중입니다.

## 게임 특징
- **간단한 조작** : 터치 한 번으로 조작하는 직관적인 게임플레이
- **다양한 캐릭터와 무기** : 다양한 캐릭터로 전략적으로 스테이지 클리어 가능
- **리더보드 시스템** : 점수를 기록하고 친구들과 경쟁
- **모바일 최적화** : iOS 및 Android 지원

## 다운로드
Block Birds를 지금 다운로드하세요! 

[![Google Play](https://img.shields.io/badge/Download-Google%20Play-green?style=for-the-badge&logo=google-play)](https://play.google.com/store/apps/details?id=com.Creaturesoft.BlockBird)
[![App Store](https://img.shields.io/badge/Download-App%20Store-blue?style=for-the-badge&logo=app-store)](https://apps.apple.com/app/id6741071606)

## 기술 스택
- **게임 엔진**: Unity 6
- **프로그래밍 언어**: C#
- **백엔드 & 데이터**: AWS Lambda, DynamoDB (유저 데이터 저장, 인앱결제 이력)
- **로그인** : Google Play Games Services, Apple Game Center, 게스트 모드, 오프라인 모드
- **UI, 아트, 이펙트, 사운드** : Unity Asset Store
- **기타 사용 패키지** : Google Mobile Ads, Unity IAP, Google Play In-app Review, Google Play In-app Updates

## 개발 및 빌드 이슈
- Unity 6 관련 GPGS 패키지 이슈 : GPGS 패키지 사용 시 자동 생성되는 파일(gpgs-plugin-support-2.0.0)의 옵션이 자동으로 체크되지 않아서 빌드 시 오류 발생
- 조치 방법 : 아래 경로의 파일에서 플랫폼 항목의 Android 옵션 체크
    Assets/GeneratedLocalRepo/GooglePlayGames/com.google.play.games/Editor/m2repository/com/google/games/gpgs-plugin-support/2.0.0/gpgs-plugin-support-2.0.0 플랫폼 Android 체크
