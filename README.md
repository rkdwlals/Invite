# Invite

NPC와 대화하며 미니게임 세 가지를 풀어 나가는 **스팀펑크 2D 게임**입니다.

- 2024년 7월~10월, 2인 개발
- 플레이 영상: [포트폴리오에서 보기](https://rkdwlals.github.io/#invite)
- Unity 6000.2.14f1 · C# · AudioMixer

## 맡은 역할

| 담당 | 내용 |
|---|---|
| 강지민 | 기획, 메인 개발, 사운드 디자인 |
| 팀원 | 시작 화면, 카드 게임 UI |

### 개발
- NPC를 클릭하면 대사가 한 글자씩 나오는 대화 시스템. 대화가 끝나면 NPC에 따라 해당 미니게임으로 이동
- 카드 게임: 카드를 끌어다 카트에 넣되, 정해진 순서대로 넣어야 통과하고 가짜 카드를 넣으면 경고
- 퍼즐 게임: 그림 조각을 누를 때마다 90도씩 회전, 모두 바로 세우면 단서 표시
- 검 게임: 여러 자루 중 진짜 검을 찾아 클릭
- 캐릭터 이동과 애니메이션 전환(`PlayerMovement`, `PlayerMovementData`, `PlayerAnimation`)
- 발판을 밟으면 다른 방으로 이동하는 텔레포트, 플레이어를 따라가는 카메라
- 맵은 외부 타일 그림을 한 칸씩 직접 찍어 완성 (씬 파일에 저장)

### 사운드
- 게임 진행도에 맞춰 장면 6개에 BGM을 한 곡씩 배치 (Suno AI로 생성)
- AudioMixer를 만들어 BGM과 효과음이 모두 믹서를 거치게 구성

## 폴더 구성

```
Assets/
  Script/                    텔레포트, 카메라, 미니게임(퍼즐·검), 시작 화면 등
  Assets_ㅇ/Script/Dialogue/ 대화 시스템
  Assets_ㅇ/Script/CardGame/ 카드 게임
  Scenes/                    Start, Main, CardScene, PuzzleScene, SwordScene, End
  Sounds/                    BGM 6곡, AudioMixer
```

## 포함하지 않은 파일

라이선스 때문에 아래 외부 에셋은 저장소에 넣지 않았습니다. 프로젝트를 열면 그림이 빠져 보이니, 완성된 모습은 플레이 영상을 참고해 주세요.

- steampunk-int, Sprites, palette(타일), craftpix 배경, LivelyNPCs, Gentleland Steampunk UI, SteamPunk Card Set, Undead Survivor
- Ethan the Hero(캐릭터 그림·애니메이션 에셋)
- 종이 넘기는 효과음

## Claude가 수정한 부분

아래는 2026년 10월 포트폴리오 영상 녹화 전에 Claude(AI)가 고친 부분입니다.

- `ReturnPosition.cs`(새 파일), `DialogueManager.cs`: 미니게임을 마치면 들어가기 전 위치로 돌아오게 함
- `CubeClick.cs`: NPC 클릭이 다른 콜라이더에 가려 안 되던 문제 수정
- `SelectSword.cs`, `GameControl.cs`: 중복 씬 로드 방지, 클리어 후 2초 뒤 이동
- `AspectRatioEnforcer.cs`(새 파일): 화면을 16:9로 고정
- `PuzzleScene`, `SwordScene`: UI가 해상도에 맞춰 커지도록 Canvas Scaler 변경

README와 `.gitignore` 정리도 Claude의 도움을 받았습니다.
