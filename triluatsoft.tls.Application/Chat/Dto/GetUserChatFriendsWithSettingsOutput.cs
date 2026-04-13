using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Castle.Components.DictionaryAdapter;
using triluatsoft.tls.Friendships.Dto;

namespace triluatsoft.tls.Chat.Dto
{
    public class GetUserChatFriendsWithSettingsOutput
    {
        public DateTime ServerTime { get; set; }

        public List<FriendDto> Friends { get; set; }

        public GetUserChatFriendsWithSettingsOutput()
        {
            Friends = new EditableList<FriendDto>();
        }
    }
}