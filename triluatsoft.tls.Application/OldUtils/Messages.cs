using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace ClaimWebsite.Code
{
    public class Messages
    {
        private static Dictionary<string, string> messages = new Dictionary<string, string>();

        static Messages()
        {
            try
            {
                lock (messages)
                {
                    XElement root = XElement.Load(HttpContext.Current.Server.MapPath("~/Code/Messages.xml"));
                    var msgs = root.Elements("Message");
                    foreach (var msg in msgs)
                    {
                        string ID = (string)msg.Attribute("ID");
                        string value = (string)msg;
                        messages.Add(ID, value);
                    }
                }
            }
            catch
            {
                //do nothing
            }
        }

        public static string GetMessage(MessageKeys key)
        {
            try
            {
                return messages.ContainsKey(key.ToString()) ? messages[key.ToString()] : key.ToString();
            }
            catch
            {
              return key.ToString();
            }
        }
        public static string GetMessage(string key) {
          try {
            return messages.ContainsKey(key) ? messages[key] : key;
          } catch {
            return key;
          }
        }
    }
}