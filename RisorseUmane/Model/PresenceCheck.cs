using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RisorseUmane.Model
{
    public class PresenceCheck
    {
        Presence presence;
        public PresenceCheck() { }
        public PresenceCheck(Presence presence) { 
            this.presence = presence;
            if (presence == null) return;

            Id = presence.Id;
            UserId = presence.UserId;
            Name = presence.User.Name;
            Date = presence.Date?.ToString("dd/MM/yyyy");
            O = presence.O ?? 0;
            S = presence.S ?? 0;
            A = presence.A;
        }

        public int Id
        {
            get; set;
        }
        public int UserId
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public string Date
        {
            get; set;
        }
        public int O
        {
            get; set;
        }
        public int S
        {
            get; set;
        }
        public string A
        {
            get; set;
        }
    }
}