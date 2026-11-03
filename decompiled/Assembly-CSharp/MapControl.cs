using System;
using CpClass;
using CpDefine;
using GameCommon;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    public enum PHASE
    {
        _INIT,
        _BACKUP_LOAD,
        _MAP_RESTART,
        _KEY_INPUT,
        _INPUT_MOVE,
        _EVENT_MOVE,
        _EVENT_CK,
        _EVENT_RUN,
        _EVENT_NEXT_MAP,
        _EVENT_MESSAGE_PAINT,
        _EVENT_SELECT_PAINT,
        _EVENT_MESSAGE_FADE_OUT,
        _EVENT_EFFECT_FADE,
        _PAUSE_MENU_I,
        _PAUSE_MENU_L,
        _PAUSE_MENU_E,
        _PAUSE_CONTINUE_SAVE_SELECT_WAIT,
        _PAUSE_BACKLOG_E,
        _PAUSE_NAMEPAINT_E,
        _PAUSE_CONFIG_E,
        _PAUSE_ITEM_PAINT_I,
        _PAUSE_ITEM_PAINT_L,
        _PAUSE_ITEM_PAINT_E
    }

    public PHASE NowMyPhase;

    public PHASE NextMyPhase;

    public int m_lPhaseCount;

    public int m_nFrameCount;

    public GameObject m_TalkWindowCtrlObj;

    public TalkWindowControl m_TalkWindowControl;

    public GameObject m_MapFrame;

    public int m_Chapter = 1;

    public CpEventDataControl m_cpEventDataControl;

    public Transform m_cpEventDataControlTrs;

    public CpEventControl m_cpEventControl;

    public CpEventControl m_cpOldEventControl;

    public Transform m_cpEventSubControlTrs;

    public CpEventSubControl m_cpEventSubControl;

    public Transform m_CpMapPaintControlTrs;

    public CpMapPaintControl m_CpMapPaintControl;

    public Transform m_CpCharacterPaintControlTrs;

    public CpCharacterPaintControl m_CpCharacterPaintControl;

    public Transform CpEventPicturePaintControlTrs;

    public CpEventPicturePaintControl m_CpEventPicturePaintControl;

    public Transform CpEffectControlTrs;

    public CpEffectControl m_CpEffectControl;

    public Transform CpMenuPaintControlTrs;

    public CpMenuPaintControl m_CpMenuPaintControl;

    public Transform CpBustUpPaintControlTrs;

    public CpBustUpPaintControl m_CpBustUpPaintControl;

    public Transform CpFacePaintControlTrs;

    public CpFacePaintControl m_CpFacePaintControl;

    public Transform CpItemPaintControlTrs;

    public CpItemPaintControl m_CpItemPaintControl;

    public GameObject MapCharacterRoot;

    public GameObject m_DebugMapSaveObj;

    public GameObject m_DebugMapMenuObj;

    public GameObject m_DebugDeathListPaintObj;

    public bool m_bEventMapChange;

    public bool m_bLoadFlag;

    public int m_lChaseCoolCount;

    public int m_lEventCoolCount;

    public int m_lAButtonEventCoolCount;

    public int m_lEventRunFlag;

    public int m_lEventRunWait;

    public int m_lOldEventNumber;

    public int m_BkupInfoNumber;

    public int PauseMenuIndex;

    public int PauseMenu_ItemIndex;

    public int PauseMenu_ItemMax;

    public int PauseMenu_ItemMode;

    public int EndSelectMode;

    public int OpenMessageMode;

    public float m_VolumeDataBGMPercentage;

    public int MapChangeButtonReTouchFlag;

    public int MapChangeButtonReTouchType;

    public int MessageWindowPaintFlag;

    public GameObject StaffCreditObj;

    public Animator SaffCreditAnime;

    public int Falset_MAP_RESTART_flag;

    public int message_skip_flag;

    public void Initialization()
    {
        GlobalVariable.m_TransitionCTRL.LoadAnimeActive(flag: true);
        MyDebugLog.Log_Time("MapControl Initialization", 5);
        GlobalVariable.m_SoundPlayerCTRL.Audio_StopBGM();
        m_Chapter = GlobalVariable.CapterNumber;
        m_bLoadFlag = false;
        GameObject original = Resources.Load<GameObject>("Prefab/Sub/TalkWindowControl");
        m_TalkWindowCtrlObj = UnityEngine.Object.Instantiate(original);
        m_TalkWindowCtrlObj.transform.SetParent(base.transform);
        m_TalkWindowControl = m_TalkWindowCtrlObj.GetComponent<TalkWindowControl>();
        m_TalkWindowControl.Initialization();
        //original = Resources.Load<GameObject>("UI/Common/MapFrame");
        //m_MapFrame = UnityEngine.Object.Instantiate(original);
        //m_MapFrame.transform.SetParent(base.transform);
        MapCharacterRoot = new GameObject();
        MapCharacterRoot.transform.SetParent(base.transform);
        MapCharacterRoot.name = "MapCharacterRoot";
        GlobalVariable.m_InfomationWindowControl.SetLanguageFont();
        m_cpEventControl = null;
        m_cpOldEventControl = null;
        m_cpEventDataControlTrs = base.transform.Find("CpEventDataControl");
        m_cpEventDataControl = m_cpEventDataControlTrs.GetComponent<CpEventDataControl>();
        m_cpEventDataControl.Initialization();
        m_cpEventDataControl.LoadCommonData();
        m_TalkWindowControl.SetVoiceList(m_cpEventDataControl.voicelist_txt);
        GlobalVariable.m_BackLogControl.SetVoiceList(m_cpEventDataControl.voicelist_txt);
        m_cpEventSubControlTrs = base.transform.Find("CpEventSubControl");
        m_cpEventSubControl = m_cpEventSubControlTrs.GetComponent<CpEventSubControl>();
        m_cpEventSubControl.Initialization();
        m_cpEventSubControl.SetChapterNumber(GlobalVariable.CapterNumber);
        m_CpMapPaintControlTrs = base.transform.Find("CpMapPaintControl");
        m_CpMapPaintControl = m_CpMapPaintControlTrs.GetComponent<CpMapPaintControl>();
        m_CpMapPaintControl.Initialization(m_Chapter);
        m_CpMapPaintControl.SetTransform(MapCharacterRoot.transform);
        m_CpCharacterPaintControlTrs = base.transform.Find("CpCharacterPaintControl");
        m_CpCharacterPaintControl = m_CpCharacterPaintControlTrs.GetComponent<CpCharacterPaintControl>();
        m_CpCharacterPaintControl.Initialization();
        m_CpCharacterPaintControl.SetTransform(MapCharacterRoot.transform);
        m_CpCharacterPaintControl.SetChapter(m_Chapter);
        m_CpCharacterPaintControl.SetEventSubControl(m_cpEventSubControl);
        CpEventPicturePaintControlTrs = base.transform.Find("CpEventPicturePaintControl");
        m_CpEventPicturePaintControl = CpEventPicturePaintControlTrs.GetComponent<CpEventPicturePaintControl>();
        m_CpEventPicturePaintControl.Initialization();
        m_CpEventPicturePaintControl.SetTransform(base.transform);
        m_CpEventPicturePaintControl.Set_eventgraphiclist_txt(m_cpEventDataControl.eventgraphiclist_txt);
        CpEffectControlTrs = base.transform.Find("CpEffectControl");
        m_CpEffectControl = CpEffectControlTrs.GetComponent<CpEffectControl>();
        m_CpEffectControl.Initialization();
        m_CpEffectControl.SetMapCharacterRoot(MapCharacterRoot);
        m_CpEffectControl.SetScreenEffect(GlobalVariable.m_SystemBackupData.config_MapScreenEffect);
        CpMenuPaintControlTrs = base.transform.Find("CpMenuPaintControl");
        m_CpMenuPaintControl = CpMenuPaintControlTrs.GetComponent<CpMenuPaintControl>();
        m_CpMenuPaintControl.Initialization();
        m_CpMenuPaintControl.SetMapNameList(m_cpEventDataControl.cpMapNameList);
        m_CpMenuPaintControl.SetSUBTITLE_LIST(GlobalVariable.m_CpDataControl.m_SubtitleList);
        CpBustUpPaintControlTrs = base.transform.Find("CpBustUpPaintControl");
        m_CpBustUpPaintControl = CpBustUpPaintControlTrs.GetComponent<CpBustUpPaintControl>();
        m_CpBustUpPaintControl.Initialization();
        m_CpBustUpPaintControl.Set_bustshotlist_txt(m_cpEventDataControl.bustshotlist_txt);
        CpFacePaintControlTrs = base.transform.Find("CpFacePaintControl");
        m_CpFacePaintControl = CpFacePaintControlTrs.GetComponent<CpFacePaintControl>();
        m_CpFacePaintControl.Initialization();
        m_CpFacePaintControl.Set_facelist_txt(m_cpEventDataControl.facelist_txt);
        CpItemPaintControlTrs = base.transform.Find("CpItemPaintControl");
        m_CpItemPaintControl = CpItemPaintControlTrs.GetComponent<CpItemPaintControl>();
        m_CpItemPaintControl.Initialization();
        m_CpItemPaintControl.Set_itemgraphiclist_txt(m_cpEventDataControl.itemgraphiclist_txt);
        GlobalVariable.m_AssetCTRL.LoadAssetBundle("Map/" + CpGameData.MapAssetBundleName[m_Chapter]);
        m_DebugMapMenuObj.SetActive(value: false);
        m_DebugMapSaveObj.SetActive(value: false);
        m_DebugDeathListPaintObj.SetActive(value: false);
        m_bEventMapChange = false;
        NextMyPhase = (NowMyPhase = PHASE._INIT);
        m_nFrameCount = 0;
        m_lEventCoolCount = 0;
        m_lAButtonEventCoolCount = 0;
        m_lChaseCoolCount = 0;
        m_lEventRunFlag = 1;
        m_lEventRunWait = 0;
        m_CpMenuPaintControl.SetActivePlayerStatus(flag: false, m_cpEventSubControl.m_Party_Set);
        m_CpMenuPaintControl.SetActiveItemList(flag: false);
        m_TalkWindowControl.SetLanguage(GlobalVariable.Language);
        EndSelectMode = 0;
        OpenMessageMode = 0;
        m_lOldEventNumber = 0;
        MapChangeButtonReTouchFlag = 0;
        MapChangeButtonReTouchType = 0;
        MyDebugLog.Log_Time("MapControl Initialization end", 5);
        MessageWindowPaintFlag = 0;
        m_BkupInfoNumber = 0;
        GlobalVariable.SkipMode = 0;
        GlobalVariable.SpeedUpLevel = 0;
        Falset_MAP_RESTART_flag = 0;
        message_skip_flag = 0;
    }

    public int MyUpdate()
    {
        int result = 0;
        int padNumber = 0;
        m_nFrameCount++;
        GlobalVariable.SkipMode = 0;
        GlobalVariable.SpeedUpLevel = 0;
        if (GlobalVariable.m_SystemBackupData.config_MessageSkip == 0 && PadInputCTRL.Get_Button2(padNumber, 0) >= 1)
        {
            GlobalVariable.SkipMode = 2;
        }
        if (!m_cpEventSubControl.m_ScriptEvent.bStaff && m_cpEventSubControl.m_OpenMesList.bFlag)
        {
            switch (OpenMessageMode)
            {
                case 0:
                    OpenMessageMode = 1;
                    break;
                case 1:
                {
                    OpenMessageMode = 2;
                    int num = m_cpEventSubControl.m_OpenMesList.nIndexList[m_cpEventSubControl.m_OpenMesList.nIndex];
                    if (num == 0)
                    {
                        OpenMessageMode = 10;
                        break;
                    }
                    m_cpEventSubControl.m_OpenMesList.nIndex++;
                    GlobalVariable.m_SystemWindowControl.SetSystemMessage(101 + num);
                    GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(457);
                    break;
                }
                case 2:
                    OpenMessageMode = 1;
                    break;
                case 10:
                    m_cpEventSubControl.m_OpenMesList.bFlag = false;
                    break;
            }
            return result;
        }
        if (EndSelectMode != 0)
        {
            if (GlobalVariable.m_SystemWindowControl.GetResult() == 1)
            {
                result = -1;
                GlobalVariable.m_InfomationWindowControl.SetInfoNumber(0);
                GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(457);
            }
            if (GlobalVariable.m_SystemWindowControl.GetResult() == 2)
            {
                EndSelectMode = 0;
                GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(456);
            }
            return result;
        }
        m_TalkWindowControl.MyUpdate();
        m_CpMapPaintControl.MyUpdate();
        m_CpCharacterPaintControl.MyUpdate();
        m_CpEffectControl.MyUpdate();
        m_CpMenuPaintControl.MyUpdate();
        m_CpBustUpPaintControl.MyUpdate();
        m_CpFacePaintControl.MyUpdate();
        m_CpItemPaintControl.MyUpdate();
        if (GlobalVariable.m_AssetCTRL.GetLoadFlag() == 0 && Falset_MAP_RESTART_flag != 0)
        {
            if (PadInputCTRL.Get_Button(padNumber, 8) == 1)
            {
                EndSelectMode = 1;
                GlobalVariable.m_SystemWindowControl.SetSystemMessage(4);
                GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(457);
                return result;
            }
            if (m_cpEventSubControl.m_ScriptEvent.bStaff)
            {
                m_cpEventSubControl.m_ScriptEvent.bStaff = false;
                GlobalVariable.m_InfomationWindowControl.SetInfoNumber(0);
                return -200;
            }
            if (m_cpEventSubControl.m_ScriptEvent.bButtonWait)
            {
                GlobalVariable.m_InfomationWindowControl.SetInfoNumber(7);
                if (PadInputCTRL.Get_Button(padNumber, 2) == 1)
                {
                    GlobalVariable.m_InfomationWindowControl.SetInfoNumber(0);
                    m_cpEventSubControl.m_ScriptEvent.bButtonWait = false;
                }
                return result;
            }
            if (m_cpEventSubControl.m_ScriptEvent.bToTitle)
            {
                m_TalkWindowControl.HideMessageWindow();
                m_cpEventSubControl.m_localData.m_bMessageWindowPaintFlag = false;
                if (m_CpBustUpPaintControl != null)
                {
                    m_CpBustUpPaintControl.FadeOutBustUp(0, nodestroy: true);
                    m_CpBustUpPaintControl.FadeOutBustUp(1, nodestroy: true);
                }
                if (m_CpFacePaintControl != null)
                {
                    m_CpFacePaintControl.FadeOutFace(nodestroy: true);
                }
                GlobalVariable.m_InfomationWindowControl.SetInfoNumber(0);
                if (m_cpEventSubControl.m_ScriptEvent.lToTitleEndCount > 0)
                {
                    m_cpEventSubControl.Event(EventCommandMap.EVENT_ENDING_PAINT);
                }
                else
                {
                    result = -1;
                    GlobalVariable.m_InfomationWindowControl.SetInfoNumber(0);
                    GlobalVariable.backUpDataFunction.Input_SystemData(GlobalVariable.m_SystemBackupData);
                    GlobalVariable.backUpDataFunction.Save_SystemData(GameData.savedataFileName[0]);
                }
                return result;
            }
        }
        return EventRun();
    }

    public void Termination()
    {
        m_TalkWindowControl.Termination();
        UnityEngine.Object.Destroy(m_TalkWindowCtrlObj);
        m_TalkWindowCtrlObj = null;
        UnityEngine.Object.Destroy(m_MapFrame);
        m_MapFrame = null;
        m_cpEventDataControl.Termination();
        m_cpEventSubControl.Termination();
        m_CpMapPaintControl.Termination();
        m_CpCharacterPaintControl.Termination();
        m_CpEffectControl.Termination();
        m_CpMenuPaintControl.Termination();
        m_CpBustUpPaintControl.Termination();
        m_CpFacePaintControl.Termination();
        m_CpItemPaintControl.Termination();
        GlobalVariable.m_SoundPlayerCTRL.Audio_StopSE(0);
        GlobalVariable.m_SoundPlayerCTRL.Audio_StopSE(4);
        GlobalVariable.m_SoundPlayerCTRL.Audio_StopSE(5);
        GlobalVariable.m_SoundPlayerCTRL.Audio_StopPointBGM();
        GlobalVariable.m_SoundPlayerCTRL.Audio_StopBGM();
        GlobalVariable.m_SoundPlayerCTRL.Audio_StopVoice();
    }

    public int EventRun()
    {
        int result = 0;
        int padNumber = 0;
        m_CpMapPaintControl.MyUpdate();
        m_CpCharacterPaintControl.MyUpdate();
        m_CpEventPicturePaintControl.MyUpdate();
        if (m_cpEventSubControl.m_localData.m_lSaveSelectIndex != 0)
        {
            GlobalVariable.SavePointNumber = m_cpEventSubControl.m_localData.m_lSaveSelectIndex;
            m_cpEventSubControl.m_localData.m_lSaveSelectIndex = 0;
            m_TalkWindowControl.HideMessageWindow();
            m_cpEventSubControl.m_localData.m_bMessageWindowPaintFlag = false;
            ExChangeBackupdata();
            if (m_lEventRunWait > 0)
            {
                m_lEventRunWait = 1;
                EventFunction();
            }
            m_BkupInfoNumber = GlobalVariable.m_InfomationWindowControl.GetPaintIndex();
            GlobalVariable.m_InfomationWindowControl.SetInfoNumber(6);
            return -101;
        }
        if (m_BkupInfoNumber != 0)
        {
            GlobalVariable.m_InfomationWindowControl.SetInfoNumber(m_BkupInfoNumber);
            m_BkupInfoNumber = 0;
        }
        if (MapChangeButtonReTouchType != 0)
        {
            if (PadInputCTRL.Get_Button2(padNumber, MapChangeButtonReTouchType) == 0)
            {
                MapChangeButtonReTouchFlag = 0;
                MapChangeButtonReTouchType = 0;
            }
        }
        else
        {
            MapChangeButtonReTouchFlag = 0;
        }
        switch (NowMyPhase)
        {
            case PHASE._INIT:
                if (GlobalVariable.m_AssetCTRL.GetLoadFlag() == 0)
                {
                    GlobalVariable.m_TransitionCTRL.LoadAnimeActive(flag: false);
                    MyDebugLog.Log_Time("MapControl PHASE._INIT", 5);
                    m_CpMapPaintControl.LoadMap(0);
                    m_CpMapPaintControl.SetMapPosition(32, 32);
                    LoadData(m_Chapter);
                    NextMyPhase = PHASE._MAP_RESTART;
                }
                break;
            case PHASE._BACKUP_LOAD:
            {
                MyDebugLog.Log_Time("MapControl PHASE._BACKUP_LOAD", 5);
                m_bLoadFlag = true;
                CHAPTER_DATA cHAPTER_DATA = GlobalVariable.m_GameBackupData.cHAPTER_DATA;
                message_skip_flag = GlobalVariable.m_GameBackupData.sysPAD2;
                MyDebugLog.Log_ColorType("pSaveData.nBgmNumber = " + cHAPTER_DATA.nBgmNumber, 5);
                if (cHAPTER_DATA.nBgmNumber != 0)
                {
                    GlobalVariable.m_SoundPlayerCTRL.Audio_PlayBGM(cHAPTER_DATA.nBgmNumber);
                }
                if (cHAPTER_DATA.nSeNumber != 0)
                {
                    GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(cHAPTER_DATA.nSeNumber, 4);
                }
                m_cpEventSubControl.m_PointBgm.lBgmNumber = cHAPTER_DATA.nPointBgmNumber;
                m_cpEventSubControl.m_PointBgm.cFlag = cHAPTER_DATA.PointBgm.cFlag;
                m_cpEventSubControl.m_PointBgm.cX = cHAPTER_DATA.PointBgm.cX;
                m_cpEventSubControl.m_PointBgm.cY = cHAPTER_DATA.PointBgm.cY;
                if (m_cpEventSubControl.m_PointBgm.cFlag != 0)
                {
                    GlobalVariable.m_SoundPlayerCTRL.Audio_PlayPointBGM(m_cpEventSubControl.m_PointBgm.lBgmNumber, 0f, 1);
                    m_cpEventSubControl.Event(EventCommandMap.EVENT_POINT_BGM_VOLUME);
                }
                m_cpEventSubControl.m_localData.fElapsedTime = cHAPTER_DATA.lElapsedTime;
                for (int j = 0; j < 8; j++)
                {
                    m_cpEventSubControl.m_localData.m_lEventFlag[j] = cHAPTER_DATA.lEventFlag[j];
                }
                m_cpEventControl.ImportEventFlag(m_cpEventSubControl.m_localData.m_lEventFlag);
                if (cHAPTER_DATA.cPlayerPaintFlag != 0)
                {
                    m_cpEventSubControl.m_ScriptEvent.bPlayCharacterPaintFlag = true;
                }
                else
                {
                    m_cpEventSubControl.m_ScriptEvent.bPlayCharacterPaintFlag = false;
                }
                m_cpEventSubControl.m_localData.m_lCharacterNumber = cHAPTER_DATA.lCharacterNumber;
                m_cpEventSubControl.m_ScriptEvent.lLightNumber = cHAPTER_DATA.LightNumber;
                m_cpEventSubControl.m_ScriptEvent.LimitTime.fLimitTimeCount = cHAPTER_DATA.lLimitTimeCount;
                if (m_cpEventSubControl.m_ScriptEvent.LimitTime.fLimitTimeCount > 0f)
                {
                    m_cpEventSubControl.m_ScriptEvent.LimitTime.bLimitTime = true;
                    m_CpMenuPaintControl.SetActiveLimitTime(flag: true);
                    m_CpMenuPaintControl.SetLimitTime(m_cpEventSubControl.m_ScriptEvent.LimitTime.fLimitTimeCount);
                }
                else
                {
                    m_CpMenuPaintControl.SetActiveLimitTime(flag: false);
                }
                m_cpEventSubControl.m_NextMapData.Flag = 1;
                m_cpEventSubControl.m_NextMapData.Map_Number = cHAPTER_DATA.lMapNumber;
                m_cpEventSubControl.m_NextMapData.X = cHAPTER_DATA.cPos[0];
                m_cpEventSubControl.m_NextMapData.Y = cHAPTER_DATA.cPos[1];
                m_cpEventSubControl.m_NextMapData.dir = cHAPTER_DATA.lCharaDirection;
                m_cpEventSubControl.m_Party_Set.lType = cHAPTER_DATA.cPtIndex;
                m_cpEventSubControl.m_Party_Set.lIndex[0] = cHAPTER_DATA.Patery[0];
                m_cpEventSubControl.m_Party_Set.lIndex[1] = cHAPTER_DATA.Patery[1];
                m_cpEventSubControl.m_Party_Set.lIndex[2] = cHAPTER_DATA.Patery[2];
                for (int k = 0; k < 4; k++)
                {
                    m_cpEventSubControl.m_Party_Set.cHp[k] = cHAPTER_DATA.cHp[k];
                }
                for (int l = 0; l < 3; l++)
                {
                    m_cpEventSubControl.m_Party_Set.Name[l] = m_cpEventSubControl.m_CpPlayerSettingList.cpPlayerSetting[0].Name;
                    m_cpEventSubControl.m_Party_Set.File[l] = m_cpEventSubControl.m_CpPlayerSettingList.cpPlayerSetting[0].PtFace;
                    for (int m = 0; m < m_cpEventSubControl.m_CpPlayerSettingList.cpPlayerSetting.Count; m++)
                    {
                        if (m_cpEventSubControl.m_CpPlayerSettingList.cpPlayerSetting[m].index == m_cpEventSubControl.m_Party_Set.lIndex[l])
                        {
                            m_cpEventSubControl.m_Party_Set.Name[l] = m_cpEventSubControl.m_CpPlayerSettingList.cpPlayerSetting[m].Name;
                            m_cpEventSubControl.m_Party_Set.File[l] = m_cpEventSubControl.m_CpPlayerSettingList.cpPlayerSetting[m].PtFace;
                            break;
                        }
                    }
                }
                m_CpMenuPaintControl.SetPtPaint(m_cpEventSubControl.m_Party_Set);
                m_cpEventSubControl.messege_log.lIndex = 0;
                for (int n = 0; n < 127; n++)
                {
                    m_cpEventSubControl.messege_log.lList[n] = cHAPTER_DATA.MessageBackLog.lList[n];
                }
                m_cpEventSubControl.messege_log.lIndex = cHAPTER_DATA.MessageBackLog.lIndex;
                int num4 = 0;
                for (int num5 = 10; num5 < 64; num5++)
                {
                    num4 = num5 / 10;
                    if (cHAPTER_DATA.lItemFlag[num5] != 0)
                    {
                        m_cpEventSubControl.SetItemData(cHAPTER_DATA.lItemFlag[num5], PaintFlag: false, num4);
                    }
                }
                for (int num6 = 0; num6 < 10; num6++)
                {
                    num4 = num6 / 10;
                    if (cHAPTER_DATA.lItemFlag[num6] != 0)
                    {
                        m_cpEventSubControl.SetItemData(cHAPTER_DATA.lItemFlag[num6], PaintFlag: false, num4);
                    }
                }
                m_cpEventSubControl.CkItemFlag();
                m_CpMenuPaintControl.SetItem(m_cpEventSubControl);
                m_cpEventSubControl.m_ScriptEvent.PtMenber.Alive = cHAPTER_DATA.PtMenber.Alive;
                m_cpEventSubControl.m_ScriptEvent.PtMenber.NameIndex = cHAPTER_DATA.PtMenber.NameIndex;
                m_cpEventSubControl.m_ScriptEvent.PtMenber.PosX = cHAPTER_DATA.PtMenber.PosX;
                m_cpEventSubControl.m_ScriptEvent.PtMenber.PosY = cHAPTER_DATA.PtMenber.PosY;
                m_cpEventSubControl.m_ScriptEvent.PtMenber.Step.AnimeFrameCount = cHAPTER_DATA.PtMenber.Step.AnimeFrameCount;
                m_cpEventSubControl.m_ScriptEvent.PtMenber.Step.AnimeFrameMax = cHAPTER_DATA.PtMenber.Step.AnimeFrameMax;
                m_cpEventSubControl.m_ScriptEvent.PtMenber.Step.AnimeNumber = cHAPTER_DATA.PtMenber.Step.AnimeNumber;
                m_cpEventSubControl.m_ScriptEvent.PtMenber.Step.AnimeCount = cHAPTER_DATA.PtMenber.Step.AnimeCount;
                m_cpEventSubControl.m_ScriptEvent.PtMenber.Step.FootType = cHAPTER_DATA.PtMenber.Step.FootType;
                m_cpEventSubControl.m_ScriptEvent.PtMenber.Step.IsDirection = cHAPTER_DATA.PtMenber.Step.IsDirection;
                m_cpEventSubControl.m_ScriptEvent.PtMenber.TranslateOneFrameX[0] = cHAPTER_DATA.PtMenber.TranslateOneFrameX[0];
                m_cpEventSubControl.m_ScriptEvent.PtMenber.TranslateOneFrameX[1] = cHAPTER_DATA.PtMenber.TranslateOneFrameX[1];
                m_cpEventSubControl.m_ScriptEvent.PtMenber.TranslateOneFrameY[0] = cHAPTER_DATA.PtMenber.TranslateOneFrameY[1];
                m_cpEventSubControl.m_ScriptEvent.PtMenber.TranslateOneFrameY[1] = cHAPTER_DATA.PtMenber.TranslateOneFrameY[1];
                m_cpEventSubControl.m_ScriptEvent.PtMenber.StokAnimeNumber[0] = cHAPTER_DATA.PtMenber.StokAnimeNumber[0];
                m_cpEventSubControl.m_ScriptEvent.PtMenber.StokAnimeNumber[1] = cHAPTER_DATA.PtMenber.StokAnimeNumber[1];
                m_cpEventSubControl.m_ScriptEvent.PtMenber.bMapChangeFlag = cHAPTER_DATA.PtMenber.bMapChangeFlag;
                m_cpEventSubControl.m_ScriptEvent.Chaser.Alive = cHAPTER_DATA.ChaserData.Alive;
                m_cpEventSubControl.m_ScriptEvent.Chaser.NameIndex = cHAPTER_DATA.ChaserData.NameIndex;
                m_cpEventSubControl.m_ScriptEvent.Chaser.PosX = cHAPTER_DATA.ChaserData.PosX;
                m_cpEventSubControl.m_ScriptEvent.Chaser.PosY = cHAPTER_DATA.ChaserData.PosY;
                m_cpEventSubControl.m_ScriptEvent.Chaser.Step.AnimeFrameCount = cHAPTER_DATA.ChaserData.Step.AnimeFrameCount;
                m_cpEventSubControl.m_ScriptEvent.Chaser.Step.AnimeFrameMax = cHAPTER_DATA.ChaserData.Step.AnimeFrameMax;
                m_cpEventSubControl.m_ScriptEvent.Chaser.Step.AnimeNumber = cHAPTER_DATA.ChaserData.Step.AnimeNumber;
                m_cpEventSubControl.m_ScriptEvent.Chaser.Step.AnimeCount = cHAPTER_DATA.ChaserData.Step.AnimeCount;
                m_cpEventSubControl.m_ScriptEvent.Chaser.Step.FootType = cHAPTER_DATA.ChaserData.Step.FootType;
                m_cpEventSubControl.m_ScriptEvent.Chaser.Step.IsDirection = cHAPTER_DATA.ChaserData.Step.IsDirection;
                m_cpEventSubControl.m_ScriptEvent.Chaser.nWarpCount = cHAPTER_DATA.ChaserData.nWarpCount;
                m_cpEventSubControl.m_ScriptEvent.Chaser.cWalkFlag = cHAPTER_DATA.ChaserData.cWalkFlag;
                m_cpEventSubControl.m_ScriptEvent.Chaser.cWalkWait = cHAPTER_DATA.ChaserData.cWalkWait;
                m_cpEventSubControl.m_ScriptEvent.Chaser.TranslateOneFrameX = cHAPTER_DATA.ChaserData.TranslateOneFrameX;
                m_cpEventSubControl.m_ScriptEvent.Chaser.TranslateOneFrameY = cHAPTER_DATA.ChaserData.TranslateOneFrameY;
                m_cpEventSubControl.m_ZappingData.bUsed = true;
                m_cpEventSubControl.m_ZappingData.Map_Number = cHAPTER_DATA.Zapping.cMapNumber;
                if (m_cpEventSubControl.m_ZappingData.Map_Number == 255)
                {
                    m_cpEventSubControl.m_ZappingData.bUsed = false;
                }
                m_cpEventSubControl.m_ZappingData.X = cHAPTER_DATA.Zapping.cX;
                m_cpEventSubControl.m_ZappingData.Y = cHAPTER_DATA.Zapping.cY;
                m_cpEventSubControl.m_ZappingData.dir = cHAPTER_DATA.Zapping.cDir;
                m_cpEventSubControl.m_ZappingData.Character_Index = cHAPTER_DATA.Zapping.nCharacterIndex;
                m_cpEventSubControl.m_ZappingData.Pt_Index[0] = cHAPTER_DATA.Zapping.nPt_Index[0];
                m_cpEventSubControl.m_ZappingData.Pt_Index[1] = cHAPTER_DATA.Zapping.nPt_Index[1];
                m_cpEventSubControl.m_ZappingData.Pt_Index[2] = cHAPTER_DATA.Zapping.nPt_Index[2];
                m_cpEventSubControl.m_ZappingData.PtNumber = cHAPTER_DATA.Zapping.cPtNumber;
                NextMyPhase = PHASE._EVENT_NEXT_MAP;
                break;
            }
            case PHASE._MAP_RESTART:
                Falset_MAP_RESTART_flag = 1;
                MyDebugLog.Log_Time("MapControl PHASE._MAP_RESTART", 5);
                if (m_bEventMapChange && !m_bLoadFlag)
                {
                    EventInFunction();
                    NextMyPhase = PHASE._EVENT_RUN;
                }
                else
                {
                    int num = m_cpEventSubControl.EventList_Check(-1, -1, 0, 0);
                    MyDebugLog.Log_ColorType(" eventno = " + num, 6);
                    if (num >= 0)
                    {
                        EventInFunction();
                        NextMyPhase = PHASE._EVENT_RUN;
                        GlobalVariable.m_InfomationWindowControl.SetInfoNumber(10);
                    }
                    else
                    {
                        EventEndFunction();
                        NextMyPhase = PHASE._KEY_INPUT;
                        GlobalVariable.m_InfomationWindowControl.SetInfoNumber(1);
                        MyDebugLog.Log_ColorType("m_ScriptEvent._MAP_RESTART NextMyPhase = PHASE._KEY_INPUT ", 1);
                    }
                }
                m_bLoadFlag = false;
                if (GlobalVariable.m_backUpDataLoadFlag != 0)
                {
                    GlobalVariable.m_backUpDataLoadFlag = 0;
                    NextMyPhase = PHASE._BACKUP_LOAD;
                }
                MapChangeButtonReTouchFlag = 0;
                break;
            case PHASE._KEY_INPUT:
            {
                if (m_lEventCoolCount > 0)
                {
                    m_lEventCoolCount--;
                }
                if (m_lAButtonEventCoolCount > 0)
                {
                    m_lAButtonEventCoolCount--;
                }
                if (EventObjMoveCtl(1) != 0 && m_lEventCoolCount <= 0 && EventCkCoolTime() > 0)
                {
                    EventInFunction();
                    NextMyPhase = PHASE._EVENT_RUN;
                    m_lEventCoolCount = 30;
                    break;
                }
                if (EventCkChaser() > 0)
                {
                    EventInFunction();
                    NextMyPhase = PHASE._EVENT_RUN;
                    break;
                }
                if (EventCkLimitTime() > 0)
                {
                    EventInFunction();
                    NextMyPhase = PHASE._EVENT_RUN;
                    break;
                }
                float num7 = 72f;
                float num8 = 0f - m_cpEventSubControl.m_MapTranslate.x;
                float y2 = m_cpEventSubControl.m_MapTranslate.y;
                int num9 = (int)(num8 / num7);
                int num10 = (int)(y2 / num7);
                int num11 = 0;
                if (MapChangeButtonReTouchFlag == 0 || MapChangeButtonReTouchType == 0)
                {
                    if (PadInputCTRL.Get_Button2(padNumber, 16) >= 1 && PadInputCTRL.Get_Button2(padNumber, 16) < 3)
                    {
                        int num12 = m_cpEventSubControl.GetNextVectr(0, num9, num10, 1, 1);
                        if (m_cpEventSubControl.m_localData.m_lMapNumber == 32 && (m_Chapter + 1 == 5 || m_Chapter + 1 == 20))
                        {
                            int num13 = num9;
                            int num14 = num10 - 1;
                            if (num13 >= 59 && num13 <= 61 && (num14 == 14 || num14 == 18 || num14 == 22))
                            {
                                num12 = 1;
                            }
                            if (num13 >= 63 && num13 <= 65 && (num14 == 14 || num14 == 18 || num14 == 22))
                            {
                                num12 = 1;
                            }
                            if (num13 >= 67 && num13 <= 69 && (num14 == 14 || num14 == 18 || num14 == 22))
                            {
                                num12 = 1;
                            }
                            if (num13 >= 71 && num13 <= 73 && (num14 == 14 || num14 == 18 || num14 == 22))
                            {
                                num12 = 1;
                            }
                        }
                        if (num11 != 0 || num12 != 1)
                        {
                            m_cpEventSubControl.StepMove(eCharaDirection.DIRECTION_UP, eCharaStep.CHARASTEP_SLOW, IsDirection: true, (eMapChipAttr)num12);
                            m_cpEventSubControl.Event(EventCommandMap.EVENT_MOVE_ONE);
                            NextMyPhase = PHASE._INPUT_MOVE;
                            m_lPhaseCount = 11;
                            MapChangeButtonReTouchType = 16;
                        }
                        else if (m_cpEventSubControl.m_localData.m_lCharaDirection != 1)
                        {
                            m_cpEventSubControl.SetVector(eCharaDirection.DIRECTION_UP);
                            m_cpEventSubControl.m_ScriptEvent.bMoveRunFlag = true;
                            NextMyPhase = PHASE._EVENT_CK;
                        }
                    }
                    else if (PadInputCTRL.Get_Button2(padNumber, 19) >= 1 && PadInputCTRL.Get_Button2(padNumber, 19) < 3)
                    {
                        int num15 = m_cpEventSubControl.GetNextVectr(0, num9, num10, 3, 1);
                        if (m_cpEventSubControl.m_localData.m_lMapNumber == 32 && (m_Chapter + 1 == 5 || m_Chapter + 1 == 20))
                        {
                            int num16 = num9 + 1;
                            int num17 = num10;
                            if (num16 >= 59 && num16 <= 61 && (num17 == 14 || num17 == 18 || num17 == 22))
                            {
                                num15 = 1;
                            }
                            if (num16 >= 63 && num16 <= 65 && (num17 == 14 || num17 == 18 || num17 == 22))
                            {
                                num15 = 1;
                            }
                            if (num16 >= 67 && num16 <= 69 && (num17 == 14 || num17 == 18 || num17 == 22))
                            {
                                num15 = 1;
                            }
                            if (num16 >= 71 && num16 <= 73 && (num17 == 14 || num17 == 18 || num17 == 22))
                            {
                                num15 = 1;
                            }
                        }
                        if (num11 != 0 || num15 != 1)
                        {
                            m_cpEventSubControl.StepMove(eCharaDirection.DIRECTION_RIGHT, eCharaStep.CHARASTEP_SLOW, IsDirection: true, (eMapChipAttr)num15);
                            m_cpEventSubControl.Event(EventCommandMap.EVENT_MOVE_ONE);
                            NextMyPhase = PHASE._INPUT_MOVE;
                            m_lPhaseCount = 11;
                            MapChangeButtonReTouchType = 19;
                        }
                        else if (m_cpEventSubControl.m_localData.m_lCharaDirection != 3)
                        {
                            m_cpEventSubControl.SetVector(eCharaDirection.DIRECTION_RIGHT);
                            m_cpEventSubControl.m_ScriptEvent.bMoveRunFlag = true;
                            NextMyPhase = PHASE._EVENT_CK;
                        }
                    }
                    else if (PadInputCTRL.Get_Button2(padNumber, 17) >= 1 && PadInputCTRL.Get_Button2(padNumber, 17) < 3)
                    {
                        int num18 = m_cpEventSubControl.GetNextVectr(0, num9, num10, 2, 1);
                        if (m_cpEventSubControl.m_localData.m_lMapNumber == 32 && (m_Chapter + 1 == 5 || m_Chapter + 1 == 20))
                        {
                            int num19 = num9;
                            int num20 = num10 + 1;
                            if (num19 >= 59 && num19 <= 61 && (num20 == 14 || num20 == 18 || num20 == 22))
                            {
                                num18 = 1;
                            }
                            if (num19 >= 63 && num19 <= 65 && (num20 == 14 || num20 == 18 || num20 == 22))
                            {
                                num18 = 1;
                            }
                            if (num19 >= 67 && num19 <= 69 && (num20 == 14 || num20 == 18 || num20 == 22))
                            {
                                num18 = 1;
                            }
                            if (num19 >= 71 && num19 <= 73 && (num20 == 14 || num20 == 18 || num20 == 22))
                            {
                                num18 = 1;
                            }
                        }
                        if (num11 != 0 || num18 != 1)
                        {
                            m_cpEventSubControl.StepMove(eCharaDirection.DIRECTION_DOWN, eCharaStep.CHARASTEP_SLOW, IsDirection: true, (eMapChipAttr)num18);
                            m_cpEventSubControl.Event(EventCommandMap.EVENT_MOVE_ONE);
                            NextMyPhase = PHASE._INPUT_MOVE;
                            m_lPhaseCount = 11;
                            MapChangeButtonReTouchType = 17;
                        }
                        else if (m_cpEventSubControl.m_localData.m_lCharaDirection != 2)
                        {
                            m_cpEventSubControl.SetVector(eCharaDirection.DIRECTION_DOWN);
                            m_cpEventSubControl.m_ScriptEvent.bMoveRunFlag = true;
                            NextMyPhase = PHASE._EVENT_CK;
                        }
                    }
                    else if (PadInputCTRL.Get_Button2(padNumber, 18) >= 1 && PadInputCTRL.Get_Button2(padNumber, 18) < 3)
                    {
                        int num21 = m_cpEventSubControl.GetNextVectr(0, num9, num10, 4, 1);
                        if (m_cpEventSubControl.m_localData.m_lMapNumber == 32 && (m_Chapter + 1 == 5 || m_Chapter + 1 == 20))
                        {
                            int num22 = num9 - 1;
                            int num23 = num10;
                            if (num22 >= 59 && num22 <= 61 && (num23 == 14 || num23 == 18 || num23 == 22))
                            {
                                num21 = 1;
                            }
                            if (num22 >= 63 && num22 <= 65 && (num23 == 14 || num23 == 18 || num23 == 22))
                            {
                                num21 = 1;
                            }
                            if (num22 >= 67 && num22 <= 69 && (num23 == 14 || num23 == 18 || num23 == 22))
                            {
                                num21 = 1;
                            }
                            if (num22 >= 71 && num22 <= 73 && (num23 == 14 || num23 == 18 || num23 == 22))
                            {
                                num21 = 1;
                            }
                        }
                        if (num11 != 0 || num21 != 1)
                        {
                            m_cpEventSubControl.StepMove(eCharaDirection.DIRECTION_LEFT, eCharaStep.CHARASTEP_SLOW, IsDirection: true, (eMapChipAttr)num21);
                            m_cpEventSubControl.Event(EventCommandMap.EVENT_MOVE_ONE);
                            NextMyPhase = PHASE._INPUT_MOVE;
                            m_lPhaseCount = 11;
                            MapChangeButtonReTouchType = 18;
                        }
                        else if (m_cpEventSubControl.m_localData.m_lCharaDirection != 4)
                        {
                            m_cpEventSubControl.SetVector(eCharaDirection.DIRECTION_LEFT);
                            m_cpEventSubControl.m_ScriptEvent.bMoveRunFlag = true;
                            NextMyPhase = PHASE._EVENT_CK;
                        }
                    }
                }
                if (NextMyPhase == PHASE._KEY_INPUT && m_lAButtonEventCoolCount <= 0 && PadInputCTRL.Get_Button(padNumber, 2) == 1)
                {
                    int num24 = m_cpEventSubControl.EventList_Check(num9, num10, m_cpEventSubControl.m_localData.m_lCharaDirection, 1);
                    if (num24 > 0)
                    {
                        MyDebugLog.Log_ColorType("BUTTON_RKEY_R  i = " + num24, 1);
                        EventInFunction();
                        NextMyPhase = PHASE._EVENT_RUN;
                        m_lAButtonEventCoolCount = 32;
                    }
                }
                if (NextMyPhase == PHASE._KEY_INPUT && PadInputCTRL.Get_Button(padNumber, 3) == 1)
                {
                    NextMyPhase = PHASE._PAUSE_MENU_I;
                    GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(457);
                }
                if (NextMyPhase != PHASE._KEY_INPUT)
                {
                }
                break;
            }
            case PHASE._INPUT_MOVE:
                do
                {
                    m_cpEventSubControl.Event(EventCommandMap.EVENT_MOVE_ONE);
                    m_cpEventSubControl.Event(EventCommandMap.EVENT_MOVE_STEP);
                    EventObjMoveCtl(1);
                    m_lPhaseCount--;
                    if (m_lPhaseCount <= 0)
                    {
                        m_cpEventSubControl.Event(EventCommandMap.EVENT_POINT_BGM_VOLUME);
                        if (EventCkMoveEnd() > 0)
                        {
                            EventInFunction();
                            NextMyPhase = PHASE._EVENT_RUN;
                        }
                        else
                        {
                            EventEndFunction();
                            NextMyPhase = PHASE._KEY_INPUT;
                            GlobalVariable.m_InfomationWindowControl.SetInfoNumber(1);
                        }
                        break;
                    }
                }
                while (GlobalVariable.SpeedUpLevel != 0);
                break;
            case PHASE._EVENT_MOVE:
                m_cpEventSubControl.Event(EventCommandMap.EVENT_EVENT_MOVE);
                m_cpEventSubControl.Event(EventCommandMap.EVENT_POS_SET);
                if (!m_cpEventSubControl.EventMoveObj.Flag)
                {
                    EventInFunction();
                    NextMyPhase = PHASE._EVENT_RUN;
                }
                EventObjMoveCtl(0);
                break;
            case PHASE._EVENT_CK:
            {
                MyDebugLog.Log_ColorType("m_ScriptEvent._EVENT_EVENT_CK m_nFrameCount = " + m_nFrameCount, 1);
                if (m_bEventMapChange)
                {
                    m_cpEventControl.ImportEventFlag(m_cpOldEventControl.ExportEventFlag());
                    m_bEventMapChange = false;
                }
                int num = -1;
                if (m_cpEventSubControl.m_ScriptEvent.bMoveRunFlag)
                {
                    float num2 = 72f;
                    float num3 = 0f - m_cpEventSubControl.m_MapTranslate.x;
                    float y = m_cpEventSubControl.m_MapTranslate.y;
                    int xpos = (int)(num3 / num2);
                    int ypos = (int)(y / num2);
                    num = m_cpEventSubControl.EventList_Check(xpos, ypos, m_cpEventSubControl.m_localData.m_lCharaDirection, 0, m_lOldEventNumber);
                    if (num < 0)
                    {
                        m_cpEventSubControl.m_ScriptEvent.bMoveRunFlag = false;
                        m_lOldEventNumber = 0;
                    }
                    else
                    {
                        m_lOldEventNumber = num;
                    }
                }
                if (num < 0)
                {
                    num = m_cpEventSubControl.EventList_Check(-1, -1, m_cpEventSubControl.m_localData.m_lCharaDirection, 0);
                    MyDebugLog.Log_ColorType(" eventno = " + num, 6);
                }
                if (num >= 0)
                {
                    MyDebugLog.Log_ColorType(" eventno = " + num, 6);
                    EventInFunction();
                    NextMyPhase = PHASE._EVENT_RUN;
                }
                else
                {
                    EventEndFunction();
                    NextMyPhase = PHASE._KEY_INPUT;
                    GlobalVariable.m_InfomationWindowControl.SetInfoNumber(1);
                    MyDebugLog.Log_ColorType("m_ScriptEvent._EVENT_EVENT_CK NextMyPhase = PHASE._KEY_INPUT ", 1);
                }
                EventObjMoveCtl(0);
                m_cpEventSubControl.Event_CkAlilve();
                break;
            }
            case PHASE._EVENT_RUN:
            {
                if (m_cpEventSubControl.m_ScriptEvent.lWaitCount > 0)
                {
                    m_cpEventSubControl.m_ScriptEvent.lWaitCount--;
                    if (GlobalVariable.SkipMode != 0)
                    {
                        m_cpEventSubControl.m_ScriptEvent.lWaitCount = 0;
                    }
                    if (m_cpEventSubControl.m_ScriptEvent.lWaitCount > 0)
                    {
                        break;
                    }
                    if (m_cpEventSubControl.m_ScriptEvent.bOldFlag)
                    {
                        m_cpEventSubControl.m_ScriptEvent.lReturnStokOld = 3;
                    }
                    else
                    {
                        m_cpEventSubControl.m_ScriptEvent.lReturnStok = 3;
                    }
                }
                MyDebugLog.Log_ColorType("m_ScriptEvent._EVENT_EVENT_RUN m_nFrameCount = " + m_nFrameCount, 1);
                float num25 = 72f;
                float num26 = 0f - m_cpEventSubControl.m_MapTranslate.x;
                float y3 = m_cpEventSubControl.m_MapTranslate.y;
                int xpos2 = (int)(num26 / num25);
                int ypos2 = (int)(y3 / num25);
                int lCharaDirection = m_cpEventSubControl.m_localData.m_lCharaDirection;
                int num27 = 0;
                int num28 = 0;
                if (m_cpEventSubControl.m_localData.ButtonWaitNumber == 1)
                {
                    m_cpEventSubControl.m_localData.ButtonWaitNumber = 2;
                    GlobalVariable.m_InfomationWindowControl.SetInfoNumber(13);
                }
                if (PadInputCTRL.Get_Button2(padNumber, 2) == 1)
                {
                    num27 = 1;
                }
                else if (PadInputCTRL.Get_Button2(padNumber, 2) > 1)
                {
                    num27 = 2;
                }
                else if (PadInputCTRL.Get_Button2(padNumber, 1) == 1)
                {
                    num27 = 3;
                }
                else if (PadInputCTRL.Get_Button2(padNumber, 1) > 1)
                {
                    num27 = 4;
                }
                if (num27 != 0 && m_cpEventSubControl.m_localData.ButtonWaitNumber != 0)
                {
                    m_cpEventSubControl.m_localData.ButtonWaitNumber = 0;
                    GlobalVariable.m_InfomationWindowControl.SetInfoNumber(10);
                }
                do
                {
                    m_cpEventSubControl.m_ScriptEvent.bEventChainFlag = false;
                    if (m_bEventMapChange)
                    {
                        num28 = m_cpEventSubControl.EventScript_RunOld(xpos2, ypos2, lCharaDirection, num27);
                        MyDebugLog.Log_ColorType(" res2 = " + num28 + " sx = " + xpos2 + " sy = " + ypos2 + " dir = " + lCharaDirection + " lIndexOld = " + m_cpEventSubControl.m_ScriptEvent.lIndexOld + " lReturnStokOld = " + m_cpEventSubControl.m_ScriptEvent.lReturnStokOld + " m_lEventData = " + m_cpEventSubControl.m_localData.m_lEventData, 1);
                    }
                    else
                    {
                        num28 = m_cpEventSubControl.EventScript_Run(xpos2, ypos2, lCharaDirection, num27);
                    }
                }
                while (num28 >= 0 && m_cpEventSubControl.m_ScriptEvent.bEventChainFlag);
                if (num28 < 0)
                {
                    if (m_cpEventSubControl.m_localData.m_bMessageWindowPaintFlag)
                    {
                        NextMyPhase = PHASE._EVENT_MESSAGE_FADE_OUT;
                        break;
                    }
                    if (m_cpEventSubControl.m_NextMapData.Flag == 1)
                    {
                        NextMyPhase = PHASE._EVENT_NEXT_MAP;
                        break;
                    }
                    if (m_cpEventSubControl.m_bEventMapChange)
                    {
                        m_cpEventControl.ImportEventFlag(m_cpOldEventControl.ExportEventFlag());
                    }
                    NextMyPhase = PHASE._EVENT_CK;
                }
                else if (m_cpEventSubControl.m_NextMapData.Flag == 1)
                {
                    if (m_cpEventSubControl.m_localData.m_bMessageWindowPaintFlag)
                    {
                        NextMyPhase = PHASE._EVENT_MESSAGE_FADE_OUT;
                    }
                    else
                    {
                        NextMyPhase = PHASE._EVENT_NEXT_MAP;
                    }
                }
                else if (m_cpEventSubControl.m_localData.m_bSelectFlag)
                {
                    m_TalkWindowControl.SetCpEventSelectData(m_cpEventSubControl.m_CpEventMessageData);
                    MessageWindowPaintFlag = 1;
                    NextMyPhase = PHASE._EVENT_SELECT_PAINT;
                    GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(463);
                    GlobalVariable.m_InfomationWindowControl.SetInfoNumber(3);
                }
                else if (m_cpEventSubControl.m_localData.m_bEventMessage)
                {
                    m_TalkWindowControl.SetCpEventMessageData(m_cpEventSubControl.m_CpEventMessageData);
                    m_cpEventSubControl.m_localData.m_bMessageWindowPaintFlag = true;
                    MessageWindowPaintFlag = 1;
                    NextMyPhase = PHASE._EVENT_MESSAGE_PAINT;
                    GlobalVariable.m_InfomationWindowControl.SetInfoNumber(2);
                }
                else if (m_cpEventSubControl.EventMoveObj.Flag)
                {
                    NextMyPhase = PHASE._EVENT_MOVE;
                }
                else if (m_cpEventSubControl.m_ScriptEvent.Effect.Fade.Used)
                {
                    NextMyPhase = PHASE._EVENT_EFFECT_FADE;
                }
                break;
            }
            case PHASE._EVENT_NEXT_MAP:
                MyDebugLog.Log_ColorType("PHASE._EVENT_NEXT_MAP", 6);
                Resources.UnloadUnusedAssets();
                GC.Collect();
                m_cpEventSubControl.m_localData.m_lMapNumber = m_cpEventSubControl.m_NextMapData.Map_Number;
                m_CpMapPaintControl.LoadMap(m_cpEventSubControl.m_localData.m_lMapNumber);
                m_cpEventDataControl.LoadDataMapAttribute(m_Chapter, m_cpEventSubControl.m_localData.m_lMapNumber);
                m_cpEventControl.SetMapAttributeArr(m_cpEventDataControl.mapAttribute);
                m_CpMapPaintControl.SetMapPosition(m_cpEventSubControl.m_NextMapData.X, m_cpEventSubControl.m_NextMapData.Y);
                m_bEventMapChange = true;
                m_cpEventSubControl.m_ScriptEvent.bMoveRunFlag = false;
                if (!m_bLoadFlag)
                {
                    m_cpEventSubControl.m_ScriptEvent.lLightNumber = 0;
                }
                LoadData(m_Chapter);
                NextMyPhase = PHASE._MAP_RESTART;
                break;
            case PHASE._EVENT_MESSAGE_PAINT:
                if (PadInputCTRL.Get_Button(padNumber, 4) == 1)
                {
                    if (MessageWindowPaintFlag != 0)
                    {
                        MessageWindowPaintFlag = 0;
                        m_TalkWindowControl.MessageWindowActiveFlag(flag: false);
                    }
                    else
                    {
                        MessageWindowPaintFlag = 1;
                        m_TalkWindowControl.MessageWindowActiveFlag(flag: true);
                    }
                }
                if (MessageWindowPaintFlag == 0 || (PadInputCTRL.Get_Button(padNumber, 2) != 1 && GlobalVariable.SkipMode == 0 && m_TalkWindowControl.GetAutoNextPage() == 0) || m_TalkWindowControl.GetSelectWindowPaintFlag() != 0)
                {
                    break;
                }
                if (m_TalkWindowControl.IsMessageAllPaint())
                {
                    if (m_TalkWindowControl.GetExtraVoiceWaitFlag() == 0)
                    {
                        m_TalkWindowControl.MessageEndFunc();
                        m_cpEventSubControl.m_localData.m_bEventMessage = false;
                        EventInFunction();
                        NextMyPhase = PHASE._EVENT_RUN;
                        GlobalVariable.m_InfomationWindowControl.SetInfoNumber(10);
                    }
                }
                else
                {
                    m_TalkWindowControl.AllPaintMessage();
                    if (GlobalVariable.SkipMode == 2 && message_skip_flag < 1024)
                    {
                        message_skip_flag++;
                    }
                }
                break;
            case PHASE._EVENT_SELECT_PAINT:
                if (m_TalkWindowControl.GetSelectWindowPaintFlag() == 1)
                {
                    if (PadInputCTRL.Get_Button(padNumber, 4) == 1)
                    {
                        GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(457);
                        if (MessageWindowPaintFlag != 0)
                        {
                            MessageWindowPaintFlag = 0;
                            m_TalkWindowControl.MessageWindowActiveFlag(flag: false);
                            m_TalkWindowControl.SelectWindowActiveFlag(flag: false);
                        }
                        else
                        {
                            MessageWindowPaintFlag = 1;
                            m_TalkWindowControl.MessageWindowActiveFlag(flag: true);
                            m_TalkWindowControl.SelectWindowActiveFlag(flag: true);
                            m_TalkWindowControl.ReSelectAnimeChange();
                        }
                    }
                    if (MessageWindowPaintFlag != 0)
                    {
                        if (PadInputCTRL.Get_ButtonRepeat2(padNumber, 16) == 1)
                        {
                            m_TalkWindowControl.SelectAnimeChange(0);
                        }
                        else if (PadInputCTRL.Get_ButtonRepeat2(padNumber, 17) == 1)
                        {
                            m_TalkWindowControl.SelectAnimeChange(1);
                        }
                        else if (PadInputCTRL.Get_Button(padNumber, 2) == 1)
                        {
                            m_TalkWindowControl.SelectAnimeChangeOn();
                            GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(464);
                        }
                    }
                }
                else if (m_TalkWindowControl.GetSelectWindowPaintFlag() != 2 && m_TalkWindowControl.GetSelectWindowPaintFlag() == 3)
                {
                    m_TalkWindowControl.SetSelectPaintFlagZero();
                    m_cpEventSubControl.m_localData.m_bEventMessage = false;
                    m_cpEventSubControl.m_localData.m_bSelectFlag = false;
                    m_cpEventSubControl.m_localData.m_lEventData = m_TalkWindowControl.GetSelectNumber();
                    EventInFunction();
                    GlobalVariable.m_InfomationWindowControl.SetInfoNumber(10);
                    m_TalkWindowControl.HideMessageWindow();
                    m_cpEventSubControl.m_localData.m_bMessageWindowPaintFlag = false;
                    if (m_CpBustUpPaintControl != null)
                    {
                        m_CpBustUpPaintControl.FadeOutBustUp(0, nodestroy: true);
                        m_CpBustUpPaintControl.FadeOutBustUp(1, nodestroy: true);
                    }
                    if (m_CpFacePaintControl != null)
                    {
                        m_CpFacePaintControl.FadeOutFace(nodestroy: true);
                    }
                    m_cpEventSubControl.m_Face.lNumber = 0;
                    m_cpEventSubControl.m_BastoUp[0].lNumber = 0;
                    m_cpEventSubControl.m_BastoUp[1].lNumber = 0;
                    MyDebugLog.Log_ColorType("PHASE._EVENT_SELECT_PAINT m_cpEventSubControl.m_localData.m_lEventData = " + m_cpEventSubControl.m_localData.m_lEventData, 6);
                    NextMyPhase = PHASE._EVENT_RUN;
                }
                break;
            case PHASE._EVENT_MESSAGE_FADE_OUT:
                m_TalkWindowControl.HideMessageWindow();
                m_cpEventSubControl.m_localData.m_bMessageWindowPaintFlag = false;
                GlobalVariable.m_SoundPlayerCTRL.Audio_StopVoice();
                if (m_CpBustUpPaintControl != null)
                {
                    m_CpBustUpPaintControl.FadeOutBustUp(0, nodestroy: true);
                    m_CpBustUpPaintControl.FadeOutBustUp(1, nodestroy: true);
                }
                if (m_CpFacePaintControl != null)
                {
                    m_CpFacePaintControl.FadeOutFace(nodestroy: true);
                }
                m_cpEventSubControl.m_Face.lNumber = 0;
                m_cpEventSubControl.m_BastoUp[0].lNumber = 0;
                m_cpEventSubControl.m_BastoUp[1].lNumber = 0;
                if (m_cpEventSubControl.m_NextMapData.Flag == 1)
                {
                    NextMyPhase = PHASE._EVENT_NEXT_MAP;
                }
                else
                {
                    NextMyPhase = PHASE._EVENT_CK;
                }
                break;
            case PHASE._EVENT_EFFECT_FADE:
                m_cpEventSubControl.Event(EventCommandMap.EVENT_EVENT_FADE);
                if (!m_cpEventSubControl.m_ScriptEvent.Effect.Fade.Used)
                {
                    NextMyPhase = PHASE._EVENT_RUN;
                }
                break;
            case PHASE._PAUSE_MENU_I:
            {
                GlobalVariable.m_InfomationWindowControl.SetInfoNumber(4);
                PauseMenuIndex = 0;
                m_CpMenuPaintControl.SetActivePauseMenu(flag: true);
                m_CpMenuPaintControl.SetActivePauseMenuBg(flag: true);
                m_CpMenuPaintControl.SetPauseMenuMode(PauseMenuIndex, 1);
                m_cpEventSubControl.CkSetActiveDeadNameTagSubPaint(flag: false);
                m_CpMenuPaintControl.CkSetActiveFloorNameMessage(flag: false);
                PauseMenu_ItemMax = 0;
                for (int i = 0; i < 8; i++)
                {
                    if (m_cpEventSubControl.m_ScriptEvent.Item[m_cpEventSubControl.m_Party_Set.lType * 10 + i].lFlagNumber != 0)
                    {
                        PauseMenu_ItemMax++;
                    }
                }
                NextMyPhase = PHASE._PAUSE_MENU_L;
                break;
            }
            case PHASE._PAUSE_MENU_L:
                if (PadInputCTRL.Get_ButtonRepeat2(padNumber, 16) >= 1 && m_CpMenuPaintControl.GetActivePauseMenu())
                {
                    if (PauseMenuIndex > 0)
                    {
                        PauseMenuIndex--;
                        m_CpMenuPaintControl.SetPauseMenuMode(PauseMenuIndex, 0);
                        GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(462);
                    }
                    else
                    {
                        PauseMenuIndex = 5;
                        m_CpMenuPaintControl.SetPauseMenuMode(PauseMenuIndex, 0);
                        GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(462);
                    }
                }
                else if (PadInputCTRL.Get_ButtonRepeat2(padNumber, 17) >= 1 && m_CpMenuPaintControl.GetActivePauseMenu())
                {
                    if (PauseMenuIndex < 5)
                    {
                        PauseMenuIndex++;
                        m_CpMenuPaintControl.SetPauseMenuMode(PauseMenuIndex, 0);
                        GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(462);
                    }
                    else
                    {
                        PauseMenuIndex = 0;
                        m_CpMenuPaintControl.SetPauseMenuMode(PauseMenuIndex, 0);
                        GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(462);
                    }
                }
                if (PadInputCTRL.Get_Button(padNumber, 2) == 1 && m_CpMenuPaintControl.GetActivePauseMenu())
                {
                    switch (PauseMenuIndex)
                    {
                        case 0:
                            GlobalVariable.m_SystemWindowControl.SetSystemMessage(3);
                            NextMyPhase = PHASE._PAUSE_CONTINUE_SAVE_SELECT_WAIT;
                            GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(457);
                            break;
                        case 1:
                            EndSelectMode = 1;
                            GlobalVariable.m_SystemWindowControl.SetSystemMessage(4);
                            GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(457);
                            break;
                        case 2:
                            if (PauseMenu_ItemMax > 0)
                            {
                                NextMyPhase = PHASE._PAUSE_ITEM_PAINT_I;
                                GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(457);
                            }
                            else
                            {
                                GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(456);
                            }
                            break;
                        case 3:
                            NextMyPhase = PHASE._PAUSE_BACKLOG_E;
                            GlobalVariable.m_InfomationWindowControl.SetInfoNumber(1010);
                            m_CpMenuPaintControl.SetActivePauseMenu(flag: false);
                            m_CpMenuPaintControl.SetActivePlayerStatus(flag: false, m_cpEventSubControl.m_Party_Set);
                            m_CpMenuPaintControl.SetActiveItemList(flag: false);
                            GlobalVariable.m_BackLogControl.PaintOn(m_cpEventControl, m_cpEventSubControl.messege_log);
                            GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(457);
                            m_VolumeDataBGMPercentage = GlobalVariable.m_SoundPlayerCTRL.m_VolumeData.BGMPercentage;
                            GlobalVariable.m_SoundPlayerCTRL.Audio_VolumeBGMPercentage(0.5f);
                            break;
                        case 4:
                            NextMyPhase = PHASE._PAUSE_NAMEPAINT_E;
                            GlobalVariable.m_InfomationWindowControl.SetInfoNumber(1020);
                            m_CpMenuPaintControl.SetActivePauseMenu(flag: false);
                            m_CpMenuPaintControl.SetActivePlayerStatus(flag: false, m_cpEventSubControl.m_Party_Set);
                            m_CpMenuPaintControl.SetActiveItemList(flag: false);
                            GlobalVariable.m_CharacterListPaintControl.waikup();
                            GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(457);
                            break;
                        case 5:
                            NextMyPhase = PHASE._PAUSE_CONFIG_E;
                            GlobalVariable.m_InfomationWindowControl.SetInfoNumber(9900);
                            m_CpMenuPaintControl.SetActivePauseMenu(flag: false);
                            m_CpMenuPaintControl.SetActivePlayerStatus(flag: false, m_cpEventSubControl.m_Party_Set);
                            m_CpMenuPaintControl.SetActiveItemList(flag: false);
                            GlobalVariable.m_ConfigControl.Language_Translation();
                            GlobalVariable.m_ConfigControl.waikup();
                            GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(457);
                            break;
                    }
                }
                if (PadInputCTRL.Get_Button(padNumber, 3) == 1 || (PadInputCTRL.Get_Button(padNumber, 1) == 1 && m_CpMenuPaintControl.GetActivePauseMenu()))
                {
                    PauseMenuIndex = -1;
                    NextMyPhase = PHASE._PAUSE_MENU_E;
                    GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(456);
                }
                break;
            case PHASE._PAUSE_MENU_E:
                m_CpMenuPaintControl.SetActivePauseMenu(flag: false);
                m_CpMenuPaintControl.SetActivePauseMenuBg(flag: false);
                m_CpMenuPaintControl.SetActivePlayerStatus(flag: true, m_cpEventSubControl.m_Party_Set);
                m_CpMenuPaintControl.SetActiveItemList(flag: true);
                m_cpEventSubControl.CkSetActiveDeadNameTagSubPaint(flag: true);
                m_CpMenuPaintControl.CkSetActiveFloorNameMessage(flag: true);
                GlobalVariable.m_InfomationWindowControl.SetInfoNumber(1);
                NextMyPhase = PHASE._KEY_INPUT;
                break;
            case PHASE._PAUSE_CONTINUE_SAVE_SELECT_WAIT:
                if (GlobalVariable.m_SystemWindowControl.GetResult() == 1)
                {
                    m_CpMenuPaintControl.SetActivePauseMenu(flag: false);
                    m_CpMenuPaintControl.SetActivePauseMenuBg(flag: false);
                    ExChangeBackupdata();
                    result = -100;
                    NextMyPhase = PHASE._PAUSE_MENU_E;
                    GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(457);
                }
                if (GlobalVariable.m_SystemWindowControl.GetResult() == 2)
                {
                    NextMyPhase = PHASE._PAUSE_MENU_L;
                    GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(456);
                }
                break;
            case PHASE._PAUSE_BACKLOG_E:
                m_CpMenuPaintControl.SetActivePauseMenu(flag: true);
                m_CpMenuPaintControl.SetActivePlayerStatus(flag: true, m_cpEventSubControl.m_Party_Set);
                m_CpMenuPaintControl.SetActiveItemList(flag: true);
                m_CpMenuPaintControl.SetPauseMenuMode(PauseMenuIndex, 1);
                GlobalVariable.m_InfomationWindowControl.SetInfoNumber(4);
                GlobalVariable.m_SoundPlayerCTRL.Audio_VolumeBGMPercentage(m_VolumeDataBGMPercentage);
                NextMyPhase = PHASE._PAUSE_MENU_L;
                break;
            case PHASE._PAUSE_NAMEPAINT_E:
                m_CpMenuPaintControl.SetActivePauseMenu(flag: true);
                m_CpMenuPaintControl.SetActivePlayerStatus(flag: true, m_cpEventSubControl.m_Party_Set);
                m_CpMenuPaintControl.SetActiveItemList(flag: true);
                m_CpMenuPaintControl.SetPauseMenuMode(PauseMenuIndex, 1);
                GlobalVariable.m_InfomationWindowControl.SetInfoNumber(4);
                NextMyPhase = PHASE._PAUSE_MENU_L;
                break;
            case PHASE._PAUSE_CONFIG_E:
                m_CpMenuPaintControl.SetActivePauseMenu(flag: true);
                m_CpMenuPaintControl.SetActivePlayerStatus(flag: true, m_cpEventSubControl.m_Party_Set);
                m_CpMenuPaintControl.SetActiveItemList(flag: true);
                m_CpMenuPaintControl.SetPauseMenuMode(PauseMenuIndex, 1);
                m_CpEffectControl.SetScreenEffect(GlobalVariable.m_SystemBackupData.config_MapScreenEffect);
                if (GlobalVariable.m_SystemBackupData.config_TimePaint == 0)
                {
                    m_CpMenuPaintControl.SetActivePlayTime(flag: true);
                }
                else
                {
                    m_CpMenuPaintControl.SetActivePlayTime(flag: false);
                }
                GlobalVariable.m_InfomationWindowControl.SetInfoNumber(4);
                NextMyPhase = PHASE._PAUSE_MENU_L;
                break;
            case PHASE._PAUSE_ITEM_PAINT_I:
                PauseMenu_ItemIndex = 0;
                PauseMenu_ItemMode = 0;
                if (PauseMenu_ItemMax % 2 != 0)
                {
                    PauseMenu_ItemMode = 1;
                }
                m_CpMenuPaintControl.SetActivePauseMenu(flag: false);
                m_CpMenuPaintControl.SetActiveItemPaint(flag: true);
                m_CpMenuPaintControl.SetItemList(m_cpEventSubControl, PauseMenu_ItemMode, PauseMenu_ItemMax);
                m_CpMenuPaintControl.SetItemListIndex(PauseMenu_ItemIndex, 1, m_cpEventSubControl, PauseMenu_ItemMode, PauseMenu_ItemMax);
                GlobalVariable.m_InfomationWindowControl.SetInfoNumber(8);
                NextMyPhase = PHASE._PAUSE_ITEM_PAINT_L;
                break;
            case PHASE._PAUSE_ITEM_PAINT_L:
                if (PadInputCTRL.Get_Button2(padNumber, 18) == 1)
                {
                    int pauseMenu_ItemIndex = PauseMenu_ItemIndex;
                    if (PauseMenu_ItemIndex > 0)
                    {
                        PauseMenu_ItemIndex--;
                    }
                    else
                    {
                        PauseMenu_ItemIndex = PauseMenu_ItemMax - 1;
                    }
                    if (pauseMenu_ItemIndex != PauseMenu_ItemIndex)
                    {
                        m_CpMenuPaintControl.SetItemListIndex(PauseMenu_ItemIndex, 0, m_cpEventSubControl, PauseMenu_ItemMode, PauseMenu_ItemMax);
                        GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(462);
                    }
                }
                if (PadInputCTRL.Get_Button2(padNumber, 19) == 1)
                {
                    int pauseMenu_ItemIndex2 = PauseMenu_ItemIndex;
                    if (PauseMenu_ItemIndex < PauseMenu_ItemMax - 1)
                    {
                        PauseMenu_ItemIndex++;
                    }
                    else
                    {
                        PauseMenu_ItemIndex = 0;
                    }
                    if (pauseMenu_ItemIndex2 != PauseMenu_ItemIndex)
                    {
                        m_CpMenuPaintControl.SetItemListIndex(PauseMenu_ItemIndex, 0, m_cpEventSubControl, PauseMenu_ItemMode, PauseMenu_ItemMax);
                        GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(462);
                    }
                }
                if (PadInputCTRL.Get_Button(padNumber, 1) == 1)
                {
                    NextMyPhase = PHASE._PAUSE_ITEM_PAINT_E;
                    GlobalVariable.m_SoundPlayerCTRL.Audio_PlaySE(456);
                }
                break;
            case PHASE._PAUSE_ITEM_PAINT_E:
                m_CpMenuPaintControl.SetActiveItemPaint(flag: false);
                m_CpMenuPaintControl.SetActivePauseMenu(flag: true);
                m_CpMenuPaintControl.SetPauseMenuMode(PauseMenuIndex, 1);
                GlobalVariable.m_InfomationWindowControl.SetInfoNumber(4);
                NextMyPhase = PHASE._PAUSE_MENU_L;
                break;
        }
        switch (NowMyPhase)
        {
            case PHASE._BACKUP_LOAD:
            case PHASE._MAP_RESTART:
            case PHASE._KEY_INPUT:
            case PHASE._INPUT_MOVE:
            case PHASE._EVENT_MOVE:
            case PHASE._EVENT_CK:
            case PHASE._EVENT_RUN:
            case PHASE._EVENT_MESSAGE_PAINT:
            case PHASE._EVENT_SELECT_PAINT:
            case PHASE._EVENT_MESSAGE_FADE_OUT:
            case PHASE._PAUSE_MENU_I:
            case PHASE._PAUSE_MENU_L:
            case PHASE._PAUSE_MENU_E:
                m_cpEventSubControl.PlusElapsedTime();
                m_CpMenuPaintControl.SetPlayTime(m_cpEventSubControl.m_localData.fElapsedTime);
                break;
        }
        PHASE nowMyPhase = NowMyPhase;
        if ((uint)(nowMyPhase - 3) <= 1u || (uint)(nowMyPhase - 9) <= 2u)
        {
            m_cpEventSubControl.Event(EventCommandMap.EVENT_LIMIT_TIME_COUNT);
        }
        nowMyPhase = NowMyPhase;
        if ((uint)(nowMyPhase - 3) <= 4u || (uint)(nowMyPhase - 9) <= 2u)
        {
            m_CpMenuPaintControl.SetPlayTime(m_cpEventSubControl.m_localData.fElapsedTime);
            if (m_cpEventSubControl.m_lMapTransCount > 0)
            {
                m_cpEventSubControl.Event(EventCommandMap.EVENT_MAP_TRANS_COUNTDOWN);
            }
            EventObjAnimeCtl();
            m_cpEventSubControl.Event(EventCommandMap.EVENT_EVENT_BMP_PAINT);
            m_cpEventSubControl.Event(EventCommandMap.EVENT_DEAD_NAME);
            if (m_cpEventSubControl.CkItemPaint() && (PadInputCTRL.Get_Button(padNumber, 2) == 1 || GlobalVariable.SkipMode != 0))
            {
                m_cpEventSubControl.SetItemPaintCount();
            }
            m_cpEventSubControl.Event(EventCommandMap.EVENT_ITEM_PAINT);
            if (m_cpEventSubControl.m_ScriptEvent.Effect.Brightness.Used)
            {
                m_cpEventSubControl.Event(EventCommandMap.EVENT_EVENT_BREGHTNESS);
            }
            m_CpCharacterPaintControl.PaneSortPri();
        }
        nowMyPhase = NowMyPhase;
        if ((uint)(nowMyPhase - 3) <= 1u)
        {
            m_cpEventSubControl.Event(EventCommandMap.EVENT_CHASER_MOVE);
            m_cpEventSubControl.Event(EventCommandMap.EVENT_CHASER_FADEIN);
        }
        m_cpEventSubControl.Event(EventCommandMap.EVENT_EVENT_BGM_LOOP_CK);
        EventFunction();
        NowMyPhase = NextMyPhase;
        return result;
    }

    private int EventCk()
    {
        float num = 72f;
        float num2 = 0f - m_cpEventSubControl.m_MapTranslate.x;
        float y = m_cpEventSubControl.m_MapTranslate.y;
        int xpos = (int)(num2 / num);
        int ypos = (int)(y / num);
        int num3 = 0;
        num3 = m_cpEventSubControl.EventList_Check(xpos, ypos, m_cpEventSubControl.m_localData.m_lCharaDirection, 0);
        if (num3 <= 0)
        {
            num3 = m_cpEventSubControl.EventList_Check(-1, -1, m_cpEventSubControl.m_localData.m_lCharaDirection, 0);
        }
        return num3;
    }

    private int EventCkMoveEnd()
    {
        float num = 72f;
        float num2 = 0f - m_cpEventSubControl.m_MapTranslate.x;
        float y = m_cpEventSubControl.m_MapTranslate.y;
        int num3 = (int)(num2 / num);
        int num4 = (int)(y / num);
        int num5 = 0;
        m_cpEventSubControl.CkMoveingPoint(num3, num4);
        num5 = m_cpEventSubControl.EventList_Check(num3, num4, m_cpEventSubControl.m_localData.m_lCharaDirection, 0);
        _ = 0;
        if (num5 > 0)
        {
            m_lOldEventNumber = num5;
            m_cpEventSubControl.m_ScriptEvent.bMoveRunFlag = true;
        }
        m_CpCharacterPaintControl.PaneSortPri();
        return num5;
    }

    private int EventCkCoolTime()
    {
        float num = 72f;
        float num2 = 0f - m_cpEventSubControl.m_MapTranslate.x;
        float y = m_cpEventSubControl.m_MapTranslate.y;
        int xpos = (int)(num2 / num);
        int ypos = (int)(y / num);
        int num3 = 0;
        num3 = m_cpEventSubControl.EventList_Check_Move(xpos, ypos, m_cpEventSubControl.m_localData.m_lCharaDirection, 0);
        if (num3 > 0)
        {
            m_lOldEventNumber = num3;
        }
        return num3;
    }

    private int EventCkChaser()
    {
        int num = 0;
        if (m_lChaseCoolCount > 0)
        {
            m_lChaseCoolCount--;
        }
        if (m_lChaseCoolCount == 0)
        {
            num = m_cpEventSubControl.HitCkChaser(m_bEventMapChange);
            if (num != 0)
            {
                num = m_cpEventSubControl.EventList_Check(-1, -1, m_cpEventSubControl.m_localData.m_lCharaDirection, 0);
                if (num > 0)
                {
                    m_lChaseCoolCount = 30;
                }
            }
        }
        return num;
    }

    private int EventCkLimitTime()
    {
        int result = 0;
        if (m_cpEventSubControl.m_ScriptEvent.LimitTime.bLimitTime && m_cpEventSubControl.m_ScriptEvent.LimitTime.fLimitTimeCount <= 0f)
        {
            result = m_cpEventSubControl.EventList_Check(-1, -1, m_cpEventSubControl.m_localData.m_lCharaDirection, 0);
        }
        return result;
    }

    private void LoadData(int chapter)
    {
        SCRIPT_EVENT scriptEvent = m_cpEventSubControl.m_ScriptEvent;
        m_cpEventDataControl.LoadData(chapter);
        m_cpEventDataControl.LoadDataMapAttribute(chapter, m_cpEventSubControl.m_localData.m_lMapNumber);
        if (!m_bLoadFlag)
        {
            if (!m_cpEventSubControl.m_NextMapData.bOldFlag)
            {
                if (m_cpEventControl != null)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        m_cpEventSubControl.m_localData.m_lEventFlag[i] = m_cpEventControl.ExportEventFlag(i);
                    }
                }
                m_cpOldEventControl = m_cpEventControl;
                m_cpEventSubControl.SetOldEventControl(m_cpOldEventControl);
            }
            else
            {
                for (int j = 0; j < 8; j++)
                {
                    m_cpEventSubControl.m_localData.m_lEventFlag[j] = m_cpOldEventControl.ExportEventFlag(j);
                }
            }
        }
        else
        {
            m_bEventMapChange = false;
        }
        m_cpEventControl = new CpEventControl();
        m_cpEventControl.Initialization(m_cpEventSubControl);
        m_cpEventSubControl.SetEventControl(m_cpEventControl);
        m_cpEventSubControl.SetMapPaintControl(m_CpMapPaintControl);
        m_cpEventSubControl.SetCharacterPaintControl(m_CpCharacterPaintControl);
        m_cpEventSubControl.SetCpEventPicturePaintControl(m_CpEventPicturePaintControl);
        m_cpEventSubControl.SetCpEffectControl(m_CpEffectControl);
        m_cpEventSubControl.SetCpMenuPaintControl(m_CpMenuPaintControl);
        m_cpEventSubControl.SetCpBustUpPaintControl(m_CpBustUpPaintControl);
        m_cpEventSubControl.SetCpFacePaintControl(m_CpFacePaintControl);
        m_cpEventSubControl.SetCpItemPaintControl(m_CpItemPaintControl);
        m_cpEventSubControl.SetTalkWindowControl(m_TalkWindowControl);
        m_cpEventSubControl.SetVoiceList(m_cpEventDataControl.voicelist_txt);
        m_cpEventSubControl.SetCHAPTER_OPEN_LIST(GlobalVariable.m_CpDataControl.m_ChapterOpenList);
        m_cpEventSubControl.SetSUBTITLE_LIST(GlobalVariable.m_CpDataControl.m_SubtitleList);
        m_cpEventSubControl.SetCpDeadDataList(m_cpEventDataControl.cpDeadDataList);
        m_cpEventSubControl.SetCpPlayerSettingList(m_cpEventDataControl.cpPlayerSettingList);
        m_cpEventSubControl.SetItemgraphiclist(m_cpEventDataControl.itemgraphiclist_txt);
        if (GlobalVariable.m_SystemBackupData.config_TimePaint == 0)
        {
            m_CpMenuPaintControl.SetActivePlayTime(flag: true);
        }
        else
        {
            m_CpMenuPaintControl.SetActivePlayTime(flag: false);
        }
        MyDebugLog.Log_ColorType("m_cpEventSubControl.m_localData.m_lMapNumber = " + m_cpEventSubControl.m_localData.m_lMapNumber, 6);
        MyDebugLog.Log_ColorType("cpMapDefaultData[" + m_cpEventSubControl.m_localData.m_lMapNumber + "].Name = " + m_cpEventDataControl.m_CpMapDefaultDataList.cpMapDefaultData[m_cpEventSubControl.m_localData.m_lMapNumber].Name, 6);
        scriptEvent.InitEffect.lDark = m_cpEventDataControl.m_CpMapDefaultDataList.cpMapDefaultData[m_cpEventSubControl.m_localData.m_lMapNumber].DarkEffect;
        scriptEvent.InitEffect.lFog = m_cpEventDataControl.m_CpMapDefaultDataList.cpMapDefaultData[m_cpEventSubControl.m_localData.m_lMapNumber].FogEffect;
        scriptEvent.InitEffect.lLight = m_cpEventDataControl.m_CpMapDefaultDataList.cpMapDefaultData[m_cpEventSubControl.m_localData.m_lMapNumber].Light;
        scriptEvent.InitEffect.lThunder = m_cpEventDataControl.m_CpMapDefaultDataList.cpMapDefaultData[m_cpEventSubControl.m_localData.m_lMapNumber].Thunder;
        scriptEvent.InitEffect.lEnemy = m_cpEventDataControl.m_CpMapDefaultDataList.cpMapDefaultData[m_cpEventSubControl.m_localData.m_lMapNumber].Enemy;
        m_CpEffectControl.SetActive(17, flag: true);
        m_CpEffectControl.SetEffectAlpha_256(17, scriptEvent.InitEffect.lLight);
        m_CpEffectControl.SetActive(9, flag: false);
        m_CpEffectControl.SetActive(10, flag: false);
        m_CpEffectControl.SetActive(11, flag: false);
        m_CpEffectControl.SetActive(12, flag: false);
        m_CpEffectControl.SetActive(13, flag: false);
        switch (scriptEvent.InitEffect.lFog)
        {
            case 1:
                m_CpEffectControl.SetActive(9, flag: true);
                break;
            case 2:
                m_CpEffectControl.SetActive(10, flag: true);
                break;
            case 3:
                m_CpEffectControl.SetActive(11, flag: true);
                break;
            case 4:
                m_CpEffectControl.SetActive(12, flag: true);
                break;
        }
        if (!m_bLoadFlag && !m_cpEventSubControl.m_NextMapData.bOldFlag)
        {
            scriptEvent.lIndexOld = scriptEvent.lIndex;
            scriptEvent.lReturnStokOld = scriptEvent.lReturnStok;
            scriptEvent.lMaxOld = scriptEvent.lMax;
            for (int k = 0; k < scriptEvent.lMaxOld; k++)
            {
                scriptEvent.ListOld[k] = scriptEvent.List[k];
            }
        }
        if (scriptEvent.InitEffect.lDark == 0)
        {
            m_CpEffectControl.SetActive(15, flag: false);
        }
        else
        {
            m_CpEffectControl.SetActive(15, flag: true);
        }
        m_CpEffectControl.SetScale(15, 6f);
        if (m_bLoadFlag && m_cpEventSubControl.m_ScriptEvent.lLightNumber != 0)
        {
            MyDebugLog.Log_ColorType(" m_cpEventSubControl.m_ScriptEvent.lLightNumber = " + m_cpEventSubControl.m_ScriptEvent.lLightNumber, 6);
            m_CpEffectControl.SetActive(15, flag: true);
            float num = (float)m_cpEventSubControl.m_ScriptEvent.lLightNumber * 288f / 64000f;
            if (num < 1f)
            {
                num = 1f;
            }
            m_CpEffectControl.SetScale(15, num);
        }
        float num2 = 72f;
        m_cpEventSubControl.m_MapTranslate.Set(0f, 0f, 0f);
        if (m_cpEventSubControl.m_NextMapData.Flag == 0)
        {
            m_cpEventSubControl.m_MapInitTranslate.x = 0f * num2 - num2 / 2f;
            m_cpEventSubControl.m_MapInitTranslate.y = 0f * num2;
        }
        else
        {
            m_cpEventSubControl.m_NextMapData.Flag = 0;
            m_cpEventSubControl.m_MapInitTranslate.x = (float)(-m_cpEventSubControl.m_NextMapData.X) * num2 - num2 / 2f;
            m_cpEventSubControl.m_MapInitTranslate.y = (float)m_cpEventSubControl.m_NextMapData.Y * num2;
        }
        m_cpEventSubControl.m_MapInitTranslate.z = 0f;
        m_cpEventSubControl.m_MapTranslate = m_cpEventSubControl.m_MapInitTranslate;
        m_cpEventSubControl.m_MapTranslateOneFrame.Set(0f, 0f, 0f);
        m_cpEventControl.C_GsCpEvent(m_cpEventDataControl.entrylistarr, m_cpEventDataControl.entrylistarr.Length, m_cpEventDataControl.entrylistBinarr, m_cpEventDataControl.entrylistBinarr.Length, m_cpEventDataControl.m_CorpsePartyEventMessage, m_cpEventDataControl.entrylistDatarr, m_cpEventDataControl.entrylistDatarr.Length, m_cpEventDataControl.eventlistarr, m_cpEventDataControl.mapAttribute);
        m_cpEventControl.ImportEventFlag(m_cpEventSubControl.m_localData.m_lEventFlag);
        m_cpEventControl.ReSetChapter(chapter, m_cpEventSubControl.m_localData.m_lMapNumber);
        m_CpCharacterPaintControl.ReSet();
        m_CpCharacterPaintControl.Set_Charalist_txt(m_cpEventDataControl.charalist_txt);
        if (!m_bLoadFlag)
        {
            m_cpEventSubControl.m_ScriptEvent.Effect.Brightness.NowLevel = 0f;
            m_cpEventSubControl.m_ScriptEvent.Effect.Brightness.NextLevel = 0f;
        }
        else if (m_cpEventSubControl.m_ScriptEvent.Effect.Brightness.NowLevel != 0f)
        {
            m_CpEffectControl.SetActive(17, flag: true);
            m_CpEffectControl.SetEffectAlpha_256(17, m_cpEventSubControl.m_ScriptEvent.Effect.Brightness.NowLevel);
        }
        bool flag = false;
        int l;
        for (l = 0; m_cpEventControl.EventList_Get(ref scriptEvent.List[l]) > 0; l++)
        {
        }
        scriptEvent.List[0].SizeX = 0;
        scriptEvent.List[0].SizeY = 0;
        scriptEvent.lMax = l;
        for (l = 0; l < scriptEvent.lMax; l++)
        {
            scriptEvent.StockList[l].Map = scriptEvent.List[l].Map;
            scriptEvent.StockList[l].Event = scriptEvent.List[l].Event;
            scriptEvent.StockList[l].PosX = scriptEvent.List[l].PosX;
            scriptEvent.StockList[l].PosY = scriptEvent.List[l].PosY;
            scriptEvent.StockList[l].SizeX = scriptEvent.List[l].SizeX;
            scriptEvent.StockList[l].SizeY = scriptEvent.List[l].SizeY;
            scriptEvent.StockList[l].Page = scriptEvent.List[l].Page;
            scriptEvent.StockList[l].FlagUse = scriptEvent.List[l].FlagUse;
            scriptEvent.StockList[l].FlagNum = scriptEvent.List[l].FlagNum;
            scriptEvent.StockList[l].FlagSW = scriptEvent.List[l].FlagSW;
            scriptEvent.StockList[l].ExecSW = scriptEvent.List[l].ExecSW;
            scriptEvent.StockList[l].CharaNo = scriptEvent.List[l].CharaNo;
            scriptEvent.StockList[l].CharaDIR = scriptEvent.List[l].CharaDIR;
            scriptEvent.StockList[l].AnimNo = scriptEvent.List[l].AnimNo;
            scriptEvent.StockList[l].Move = scriptEvent.List[l].Move;
            scriptEvent.StockList[l].HitChck = scriptEvent.List[l].HitChck;
            scriptEvent.StockList[l].CharaST = scriptEvent.List[l].CharaST;
            scriptEvent.StockList[l].AnimSpd = scriptEvent.List[l].AnimSpd;
            scriptEvent.StockList[l].PAD1 = scriptEvent.List[l].PAD1;
            scriptEvent.StockList[l].Sort = scriptEvent.List[l].Sort;
            scriptEvent.StockList[l].Effect = scriptEvent.List[l].Effect;
            scriptEvent.StockList[l].SE_request = scriptEvent.List[l].SE_request;
            scriptEvent.StockList[l].PAD3 = scriptEvent.List[l].PAD3;
            scriptEvent.StockList[l].PAD4 = scriptEvent.List[l].PAD4;
        }
        if (m_bLoadFlag)
        {
            CHAPTER_DATA cHAPTER_DATA = GlobalVariable.m_GameBackupData.cHAPTER_DATA;
            scriptEvent.lReturnStok = cHAPTER_DATA.lReturnStok;
            for (l = 0; l < 160; l++)
            {
                scriptEvent.List[l].PosX = (ushort)cHAPTER_DATA.EventData[l].PosX;
                scriptEvent.List[l].PosY = (ushort)cHAPTER_DATA.EventData[l].PosY;
                scriptEvent.List[l].CharaNo = (ushort)cHAPTER_DATA.EventData[l].CharaNo;
                scriptEvent.List[l].CharaDIR = (ushort)cHAPTER_DATA.EventData[l].CharaDIR;
                scriptEvent.List[l].AnimNo = (ushort)cHAPTER_DATA.EventData[l].AnimNo;
                scriptEvent.List[l].HitChck = (ushort)cHAPTER_DATA.EventData[l].HitChck;
                scriptEvent.List[l].CharaST = (ushort)cHAPTER_DATA.EventData[l].CharaST;
                scriptEvent.List[l].AnimSpd = (ushort)cHAPTER_DATA.EventData[l].AnimSpd;
            }
            scriptEvent.List[0].CharaNo = (ushort)m_cpEventSubControl.m_localData.m_lCharacterNumber;
            scriptEvent.Effect.Brightness.NowLevel = (int)cHAPTER_DATA.cBrightness;
            scriptEvent.Effect.Brightness.NextLevel = (int)cHAPTER_DATA.cBrightness;
            m_CpEffectControl.SetActive(17, flag: true);
            m_CpEffectControl.SetEffectAlpha_256(17, scriptEvent.Effect.Brightness.NowLevel);
            if (cHAPTER_DATA.cRain != 0)
            {
                m_CpEffectControl.SetActive(13, flag: true);
            }
            if (cHAPTER_DATA.cFog[0] != 0)
            {
                m_CpEffectControl.SetActive(9, flag: true);
            }
            if (cHAPTER_DATA.cFog[1] != 0)
            {
                m_CpEffectControl.SetActive(10, flag: true);
            }
            if (cHAPTER_DATA.cFog[2] != 0)
            {
                m_CpEffectControl.SetActive(11, flag: true);
            }
            if (cHAPTER_DATA.cFog[3] != 0)
            {
                m_CpEffectControl.SetActive(12, flag: true);
            }
        }
        m_cpEventSubControl.AliveCount = 0;
        MyDebugLog.Log_ColorType("lMax = " + scriptEvent.lMax, 6);
        for (l = 0; l < scriptEvent.lMax; l++)
        {
            m_cpEventSubControl.Alive[l] = m_cpEventControl.EventList_EnableCheck(scriptEvent.List, scriptEvent.List[l], scriptEvent.lMax);
            if (l == 0)
            {
                m_CpCharacterPaintControl.LoadCharacterSprite(m_cpEventSubControl.m_localData.m_lCharacterNumber);
            }
            else
            {
                m_CpCharacterPaintControl.LoadCharacterSprite(scriptEvent.List[l].CharaNo);
            }
            if (m_cpEventSubControl.Alive[l] != 0)
            {
                m_cpEventSubControl.AliveCount++;
            }
            scriptEvent.Data[l].Alive = m_cpEventSubControl.Alive[l];
            scriptEvent.Data[l].NowPos.x = (float)(int)scriptEvent.List[l].PosX * num2 + num2 / 2f;
            scriptEvent.Data[l].NowPos.y = (float)(-scriptEvent.List[l].PosY) * num2;
            scriptEvent.Data[l].NowPos.z = 0f;
            scriptEvent.Data[l].NowPos += m_cpEventSubControl.m_MapInitTranslate;
            m_CpCharacterPaintControl.SetPosition(l, scriptEvent.Data[l].NowPos);
            if (l == 0 && chapter == CpGameData.Chapter100 && !m_bLoadFlag && !m_cpEventSubControl.m_NextMapData.bOldFlag)
            {
                scriptEvent.List[l].AnimNo = 1;
                scriptEvent.List[l].AnimSpd = 23;
            }
            if (scriptEvent.List[l].CharaDIR >= 0 && 4 > scriptEvent.List[l].CharaDIR)
            {
                m_CpCharacterPaintControl.SetSpriteNumber(l, scriptEvent.List[l].CharaDIR);
            }
            if (scriptEvent.List[l].AnimNo == 0)
            {
                if (scriptEvent.List[l].CharaST < 0 || scriptEvent.List[l].CharaST > 2)
                {
                    scriptEvent.List[l].CharaST = 0;
                }
                if (scriptEvent.List[l].CharaDIR >= 0 && 4 > scriptEvent.List[l].CharaDIR)
                {
                    m_CpCharacterPaintControl.SetSpriteNumber(l, scriptEvent.List[l].CharaDIR, scriptEvent.List[l].CharaST);
                }
            }
            if (scriptEvent.List[l].CharaNo != 0 && scriptEvent.Data[l].Alive != 0)
            {
                m_CpCharacterPaintControl.SetActive(l, _flag: true, 0);
                flag = true;
            }
            else
            {
                m_CpCharacterPaintControl.SetActive(l, _flag: false, 0);
                flag = false;
            }
            scriptEvent.Data[l].EffectNumber = 0;
            m_CpCharacterPaintControl.LoadEffectSprite(0, l);
            m_CpCharacterPaintControl.SetEffectActive(l, _flag: false);
            if (scriptEvent.List[l].Effect != 0)
            {
                scriptEvent.Data[l].EffectNumber = (sbyte)scriptEvent.List[l].Effect;
                scriptEvent.Data[l].EffectAnimeNumber = 0;
                scriptEvent.Data[l].EffectAnimeCount = 0;
                m_CpCharacterPaintControl.SetEffectActive(l, flag);
                m_CpCharacterPaintControl.SetEffectPosition(l, scriptEvent.Data[l].NowPos);
                int animeNo = scriptEvent.Data[l].EffectNumber - 1 + scriptEvent.Data[l].EffectAnimeNumber;
                m_CpCharacterPaintControl.SetEffectSpriteNumberDirect(l, animeNo);
                m_CpCharacterPaintControl.SetEffectScale(l, scriptEvent.Data[l].EffectAnimeNumber);
            }
            scriptEvent.Data[l].cWalkFlag = 0;
            scriptEvent.Data[l].cWalkWait = 0;
            scriptEvent.Data[l].cWalkEndFlag = 0;
        }
        if ((m_bLoadFlag || scriptEvent.PtMenber.bMapChangeFlag) && scriptEvent.PtMenber.Alive != 0)
        {
            if (!m_bLoadFlag)
            {
                scriptEvent.PtMenber.PosX = 0f;
                scriptEvent.PtMenber.PosY = 0f;
                scriptEvent.PtMenber.TranslateOneFrameX[0] = 0f;
                scriptEvent.PtMenber.TranslateOneFrameX[1] = 0f;
                scriptEvent.PtMenber.TranslateOneFrameY[0] = 0f;
                scriptEvent.PtMenber.TranslateOneFrameY[1] = 0f;
                scriptEvent.PtMenber.StokAnimeNumber[0] = 0;
                scriptEvent.PtMenber.StokAnimeNumber[1] = 0;
            }
            Vector3 zero = Vector3.zero;
            zero.x = scriptEvent.PtMenber.PosX + m_cpEventSubControl.m_OffsetTrans.x;
            zero.y = scriptEvent.PtMenber.PosY + m_cpEventSubControl.m_OffsetTrans.y;
            zero.z = m_cpEventSubControl.m_OffsetTrans.z;
            m_CpCharacterPaintControl.LoadPTMenberSprite(scriptEvent.PtMenber.NameIndex);
            m_CpCharacterPaintControl.SetSpriteNumberOffsetPTMenber(scriptEvent.List[0].CharaDIR * 4);
            m_CpCharacterPaintControl.SetPositionPTMenber(zero);
        }
        if (m_cpEventSubControl.m_ZappingData.bUsed)
        {
            m_CpCharacterPaintControl.LoadZappingSprite(m_cpEventSubControl.m_ZappingData.Character_Index);
            if (m_cpEventSubControl.m_ZappingData.Map_Number == m_cpEventSubControl.m_localData.m_lMapNumber)
            {
                Vector3 zero2 = Vector3.zero;
                m_cpEventSubControl.m_ZappingData.Pos.x = (float)(-m_cpEventSubControl.m_ZappingData.X) * num2 - num2 / 2f - m_cpEventSubControl.m_MapTranslate.x;
                m_cpEventSubControl.m_ZappingData.Pos.y = (float)m_cpEventSubControl.m_ZappingData.Y * num2 - m_cpEventSubControl.m_MapTranslate.y;
                m_cpEventSubControl.m_ZappingData.Pos.x = 0f - m_cpEventSubControl.m_ZappingData.Pos.x;
                m_cpEventSubControl.m_ZappingData.Pos.y = 0f - m_cpEventSubControl.m_ZappingData.Pos.y;
                zero2.x = m_cpEventSubControl.m_ZappingData.Pos.x;
                zero2.y = m_cpEventSubControl.m_ZappingData.Pos.y;
                zero2.z = m_cpEventSubControl.m_OffsetTrans.z;
                m_CpCharacterPaintControl.SetPositionZapping(zero2);
                m_CpCharacterPaintControl.SetSpriteNumberDirectZapping(7);
            }
            else
            {
                m_CpCharacterPaintControl.m_Zapping.m_ObjMain.SetActive(value: false);
            }
        }
        if (m_bLoadFlag)
        {
            if (scriptEvent.Chaser.Alive != 0 && scriptEvent.Chaser.nWarpCount == 0)
            {
                Vector3 zero3 = Vector3.zero;
                zero3.x = scriptEvent.Chaser.PosX + m_cpEventSubControl.m_OffsetTrans.x;
                zero3.y = scriptEvent.Chaser.PosY + m_cpEventSubControl.m_OffsetTrans.y;
                zero3.z = m_cpEventSubControl.m_OffsetTrans.z;
                m_CpCharacterPaintControl.LoadChaserSprite(scriptEvent.Chaser.NameIndex);
                m_CpCharacterPaintControl.SetPositionChaser(zero3);
            }
        }
        else if (scriptEvent.Chaser.Alive != 0)
        {
            scriptEvent.Chaser.PosX = 0f;
            scriptEvent.Chaser.PosY = 0f;
            scriptEvent.Chaser.nWarpCount = 180;
            scriptEvent.Chaser.cWalkWait = 0;
            scriptEvent.Chaser.cWalkFlag = 0;
        }
        int num3 = 0;
        scriptEvent.Data[num3].NowPos = Vector3.zero;
        m_CpCharacterPaintControl.SetPosition(num3, scriptEvent.Data[num3].NowPos);
        if (scriptEvent.bPlayCharacterPaintFlag)
        {
            m_CpCharacterPaintControl.SetActive(0, _flag: true, 0);
        }
        else
        {
            m_CpCharacterPaintControl.SetActive(0, _flag: false, 0);
        }
        switch (m_cpEventSubControl.m_NextMapData.dir)
        {
            case 1:
                scriptEvent.List[0].CharaDIR = 0;
                m_cpEventSubControl.m_localData.m_lCharaDirection = 1;
                m_CpCharacterPaintControl.SetSpriteNumber(0, scriptEvent.List[0].CharaDIR, scriptEvent.List[0].CharaST);
                break;
            case 2:
                scriptEvent.List[0].CharaDIR = 2;
                m_cpEventSubControl.m_localData.m_lCharaDirection = 2;
                m_CpCharacterPaintControl.SetSpriteNumber(0, scriptEvent.List[0].CharaDIR, scriptEvent.List[0].CharaST);
                break;
            case 3:
                scriptEvent.List[0].CharaDIR = 1;
                m_cpEventSubControl.m_localData.m_lCharaDirection = 3;
                m_CpCharacterPaintControl.SetSpriteNumber(0, scriptEvent.List[0].CharaDIR, scriptEvent.List[0].CharaST);
                break;
            case 4:
                scriptEvent.List[0].CharaDIR = 3;
                m_cpEventSubControl.m_localData.m_lCharaDirection = 4;
                m_CpCharacterPaintControl.SetSpriteNumber(0, scriptEvent.List[0].CharaDIR, scriptEvent.List[0].CharaST);
                break;
        }
        if (scriptEvent.PtMenber.Alive != 0)
        {
            m_CpCharacterPaintControl.SetSpriteNumberOffsetPTMenber(scriptEvent.List[0].CharaDIR * 4);
        }
        if (m_cpEventSubControl.m_BmpWindow.Used)
        {
            m_cpEventSubControl.m_BmpWindow.Used = false;
            m_cpEventSubControl.m_BmpWindow.FadeInCount = 0;
            m_cpEventSubControl.m_BmpWindow.FadeInMax = 0;
            m_cpEventSubControl.m_BmpWindow.Count = 0;
            m_cpEventSubControl.m_BmpWindow.FadeOutCount = 0;
            m_cpEventSubControl.m_BmpWindow.FadeOutMax = 0;
            m_cpEventSubControl.m_BmpWindow.Index = 0;
            m_CpMenuPaintControl.SetActiveFloorName(flag: false);
        }
        m_CpCharacterPaintControl.PaneSortPri();
    }

    private void ExChangeBackupdata()
    {
        GlobalVariable.m_GameBackupData = new GameBackupData();
        CHAPTER_DATA cHAPTER_DATA = GlobalVariable.m_GameBackupData.cHAPTER_DATA;
        float num = 72f;
        GlobalVariable.m_GameBackupData.Language = GlobalVariable.Language;
        GlobalVariable.m_GameBackupData.sysPAD2 = message_skip_flag;
        cHAPTER_DATA.nBgmNumber = 0;
        if (GlobalVariable.m_SoundPlayerCTRL.IsPlayBGM())
        {
            cHAPTER_DATA.nBgmNumber = GlobalVariable.m_SoundPlayerCTRL.GetPlayBGMNumber();
        }
        cHAPTER_DATA.nSeNumber = 0;
        if (GlobalVariable.m_SoundPlayerCTRL.Environmental_SoundNo != 0)
        {
            cHAPTER_DATA.nSeNumber = GlobalVariable.m_SoundPlayerCTRL.Environmental_SoundNo;
        }
        cHAPTER_DATA.nPointBgmNumber = m_cpEventSubControl.m_PointBgm.lBgmNumber;
        cHAPTER_DATA.PointBgm.cFlag = m_cpEventSubControl.m_PointBgm.cFlag;
        cHAPTER_DATA.PointBgm.cX = m_cpEventSubControl.m_PointBgm.cX;
        cHAPTER_DATA.PointBgm.cY = m_cpEventSubControl.m_PointBgm.cY;
        cHAPTER_DATA.lElapsedTime = (int)m_cpEventSubControl.m_localData.fElapsedTime;
        cHAPTER_DATA.lCapNumber = m_Chapter;
        cHAPTER_DATA.lCapNumber = m_Chapter;
        cHAPTER_DATA.cSavePointIndex = (byte)m_cpEventSubControl.m_localData.m_lSaveSelectIndex;
        cHAPTER_DATA.lMapNumber = m_cpEventSubControl.m_localData.m_lMapNumber;
        for (int i = 0; i < 8; i++)
        {
            cHAPTER_DATA.lEventFlag[i] = m_cpEventControl.ExportEventFlag(i);
        }
        if (m_cpEventSubControl.m_ScriptEvent.bPlayCharacterPaintFlag)
        {
            cHAPTER_DATA.cPlayerPaintFlag = 1;
        }
        else
        {
            cHAPTER_DATA.cPlayerPaintFlag = 0;
        }
        cHAPTER_DATA.lCharacterNumber = m_cpEventSubControl.m_localData.m_lCharacterNumber;
        cHAPTER_DATA.lCharaDirection = m_cpEventSubControl.m_localData.m_lCharaDirection;
        cHAPTER_DATA.LightNumber = m_cpEventSubControl.m_ScriptEvent.lLightNumber;
        cHAPTER_DATA.lLimitTimeCount = (int)m_cpEventSubControl.m_ScriptEvent.LimitTime.fLimitTimeCount;
        if (cHAPTER_DATA.lLimitTimeCount == 0 && m_cpEventSubControl.m_ScriptEvent.LimitTime.fLimitTimeCount > 0f)
        {
            cHAPTER_DATA.lLimitTimeCount = 1;
        }
        cHAPTER_DATA.cPos[0] = (byte)((0f - m_cpEventSubControl.m_MapTranslate.x - num / 2f) / num);
        cHAPTER_DATA.cPos[1] = (byte)(m_cpEventSubControl.m_MapTranslate.y / num);
        cHAPTER_DATA.lReturnStok = m_cpEventSubControl.m_ScriptEvent.lReturnStok;
        switch (m_cpEventSubControl.m_localData.m_lCharaDirection)
        {
            case 1:
                cHAPTER_DATA.EventData[0].CharaDIR = 0;
                break;
            case 2:
                cHAPTER_DATA.EventData[0].CharaDIR = 2;
                break;
            case 3:
                cHAPTER_DATA.EventData[0].CharaDIR = 1;
                break;
            case 4:
                cHAPTER_DATA.EventData[0].CharaDIR = 3;
                break;
        }
        for (int j = 0; j < 127; j++)
        {
            cHAPTER_DATA.MessageBackLog.lList[j] = m_cpEventSubControl.messege_log.lList[j];
        }
        cHAPTER_DATA.MessageBackLog.lIndex = m_cpEventSubControl.messege_log.lIndex;
        for (int k = 0; k < 160; k++)
        {
            cHAPTER_DATA.EventData[k].PosX = m_cpEventSubControl.m_ScriptEvent.List[k].PosX;
            cHAPTER_DATA.EventData[k].PosY = m_cpEventSubControl.m_ScriptEvent.List[k].PosY;
            cHAPTER_DATA.EventData[k].CharaNo = m_cpEventSubControl.m_ScriptEvent.List[k].CharaNo;
            cHAPTER_DATA.EventData[k].CharaDIR = m_cpEventSubControl.m_ScriptEvent.List[k].CharaDIR;
            cHAPTER_DATA.EventData[k].AnimNo = m_cpEventSubControl.m_ScriptEvent.List[k].AnimNo;
            cHAPTER_DATA.EventData[k].HitChck = m_cpEventSubControl.m_ScriptEvent.List[k].HitChck;
            cHAPTER_DATA.EventData[k].CharaST = m_cpEventSubControl.m_ScriptEvent.List[k].CharaST;
            cHAPTER_DATA.EventData[k].AnimSpd = m_cpEventSubControl.m_ScriptEvent.List[k].AnimSpd;
        }
        cHAPTER_DATA.cPtIndex = (byte)m_cpEventSubControl.m_Party_Set.lType;
        cHAPTER_DATA.Patery[0] = m_cpEventSubControl.m_Party_Set.lIndex[0];
        cHAPTER_DATA.Patery[1] = m_cpEventSubControl.m_Party_Set.lIndex[1];
        cHAPTER_DATA.Patery[2] = m_cpEventSubControl.m_Party_Set.lIndex[2];
        for (int l = 0; l < 4; l++)
        {
            cHAPTER_DATA.cHp[l] = m_cpEventSubControl.m_Party_Set.cHp[l];
        }
        for (int m = 0; m < 64; m++)
        {
            cHAPTER_DATA.lItemFlag[m] = (byte)m_cpEventSubControl.m_ScriptEvent.Item[m].lFlagNumber;
        }
        cHAPTER_DATA.PtMenber.Alive = m_cpEventSubControl.m_ScriptEvent.PtMenber.Alive;
        cHAPTER_DATA.PtMenber.NameIndex = m_cpEventSubControl.m_ScriptEvent.PtMenber.NameIndex;
        cHAPTER_DATA.PtMenber.PosX = m_cpEventSubControl.m_ScriptEvent.PtMenber.PosX;
        cHAPTER_DATA.PtMenber.PosY = m_cpEventSubControl.m_ScriptEvent.PtMenber.PosY;
        cHAPTER_DATA.PtMenber.Step.AnimeFrameCount = m_cpEventSubControl.m_ScriptEvent.PtMenber.Step.AnimeFrameCount;
        cHAPTER_DATA.PtMenber.Step.AnimeFrameMax = m_cpEventSubControl.m_ScriptEvent.PtMenber.Step.AnimeFrameMax;
        cHAPTER_DATA.PtMenber.Step.AnimeNumber = m_cpEventSubControl.m_ScriptEvent.PtMenber.Step.AnimeNumber;
        cHAPTER_DATA.PtMenber.Step.AnimeCount = m_cpEventSubControl.m_ScriptEvent.PtMenber.Step.AnimeCount;
        cHAPTER_DATA.PtMenber.Step.FootType = m_cpEventSubControl.m_ScriptEvent.PtMenber.Step.FootType;
        cHAPTER_DATA.PtMenber.Step.IsDirection = m_cpEventSubControl.m_ScriptEvent.PtMenber.Step.IsDirection;
        cHAPTER_DATA.PtMenber.TranslateOneFrameX[0] = m_cpEventSubControl.m_ScriptEvent.PtMenber.TranslateOneFrameX[0];
        cHAPTER_DATA.PtMenber.TranslateOneFrameX[1] = m_cpEventSubControl.m_ScriptEvent.PtMenber.TranslateOneFrameX[1];
        cHAPTER_DATA.PtMenber.TranslateOneFrameY[0] = m_cpEventSubControl.m_ScriptEvent.PtMenber.TranslateOneFrameY[1];
        cHAPTER_DATA.PtMenber.TranslateOneFrameY[1] = m_cpEventSubControl.m_ScriptEvent.PtMenber.TranslateOneFrameY[1];
        cHAPTER_DATA.PtMenber.StokAnimeNumber[0] = m_cpEventSubControl.m_ScriptEvent.PtMenber.StokAnimeNumber[0];
        cHAPTER_DATA.PtMenber.StokAnimeNumber[1] = m_cpEventSubControl.m_ScriptEvent.PtMenber.StokAnimeNumber[1];
        cHAPTER_DATA.PtMenber.bMapChangeFlag = m_cpEventSubControl.m_ScriptEvent.PtMenber.bMapChangeFlag;
        cHAPTER_DATA.ChaserData.Alive = m_cpEventSubControl.m_ScriptEvent.Chaser.Alive;
        cHAPTER_DATA.ChaserData.NameIndex = m_cpEventSubControl.m_ScriptEvent.Chaser.NameIndex;
        cHAPTER_DATA.ChaserData.PosX = m_cpEventSubControl.m_ScriptEvent.Chaser.PosX;
        cHAPTER_DATA.ChaserData.PosY = m_cpEventSubControl.m_ScriptEvent.Chaser.PosY;
        cHAPTER_DATA.ChaserData.Step.AnimeFrameCount = m_cpEventSubControl.m_ScriptEvent.Chaser.Step.AnimeFrameCount;
        cHAPTER_DATA.ChaserData.Step.AnimeFrameMax = m_cpEventSubControl.m_ScriptEvent.Chaser.Step.AnimeFrameMax;
        cHAPTER_DATA.ChaserData.Step.AnimeNumber = m_cpEventSubControl.m_ScriptEvent.Chaser.Step.AnimeNumber;
        cHAPTER_DATA.ChaserData.Step.AnimeCount = m_cpEventSubControl.m_ScriptEvent.Chaser.Step.AnimeCount;
        cHAPTER_DATA.ChaserData.Step.FootType = m_cpEventSubControl.m_ScriptEvent.Chaser.Step.FootType;
        cHAPTER_DATA.ChaserData.Step.IsDirection = m_cpEventSubControl.m_ScriptEvent.Chaser.Step.IsDirection;
        cHAPTER_DATA.ChaserData.nWarpCount = m_cpEventSubControl.m_ScriptEvent.Chaser.nWarpCount;
        cHAPTER_DATA.ChaserData.cWalkFlag = m_cpEventSubControl.m_ScriptEvent.Chaser.cWalkFlag;
        cHAPTER_DATA.ChaserData.cWalkWait = m_cpEventSubControl.m_ScriptEvent.Chaser.cWalkWait;
        cHAPTER_DATA.ChaserData.TranslateOneFrameX = m_cpEventSubControl.m_ScriptEvent.Chaser.TranslateOneFrameX;
        cHAPTER_DATA.ChaserData.TranslateOneFrameY = m_cpEventSubControl.m_ScriptEvent.Chaser.TranslateOneFrameY;
        cHAPTER_DATA.Zapping.cMapNumber = (byte)m_cpEventSubControl.m_ZappingData.Map_Number;
        if (!m_cpEventSubControl.m_ZappingData.bUsed)
        {
            cHAPTER_DATA.Zapping.cMapNumber = byte.MaxValue;
        }
        cHAPTER_DATA.Zapping.cX = (byte)m_cpEventSubControl.m_ZappingData.X;
        cHAPTER_DATA.Zapping.cY = (byte)m_cpEventSubControl.m_ZappingData.Y;
        cHAPTER_DATA.Zapping.cDir = (byte)m_cpEventSubControl.m_ZappingData.dir;
        cHAPTER_DATA.Zapping.nCharacterIndex = m_cpEventSubControl.m_ZappingData.Character_Index;
        cHAPTER_DATA.Zapping.nPt_Index[0] = m_cpEventSubControl.m_ZappingData.Pt_Index[0];
        cHAPTER_DATA.Zapping.nPt_Index[1] = m_cpEventSubControl.m_ZappingData.Pt_Index[1];
        cHAPTER_DATA.Zapping.nPt_Index[2] = m_cpEventSubControl.m_ZappingData.Pt_Index[2];
        cHAPTER_DATA.Zapping.cPtNumber = (byte)m_cpEventSubControl.m_ZappingData.PtNumber;
        cHAPTER_DATA.cBrightness = (byte)m_cpEventSubControl.m_ScriptEvent.Effect.Brightness.NowLevel;
        cHAPTER_DATA.cBrightness = (byte)m_cpEventSubControl.m_ScriptEvent.Effect.Brightness.NextLevel;
        if (m_CpEffectControl.GetActive(13))
        {
            cHAPTER_DATA.cRain = 1;
        }
        else
        {
            cHAPTER_DATA.cRain = 0;
        }
        if (m_CpEffectControl.GetActive(9))
        {
            cHAPTER_DATA.cFog[0] = 1;
        }
        else
        {
            cHAPTER_DATA.cFog[0] = 0;
        }
        if (m_CpEffectControl.GetActive(10))
        {
            cHAPTER_DATA.cFog[1] = 1;
        }
        else
        {
            cHAPTER_DATA.cFog[1] = 0;
        }
        if (m_CpEffectControl.GetActive(11))
        {
            cHAPTER_DATA.cFog[2] = 1;
        }
        else
        {
            cHAPTER_DATA.cFog[2] = 0;
        }
        if (m_CpEffectControl.GetActive(12))
        {
            cHAPTER_DATA.cFog[3] = 1;
        }
        else
        {
            cHAPTER_DATA.cFog[3] = 0;
        }
    }

    public int EventObjMoveCtl(int flag)
    {
        SCRIPT_EVENT scriptEvent = m_cpEventSubControl.m_ScriptEvent;
        Vector3 zero = Vector3.zero;
        Vector3 mapTranslate = m_cpEventSubControl.m_MapTranslate;
        int result = 0;
        float num = 72f;
        for (int i = 0; i < scriptEvent.lMax; i++)
        {
            if (scriptEvent.Data[i].Alive == 0)
            {
                continue;
            }
            if (scriptEvent.Data[i].cWalkWait > 0)
            {
                scriptEvent.Data[i].cWalkWait--;
            }
            if (scriptEvent.Data[i].cWalkFlag > 0)
            {
                scriptEvent.Data[i].cWalkFlag--;
                scriptEvent.Data[i].NowPos += scriptEvent.Data[i].MovePower;
                Vector3 pos = new Vector3(scriptEvent.Data[i].NowPos.x + zero.x, scriptEvent.Data[i].NowPos.y + zero.y, scriptEvent.Data[i].NowPos.z + zero.z);
                m_CpCharacterPaintControl.SetPosition(i, pos);
                _ = scriptEvent.Data[i].EffectNumber;
                scriptEvent.List[i].PosX = (ushort)((scriptEvent.Data[i].NowPos.x - mapTranslate.x - num / 2f) / num);
                scriptEvent.List[i].PosY = (ushort)((0f - (scriptEvent.Data[i].NowPos.y - mapTranslate.y)) / num);
                if (scriptEvent.Data[i].cWalkFlag == 0)
                {
                    scriptEvent.Data[i].cWalkEndFlag = 1;
                }
            }
            int num2 = -1;
            if (flag != 0)
            {
                switch (scriptEvent.List[i].Move)
                {
                    case 1:
                        if (scriptEvent.Data[i].cWalkWait == 0)
                        {
                            num2 = m_cpEventSubControl.GetRandomDir(mapTranslate, scriptEvent.Data[i].NowPos);
                            result = 1;
                        }
                        break;
                    case 2:
                        if (scriptEvent.Data[i].cWalkWait == 0)
                        {
                            num2 = m_cpEventSubControl.GetTargetDir(mapTranslate, scriptEvent.Data[i].NowPos);
                            result = 2;
                        }
                        break;
                }
            }
            if (num2 > -1)
            {
                scriptEvent.Data[i].cWalkFlag = 12;
                scriptEvent.Data[i].cWalkWait = 30;
                scriptEvent.Data[i].MovePower.Set(0f, 0f, 0f);
            }
            switch (num2)
            {
                case 1:
                    if (scriptEvent.List[i].CharaDIR != 1)
                    {
                        scriptEvent.Data[i].AnimeCount = 100;
                    }
                    scriptEvent.List[i].CharaDIR = 1;
                    scriptEvent.Data[i].MovePower.x = num / 12f;
                    break;
                case 4:
                    if (scriptEvent.List[i].CharaDIR != 1)
                    {
                        scriptEvent.Data[i].AnimeCount = 100;
                    }
                    scriptEvent.List[i].CharaDIR = 1;
                    scriptEvent.Data[i].MovePower.x = num / 12f;
                    scriptEvent.Data[i].MovePower.y = num / 12f;
                    break;
                case 5:
                    if (scriptEvent.List[i].CharaDIR != 1)
                    {
                        scriptEvent.Data[i].AnimeCount = 100;
                    }
                    scriptEvent.List[i].CharaDIR = 1;
                    scriptEvent.Data[i].MovePower.x = num / 12f;
                    scriptEvent.Data[i].MovePower.y = 0f - num / 12f;
                    break;
                case 3:
                    if (scriptEvent.List[i].CharaDIR != 3)
                    {
                        scriptEvent.Data[i].AnimeCount = 100;
                    }
                    scriptEvent.List[i].CharaDIR = 3;
                    scriptEvent.Data[i].MovePower.x = 0f - num / 12f;
                    break;
                case 6:
                    if (scriptEvent.List[i].CharaDIR != 3)
                    {
                        scriptEvent.Data[i].AnimeCount = 100;
                    }
                    scriptEvent.List[i].CharaDIR = 3;
                    scriptEvent.Data[i].MovePower.x = 0f - num / 12f;
                    scriptEvent.Data[i].MovePower.y = 0f - num / 12f;
                    break;
                case 7:
                    if (scriptEvent.List[i].CharaDIR != 3)
                    {
                        scriptEvent.Data[i].AnimeCount = 100;
                    }
                    scriptEvent.List[i].CharaDIR = 3;
                    scriptEvent.Data[i].MovePower.x = 0f - num / 12f;
                    scriptEvent.Data[i].MovePower.y = num / 12f;
                    break;
                case 2:
                    if (scriptEvent.List[i].CharaDIR != 2)
                    {
                        scriptEvent.Data[i].AnimeCount = 100;
                    }
                    scriptEvent.List[i].CharaDIR = 2;
                    scriptEvent.Data[i].MovePower.y = 0f - num / 12f;
                    break;
                case 0:
                    if (scriptEvent.List[i].CharaDIR != 0)
                    {
                        scriptEvent.Data[i].AnimeCount = 100;
                    }
                    scriptEvent.List[i].CharaDIR = 0;
                    scriptEvent.Data[i].MovePower.y = num / 12f;
                    break;
            }
        }
        return result;
    }

    public void EventObjAnimeCtl()
    {
        int[] array = new int[4] { 1, 0, 1, 2 };
        int[] array2 = new int[4] { 0, 4, 8, 12 };
        int num = 8;
        SCRIPT_EVENT scriptEvent = m_cpEventSubControl.m_ScriptEvent;
        for (int i = 0; i < scriptEvent.lMax; i++)
        {
            if (scriptEvent.Data[i].Alive != 0 && scriptEvent.List[i].AnimNo == 1)
            {
                scriptEvent.Data[i].AnimeCount++;
                num = 8;
                if (scriptEvent.List[i].AnimSpd != 0)
                {
                    num = scriptEvent.List[i].AnimSpd - 1;
                }
                if (num < scriptEvent.Data[i].AnimeCount)
                {
                    scriptEvent.Data[i].AnimeCount = 0;
                    scriptEvent.Data[i].AnimeType++;
                    scriptEvent.Data[i].AnimeType = scriptEvent.Data[i].AnimeType % 4;
                    int animeNo = array2[scriptEvent.List[i].CharaDIR] + array[scriptEvent.Data[i].AnimeType];
                    m_CpCharacterPaintControl.SetSpriteNumberOffset(i, animeNo);
                }
            }
            if (scriptEvent.Data[i].EffectNumber != 0)
            {
                scriptEvent.Data[i].EffectAnimeCount++;
                if (scriptEvent.Data[i].EffectAnimeCount > 8)
                {
                    scriptEvent.Data[i].EffectAnimeCount = 0;
                    scriptEvent.Data[i].EffectAnimeNumber++;
                    scriptEvent.Data[i].EffectAnimeNumber = (sbyte)(scriptEvent.Data[i].EffectAnimeNumber % 2);
                    _ = scriptEvent.Data[i].EffectNumber;
                    _ = scriptEvent.Data[i].EffectAnimeNumber;
                    m_CpCharacterPaintControl.SetEffectScale(i, scriptEvent.Data[i].EffectAnimeNumber);
                }
            }
        }
        EVENT_MOVE_OBJ eventMoveObj = m_cpEventSubControl.EventMoveObj;
        if (!eventMoveObj.Flag || eventMoveObj.AnimeFlag == 0)
        {
            return;
        }
        int index = eventMoveObj.Index;
        if (index >= 0 && (scriptEvent.Data[index].Alive == 0 || scriptEvent.List[index].AnimNo != 1))
        {
            scriptEvent.Data[index].AnimeCount++;
            num = 8;
            if (scriptEvent.List[index].AnimSpd != 0)
            {
                num = scriptEvent.List[index].AnimSpd - 1;
            }
            num = m_cpEventSubControl.SetMoveFream(eventMoveObj.SpeedType);
            if (num < scriptEvent.Data[index].AnimeCount)
            {
                scriptEvent.Data[index].AnimeCount = 0;
                scriptEvent.Data[index].AnimeType++;
                scriptEvent.Data[index].AnimeType = scriptEvent.Data[index].AnimeType % 4;
                int animeNo2 = array2[scriptEvent.List[index].CharaDIR] + array[scriptEvent.Data[index].AnimeType];
                m_CpCharacterPaintControl.SetSpriteNumberOffset(index, animeNo2);
            }
        }
    }

    public void EventInFunction()
    {
        if (m_lEventRunFlag == 0)
        {
            m_lEventRunFlag = 1;
            if (m_lEventRunWait == 0)
            {
                m_lEventRunWait = 4;
            }
        }
    }

    public void EventEndFunction()
    {
        if (m_lEventRunFlag != 0 && m_cpEventSubControl.m_localData.EventRunDepth == 0)
        {
            m_lEventRunFlag = 0;
            m_lEventRunWait = 0;
            m_CpMenuPaintControl.SetActivePlayerStatus(flag: true, m_cpEventSubControl.m_Party_Set);
            m_CpMenuPaintControl.SetActiveItemList(flag: true);
        }
    }

    public void EventFunction()
    {
        if (m_cpEventSubControl.m_localData.EventRunDepth < 10 || m_lEventRunWait <= 0)
        {
            return;
        }
        m_lEventRunWait--;
        if (m_lEventRunWait == 0)
        {
            m_CpMenuPaintControl.SetActivePlayerStatus(flag: false, m_cpEventSubControl.m_Party_Set);
            m_CpMenuPaintControl.SetActiveItemList(flag: false);
            if (GlobalVariable.m_InfomationWindowControl.GetPaintIndex() == 1)
            {
                GlobalVariable.m_InfomationWindowControl.SetInfoNumber(10);
            }
        }
    }

    public bool IsInit()
    {
        if (NowMyPhase != PHASE._INIT)
        {
            return false;
        }
        return true;
    }
}
