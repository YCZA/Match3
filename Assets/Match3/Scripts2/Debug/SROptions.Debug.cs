using System;
using System.ComponentModel;
using Match3.Scripts1;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Town;
using UnityEngine;
// using SRDebugger;

namespace Match3.Scripts2.SRDebug
{
    public partial class SROptions
    {
        #region 速度
        // [Category("速度")] [DisplayName("10倍速度")] [SortAttribute(1000)]
        public void Speedx10()
        {
            Time.timeScale = 10;
        }
        // [Category("速度")] [DisplayName("1倍速度")] [SortAttribute(1001)]
        public void Speedx1()
        {
            Time.timeScale = 1;
        }
        // [Category("速度")] [DisplayName("0.1倍速度")] [SortAttribute(1002)]
        public void Speedx_10()
        {
            Time.timeScale = 0.1f;
        }
        #endregion
    
        #region 关卡
        // [Category("关卡")] [DisplayName("一键通关")] [SortAttribute(1000)]
        public void AutoPlay()
        {
            M3_LevelRoot.debug.FinishGame(true);
        }
        // [Category("关卡")] [DisplayName("关卡进度+1")] [SortAttribute(1001)]
        public void GoToNextLevel()
        {
            var go = GameObject.FindObjectOfType<TownCheatsRoot>();
            go.Handle(Cheats.GoToNextLevel);
        }
        // [Category("关卡")] [DisplayName("关卡进度-1")] [SortAttribute(1001)]
        public void GoToPreviousLevel()
        {
            var go = GameObject.FindObjectOfType<TownCheatsRoot>();
            go.Handle(Cheats.GoToPreviousLevel);
        }
    
