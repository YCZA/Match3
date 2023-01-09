using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Wooga.Core.Data;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services;
using Match3.Scripts1.Wooga.Services.KeyValueStore;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using Wooga.Foundation.Json;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200073B RID: 1851
	public class AdminService : AService
	{
		// Token: 0x06002DE7 RID: 11751 RVA: 0x000D52C4 File Offset: 0x000D36C4
		public AdminService()
		{
			WooroutineRunner.StartCoroutine(this.LoadRoutine(), null);
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06002DE8 RID: 11752 RVA: 0x000D52D9 File Offset: 0x000D36D9
		// (set) Token: 0x06002DE9 RID: 11753 RVA: 0x000D52E1 File Offset: 0x000D36E1
		public CommandList commands { get; private set; }

		// Token: 0x06002DEA RID: 11754 RVA: 0x000D52EC File Offset: 0x000D36EC
		private IEnumerator LoadRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			if (this.sbs.IsAuthenticated)
			{
				yield return this.LoadCommands();
			}
			yield break;
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x000D5308 File Offset: 0x000D3708
		public IEnumerator LoadCommands()
		{
			CommandList localCommands = this.LoadCommandListFromDisk();
			CommandList serverCommands = null;
			yield return this.DoReloadData("command", 1, delegate(CommandList commandsFromServer)
			{
				serverCommands = commandsFromServer;
				this.commands = this.MergeCommandBuckets(commandsFromServer, localCommands);
			});
			if (this.commands != null && this.commands.data != null && serverCommands != null && serverCommands.data != null && this.commands.data.Count != serverCommands.data.Count)
			{
				yield return this.SaveCommandList();
			}
			yield return WooroutineRunner.StartCoroutine(this.ExecuteCommands(), null);
			if (serverCommands.data == null)
			{
				serverCommands.data = new List<CommandData>();
				CommandData dummyCommand = new CommandData();
				dummyCommand.name = "InitialData";
				dummyCommand.id = "0";
				dummyCommand.processed = true;
				serverCommands.data.Add(dummyCommand);
				yield return this.WriteJsonToBucket(serverCommands);
			}
			base.OnInitialized.Dispatch();
			yield break;
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x000D5324 File Offset: 0x000D3724
		private IEnumerator WriteJsonToBucket(CommandList serverCommands)
		{
			if (!this.sbs.IsAuthenticated)
			{
				yield break;
			}
			yield return SBS.KeyValueStore.WriteJsonToBucket("command", JSON.Serialize(serverCommands.data, false, 1, ' '), 1, new SbsMergeHandler(this.MergeHandler), null).Catch(delegate(Exception ex)
			{
				WoogaDebug.Log(new object[]
				{
					"failed to write commands"
				});
			});
			yield break;
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x000D5346 File Offset: 0x000D3746
		public override void OnResume()
		{
			WooroutineRunner.StartCoroutine(this.LoadCommands(), null);
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x000D5358 File Offset: 0x000D3758
		private CommandList MergeCommandBuckets(CommandList a, CommandList b)
		{
			if (a == null || a.data == null)
			{
				return b;
			}
			if (b == null || b.data == null)
			{
				return a;
			}
			Dictionary<string, CommandData> dictionary = new Dictionary<string, CommandData>();
			foreach (CommandData commandData in a.data)
			{
				bool flag = false;
				foreach (CommandData commandData2 in b.data)
				{
					if (commandData.id == commandData2.id)
					{
						if (commandData.processed || commandData2.processed)
						{
							commandData.processed = true;
						}
						if (!dictionary.ContainsKey(commandData.id))
						{
							dictionary.Add(commandData.id, commandData);
						}
						flag = true;
					}
				}
				if (!flag && !dictionary.ContainsKey(commandData.id))
				{
					dictionary.Add(commandData.id, commandData);
				}
			}
			foreach (CommandData commandData3 in b.data)
			{
				bool flag2 = false;
				foreach (CommandData commandData4 in a.data)
				{
					if (commandData4.id == commandData3.id)
					{
						if (commandData4.processed || commandData3.processed)
						{
							commandData3.processed = true;
						}
						if (!dictionary.ContainsKey(commandData3.id))
						{
							dictionary.Add(commandData3.id, commandData3);
						}
						flag2 = true;
					}
				}
				if (!flag2 && !dictionary.ContainsKey(commandData3.id))
				{
					dictionary.Add(commandData3.id, commandData3);
				}
			}
			return new CommandList
			{
				data = dictionary.Values.ToList<CommandData>()
			};
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x000D55C8 File Offset: 0x000D39C8
		private CommandList LoadCommandListFromDisk()
		{
			CommandList commandList = new CommandList();
			if (PlayerPrefs.HasKey("localCommandList"))
			{
				string @string = PlayerPrefs.GetString("localCommandList");
				commandList.data = JSON.Deserialize<List<CommandData>>(@string);
			}
			return commandList;
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x000D5604 File Offset: 0x000D3A04
		private IEnumerator SaveCommandList()
		{
			string commandsSerialized = JSON.Serialize(this.commands.data, false, 1, ' ');
			PlayerPrefs.SetString("localCommandList", commandsSerialized);
			yield return SBS.KeyValueStore.WriteJsonToBucket("command", commandsSerialized, 1, new SbsMergeHandler(this.MergeHandler), null).Catch(delegate(Exception ex)
			{
				WoogaDebug.Log(new object[]
				{
					ex,
					"failed to write commands"
				});
			});
			yield break;
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x000D561F File Offset: 0x000D3A1F
		private ISbsData MergeHandler(ISbsData myData, ISbsData serverData)
		{
			return myData;
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x000D5624 File Offset: 0x000D3A24
		public IEnumerator ExecuteCommands()
		{
			bool commandListDirty = false;
			if (this.commands != null && this.commands.data != null)
			{
				foreach (CommandData command in this.commands.data)
				{
					if (!command.processed)
					{
						this.ExecuteCommand(command);
						commandListDirty = true;
						yield return null;
					}
					else if (command.completed_at == 0)
					{
						command.completed_at = command.created_at;
						command.success = true;
						command.result = "success";
						commandListDirty = true;
					}
				}
			}
			if (commandListDirty)
			{
				yield return this.SaveCommandList();
			}
			yield break;
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x000D5640 File Offset: 0x000D3A40
		private void ExecuteCommand(CommandData command)
		{
			string name = command.name;
			if (name != null)
			{
				if (!(name == "AddResource"))
				{
					if (name == "UnlockLevel")
					{
						CommandUnlockLevelArgument commandUnlockLevelArgument = JSON.Deserialize<CommandUnlockLevelArgument>(command.argument);
						if (commandUnlockLevelArgument != null)
						{
							if (commandUnlockLevelArgument.level > this.progressionService.UnlockedLevel)
							{
								this.progressionService.UnlockedLevel = commandUnlockLevelArgument.level;
								command.success = true;
								command.result = this.progressionService.UnlockedLevel.ToString();
							}
						}
						else
						{
							WoogaDebug.LogWarning(new object[]
							{
								"Invalid argument type for Unlock Level."
							});
						}
					}
				}
				else
				{
					CommandResourceArgument commandResourceArgument = JSON.Deserialize<CommandResourceArgument>(command.argument);
					if (commandResourceArgument != null)
					{
						if (commandResourceArgument.type == "lives_unlimited")
						{
							this.livesService.StartUnlimitedLives(commandResourceArgument.amount);
							command.result = commandResourceArgument.amount.ToString();
						}
						else
						{
							this.gameStateService.Resources.AddMaterial(commandResourceArgument.type, commandResourceArgument.amount, true);
							command.result = this.gameStateService.Resources.GetAmount(commandResourceArgument.type).ToString();
						}
						command.success = true;
					}
					else
					{
						WoogaDebug.LogWarning(new object[]
						{
							"Invalid argument type for Add Resource."
						});
					}
				}
			}
			command.completed_at = DateTime.UtcNow.ToUnixTimeStamp();
			command.processed = true;
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x000D57D4 File Offset: 0x000D3BD4
		private IEnumerator DoReloadData(string bucketName, int formatVersion, Action<CommandList> setCommandList)
		{
			ISbsKeyValueStore keyValueStore = SBS.KeyValueStore;
			int kvsRead = this.sbs.SbsConfig.sbs_timeouts.kvsRead;
			Wooroutine<SbsReadResult> readOp = WooroutineRunner.StartWooroutine<SbsReadResult>(keyValueStore.ReadFromBucket(bucketName, null, kvsRead));
			yield return readOp;
			SbsReadResult readResult = new SbsReadResult
			{
				Data = null,
				ETag = string.Empty
			};
			try
			{
				readResult = readOp.ReturnValue;
			}
			catch (Exception ex)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"AdminService",
					ex.Message
				});
			}
			CommandList commandList = new CommandList();
			if (readResult.Data != null && readResult.Data.FormatVersion == formatVersion)
			{
				commandList = JsonUtility.FromJson<CommandList>(readResult.Data.ToString());
			}
			setCommandList(commandList);
			yield break;
		}

		// Token: 0x04005766 RID: 22374
		private const string COMMAND_BUCKET = "command";

		// Token: 0x04005767 RID: 22375
		private const int FORMAT_VERSION = 1;

		// Token: 0x04005768 RID: 22376
		private const string COMMAND_LIST_PREFS_KEY = "localCommandList";

		// Token: 0x04005769 RID: 22377
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x0400576A RID: 22378
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionService;

		// Token: 0x0400576B RID: 22379
		[WaitForService(true, true)]
		private SBSService sbs;

		// Token: 0x0400576C RID: 22380
		[WaitForService(true, true)]
		private LivesService livesService;
	}
}
