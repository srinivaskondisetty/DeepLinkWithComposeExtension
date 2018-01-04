using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using Microsoft.Bot.Connector.Teams.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepLinkWithComposeExt.Utilities
{
    public static class TemplateUtility
    {
        public static ComposeExtensionAttachment CreateComposeExtensionCardsAttachments(string title, string text, string imageUrl, string state, string deeplinkurl)
        {
            return GetComposeExtensionMainResultAttachment(title, text, imageUrl, deeplinkurl, state).ToComposeExtensionAttachment(GetComposeExtensionPreviewAttachment(title, text, imageUrl, deeplinkurl, state));
        }

        public static Attachment GetComposeExtensionMainResultAttachment(string title, string text, string imageUrl, string deepLinkUrl, string state)
        {
            if (string.Equals(state.ToLower(), "hero"))
            {
                return new HeroCard()
                {
                    Title = title,
                    Text = text,
                    Buttons = new List<CardAction> {
                        new CardAction() {
                             Image = imageUrl,
                             Title = "Go to Config Tab",
                             Type = ActionTypes.OpenUrl,
                             Value = deepLinkUrl
                        }
                    }
                }.ToAttachment();
            }
            else
            {
                return new ThumbnailCard()
                {
                    Title = title,
                    Text = text,
                    Images =
                    {
                        new CardImage(imageUrl)
                    },
                }.ToAttachment();
            }
        }

        public static Attachment GetComposeExtensionPreviewAttachment(string title, string text, string imageUrl, string deepLinkUrl, string state)
        {
            if (string.Equals(state.ToLower(), "hero"))
            {
                return new HeroCard()
                {
                    Title = title,
                    Text = text,
                    Images = {
                        new CardImage(imageUrl)
                    }
                }.ToAttachment();
            }
            else
            {
                return new ThumbnailCard()
                {
                    Title = title,
                    Images =
                    {
                        new CardImage(imageUrl)
                    },
                }.ToAttachment();
            }
        }

        public static string ParseJson(string inputString)
        {
            JObject invokeObjects = JObject.Parse(inputString);
            if (invokeObjects.Count > 0)
            {
                foreach (var item in invokeObjects)
                {
                    return Convert.ToString(item.Value);
                }
            }

            return null;
        }

        public static string GetDeepLink(Activity activity, string type)
        {
            if (type == "channel")
            {
                var teamsChannelData = activity.GetChannelData<TeamsChannelData>();
                var teamId = teamsChannelData.Team == null ? "" : teamsChannelData.Team.Id;
                var channelId = teamsChannelData.Channel == null ? "" : teamsChannelData.Channel.Id;

                // The app ID, stored in the web.config file, should be the appID from your manifest.json file.
                var appId = System.Configuration.ConfigurationManager.AppSettings["MicrosoftAppId"];
                var entity = $"TabLink"; // Match the entity ID we setup when configuring the tab
                var tabContext = new TabContext()
                {
                    ChannelId = channelId,
                    CanvasUrl = "https://teams.microsoft.com"
                };

                string tabName = "tabname";
                var url = $"https://teams.microsoft.com/l/entity/{HttpUtility.UrlEncode(appId)}/{HttpUtility.UrlEncode(entity)}?label={HttpUtility.UrlEncode(tabName)}&context={HttpUtility.UrlEncode(JsonConvert.SerializeObject(tabContext))}";

                return url;
            }
            else {
                // The app ID, stored in the web.config file, should be the appID from your manifest.json file.
                var BotId = System.Configuration.ConfigurationManager.AppSettings["BotId"];
                var TabEntityID = $"staticTab"; // Match the entity ID we setup when configuring the tab
                return "https://teams.microsoft.com/l/entity/28:" + BotId + "/" + TabEntityID + "?conversationType=chat";
            }
        }
    }

    internal class TabContext
    {
        public TabContext()
        {
        }

        public string ChannelId { get; set; }
        public string CanvasUrl { get; set; }
    }
}