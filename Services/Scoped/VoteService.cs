using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhatToEat.Services.Singleton;
using WhatToEat.Types;
using WhatToEat.Types.Enums;
using WhatToEat.Types.TableEntities;

namespace WhatToEat.Services.Scoped
{
  public sealed class VoteService: IDisposable
  {
    public UserVote UserVote { get; set; }

    public UserService UserService { get; set; }

    public StorageService StorageService { get; set; }

    public EventService EventService { get; set; }

    public event Func<VoteChanged, Task> OnBroadcastVoteChanged;

    public VoteService(
      UserService userService,
      StorageService storageService,
      EventService eventService)
    {
      UserService = userService;
      StorageService = storageService;
      EventService = eventService;
      EventService.OnMessage += OnMessage;
    }

    private void OnMessage(BroadcastMessage message)
    {
      if (message.Type == BroadcastEventType.VoteChanged)
      {
        OnBroadcastVoteChanged?.Invoke((VoteChanged)message);
      }
    }

    public async Task SetVoteAsync(List<string> ids)
    {
      if (!ids.Any())
        await StorageService.DeleteVoteAsync(UserService.UserData.GetUserId());
      else
        await StorageService.UpsertVoteAsync(UserService.UserData.GetUserId(), ids);

      EventService.CreateMessage(new VoteChanged { Type = BroadcastEventType.VoteChanged });
    }

    public async Task<List<string>> GetVotesAsync()
    {
      UserVote = await StorageService.GetVoteAsync(UserService.UserData.GetUserId());
      if (UserVote == null)
        return new List<string>();

      var votes = UserVote.GetVotesTyped();
      return (await StorageService.GetRestaurantsAsync())
        .Where(x => votes.Contains(x.RowKey))
        .Select(x => x.Name).ToList();
    }

    public async Task<List<UserVote>> GetVotesAsync(bool todayOnly)
    {
      var votes = await StorageService.GetVotesAsync(todayOnly ? DateTime.UtcNow : null);
      return votes;
    }

    public void Dispose()
    {
      EventService.OnMessage -= OnMessage;
    }
  }
}
