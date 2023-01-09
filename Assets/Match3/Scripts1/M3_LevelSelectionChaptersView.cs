using System;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Wooga.Signals;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020006E6 RID: 1766
namespace Match3.Scripts1
{
	public class M3_LevelSelectionChaptersView : MonoBehaviour, IHandler<ChapterInfo>
	{
		// Token: 0x06002BD7 RID: 11223 RVA: 0x000C9A2C File Offset: 0x000C7E2C
		public static ChapterInfo CreateChapterInfo(QuestService questService, ChapterData data)
		{
			return new ChapterInfo
			{
				id = data.chapter,
				firstlevel = data.first_level,
				state = ((!M3_LevelSelectionChaptersView.IsChapterUnlocked(questService, data)) ? ChapterState.Locked : ChapterState.Complete)
			};
		}

		// Token: 0x06002BD8 RID: 11224 RVA: 0x000C9A71 File Offset: 0x000C7E71
		public static bool IsChapterUnlocked(QuestService questService, ChapterData data)
		{
			return questService.UnlockedLevelWithQuestAndEndOfContent >= data.first_level;
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06002BD9 RID: 11225 RVA: 0x000C9A84 File Offset: 0x000C7E84
		public ChapterData ActiveChapter
		{
			get
			{
				return Array.FindLast<ChapterData>(this.configService.chapter.chapters, (ChapterData c) => this.CheckSnapChapter(c) && M3_LevelSelectionChaptersView.IsChapterUnlocked(this.questService, c));
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06002BDA RID: 11226 RVA: 0x000C9AA8 File Offset: 0x000C7EA8
		public int LastUnlockedChapterID
		{
			get
			{
				if (this.lastUnlockedChapterID == -1)
				{
					ChapterData chapterData = Array.FindLast<ChapterData>(this.configService.chapter.chapters, (ChapterData c) => M3_LevelSelectionChaptersView.IsChapterUnlocked(this.questService, c));
					if (chapterData != null)
					{
						this.lastUnlockedChapterID = chapterData.chapter;
					}
				}
				return this.lastUnlockedChapterID;
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06002BDB RID: 11227 RVA: 0x000C9AFC File Offset: 0x000C7EFC
		public int currentChapterID
		{
			get
			{
				ChapterData activeChapter = this.ActiveChapter;
				return (activeChapter != null) ? activeChapter.chapter : 1;
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06002BDC RID: 11228 RVA: 0x000C9B22 File Offset: 0x000C7F22
		// (set) Token: 0x06002BDD RID: 11229 RVA: 0x000C9B2A File Offset: 0x000C7F2A
		public bool isDragging { get; private set; }

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06002BDE RID: 11230 RVA: 0x000C9B34 File Offset: 0x000C7F34
		public ChapterInfo[] Chapters
		{
			get
			{
				ChapterInfo[] result;
				if ((result = this.chapters) == null)
				{
					result = (this.chapters = this.GetChapters());
				}
				return result;
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06002BDF RID: 11231 RVA: 0x000C9B5D File Offset: 0x000C7F5D
		public int SelectedChapter
		{
			get
			{
				return this._selectedChapter;
			}
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x000C9B65 File Offset: 0x000C7F65
		private void Awake()
		{
			this.snapper.onBeginDrag.AddListener(new Action<PointerEventData>(this.HandleBeginDrag));
			this.snapper.onEndDrag.AddListener(new Action<PointerEventData>(this.HandleEndDrag));
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x000C9BA0 File Offset: 0x000C7FA0
		private void FixedUpdate()
		{
			bool flag = !this.isDragging && this._selectedChapter > 0;
			if (!this.isInitialized || flag)
			{
				return;
			}
			if (this.SelectedChapter != this.currentChapterID)
			{
				this.TrySetSelectedChapter(this.currentChapterID);
			}
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x000C9BF4 File Offset: 0x000C7FF4
		private void LateUpdate()
		{
			if (this.isDragging || !this.isInitialized)
			{
				return;
			}
			float chaptersSnapValue = this.GetChaptersSnapValue((float)this.SelectedChapter);
			ScrollRect scrollRect = this.snapper.scrollRect;
			if (Mathf.Abs(chaptersSnapValue - scrollRect.horizontalNormalizedPosition) <= 0.001f)
			{
				return;
			}
			scrollRect.horizontalNormalizedPosition = Mathf.SmoothDamp(scrollRect.horizontalNormalizedPosition, chaptersSnapValue, ref this.dampVelocity, this.snapper.scrollRect.elasticity, float.PositiveInfinity, Time.unscaledDeltaTime);
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x000C9C7C File Offset: 0x000C807C
		private void OnDisable()
		{
			this.lastUnlockedChapterID = -1;
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x000C9C88 File Offset: 0x000C8088
		public void TrySetSelectedChapter(int newChapter)
		{
			this._selectedChapter = Mathf.Min(newChapter, this.LastUnlockedChapterID);
			// 设置标题
			this.header.Set(this.locaService.GetText(string.Format("quest.chapter.name_{0}", this.SelectedChapter), new LocaParam[0]), this.SelectedChapter);
			this.dataSource.Show(this.GetChapters());
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x000C9CEF File Offset: 0x000C80EF
		public void Init(ILocalizationService locaService, ConfigService configService, QuestService questService)
		{
			this.locaService = locaService;
			this.configService = configService;
			this.questService = questService;
			this.isInitialized = true;
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x000C9D10 File Offset: 0x000C8110
		private ChapterInfo[] GetChapters()
		{
			ChapterInfo[] array = Array.ConvertAll<ChapterData, ChapterInfo>(this.configService.chapter.chapters, new Converter<ChapterData, ChapterInfo>(this.GetChapterInfo));
			ChapterInfo chapterInfo = null;
			for (int i = array.Length - 1; i >= 0; i--)
			{
				if (array[i].state != ChapterState.Locked)
				{
					chapterInfo = array[i];
					chapterInfo.isCurrent = true;
					break;
				}
			}
			if (chapterInfo == null)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Couldn't find current chapter, all chapters locked!"
				});
			}
			return array;
		}

		// Token: 0x06002BE7 RID: 11239 RVA: 0x000C9D8F File Offset: 0x000C818F
		public ChapterInfo GetChapterInfo(ChapterData data)
		{
			return M3_LevelSelectionChaptersView.CreateChapterInfo(this.questService, data);
		}

		// Token: 0x06002BE8 RID: 11240 RVA: 0x000C9D9D File Offset: 0x000C819D
		public void Handle(ChapterInfo chapter)
		{
			this.TrySetSelectedChapter(chapter.id);
			this.onChapterSelected.Dispatch(chapter);
		}

		// Token: 0x06002BE9 RID: 11241 RVA: 0x000C9DB7 File Offset: 0x000C81B7
		private float GetChaptersSnapValue(float chapter)
		{
			return (chapter - 1f) / Mathf.Max(1f, (float)(this.configService.chapter.chapters.Length - 1));
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x000C9DE0 File Offset: 0x000C81E0
		private bool CheckSnapChapter(ChapterData chapter)
		{
			int chapter2 = chapter.chapter;
			float horizontalNormalizedPosition = this.snapper.scrollRect.horizontalNormalizedPosition;
			return this.GetChaptersSnapValue((float)chapter2 + -0.5f) < horizontalNormalizedPosition;
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x000C9E16 File Offset: 0x000C8216
		private void HandleBeginDrag(PointerEventData evt)
		{
			if (!this.isInitialized)
			{
				return;
			}
			this.chapterPreviouslySelected = this.SelectedChapter;
			this.isDragging = true;
			this.dampVelocity = 0f;
		}

		// Token: 0x06002BEC RID: 11244 RVA: 0x000C9E44 File Offset: 0x000C8244
		private void HandleEndDrag(PointerEventData evt)
		{
			if (!this.isInitialized || !this.isDragging)
			{
				return;
			}
			this.isDragging = false;
			if (this.chapterPreviouslySelected != this.SelectedChapter)
			{
				this.TrySetSelectedChapter(this.SelectedChapter);
				this.onChapterSelected.Dispatch(this.Chapters[this.SelectedChapter - 1]);
			}
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x000C9EA6 File Offset: 0x000C82A6
		public void SnapToChapter(int chapter)
		{
			if (this.isDragging)
			{
				return;
			}
			this.TrySetSelectedChapter(chapter);
		}

		// Token: 0x04005500 RID: 21760
		private const float CHAPTER_SNAP_CORRECTION = -0.5f;

		// Token: 0x04005501 RID: 21761
		private const int NOT_INITIALIZED = -1;

		// Token: 0x04005502 RID: 21762
		private ILocalizationService locaService;

		// Token: 0x04005503 RID: 21763
		private ConfigService configService;

		// Token: 0x04005504 RID: 21764
		private QuestService questService;

		// Token: 0x04005505 RID: 21765
		public ChaptersDataSource dataSource;

		// Token: 0x04005506 RID: 21766
		public TableViewSnapper snapper;

		// Token: 0x04005507 RID: 21767
		public HeaderUi header;

		// Token: 0x04005508 RID: 21768
		private bool isInitialized;

		// Token: 0x04005509 RID: 21769
		private ChapterInfo[] chapters;

		// Token: 0x0400550A RID: 21770
		private float dampVelocity;

		// Token: 0x0400550B RID: 21771
		private int chapterPreviouslySelected;

		// Token: 0x0400550C RID: 21772
		private int lastUnlockedChapterID = -1;

		// Token: 0x0400550E RID: 21774
		public readonly Signal<ChapterInfo> onChapterSelected = new Signal<ChapterInfo>();

		// Token: 0x0400550F RID: 21775
		private int _selectedChapter;
	}
}