        // [Category("关卡")] [DisplayName("关卡进度+10")] [SortAttribute(1002)]
        public void GoToNextLevelx10()
        {
            var go = GameObject.FindObjectOfType<TownCheatsRoot>();
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    go.Handle(Cheats.GoToNextLevel);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError(e);
                }
            }
        }
        // [Category("关卡")] [DisplayName("关卡进度-10")] [SortAttribute(1002)]
        // public void GoToPreviousLevelx10()
        // {
        //     var go = GameObject.FindObjectOfType<TownCheatsRoot>();
        //     for (int i = 0; i < 10; i++)
        //     {
        //         try
        //         {
        //             go.Handle(Cheats.GoToPreviousLevel);
        //         }
        //         catch (Exception e)
        //         {
        //             Debug.LogError(e);
        //         }
        //     }
        // }
        //
        // [Category("关卡")] [DisplayName("关卡进度+50")] [SortAttribute(1003)]
        public void GoToNextLevelx50()
        {
            var go = GameObject.FindObjectOfType<TownCheatsRoot>();
            for (int i = 0; i < 50; i++)
            {
                try
                {
                    go.Handle(Cheats.GoToNextLevel);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError(e);
                }
            }
        }
        // [Category("关卡")] [DisplayName("关卡进度-50")] [SortAttribute(1004)]
        // public void GoToPreviousLevelx50()
        // {
        //     var go = GameObject.FindObjectOfType<TownCheatsRoot>();
        //     for (int i = 0; i < 50; i++)
        //     {
        //         try
        //         {
        //             go.Handle(Cheats.GoToPreviousLevel);
        //         }
        //         catch (Exception e)
        //         {
        //             Debug.LogError(e);
        //         }
        //     }
        // }
        [Category("关卡")]
        [DisplayName("关卡进度+500")]
        // [SortAttribute(1005)]
        public void GoToNextLevelx500()
        {
            var go = GameObject.FindObjectOfType<TownCheatsRoot>();
            for (int i = 0; i < 500; i++)
            {
                try
                {
                    go.Handle(Cheats.GoToNextLevel);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError(e);
                }
            }
        }
        // [Category("关卡")]
        // [DisplayName("关卡进度-500")]
        // [SortAttribute(1006)]
        // public void GoToPreviousLevelx500()
        // {
        //     var go = GameObject.FindObjectOfType<TownCheatsRoot>();
        //     for (int i = 0; i < 500; i++)
        //     {
        //         try
        //         {
        //             go.Handle(Cheats.GoToPreviousLevel);
        //         }
        //         catch (Exception e)
        //         {
        //             Debug.LogError(e);
        //         }
        //     }
        // }
        // [Category("关卡")]
        // [DisplayName("重置存档")]
        // [SortAttribute(1005)]
        // public void ResetProgress()
        // {
        //     var go = GameObject.FindObjectOfType<TownCheatsRoot>();
        //     go.Handle(Cheats.ResetProgress);
        // }
        #endregion

        #region 经济
        // [Category("经济")] [DisplayName("金币+1000")] [SortAttribute(2000)]
        public void Add1000Coins()
        {
            var go = GameObject.FindObjectOfType<TownCheatsRoot>();
            go.Handle(Cheats.Add1000Coins);
        }

        [Category("经济")]
        [DisplayName("生命+1")]
        // [SortAttribute(2001)]
        public void Add1Life()
        {
            var go = GameObject.FindObjectOfType<TownCheatsRoot>();
            go.Handle(Cheats.Add1Life);
        }

        [Category("经济")]
        [DisplayName("生命-1")]
        // [SortAttribute(2002)]
        public void UseLife()
        {
            var go = GameObject.FindObjectOfType<TownCheatsRoot>();
            go.Handle(Cheats.UseLife);
        }
    
        [Category("经济")]
        [DisplayName("宝石+100")]
        // [SortAttribute(2003)]
        public void Add100Diamonds()
        {
            var go = GameObject.FindObjectOfType<TownCheatsRoot>();
            go.Handle(Cheats.Add100Diamonds);
        }
    
        #endregion

        #region 挑战
        // [Category("挑战")] [DisplayName("爪子+100")] [SortAttribute(3000)]
        public void Add1000Paws()
        {
            var go = GameObject.FindObjectOfType<TownCheatsRoot>();
            go.Handle(Cheats.ChallengeAdd100Paws);
        }
        #endregion
    
        #region 转盘
        // [Category("转盘")] [DisplayName("重置转盘广告")] [SortAttribute(4000)]
        public void ResetWheelAd()
        {
            var go = GameObject.FindObjectOfType<TownCheatsRoot>();
            go.Handle(Cheats.ResetWheelAds);
        }
        #endregion

        #region 银行
        // [Category("银行")] [DisplayName("重置银行钻石数")] [SortAttribute(5000)]
        public void ResetBank()
        {
            var go = GameObject.FindObjectOfType<TownCheatsRoot>();
            go.Handle(Cheats.BankEventReset);
        }
        // [Category("银行")] [DisplayName("增加100银行钻石")] [SortAttribute(5001)]
        public void Add100BankedDiamonds()
        {
            var go = GameObject.FindObjectOfType<TownCheatsRoot>();
            go.Handle(Cheats.Add100BankedDiamonds);
        }
        #endregion
    
        #region 存档
        private bool delArchiveState = false;
        [Category("服务器存档操作")]
        [DisplayName("清除当前存档, 并退出游戏")]
        // [SortAttribute(1005)]
        public void DelCurArchive()
        {
            // OptionDefinition op1 = null;
            // OptionDefinition op2 = null;
            // op1 = OptionDefinition.FromMethod("确定", () =>
            // {
            //     SRDebug.Instance.RemoveOption(op1);
            //     SRDebug.Instance.RemoveOption(op2);
            //     delArchiveState = false;
            //     WooroutineRunner.StartCoroutine(ToServer.Instance.DelArchive((() =>
            //     {
            //         TownMainRoot.saveGameOnQuit = false;
            //         #if UNITY_EDITOR
            //             EditorApplication.ExitPlaymode();
            //         #else
            //             Application.Quit();
            //         #endif
            //     })));
            // }, "服务器存档操作", 1003);
            // op2 = OptionDefinition.FromMethod("取消", () =>
            // {
            //     SRDebug.Instance.RemoveOption(op1);
            //     SRDebug.Instance.RemoveOption(op2);
            //     delArchiveState = false;
            // }, "服务器存档操作", 1004);
            //
            // if (!delArchiveState)
            // {
            //     SRDebug.Instance.AddOption(op1);
            //     SRDebug.Instance.AddOption(op2);
            //     delArchiveState = true;
            // }
        }

        [Category("服务器存档操作")]
        [DisplayName("获取存档列表")]
        public void GetArchiveList()
        {
        
        }

        [Category("服务器存档操作")]
        [DisplayName("存档名称(字母数字中文)")]
        // [SortAttribute(1006)]
        public string ServerArchiveName
        {
            get;
            set;
        }

        [Category("服务器存档操作")]
        [DisplayName("拉取指定存档, 并退出游戏")]
        // [SortAttribute(1007)]
        public void PullSpecifyArchive()
        {
        
        }
    
        [Category("服务器存档操作")]
        [DisplayName("上传/覆盖指定存档")]
        // [SortAttribute(1008)]
        public void PushSpecifyArchive()
        {
        
        }
    
        [Category("服务器存档操作")]
        [DisplayName("删除指定存档(没实现)")]
        // [SortAttribute(1009)]
        public void DelSpecifyArchive()
        {
        
        }
        #endregion

        #region 其它
        // [Category("跳转关卡")] [DisplayName("关卡id:")] [SortAttribute(1000)]
        public string LevelId { get; set; }
        // [Category("跳转关卡")] [DisplayName("Go")] [SortAttribute(1001)]
        public void StartLevelImmediate()
        {
            var go = GameObject.FindObjectOfType<TownCheatsRoot>();
            go.StartLevelImmediate(LevelId);
        }

        [Category("每日奖励")]
        [DisplayName("显示每日奖励界面")]
        // [SortAttribute(1002)]
        public void ShowDailyGift()
        {
            var go = GameObject.FindObjectOfType<TownCheatsRoot>();
            go.Handle(CheatCategory.UiTests);
        }
        #endregion
    }
}